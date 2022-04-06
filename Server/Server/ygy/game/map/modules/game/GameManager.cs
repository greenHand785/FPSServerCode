using Server.ygy.game.map.util.common.db;
using Server.ygy.game.map.util.common.define;
using Server.Ygy.Game.Pb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.modules.game
{
    public class GameManager
    {
        private static GameManager instance;
        private static object obj = new object();
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        if (instance == null)
                        {
                            instance = new GameManager();
                        }
                    }
                }
                return instance;
            }
        }
        private GameManager()
        {
        }
        private Dictionary<long, GameInfo> games; // 所有对局列表
        private List<GameInfo> gameList; // 游戏对局
        private int flushFrequent = 60; // 刷新频率 ms/次
        private long curMaxRoomID; // 当前房间id

        // 创建一局游戏
        private void CreateNewGame(int maxPlayer)
        {
            if(games == null)
            {
                games = new Dictionary<long, GameInfo>();
            }
            if(gameList == null)
            {
                gameList = new List<GameInfo>();
            }
            if(curMaxRoomID == 0)
            {
                curMaxRoomID = DBTool.Instance.GetMaxRoomID();
            }
            curMaxRoomID++;
            GameInfo game = new GameInfo(curMaxRoomID, maxPlayer, 10, flushFrequent);
            games.Add(curMaxRoomID, game);
            gameList.Add(game);
        }

        // 销毁一局游戏
        private void DestoryGame(long roomID)
        {
            if(games == null)
            {
                return;
            }
            if(gameList == null)
            {
                return;
            }
            games.TryGetValue(roomID, out GameInfo gameInfo);
            if(gameInfo == null)
            {
                return;
            }
            if (gameInfo.IsReleace())
            {
                games.Remove(roomID);
                gameList.Remove(gameInfo);
            }
        }

        // 返回加入的游戏room_id，返回-1表示失败
        public long AddPlayer(string account)
        {
            if(games == null)
            {
                games = new Dictionary<long, GameInfo>();
            }
            foreach (var item in games)
            {
                if(item.Value != null)
                {
                    if (item.Value.IsCouldComeIn())
                    {
                        item.Value.AddPlayer(account);
                        return item.Key;
                    }
                }
            }
            CreateNewGame(20);
            foreach (var item in games)
            {
                if (item.Value != null)
                {
                    if (item.Value.IsCouldComeIn())
                    {
                        item.Value.AddPlayer(account);
                        return item.Key;
                    }
                }
            }
            return -1;
        }

        // 退出房间
        public int ExitRoom(long roomID, string account)
        {
            GameInfo info = GetGameInfo(roomID);
            if(info == null)
            {
                return EReturnGameInfo.ERETURNCODE_GAMEINFO_NO_ROOM;
            }
            info.ExitRoom(account);
            return EReturnCode.ERETURNCODE_SUCCESS;
        }

        // 添加玩家操作
        public int AddPlayerOption(long roomID, PBMsgPlayerOption option)
        {
            GameInfo info = GetGameInfo(roomID);
            if(info == null || option == null)
            {
                return EReturnGameInfo.ERETURNCODE_GAMEINFO_NO_DATA;
            }
            return info.AddPlayerOption(option);
        }

        // 玩家请求选择背包
        public int ChosePackage(long roomID, string account, int packageID)
        {
            GameInfo info = GetGameInfo(roomID);
            if(info == null)
            {
                return EReturnGameInfo.ERETURNCODE_GAMEINFO_NO_DATA;
            }
            return info.SetPlayerPackageID(account, packageID);
        }

        // 玩家请求选择技能
        public int ChoseSkill(long roomID, string account, int skillID)
        {
            GameInfo info = GetGameInfo(roomID);
            if (info == null)
            {
                return EReturnGameInfo.ERETURNCODE_GAMEINFO_NO_DATA;
            }
            return info.SetPlayerSkillID(account, skillID);
        }

        // 退出游戏
        public int ExitGame(long roomID, PBMsgGameData gameData)
        {
            GameInfo info = GetGameInfo(roomID);
            if(info == null)
            {
                return EReturnGameInfo.ERETURNCODE_GAMEINFO_NO_ROOM;
            }
            return info.PlayerExitGame(gameData);
        }

        // 开始游戏
        public int StartGame(long roomID, string account)
        {
            GameInfo info = GetGameInfo(roomID);
            if(info == null)
            {
                return EReturnGameInfo.ERETURNCODE_GAMEINFO_NO_ROOM;
            }
            return info.StartGame(account);
        }
        
        // 获得房间内的玩家信息
        public List<PBMsgPlayerInfo> GetGamePlayers(long id)
        {
            GameInfo info = GetGameInfo(id);
            if(info == null)
            {
                return null;
            }
            return info.GetAllPlayerInfo();
        }

        // 获得房间信息
        public GameInfo GetGameInfo(long id)
        {
            if(games == null)
            {
                return null;
            }
            games.TryGetValue(id, out GameInfo info);
            return info;
        }

        public void Update()
        {
            if(games == null)
            {
                return;
            }
            if(games == null)
            {
                return;
            }
            if(gameList == null)
            {
                return;
            }
            for (int i = gameList.Count - 1; i >= 0; i--)
            {
                if (gameList[i].IsReleace())
                {
                    DestoryGame(gameList[i].GetRoomID());
                    continue;
                }
                gameList[i].Update();
            }
        }
    }
}
