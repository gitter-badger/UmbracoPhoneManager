﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Wr.UmbracoPhoneManager.Models;

namespace Wr.UmbracoPhoneManager
{
    public static class XmlHelper
    {
        /// <summary>
        /// Deserialise XPathNavigator item to Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="navigatorItem"></param>
        /// <returns></returns>
        public static List<PhoneManagerCampaignDetail> XPathNavigatorToModel_NOTINUSE(IEnumerable<XPathNavigator> navigatorItems)
        {
            List<PhoneManagerCampaignDetail> result = new List<PhoneManagerCampaignDetail>();
            foreach (var item in navigatorItems)
            {
                var serializer = new XmlSerializer(typeof(PhoneManagerCampaignDetail));
                var textResult = new StringReader(item.OuterXml);
                try
                {
                    var tmp = (PhoneManagerCampaignDetail)serializer.Deserialize(textResult);
                    if (tmp != null)
                        result.Add(tmp);
                }
                catch (Exception ex)
                {

                }
            }

            return result;
        }

        public static List<PhoneManagerCampaignDetail> XPathNavigatorToModel(IEnumerable<XPathNavigator> navigatorItems)
        {
            List<PhoneManagerCampaignDetail> result = new List<PhoneManagerCampaignDetail>();
            StringBuilder sb = new StringBuilder();

            foreach (var item in navigatorItems)
            {
                sb.Append(item.OuterXml);
            }
            if (sb.Length > 0)
            {
                var holder = string.Format("<{0}>{1}</{0}>", AppConstants.UmbracoDocTypeAliases.PhoneManagerModel.PhoneManager, sb.ToString());
                var serializer = new XmlSerializer(typeof(PhoneManagerModel));
                var textResult = new StringReader(holder);
                try
                {
                    var tmp = (PhoneManagerModel)serializer.Deserialize(textResult);
                    if (tmp != null)
                        result = tmp.PhoneManagerCampaignDetail;
                }
                catch (Exception ex)
                {

                }
            }

            return result;
        }

        /// <summary>
        /// Deserialise XPathNavigator items to Model list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="navigatorItem"></param>
        /// <returns></returns>
        public static T XPathNavigatorToModel<T>(XPathNavigator navigatorItem) where T : class, new()
        {
            T result = null; // = new T();
            if (navigatorItem != null)
            {
                var serializer = new XmlSerializer(typeof(T));
                var textResult = new StringReader(navigatorItem.OuterXml);
                try
                {
                    result = (T)serializer.Deserialize(textResult);
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR XPathNavigatorToModel: " + ex.Message);
                }
            }
                        
            return result;
        }

        public static class SerializeXml
        {
            public static XmlDocument ToXML(Object oObject)
            {
                var xns = new XmlSerializerNamespaces();
                xns.Add(string.Empty, string.Empty); // remomve default namespaces
                XmlDocument xmlDoc = new XmlDocument();
                XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, oObject, xns);
                    xmlStream.Position = 0;
                    xmlDoc.Load(xmlStream);
                    return xmlDoc;
                }
            }

            public static String ToXmlString2(Object oObject, bool incEncoding = false)
            {
                var xns = new XmlSerializerNamespaces();
                xns.Add(string.Empty, string.Empty); // remove default namespaces
                XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());

                if (incEncoding)
                {
                    string utf16;
                    using (StringWriter writer = new Utf16StringWriter())
                    {
                        xmlSerializer.Serialize(writer, oObject, xns);
                        utf16 = writer.ToString();
                    }
                    return utf16;
                }
                else
                {
                    XmlDocument xmlDoc = new XmlDocument();

                    using (MemoryStream xmlStream = new MemoryStream())
                    {
                        xmlSerializer.Serialize(xmlStream, oObject, xns);
                        xmlStream.Position = 0;
                        xmlDoc.Load(xmlStream);
                        return xmlDoc.InnerXml;
                    }
                }
            }

            public class Utf16StringWriter : StringWriter
            {
                public override Encoding Encoding { get { return Encoding.Unicode; } }
            }
        }
    }
}