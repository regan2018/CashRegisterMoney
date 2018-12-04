using CefSharp;
using CefSharp.WinForms;
using SuperCollectingSilver.com.he.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperCollectingSilver.com.he.ExtChromiumBrowser
{
    class MenuHandler : IContextMenuHandler
    {
        public static Form mainWindow { get; set; }

        public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            model.Clear();
            model.AddItem(CefMenuCommand.Copy, "复制");
            model.AddItem(CefMenuCommand.Paste, "粘贴");
            model.AddItem(CefMenuCommand.Print, "打印");
        }

        public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            switch (commandId)
            {
                case CefMenuCommand.Copy://复制
                    var selectText= parameters.SelectionText;//获取选中的内容
                    frame.Copy();
                    break;
                case CefMenuCommand.Paste://粘贴
                    frame.Paste();
                    break;
                case CefMenuCommand.Print://打印
                    browser.Print();
                    break;
            }

            return true;
        }

        public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
            
        }

        public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;//返回true则不显示，返回false则显示
        }
    }
}
