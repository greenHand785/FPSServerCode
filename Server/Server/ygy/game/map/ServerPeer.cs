using Server.ygy.game.map.util.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Ygy.Game.Map
{
    /// <summary>
    /// 服务器底层，处理客户端的连接，以及收发信息
    /// </summary>
    public class ServerPeer
    {
        //服务器套接字
        private Socket serverSocket;
        //端口号
        private int port;
        //最大的连接数量
        private int maxCount;
        //对象连接池
        private ClientPool clientPool;
        //信号量，用来限制访问数量
        private Semaphore acceptSemaphore;
        //应用层
        private IGameApplication app;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="port"></param>
        /// <param name="maxCount"></param>
        public ServerPeer(int port, int maxCount)
        {
            this.port = port;
            this.maxCount = maxCount;
            acceptSemaphore = new Semaphore(maxCount, maxCount);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientPool = new ClientPool(maxCount);
            ClientPeer initClient = null;
            for (int i = 0; i < maxCount; i++)
            {
                initClient = new ClientPeer(null);
                initClient.receivedCompleted += ProcessReceivedDataComplete;
                initClient.ReceiveDataArgs.Completed += ReceiveDataArgs_Completed;
                initClient.sendDisconnected += Disconnected;
                clientPool.Enqueue(initClient);
            }
        }
        /// <summary>
        /// 设置应用层的接口
        /// </summary>
        /// <param name="app"></param>
        public void SetApplication(IGameApplication app)
        {
            this.app = app;
        }
        //开始启动服务器
        public void Start()
        {
            try
            {
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                serverSocket.Listen(10);
                LogNoteManager.Instance.Log("服务器已启动......");
                //开始接受客户端的连接
                StartAcceptConnect(null);
                Console.ReadKey();
            }
            catch (Exception e)
            {
                LogNoteManager.Instance.Log("74行的错误" + e);
                LogNoteManager.Instance.Log("对象池中的数量" + clientPool.pool.Count);
            }
        }
        /// <summary>
        /// 开始接受客户端连接函数
        /// </summary>
        private void StartAcceptConnect(SocketAsyncEventArgs connectArgs)
        {
            if (connectArgs == null)
            {
                connectArgs = new SocketAsyncEventArgs();
                connectArgs.Completed += ConnectArgs_Completed;
            }
            else
            {
                connectArgs.AcceptSocket = null;
            }
            bool r = serverSocket.AcceptAsync(connectArgs);
            if (!r)
            {
                ProcessAccepted(connectArgs);
            }
        }
        /// <summary>
        /// 当这个异步事件完成时，触发这个委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccepted(e);
        }
        //处理接受到的客户端套接字
        private void ProcessAccepted(SocketAsyncEventArgs e)
        {
            acceptSemaphore.WaitOne();
            ClientPeer client = clientPool.Dequeue();
            client.ClientSocket = e.AcceptSocket;
            LogNoteManager.Instance.Log(DateTime.Now + "：" + client.ClientSocket.RemoteEndPoint + "：连接成功");
            client.IsDisconnected = false;
            //开始一直接收数据
            //开始接收数据
            StartReceiveData(client);
            StartAcceptConnect(e);
        }
        //开始接收数据
        private void StartReceiveData(ClientPeer client)
        {
            if (!client.IsDisconnected)
            {
                bool r = client.ClientSocket.ReceiveAsync(client.ReceiveDataArgs);
                if (!r)
                {
                    ProcessReceivedData(client);
                }
            }
            else
            {
                //断线
                Disconnected(client, "客户端主动下线");
            }
        }
        //接收完成
        private void ReceiveDataArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                ProcessReceivedData(e.UserToken as ClientPeer);
            }
            catch (SocketException e1)
            {
                LogNoteManager.Instance.Log(DateTime.Now + "145行出现的问题" + e1);
                clientPool.Enqueue(e.UserToken as ClientPeer);
                acceptSemaphore.Release();
                LogNoteManager.Instance.Log("对象池中的数量" + clientPool.pool.Count);
            }
            catch (Exception e2)
            {
                LogNoteManager.Instance.Log(DateTime.Now.Date + "152行的错误" + e2);
                LogNoteManager.Instance.Log("对象池中的数量" + clientPool.pool.Count);
            }
        }
        //处理接收到的数据
        private void ProcessReceivedData(ClientPeer client)
        {
            if (client.ReceiveDataArgs.SocketError == SocketError.Success && client.ReceiveDataArgs.BytesTransferred > 0)
            {
                //获得缓冲区的数据
                byte[] packet = new byte[client.ReceiveDataArgs.BytesTransferred];
                Buffer.BlockCopy(client.ReceiveDataArgs.Buffer, 0, packet, 0, packet.Length);
                //对接收到的数据的处理
                client.ProcessReceivedData(packet);
                //伪递归调用，重新开始接收数据
                StartReceiveData(client);
            }
            else
            {
                if (client.ReceiveDataArgs.SocketError == SocketError.Success && client.ReceiveDataArgs.BytesTransferred == 0)
                {
                    //客户端主动断开连接
                    Disconnected(client, "客户端主动断开连接");
                }
                else
                {
                    //被动断开连接
                    Disconnected(client, client.ReceiveDataArgs.SocketError.ToString());
                }
            }
        }
        //断开连接
        private void Disconnected(ClientPeer client, string reason)
        {
            //应用层需要知道这个客户端断开连接了
            app.Disconnected(client, reason);
            client.Disconnected();
            clientPool.Enqueue(client);
            acceptSemaphore.Release();
        }
        //当接收到并解析完成数据需要调用的方法
        private void ProcessReceivedDataComplete(ClientPeer client, SocketMessage msg)
        {
            //应用层调用
            app.ReceiveMessage(client, msg);
            //客户端主动下线
            //if (msg.opCode == (int)ApplicationProtocol.OFFLINE)
            {
                client.IsDisconnected = true;
            }
        }
    }
}
