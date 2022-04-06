using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ygy.Game.Map.Util.Common.EventManager
{
    public class EventDefine
    {
        #region 自动执行事件 1 - 3000
        public static int Event_NewDay      = 1; // 新一天
        #endregion

        #region 手动执行事件 -1 - -3000
        public static int Event_OnLine      = -1; // 上线
        public static int Event_OffLine     = -2; // 离线  param: string account
        public static int Event_Update      = -3; // 按固定频率更新 param: null
        public static int Event_AddPlayer   = -4; // 房间加入玩家, param: roomID, account
        public static int Event_PlayerExitGame = -5; // 玩家退出游戏, param: roomID, account
        public static int Event_PlayerExitRoom = -6; // 玩家退出房间, param: roomID, account

        #endregion
    }
}
