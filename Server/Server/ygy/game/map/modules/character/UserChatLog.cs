using Server.ygy.game.map.util.common.interfaceDefine;
using Server.Ygy.Game.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.modules.character
{
    public class UserChatLog : ICommonBean
    {
        private string receive_account;
        private string msg;
        private string date;

        public UserChatLog(string receive_account, string msg, string date)
        {
            Receive_account = receive_account;
            Msg = msg;
            Date = date;
        }

        public string Receive_account { get => receive_account; set => receive_account = value; }
        public string Msg { get => msg; set => msg = value; }
        public string Date { get => date; set => date = value; }

        public void Save2DB(object dbMsg)
        {
            if (dbMsg == null)
            {
                dbMsg = new DBUserChatLog();
            }
            DBUserChatLog dBUserChatLog = dbMsg as DBUserChatLog;
            if(dBUserChatLog == null)
            {
                return;
            }
            dBUserChatLog.ReceiveAccount = receive_account;
            dBUserChatLog.Msg = msg;
            dBUserChatLog.Date = date;
        }

        public void Serialie2PB(object pbMsg)
        {
            throw new NotImplementedException();
        }

        public void SerialieFromDB(object dbMsg)
        {
            DBUserChatLog dBUserChatLog = dbMsg as DBUserChatLog;
            if (dBUserChatLog == null)
            {
                return;
            }
            receive_account = dBUserChatLog.ReceiveAccount;
            msg = dBUserChatLog.Msg;
            date = dBUserChatLog.Date;
        }
    }
}
