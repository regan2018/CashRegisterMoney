using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json.Linq;
using SuperCollectingSilver.com.he.util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SuperCollectingSilver.com.he.util.PublicUtil;

namespace SuperCollectingSilver.com.he.ExtChromiumBrowser
{
    /// <summary>
    /// 自定义谷歌内核浏览器--扩展（支持第二屏幕显示用）
    /// </summary>
    public class MyChromiumBrowserExtend : Control
    {
        private ChromiumWebBrowser webBrowser;//浏览器


        /// <summary>
        /// 承载浏览器的窗体类，必须设置
        /// </summary>
        private MainForm mainWindow;

        /// <summary>
        /// 承载浏览器的窗体类，需要第二显示器显示时设置
        /// </summary>
        public SecondScreenShowForm secodeScreenShowWindow;

        private Form window;


        #region 浏览器初始化
        /// <summary>
        /// 浏览器初始化
        /// </summary>
        public static void BrowserInit()
        {
            #region 浏览器全局设置

            var setting = new CefSharp.CefSettings();
            var osVersion = Environment.OSVersion;
            //Disable GPU for Windows 7          
            if (osVersion.Version.Major == 6 && osVersion.Version.Minor == 1)
            {
                // Disable GPU in WPF and Offscreen examples until #1634 has been resolved6                 
                //setting.CefCommandLineArgs.Add("disable-gpu", "1");//禁用GPU
            }
            setting.CefCommandLineArgs.Add("disable-gpu", "1");//禁用GPU
            setting.CefCommandLineArgs.Add("enable-webgl", "1");
            setting.Locale = "zh-CN";
            //缓存路径
            setting.CachePath = Application.StartupPath + "/BrowserCache";
            //浏览器引擎的语言
            setting.AcceptLanguageList = "zh-CN,zh;q=0.9,en;q=0.6";
            setting.LocalesDirPath = Application.StartupPath + "/localeDir";
            //日志文件
            setting.LogFile = Application.StartupPath + "/LogData";
            //只记录错误日志
            setting.LogSeverity = LogSeverity.Error;

            setting.PersistSessionCookies = true;
            setting.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
            setting.UserDataPath = Application.StartupPath + "/userData";

            //开启ppapi-flash
            setting.CefCommandLineArgs.Add("enable-npapi", "1");
            setting.CefCommandLineArgs.Add("--ppapi-flash-path", System.AppDomain.CurrentDomain.BaseDirectory + "Plugins\\pepflashplayer.dll"); //指定flash的版本，不使用系统安装的flash版本
            setting.CefCommandLineArgs.Add("--ppapi-flash-version", "22.0.0.192");

            setting.CefCommandLineArgs.Add("Connection", "keep-alive");
            setting.CefCommandLineArgs.Add("Accept-Encoding", "gzip, deflate, br,compress");
            setting.CefCommandLineArgs.Add("enable-media-stream", "1");

            Cef.Initialize(setting, false, false);
           
            #endregion

        }
        #endregion

        #region 公共设置
        /// <summary>
        /// 公共设置
        /// </summary>
        private void publicSet()
        {

            webBrowser = new ChromiumWebBrowser("about:blank");

            #region 设置js与cefSharp互通
            //CefSharp.CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            //webBrowser.RegisterJsObject("googleBrower", this, new CefSharp.BindingOptions() { CamelCaseJavascriptNames = false });
            webBrowser.RegisterJsObject("googleBrower", this,false);
            #endregion

            
            BrowserSettings bset = new BrowserSettings();
            bset.Plugins = CefState.Enabled;//启用插件
            bset.WebSecurity = CefState.Disabled;//禁用跨域限制
            webBrowser.BrowserSettings = bset;

            webBrowser.DownloadHandler = new DownloadHandler();
            webBrowser.KeyboardHandler = new KeyBoardHandler();
            //MenuHandler.mainWindow = mainWindow;
            //webBrowser.MenuHandler = new MenuHandler();

            //webBrowser.Dock = DockStyle.Fill;
            //webBrowser.Margin = new Padding(0, 0, 0, 0);
            //mainWindow.Controls.Add(webBrowser);
        }
        #endregion

        public MyChromiumBrowserExtend() { }

