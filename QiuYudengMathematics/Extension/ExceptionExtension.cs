using System;

namespace QiuYudengMathematics.Extension
{
    public static class ExceptionExtension
    {
        public static string GetErrorMsg(this Exception ex)
        {
            var errorMsg = "Msg:[" + ex.Message + "]";
            if (ex.InnerException != null)
            {
                errorMsg += ",Msg2:[" + ex.InnerException.Message + "]";
                if (ex.InnerException.InnerException != null)
                {
                    errorMsg += ",Msg3:[" + ex.InnerException.InnerException.Message + "]";
                }
            }

            return errorMsg;
        }
    }
}
