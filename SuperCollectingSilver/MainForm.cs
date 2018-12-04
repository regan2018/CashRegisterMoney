using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.WinForms;
using System.Drawing.Printing;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections;
using SuperCollectingSilver.com.he.util;
using CefSharp;
using SuperCollectingSilver.com.he.ExtChromiumBrowser;
using System.Diagnostics;
using OAUS.Core;
using System.Configuration;
using System.Threading;
using static SuperCollectingSilver.com.he.util.PublicUtil;
using System.Runtime.InteropServices;
using AutoUpdater;
using SuperCollectingSilver.Properties;

namespace SuperCollectingSilver
{
    public partial class MainForm : Form
    {

        private MyChromiumBrowserExtend myBrowser;//浏览器对象

        private string printFilePath;//测试打印文件路径

        private string oausServerIP = "119.23.15.8";//默认升级程序服务地址
        private int oausServerPort = 4540;//默认升级程序服务请求商品

        public Panel panel;

        public Screen[] screenList;//显示器列表

        public SecondScreenShowForm secondScreenShowWindow;//第二显示器显示的窗体

        public MainForm()
        {
            InitializeComponent();

            this.Check();

            //如果是xp系统，就进行谷歌浏览器初始化设置，不是xp系统则不设置
            //if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 1)
            //{
            //    //初始化谷歌浏览器的全局设置
            //    MyChromiumBrowserExtend.BrowserInit();
            //}

            //初始化谷歌浏览器的全局设置
            MyChromiumBrowserExtend.BrowserInit();


            #region 分屏显示设置
            if (Screen.AllScreens.Length > 1)
            {
                secondScreenShowWindow = new SecondScreenShowForm();
                showOnMonitor(secondScreenShowWindow, 1);
            }
            #endregion

            this.Load += MainForm_Load;
            this.FormClosing += MainForm_FormClosing;

            //Windows 7 不使用任务栏的退出程序功能       
            if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1)
            {
                this.退出系统ToolStripMenuItem.Visible = false;
            }
        }

        #region 分屏显示设置
        /// <summary>
        /// 分屏显示处理
        /// </summary>
        /// <param name="window">要显示在指定屏幕的窗体</param>
        /// <param name="showOnMonitor">设置显示在第几的一个显示监视器（从0开始）</param>
        private void showOnMonitor(Form window,int showOnMonitor)
        {
            Screen[] sc;
            sc = Screen.AllScreens;
            if (showOnMonitor >= sc.Length)
            {
                showOnMonitor = sc.Length-1;
            }

            if(null== window)
            {
                window = new Form();
            }


            window.StartPosition = FormStartPosition.Manual;
            window.Location = new Point(sc[showOnMonitor].Bounds.Left, sc[showOnMonitor].Bounds.Top);
            // 如果你想使形式最大化，把它变成常态，然后最大化。
            window.WindowState = FormWindowState.Normal;
            window.WindowState = FormWindowState.Maximized;
            window.Show();
        }
        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
           
            myBrowser.actionJsCode("handoverClass();");
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.panel = new Panel();
            this.Controls.Add(panel);

            var path = Application.StartupPath + @"\html\test.html";
            path = path.Replace("#", "%23");

            myBrowser = new MyChromiumBrowserExtend(this);
            myBrowser.secodeScreenShowWindow = secondScreenShowWindow;


            string baseUrl = "http://119.23.15.8:8080/tty";
            try
            {
                baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            }
            catch (Exception)
            {
                baseUrl = "http://119.23.15.8:8080/tty";
            }
            myBrowser.Navigate(baseUrl);
            this.panel.Controls.Add(myBrowser.getWebBrowser());
            this.panel.Dock = DockStyle.Fill;

