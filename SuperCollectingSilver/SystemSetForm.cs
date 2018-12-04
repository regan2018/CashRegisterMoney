using SuperCollectingSilver.com.he.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using static SuperCollectingSilver.com.he.util.PublicUtil;

namespace SuperCollectingSilver
{
    public partial class SystemSetForm : Form
    {
        
        public SystemSetForm()
        {
            InitializeComponent();
            this.Load += SystemSetForm_Load;
        }

        private void SystemSetForm_Load(object sender, EventArgs e)
        {
            var search = new ManagementObjectSearcher(@"SELECT * FROM Win32_SerialPort");
            //var search = new ManagementObjectSearcher(@"SELECT * FROM Win32_USBHub");
            string[] comPortsNamesArr = SerialPort.GetPortNames();
            List<string> portList = new List<string>();
            for(int i = 0; i < comPortsNamesArr.Length; i++)
            {
                portList.Add(comPortsNamesArr[i]);
            }
            foreach (ManagementObject item in search.Get())
            {
                
                //string value = item["DeviceID"].ToString();
                //portList.Add(item["DeviceID"].ToString());
                portList.Add(item["Name"].ToString());
            }

            #region 钱箱设置处理
            portList.Add(AppSettionsType.自定义COM扩展端口);//打印机端口
            string moneyPort = ConfigurationManager.AppSettings[AppSettionsType.钱箱端口];
            string ledCustomerPort = ConfigurationManager.AppSettings[AppSettionsType.LED客显端口];
            if (portList.Count > 0)
            {
                for (int i = 0; i < portList.Count; i++)
                {
                    //钱箱端口设置
                    this.cmb_port.Items.Add(portList[i]);
                    //led客显端口设置
                    this.com_ledCustomerPort.Items.Add(portList[i]);

                    if (portList[i].Equals(moneyPort))
                    {
                        this.cmb_port.SelectedIndex = i;
                    }
                    if (portList[i].Equals(ledCustomerPort))
                    {
                        this.com_ledCustomerPort.SelectedIndex = i;
                    }
                }
            }

            string moneyBoxType = ConfigurationManager.AppSettings[AppSettionsType.钱箱引脚类型];
            if ("2".Equals(moneyBoxType))
            {
                this.rb_2_line.Select();
            }
            if ("5".Equals(moneyBoxType))
            {
                this.rb_5_line.Select();
            }
            #endregion

            #region 客显设置处理
            this.rb_LEDCustomer.Click += CustomerType_CheckedChanged;
            this.rb_LCDCustomer.Click += CustomerType_CheckedChanged;
            string customerDisplayType = ConfigurationManager.AppSettings[AppSettionsType.客显类型];
            if ("LED".ToUpper().Equals(customerDisplayType.ToUpper())){
                CustomerTypeChange(Config.CustomerDisplayType.LED客显);
            }
            if ("LCD".ToUpper().Equals(customerDisplayType.ToUpper()))
            {
                CustomerTypeChange(Config.CustomerDisplayType.LCD液晶客显);
            }

            string ledCustomerDisplayPortBaudRate = ConfigurationManager.AppSettings[AppSettionsType.LED客显端口通信波特率];
            this.com_LedBaudRate.SelectedText = ledCustomerDisplayPortBaudRate.Trim() ;
            string LedCustomerDisplayDataBits = ConfigurationManager.AppSettings[AppSettionsType.LED客显数据位];
            this.tb_LedDataBits.Text = LedCustomerDisplayDataBits.Trim();

            this.cmb_ledCustomerTestType.SelectedIndex = 0;


            this.cmb_LedCustomerDisplaySpecification.Items.Add(PublicUtil.Config.CustomerDisplaySpecification.VT_VFD8C);
            this.cmb_LedCustomerDisplaySpecification.SelectedItem = ConfigurationManager.AppSettings[AppSettionsType.LED客显型号];

            #endregion


            //var search = new ManagementObjectSearcher(@"root\cimv2", "SELECT * FROM Win32_SerialPort");

            //string moneyPort = ConfigurationManager.AppSettings[AppSettionsType.钱箱端口];
            //if (search.Get().Count > 0)
            //{
            //    foreach (ManagementObject item in search.Get())
            //    {
            //        this.cmb_port.Items.Add(item["Name"]);
            //        if (item["Name"].ToString().Equals(moneyPort))
            //        {
            //            this.cmb_port.SelectedText = item["Name"].ToString();
            //        }
            //    }
            //}
        }

        private void CustomerType_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if ("rb_LCDCustomer".ToUpper().Equals(rb.Name.ToUpper())){
                CustomerTypeChange(Config.CustomerDisplayType.LCD液晶客显);
            }
            if ("rb_LEDCustomer".ToUpper().Equals(rb.Name.ToUpper()))
            {
                CustomerTypeChange(Config.CustomerDisplayType.LED客显);
            }
        }
        /// <summary>
        /// 客显切换处理
        /// </summary>
        /// <param name="type"></param>
        private void CustomerTypeChange(PublicUtil.Config.CustomerDisplayType type)
        {
            if (type==PublicUtil.Config.CustomerDisplayType.LED客显)
            {
                this.rb_LCDCustomer.Checked = false;
                this.rb_LEDCustomer.Checked = true;
                this.gb_ledCustomer.Visible = true;
            }
            if(type==PublicUtil.Config.CustomerDisplayType.LCD液晶客显)
            {
                this.rb_LCDCustomer.Checked = true;
                this.rb_LEDCustomer.Checked = false;
                this.gb_ledCustomer.Visible = false;
            }
        }

