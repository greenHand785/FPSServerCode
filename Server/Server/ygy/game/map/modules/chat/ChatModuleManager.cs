using Server.ygy.game.map.modules.character;
using Server.ygy.game.map.modules.login;
using Server.ygy.game.map.util.common.db;
using Server.ygy.game.map.util.common.define;
using Server.Ygy.Game.Map;
using Server.Ygy.Game.Pb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.modules.chat
{
    public class ChatModuleManager
    {
        private static ChatModuleManager instance;
        private static object obj = new object();
        public static ChatModuleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        if(instance == null)
                        {
                            instance = new ChatModuleManager();
                        }
                    }
                }
                return instance;
            }
        }
        private ChatModuleManager()
        {

        }

        public void RegisterMessage(MessagesMangerSystem manager)
        {
            manager.RegisterMessage(MSGID.MSGID_CHAT_SEND_REQUEST, OnChatSendRequest);
            manager.RegisterMessage(MSGID.MSGID_CHAT_SEND_FRIEND_REQUEST, OnSendAddFriendRequest);
            manager.RegisterMessage(MSGID.MSGID_CHAT_FRIEND_DATA_REQUEST, OnFriendDataRequest);
            manager.RegisterMessage(MSGID.MSGID_CHAT_FRIEND_MSG_LOG_REQUEST, OnFriendMsgLogRequest);
            manager.RegisterMessage(MSGID.MSGID_CHAT_SELF_MSG_LOG_REQUEST, OnSelfMsgLogRequest);
            manager.RegisterMessage(MSGID.MSGID_CHAT_USER_DATA_REQUEST, OnQueryUserDataRequest);
            manager.RegisterMessage(MSGID.MSGID_CHAT_ANSWER_ADD_FRIEND_REQUEST, OnAnswerAddFriendRequest);
        }

        public void ReleaseMessage(MessagesMangerSystem manager)
        {
            manager.ReleaseMessage(MSGID.MSGID_CHAT_SEND_REQUEST);
            manager.ReleaseMessage(MSGID.MSGID_CHAT_SEND_FRIEND_REQUEST);
            manager.ReleaseMessage(MSGID.MSGID_CHAT_FRIEND_DATA_REQUEST);
            manager.ReleaseMessage(MSGID.MSGID_CHAT_FRIEND_MSG_LOG_REQUEST);
            manager.ReleaseMessage(MSGID.MSGID_CHAT_SELF_MSG_LOG_REQUEST);
            manager.ReleaseMessage(MSGID.MSGID_CHAT_USER_DATA_REQUEST);
            manager.ReleaseMessage(MSGID.MSGID_CHAT_ANSWER_ADD_FRIEND_REQUEST);
        }

        #region 消息

        // 发送消息请求
        private void OnChatSendRequest(ClientPeer client, int msgId, object msgObj)
        {
            PBMsgChatSendRequest pBMsgChatSendRequest = msgObj as PBMsgChatSendRequest;
            PBMsgChatSendResponse pBMsgChatSendResponse = new PBMsgChatSendResponse();
            Character ch = client.character; 
            if (pBMsgChatSendRequest == null || ch == null)
            {
                pBMsgChatSendResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_SEND_RESPONSE, pBMsgChatSendResponse);
                return;
            }
            string receiveAccount = pBMsgChatSendRequest.ReceiveAccount;
            string msg = pBMsgChatSendRequest.Msg;
            long date = pBMsgChatSendRequest.Date;
            if(LoginModuleManager.Instance.ClientDic == null)
            {
                pBMsgChatSendResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_SEND_RESPONSE, pBMsgChatSendResponse);
                return;
            }
            if (DBTool.Instance.isExisitAccount(receiveAccount) == true)
            {
                UserChatLog userChatLog = new UserChatLog{ Send_account = ch.GetAccount(), Receive_account = receiveAccount, Msg = msg, Date = date };
                if(ch.GetUserSendChatLog() == null)
                {
                    pBMsgChatSendResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                    client.Send(MSGID.MSGID_CHAT_SEND_RESPONSE, pBMsgChatSendResponse);
                    return;
                }
                ch.AddMessage(userChatLog);
            }
            LoginModuleManager.Instance.ClientDic.TryGetValue(receiveAccount, out ClientPeer receiveClient);
            if(receiveClient == null)
            {
                pBMsgChatSendResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_SEND_RESPONSE, pBMsgChatSendResponse);
                return;
            }
            PBMsgChatSendNotify pBMsgChatSendNotify = new PBMsgChatSendNotify();
            pBMsgChatSendNotify.SendAccount = ch.GetAccount();
            pBMsgChatSendNotify.Msg = msg;
            pBMsgChatSendNotify.Date = date;
            pBMsgChatSendNotify.ReceiveAccount = receiveAccount;
            // 向指定好友发送消息通知
            receiveClient.Send(MSGID.MSGID_CHAT_SEND_NOTIFY, pBMsgChatSendNotify);
            client.Send(MSGID.MSGID_CHAT_SEND_NOTIFY, pBMsgChatSendNotify);
            pBMsgChatSendResponse.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            client.Send(MSGID.MSGID_CHAT_SEND_RESPONSE, pBMsgChatSendResponse);
        }
        // 发送好友添加请求
        private void OnSendAddFriendRequest(ClientPeer client, int msgId, object msgObj)
        {
            PBMsgChatSendFriendRequest pBMsgChatSendFriendRequest = msgObj as PBMsgChatSendFriendRequest;
            PBMsgChatSendFriendResponse pBMsgChatSendFriendResponse = new PBMsgChatSendFriendResponse();
            Character ch = client.character;
            if (pBMsgChatSendFriendRequest == null || ch == null)
            {
                pBMsgChatSendFriendResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_SEND_FRIEND_RESPONSE, pBMsgChatSendFriendResponse);
                return;
            }
            string receiveAccount = pBMsgChatSendFriendRequest.DirAccount;
            if(DBTool.Instance.isExisitAccount(receiveAccount) == false)
            {
                pBMsgChatSendFriendResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_NO_ACCOUNT;
                client.Send(MSGID.MSGID_CHAT_SEND_FRIEND_RESPONSE, pBMsgChatSendFriendResponse);
                return;
            }
            if (ch.IsFriend(receiveAccount))
            {
                pBMsgChatSendFriendResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_ALREADY_FRIEND;
                client.Send(MSGID.MSGID_CHAT_SEND_FRIEND_RESPONSE, pBMsgChatSendFriendResponse);
                return;
            }
            PBMsgChatSendFriendNotify pBMsgChatSendFriendNotify = new PBMsgChatSendFriendNotify();
            pBMsgChatSendFriendNotify.RequestAccount = ch.GetAccount();
            pBMsgChatSendFriendNotify.RequestName = ch.GetName();
            if (LoginModuleManager.Instance.ClientDic == null)
            {
                pBMsgChatSendFriendResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_SEND_FRIEND_RESPONSE, pBMsgChatSendFriendResponse);
                return;
            }
            LoginModuleManager.Instance.ClientDic.TryGetValue(receiveAccount, out ClientPeer dirClient);
            if(dirClient == null)
            {
                pBMsgChatSendFriendResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_ACCOUNT_OFFLINE;
                client.Send(MSGID.MSGID_CHAT_SEND_FRIEND_RESPONSE, pBMsgChatSendFriendResponse);
                return;
            }
            dirClient.Send(MSGID.MSGID_CHAT_SEND_FRIEND_NOTIFY, pBMsgChatSendFriendNotify);
            pBMsgChatSendFriendResponse.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            client.Send(MSGID.MSGID_CHAT_SEND_FRIEND_RESPONSE, pBMsgChatSendFriendResponse);
        }
        // 发送好友列表请求
        private void OnFriendDataRequest(ClientPeer client, int msgId, object msgObj)
        {
            PBMsgUserFriendListResponse pBMsgChatFriendDataResponse = new PBMsgUserFriendListResponse();
            Character ch = client.character;
            if(ch == null)
            {
                pBMsgChatFriendDataResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_NO_CHARACTER;
                client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatFriendDataResponse);
                return;
            }
            if(ch.GetUserFriend() == null)
            {
                pBMsgChatFriendDataResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_NO_CHARACTER;
                client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatFriendDataResponse);
                return;
            }
            pBMsgChatFriendDataResponse.Friends = new List<PBMsgFriendInfo>();
            ch.GetUserFriend().Serialie2PB(pBMsgChatFriendDataResponse.Friends);
            pBMsgChatFriendDataResponse.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatFriendDataResponse);
        }
        // 好友消息记录列表请求
        private void OnFriendMsgLogRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgFriendChatMsgLogResponse response = new PBMsgFriendChatMsgLogResponse();
            if (client == null)
            {
                response.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_FRIEND_MSG_LOG_RESPONSE, response);
                return;
            }
            Character ch = client.character;
            if(ch == null)
            {
                response.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_NO_CHARACTER;
                client.Send(MSGID.MSGID_CHAT_FRIEND_MSG_LOG_RESPONSE, response);
                return;
            }
            response.Msgs = new List<PBMsgFriendChatMsg>();
            ch.Serealize2PBMsgFriend(response.Msgs);
            response.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            client.Send(MSGID.MSGID_CHAT_FRIEND_MSG_LOG_RESPONSE, response);
        }
        //玩家发送给好友的聊天记录请求
        private void OnSelfMsgLogRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgSelfSendChatMsgLogResponse response = new PBMsgSelfSendChatMsgLogResponse();
            if (client == null)
            {
                response.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_SELF_MSG_LOG_RESPONSE, response);
                return;
            }
            Character ch = client.character;
            if (ch == null)
            {
                response.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_NO_CHARACTER;
                client.Send(MSGID.MSGID_CHAT_SELF_MSG_LOG_RESPONSE, response);
                return;
            }
            response.Msgs = new List<PBMsgFriendChatMsg>();
            ch.Serealize2PBMsgSelf(response.Msgs);
            response.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            client.Send(MSGID.MSGID_CHAT_SELF_MSG_LOG_RESPONSE, response);
        }
        //用户数据查询请求
        private void OnQueryUserDataRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgSelectUserDataRequest request = pbMsgObj as PBMsgSelectUserDataRequest;
            PBMsgSelectUserDataResponse response = new PBMsgSelectUserDataResponse();
            if (client == null)
            {
                response.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_USER_DATA_RESPONSE, response);
                return;
            }
            if (request == null)
            {
                response.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_USER_DATA_RESPONSE, response);
                return;
            }
            if(request.Account == null && request.Name == null)
            {
                response.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_USER_DATA_RESPONSE, response);
                return;
            }
            if(request.Account != null && request.Name != null)
            {
                response.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_USER_DATA_RESPONSE, response);
                return;
            }
            if(request.Account == null)
            {
                response.Users = DBTool.Instance.SelectUserByName(request.Name);
                response.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
                client.Send(MSGID.MSGID_CHAT_USER_DATA_RESPONSE, response);
                return;
            }
            response.Users = DBTool.Instance.SelectUserByAccount(request.Account);
            response.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            client.Send(MSGID.MSGID_CHAT_USER_DATA_RESPONSE, response);
        }
        //回应他人的添加好友请求
        private void OnAnswerAddFriendRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgAnswerAddFriendRequest request = pbMsgObj as PBMsgAnswerAddFriendRequest;
            PBMsgAnswerAddFriendResponse response = new PBMsgAnswerAddFriendResponse();
            Character ch = client.character;
            if (request == null || ch == null)
            {
                response.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_ANSWER_ADD_FRIEND_RESPONSE, response);
            }
            if (LoginModuleManager.Instance.ClientDic == null)
            {
                response.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_ANSWER_ADD_FRIEND_RESPONSE, response);
                return;
            }
            if (ch.IsFriend(request.AnswerAccount))
            {
                response.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_ALREADY_FRIEND;
                client.Send(MSGID.MSGID_CHAT_ANSWER_ADD_FRIEND_RESPONSE, response);
                return;
            }
            LoginModuleManager.Instance.ClientDic.TryGetValue(request.AnswerAccount, out ClientPeer dirClient);
            if (dirClient == null)
            {
                response.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_ACCOUNT_OFFLINE;
                client.Send(MSGID.MSGID_CHAT_ANSWER_ADD_FRIEND_RESPONSE, response);
                return;
            }
            response.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            if (request.Result == false)
            {
                client.Send(MSGID.MSGID_CHAT_ANSWER_ADD_FRIEND_RESPONSE, response);
                dirClient.Send(MSGID.MSGID_CHAT_ANSWER_ADD_FRIEND_NOTIFY, new PBMsgAnswerAddFriendNotify());
                return;
            }
            PBMsgAnswerAddFriendNotify notify = new PBMsgAnswerAddFriendNotify();
            notify.AddFriendInfo = new PBMsgFriendInfo { Account = ch.GetAccount(), 
                Name = ch.GetName(), UserImg = ch.GetUserImg() };
            response.FriendInfo = new PBMsgFriendInfo()
            {
                Account = dirClient.character.GetAccount(),
                Name = dirClient.character.GetName(),
                UserImg = dirClient.character.GetUserImg(),
            };
            // 保存好友关系
            ch.AddFriend(response.FriendInfo.Account, response.FriendInfo.Name, response.FriendInfo.UserImg);
            dirClient.Send(MSGID.MSGID_CHAT_ANSWER_ADD_FRIEND_NOTIFY, notify);
            client.Send(MSGID.MSGID_CHAT_ANSWER_ADD_FRIEND_RESPONSE, response);
        }
        #endregion
    }
}
