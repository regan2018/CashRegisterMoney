using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace OAUS.Core
{
    /// <summary>
    /// 给应用的客户端使用，用于获取升级的版本信息。
    /// </summary>
    public static class VersionHelper
    {
        /// <summary>
        /// 获取当前客户端的版本号。
        /// </summary>        
        public static int GetCurrentVersion()
        {
            //string path = AppDomain.CurrentDomain.BaseDirectory + "UpdateConfiguration.xml";
            string path = UpdateConfiguration.ConfigurationPath;

            #region 没有升级版本的xml配置文件时，生成一个配置文件
            if (!File.Exists(path))
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement objXml= xmlDoc.CreateElement("object");
                objXml.SetAttribute("type", "OAUS.Core.UpdateConfiguration,OAUS.Core");

                xmlDoc.AppendChild(objXml);

                XmlElement property = xmlDoc.CreateElement("property");
                property.SetAttribute("name", "ConfigurationPath");
                property.SetAttribute("value", path);
                objXml.AppendChild(property);

                property = xmlDoc.CreateElement("property");
                property.SetAttribute("name", "FileList");

                XmlElement list = xmlDoc.CreateElement("list");
                list.SetAttribute("element-type", "OAUS.Core.FileUnit,OAUS.Core");
                property.AppendChild(list);
                objXml.AppendChild(property);

                property = xmlDoc.CreateElement("property");
                property.SetAttribute("name", "ClientVersion");
                property.SetAttribute("value", "1");
                objXml.AppendChild(property);

                xmlDoc.Save(path);

            }
            #endregion

            UpdateConfiguration config = (UpdateConfiguration)UpdateConfiguration.Load(path);
            return config.ClientVersion;
        }

        /// <summary>
        /// 从服务端获得最新客户端的版本号。【前提是服务端的配置项RemotingServiceEnabled必需为true】
        /// </summary>
        /// <param name="oausServerIP">OAUS服务端的IP</param>
        /// <param name="oausServerPort">OAUS服务端的端口</param>        
        public static int GetLatestVersion(string oausServerIP, int oausServerPort)
        {
            IOausService service = (IOausService)Activator.GetObject(typeof(IOausService), string.Format("tcp://{0}:{1}/OausService", oausServerIP, oausServerPort+2));
            return service.GetLatestVersion();        
        }

        /// <summary>
        /// 是否有新版本？【前提是服务端的配置项RemotingServiceEnabled必需为true】
        /// </summary>
        /// <param name="oausServerIP">OAUS服务端的IP</param>
        /// <param name="oausServerPort">OAUS服务端的端口</param>        
        public static bool HasNewVersion(string oausServerIP, int oausServerPort)
        {
            return VersionHelper.GetLatestVersion(oausServerIP, oausServerPort) > VersionHelper.GetCurrentVersion();
        }
    }
}
