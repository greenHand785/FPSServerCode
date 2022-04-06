using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.modules.game
{
    public class EReturnGameInfo
    {
        /// <summary>
        /// Code : -400
        /// 配置错误
        /// </summary>
        public static int ERETURNCODE_CONFIG_ERROR = -400;

        /// <summary>
        /// Code : -401
        /// 空间不足，无法加入
        /// </summary>
        public static int ERETURNCODE_GAMEINFO_NO_SPACE = -401;

        /// <summary>
        /// Code : -402
        /// 数据不存在
        /// </summary>
        public static int ERETURNCODE_GAMEINFO_NO_DATA = -402;
        /// <summary>
        /// Code : -403
        /// 操作不存在
        /// </summary>
        public static int ERETURNCODE_GAMEINFO_NO_OPTION_DATA = -403;
        /// <summary>
        /// Code : -404
        /// 房间不存在
        /// </summary>
        public static int ERETURNCODE_GAMEINFO_NO_ROOM = -404;
        /// <summary>
        /// Code : -405
        /// 有的客户端未准备完成
        /// </summary>
        public static int ERETURNCODE_GAMEINFO_NOT_ALL_PLAYER_READY = -405;
        /// <summary>
        /// Code : -406
        /// 玩家不存在
        /// </summary>
        public static int ERETURNCODE_GAMEINFO_NO_PLAYER = -406;

        /// <summary>
        /// Code : -407
        /// 获得房主地址错误
        /// </summary>
        public static int ERETURNCODE_GAMEINFO_GET_HOST_ERROR = -407;


    }
}