            //清理内存
            new Thread(TimerCleanLocalMemory).Start();
        }

        #region 清理本程序内存
        /// <summary>
        /// 定时清理本程序内存
        /// </summary>
        public void TimerCleanLocalMemory()
        {
            try
            {
                string autoClearTimer = ConfigurationManager.AppSettings[AppSettionsType.定时清理内存时间_分];
                int timer = Convert.ToInt32(autoClearTimer);

                
                while (true)
                {
                    Thread.Sleep(1000 * 60 * timer);//睡眠10分钟

                    //清理本程序内存
                    ClearLocalMemory();

                    //清理所有程序的内存
                    //ClearMemory();
                }
            }
            catch { }
        }

        #endregion

        #region 检查更新
        public void Check()
        {
            try
            {
                try
                {
                    oausServerIP = ConfigurationManager.AppSettings["UpdateUrl"].ToString();
                    oausServerPort = int.Parse(ConfigurationManager.AppSettings["UpdatePort"].ToString());
                }
                catch { }
                bool flag = VersionHelper.HasNewVersion(oausServerIP, oausServerPort);

                if (flag)
                {
                    bool flag2 = DialogResult.OK == MessageBox.Show("亲,有新版本哦,赶快点击“确定”升级吧！", "升级提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (flag2)
                    {
                        //string serverIP = ConfigurationManager.AppSettings["ServerIP"];
                        //int serverPort = int.Parse(ConfigurationManager.AppSettings["ServerPort"]);
                        string systemName = ConfigurationManager.AppSettings["SystemName"];
                        string title = "客户端系统升级";
                        string processName = systemName;// systemName.Substring(0, systemName.Length - 4);
                        bool haveRun = ESBasic.Helpers.ApplicationHelper.IsAppInstanceExist(processName);
                        if (haveRun)
                        {
                            MessageBox.Show(Resources.TargetIsRunning);
                            return;
                        }

                        AutoUpdaterForm  auto = new AutoUpdaterForm(oausServerIP, oausServerPort, systemName, title,this);
                        auto.Show();

                        //string fileName = AppDomain.CurrentDomain.BaseDirectory + "AutoUpdater\\AutoUpdater.exe";
                        //Process process = Process.Start(fileName);
                        //Process.GetCurrentProcess().Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取版本信息失败！");
                LogHelper.WriteLog(typeof(MainForm), ex.Message + "  :获取更新信息异常，请手动下载更新包！");
            }
        }


        #endregion


        #region 右下角Icon图标右键菜单

        private void 打印设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printFilePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"html\print_test.html";
            this.Invoke((EventHandler)delegate
            {
                //设置打印文件路径
                PrintUtil.Instance.SetPrintFilePath(printFilePath, ActionType.打印设置);
            });
        }

        private void 退出系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("您确定要退出吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                this.Exit();
            }
        }

        private void 打印测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printFilePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"html\print_test.html";
            this.Invoke((EventHandler)delegate
            {
                //设置打印文件路径
                PrintUtil.Instance.SetPrintFilePath(printFilePath, ActionType.弹窗打印);
            });
        }
        /// <summary>
        /// 显示主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.Activate();
            this.WindowState = FormWindowState.Maximized;
        }

        private void 显示主界面toolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.notifyIcon_DoubleClick(null, null);
        }

        //private void 清除登录信息ToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    //Cef.GetGlobalCookieManager().DeleteCookies();
        //    myBrowser.Reload();
        //    MessageBox.Show("清除成功");
        //}

        private void 清理内存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ClearMemory();
            ClearLocalMemory();
        }

        private void 系统设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemSetForm set = new SystemSetForm();
            set.ShowDialog();
        }
       
        #endregion

        #region 退出应用程序
        /// <summary>
        /// 退出应用程序
        /// </summary>
        public void Exit()
        {
                this.Invoke((EventHandler)delegate
                {
                    this.WindowState = FormWindowState.Minimized;
                    this.ShowInTaskbar = false;
                    this.notifyIcon.Visible = false;
                    if (null != secondScreenShowWindow)
                    {
                        secondScreenShowWindow.Hide();
                    }

                    #region 删除历史文件
                    try
                    {
                        new Thread(ClearFilesResources).Start();
                    }
                    catch { }
                    #endregion

                    if (!PublicUtil.Config.安装时间)
                    {
                        if (File.Exists(PublicUtil.Config.下载的更新程序完整路径))
                        {
                            System.Diagnostics.Process myProcess = System.Diagnostics.Process.Start(PublicUtil.Config.下载的更新程序完整路径);
                        }
                    }

                    #region 浏览器关闭处理
                    try
                    {
                        myBrowser.getWebBrowser().CloseDevTools();
                        myBrowser.getWebBrowser().GetBrowser().CloseBrowser(true);
                    }
                    catch { }

                    try
                    {
                        if (myBrowser.getWebBrowser() != null)
                        {
                            //Cef.ClearSchemeHandlerFactories();
                            myBrowser.getWebBrowser().Dispose();
                            Cef.Shutdown();
                        }
                    }
                    catch { }
                    #endregion
                    

                    Process.GetCurrentProcess().Kill();

                });
            
        }
        #endregion

        #region 清理文件
        /// <summary>
        /// 清理文件
        /// </summary>
        public void ClearFilesResources()
        {
            try
            {
                string cacheFileValidityPeriod = ConfigurationManager.AppSettings[AppSettionsType.缓存文件的有效期_天];
                if (cacheFileValidityPeriod != null)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "BrowserCache";
                    DirectoryInfo folder = new DirectoryInfo(path);
                    foreach (FileInfo file in folder.GetFiles("f_*"))
                    {
                        //文件的创建时间
                        var createTime = file.CreationTime;
                        //文件的有效时间线
                        var fileValidityTime = DateTime.Now.AddDays(-Convert.ToDouble(cacheFileValidityPeriod));
                        if (fileValidityTime >= createTime)
                        {
                            if (File.Exists(file.FullName))
                            {
                                File.Delete(file.FullName);
                            }
                        }

                    }

                }
            }
            catch { }

        }
        #endregion


        #region 释放内存
        [DllImport("psapi.dll")]
        static extern int EmptyWorkingSet(IntPtr hwProc);

        #region 清理本机应用程序内存
        /// <summary>
        /// 释放本机应用程序内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                //对于系统进程会拒绝访问，导致出错，此处对异常不进行处理。
                try
                {
                    EmptyWorkingSet(process.Handle);
                }
                catch
                {
                }
            }
        }
        #endregion

        #region 清理本程序的内存
        /// <summary>
        /// 清理本程序的内存
        /// </summary>
        public static void ClearLocalMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            #region 清理主程序内存
            Process current = Process.GetCurrentProcess();
            //对于系统进程会拒绝访问，导致出错，此处对异常不进行处理。
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                //对于系统进程会拒绝访问，导致出错，此处对异常不进行处理。
                try
                {
                    if (process.Id == current.Id)
                    {
                        EmptyWorkingSet(process.Handle);
                    }
                }
                catch
                {
                }
            }
            #endregion

            #region 清理插件内存
            Process[] processCefSharp = Process.GetProcessesByName("CefSharp.BrowserSubprocess");
            foreach (Process process in processCefSharp)
            {
                //对于系统进程会拒绝访问，导致出错，此处对异常不进行处理。
                try
                {
                    EmptyWorkingSet(process.Handle);
                }
                catch
                {
                }
            }
            #endregion

        }
        #endregion

        #endregion


    }
}
