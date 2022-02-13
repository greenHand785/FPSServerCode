using Server.Ygy.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.util.common.eventManager
{
    public class ServerEvent
    {
        private ClientPeer client;
        private Action<ClientPeer> action;

        public ClientPeer Client { get => client; set => client = value; }
        public Action<ClientPeer> Action {
            get
            {
                return action;
            }
            set
            {
                action = value;
            }
        }
        public ServerEvent(ClientPeer client, Action<ClientPeer> action)
        {
            Client = client;
            Action = action;
        }
    }
}
