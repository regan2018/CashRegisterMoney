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
    /// <summary>
    /// 键盘事件处理类
    /// </summary>
    class KeyBoardHandler : IKeyboardHandler
    {
        public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            if (type == KeyType.KeyUp && Enum.IsDefined(typeof(Keys), windowsKeyCode))
            {
                var key = (Keys)windowsKeyCode;
                switch (key)
                {
                    case Keys.F1://F1事件，打开钱箱
                        PublicUtil.openMoneyBox();
                        break;
                    case Keys.F4://F4事件，释放内存
                        MainForm.ClearMemory();
                        break;
                    case Keys.F5://F5事件，刷新
                        browser.Reload();
                        break;
                    case Keys.F12://F12事件，调出开发者工具
                        browser.ShowDevTools();
                        break;
                }
            }
            return false;
        }

        public bool OnPreKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            return false;
        }
    }
}
