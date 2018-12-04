using SuperCollectingSilver.com.he.ExtChromiumBrowser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperCollectingSilver
{
    /// <summary>
    /// 第二屏幕显示的窗体
    /// </summary>
    public partial class SecondScreenShowForm : Form
    {

        public MyChromiumBrowserExtend myBrowser;////浏览器对象
        public Panel panel;

        public SecondScreenShowForm()
        {
            InitializeComponent();
            this.Load += SecondScreenShowForm_Load;
        }

        public void SecondScreenShowForm_Load(object sender, EventArgs e)
        {
            this.panel = new Panel();
            this.Controls.Add(panel);

            var path = Application.StartupPath + "\\HtmlUI\\test.html";
            path = path.Replace("#", "%23");

            myBrowser =new MyChromiumBrowserExtend(this);

            //string baseUrl = "http://119.23.15.8:8080/tty";
            //try
            //{
            //    baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            //}
            //catch (Exception)
            //{
            //    baseUrl = "http://119.23.15.8:8080/tty";
            //}
            //myBrowser.Navigate(path);
        }
    }
}
