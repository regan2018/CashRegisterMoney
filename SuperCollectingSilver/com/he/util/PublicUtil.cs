using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SuperCollectingSilver.com.he.util
{
    class PublicUtil
    {
        /// <summary>
        /// AppSettings中的配置参数名称
        /// </summary>
        public static class AppSettionsType
        {
            public static readonly string 系统页面地址 = "BaseUrl";
            public static readonly string 升级程序的服务器IP = "UpdateUrl";
            public static readonly string 升级程序的服务器请求端口 = "UpdatePort";
            public static readonly string 钱箱端口 = "MoneyPort";
            public static readonly string 钱箱引脚类型 = "MoneyBoxType";
            public static readonly string 打开钱箱编码_2线针脚接口 = "OpenMoneyCode2";
            public static readonly string 打开钱箱编码_5线针脚接口 = "OpenMoneyCode5"; 
            public static readonly string 缓存文件的有效期_天 = "CacheFileValidityPeriod";
            public static readonly string 定时清理内存时间_分 = "AutoClearTimer";
            public static readonly string 自定义COM扩展端口 = "直连小票机"; 
            public static readonly string 客显类型 = "CustomerDisplayType"; 
            public static readonly string LED客显端口 = "LedCustomerDisplayPort";
            public static readonly string LED客显端口通信波特率 = "LedCustomerDisplayPortBaudRate"; 
            public static readonly string LED客显数据位 = "LedCustomerDisplayDataBits"; 
            public static readonly string LED客显型号 = "LedCustomerDisplaySpecification"; 
        }

        public static class Config
        {
            /// <summary>
            /// false=退出时安装，true=立即安装
            /// </summary>
            public static bool 安装时间 = false;

            /// <summary>
            /// 下载更新程序退出时赋值
            /// </summary>
            public static string 下载的更新程序完整路径="";

            /// <summary>
            /// 客显类型
            /// </summary>
            public enum CustomerDisplayType
            {
                LED客显,
                LCD液晶客显
            }

            #region 客显型号
            /// <summary>
            /// 客显型号
            /// </summary>
            public static class CustomerDisplaySpecification
            {
                /// <summary>
                /// (唯拓)VT-VFD8C
                /// </summary>
                public static readonly string VT_VFD8C = "(唯拓)VT-VFD8C";

            }
            #endregion
        }

        #region 修改或添加AppSettings中配置
        /// <summary>
        /// 修改或添加AppSettings中配置
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="value">相应值</param>
        public static bool SetConfigValue(string key, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[key] != null)
                    config.AppSettings.Settings[key].Value = value;
                else
                    config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 打开钱箱
        /// <summary>
        /// 打开钱箱
        /// </summary>
        /// <returns></returns>
        public static bool openMoneyBox()
        {
            bool flag = false;

            string port = ConfigurationManager.AppSettings[AppSettionsType.钱箱端口];
            string openMoneyBox = ConfigurationManager.AppSettings[AppSettionsType.打开钱箱编码_2线针脚接口];
            string moneyBoxType = ConfigurationManager.AppSettings[AppSettionsType.钱箱引脚类型];

            if ("2".Equals(moneyBoxType))
            {
                openMoneyBox = ConfigurationManager.AppSettings[AppSettionsType.打开钱箱编码_2线针脚接口];
            }
            if ("5".Equals(moneyBoxType))
            {
                openMoneyBox = ConfigurationManager.AppSettings[AppSettionsType.打开钱箱编码_5线针脚接口];
            }
            try
            {
                if (null == openMoneyBox || openMoneyBox.Length == 0)
                {
                    LogHelper.WriteLog(typeof(PublicUtil), port + "开钱箱编码有误！");
                    return flag;
                }
                string[] openCode = openMoneyBox.Split(',');
                if (openCode.Length < 5)
                {
                    LogHelper.WriteLog(typeof(PublicUtil), port + "开钱箱编码有误！");
                    return flag;
                }
                string send = "" + (char)Convert.ToInt32(openCode[0]) + (char)Convert.ToInt32(openCode[1]) + (char)Convert.ToInt32(openCode[2]) + (char)Convert.ToInt32(openCode[3]) + (char)Convert.ToInt32(openCode[4]);

                if (port.ToString().Equals(AppSettionsType.自定义COM扩展端口))
                {//直连小票机
                    String printName = PrinterHelper.GetDeaultPrinterName();
                    if (PrinterHelper.StartQianXiang(printName, send))
                    {
                        flag = true;
                        LogHelper.WriteLog(typeof(PublicUtil), "钱箱打开中……");
                    }
                    else
                    {
                        flag = false;
                        LogHelper.WriteLog(typeof(PublicUtil), "钱箱打开失败");
                    }
                }
                else
                {//使用钱箱中间件
                    System.IO.Ports.SerialPort com = new System.IO.Ports.SerialPort(port);
                    com.Open();
                    com.WriteLine(send);
                    com.Close();
                    flag = true;
                    LogHelper.WriteLog(typeof(PublicUtil), "钱箱打开中……");
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(PublicUtil), port + "端口打开失败");
                flag = false;
            }
            return flag;
        }
        #endregion

        #region LED客显显示数据
        /// <summary>
        /// LED客显显示数据
        /// </summary>
        /// <param name="data">显示数据</param>
        /// <param name="dispiayType">显示类型</param>
        /// <returns></returns>
        public static bool ledCustomerShow(decimal data, LedCustomerDispiayType dispiayType )
        {
            bool flag = false;
            try
            {
                string ledCustomerPort = ConfigurationManager.AppSettings[AppSettionsType.LED客显端口];
                string ledCustomerPortBaudRate = ConfigurationManager.AppSettings[AppSettionsType.LED客显端口通信波特率];
                string LedCustomerDisplayDataBits = ConfigurationManager.AppSettings[AppSettionsType.LED客显数据位];
                string LedCustomerDisplaySpecification = ConfigurationManager.AppSettings[AppSettionsType.LED客显型号];

                LedCustomerDisplay display = new LedCustomerDisplay(ledCustomerPort, Convert.ToInt32(ledCustomerPortBaudRate.Trim()), System.IO.Ports.StopBits.One.ToString(), Convert.ToInt32(LedCustomerDisplayDataBits.Trim()));

                if (dispiayType == LedCustomerDispiayType.Clear)
                {
                    //唯拓VT_VFD8C型号
                    if (Config.CustomerDisplaySpecification.VT_VFD8C.ToUpper().Equals(LedCustomerDisplaySpecification.ToUpper()))
                    {
                        //清屏
                        display.DisplayData("", LedCustomerDispiayType_VT_VFD8C.Clear);
                    }else
                    {
                        //缺省型号
                        //清屏
                        display.DisplayData("", LedCustomerDispiayType.Clear);
                    }
                }
                else
                {
                    //唯拓VT_VFD8C型号
                    if (Config.CustomerDisplaySpecification.VT_VFD8C.ToUpper().Equals(LedCustomerDisplaySpecification.ToUpper()))
                    {
                        LedCustomerDispiayType_VT_VFD8C type = LedCustomerDispiayType_VT_VFD8C.Clear;
                        if (dispiayType == LedCustomerDispiayType.Clear) { type = LedCustomerDispiayType_VT_VFD8C.Clear; }
                        if (dispiayType == LedCustomerDispiayType.Change) { type = LedCustomerDispiayType_VT_VFD8C.Change; }
                        if (dispiayType == LedCustomerDispiayType.Price) { type = LedCustomerDispiayType_VT_VFD8C.Price; }
                        if (dispiayType == LedCustomerDispiayType.Recive) { type = LedCustomerDispiayType_VT_VFD8C.Recive; }
                        if (dispiayType == LedCustomerDispiayType.Total) { type = LedCustomerDispiayType_VT_VFD8C.Total; }
                        //数据显示
                        display.DisplayData(data + "", type);
                    }else
                    {
                        //缺省型号
                        //数据显示
                        display.DisplayData(data + "", dispiayType);
                    }
                }

                flag = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("显示失败，原因：" + ex.Message);
                flag = false;
            }
            return flag;
        }
        #endregion

    }
}
