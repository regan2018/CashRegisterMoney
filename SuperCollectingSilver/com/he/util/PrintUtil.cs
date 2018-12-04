using Microsoft.Win32;
using SuperCollectingSilver.com.he.ExtChromiumBrowser;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperCollectingSilver.com.he.util
{
    sealed class PrintUtil
    {
        public static readonly PrintUtil Instance = new PrintUtil();
        static PrintUtil() { }

        private WebBrowser webBrowser;
        private string printFilePath;
        private ActionType actionType;

        private PrintUtil() {
            this.Init();
        }

        private void Init()
        {
            try
            {
                
                this.webBrowser.Dispose();
                GC.Collect();
                //GC.WaitForPendingFinalizers();
                this.webBrowser = null;

            }
            catch (Exception e) { }

            this.webBrowser = new WebBrowser();
            this.webBrowser.Hide();
            this.webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
        }

        /// <summary>
        /// 设置预打印文件的路径，并执行对应的操作
        /// </summary>
        /// <param name="printFilePath">预打印文件的路径（可以是网页地址）</param>
        /// <param name="actionType">操作</param>
        public void SetPrintFilePath(string printFilePath,ActionType actionType)
        {
            this.Init();

            this.actionType = actionType;
            this.printFilePath = printFilePath;
            this.webBrowser.Navigate(printFilePath);
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var mySize= this.webBrowser.Document.Window.Size;
            this.webBrowser.Width = mySize.Width;
            this.webBrowser.Height = mySize.Height;
            switch (actionType)
            {
                case ActionType.直接打印:
                    this.Print();
                    break;
                case ActionType.弹窗打印:
                    this.ShowPrintDialog();
                    break;
                case ActionType.打印设置:
                    this.ShowPageSetupDialog();
                    break;
                case ActionType.打印预览:
                    this.ShowPrintPreviewDialog();
                    break; 
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        private void Print()
        {
            this.webBrowser.Print();

            if (File.Exists(printFilePath))
            {
                File.Delete(printFilePath);
            }
        }

        /// <summary>
        /// 显示打印设置
        /// </summary>
        private void ShowPageSetupDialog()
        {
            try
            {
                PrintDocument fPrintDocument = new PrintDocument();
                RegistryKey reg = Registry.CurrentUser;
                RegistryKey Software = reg.OpenSubKey("Software", true);
                RegistryKey Microsoft = Software.OpenSubKey("Microsoft", true);
                RegistryKey InternetExplorer = Microsoft.OpenSubKey("Internet Explorer", true);
                RegistryKey PageSetup = InternetExplorer.OpenSubKey("PageSetup", true);
                String footer = PageSetup.GetValue("footer").ToString();
                String header = PageSetup.GetValue("header").ToString();
                double margin_bottom = Convert.ToDouble(PageSetup.GetValue("margin_bottom"));
                double margin_left = Convert.ToDouble(PageSetup.GetValue("margin_left"));
                double margin_top = Convert.ToDouble(PageSetup.GetValue("margin_top"));
                double margin_right = Convert.ToDouble(PageSetup.GetValue("margin_right"));
                Margins marg = new Margins();
                marg.Bottom = Convert.ToInt32(margin_bottom);
                marg.Top = Convert.ToInt32(margin_top);
                marg.Left = Convert.ToInt32(margin_left);
                marg.Right = Convert.ToInt32(margin_right);

                fPrintDocument.DefaultPageSettings.Margins = marg;

                PageSetupDialog pageDialog = new PageSetupDialog();
                pageDialog.Document = fPrintDocument;
                pageDialog.AllowPaper = false;
                pageDialog.AllowPrinter = false;
                pageDialog.EnableMetric = true;

                if (DialogResult.OK == pageDialog.ShowDialog())
                {
                    PageSetup.SetValue("margin_top", fPrintDocument.DefaultPageSettings.Margins.Top, RegistryValueKind.String);
                    PageSetup.SetValue("margin_bottom", fPrintDocument.DefaultPageSettings.Margins.Bottom, RegistryValueKind.String);
                    PageSetup.SetValue("margin_left", fPrintDocument.DefaultPageSettings.Margins.Left, RegistryValueKind.String);
                    PageSetup.SetValue("margin_right", fPrintDocument.DefaultPageSettings.Margins.Right, RegistryValueKind.String);
                }

            }
            catch (Exception ex) { }

            //this.webBrowser.ShowPageSetupDialog();
        }

        /// <summary>
        /// 显示打印对话框
        /// </summary>
        private void ShowPrintDialog()
        {
            this.webBrowser.ShowPrintDialog();
        }
        /// <summary>
        /// 显示打印预览
        /// </summary>
        private void ShowPrintPreviewDialog()
        {
            this.webBrowser.ShowPrintPreviewDialog();
        }

    }
}
