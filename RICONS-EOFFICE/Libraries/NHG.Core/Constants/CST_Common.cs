using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHG.Core.Constants
{
    public class CST_Common
    {
        public const string CST_DELETE = "1";
        public const string CST_NOT_DELETE = "0";

        public const string CST_ACTIVE = "1";
        public const string CST_NOT_ACTIVE = "0";

        #region LoginState
        public const byte CST_Login = 1;
        public const byte CST_LogOut = 2;
        public const byte CST_Lock = 5;
        /// <summary>
        /// Trang thai login
        /// </summary>
        public static String CST_LOGIN_STATE_DISP = "Login";
        /// <summary>
        /// Trang thai logout
        /// </summary>
        public static String CST_LOGOUT_STATE_DISP = "Logout";
        /// <summary>
        /// Trang thai bi lock
        /// </summary>
        public static String CST_LOCK_STATE_DISP = "Lock";

        #endregion

        public const string CST_NOT_PUBLIC_EVENT = "0";
        public const string CST_PUBLIC_EVENT = "1";
    }
}
