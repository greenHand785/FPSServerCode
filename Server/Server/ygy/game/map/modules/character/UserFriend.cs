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
    public class UserFriend : ICommonBean
    {
        private List<FriendInfo> friendsList;

        public void Save2DB(object dbMsg)
        {
            if(dbMsg == null)
            {
                dbMsg = new DBUserFriend();
            }
            DBUserFriend dBUserFriend = dbMsg as DBUserFriend;
            if (dBUserFriend == null)
            {
                return;
            }
            if(dBUserFriend.Friends == null)
            {
                dBUserFriend.Friends = new List<DBFriendInfo>();
            }
            dBUserFriend.Friends.Clear();
            foreach (var item in friendsList)
            {
                DBFriendInfo info = null;
                item.Save2DB(info);
                dBUserFriend.Friends.Add(info);
            }
        }

        public void Serialie2PB(object pbMsg)
        {
            if(pbMsg == null)
            {
                return;
            }
            List<PBMsgFriendInfo> pBMsgChatFriendDataList = pbMsg as List<PBMsgFriendInfo>;
            if(pBMsgChatFriendDataList == null)
            {
                return;
            }
            if(friendsList == null)
            {
                return;
            }
            foreach (var item in friendsList)
            {
                PBMsgFriendInfo info = new PBMsgFriendInfo();
                item.Serialie2PB(info);
                pBMsgChatFriendDataList.Add(info);
            }
        }

        public void SerialieFromDB(object dbMsg)
        {
            DBUserFriend dBUserFriend = dbMsg as DBUserFriend;
            if (dBUserFriend == null)
            {
                return;
            }
            if (dBUserFriend.Friends == null)
            {
                return;
            }
            if(friendsList == null)
            {
                friendsList = new List<FriendInfo>();
            }
            friendsList.Clear();
            foreach (var item in dBUserFriend.Friends)
            {
                FriendInfo info = new FriendInfo();
                info.SerialieFromDB(item);
                friendsList.Add(info);
            }
        }

        public void AddFriend(FriendInfo info)
        {
            if(info == null)
            {
                return;
            }
            if(friendsList == null)
            {
                friendsList = new List<FriendInfo>();
            }
            friendsList.Add(info);
        }

        public FriendInfo GetFriendInfo(string account)
        {
            if(friendsList == null)
            {
                return null;
            }
            foreach (var item in friendsList)
            {
                if(item.Account == account)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
