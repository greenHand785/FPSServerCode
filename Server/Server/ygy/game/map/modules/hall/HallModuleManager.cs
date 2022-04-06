using Server.ygy.game.map.modules.character;
using Server.ygy.game.map.modules.game;
using Server.ygy.game.map.modules.login;
using Server.ygy.game.map.util.common.db;
using Server.ygy.game.map.util.common.define;
using Server.Ygy.Game.Map;
using Server.Ygy.Game.Pb;
using System;

namespace Server.ygy.game.map.modules.hall
{
    public class HallModuleManager
    {
        private static HallModuleManager instance;
        private static object obj = new object();
        public static HallModuleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        if (instance == null)
                        {
                            instance = new HallModuleManager();
                        }
                    }
                }
                return instance;
            }
        }
        private HallModuleManager()
        {

        }

        internal void RegisterMessage(MessagesMangerSystem messagesMangerSystem)
        {
            messagesMangerSystem.RegisterMessage(MSGID.MSGID_HALL_CHANGE_USER_IMG_REQUEST, OnHallChangeUserImgRequest);
            messagesMangerSystem.RegisterMessage(MSGID.MSGID_HALL_USER_DATA_REQUEST, OnHallUserDataRequest);
            messagesMangerSystem.RegisterMessage(MSGID.MSGID_HALL_USER_GAME_RECORD_REQUEST, OnHallUserGameRecordRequest);
            messagesMangerSystem.RegisterMessage(MSGID.MSGID_HALL_GET_ALL_RANK_LIST_REQUEST, OnHallGetAllRankListRequest);

            messagesMangerSystem.RegisterMessage(MSGID.MSGID_HALL_START_MATCHING_REQUEST, OnHallStartMatchingRequest);
            messagesMangerSystem.RegisterMessage(MSGID.MSGID_HALL_CANCEL_MATCHING_REQUEST, OnHallCancelMatchingRequest);
            messagesMangerSystem.RegisterMessage(MSGID.MSGID_HALL_CHANGE_USER_NAME_REQUEST, OnHallChangeUserNameRequest);

            messagesMangerSystem.RegisterMessage(MSGID.MSGID_HALL_GAME_START_REQUEST, OnHallStartGameRequest);
            messagesMangerSystem.RegisterMessage(MSGID.MSGID_HALL_GAME_RUNNING_REQUEST, OnGameRunningRequest);
            messagesMangerSystem.RegisterMessage(MSGID.MSGID_HALL_EXIT_GAME_REQUEST, OnExitGameRequest);
            messagesMangerSystem.RegisterMessage(MSGID.MSGID_HALL_CHOSE_PACKAGE_REQUEST, OnChosePackageRequest);
            messagesMangerSystem.RegisterMessage(MSGID.MSGID_HALL_CHOSE_SKILL_REQUEST, OnChoseSkillRequest);

            messagesMangerSystem.RegisterMessage(MSGID.MSGID_HALL_HOST_CONNECTED_REPORT, OnHallHostConnectedReport);
        }

        internal void ReleaseMessage(MessagesMangerSystem messagesMangerSystem)
        {
            messagesMangerSystem.ReleaseMessage(MSGID.MSGID_HALL_CHANGE_USER_IMG_REQUEST);
            messagesMangerSystem.ReleaseMessage(MSGID.MSGID_HALL_USER_DATA_REQUEST);
            messagesMangerSystem.ReleaseMessage(MSGID.MSGID_HALL_USER_GAME_RECORD_REQUEST);
        }

        // 用户对局数据请求
        private void OnHallUserGameRecordRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgUserGameRecordResponse response = new PBMsgUserGameRecordResponse();
            PBMsgUserGameRecordRequest request = pbMsgObj as PBMsgUserGameRecordRequest;
            Character ch = client.character;
            if (client == null || ch == null)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_CONFIG_ERROR;
                client.Send(MSGID.MSGID_HALL_USER_GAME_RECORD_RESPONSE, response);
                return;
            }
            ch.Serealize2PBMsgGameRecord(response);
            response.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            client.Send(MSGID.MSGID_HALL_USER_GAME_RECORD_RESPONSE, response);
        }
        // 用户数据请求
        private void OnHallUserDataRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgUserDataResponse response = new PBMsgUserDataResponse();
            Character ch = client.character;
            if (client == null || ch == null)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_CONFIG_ERROR;
                client.Send(MSGID.MSGID_HALL_USER_DATA_RESPONSE, response);
                return;
            }
            ch.Serealize2PBMsgUserData(response);
            response.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            client.Send(MSGID.MSGID_HALL_USER_DATA_RESPONSE, response);
        }
        // 请求更换头像
        private void OnHallChangeUserImgRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgChangeUserImgResponse response = new PBMsgChangeUserImgResponse();
            PBMsgChangeUserImgRequest request = pbMsgObj as PBMsgChangeUserImgRequest;
            Character ch = client.character;
            if (client == null || ch == null || request == null)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_CONFIG_ERROR;
                client.Send(MSGID.MSGID_HALL_CHANGE_USER_IMG_RESPONSE, response);
                return;
            }
            string imgPath = request.NewUserImg;
            ch.SetUserImg(imgPath);
            response.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            response.NewUserImg = imgPath;
            client.Send(MSGID.MSGID_HALL_CHANGE_USER_IMG_RESPONSE, response);
        }

        // 请求全网排名前十列表
        private void OnHallGetAllRankListRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgGetAllRankResponse response = new PBMsgGetAllRankResponse();
            Character ch = client.character;
            if (client == null || ch == null)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_CONFIG_ERROR;
                client.Send(MSGID.MSGID_HALL_GET_ALL_RANK_LIST_RESPONSE, response);
                return;
            }
            ch.Serealize2PBMsgRankList(response.Info);
            response.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            client.Send(MSGID.MSGID_HALL_GET_ALL_RANK_LIST_RESPONSE, response);
        }

        // 开始匹配请求
        private void OnHallStartMatchingRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgStartMatchingRequest request = pbMsgObj as PBMsgStartMatchingRequest;
            PBMsgStartMatchingResponse response = new PBMsgStartMatchingResponse();
            if (request == null)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_CONFIG_ERROR;
                client.Send(MSGID.MSGID_HALL_START_MATCHING_RESPONSE, response);
                return;
            }
            if (LoginModuleManager.Instance.IsOnLine(request.Account) == false)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_PLAYER_NOT_ONLINE;
                client.Send(MSGID.MSGID_HALL_START_MATCHING_RESPONSE, response);
                return;
            }
            long roomID = GameManager.Instance.AddPlayer(request.Account);
            if (roomID == -1)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_PLAYER_ADD_ROOM_ERROR;
                client.Send(MSGID.MSGID_HALL_START_MATCHING_RESPONSE, response);
                return;
            }
            GameInfo gameInfo = GameManager.Instance.GetGameInfo(roomID);
            if (gameInfo == null)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_PLAYER_GET_ROOM_INFO_ERROR;
                client.Send(MSGID.MSGID_HALL_START_MATCHING_RESPONSE, response);
                return;
            }
            response.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            response.RoomId = roomID;
            response.RoomPlayerCount = gameInfo.GetCurPlayerCount();
            client.Send(MSGID.MSGID_HALL_START_MATCHING_RESPONSE, response);
        }

        //取消匹配请求
        private void OnHallCancelMatchingRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgCancelMatchingRequest request = pbMsgObj as PBMsgCancelMatchingRequest;
            PBMsgCancelMatchingResponse response = new PBMsgCancelMatchingResponse();
            if (request == null)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_CONFIG_ERROR;
                client.Send(MSGID.MSGID_HALL_CANCEL_MATCHING_RESPONSE, response);
                return;
            }
            int ret = GameManager.Instance.ExitRoom(request.RoomId, request.Account);
            response.ReturnCode = ret;
            client.Send(MSGID.MSGID_HALL_CANCEL_MATCHING_RESPONSE, response);
        }

        // 修改用户名称请求
        private void OnHallChangeUserNameRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgChangeUserNameRequest request = pbMsgObj as PBMsgChangeUserNameRequest;
            PBMsgChangeUserNameResponse response = new PBMsgChangeUserNameResponse();
            if (request == null)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_CONFIG_ERROR;
                client.Send(MSGID.MSGID_HALL_CHANGE_USER_NAME_RESPONSE, response);
                return;
            }
            if (client == null || client.character == null)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_CONFIG_ERROR;
                client.Send(MSGID.MSGID_HALL_CHANGE_USER_NAME_RESPONSE, response);
                return;
            }
            if (string.IsNullOrEmpty(request.Name.Trim()))
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_NO_NULL;
                client.Send(MSGID.MSGID_HALL_CHANGE_USER_NAME_RESPONSE, response);
                return;
            }
            if(request.Name.Trim().Length > 15)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_LENGTH_OVERFLOW;
                client.Send(MSGID.MSGID_HALL_CHANGE_USER_NAME_RESPONSE, response);
                return;
            }
            if (DBTool.Instance.isExisitName(request.Name))
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_NAME_REPEAT;
                client.Send(MSGID.MSGID_HALL_CHANGE_USER_NAME_RESPONSE, response);
                return;
            }

            Character ch = client.character;
            ch.SetUserName(request.Name);
            response.ReturnCode = EReturnCode.ERETURNCODE_SUCCESS;
            response.Name = request.Name;
            client.Send(MSGID.MSGID_HALL_CHANGE_USER_NAME_RESPONSE, response);
        }

        // 退出游戏请求
        private void OnExitGameRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgExitGameRequest request = pbMsgObj as PBMsgExitGameRequest;
            PBMsgExitGameResponse response = new PBMsgExitGameResponse();
            if (request == null)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_CONFIG_ERROR;
                client.Send(MSGID.MSGID_HALL_EXIT_GAME_RESPONSE, response);
                return;
            }
            int ret = GameManager.Instance.ExitGame(request.RoomId, request.GameData);
            response.ReturnCode = ret;
            client.Send(MSGID.MSGID_HALL_EXIT_GAME_RESPONSE, response);
        }

        // 游戏操作转发请求
        private void OnGameRunningRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgGameRunningRequest request = pbMsgObj as PBMsgGameRunningRequest;
            PBMsgGameRunningResponse response = new PBMsgGameRunningResponse();
            if (request == null)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_CONFIG_ERROR;
                client.Send(MSGID.MSGID_HALL_GAME_RUNNING_RESPONSE, response);
                return;
            }
            int ret = GameManager.Instance.AddPlayerOption(request.RoomId, request.Option);
            response.ReturnCode = ret;
            client.Send(MSGID.MSGID_HALL_GAME_RUNNING_RESPONSE, response);
        }

        // 开始游戏请求
        private void OnHallStartGameRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgStartGameRequest request = pbMsgObj as PBMsgStartGameRequest;
            PBMsgStartGameResponse response = new PBMsgStartGameResponse();
            if (request == null)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_CONFIG_ERROR;
                client.Send(MSGID.MSGID_HALL_GAME_START_RESPONSE, response);
                return;
            }
            int ret = GameManager.Instance.StartGame(request.RoomId, request.Account);
            response.ReturnCode = ret;
            client.Send(MSGID.MSGID_HALL_GAME_START_RESPONSE, response);
        }

        // 当选择技能请求
        private void OnChoseSkillRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgChoseSkillRequest request = pbMsgObj as PBMsgChoseSkillRequest;
            PBMsgChoseSkillResponse response = new PBMsgChoseSkillResponse();
            if (request == null)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_CONFIG_ERROR;
                client.Send(MSGID.MSGID_HALL_CHOSE_SKILL_RESPONSE, response);
                return;
            }
            int ret = GameManager.Instance.ChoseSkill(request.RoomId, request.Account, request.SkillId);
            response.ReturnCode = ret;
            response.Account = request.Account;
            response.SkillId = request.SkillId;
            client.Send(MSGID.MSGID_HALL_CHOSE_SKILL_RESPONSE, response);
        }
        // 当选择背包请求
        private void OnChosePackageRequest(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgChosePackageRequest request = pbMsgObj as PBMsgChosePackageRequest;
            PBMsgChosePackageResponse response = new PBMsgChosePackageResponse();
            if (request == null)
            {
                response.ReturnCode = EReturnCodeHall.ERETURNCODEHALL_CONFIG_ERROR;
                client.Send(MSGID.MSGID_HALL_CHOSE_PACKAGE_RESPONSE, response);
                return;
            }
            int ret = GameManager.Instance.ChosePackage(request.RoomId, request.Account, request.PackageId);
            response.ReturnCode = ret;
            response.Account = request.Account;
            response.PackageId = request.PackageId;
            client.Send(MSGID.MSGID_HALL_CHOSE_PACKAGE_RESPONSE, response);
        }

        // 客户端连接主机结果上报
        private void OnHallHostConnectedReport(ClientPeer client, int pbMsg, object pbMsgObj)
        {
            PBMsgHostConnectedReport report = pbMsgObj as PBMsgHostConnectedReport;
            if(report == null)
            {
                return;
            }
            GameInfo gameInfo = GameManager.Instance.GetGameInfo(report.RoomId);
            if(gameInfo == null)
            {
                return;
            }
            gameInfo.SetConnectedHostState(report.Account, report.IsConnected);
        }
    }
}