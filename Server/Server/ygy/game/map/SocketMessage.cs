using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ygy.Game.Map
{
    [Serializable]
    public class SocketMessage
    {
        //操作码
        public int opCode { get; }
        //子操作
        public int subCode { get; }
        //携带参数数据
        public object value { get; }
        public SocketMessage(int opcode,int subcode, object value)
        {
            this.opCode = opcode;
            this.subCode = subcode;
            this.value = value;
        }
    }
}
