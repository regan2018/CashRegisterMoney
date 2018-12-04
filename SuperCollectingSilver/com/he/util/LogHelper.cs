using log4net;
using System;

namespace SuperCollectingSilver.com.he.util
{
	public class LogHelper
	{
		public static void WriteLog(Type t, Exception ex)
		{
			ILog logger = LogManager.GetLogger(t);
			logger.Error("Error", ex);
		}

		public static void WriteLog(Type t, string msg)
		{
			ILog logger = LogManager.GetLogger(t);
			logger.Error(msg);
		}
	}
}
