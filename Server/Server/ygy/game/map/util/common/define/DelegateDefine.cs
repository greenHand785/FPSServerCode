using Server.Ygy.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.util.common.define
{
    public class DelegateDefine
    {
        // 消息方法
        public delegate void MessageFunc(ClientPeer client, int pbMsg, object pbMsgObj);
        // 事件方法
        public delegate void EventFunc(ClientPeer client);
    }
}
