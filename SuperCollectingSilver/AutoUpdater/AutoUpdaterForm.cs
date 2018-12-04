using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESBasic;
using ESBasic.Helpers;
using SuperCollectingSilver.Properties;
using SuperCollectingSilver.com.he.util;
using OAUS.Core;
using SuperCollectingSilver;

namespace AutoUpdater
{
    /// <summary>
    /// 说明：
    /// OAUS使用的是免费版的通信框架ESFramework，最多支持10个人同时在线更新。如果要突破10人限制，请联系 www.oraycn.com
    /// </summary>
    public partial class AutoUpdaterForm : Form
    {
        private Updater updater;    
        private int fileCount = 0; //要升级的文件个数。
        private Timer timer = new Timer();
        private string callBackExeName;   //自动升级完成后，要启动的exe的名称。
        private string callBackPath = ""; //自动升级完成后，要启动的exe的完整路径。        
        private bool startAppAfterClose = false; //关闭升级窗体前，是否启动应用程序。

        private string runProgramName;//当前运行主程序的应用名称

        private MainForm mainForm;//主程序


        public AutoUpdaterForm(string serverIP, int serverPort,string _runProgramName, string title,MainForm _mainForm)
        {
            InitializeComponent();
            this.updater = new Updater(serverIP, serverPort);
            this.updater.ToBeUpdatedFilesCount += new CbGeneric<int>(updater_ToBeUpdatedFilesCount);
            this.updater.UpdateStarted += new CbGeneric(updater_UpdateStarted);
            this.updater.FileToBeUpdated += new CbGeneric<int, string, ulong>(updater_FileToBeUpdated);
            this.updater.CurrentFileUpdatingProgress += new CbGeneric<ulong, ulong>(updater_CurrentFileUpdatingProgress);
            this.updater.UpdateDisruptted += new CbGeneric<string>(updater_UpdateDisruptted);
            this.updater.UpdateCompleted += new CbGeneric(updater_UpdateCompleted);
            this.updater.ConnectionInterrupted += new CbGeneric(updater_ConnectionInterrupted);
            this.updater.UpdateContinued += new CbGeneric(updater_UpdateContinued);

            this.timer.Interval = 1000;
            this.timer.Tick += new EventHandler(timer_Tick);

            //DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);           
            //this.callBackPath = dir.Parent.FullName + "\\" + this.callBackExeName; //自动升级完成后，要启动的exe的完整路径。（1）被分发的程序的可执行文件exe必须位于部署目录的根目录。（2）OAUS的客户端（即整个AutoUpdater文件夹)也必须位于这个根目录。

            this.runProgramName = _runProgramName;
            this.mainForm = _mainForm;

            this.Text = title;
            this.label1.Text = Resources.InitialInformation;

            this.progressBar1.Visible = false;

            this.updater.Start();
                       
        }

