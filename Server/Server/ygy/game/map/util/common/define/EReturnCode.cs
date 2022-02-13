using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.util.common.define
{
    public class EReturnCode
    {
        /// <summary>
        /// code : 0
        /// 成功
        /// </summary>
        public static int ERETURNCODE_SUCCESS = 0;

        /// <summary>
        /// -100 ~ - 200 
        /// 登陆返回码范围
        /// </summary>
        public static int ERETURNCODE_LOGIN = -100;

        /// <summary>
        /// -200 ~ - 300 
        /// 登陆返回码范围
        /// </summary>
        public static int ERETURNCODE_CHAT = -200;

    }
}
