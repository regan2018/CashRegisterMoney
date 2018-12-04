using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCollectingSilver.com.he.ExtChromiumBrowser
{
    class DownloadHandler : IDownloadHandler
    {
        public void OnBeforeDownload(IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    callback.Continue(@"C:\Users\" +
                            System.Security.Principal.WindowsIdentity.GetCurrent().Name +
                            @"\Downloads\" +
                            downloadItem.SuggestedFileName,
                        showDialog: true);
                }
            }
        }

       
        public void OnDownloadUpdated(IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            //throw new NotImplementedException();
        }
        public bool OnDownloadUpdated(CefSharp.DownloadItem downloadItem)
        {
            return false;
        }
    }
}
