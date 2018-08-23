using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MonitorSwitcher
{
    public class ConfigHandler
    {
        private readonly string configFileName = "ConfigFiles\\MonitorSwitcherConfig.xml";
        private readonly XmlDocument doc = new XmlDocument();
        private readonly string rootElementName = "ConfigurationFile";
        private readonly string configElementName = "Config";

        public ConfigHandler()
        {
            try
            {
                doc.Load(configFileName);
            }
            catch (Exception)
            {
                CreateConfig();
            }
        }

        public void AddConfigElement(string configName)
        {
            var root = doc.SelectSingleNode(rootElementName);
            var configElement = doc.CreateElement(configElementName);
            configElement.InnerText = configName;
            root.AppendChild(configElement);
            doc.Save(configFileName);
        }

        public void DeleteConfig(string configName)
        {
            var rootElement = doc.SelectSingleNode(rootElementName);
            var rootNodes = rootElement.SelectNodes(configElementName);
            foreach (XmlNode configElement in rootNodes)
            {
                if(configElement.InnerText == configName)
                {
                    rootElement.RemoveChild(configElement);
                    break;
                }
            }
            doc.Save(configFileName);
        }

        public List<string> GetConfigElements()
        {
            List<string> configElements = new List<string>();

            var rootNodes = doc.SelectSingleNode(rootElementName).SelectNodes(configElementName);
            foreach(XmlNode configElement in rootNodes)
            {
                configElements.Add(configElement.InnerText);
            }
            return configElements;
        }

        private void CreateConfig()
        {
            XmlNode node = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(node);

            XmlNode rootElement = doc.CreateElement(rootElementName);
            doc.AppendChild(rootElement);

            doc.Save(configFileName);
        }
    }
}
