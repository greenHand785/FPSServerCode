using Server.ygy.game.map.util.common.interfaceDefine;
using Server.Ygy.Game.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.modules.character
{
    public class UserData : ICommonBean
    {
        private UserCommonData common_data;
        private UserFightData fight_data;

        public UserCommonData Common_data { get => common_data; set => common_data = value; }
        public UserFightData Fight_data { get => fight_data; set => fight_data = value; }

        public UserData()
        {
            this.Common_data = new UserCommonData();
            this.Fight_data = new UserFightData();
        }

        public UserData(UserCommonData userCommonData, UserFightData userFightData)
        {
            if(userCommonData == null)
            {
                userCommonData = new UserCommonData();
            }
            if(userFightData == null)
            {
                userFightData = new UserFightData();
            }
            this.Common_data = userCommonData;
            this.Fight_data = userFightData;
        }

        // 将数据存入pbMsg中
        public void Serialie2PB(object pbMsg)
        {

        }

        // 从数据库中获取数据
        public void SerialieFromDB(object dbMsg)
        {
            DBUserData dBUserData = dbMsg as DBUserData;
            if(dBUserData == null)
            {
                return;
            }
            common_data.SerialieFromDB(dBUserData.CommonData);
            fight_data.SerialieFromDB(dBUserData.FightData);
        }

        // 将数据保存到数据库中
        public void Save2DB(object dbMsg)
        {
            if(dbMsg == null)
            {
                dbMsg = new DBUserData();
            }
            DBUserData dBUserData = dbMsg as DBUserData;
            if (dBUserData == null)
            {
                return;
            }
            common_data.Save2DB(dBUserData.CommonData);
            fight_data.Save2DB(dBUserData.FightData);
        }

    }
}