        /// <summary>
        /// 获取实例
        /// <param name="mainWindow">主屏显示的窗体</param>
        /// </summary>
        public MyChromiumBrowserExtend(MainForm mainWindow)
        {
            this.publicSet();
            
            this.mainWindow = mainWindow;
            this.window = this.mainWindow;

            #region 处理一些浏览器事件
            webBrowser.DownloadHandler = new DownloadHandler();
            webBrowser.KeyboardHandler = new KeyBoardHandler();
            MenuHandler.mainWindow = mainWindow;
            webBrowser.MenuHandler = new MenuHandler();
            webBrowser.DisplayHandler = new DisplayHandler();

            webBrowser.KeyUp += WebBrowser_KeyUp;
            #endregion

            webBrowser.Dock = DockStyle.Fill;
            //添加到窗体中的panel容器中
            //mainWindow.panel.Controls.Add(webBrowser);

            //mainWindow.panel.Dock = DockStyle.Fill;
            //mainWindow.panel.SizeChanged += Panel_SizeChanged;
            //Panel_SizeChanged(null, null);

        }

        
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="seconddScreenShowWindow">第二屏幕显示的窗体</param>
        /// <returns></returns>
        public MyChromiumBrowserExtend(SecondScreenShowForm secodeScreenShowWindow)
        {
            this.publicSet();

            this.secodeScreenShowWindow = secodeScreenShowWindow;
            this.window = this.secodeScreenShowWindow;

            #region 处理一些浏览器事件
            webBrowser.DownloadHandler = new DownloadHandler();
            webBrowser.KeyboardHandler = new KeyBoardHandler();
            MenuHandler.mainWindow = secodeScreenShowWindow;
            webBrowser.MenuHandler = new MenuHandler();

            webBrowser.KeyUp += WebBrowser_KeyUp;

            webBrowser.FrameLoadEnd += SecondScreenShowWebBrowser_FrameLoadEnd;
            #endregion

            webBrowser.Dock = DockStyle.Fill;
            //添加到窗体中的panel容器中
            secodeScreenShowWindow.panel.Controls.Add(webBrowser);

            secodeScreenShowWindow.panel.Dock = DockStyle.Fill;
            secodeScreenShowWindow.panel.SizeChanged += SecondScreenShowPanel_SizeChanged;
            SecondScreenShowPanel_SizeChanged(null, null);
        }

        private void WebBrowser_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                //显示开发者工具
                webBrowser.ShowDevTools();
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void Reload()
        {
            webBrowser.Reload();
        }

        private void Panel_SizeChanged(object sender, EventArgs e)
        {
            mainWindow.panel.Width = mainWindow.Width;
            mainWindow.panel.Height = mainWindow.Height-35;
            mainWindow.panel.Top = -5;
            mainWindow.panel.Left = 0;
        }

        private void SecondScreenShowPanel_SizeChanged(object sender, EventArgs e)
        {
            secodeScreenShowWindow.panel.Width = secodeScreenShowWindow.Width;
            secodeScreenShowWindow.panel.Height = secodeScreenShowWindow.Height;
            secodeScreenShowWindow.panel.Top = 0;
            secodeScreenShowWindow.panel.Left = 0;
        }
        private static string filePath = "";
        private void SecondScreenShowWebBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            filePath = filePath.Replace("%23", "#");
            //第二屏幕显示的内容加载完后删除html文件
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        #region cefSharp与js交互
        /// <summary>
        /// 运行js代码
        /// </summary>
        /// <param name="jsCode"></param>
        public void actionJsCode(string jsCode)
        {
            webBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(jsCode);
        }
        #endregion

