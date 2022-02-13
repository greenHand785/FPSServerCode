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
        private List<string> friendsList;

        public UserFriend()
        {
            FriendsList = new List<string>();
        }

        public List<string> FriendsList { get => friendsList; set => friendsList = value; }

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
                return;
            }
            dBUserFriend.Friends.Clear();
            foreach (var item in friendsList)
            {
                dBUserFriend.Friends.Add(item);
            }
        }

        public void Serialie2PB(object pbMsg)
        {
            if(pbMsg == null)
            {
                pbMsg = new PBMsgChatFriendDataList();
            }
            PBMsgChatFriendDataList pBMsgChatFriendDataList = pbMsg as PBMsgChatFriendDataList;
            if(pBMsgChatFriendDataList == null)
            {
                return;
            }
            pBMsgChatFriendDataList.Friends = new List<string>();
            if(FriendsList == null)
            {
                return;
            }
            foreach (var item in FriendsList)
            {
                pBMsgChatFriendDataList.Friends.Add(item);
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
            friendsList.Clear();
            foreach (var item in dBUserFriend.Friends)
            {
                friendsList.Add(item);
            }
        }
    }
}
