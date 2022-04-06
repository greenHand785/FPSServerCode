using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.modules.hall
{
    class EReturnCodeHall
    {
        /// <summary>
        /// Code : -300
        /// 配置错误
        /// </summary>
        public static int ERETURNCODEHALL_CONFIG_ERROR = -300;

        /// <summary>
        /// Code : -301
        /// 名称已存在
        /// </summary>
        public static int ERETURNCODEHALL_NAME_REPEAT = -301;
        /// <summary>
        /// Code : -302
        /// 玩家不在线
        /// </summary>
        public static int ERETURNCODEHALL_PLAYER_NOT_ONLINE = -302;
        /// <summary>
        /// Code : -303
        /// 加入房间错误
        /// </summary>
        public static int ERETURNCODEHALL_PLAYER_ADD_ROOM_ERROR = -303;
        /// <summary>
        /// Code : -304
        /// 获取房间信息错误
        /// </summary>
        public static int ERETURNCODEHALL_PLAYER_GET_ROOM_INFO_ERROR = -304;
        /// <summary>
        /// Code : -305
        /// 名称不能为空
        /// </summary>
        public static int ERETURNCODEHALL_NO_NULL = -305;
        /// <summary>
        /// Code : -306
        /// 名称长度超出限制
        /// </summary>
        public static int ERETURNCODEHALL_LENGTH_OVERFLOW = -306;
    }
}
