using Server.Ygy.Game.Map;
using Server.Ygy.Game.Pb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.util.common.pbCodingTool
{
    public class PBCodingTool
    {
        public delegate T DeserializeTmp<T>(Stream stream);
        public delegate void SerializeTmp<T>(Stream stream, T instance);
        
        // 序列化pb协议
        public static void PBSerialize(MSGID msgId, Stream stream, object value)
        {
            switch (msgId)
            {
                // login
                case MSGID.MSGID_LOGIN_REQUEST:
                    {
                        PBMsgLoginRequest.Serialize(stream, value as PBMsgLoginRequest);
                    }; break;
                case MSGID.MSGID_LOGIN_RESPONSE:
                    {
                        PBMsgLoginResponse.Serialize(stream, value as PBMsgLoginResponse);
                    }; break;
                case MSGID.MSGID_REGISTER_REQUEST:
                    {
                        PBMsgRegisterRequest.Serialize(stream, value as PBMsgRegisterRequest);
                    }; break;
                case MSGID.MSGID_REGISTER_RESPONSE:
                    {
                        PBMsgRegisterResponse.Serialize(stream, value as PBMsgRegisterResponse);
                    }; break;
                case MSGID.MSGID_AFFIRM_LOGIN_REQUEST:
                    {
                        PBMsgAffirmLoginRequest.Serialize(stream, value as PBMsgAffirmLoginRequest);
                    }; break;
                case MSGID.MSGID_AFFIRM_LOGIN_RESPONSE:
                    {
                        PBMsgAffirmLoginResponse.Serialize(stream, value as PBMsgAffirmLoginResponse);
                    }; break;

                // chat
                case MSGID.MSGID_CHAT_SEND_REQUEST:
                    {
                        PBMsgChatSendRequest.Serialize(stream, value as PBMsgChatSendRequest);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_RESPONSE:
                    {
                        PBMsgChatSendResponse.Serialize(stream, value as PBMsgChatSendResponse);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_NOTIFY:
                    {
                        PBMsgChatSendNotify.Serialize(stream, value as PBMsgChatSendNotify);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_FRIEND_REQUEST:
                    {
                        PBMsgChatSendFriendRequest.Serialize(stream, value as PBMsgChatSendFriendRequest);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_FRIEND_RESPONSE:
                    {
                        PBMsgChatSendFriendResponse.Serialize(stream, value as PBMsgChatSendFriendResponse);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_FRIEND_NOTIFY:
                    {
                        PBMsgChatSendFriendNotify.Serialize(stream, value as PBMsgChatSendFriendNotify);
                    }; break;
                case MSGID.MSGID_CHAT_FRIEND_DATA_REQUEST:
                    {
                        PBMsgUserFriendListRequest.Serialize(stream, value as PBMsgUserFriendListRequest);
                    }; break;
                case MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE:
                    {
                        PBMsgUserFriendListResponse.Serialize(stream, value as PBMsgUserFriendListResponse);
                    }; break;

                case MSGID.MSGID_CHAT_FRIEND_MSG_LOG_REQUEST:
                    {
                        PBMsgFriendChatMsgLogRequest.Serialize(stream, value as PBMsgFriendChatMsgLogRequest);
                    }; break;
                case MSGID.MSGID_CHAT_FRIEND_MSG_LOG_RESPONSE:
                    {
                        PBMsgFriendChatMsgLogResponse.Serialize(stream, value as PBMsgFriendChatMsgLogResponse);
                    }; break;
                case MSGID.MSGID_CHAT_SELF_MSG_LOG_REQUEST:
                    {
                        PBMsgSelfSendChatMsgLogRequest.Serialize(stream, value as PBMsgSelfSendChatMsgLogRequest);
                    }; break;
                case MSGID.MSGID_CHAT_SELF_MSG_LOG_RESPONSE:
                    {
                        PBMsgSelfSendChatMsgLogResponse.Serialize(stream, value as PBMsgSelfSendChatMsgLogResponse);
                    }; break;
                case MSGID.MSGID_CHAT_USER_DATA_REQUEST:
                    {
                        PBMsgSelectUserDataRequest.Serialize(stream, value as PBMsgSelectUserDataRequest);
                    }; break;
                case MSGID.MSGID_CHAT_USER_DATA_RESPONSE:
                    {
                        PBMsgSelectUserDataResponse.Serialize(stream, value as PBMsgSelectUserDataResponse);
                    }; break;
                case MSGID.MSGID_CHAT_ANSWER_ADD_FRIEND_REQUEST:
                    {
                        PBMsgAnswerAddFriendRequest.Serialize(stream, value as PBMsgAnswerAddFriendRequest);
                    }; break;
                case MSGID.MSGID_CHAT_ANSWER_ADD_FRIEND_RESPONSE:
                    {
                        PBMsgAnswerAddFriendResponse.Serialize(stream, value as PBMsgAnswerAddFriendResponse);
                    }; break;
                case MSGID.MSGID_CHAT_ANSWER_ADD_FRIEND_NOTIFY:
                    {
                        PBMsgAnswerAddFriendNotify.Serialize(stream, value as PBMsgAnswerAddFriendNotify);
                    }; break;

                // hall
                case MSGID.MSGID_HALL_CHANGE_USER_IMG_REQUEST:
                    {
                        PBMsgChangeUserImgRequest.Serialize(stream, value as PBMsgChangeUserImgRequest);
                    }; break;
                case MSGID.MSGID_HALL_CHANGE_USER_IMG_RESPONSE:
                    {
                        PBMsgChangeUserImgResponse.Serialize(stream, value as PBMsgChangeUserImgResponse);
                    }; break;
                case MSGID.MSGID_HALL_USER_DATA_REQUEST:
                    {
                        PBMsgUserDataRequest.Serialize(stream, value as PBMsgUserDataRequest);
                    }; break;
                case MSGID.MSGID_HALL_USER_DATA_RESPONSE:
                    {
                        PBMsgUserDataResponse.Serialize(stream, value as PBMsgUserDataResponse);
                    }; break;
                case MSGID.MSGID_HALL_USER_GAME_RECORD_REQUEST:
                    {
                        PBMsgUserGameRecordRequest.Serialize(stream, value as PBMsgUserGameRecordRequest);
                    }; break;
                case MSGID.MSGID_HALL_USER_GAME_RECORD_RESPONSE:
                    {
                        PBMsgUserGameRecordResponse.Serialize(stream, value as PBMsgUserGameRecordResponse);
                    }; break;

                case MSGID.MSGID_HALL_START_MATCHING_REQUEST:
                    {
                        PBMsgStartMatchingRequest.Serialize(stream, value as PBMsgStartMatchingRequest);
                    }; break;
                case MSGID.MSGID_HALL_START_MATCHING_RESPONSE:
                    {
                        PBMsgStartMatchingResponse.Serialize(stream, value as PBMsgStartMatchingResponse);
                    }; break;
                case MSGID.MSGID_HALL_START_MATCHING_NOTIFY:
                    {
                        PBMsgStartMatchingNotify.Serialize(stream, value as PBMsgStartMatchingNotify);
                    }; break;
                case MSGID.MSGID_HALL_CANCEL_MATCHING_REQUEST:
                    {
                        PBMsgCancelMatchingRequest.Serialize(stream, value as PBMsgCancelMatchingRequest);
                    }; break;
                case MSGID.MSGID_HALL_CANCEL_MATCHING_RESPONSE:
                    {
                        PBMsgCancelMatchingResponse.Serialize(stream, value as PBMsgCancelMatchingResponse);
                    }; break;
                case MSGID.MSGID_HALL_CHANGE_USER_NAME_REQUEST:
                    {
                        PBMsgChangeUserNameRequest.Serialize(stream, value as PBMsgChangeUserNameRequest);
                    }; break;
                case MSGID.MSGID_HALL_CHANGE_USER_NAME_RESPONSE:
                    {
                        PBMsgChangeUserNameResponse.Serialize(stream, value as PBMsgChangeUserNameResponse);
                    }; break;

                case MSGID.MSGID_HALL_GAME_START_REQUEST:
                    {
                        PBMsgStartGameRequest.Serialize(stream, value as PBMsgStartGameRequest);
                    }; break;
                case MSGID.MSGID_HALL_GAME_START_RESPONSE:
                    {
                        PBMsgStartGameResponse.Serialize(stream, value as PBMsgStartGameResponse);
                    }; break;
                case MSGID.MSGID_HALL_GAME_START_NOTIFY:
                    {
                        PBMsgStartGameNotify.Serialize(stream, value as PBMsgStartGameNotify);
                    }; break;
                case MSGID.MSGID_HALL_GAME_RUNNING_NOTIFY:
                    {
                        PBMsgGameRunningNotify.Serialize(stream, value as PBMsgGameRunningNotify);
                    }; break;
                case MSGID.MSGID_HALL_GAME_RUNNING_REQUEST:
                    {
                        PBMsgGameRunningRequest.Serialize(stream, value as PBMsgGameRunningRequest);
                    }; break;
                case MSGID.MSGID_HALL_GAME_RUNNING_RESPONSE:
                    {
                        PBMsgGameRunningResponse.Serialize(stream, value as PBMsgGameRunningResponse);
                    }; break;
                case MSGID.MSGID_HALL_EXIT_GAME_REQUEST:
                    {
                        PBMsgExitGameRequest.Serialize(stream, value as PBMsgExitGameRequest);
                    }; break;
                case MSGID.MSGID_HALL_EXIT_GAME_RESPONSE:
                    {
                        PBMsgExitGameResponse.Serialize(stream, value as PBMsgExitGameResponse);
                    }; break;
                case MSGID.MSGID_HALL_CHOSE_PACKAGE_REQUEST:
                    {
                        PBMsgChosePackageRequest.Serialize(stream, value as PBMsgChosePackageRequest);
                    }; break;
                case MSGID.MSGID_HALL_CHOSE_PACKAGE_RESPONSE:
                    {
                        PBMsgChosePackageResponse.Serialize(stream, value as PBMsgChosePackageResponse);
                    }; break;
                case MSGID.MSGID_HALL_CHOSE_SKILL_REQUEST:
                    {
                        PBMsgChoseSkillRequest.Serialize(stream, value as PBMsgChoseSkillRequest);
                    }; break;
                case MSGID.MSGID_HALL_CHOSE_SKILL_RESPONSE:
                    {
                        PBMsgChoseSkillResponse.Serialize(stream, value as PBMsgChoseSkillResponse);
                    }; break;
                case MSGID.MSGID_HALL_ROOM_HOST_IP_NOTIFY:
                    {
                        PBMsgRoomHostIPAddressNotify.Serialize(stream, value as PBMsgRoomHostIPAddressNotify);
                    }; break;
                case MSGID.MSGID_HALL_HOST_CONNECTED_REPORT:
                    {
                        PBMsgHostConnectedReport.Serialize(stream, value as PBMsgHostConnectedReport);
                    }; break;


            }

        }

        // 反序列化pb协议
        public static object PBDeSerialize(MSGID msgId, Stream stream)
        {
            object value = null;
            switch (msgId)
            {
                // login
                case MSGID.MSGID_LOGIN_REQUEST:
                    {
                        value = PBMsgLoginRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_LOGIN_RESPONSE:
                    {
                        value = PBMsgLoginResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_REGISTER_REQUEST:
                    {
                        value = PBMsgRegisterRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_REGISTER_RESPONSE:
                    {
                        value = PBMsgRegisterResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_AFFIRM_LOGIN_REQUEST:
                    {
                        value = PBMsgAffirmLoginRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_AFFIRM_LOGIN_RESPONSE:
                    {
                        value = PBMsgAffirmLoginResponse.Deserialize(stream);
                    }; break;

                // chat
                case MSGID.MSGID_CHAT_SEND_REQUEST:
                    {
                        value = PBMsgChatSendRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_RESPONSE:
                    {
                        value = PBMsgChatSendResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_NOTIFY:
                    {
                        value = PBMsgChatSendNotify.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_FRIEND_REQUEST:
                    {
                        value = PBMsgChatSendFriendRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_FRIEND_RESPONSE:
                    {
                        value = PBMsgChatSendFriendResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_SEND_FRIEND_NOTIFY:
                    {
                        value = PBMsgChatSendFriendNotify.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_FRIEND_DATA_REQUEST:
                    {
                        value = PBMsgUserFriendListRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_FRIEND_DATA_RESPONSE:
                    {
                        value = PBMsgUserFriendListResponse.Deserialize(stream);
                    }; break;


                case MSGID.MSGID_CHAT_FRIEND_MSG_LOG_REQUEST:
                    {
                        value = PBMsgFriendChatMsgLogRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_FRIEND_MSG_LOG_RESPONSE:
                    {
                        value = PBMsgFriendChatMsgLogResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_SELF_MSG_LOG_REQUEST:
                    {
                        value = PBMsgSelfSendChatMsgLogRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_SELF_MSG_LOG_RESPONSE:
                    {
                        value = PBMsgSelfSendChatMsgLogResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_USER_DATA_REQUEST:
                    {
                        value = PBMsgSelectUserDataRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_USER_DATA_RESPONSE:
                    {
                        value = PBMsgSelectUserDataResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_ANSWER_ADD_FRIEND_REQUEST:
                    {
                        value = PBMsgAnswerAddFriendRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_ANSWER_ADD_FRIEND_RESPONSE:
                    {
                        value = PBMsgAnswerAddFriendResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_CHAT_ANSWER_ADD_FRIEND_NOTIFY:
                    {
                        value = PBMsgAnswerAddFriendNotify.Deserialize(stream);
                    }; break;


                // hall
                case MSGID.MSGID_HALL_CHANGE_USER_IMG_REQUEST:
                    {
                        value = PBMsgChangeUserImgRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_CHANGE_USER_IMG_RESPONSE:
                    {
                        value = PBMsgChangeUserImgResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_USER_DATA_REQUEST:
                    {
                        value = PBMsgUserDataRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_USER_DATA_RESPONSE:
                    {
                        value = PBMsgUserDataResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_USER_GAME_RECORD_REQUEST:
                    {
                        value = PBMsgUserGameRecordRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_USER_GAME_RECORD_RESPONSE:
                    {
                        value = PBMsgUserGameRecordResponse.Deserialize(stream);
                    }; break;

                case MSGID.MSGID_HALL_START_MATCHING_REQUEST:
                    {
                        value = PBMsgStartMatchingRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_START_MATCHING_RESPONSE:
                    {
                        value = PBMsgStartMatchingResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_START_MATCHING_NOTIFY:
                    {
                        value = PBMsgStartMatchingNotify.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_CANCEL_MATCHING_REQUEST:
                    {
                        value = PBMsgCancelMatchingRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_CANCEL_MATCHING_RESPONSE:
                    {
                        value = PBMsgCancelMatchingResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_CHANGE_USER_NAME_REQUEST:
                    {
                        value = PBMsgChangeUserNameRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_CHANGE_USER_NAME_RESPONSE:
                    {
                        value = PBMsgChangeUserNameResponse.Deserialize(stream);
                    }; break;

                case MSGID.MSGID_HALL_GAME_START_REQUEST:
                    {
                        value = PBMsgStartGameRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_GAME_START_RESPONSE:
                    {
                        value = PBMsgStartGameResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_GAME_START_NOTIFY:
                    {
                        value = PBMsgStartGameNotify.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_GAME_RUNNING_NOTIFY:
                    {
                        value = PBMsgGameRunningNotify.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_GAME_RUNNING_REQUEST:
                    {
                        value = PBMsgGameRunningRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_GAME_RUNNING_RESPONSE:
                    {
                        value = PBMsgGameRunningResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_EXIT_GAME_REQUEST:
                    {
                        value = PBMsgExitGameRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_EXIT_GAME_RESPONSE:
                    {
                        value = PBMsgExitGameResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_CHOSE_PACKAGE_REQUEST:
                    {
                        value = PBMsgChosePackageRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_CHOSE_PACKAGE_RESPONSE:
                    {
                        value = PBMsgChosePackageResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_CHOSE_SKILL_REQUEST:
                    {
                        value = PBMsgChoseSkillRequest.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_CHOSE_SKILL_RESPONSE:
                    {
                        value = PBMsgChoseSkillResponse.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_ROOM_HOST_IP_NOTIFY:
                    {
                        value = PBMsgRoomHostIPAddressNotify.Deserialize(stream);
                    }; break;
                case MSGID.MSGID_HALL_HOST_CONNECTED_REPORT:
                    {
                        value = PBMsgHostConnectedReport.Deserialize(stream);
                    }; break;
            }
            return value;
        }
    }
}