        #region 用于页面js与后台cefSharp互通操作
        /// <summary>
        /// 用于页面与后台互通操作
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="action">动作：print=打印，printSet=打印设置，printPreview=打印预览</param>
        public string cefQuery(string data, string action)
        {
            bool ret = true;//处理成功或失败状态
            string msg = "";//返回消息
            string retData = "";//返回信息
            try
            {

                ActionType actionType = ActionType.直接打印;
                if ("print".ToLower().Equals(action.ToLower().Trim()))
                {
                    actionType = ActionType.直接打印;
                }
                if ("printSet".ToLower().Equals(action.ToLower().Trim()))
                {
                    actionType = ActionType.打印设置;
                }
                if ("printPreview".ToLower().Equals(action.ToLower().Trim()))
                {
                    actionType = ActionType.打印预览;
                }
                if ("secondScreen".ToLower().Equals(action.ToLower().Trim()))
                {
                    actionType = ActionType.第二显示器;
                }
                if ("exit".ToLower().Equals(action.ToLower().Trim()))
                {
                    actionType = ActionType.退出程序;
                }
                if ("openMoneyBox".ToLower().Equals(action.ToLower().Trim()))
                {
                    actionType = ActionType.打开钱箱;
                }

                #region 客显相关
                if ("checkCDPType".ToLower().Equals(action.ToLower().Trim()))
                {
                    actionType = ActionType.检测客显类型;
                }
                if ("LED_CDP_Clear".ToLower().Equals(action.ToLower().Trim()))
                {
                    actionType = ActionType.LED客显_清屏;
                }
                if ("LED_CDP_Price".ToLower().Equals(action.ToLower().Trim()))
                {
                    actionType = ActionType.LED客显_单价;
                }
                if ("LED_CDP_Total".ToLower().Equals(action.ToLower().Trim()))
                {
                    actionType = ActionType.LED客显_总计;
                }
                if ("LED_CDP_Recive".ToLower().Equals(action.ToLower().Trim()))
                {
                    actionType = ActionType.LED客显_收款;
                }
                if ("LED_CDP_Change".ToLower().Equals(action.ToLower().Trim()))
                {
                    actionType = ActionType.LED客显_找零;
                }
                #endregion

                switch (actionType)
                {
                    case ActionType.第二显示器://用于第二显示器显示

                        //将数据转成html文件
                        data = HtmlTextConvertFile(data);

                        if (null != this.secodeScreenShowWindow)
                        {
                            data = data.Replace("#", "%23");
                            filePath = data;
                            this.secodeScreenShowWindow.myBrowser.Navigate(data);
                        }
                        break;
                    case ActionType.退出程序://用于关闭应用程序
                        if (null != this.mainWindow)
                        {
                            this.mainWindow.Exit();
                        }
                        break;
                    case ActionType.打开钱箱://用于打开钱箱
                        PublicUtil.openMoneyBox();
                        break;

                    #region 客显相关
                    case ActionType.检测客显类型:
                        string customerDisplayType = ConfigurationManager.AppSettings[AppSettionsType.客显类型];
                        retData = customerDisplayType;
                        break;
                    case ActionType.LED客显_单价:
                        try
                        {
                            PublicUtil.ledCustomerShow(Convert.ToDecimal(data), LedCustomerDispiayType.Price);
                        }
                        catch {
                            ret = false;
                            msg = "请传入浮点型数据";
                        }
                        break;
                    case ActionType.LED客显_总计:
                        try
                        {
                            PublicUtil.ledCustomerShow(Convert.ToDecimal(data), LedCustomerDispiayType.Total);
                        }
                        catch
                        {
                            ret = false;
                            msg = "请传入浮点型数据";
                        }
                        break;
                    case ActionType.LED客显_收款:
                        try
                        {
                            PublicUtil.ledCustomerShow(Convert.ToDecimal(data), LedCustomerDispiayType.Recive);
                        }
                        catch
                        {
                            ret = false;
                            msg = "请传入浮点型数据";
                        }
                        break;
                    case ActionType.LED客显_找零:
                        try
                        {
                            PublicUtil.ledCustomerShow(Convert.ToDecimal(data), LedCustomerDispiayType.Change);
                        }
                        catch
                        {
                            ret = false;
                            msg = "请传入浮点型数据";
                        }
                        break;
                    case ActionType.LED客显_清屏:
                        try
                        {
                            PublicUtil.ledCustomerShow(0, LedCustomerDispiayType.Clear);
                        }
                        catch
                        {
                            ret = false;
                            msg = "请传入浮点型数据";
                        }
                        break;
                    #endregion

                    default:
                        //将数据转成html文件
                        data = HtmlTextConvertFile(data);

                        this.window.Invoke((EventHandler)delegate
                        {
                        //设置预打印文件的路径，并执行对应的操作,data为打印文件路径
                        PrintUtil.Instance.SetPrintFilePath(data, actionType);
                        });
                        break;
                }
                
            }
            catch (Exception ex)
            {
                ret = false;
                msg = "请求发生异常，异常消息：" + ex.Message;
            }

            JObject retJob = new JObject();
            retJob.Add("ret", ret);
            retJob.Add("msg", msg);
            retJob.Add("data", retData);

            return retJob.ToString();
        }

        #endregion

        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="url"></param>
        public void Navigate(string url)
        {
            webBrowser.Load(url);
        }

        /// <summary>
        /// 获取浏览器对象
        /// </summary>
        /// <returns></returns>
        public ChromiumWebBrowser getWebBrowser()
        {
            return this.webBrowser;
        }

        #region HTML文本内容转HTML文件
        /// <summary>
        /// HTML文本内容转HTML文件
        /// </summary>
        /// <param name="strHtml">HTML文本内容</param>
        /// <returns>HTML文件的路径</returns>
        public static string HtmlTextConvertFile(string strHtml)
        {
            if (string.IsNullOrEmpty(strHtml))
            {
                throw new Exception("HTML text content cannot be empty.");
            }

            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"html\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = path + DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 10000) + ".html";
                FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
                streamWriter.Write(strHtml);
                streamWriter.Flush();

                streamWriter.Close();
                streamWriter.Dispose();
                fileStream.Close();
                fileStream.Dispose();
                return fileName;
            }
            catch
            {
                throw new Exception("HTML text content error.");
            }
        }
        #endregion
    }
}
