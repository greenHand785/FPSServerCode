using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Ygy.Game.Map;
using Server.Ygy.Game.Pb;
namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer serverPeer = new ServerPeer(8888, 10);
            serverPeer.SetApplication(new MessagesMangerSystem());
            serverPeer.Start();

            Console.Read();
        }
    }
}
