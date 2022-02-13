using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ygy.Game.Map
{
    public class ClientPool
    {
        public Queue<ClientPeer> pool;
        public ClientPool(int capcity)
        {
            pool = new Queue<ClientPeer>(capcity);
        }
        //出队
        public ClientPeer Dequeue()
        {
            return pool.Dequeue();
        }
        //入队
        public void Enqueue(ClientPeer c)
        {
            pool.Enqueue(c);
        }
    }
}
