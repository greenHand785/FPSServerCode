using Server.ygy.game.map.util.common.db;
using Server.ygy.game.map.util.common.interfaceDefine;
using Server.Ygy.Game.Db;
using Server.Ygy.Game.Pb;
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
        private List<UserChatLog> user_send_chat_log;
        private List<UserChatLog> user_receive_chat_log;
        private List<GameRecordData> game_data;

        public Character()
        {
            user_data = new UserData();
            user_friend = new UserFriend();
            user_send_chat_log = new List<UserChatLog>();
            user_receive_chat_log = new List<UserChatLog>();
            game_data = new List<GameRecordData>();
        }

        // 将数据存入pbMsg中
        public void Serialie2PB(object pbMsg)
        {

        }

        // 从数据库中获取数据
        public void SerialieFromDB(object dbMsg)
        {
            DBCharacter dBCharacter = dbMsg as DBCharacter;
            if (dBCharacter == null)
            {
                return;
            }
            user_data.SerialieFromDB(dBCharacter.UserData);
            user_friend.SerialieFromDB(dBCharacter.UserFriend);
            user_send_chat_log.Clear();
            foreach (DBUserChatLog item in dBCharacter.UserSendChatLog)
            {
                UserChatLog userChatLog = new UserChatLog();
                userChatLog.SerialieFromDB(item);
                user_send_chat_log.Add(userChatLog);
            }
            user_receive_chat_log.Clear();
            foreach (DBUserChatLog item in dBCharacter.UserReceiveChatLog)
            {
                UserChatLog userChatLog = new UserChatLog();
                userChatLog.SerialieFromDB(item);
                user_receive_chat_log.Add(userChatLog);
            }
            game_data.Clear();
            foreach (DBGameRecordData item in dBCharacter.GameData)
            {
                GameRecordData gameData = new GameRecordData();
                gameData.SerialieFromDB(item);
                game_data.Add(gameData);
            }
        }

        // 将数据保存到数据库中
        public void Save2DB(object dbMsg)
        {
            if (dbMsg == null)
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
            if (dBCharacter.UserSendChatLog == null)
            {
                dBCharacter.UserSendChatLog = new List<DBUserChatLog>();
            }
            dBCharacter.UserSendChatLog.Clear();
            foreach (var item in user_send_chat_log)
            {
                DBUserChatLog dBUserChatLog = new DBUserChatLog();
                item.Save2DB(dBUserChatLog);
                dBCharacter.UserSendChatLog.Add(dBUserChatLog);
            }
            if (dBCharacter.UserReceiveChatLog == null)
            {
                dBCharacter.UserReceiveChatLog = new List<DBUserChatLog>();
            }
            dBCharacter.UserReceiveChatLog.Clear();
            foreach (var item in user_receive_chat_log)
            {
                DBUserChatLog dBUserChatLog = new DBUserChatLog();
                item.Save2DB(dBUserChatLog);
                dBCharacter.UserReceiveChatLog.Add(dBUserChatLog);
            }
            if (dBCharacter.GameData == null)
            {
                dBCharacter.GameData = new List<DBGameRecordData>();
            }
            dBCharacter.GameData.Clear();
            foreach (var item in game_data)
            {
                DBGameRecordData dBGameData = new DBGameRecordData();
                item.Save2DB(dBGameData);
                dBCharacter.GameData.Add(dBGameData);
            }
        }

        // 添加消息
        public void AddMessage(UserChatLog msg)
        {
            if (user_send_chat_log == null)
            {
                return;
            }
            user_send_chat_log.Add(msg);
            // 保存消息
            DBTool.Instance.SaveMsg(msg);
        }

        // 添加好友
        public void AddFriend(string friendAccount, string name, string userImg)
        {
            if (user_friend == null)
            {
                return;
            }
            user_friend.AddFriend(new FriendInfo
            {
                Account = friendAccount,
                Name = name,
                User_img = userImg
            });
            // 保存
            DBTool.Instance.SaveFriend(GetAccount(), friendAccount);
        }

        // 设置用户头像
        public void SetUserImg(string path)
        {
            if (user_data == null || user_data.Common_data == null)
            {
                return;
            }
            user_data.Common_data.User_img = path;
            // 保存当前玩家的头像
            DBTool.Instance.SaveUserImg(path, GetAccount());
        }

        // 设置用户名称
        public void SetUserName(string name)
        {
            if (user_data == null || user_data.Common_data == null)
            {
                return;
            }
            user_data.Common_data.Name = name;
            // 保存当前玩家名称
            DBTool.Instance.SaveUserName(name, GetAccount());
        }

        public List<UserChatLog> GetUserSendChatLog()
        {
            if (user_send_chat_log == null)
            {
                return null;
            }
            return user_send_chat_log;
        }

        public UserData GetUserData()
        {
            return user_data;
        }

        public UserFriend GetUserFriend()
        {
            return user_friend;
        }
        // 获得好友发送过来的消息
        public void Serealize2PBMsgFriend(object pbObj)
        {
            if (pbObj == null)
            {
                return;
            }
            List<PBMsgFriendChatMsg> list = pbObj as List<PBMsgFriendChatMsg>;
            if (user_receive_chat_log == null)
            {
                return;
            }
            Dictionary<string, PBMsgFriendChatMsg> msgDic = new Dictionary<string, PBMsgFriendChatMsg>();
            foreach (var item in user_receive_chat_log)
            {
                if (msgDic.ContainsKey(item.Send_account))
                {
                    msgDic[item.Send_account].Account = item.Send_account;
                    if(msgDic[item.Send_account].ChatMsgs == null)
                    {
                        msgDic[item.Send_account].ChatMsgs = new List<PBMsgChatMsg>();
                    }
                    PBMsgChatMsg chatMsg = new PBMsgChatMsg();
                    item.Serialie2PB(chatMsg);
                    msgDic[item.Send_account].ChatMsgs.Add(chatMsg);
                }
                else
                {
                    PBMsgFriendChatMsg pbMsg = new PBMsgFriendChatMsg();
                    pbMsg.Account = item.Send_account;
                    if (pbMsg.ChatMsgs == null)
                    {
                        pbMsg.ChatMsgs = new List<PBMsgChatMsg>();
                    }
                    PBMsgChatMsg chatMsg = new PBMsgChatMsg();
                    item.Serialie2PB(chatMsg);
                    pbMsg.ChatMsgs.Add(chatMsg);
                    msgDic.Add(item.Send_account, pbMsg);
                }
            }
            foreach (var item in msgDic)
            {
                list.Add(item.Value);
            }
        }

        // 获得自己发送给好友的消息
        public void Serealize2PBMsgSelf(object pbObj)
        {
            if (pbObj == null)
            {
                return;
            }
            List<PBMsgFriendChatMsg> list = pbObj as List<PBMsgFriendChatMsg>;
            if (user_send_chat_log == null)
            {
                return;
            }
            Dictionary<string, PBMsgFriendChatMsg> msgDic = new Dictionary<string, PBMsgFriendChatMsg>();
            foreach (var item in user_send_chat_log)
            {
                if (msgDic.ContainsKey(item.Receive_account))
                {
                    msgDic[item.Receive_account].Account = item.Receive_account;
                    if (msgDic[item.Receive_account].ChatMsgs == null)
                    {
                        msgDic[item.Receive_account].ChatMsgs = new List<PBMsgChatMsg>();
                    }
                    PBMsgChatMsg chatMsg = new PBMsgChatMsg();
                    item.Serialie2PB(chatMsg);
                    msgDic[item.Receive_account].ChatMsgs.Add(chatMsg);
                }
                else
                {
                    PBMsgFriendChatMsg pbMsg = new PBMsgFriendChatMsg();
                    pbMsg.Account = item.Receive_account;
                    if (pbMsg.ChatMsgs == null)
                    {
                        pbMsg.ChatMsgs = new List<PBMsgChatMsg>();
                    }
                    PBMsgChatMsg chatMsg = new PBMsgChatMsg();
                    item.Serialie2PB(chatMsg);
                    pbMsg.ChatMsgs.Add(chatMsg);
                    msgDic.Add(item.Receive_account, pbMsg);
                }
            }
            foreach (var item in msgDic)
            {
                list.Add(item.Value);
            }
        }

        // 获得游戏对局数据
        public void Serealize2PBMsgGameRecord(PBMsgUserGameRecordResponse response)
        {
            if (response == null)
            {
                response = new PBMsgUserGameRecordResponse();
            }
            if (game_data == null)
            {
                return;
            }
            response.Info = new List<PBMsgGameRecordInfo>();
            foreach (var item in game_data)
            {
                PBMsgGameRecordInfo info = new PBMsgGameRecordInfo();
                item.Serialie2PB(info);
                response.Info.Add(info);
            }
        }
        // 获得用户数据
        public void Serealize2PBMsgUserData(PBMsgUserDataResponse response)
        {
            if (response == null)
            {
                response = new PBMsgUserDataResponse();
            }
            if (user_data == null)
            {
                return;
            }
            response.UserData = new PBMsgUserData();
            user_data.Serialie2PB(response.UserData);
        }

        // 获得排名前十列表
        public void Serealize2PBMsgRankList(List<PBMsgGameData> list)
        {
            if (list == null)
            {
                list = new List<PBMsgGameData>();
            }
            list = DBTool.Instance.SelectRankTenListByKillNum();
        }

        // 获得密码
        public string GetPassWord()
        {
            if (user_data == null)
            {
                return null;
            }
            if (user_data.Common_data == null)
            {
                return null;
            }
            return user_data.Common_data.Password;
        }

        // 获得名称
        public string GetName()
        {
            if (user_data == null)
            {
                return null;
            }
            if (user_data.Common_data == null)
            {
                return null;
            }
            return user_data.Common_data.Name;
        }

        public string GetUserImg()
        {
            if (user_data == null)
            {
                return null;
            }
            if (user_data.Common_data == null)
            {
                return null;
            }
            return user_data.Common_data.User_img;
        }

        // 获得账号
        public string GetAccount()
        {
            if (user_data == null)
            {
                return null;
            }
            if (user_data.Common_data == null)
            {
                return null;
            }
            return user_data.Common_data.Account;
        }

        // 是否已经添加为好友
        public bool IsFriend(string account)
        {
            if (user_friend == null)
            {
                return false;
            }
            if (user_friend.GetFriendInfo(account) == null)
            {
                return false;
            }
            return true;
        }
    }
}
