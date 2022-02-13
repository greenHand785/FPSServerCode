using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.util.common.interfaceDefine
{
    public interface ICommonBean
    {
        // 将数据存入pbMsg中
        void Serialie2PB(object pbMsg);

        // 从数据库中获取数据
        void SerialieFromDB(object dbMsg);

        // 将数据保存到数据库中
        void Save2DB(object dbMsg);
    }
}
