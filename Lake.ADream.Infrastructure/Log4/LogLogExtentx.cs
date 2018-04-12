using System;
using System.Collections.Generic;
using System.Text;
using log4net.Util;
namespace Lake.ADream.Log4
{
    /// <summary>
    /// 
    /// </summary>
    public static class Loger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLog"></param>
        /// <param name="ex"></param>
        public static void Error( Exception ex)
        {
            LogLog.Error(ex.GetType(), ex.Message);
        }
    }
}
