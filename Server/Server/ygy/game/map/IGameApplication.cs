using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ygy.Game.Map
{
    public interface IGameApplication
    {
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        void ReceiveMessage(ClientPeer client, SocketMessage msg);
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="client"></param>
        /// <param name="reason"></param>
        void Disconnected(ClientPeer client, string reason);
        /// <summary>
        /// 开始游戏
        /// </summary>
        void StartGaming();
    }
}