        void updater_UpdateContinued()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric(this.updater_UpdateContinued));
            }
            else
            {
                //this.label_reconnect.Visible = false;
                this.label_reconnect.Text = "重连成功，正在续传...";
            }
        }

        void updater_ConnectionInterrupted()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric(this.updater_ConnectionInterrupted));
            }
            else
            {
                this.label_reconnect.Visible = true;
                this.label_reconnect.Text = "连接断开，正在重连中...";
            }
        }

        void updater_UpdateCompleted()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric(this.updater_UpdateCompleted));
            }
            else
            {
                this.label1.Text = Resources.UpdateSuccess;
                this.timer.Start();
            }
        }

        void updater_UpdateDisruptted(string disrupttedType)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CbGeneric<string>(updater_UpdateDisruptted), disrupttedType);
            }
            else
            {
                this.label1.Text = Resources.UpdateFailed;              
                this.label1.ForeColor = Color.Red;                
                MessageBox.Show(Resources.ConnectionInterrupted);
                this.startAppAfterClose = false;
                this.Close();
            }
        }

        void updater_CurrentFileUpdatingProgress(ulong total, ulong transfered)
        {
            this.SetProgress(total, transfered);
        }

        void updater_FileToBeUpdated(int fileIndex, string fileName, ulong fileSize)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CbGeneric<int, string, ulong>(updater_FileToBeUpdated), fileIndex, fileName, fileSize);
            }
            else
            {
                this.label1.Text = string.Format("{0}{1}{2}{3}{4}", Resources.Updateing_string1, this.fileCount, Resources.Updateing_string2, fileIndex + 1, Resources.Updateing_string3);
                this.label2.Text = fileName;

                this.callBackExeName = fileName;
            }
        }

        void updater_UpdateStarted()
        {
            this.ShowMessage(2);
        }

        void updater_ToBeUpdatedFilesCount(int needUpdatedFileCount)
        {
            this.fileCount = needUpdatedFileCount;
            if (needUpdatedFileCount == 0)
            {
                this.ShowMessage(1);
            }
        }      

        private void ShowMessage(int messageType)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<int>(this.ShowMessage), messageType);
            }
            else
            {
                string message = "";
                if (messageType == 1)
                {
                    message = Resources.NoFileNeedUpdate;
                    this.timer.Start();
                }
                if (messageType == 2)
                {
                    message = string.Format("{0}{1}{2}", Resources.NeedUpdate_string1, this.fileCount, Resources.NeedUpdate_string2);
                    this.progressBar1.Visible = true;
                }
                if (messageType == 3)
                {
                    MessageBox.Show(Resources.ConnectionInterrupted);
                    this.startAppAfterClose = false;
                    this.Close();                    
                }

                this.label1.Text = message;
            }
        }       
       
        void timer_Tick(object sender, EventArgs e)
        {
            this.TimeUp();
        }

        private void TimeUp()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric(this.TimeUp));
            }
            else
            {
                this.startAppAfterClose = true;
                this.timer.Stop();
                this.Close();  
            }
        }       

        #region SetProgress
        private DateTime lastShowTime = DateTime.Now;
        /// <summary>
        /// 设置UI显示的进度表。
        /// </summary>       
        private void SetProgress(ulong total, ulong transmitted)
        {
            if (this.InvokeRequired)
            {
                object[] args = { total, transmitted };
                this.BeginInvoke(new CbGeneric<ulong, ulong>(this.SetProgress), args);
            }
            else
            {
                this.progressBar1.Maximum = 1000;
                this.progressBar1.Value = (int)(transmitted * 1000 / total);

                TimeSpan span = DateTime.Now - this.lastShowTime;
                if (span.TotalSeconds >= 1)
                {
                    this.lastShowTime = DateTime.Now;
                    this.label_progress.Text = string.Format("{0}/{1}", PublicHelper.GetSizeString(transmitted), PublicHelper.GetSizeString(total));
                }
            }
        }    
        #endregion     

        #region MainForm_FormClosing
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (null == this.callBackExeName || this.callBackExeName.Length == 0)
                {
                    //MessageBox.Show("文件名不存在，无法启动");
                    return;
                }
                DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                this.callBackPath = dir.Parent.FullName + "\\" + this.callBackExeName; //自动升级完成后，要启动的exe的完整路径。（1）被分发的程序的可执行文件exe必须位于部署目录的根目录。（2）OAUS的客户端（即整个AutoUpdater文件夹)也必须位于这个根目录。
                PublicUtil.Config.下载的更新程序完整路径 = this.callBackPath;

                string processName = this.runProgramName;//this.callBackExeName.Substring(0, this.callBackExeName.Length - 4);
                ESBasic.Helpers.ApplicationHelper.ReleaseAppInstance(processName);

                if (!this.startAppAfterClose)
                {
                    return;
                }

                PublicUtil.Config.安装时间 = DialogResult.Yes == MessageBox.Show("更新程序已下载完成！现在安装最新版请点击“是”，若要在退出时安装请点击“否”。", "新版本安装提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //退出时安装
                if (PublicUtil.Config.安装时间)
                {
                    if (File.Exists(this.callBackPath))
                    {
                        System.Diagnostics.Process myProcess = System.Diagnostics.Process.Start(this.callBackPath);
                        //退出主程序
                        mainForm.Exit();
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }   
        #endregion              
    }
}
