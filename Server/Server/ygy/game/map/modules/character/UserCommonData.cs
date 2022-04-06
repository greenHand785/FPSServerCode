using Server.ygy.game.map.util.common.interfaceDefine;
using Server.Ygy.Game.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.modules.character
{
    public class UserCommonData : ICommonBean
    {
        private string account;
        private string password;
        private string phone_num;
        private string email;
        private string name;
        private string user_img;

        public string Account { get => account; set => account = value; }
        public string Password { get => password; set => password = value; }
        public string Phone_num { get => phone_num; set => phone_num = value; }
        public string Email { get => email; set => email = value; }
        public string Name { get => name; set => name = value; }
        public string User_img { get => user_img; set => user_img = value; }

        

        public UserCommonData()
        {

        }

        public UserCommonData(string account, string password, string phone_num, string email, string name, string user_img)
        {
            Account = account;
            Password = password;
            Phone_num = phone_num;
            Email = email;
            Name = name;
            User_img = user_img;
        }

        // 将数据存入pbMsg中
        public void Serialie2PB(object pbMsg)
        {

        }

        // 从数据库中获取数据
        public void SerialieFromDB(object dbMsg)
        {
            DBUserCommonData dBUserCommonData = dbMsg as DBUserCommonData;
            if(dBUserCommonData == null)
            {
                return;
            }
            account = dBUserCommonData.Account;
            password = dBUserCommonData.Password;
            phone_num = dBUserCommonData.PhoneNum;
            email = dBUserCommonData.Email;
            name = dBUserCommonData.Name;
            user_img = dBUserCommonData.UserImg;
        }

        // 将数据保存到数据库中
        public void Save2DB(object dbMsg)
        {
            if (dbMsg == null)
            {
                dbMsg = new DBUserCommonData();
            }
            DBUserCommonData dBUserCommonData = dbMsg as DBUserCommonData;
            if (dBUserCommonData == null)
            {
                return;
            }
            dBUserCommonData.Account = account;
            dBUserCommonData.Password = password;
            dBUserCommonData.PhoneNum = phone_num;
            dBUserCommonData.Email = email;
            dBUserCommonData.Name = name;
            dBUserCommonData.UserImg = user_img;
        }

    }
}
