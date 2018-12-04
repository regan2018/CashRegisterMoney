using CefSharp;
using CefSharp.WinForms;
using SuperCollectingSilver.com.he.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperCollectingSilver.com.he.ExtChromiumBrowser
{
    /// <summary>
    /// 自定义谷歌内核浏览器
    /// </summary>
    class MyChromiumBrowser: Control
    {
        private static readonly MyChromiumBrowser instance = new MyChromiumBrowser();
        private static ChromiumWebBrowser webBrowser;//浏览器
        private static MainForm mainWindow;//承载浏览器的窗体类，必须设置

        //static MyChromiumBrowser() { }
        private MyChromiumBrowser()
        {
            #region 浏览器全局设置
            var setting = new CefSharp.CefSettings();
            setting.Locale = "zh-CN";
            //缓存路径
            setting.CachePath = Application.StartupPath + "/BrowserCache";
            //浏览器引擎的语言
            setting.AcceptLanguageList = "zh-CN,zh;q=0.9";
            setting.LocalesDirPath = Application.StartupPath + "/localeDir";
            //日志文件
            setting.LogFile = Application.StartupPath + "/LogData";
            setting.PersistSessionCookies = true;
            setting.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
            setting.UserDataPath = Application.StartupPath + "/userData";

            //开启ppapi-flash
            //setting.CefCommandLineArgs.Add("enable-npapi", "1");
            setting.CefCommandLineArgs.Add("--ppapi-flash-path", System.AppDomain.CurrentDomain.BaseDirectory + "Plugins\\pepflashplayer.dll"); //指定flash的版本，不使用系统安装的flash版本
            setting.CefCommandLineArgs.Add("--ppapi-flash-version", "22.0.0.192");

            setting.CefCommandLineArgs.Add("Connection", "keep-alive");
            setting.CefCommandLineArgs.Add("Accept-Encoding", "gzip, deflate, br");

            CefSharp.Cef.Initialize(setting);
            #endregion

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

            //webBrowser.DownloadHandler = new DownloadHandler();
            //webBrowser.KeyboardHandler = new KeyBoardHandler();
            
            //MenuHandler.mainWindow = mainWindow;
            //webBrowser.MenuHandler = new MenuHandler();

            //webBrowser.Dock = DockStyle.Fill;
            //webBrowser.Margin = new Padding(0, 0, 0, 0);
            //mainWindow.Controls.Add(webBrowser);
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        public static MyChromiumBrowser Instance(MainForm mainWindow) {

            MyChromiumBrowser.mainWindow = mainWindow;

            #region 处理一些浏览器事件
            webBrowser.DownloadHandler = new DownloadHandler();
            webBrowser.KeyboardHandler = new KeyBoardHandler();
            MenuHandler.mainWindow = mainWindow;
            webBrowser.MenuHandler = new MenuHandler();

            webBrowser.KeyUp += WebBrowser_KeyUp;
            #endregion

            webBrowser.Dock = DockStyle.Fill;
            //添加到窗体中的panel容器中
            mainWindow.panel.Controls.Add(webBrowser);

            mainWindow.panel.Dock = DockStyle.Fill;
            mainWindow.panel.SizeChanged += Panel_SizeChanged;
            Panel_SizeChanged(null, null);
            return instance;
        }

        private static void WebBrowser_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                //显示开发者工具
                webBrowser.ShowDevTools();
            }
        }

        private static void Panel_SizeChanged(object sender, EventArgs e)
        {
            mainWindow.panel.Width = mainWindow.Width;
            mainWindow.panel.Height = mainWindow.Height-35;
            mainWindow.panel.Top = -5;
            mainWindow.panel.Left = 0;
        }

        #region cefSharp与js交互
        /// <summary>
        /// 运行js代码
        /// </summary>
        /// <param name="browser"></param>
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
        public void cefQuery(string data, string action)
        {
            //将数据转成html文件
            data= HtmlTextConvertFile(data);

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
            mainWindow.Invoke((EventHandler)delegate
            {
                //设置预打印文件的路径，并执行对应的操作,data为打印文件路径
                PrintUtil.Instance.SetPrintFilePath(data, actionType);
            });

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
