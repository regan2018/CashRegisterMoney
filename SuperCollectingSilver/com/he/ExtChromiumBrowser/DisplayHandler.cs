using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperCollectingSilver.com.he.ExtChromiumBrowser
{
    class DisplayHandler : IDisplayHandler
    {
        public void OnAddressChanged(IWebBrowser browserControl, AddressChangedEventArgs addressChangedArgs)
        {
        }

        public bool OnConsoleMessage(IWebBrowser browserControl, ConsoleMessageEventArgs consoleMessageArgs)
        {
            return true;
        }

        public void OnFaviconUrlChange(IWebBrowser browserControl, IBrowser browser, IList<string> urls)
        {
        }

        public void OnFullscreenModeChange(IWebBrowser browserControl, IBrowser browser, bool fullscreen)
        {
        }

        public void OnStatusMessage(IWebBrowser browserControl, StatusMessageEventArgs statusMessageArgs)
        {
        }

        public void OnTitleChanged(IWebBrowser browserControl, TitleChangedEventArgs titleChangedArgs)
        {
        }

        public bool OnTooltipChanged(IWebBrowser browserControl, string text)
        {//此方法无法访问
            return false;
        }
    }
}
