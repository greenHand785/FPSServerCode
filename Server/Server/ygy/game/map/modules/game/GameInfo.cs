using Server.ygy.game.map.modules.character;
using Server.ygy.game.map.modules.login;
using Server.ygy.game.map.util.common.define;
using Server.ygy.game.map.util.common.eventManager;
using Server.Ygy.Game.Map;
using Server.Ygy.Game.Map.Util.Common.EventManager;
using Server.Ygy.Game.Pb;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.modules.game
{
    public class GameInfo
    {
        public class Player
        {
            public bool isReady; // 是否准备完成
            public string account; // 账号
            public bool isAcceptCurFrameOption; // 是否接受到当前帧数据
            public bool isOnLine; // 是否在线
            public int skillID; // 技能id
            public int packageID; // 背包id
            public bool isConnected2Host; // 是否连接上主机
        }

        private List<Player> playerList; // 玩家列表
        private int capcity; // 最大玩家容量
        private bool isOpen; // 是否开启游戏
        private long curFrameCount; // 当前帧数
        private GameRecordData gameData;
        private int readyTime; // 准备时间 单位：s
        private long runningTimeTicks;
        private int flushFrequent; // 刷新频率  单位： ms/次
        private ConcurrentDictionary<long, List<PBMsgPlayerOption>> optionDic; // 操作集合
        private int leaveTime; // 距离开始游戏预计剩余时间

        public GameInfo(long roomID, int count, int readyTime, int flush)
        {
            flushFrequent = flush;
            this.readyTime = readyTime;
            runningTimeTicks = DateTime.Now.Ticks + readyTime * 10000000;
            gameData = new GameRecordData();
            gameData.Id = roomID;
            capcity = count;
            playerList = new List<Player>();

            EventManager.Instance.RegisterEvent(EventDefine.Event_OffLine, OnPlayerOffLine);
        }
        #region 事件
        // 当玩家离线
        private void OnPlayerOffLine(object[] eve)
        {
            if(eve == null || eve.Length == 0)
            {
                return;
            }
            string account = eve[0] as string;
            if (string.IsNullOrEmpty(account))
            {
                return;
            }
            Player player = GetPlayer(account);
            if(player == null)
            {
                return;
            }
            player.isOnLine = false;
        }
        #endregion
        // 是否释放房间资源
        public bool IsReleace()
        {
            if(isOpen && (playerList.Count <= 0 || IsAllDisconted()))
            {
                return true;
            }
            return false;
        }

        // 添加玩家
        public int AddPlayer(string account)
        {
            if(playerList == null)
            {
                return EReturnGameInfo.ERETURNCODE_CONFIG_ERROR;
            }
            if(playerList.Count >= capcity)
            {
                return EReturnGameInfo.ERETURNCODE_GAMEINFO_NO_SPACE;
            }
            playerList.Add(new Player { account = account, isAcceptCurFrameOption = false, isReady = false, isOnLine = true});
            EventManager.Instance.PushEvent(EventDefine.Event_AddPlayer, true, 0, gameData.Id, account);
            return EReturnCode.ERETURNCODE_SUCCESS;
        }

        // 设置玩家技能id
        public int SetPlayerSkillID(string account, int skillID)
        {
            Player player = GetPlayer(account);
            if(player == null)
            {
                return EReturnGameInfo.ERETURNCODE_GAMEINFO_NO_PLAYER;
            }
            player.skillID = skillID;
            return EReturnCode.ERETURNCODE_SUCCESS;
        }
        // 设置玩家背包id
        public int SetPlayerPackageID(string account, int packageID)
        {
            Player player = GetPlayer(account);
            if (player == null)
            {
                return EReturnGameInfo.ERETURNCODE_GAMEINFO_NO_PLAYER;
            }
            player.packageID = packageID;
            return EReturnCode.ERETURNCODE_SUCCESS;
        }

        // 退出房间
        public void ExitRoom(string account)
        {
            if(playerList == null)
            {
                return;
            }
            if (Contains(account))
            {
                Remove(account);
                EventManager.Instance.PushEvent(EventDefine.Event_PlayerExitRoom, true, 0, gameData.Id, account);
            }
        }

        // 玩家退出游戏
        public int PlayerExitGame(PBMsgGameData pbGameData)
        {
            if(pbGameData == null)
            {
                return EReturnGameInfo.ERETURNCODE_GAMEINFO_NO_DATA;
            }
            GameRecordInfo gameInfo = new GameRecordInfo();
            gameInfo.SerializeFromPB(pbGameData);
            if (Contains(pbGameData.Account))
            {
                Remove(pbGameData.Account);
            }
            EventManager.Instance.PushEvent(EventDefine.Event_PlayerExitGame, true, 0, gameData.Id, pbGameData.Account);
            return EReturnCode.ERETURNCODE_SUCCESS;
        }

        // 设置连接玩家主机状态
        public void SetConnectedHostState(string account, bool state)
        {
            Player player = GetPlayer(account);
            if(player == null)
            {
                return;
            }
            player.isConnected2Host = state;
            if (IsAllPlayerConnected2Host())
            {
                // 所有玩家都连接上，返回游戏正式开始通知
                Send2All(MSGID.MSGID_HALL_GAME_START_NOTIFY, new PBMsgStartGameNotify()
                {
                    InitFrameCount = curFrameCount,
                    FlushTime = flushFrequent,
                    Players = GetAllPlayerInfo()
                });
            }
        }

        // 开始游戏
        public int StartGame(string account)
        {
            Player player = GetPlayer(account);
            if (player != null)
            {
                player.isReady = true;
            }
            if (IsAllPlayerReady() == false)
            {
                return EReturnGameInfo.ERETURNCODE_GAMEINFO_NOT_ALL_PLAYER_READY;
            }
            isOpen = true;
            gameData.Start_date = DateTime.Now.Ticks;
            #region 帧同步

            //curFrameCount = 0;
            //// 回复一个0帧给客户端， 客户端发送下一帧操作给服务端
            //Send2All(MSGID.MSGID_HALL_GAME_START_NOTIFY, new PBMsgStartGameNotify()
            //{
            //    InitFrameCount = curFrameCount,
            //    FlushTime = flushFrequent,
            //    Players = GetAllPlayerInfo()
            //});
            //curFrameCount++;

            #endregion

            #region 使用mirror状态同步

            // 从房间内选择一个玩家作为主机，并将主机的信息发送给其他所有客户端，让其他客户端与主机相连。待连接成功后将消息发送给服务端
            // 等待服务端，发送玩家加载完成消息。
            string hostIp = GetRoomHostIPAddress();
            if (hostIp == null)
            {
                return EReturnGameInfo.ERETURNCODE_GAMEINFO_GET_HOST_ERROR;
            }
            if (playerList == null || playerList.Count == 0)
            {
                return EReturnGameInfo.ERETURNCODE_CONFIG_ERROR;
            }
            Send2All(MSGID.MSGID_HALL_ROOM_HOST_IP_NOTIFY, new PBMsgRoomHostIPAddressNotify
            {
                 IpAddress = hostIp,
                 HostAccount = playerList[0].account,
                 Players = GetAllPlayerInfo()
            });
            #endregion
            return EReturnCode.ERETURNCODE_SUCCESS;
        }

        // 是否开始游戏
        public bool IsOpen()
        {
            return isOpen;
        }

        // 是否可以加入
        public bool IsCouldComeIn()
        {
            return leaveTime > 0;
        }

        // 获得当前人数
        public int GetCurPlayerCount()
        {
            if(playerList == null)
            {
                return 0;
            }
            return playerList.Count;
        }

        // 获得所有玩家基本信息
        public List<PBMsgPlayerInfo> GetAllPlayerInfo()
        {
            if(playerList == null)
            {
                return null;
            }
            List<PBMsgPlayerInfo> list = new List<PBMsgPlayerInfo>();
            foreach (var item in playerList)
            {
                PBMsgPlayerInfo info = new PBMsgPlayerInfo();
                info.Account = item.account;
                info.PackageId = item.packageID;
                info.SkillId = item.skillID;
                info.Name = LoginModuleManager.Instance.GetPlayerName(item.account);
                list.Add(info);
            }
            return list;
        }

        // 获得房间id
        public long GetRoomID()
        {
            if(gameData == null)
            {
                return -1;
            }
            return gameData.Id;
        }

        // 更新 60ms/frame
        public void Update()
        {
            ReadyState();
          //  RunningGameState();
        }

        // 准备阶段
        private void ReadyState()
        {
            long nowTicks = DateTime.Now.Ticks;
            if (isOpen || nowTicks >= runningTimeTicks)
            {
                // 进入游戏阶段
                return;
            }
            leaveTime = Convert.ToInt32((runningTimeTicks - nowTicks) / 10000000);
            // 发送剩余时间到客户端
            Send2All(MSGID.MSGID_HALL_START_MATCHING_NOTIFY, new PBMsgStartMatchingNotify { IsOpen = isOpen,
                LeaveTime = leaveTime,
                RoomPlayerCount = playerList.Count});
        }

        // 运行游戏阶段
        private void RunningGameState()
        {
            if (!isOpen)
            {
                return;
            }
            if(IsAcceptAllClientOption(curFrameCount) == true) 
            {
                // 当接受到所有客户端的操作
                // 将所有人的操作, 下一帧数 转发给客户端
                lock (optionDic)
                {
                    Send2All(MSGID.MSGID_HALL_GAME_RUNNING_NOTIFY, new PBMsgGameRunningNotify
                    {
                        CurrentFrameCount = curFrameCount,
                        Options = GetOptions(curFrameCount),
                        FlushTime = flushFrequent,
                    });
                    // 初始化接受状态
                    InitPlayerListState();
                    // 进入下一帧
                    curFrameCount++;
                }
            }
        }

        // 发送消息给房间内所有玩家
        private void Send2All(MSGID msg, object pbMsg)
        {
            if(pbMsg == null)
            {
                return;
            }
            if(playerList == null)
            {
                return;
            }
            foreach (var player in playerList)
            {
                if (player.isOnLine)
                {
                    int ret = LoginModuleManager.Instance.SendMessage(player.account, msg, pbMsg);
                }
            }
        }

        // 获得房间内主机的ip地址
        private string GetRoomHostIPAddress()
        {
            if(playerList == null || playerList.Count == 0)
            {
                return null;
            }
            if(playerList[0] == null)
            {
                return null;
            }
            return GetPlayerAddress(playerList[0].account);
        }

        // 根据账号，获得对应客户端地址
        private string GetPlayerAddress(string account)
        {
            Player player = GetPlayer(account);
            if(player == null)
            {
                return null;
            }
            if(LoginModuleManager.Instance.ClientDic == null)
            {
                return null;
            }
            LoginModuleManager.Instance.ClientDic.TryGetValue(account, out ClientPeer client);
            if(client == null || client.ClientSocket == null)
            {
                return null;
            }
            return client.ClientSocket.RemoteEndPoint.ToString();
        }

        // 添加操作指令
        public int AddPlayerOption(PBMsgPlayerOption option)
        {
            if (optionDic == null)
            {
                optionDic = new ConcurrentDictionary<long, List<PBMsgPlayerOption>>();
            }
            lock (optionDic)
            {
                if (option == null)
                {
                    return EReturnGameInfo.ERETURNCODE_GAMEINFO_NO_OPTION_DATA;
                }
                optionDic.TryGetValue(option.FrameCount, out List<PBMsgPlayerOption> options);
                if (options == null)
                {
                    options = new List<PBMsgPlayerOption>();
                    optionDic.TryAdd(option.FrameCount, options);
                }
                AddPlayerCommand(option, options);
                Player player = GetPlayer(option.Account);
                if (player != null)
                {
                    player.isAcceptCurFrameOption = true;
                }
                return EReturnCode.ERETURNCODE_SUCCESS;
            }
        }

        // 当前帧操作列表中，账号与给定账号相同的操作
        private void AddPlayerCommand(PBMsgPlayerOption newOption, List<PBMsgPlayerOption> options)
        {
            if(newOption == null || newOption.PlayerOption == null)
            {
                return;
            }
            if(options == null)
            {
                return;
            }
            PBMsgPlayerOption old = null;
            foreach (var item in options)
            {
                if (item.Account == newOption.Account)
                {
                    old = item;
                    break;
                }
            }
            if(old == null)
            {
                old = new PBMsgPlayerOption();
                old.PlayerOption = new List<PBMsgOption>();
                options.Add(old);
            }
            old.Account = newOption.Account;
            old.FrameCount = newOption.FrameCount;
            foreach (var item in newOption.PlayerOption)
            {
                old.PlayerOption.Add(item);
            }
        }

        // 是否接受到所有客户端的操作
        private bool IsAcceptAllClientOption(long frame)
        {
            #region 方法一
            //if(optionDic == null)
            //{
            //    return false;
            //}
            //optionDic.TryGetValue(frame, out List<PBMsgPlayerOption> options);
            //if(options == null)
            //{
            //    return false;
            //}
            //if(options.Count != playerList.Count)
            //{
            //    return false;
            //}
            //foreach (var account in playerList)
            //{
            //    foreach (var option in options)
            //    {
            //        if(account.account == option.Account)
            //        {
            //            continue;
            //        }
            //    }
            //    return false;
            //}
            //return true;
            #endregion

            #region 方法二
            if(playerList == null)
            {
                return false;
            }
            foreach (var item in playerList)
            {
                if(item.isOnLine == false)
                {
                    continue;
                }
                if(item.isAcceptCurFrameOption == false)
                {
                    return false;
                }
            }
            return true;
            #endregion

        }

        // 获得帧，所有玩家操作
        private List<PBMsgPlayerOption> GetOptions(long frame)
        {
            if(optionDic == null)
            {
                return null;
            }
            optionDic.TryGetValue(frame, out List<PBMsgPlayerOption> options);
            return options;
        }

        // 是否包含该玩家
        private bool Contains(string account)
        {
            if(playerList == null)
            {
                return false;
            }
            foreach (var item in playerList)
            {
                if(item.account == account)
                {
                    return true;
                }
            }
            return false;
        }
        // 移除该玩家
        private void Remove(string account)
        {
            if (playerList == null)
            {
                return;
            }
            for (int i = playerList.Count - 1; i >= 0; i--)
            {
                if(playerList[i].account == account)
                {
                    playerList.RemoveAt(i);
                }
            }
        }
        // 获得玩家账号列表
        private List<string> GetPlayerAccountList()
        {
            if(playerList == null)
            {
                return null;
            }
            List<string> list = new List<string>();
            foreach (var item in playerList)
            {
                list.Add(item.account);
            }
            return list;
        }
        // 初始化玩家列表状态
        private void InitPlayerListState()
        {
            if(playerList == null)
            {
                return;
            }
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].isAcceptCurFrameOption = false;
            }
        }

        private Player GetPlayer(string account)
        {
            if(playerList == null)
            {
                return null;
            }
            foreach (var item in playerList)
            {
                if(item.account == account)
                {
                    return item;
                }
            }
            return null;
        }
        // 是否所有玩家掉线
        private bool IsAllDisconted()
        {
            if (playerList == null)
            {
                return true;
            }
            foreach (var item in playerList)
            {
                if (item.isOnLine)
                {
                    return false;
                }
            }
            return true;
        }

        // 是否所有玩家准备完成
        private bool IsAllPlayerReady()
        {
            if(playerList == null)
            {
                return false;
            }
            foreach (var item in playerList)
            {
                if (item.isReady == false)
                {
                    return false;
                }
            }
            return true;
        }

        // 检测是否所有玩家都连接上主机
        private bool IsAllPlayerConnected2Host()
        {
            if(playerList == null)
            {
                return false;
            }
            foreach (var item in playerList)
            {
                if(item.isConnected2Host == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
