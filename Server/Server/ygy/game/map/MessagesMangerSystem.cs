using Server.ygy.game.map.modules.character;
using Server.ygy.game.map.modules.chat;
using Server.ygy.game.map.modules.game;
using Server.ygy.game.map.modules.hall;
using Server.ygy.game.map.modules.login;
using Server.ygy.game.map.util.common.eventManager;
using Server.Ygy.Game.Map.Util.Common.EventManager;
using Server.Ygy.Game.Pb;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;
using static Server.ygy.game.map.util.common.define.DelegateDefine;

namespace Server.Ygy.Game.Map
{
    public class MessagesMangerSystem : IGameApplication
    {
        // 模块字典
        private ConcurrentDictionary<MSGID, MessageFunc> messageDic;

        private Timer timer;

        /// <summary>
        /// 构造函数，
        /// 创建模块列表对象，并将模块添加进模块列表中
        /// </summary>
        public MessagesMangerSystem()
        {
            messageDic = new ConcurrentDictionary<MSGID, MessageFunc>();
            timer = new Timer(60);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            // 初始化消息
            RegisterMessage();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Update();
        }

        /// <summary>
        /// 消息初始化
        /// </summary>
        private void RegisterMessage()
        {
            LoginModuleManager.Instance.RegisterMessage(this);
            ChatModuleManager.Instance.RegisterMessage(this);
            HallModuleManager.Instance.RegisterMessage(this);

        }

        /// <summary>
        /// 移除所有模块的消息
        /// </summary>
        private void ReleaseMessage()
        {
            LoginModuleManager.Instance.ReleaseMessage(this);
            ChatModuleManager.Instance.ReleaseMessage(this);
            HallModuleManager.Instance.ReleaseMessage(this);
        }

        /// <summary>
        /// 每隔10s 时间发送一次心跳包
        /// </summary>
        private void SendHeadPacket()
        {
            //foreach (var item in ClientDicnary.ClientsList.Keys)
            //{
            //    if (ClientDicnary.ClientsList.ContainsKey(item))
            //    {
            //        ClientDicnary.ClientsList[item].Send((int)ApplicationProtocol.HEARTPACKET, (int)ApplicationProtocol.HEARTPACKETSUB, null);
            //    }
            //}
            //timeMashine.AddTimeTask(10000, SendHeadPacket);
            ////检测此次心跳包是否被接受到
            //timeMashine.AddTimeTask(5000, CheckIsRecvHeatPack);
        }

        /// <summary>
        /// 检测是否收到心跳包
        /// </summary>
        private void CheckIsRecvHeatPack()
        {
            //foreach (var account in ClientDicnary.ClientsList.Keys)
            //{
            //    //没有接收到心跳包回复的，超时计数+1
            //    if (!ClientDicnary.ClientsList[account].IsRecvHeartPacket)
            //    {
            //        ClientDicnary.ClientsList[account].OutTimeCount++;
            //    }
            //    //判断是否超过两次没有接受到心跳回复，如果>=2则移除这个客户端的连接
            //    if (ClientDicnary.ClientsList[account].OutTimeCount >= 2)
            //    {
            //        ClientDicnary.ClientsList[account].IsDisconnected = true;
            //    }
            //}
        }
        
        /// <summary>
        /// 注册消息事件
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="pbMsg"></param>
        public void RegisterMessage(MSGID msgId, MessageFunc pbMsg)
        {
            if(messageDic == null)
            {
                return;
            }
            messageDic.TryAdd(msgId, pbMsg);
        }

        /// <summary>
        /// 移除消息事件
        /// </summary>
        /// <param name="msgId"></param>
        public void ReleaseMessage(MSGID msgId)
        {
            if(messageDic == null)
            {
                return;
            }
            if(messageDic.ContainsKey(msgId) == true)
            {
                MessageFunc f;
                messageDic.TryRemove(msgId, out f);
            }
        }

        /// <summary>
        /// 客户端掉线处理
        /// </summary>
        /// <param name="client"></param>
        /// <param name="reason"></param>
        public void Disconnected(ClientPeer client, string reason)
        {
            Character ch = client.character;
            if(ch == null)
            {
                return;
            }
            Console.WriteLine("掉线了 :" + reason);
            EventManager.Instance.PushEvent(EventDefine.Event_OffLine, true, 0, ch.GetAccount());

            //将客户端字典中的记录移除
            //if (client.Account != null)
            //{
            //    //ClientDicnary.OnDisconnected(client);
            //}
            ////将排队等待列表中的记录移除
            //if (client.PiPeiType != -1)
            //{
            //    //PiPeiWaitQueue.OnDisconected((ApplicationProtocol)client.PiPeiType, client);
            //    //移除排位列表中的记录
            //}
            //Console.WriteLine(DateTime.Now+client.Account + "  :  " + reason);
        }
        /// <summary>
        /// 对接收到的数据进行转发到不同的模块中，来进行分模块处理；
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        public void ReceiveMessage(ClientPeer client, SocketMessage msg)
        {
            if(messageDic == null)
            {
                return;
            }
            if (messageDic.ContainsKey((MSGID)msg.opCode))
            {
                MessageFunc function = messageDic[(MSGID)msg.opCode];
                function(client, msg.opCode, msg.value);
            }
        }

        public void StartGaming()
        {
            
        }

        /// <summary>
        /// 刷新速率，60ms执行一次
        /// </summary>
        public void Update()
        {
            // 事件管理器刷新
            EventManager.Instance.Update();

            // 游戏对局管理器
            GameManager.Instance.Update();
        }
    }
}
