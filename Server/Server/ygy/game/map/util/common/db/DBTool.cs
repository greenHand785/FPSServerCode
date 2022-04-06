using Server.ygy.game.map.modules.character;
using Server.Ygy.Game.Db;
using Server.Ygy.Game.Pb;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.util.common.db
{
    /// <summary>
    /// 連接數據庫，初始化玩家數據，將數據庫中的數據存入character類中。
    /// </summary>
    public class DBTool
    {
        private static DBTool instance;
        private string connecStr;
        private SqlConnection connecObj;

        public static DBTool Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new DBTool();
                }
                return instance;
            }
        }

        private DBTool()
        {
            connecStr = "data source=127.0.0.1;initial catalog=FlowerOfLife;user id=sa;pwd=ygy14779817876";
        }

        // TODO 获得玩家数据
        public DBCharacter GetDBCharacter(string account)
        {
            DBCharacter ch = new DBCharacter();
            ch.UserData = GetUserData(account);
            ch.UserFriend = GetFriendUserList(account);
            ch.UserSendChatLog = GetSendUserChatLogList(account);
            ch.UserReceiveChatLog = GetReceiveUserChatLogList(account);
            ch.GameData = GetGameRecordDataList(account);
            return ch;
        }

        // 读取账号数据
        private DBUserData GetUserData(string account)
        {
            DBUserData data = new DBUserData();
            data.CommonData = GetUserCommonData(account);
            data.FightData = GetUserFightData(account);
            return data;
        }
        // 设置账号数据
        private void SetUserData(DBUserData data)
        {
            if(data == null || data.CommonData == null)
            {
                return;
            }
            // 设置账号基本数据
            SetUserCommonData(data.CommonData);
        }

        // 获得账号基本数据
        public DBUserCommonData GetUserCommonData(string account)
        {
            DBUserCommonData data = new DBUserCommonData();
            string queryStr = $"select * from Character where account = '{account}'";
            try
            {
                using (connecObj = new SqlConnection(connecStr))
                {
                    connecObj.Open();
                    SqlCommand querCom = new SqlCommand(queryStr, connecObj);
                    using (SqlDataReader reader = querCom.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader[0].GetType() != typeof(DBNull))
                            {
                                data.Account = reader[0] as string;
                            }
                            if (reader[1].GetType() != typeof(DBNull))
                            {
                                data.PhoneNum = reader[1] as string;
                            }
                            if (reader[2].GetType() != typeof(DBNull))
                            {
                                data.Email = reader[2] as string;
                            }
                            if (reader[3].GetType() != typeof(DBNull))
                            {
                                data.Password = reader[3] as string;
                            }
                            if (reader[4].GetType() != typeof(DBNull))
                            {
                                data.Name = reader[4] as string;
                            }
                            if (reader[5].GetType() != typeof(DBNull))
                            {
                                data.UserImg = reader[5] as string;
                            }
                        }
                    }
                }
                connecObj.Close();
            }
            catch (Exception e)
            {
                LogNoteManager.Instance.Log("DBTool: line:95 error" + e);
            }
            return data;
        }
        // 设置基本数据
        private void SetUserCommonData(DBUserCommonData data)
        {   
            string queryStr = $"update Character set name = '{data.Name}', phone_num = '{data.PhoneNum}', email = '{data.Email}', user_img = '{data.UserImg}', password = '{data.Password}' where account = '{data.Account}'";
            try
            {
                using (connecObj = new SqlConnection(connecStr))
                {
                    connecObj.Open();
                    SqlCommand querCom = new SqlCommand(queryStr, connecObj);
                    querCom.ExecuteNonQuery();
                }
                connecObj.Close();
            }
            catch (Exception e)
            {
                LogNoteManager.Instance.Log("DBTool: line:95 error" + e);
            }
        }

        // 获得账号战斗总数据
        private DBUserFightData GetUserFightData(string account)
        {
            DBUserFightData data = new DBUserFightData();
            string queryStr = $"select SUM(kill_num), SUM(head_shot_num), SUM(death_num),SUM(harm_num) from GameRecordInfo where account = '{account}'";
            try
            {
                using (connecObj = new SqlConnection(connecStr))
                {
                    connecObj.Open();
                    SqlCommand querCom = new SqlCommand(queryStr, connecObj);
                    using (SqlDataReader reader = querCom.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if(reader[0].GetType() != typeof(DBNull))
                            {
                                data.KillNum = Convert.ToInt32(reader[0]);
                            }
                            if(reader[1].GetType() != typeof(DBNull))
                            {
                                data.HeadShotNum = Convert.ToInt32(reader[1]);
                            }
                            if (reader[2].GetType() != typeof(DBNull))
                            {
                                data.DeathNum = Convert.ToInt32(reader[2]);
                            }
                            if (reader[3].GetType() != typeof(DBNull))
                            {
                                data.HarmNum = Convert.ToInt64(reader[3]);
                            }
                        }
                    }
                }
                connecObj.Close();
            }
            catch (Exception e)
            {
                LogNoteManager.Instance.Log("DBTool: line:126 error" + e);
            }
            return data;
        }

        // 获得好友账号列表
        private DBUserFriend GetFriendUserList(string account)
        {
            DBUserFriend data = new DBUserFriend();
            data.Friends = new List<DBFriendInfo>();
            string queryStr = $"select * from Friend,Character where friend_account1 = '{account}' and friend_account2 = account";
            try
            {
                using (connecObj = new SqlConnection(connecStr))
                {
                    connecObj.Open();
                    SqlCommand querCom = new SqlCommand(queryStr, connecObj);
                    using (SqlDataReader reader = querCom.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DBFriendInfo info = new DBFriendInfo();
                            if (reader[1].GetType() != typeof(DBNull))
                            {
                                info.Account = reader.GetString(1);
                            }
                            if (reader[6].GetType() != typeof(DBNull))
                            {
                                info.Name = reader.GetString(6);
                            }
                            if (reader[7].GetType() != typeof(DBNull))
                            {
                                info.UserImg = reader.GetString(7);
                            }
                            data.Friends.Add(info);
                        }
                    }
                }
                connecObj.Close();
            }
            catch (Exception e)
            {
                LogNoteManager.Instance.Log("DBTool: line:126 error" + e);
            }
            return data;
        }
       
        // 获得发送聊天记录列表
        private List<DBUserChatLog> GetSendUserChatLogList(string account)
        {
            List<DBUserChatLog> data = new List<DBUserChatLog>();
            string queryStr = $"select * from Message where send_account = '{account}'";
            try
            {
                using (connecObj = new SqlConnection(connecStr))
                {
                    connecObj.Open();
                    SqlCommand querCom = new SqlCommand(queryStr, connecObj);
                    using (SqlDataReader reader = querCom.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DBUserChatLog dBUserChat = new DBUserChatLog();
                            if (reader[0].GetType() != typeof(DBNull))
                            {
                                dBUserChat.SendAccount = reader[0] as string;
                            }
                            if (reader[1].GetType() != typeof(DBNull))
                            {
                                dBUserChat.ReceiveAccount = reader[1] as string;
                            }
                            if (reader[2].GetType() != typeof(DBNull))
                            {
                                dBUserChat.Msg = reader[2] as string;
                            }
                            if (reader[3].GetType() != typeof(DBNull))
                            {
                                dBUserChat.Date = Convert.ToInt64(reader[3]);
                            }
                            data.Add(dBUserChat);
                        }
                    }
                }
                connecObj.Close();
            }
            catch (Exception e)
            {
                LogNoteManager.Instance.Log("DBTool: line:126 error" + e);
            }
            return data;
        }

        // 获得接受聊天记录列表
        private List<DBUserChatLog> GetReceiveUserChatLogList(string account)
        {
            List<DBUserChatLog> data = new List<DBUserChatLog>();
            string queryStr = $"select * from Message where receive_account = '{account}'";
            try
            {
                using (connecObj = new SqlConnection(connecStr))
                {
                    connecObj.Open();
                    SqlCommand querCom = new SqlCommand(queryStr, connecObj);
                    using (SqlDataReader reader = querCom.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DBUserChatLog dBUserChat = new DBUserChatLog();
                            if (reader[0].GetType() != typeof(DBNull))
                            {
                                dBUserChat.SendAccount = reader[0] as string;
                            }
                            if (reader[1].GetType() != typeof(DBNull))
                            {
                                dBUserChat.ReceiveAccount = reader[1] as string;
                            }
                            if (reader[2].GetType() != typeof(DBNull))
                            {
                                dBUserChat.Msg = reader[2] as string;
                            }
                            if (reader[3].GetType() != typeof(DBNull))
                            {
                                dBUserChat.Date = Convert.ToInt64(reader[3]);
                            }
                            data.Add(dBUserChat);
                        }
                    }
                }
                connecObj.Close();
            }
            catch (Exception e)
            {
                LogNoteManager.Instance.Log("DBTool: line:126 error" + e);
            }
            return data;
        }

        // 获得对局记录
        private List<DBGameRecordData> GetGameRecordDataList(string account)
        {
            List<DBGameRecordData> data = new List<DBGameRecordData>();
            string queryStr = $"select GameRecord.id,GameRecord.start_time from GameRecordInfo,GameRecord where account = '{account}' and GameRecord.id = GameRecordInfo.id";
            try
            {
                using (connecObj = new SqlConnection(connecStr))
                {
                    connecObj.Open();
                    SqlCommand querCom = new SqlCommand(queryStr, connecObj);
                    using (SqlDataReader reader = querCom.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DBGameRecordData dbGame = new DBGameRecordData();
                            if (reader[0].GetType() != typeof(DBNull))
                            {
                                dbGame.Id = Convert.ToInt32(reader[0]);
                            }
                            if (reader[1].GetType() != typeof(DBNull))
                            {
                                dbGame.StartDate = Convert.ToInt64(reader[1]);
                            }
                            data.Add(dbGame);
                        }
                    }
                }
                connecObj.Close();
            }
            catch (Exception e)
            {
                LogNoteManager.Instance.Log("DBTool: line:126 error" + e);
            }
            foreach (var item in data)
            {
                item.Info = GetGameInfoList(item.Id);
            }
            return data;
        }

        // 获得对局信息列表
        private List<DBGameData> GetGameInfoList(long id)
        {
            List<DBGameData> data = new List<DBGameData>();
            string queryStr = $"select * from GameRecordInfo,Character where id = {id} and Character.account = GameRecordInfo.account";
            try
            {
                using (connecObj = new SqlConnection(connecStr))
                {
                    connecObj.Open();
                    SqlCommand querCom = new SqlCommand(queryStr, connecObj);
                    using (SqlDataReader reader = querCom.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DBGameData dbGame = new DBGameData();
                            if (reader[1].GetType() != typeof(DBNull))
                            {
                                dbGame.EndDate = Convert.ToInt64(reader[1]);
                            }
                            if (reader[2].GetType() != typeof(DBNull))
                            {
                                dbGame.HeadShotNum = Convert.ToInt32(reader[2]);
                            }
                            if (reader[3].GetType() != typeof(DBNull))
                            {
                                dbGame.DeathNum = Convert.ToInt32(reader[3]);
                            }
                            if (reader[4].GetType() != typeof(DBNull))
                            {
                                dbGame.KillNum = Convert.ToInt32(reader[4]);
                            }
                            if (reader[5].GetType() != typeof(DBNull))
                            {
                                dbGame.HarmNum = Convert.ToInt32(reader[5]);
                            }
                            if (reader[6].GetType() != typeof(DBNull))
                            {
                                dbGame.Rank = Convert.ToInt32(reader[6]);
                            }
                            if (reader[7].GetType() != typeof(DBNull))
                            {
                                dbGame.Account = Convert.ToString(reader[7]);
                            }
                            if (reader[12].GetType() != typeof(DBNull))
                            {
                                dbGame.Name = reader.GetString(12);
                            }
                            data.Add(dbGame);
                        }
                    }
                }
                connecObj.Close();
            }
            catch (Exception e)
            {
                LogNoteManager.Instance.Log("DBTool: line:126 error" + e);
            }
            return data;
        }

        // TODO 保存玩家数据
        public void SaveCharacter(DBCharacter dBCharacter)
        {
            if(dBCharacter == null)
            {
                return;
            }
            SetUserData(dBCharacter.UserData);
        }
        // 创建账号
        public void CreateAccount(string account, string psw, string name)
        {
            string sql = $"insert into Character(account, password, name) values('{account}', '{psw}', '{name}')";
            ExecuteSql(sql);
        }

        // 保存消息
        public void SaveMsg(UserChatLog msg)
        {
            string sql = $"insert into Message(send_account, receive_account, msg, date) values('{msg.Send_account}','{msg.Receive_account}','{msg.Msg}',{msg.Date})";
            ExecuteSql(sql);
        }

        // 保存用户头像
        public void SaveUserImg(string userImg, string account)
        {
            string sql = $"update Character set user_img = '{userImg}' where account = '{account}'";
            ExecuteSql(sql);
        }

        // 保存用户名称
        public void SaveUserName(string userName, string account)
        {
            string sql = $"update Character set name = '{userName}' where account = '{account}'";
            ExecuteSql(sql);
        }

        // 添加好友
        public void SaveFriend(string account1, string account2)
        {
            string sql = $"insert into Friend(friend_account1, friend_account2) values('{account1}','{account2}')";
            string sql2 = $"insert into Friend(friend_account1, friend_account2) values('{account2}','{account1}')";
            ExecuteSql(sql);
            ExecuteSql(sql2);
        }

        // 按名称查询
        public List<PBMsgFriendInfo> SelectUserByName(string name)
        {
            string sql = $"select * from Character where name like '%{name}%'";
            List<PBMsgFriendInfo> list = new List<PBMsgFriendInfo>();
            try
            {
                using (connecObj = new SqlConnection(connecStr))
                {
                    connecObj.Open();
                    SqlCommand querCom = new SqlCommand(sql, connecObj);
                    using (SqlDataReader reader = querCom.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PBMsgFriendInfo info = new PBMsgFriendInfo();
                            if (reader[0].GetType() != typeof(DBNull))
                            {
                                info.Account = reader.GetString(0);
                            }
                            if (reader[4].GetType() != typeof(DBNull))
                            {
                                info.Name = reader.GetString(4);
                            }
                            if (reader[5].GetType() != typeof(DBNull))
                            {
                                info.UserImg = reader.GetString(5);
                            }
                            list.Add(info);
                        }
                    }
                }
                connecObj.Close();
            }
            catch (Exception e)
            {
                LogNoteManager.Instance.Log("DBTool: line:126 error" + e);
            }
            return list;
        }
        // 按账号查询
        public List<PBMsgFriendInfo> SelectUserByAccount(string account)
        {
            string sql = $"select * from Character where account like '%{account}%'";
            List<PBMsgFriendInfo> list = new List<PBMsgFriendInfo>();
            try
            {
                using (connecObj = new SqlConnection(connecStr))
                {
                    connecObj.Open();
                    SqlCommand querCom = new SqlCommand(sql, connecObj);
                    using (SqlDataReader reader = querCom.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PBMsgFriendInfo info = new PBMsgFriendInfo();
                            if(reader[0].GetType() != typeof(DBNull))
                            {
                                info.Account = reader.GetString(0);
                            }
                            if (reader[4].GetType() != typeof(DBNull))
                            {
                                info.Name = reader.GetString(4);
                            }
                            if (reader[5].GetType() != typeof(DBNull))
                            {
                                info.UserImg = reader.GetString(5);
                            }
                            list.Add(info);
                        }
                    }
                }
                connecObj.Close();
            }
            catch (Exception e)
            {
                LogNoteManager.Instance.Log("DBTool: line:126 error" + e);
            }
            return list;
        }

        // 获得排名前十列表
        public List<PBMsgGameData> SelectRankTenListByKillNum()
        {
            List<PBMsgGameData> list = new List<PBMsgGameData>();
            string sql = $"select SUM(kill_num), account from GameRecordInfo group by account order by SUM(kill_num) desc";
            try
            {
                using (connecObj = new SqlConnection(connecStr))
                {
                    connecObj.Open();
                    SqlCommand querCom = new SqlCommand(sql, connecObj);
                    using (SqlDataReader reader = querCom.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PBMsgGameData dbGame = new PBMsgGameData();
                            if (reader[0].GetType() != typeof(DBNull))
                            {
                                dbGame.KillNum = Convert.ToInt32(reader[0]);
                            }
                            if (reader[1].GetType() != typeof(DBNull))
                            {
                                dbGame.Account = reader[1] as string;
                            }
                            list.Add(dbGame);
                        }
                    }
                }
                connecObj.Close();
            }
            catch (Exception e)
            {
                LogNoteManager.Instance.Log("DBTool: line:126 error" + e);
            }
            foreach (var item in list)
            {
                DBUserCommonData data = GetUserCommonData(item.Account);
                if(data != null)
                {
                    item.Name = data.Name;
                }
            }
            return list;
        }

        // 执行sql语句
        private void ExecuteSql(string sql)
        {
            try
            {
                using (connecObj = new SqlConnection(connecStr))
                {
                    connecObj.Open();
                    SqlCommand querCom = new SqlCommand(sql, connecObj);
                    querCom.ExecuteNonQuery();
                }
                connecObj.Close();
            }
            catch (Exception e)
            {
                LogNoteManager.Instance.Log("DBTool: line:126 error" + e);
            }
        }

        // 查询是否存在某个账号
        public bool isExisitAccount(string account)
        {
            if(account == null)
            {
                return false;
            }
            string queryStr = $"select * from Character where account = '{account}'";
            using (connecObj = new SqlConnection(connecStr))
            {
                connecObj.Open();
                SqlCommand querCom = new SqlCommand(queryStr, connecObj);
                using (SqlDataReader reader = querCom.ExecuteReader())
                {
                    if (reader.HasRows == false)
                    {
                        return false;
                    }
                }
                connecObj.Close();
            }
            return true;
        }

        // 查询是否存在某个名称
        public bool isExisitName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            string queryStr = $"select * from Character where name = '{name}'";
            using (connecObj = new SqlConnection(connecStr))
            {
                connecObj.Open();
                SqlCommand querCom = new SqlCommand(queryStr, connecObj);
                using (SqlDataReader reader = querCom.ExecuteReader())
                {
                    if (reader.HasRows == false)
                    {
                        return false;
                    }
                }
                connecObj.Close();
            }
            return true;
        }

        // 查询最大的roomid
        public long GetMaxRoomID()
        {
            string queryStr = $"select MAX(id) from GameRecord";
            long result = 0;
            using (connecObj = new SqlConnection(connecStr))
            {
                connecObj.Open();
                SqlCommand querCom = new SqlCommand(queryStr, connecObj);
                using (SqlDataReader reader = querCom.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if(reader[0].GetType() != typeof(DBNull))
                        {
                            result = reader.GetInt64(0);
                        }
                    }
                }
                connecObj.Close();
            }
            return result;
        }

    }
}
