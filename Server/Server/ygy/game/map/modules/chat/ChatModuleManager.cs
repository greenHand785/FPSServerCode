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
        }

        public void ReleaseMessage(MessagesMangerSystem manager)
        {
            manager.ReleaseMessage(MSGID.MSGID_CHAT_SEND_REQUEST);
            manager.ReleaseMessage(MSGID.MSGID_CHAT_SEND_FRIEND_REQUEST);
            manager.ReleaseMessage(MSGID.MSGID_CHAT_FRIEND_DATA_REQUEST);
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
                client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatSendResponse);
                return;
            }
            string receiveAccount = pBMsgChatSendRequest.ReceiveAccount;
            string msg = pBMsgChatSendRequest.Msg;
            string date = pBMsgChatSendRequest.Date;
            if(LoginModuleManager.Instance.ClientDic == null)
            {
                pBMsgChatSendResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatSendResponse);
                return;
            }
            if (DBTool.Instance.isExisitAccount(receiveAccount) == true)
            {
                UserChatLog userChatLog = new UserChatLog(receiveAccount, msg, date);
                if(ch.User_chat_log == null)
                {
                    pBMsgChatSendResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                    client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatSendResponse);
                    return;
                }
                ch.User_chat_log.Add(userChatLog);
            }
            ClientPeer receiveClient = LoginModuleManager.Instance.ClientDic[receiveAccount];
            if(receiveClient == null)
            {
                pBMsgChatSendResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatSendResponse);
                return;
            }
            PBMsgChatSendNotify pBMsgChatSendNotify = new PBMsgChatSendNotify();
            pBMsgChatSendNotify.SendAccount = ch.GetAccount();
            pBMsgChatSendNotify.Msg = msg;
            pBMsgChatSendNotify.Date = date;
            // 向指定好友发送消息通知
            receiveClient.Send(MSGID.MSGID_CHAT_SEND_NOTIFY, pBMsgChatSendNotify);
            pBMsgChatSendResponse.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatSendResponse);
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
                client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatSendFriendResponse);
                return;
            }
            string receiveAccount = pBMsgChatSendFriendRequest.DirAccount;
            if(DBTool.Instance.isExisitAccount(receiveAccount) == false)
            {
                pBMsgChatSendFriendResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_NO_ACCOUNT;
                client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatSendFriendResponse);
                return;
            }
            PBMsgChatSendFriendNotify pBMsgChatSendFriendNotify = new PBMsgChatSendFriendNotify();
            pBMsgChatSendFriendNotify.RequestAccount = ch.GetAccount();
            if(LoginModuleManager.Instance.ClientDic == null)
            {
                pBMsgChatSendFriendResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatSendFriendResponse);
                return;
            }
            ClientPeer dirClient = LoginModuleManager.Instance.ClientDic[receiveAccount];
            if(dirClient == null)
            {
                pBMsgChatSendFriendResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_ACCOUNT_OFFLINE;
                client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatSendFriendResponse);
                return;
            }
            dirClient.Send(MSGID.MSGID_CHAT_SEND_FRIEND_NOTIFY, pBMsgChatSendFriendNotify);
            pBMsgChatSendFriendResponse.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatSendFriendResponse);
        }

        // 发送好友列表请求
        private void OnFriendDataRequest(ClientPeer client, int msgId, object msgObj)
        {
            PBMsgChatFriendDataRequest pBMsgChatFriendDataRequest = msgObj as PBMsgChatFriendDataRequest;
            PBMsgChatFriendDataResponse pBMsgChatFriendDataResponse = new PBMsgChatFriendDataResponse();
            if (pBMsgChatFriendDataRequest == null)
            {
                pBMsgChatFriendDataResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_CONFIG_ERROR;
                client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatFriendDataResponse);
                return;
            }
            Character ch = client.character;
            if(ch == null)
            {
                pBMsgChatFriendDataResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_NO_CHARACTER;
                client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatFriendDataResponse);
                return;
            }
            if(ch.User_friend == null)
            {
                pBMsgChatFriendDataResponse.ReturnCode = EReturnCodeChat.ERETURNCODECHAT_NO_CHARACTER;
                client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatFriendDataResponse);
                return;
            }
            ch.User_friend.Serialie2PB(pBMsgChatFriendDataResponse.FriendList);
            pBMsgChatFriendDataResponse.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            client.Send(MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE, pBMsgChatFriendDataResponse);
        }

        #endregion
    }
}
