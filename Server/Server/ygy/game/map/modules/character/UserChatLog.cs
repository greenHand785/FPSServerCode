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
    public class UserChatLog : ICommonBean
    {
        private string send_account;
        private string receive_account;
        private string msg;
        private long date;

        public UserChatLog()
        {

        }

        public UserChatLog(string send_account, string receive_account, string msg, long date)
        {
            Send_account = send_account;
            Receive_account = receive_account;
            Msg = msg;
            Date = date;
        }

        public string Receive_account { get => receive_account; set => receive_account = value; }
        public string Msg { get => msg; set => msg = value; }
        public long Date { get => date; set => date = value; }
        public string Send_account { get => send_account; set => send_account = value; }

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
            if(pbMsg == null)
            {
                return;
            }
            PBMsgChatMsg msg = pbMsg as PBMsgChatMsg;
            if(msg == null)
            {
                return;
            }
            msg.Date = date;
            msg.Msg = this.msg;
        }

        public void SerialieFromDB(object dbMsg)
        {
            DBUserChatLog dBUserChatLog = dbMsg as DBUserChatLog;
            if (dBUserChatLog == null)
            {
                return;
            }
            Send_account = dBUserChatLog.SendAccount;
            receive_account = dBUserChatLog.ReceiveAccount;
            msg = dBUserChatLog.Msg;
            date = dBUserChatLog.Date;
        }
    }
}
