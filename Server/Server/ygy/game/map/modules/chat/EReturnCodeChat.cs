using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.modules.chat
{
    public class EReturnCodeChat
    {
        /// <summary>
        /// Code : -200
        /// 配置错误
        /// </summary>
        public static int ERETURNCODECHAT_CONFIG_ERROR = -200;

        /// <summary>
        /// Code : -201
        /// 不存在该账号
        /// </summary>
        public static int ERETURNCODECHAT_NO_ACCOUNT = -201;

        /// <summary>
        /// Code : -202
        /// 该账号不在线
        /// </summary>
        public static int ERETURNCODECHAT_ACCOUNT_OFFLINE = -202;

        /// <summary>
        /// Code : -203
        /// character不存在
        /// </summary>
        public static int ERETURNCODECHAT_NO_CHARACTER = -203;

        /// <summary>
        /// Code : -204
        /// 好友列表不存在
        /// </summary>
        public static int ERETURNCODECHAT_NO_FRIEND_LIST = -204;

        /// <summary>
        /// Code : -205
        /// 已经是好友
        /// </summary>
        public static int ERETURNCODECHAT_ALREADY_FRIEND = -205;


    }
}
