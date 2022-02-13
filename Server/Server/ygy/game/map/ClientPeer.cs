using Server.ygy.game.map.modules.character;
using Server.ygy.game.map.util.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ygy.Game.Map
{
    public class ClientPeer
    {
        public delegate void ReceivedCompleted(ClientPeer client, SocketMessage msg);
        public ReceivedCompleted receivedCompleted;
        public delegate void SendDisconnected(ClientPeer client, string reason);
        public SendDisconnected sendDisconnected;
        public Socket ClientSocket { get; set; }
        public SocketAsyncEventArgs ReceiveDataArgs { get; }
        private SocketAsyncEventArgs SendDataArgs;
        private Queue<byte[]> sendDataCache { get; }
        private List<byte> ReceiveDataCache { get; }
        private bool receiveIsProcess = false;
        private bool sendIsProcess = false;
        public Character character { get; set; }
        public int PiPeiType { get; set; }
        /// <summary>
        /// 是否发送了当前等待人数
        /// </summary>
        public bool IsSendWaitingCount { get; set; }
        /// <summary>
        /// 是否下线
        /// </summary>
        public bool IsDisconnected = true;
        /// <summary>
        /// 没有收到心跳包的次数
        /// </summary>
        public int OutTimeCount = 0;
        /// <summary>
        /// 是否接受到心跳包
        /// </summary>
        public bool IsRecvHeartPacket = false;
        public ClientPeer(Socket c)
        {
            character = null;
            IsSendWaitingCount = false;
            PiPeiType = -1;
            ClientSocket = c;
            ReceiveDataArgs = new SocketAsyncEventArgs();
            ReceiveDataCache = new List<byte>();
            sendDataCache = new Queue<byte[]>();
            SendDataArgs = new SocketAsyncEventArgs();
            SendDataArgs.Completed += SendDataArgs_Completed;
            //设置接收数据缓冲区的大小
            ReceiveDataArgs.SetBuffer(new byte[1024], 0, 1024);
            ReceiveDataArgs.UserToken = this;
        }
        //接收数据
        public void ProcessReceivedData(byte[] packet)
        {
            //将数据包添加到数据缓冲区中
            ReceiveDataCache.AddRange(packet);
            if (!receiveIsProcess)
            {
                processingReceive();
            }
        }
        private void processingReceive()
        {
            receiveIsProcess = true;
            byte[] data;
            //解码
            try
            {
                data = CodingTool.DecodingPacket(ReceiveDataCache);
            }
            catch(Exception e)
            {
                data = null;
            }
            if (data == null)
            {
                receiveIsProcess = false;
                return;
            }
            SocketMessage msg = CodingTool.DecodingMessage(data);
            receivedCompleted(this, msg);
            processingReceive();
        }
        //发送数据
        public void Send(int opCode, int subCode, object value)
        {
            lock (this)
            {
                SocketMessage msg = new SocketMessage(opCode, subCode, value);
                byte[] data = CodingTool.EncodingMessage(msg);
                byte[] packet = CodingTool.EncodingPacket(data);
                sendDataCache.Enqueue(packet);
                //开始发送数据
                if (!sendIsProcess)
                {
                    StartSendData();
                }
            }
        }

        // 發送數據
        public void Send(MSGID msgId, object value)
        {
            lock (this)
            {
                SocketMessage msg = new SocketMessage((int)msgId, 0, value);
                byte[] data = CodingTool.EncodingMessage(msg);
                byte[] packet = CodingTool.EncodingPacket(data);
                sendDataCache.Enqueue(packet);
                //开始发送数据
                if (!sendIsProcess)
                {
                    StartSendData();
                }
            }
        }

        // 强制下线
        public void Abort()
        {
            try
            {
                ClientSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception e)
            {
                LogNoteManager.Instance.Log("ClientPeer: 132 Shutdown error!");
            }
            ClientSocket.Close();
        }

        /// <summary>
        /// 开始发送数据
        /// </summary>
        private void StartSendData()
        {
            if (sendDataCache.Count == 0)
            {
                sendIsProcess = false;
                return;
            }
            sendIsProcess = true;
            byte[] packet = sendDataCache.Dequeue();
            SendDataArgs.SetBuffer(packet, 0, packet.Length);
            bool r = ClientSocket.SendAsync(SendDataArgs);
            if (!r)
            {
                ProcessSendComplete();
            }
        }
        private void ProcessSendComplete()
        {
            if (SendDataArgs.SocketError != SocketError.Success)
            {
                sendDisconnected(this, SendDataArgs.SocketError.ToString());
            }
            else
            {
                StartSendData();
            }
        }
        /// <summary>
        /// 当发送完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendDataArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessSendComplete();
        }
        //断开连接
        public void Disconnected()
        {
            ReceiveDataCache.Clear();
            sendDataCache.Clear();
            character = null;
            OutTimeCount = 0;
            PiPeiType = -1;
            IsRecvHeartPacket = false;
            IsDisconnected = true;
            sendIsProcess = false;
            receiveIsProcess = false;
            try
            {
                ClientSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception e) { }
            ClientSocket.Close();
            ClientSocket = null;
        }
    }
}