        #region 保存设置
        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                string ledCustomerPort = "";//led客显端口
                string customerDisplayType = "LED";//客显类型
                string ledCustomerDisplaySpecification = "";//客显型号
                string ledCustomerDisplayPortBaudRate = "";//led客显端口通信波特率
                string LedDataBits = "";//led客显数据位


                Config.CustomerDisplayType displayType;//客显类型
                if (this.rb_LCDCustomer.Checked)
                {
                    displayType = Config.CustomerDisplayType.LCD液晶客显;
                    customerDisplayType = "LCD";
                }
                if (this.rb_LEDCustomer.Checked)
                {
                    displayType = Config.CustomerDisplayType.LED客显;
                    customerDisplayType = "LED";
                    if (null == this.com_ledCustomerPort.SelectedItem)
                    {
                        MessageBox.Show("请设置LED客显端口");
                        return;
                    }
                    ledCustomerPort = this.com_ledCustomerPort.SelectedItem.ToString();//led客显端口
                    LedDataBits = this.tb_LedDataBits.Text.Trim();
                    if (null== this.cmb_LedCustomerDisplaySpecification.SelectedItem)
                    {
                        MessageBox.Show("请选择客显型号");
                        return;
                    }
                    ledCustomerDisplaySpecification = this.cmb_LedCustomerDisplaySpecification.SelectedItem.ToString();//客显型号
                    try
                    {
                        ledCustomerDisplayPortBaudRate = this.com_LedBaudRate.SelectedItem.ToString().Trim();//led客显端口通信波特率
                    }
                    catch (Exception)
                    {
                        ledCustomerDisplayPortBaudRate = this.com_LedBaudRate.Text.Trim();//led客显端口通信波特率
                    }
                    if (ledCustomerDisplayPortBaudRate.Length == 0)
                    {
                        MessageBox.Show("请设置LED客显端口的通信波特率");
                        return;
                    }
                    if (LedDataBits.Length == 0)
                    {
                        MessageBox.Show("请设置LED客显数据位");
                        return;
                    }
                   
                }
                string moneyBoxType = "2";//钱箱引脚数
                if (this.rb_2_line.Checked)
                {
                    moneyBoxType = "2";
                }
                else if(this.rb_5_line.Checked){
                    moneyBoxType = "5";
                }
                string port = this.cmb_port.SelectedItem.ToString();

                if (PublicUtil.SetConfigValue(AppSettionsType.钱箱端口, port))
                {
                    PublicUtil.SetConfigValue(AppSettionsType.钱箱引脚类型, moneyBoxType);

                    PublicUtil.SetConfigValue(AppSettionsType.客显类型, customerDisplayType);
                    PublicUtil.SetConfigValue(AppSettionsType.LED客显端口, ledCustomerPort);
                    PublicUtil.SetConfigValue(AppSettionsType.LED客显端口通信波特率, ledCustomerDisplayPortBaudRate);
                    PublicUtil.SetConfigValue(AppSettionsType.LED客显数据位, LedDataBits);
                    PublicUtil.SetConfigValue(AppSettionsType.LED客显型号, ledCustomerDisplaySpecification);

                    MessageBox.Show("保存设置成功，软件重启后设置生效！");
                }
                else
                {
                    MessageBox.Show("保存设置失败");
                }
            }catch(Exception ex)
            {
                MessageBox.Show("保存设置出错");
            }
        }
        #endregion

        #region 测试打开钱箱
        /// <summary>
        /// 测试打开钱箱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_test_Click(object sender, EventArgs e)
        {
            if (PublicUtil.openMoneyBox())
            {
                MessageBox.Show("钱箱打开中……");
            }
            else
            {
                MessageBox.Show("钱箱打开失败");
            }
        }
        #endregion

        #region 测试led客显
        /// <summary>
        /// 测试led客显
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LedCustomerDisplay_Click(object sender, EventArgs e)
        {
            string type=this.cmb_ledCustomerTestType.SelectedItem.ToString().Trim();

            LedCustomerDispiayType customerDispiayType = LedCustomerDispiayType.Clear;
            decimal showContent = 0.00M;

            if ("清屏".Equals(type))
            {
                showContent = 0.00M;
                customerDispiayType = LedCustomerDispiayType.Clear;
            }
            if ("单价".Equals(type))
            {
                showContent = 90.50M;
                customerDispiayType = LedCustomerDispiayType.Price;
            }
            if ("总计".Equals(type))
            {
                showContent = 181.00M;
                customerDispiayType = LedCustomerDispiayType.Total;
            }
            if ("收款".Equals(type))
            {
                showContent = 200.00M;
                customerDispiayType = LedCustomerDispiayType.Recive;
            }
            if ("找零".Equals(type))
            {
                showContent = 19.00M;
                customerDispiayType = LedCustomerDispiayType.Change;
            }
            if (PublicUtil.ledCustomerShow(showContent, customerDispiayType))
            {
                MessageBox.Show("显示中……");
            }else
            {
                MessageBox.Show("显示失败");
            }
            
        }
        #endregion
    }
}
