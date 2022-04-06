using Server.ygy.game.map.modules.character;
using Server.ygy.game.map.util.common.db;
using Server.ygy.game.map.util.common.define;
using Server.ygy.game.map.util.common.eventManager;
using Server.Ygy.Game.Db;
using Server.Ygy.Game.Map;
using Server.Ygy.Game.Map.Util.Common.EventManager;
using Server.Ygy.Game.Pb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.modules.login
{
    public class LoginModuleManager
    {
        private static LoginModuleManager instance;
        private static object obj = new object();

        private Dictionary<string, ClientPeer> clientDic;

        public Dictionary<string, ClientPeer> ClientDic
        {
            get
            {
                return clientDic;
            }
        }

        public static LoginModuleManager Instance
        {
            get
            {
                if(instance == null)
                {
                    lock (obj)
                    {
                        if(instance == null)
                        {
                            instance = new LoginModuleManager();
                        }
                    }
                }
                return instance;
            }
        }

        private LoginModuleManager()
        {
            clientDic = new Dictionary<string, ClientPeer>();
        }

        // 向玩家发送消息根据账号
        public int SendMessage(string account, MSGID msg, object pbMsg)
        {
            if(ClientDic == null)
            {
                return EReturnCodeLogin.ERETURNCODELOGIN_NO_USER_DIC;
            }
            clientDic.TryGetValue(account, out ClientPeer client);
            if(client == null)
            {
                return EReturnCodeLogin.ERETURNCODELOGIN_SEND_MESSAGE_ERROR;
            }
            client.Send(msg, pbMsg);
            return EReturnCode.ERETURNCODE_SUCCESS;
        }
        // 判断玩家是否在线
        public bool IsOnLine(string account)
        {
            if(clientDic == null)
            {
                return false;
            }
            if (clientDic.ContainsKey(account))
            {
                return true;
            }
            return false;
        }

        // 获得玩家的名称
        public string GetPlayerName(string account)
        {
            if(ClientDic == null)
            {
                DBUserCommonData data = DBTool.Instance.GetUserCommonData(account);
                if(data == null)
                {
                    return null;
                }
                return data.Name;
            }
            ClientDic.TryGetValue(account, out ClientPeer client);
            if(client == null || client.character == null)
            {
                DBUserCommonData data = DBTool.Instance.GetUserCommonData(account);
                if (data == null)
                {
                    return null;
                }
                return data.Name;
            }
            return client.character.GetAccount();
        }

        // 注册消息
        public void RegisterMessage(MessagesMangerSystem manager)
        {
            //   MessagesMangerSystem.Instance.RegisterMessage()
            manager.RegisterMessage(MSGID.MSGID_LOGIN_REQUEST, OnLoginRequest);
            manager.RegisterMessage(MSGID.MSGID_REGISTER_REQUEST, OnRegisterRequest);
            manager.RegisterMessage(MSGID.MSGID_AFFIRM_LOGIN_REQUEST, OnAffirmLoginRequest);

            // 注册事件
            EventManager.Instance.RegisterAutoEvent(EventDefine.Event_NewDay, OnNewDay);
            EventManager.Instance.RegisterEvent(EventDefine.Event_OffLine, OnOffLine);
            EventManager.Instance.RegisterEvent(EventDefine.Event_Update, Update);
        }

        

        // 移除消息
        public void ReleaseMessage(MessagesMangerSystem manager)
        {
            //MessagesMangerSystem.Instance.ReleaseMessage(MSGID.MSGID_TEST_DATA_NOTIFY);
        }

        // 刷新
        public void Update(object[] param)
        {

        }

        // 新一天时会触发的事件
        private void OnNewDay(ClientPeer client)
        {

        }

        // 下线
        private void OnOffLine(object[] param)
        {
            if(param == null)
            {
                return;
            }
            if(param.Length == 0)
            {
                return;
            }
            string account = param[0] as string;
            if(account == null)
            {
                return;
            }
            if(ClientDic == null)
            {
                return;
            }
            if (ClientDic.ContainsKey(account) == false)
            {
                return;
            }
            if(ClientDic[account] == null)
            {
                return;
            }
            if(ClientDic[account].character == null)
            {
                return;
            }
            DBCharacter dBCharacter = new DBCharacter();
            ClientDic[account].character.Save2DB(dBCharacter);
            DBTool.Instance.SaveCharacter(dBCharacter);
            ClientDic.Remove(account);
        }

        // 登陸回復
        private void OnLoginRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgLoginRequest pBMsgLoginRequest = pbMsgObj as PBMsgLoginRequest;
            PBMsgLoginResponse pBMsgLoginResponse = new PBMsgLoginResponse();
            if (pBMsgLoginRequest == null)
            {
                pBMsgLoginResponse.ReturnCode = EReturnCodeLogin.ERETURNCODELOGIN_REQUEST_ERROR; // 请求数据错误
                client.Send(MSGID.MSGID_LOGIN_RESPONSE, pBMsgLoginResponse);
                return;
            }
            Character character = new Character();
            DBCharacter dBCharacter = DBTool.Instance.GetDBCharacter(pBMsgLoginRequest.Account);
            character.SerialieFromDB(dBCharacter);
            if (character.GetAccount() != pBMsgLoginRequest.Account)
            {
                pBMsgLoginResponse.ReturnCode = EReturnCodeLogin.ERETURNCODELOGIN_NO_ACCOUNT; // 账号不存在
                client.Send(MSGID.MSGID_LOGIN_RESPONSE, pBMsgLoginResponse);
                return;
            }
            if(character.GetPassWord() != pBMsgLoginRequest.UserPassword)
            {
                pBMsgLoginResponse.ReturnCode = EReturnCodeLogin.ERETURNCODELOGIN_PASSWORD_ERROR; // 密码不正确
                client.Send(MSGID.MSGID_LOGIN_RESPONSE, pBMsgLoginResponse);
                return;
            }
            client.character = character;
            if(ClientDic.ContainsKey(pBMsgLoginRequest.Account) == true)
            {
                pBMsgLoginResponse.ReturnCode = EReturnCodeLogin.ERETURNCODELOGIN_ALREALDY_LOGIN; // 账号已被登陆
                pBMsgLoginResponse.Account = pBMsgLoginRequest.Account;
                client.Send(MSGID.MSGID_LOGIN_RESPONSE, pBMsgLoginResponse);
                return;
            }
            clientDic.Add(pBMsgLoginRequest.Account, client);
            // 上线事件
            EventManager.Instance.PushEvent(EventDefine.Event_OnLine, true, 0, pBMsgLoginRequest.Account);
            // 登陸成功
            pBMsgLoginResponse.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            pBMsgLoginResponse.Account = pBMsgLoginRequest.Account;
            pBMsgLoginResponse.Psw = pBMsgLoginRequest.UserPassword;
            client.Send(MSGID.MSGID_LOGIN_RESPONSE, pBMsgLoginResponse);
        }

        // 註冊回復
        private void OnRegisterRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgRegisterRequest pBMsgRegisterRequest = pbMsgObj as PBMsgRegisterRequest;
            PBMsgRegisterResponse pBMsgRegisterResponse = new PBMsgRegisterResponse();
            if (pBMsgRegisterRequest == null)
            {
                pBMsgRegisterResponse.ReturnCode = EReturnCodeLogin.ERETURNCODELOGIN_REQUEST_ERROR;
                client.Send(MSGID.MSGID_REGISTER_RESPONSE, pBMsgRegisterResponse);
                return;
            }
            string account = pBMsgRegisterRequest.Account;
            string psw = pBMsgRegisterRequest.UserPassword;
            int ret = CheckAccountLegal(account);
            if (ret != EReturnCode.ERETURNCODE_SUCCESS)
            {
                pBMsgRegisterResponse.ReturnCode = ret;
                client.Send(MSGID.MSGID_REGISTER_RESPONSE, pBMsgRegisterResponse);
                return;
            }
            int ret2 = CheckPasswordLegal(psw);
            if(ret2 != EReturnCode.ERETURNCODE_SUCCESS)
            {
                pBMsgRegisterResponse.ReturnCode = ret2;
                client.Send(MSGID.MSGID_REGISTER_RESPONSE, pBMsgRegisterResponse);
                return;
            }
            // 创建账号
            string name = GetUnRepeatName(3);
            DBTool.Instance.CreateAccount(account, psw, name);
            pBMsgRegisterResponse.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            client.Send(MSGID.MSGID_REGISTER_RESPONSE, pBMsgRegisterResponse);
        }

        // 确认登陆
        private void OnAffirmLoginRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgAffirmLoginRequest pBMsgAffirmLoginRequest = pbMsgObj as PBMsgAffirmLoginRequest;
            if(pBMsgAffirmLoginRequest == null)
            {
                return;
            }
            ClientPeer oldClient = ClientDic[pBMsgAffirmLoginRequest.Account];
            if(oldClient == null)
            {
                return;
            }
            PBMsgAffirmLoginResponse oldAffirm = new PBMsgAffirmLoginResponse();
            oldAffirm.ReturnCode = EReturnCodeLogin.ERETURNCODELOGIN_CLOSE;
            oldClient.Send(MSGID.MSGID_AFFIRM_LOGIN_RESPONSE, oldAffirm);
            PBMsgAffirmLoginResponse affirm = new PBMsgAffirmLoginResponse();
            affirm.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            client.Send(MSGID.MSGID_AFFIRM_LOGIN_RESPONSE, affirm);

            clientDic[pBMsgAffirmLoginRequest.Account] =  client;
            // 上线事件
            EventManager.Instance.PushEvent(EventDefine.Event_OnLine, true, 0, pBMsgAffirmLoginRequest.Account);
        }

        // 检查密码是否合法， 1. 长度是否在8 - 15个字符之间，2. 以英文字母开头
        private int CheckPasswordLegal(string psw)
        {
            if (psw == null)
            {
                return EReturnCodeLogin.ERETURNCODELOGIN_PASSWORD_ILEGAL;
            }
            if (psw.Length < 8 || psw.Length > 15)
            {
                return EReturnCodeLogin.ERETURNCODELOGIN_LENGTH_ILEGAL;
            }
            // 判断是否存在两种字符
            if(System.Text.RegularExpressions.Regex.IsMatch(psw, @"^[a-zA-Z]{1,}[\d\W]{1,}$") == false)
            {
                return EReturnCodeLogin.ERETURNCODELOGIN_PASSWORD_ILEGAL;
            }
            return EReturnCode.ERETURNCODE_SUCCESS;
        }

        // 检查账号是否合法， 1. 长度是否在8 - 15个字符之间， 2. 账号是否已经注册
        private int CheckAccountLegal(string account)
        {
            if(account == null)
            {
                return EReturnCodeLogin.ERETURNCODELOGIN_NO_ACCOUNT;
            }
            if (account.Length < 8 || account.Length > 15)
            {
                return EReturnCodeLogin.ERETURNCODELOGIN_LENGTH_ILEGAL;
            }
            if(DBTool.Instance.isExisitAccount(account) == true)
            {
                return EReturnCodeLogin.ERETURNCODELOGIN_ACOUNT_EXISIT;
            }
            return EReturnCode.ERETURNCODE_SUCCESS;
        }

        // 获得独一无二的名称
        private string GetUnRepeatName(int length)
        {
            string name = "笑嘻嘻氨基酸的离开垃圾上单买奶茶的爱速度为去沃尔夫就啊啊是大家看";
            int randomIndex = 0;
            string resultName = "";
            Random random = new Random(DateTime.Now.Second);
            while (true)
            {
                randomIndex = random.Next(0, name.Length);
                resultName += name[randomIndex];
                if(resultName.Length >= length)
                {
                    if (DBTool.Instance.isExisitName(resultName))
                    {
                        resultName = "";
                    }
                    else
                    {
                        return resultName;
                    }
                }
            }
        }
    }
}
