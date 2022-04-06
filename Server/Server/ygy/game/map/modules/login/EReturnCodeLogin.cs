using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.modules.login
{
    public class EReturnCodeLogin
    {
        /// <summary>
        /// code : -100
        /// 不存在该账号
        /// </summary>
        public static int ERETURNCODELOGIN_NO_ACCOUNT = -100;

        /// <summary>
        /// code : -101
        /// 请求数据错误
        /// </summary>
        public static int ERETURNCODELOGIN_REQUEST_ERROR = -101;

        /// <summary>
        /// code : -102
        /// 密码错误
        /// </summary>
        public static int ERETURNCODELOGIN_PASSWORD_ERROR = -102;

        /// <summary>
        /// code : -103
        /// 账号已被登陆
        /// </summary>
        public static int ERETURNCODELOGIN_ALREALDY_LOGIN = -103;

        /// <summary>
        /// code : -104
        /// 账号被强占，导致退出游戏
        /// </summary>
        public static int ERETURNCODELOGIN_CLOSE = -104;

        /// <summary>
        /// code : -105
        /// 长度不合法
        /// </summary>
        public static int ERETURNCODELOGIN_LENGTH_ILEGAL = -105;

        /// <summary>
        /// code : -106
        /// 账号已存在
        /// </summary>
        public static int ERETURNCODELOGIN_ACOUNT_EXISIT = -106;

        /// <summary>
        /// code : -107
        /// 密码不合法
        /// </summary>
        public static int ERETURNCODELOGIN_PASSWORD_ILEGAL = -107;
        /// <summary>
        /// code : -108
        /// 用户连接集合不存在
        /// </summary>
        public static int ERETURNCODELOGIN_NO_USER_DIC = -108;

        /// <summary>
        /// code : -109
        /// 发送消息失败
        /// </summary>
        public static int ERETURNCODELOGIN_SEND_MESSAGE_ERROR = -109;
    }
}
