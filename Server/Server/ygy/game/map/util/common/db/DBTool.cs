using Server.ygy.game.map.modules.character;
using Server.Ygy.Game.Db;
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
            string queryStr = "";
            // TODO 将玩家数据保存到character对象中并返回
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
                            //user = new User();
                            //user.UserName = reader[0] as string;
                            //user.AccountNum = reader[1] as string;
                            //user.PhoneNum = reader[2] as string;
                            //user.PassWord = reader[3] as string;
                            //user.TitleImageURL = reader[4] as string;
                            //user.Golds = Convert.ToInt64(reader[5]);
                            //user.Diamond = Convert.ToInt64(reader[6]);
                            //user.PositionScore = Convert.ToInt64(reader[7]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogNoteManager.Instance.Log("DBTool: line:68 error" + e);
            }
            return ch;
        }

        // TODO 保存玩家数据, 如果没有数据，则创建一个新的用户
        public void SaveCharacter(DBCharacter dBCharacter)
        {
            if(dBCharacter == null)
            {
                return;
            }

        }

        // 查询是否存在某个账号
        public bool isExisitAccount(string account)
        {
            if(account == null)
            {
                return false;
            }
            string queryStr = "";
            using (connecObj = new SqlConnection(connecStr))
            {
                connecObj.Open();
                SqlCommand querCom = new SqlCommand(queryStr, connecObj);
                using (SqlDataReader reader = querCom.ExecuteReader())
                {
                    if (reader.Read() == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
