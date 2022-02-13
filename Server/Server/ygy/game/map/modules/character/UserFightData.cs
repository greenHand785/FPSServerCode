using Server.ygy.game.map.util.common.interfaceDefine;
using Server.Ygy.Game.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.modules.character
{
    public class UserFightData : ICommonBean
    {
        private int kill_num;
        private int head_shoot_num;
        private int death_num;
        private long harm_num;

        public int Head_shoot_num { get => head_shoot_num; set => head_shoot_num = value; }
        public int Death_num { get => death_num; set => death_num = value; }
        public long Harm_num { get => harm_num; set => harm_num = value; }
        public int Kill_num { get => kill_num; set => kill_num = value; }

        public UserFightData(int kill_num, int head_shoot_num, int death_num, int harm_num)
        {
            Kill_num = kill_num;
            Head_shoot_num = head_shoot_num;
            Death_num = death_num;
            Harm_num = harm_num;
        }

        public UserFightData()
        {

        }

        // 将数据存入pbMsg中
        public void Serialie2PB(object pbMsg)
        {

        }

        // 从数据库中获取数据
        public void SerialieFromDB(object dbMsg)
        {
            DBUserFightData dBUserFightData = dbMsg as DBUserFightData;
            if(dBUserFightData == null)
            {
                return;
            }
            kill_num = dBUserFightData.KillNum;
            head_shoot_num = dBUserFightData.HeadShotNum;
            death_num = dBUserFightData.DeathNum;
            harm_num = dBUserFightData.HarmNum;
        }

        // 将数据保存到数据库中
        public void Save2DB(object dbMsg)
        {
            if(dbMsg == null)
            {
                dbMsg = new DBUserFightData();
            }
            DBUserFightData dBUserFightData = dbMsg as DBUserFightData;
            if(dBUserFightData == null)
            {
                return;
            }
            dBUserFightData.KillNum = kill_num;
            dBUserFightData.HeadShotNum = head_shoot_num;
            dBUserFightData.DeathNum = death_num;
            dBUserFightData.HarmNum = harm_num;
        }

    }
}
