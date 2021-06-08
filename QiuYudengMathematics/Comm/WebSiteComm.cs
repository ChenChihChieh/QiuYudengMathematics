using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace QiuYudengMathematics.Comm
{
    public class WebSiteComm
    {
        public static string CurrentUserAccount
        {
            get
            {
                FormsIdentity identity = HttpContext.Current.User.Identity as FormsIdentity;
                if (identity == null)
                    return string.Empty;
                else
                    return identity.Name;
            }
        }
        public static string CurrentUserName
        {
            get
            {
                FormsIdentity identity = HttpContext.Current.User.Identity as FormsIdentity;
                if (identity == null)
                    return string.Empty;
                else
                    return identity.Ticket.UserData;
            }
        }
    }
}