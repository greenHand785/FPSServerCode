using Server.ygy.game.map.util.common.interfaceDefine;
using Server.Ygy.Game.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.modules.character
{
    
    public class Character : ICommonBean
    {
        // ----------------属性------------------------
        private UserData user_data;
        private UserFriend user_friend;
        private List<UserChatLog> user_chat_log;
        private List<GameData> game_data;

        public UserData User_data { get => user_data; set => user_data = value; }
        public UserFriend User_friend { get => user_friend; set => user_friend = value; }
        public List<UserChatLog> User_chat_log { get => user_chat_log; set => user_chat_log = value; }
        public List<GameData> Game_data { get => game_data; set => game_data = value; }

        // --------------------------------------------

        public Character()
        {
            User_chat_log = new List<UserChatLog>();
            Game_data = new List<GameData>();
            User_data = new UserData();
            User_friend = new UserFriend();
        }

        // 将数据存入pbMsg中
        public void Serialie2PB(object pbMsg)
        {

        }

        // 从数据库中获取数据
        public void SerialieFromDB(object dbMsg)
        {
            DBCharacter dBCharacter = dbMsg as DBCharacter;
            if(dBCharacter == null)
            {
                return;
            }
            user_data.SerialieFromDB(dBCharacter.UserData);
            user_friend.SerialieFromDB(dBCharacter.UserFriend);
            user_chat_log.Clear();
            foreach (DBUserChatLog item in dBCharacter.UserChatLog)
            {
                UserChatLog userChatLog = new UserChatLog(item.ReceiveAccount, item.Msg, item.Date);
                userChatLog.SerialieFromDB(item);
                user_chat_log.Add(userChatLog);
            }
            Game_data.Clear();
            foreach(DBGameData item in dBCharacter.GameData)
            {
                GameData gameData = new GameData();
                gameData.SerialieFromDB(item);
                Game_data.Add(gameData);
            }
        }

        // 将数据保存到数据库中
        public void Save2DB(object dbMsg)
        {
            if(dbMsg == null)
            {
                dbMsg = new DBCharacter();
            }
            DBCharacter dBCharacter = dbMsg as DBCharacter;
            if (dBCharacter == null)
            {
                return;
            }
            user_data.Save2DB(dBCharacter.UserData);
            user_friend.Save2DB(dBCharacter.UserFriend);
            dBCharacter.UserChatLog.Clear();
            foreach (var item in user_chat_log)
            {
                DBUserChatLog dBUserChatLog = new DBUserChatLog();
                item.Save2DB(dBUserChatLog);
                dBCharacter.UserChatLog.Add(dBUserChatLog);
            }
            dBCharacter.GameData.Clear();
            foreach (var item in Game_data)
            {
                DBGameData dBGameData = new DBGameData();
                item.Save2DB(dBGameData);
                dBCharacter.GameData.Add(dBGameData);
            }
        }

        // 获得密码
        public string GetPassWord()
        {
            if(User_data == null)
            {
                return null;
            }
            if(User_data.Common_data == null)
            {
                return null;
            }
            return User_data.Common_data.Password;
        }

        // 获得名称
        public string GetName()
        {
            if (User_data == null)
            {
                return null;
            }
            if (User_data.Common_data == null)
            {
                return null;
            }
            return User_data.Common_data.Name;
        }

        // 获得账号
        public string GetAccount()
        {
            if (User_data == null)
            {
                return null;
            }
            if (User_data.Common_data == null)
            {
                return null;
            }
            return User_data.Common_data.Account;
        }
    }
}
