using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace SuperCollectingSilver.com.he.util
{
    /// <summary>
    /// LED客显类
    /// </summary>
    public class LedCustomerDisplay
    {
        #region 成员变量

        public string spPortName;
        private int spBaudRate;
        private StopBits spStopBits;
        private int spDataBits;

        #endregion

        #region 构造函数
        /// <summary>  
        /// 构造函数  
        /// </summary>  
        /// <param name="_spPortName">端口名称（COM1,COM2，COM3...）</param>  
        /// <param name="_spBaudRate">通信波特率（2400,9600....）</param>  
        /// <param name="_spStopBits">停止位</param>  
        /// <param name="_spDataBits">数据位</param>  
        public LedCustomerDisplay(string _spPortName, int _spBaudRate, string _spStopBits, int _spDataBits)
        {
            this.spBaudRate = _spBaudRate;
            this.spDataBits = _spDataBits;
            this.spPortName = _spPortName;
            this.spStopBits = (StopBits)(Enum.Parse(typeof(StopBits), _spStopBits));
        }
        #endregion --构造函数

        #region 公共方法

        #region 缺省型号
        /// <summary>  
        /// 数据信息展现  
        /// </summary>  
        /// <param name="data">发送的数据（清屏可以为null或者空）</param>  
        /// <param name="dispiayType">客显类型</param>  
        public void DisplayData(string data, LedCustomerDispiayType dispiayType)
        {
            SerialPort serialPort = new SerialPort();
            serialPort.PortName = spPortName;
            serialPort.BaudRate = spBaudRate;
            serialPort.StopBits = spStopBits;
            serialPort.DataBits = spDataBits;

            if (!serialPort.IsOpen)
            {
                serialPort.Open();
            }
            serialPort.BaseStream.Flush();

            //先清屏
            serialPort.Write(((char)12).ToString());

            //指示灯
            string str = ((char)27).ToString() + ((char)115).ToString() + (((int)dispiayType)).ToString();
            serialPort.Write(str);


            //发送数据 
            if (!string.IsNullOrEmpty(data))
            {
                serialPort.Write(((char)27).ToString() + ((char)81).ToString() + ((char)65).ToString() + data + ((char)13).ToString());

            }

            serialPort.Close();

        }
        #endregion

        #region VT-VFD8C型号
        /// <summary>  
        /// 数据信息展现  
        /// </summary>  
        /// <param name="data">发送的数据（清屏可以为null或者空）</param>  
        /// <param name="dispiayType">客显类型</param>  
        public void DisplayData(string data, LedCustomerDispiayType_VT_VFD8C dispiayType)
        {
            SerialPort serialPort = new SerialPort();
            serialPort.PortName = spPortName;
            serialPort.BaudRate = spBaudRate;
            serialPort.StopBits = spStopBits;
            serialPort.DataBits = spDataBits;

            if (!serialPort.IsOpen)
            {
                serialPort.Open();
            }
            serialPort.BaseStream.Flush();

            //先清屏
            serialPort.Write(((char)12).ToString());

            //指示灯
            string str = ((char)27).ToString() + ((char)115).ToString() + (((int)dispiayType)).ToString();
            serialPort.Write(str);


            //发送数据 
            if (!string.IsNullOrEmpty(data))
            {
                serialPort.Write(((char)27).ToString() + ((char)81).ToString() + ((char)65).ToString() + data + ((char)13).ToString());

            }

            serialPort.Close();

        }
        #endregion

        #endregion --公共方法
    }


    #region 客显类型

    #region 缺省型号客显类型
    /// <summary>  
    /// 客显类型
    /// </summary>  
    public enum LedCustomerDispiayType
    {
        /// <summary>  
        /// 清屏  
        /// </summary>  
        Clear = 0,
        /// <summary>  
        /// 单价  
        /// </summary>  
        Price = 1,
        /// <summary>  
        /// 总计  
        /// </summary>  
        Total = 2,
        /// <summary>  
        /// 收款  
        /// </summary>  
        Recive = 3,
        /// <summary>  
        /// 找零  
        /// </summary>  
        Change = 4
    }
    #endregion

    #region 分型号客显类型
    /// <summary>  
    /// 客显类型(型号：唯拓VT-VFD8C)  
    /// </summary>  
    public enum LedCustomerDispiayType_VT_VFD8C
    {
        /// <summary>  
        /// 清屏  
        /// </summary>  
        Clear=0,
        /// <summary>  
        /// 单价  
        /// </summary>  
        Price=1,
        /// <summary>  
        /// 总计  
        /// </summary>  
        Total=2,
        /// <summary>  
        /// 收款  
        /// </summary>  
        Recive=3,
        /// <summary>  
        /// 找零  
        /// </summary>  
        Change=4
    }
    #endregion

    #endregion
}


