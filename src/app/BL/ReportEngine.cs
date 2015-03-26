using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Xml;
using Codentia.Common.Reporting.DL;

namespace Codentia.Common.Reporting.BL
{
    /// <summary>
    /// ReportEngine class
    /// </summary>
    public static class ReportEngine
    {
        /// <summary>
        /// Sets the name of the data source.
        /// </summary>
        /// <value>The name of the data source.</value>
        public static string DataSourceName
        {
            set
            {
                ReportingData.DataSourceName = value;
            }
        }

        /// <summary>
        /// Get Rdl StringReader
        /// </summary>
        /// <param name="applicationPath">Application Path</param>
        /// <param name="rdlFileName">Name of rdl file (without path or extension)</param>
        /// <param name="dr">A data row for use with tagReplacementString</param>
        /// <param name="tagReplacementString">xml format string for use with dr</param>
        /// <returns>StringReader for the Rdl</returns>
        public static StringReader GetRdlStringReader(string applicationPath, string rdlFileName, DataRow dr, string tagReplacementString)
        {
            string rdlSubFolder = string.Empty;

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReportSubFolder"]))
            {
                rdlSubFolder = string.Format("{0}\\", ConfigurationManager.AppSettings["ReportSubFolder"]);                
            }

            string rdlPath = string.Format("{0}\\{1}{2}.rdl", applicationPath, rdlSubFolder, rdlFileName);            

            XmlDocument rdlDoc = GetRDLAsXMLDoc(rdlPath);

            if (!string.IsNullOrEmpty(tagReplacementString))
            {
                XmlDocument tagReplacementDoc = new XmlDocument();
                tagReplacementDoc.LoadXml(tagReplacementString);

                if (dr != null)
                {
                    rdlDoc = ReplaceTags(rdlDoc, dr, tagReplacementDoc);
                }
            }

            StringReader sr = new StringReader(rdlDoc.InnerXml);
            return sr;
        }

        private static XmlDocument GetRDLAsXMLDoc(string rdlPath)
        {
            StreamReader rdlInputStream = new StreamReader(rdlPath);
            string xml = rdlInputStream.ReadToEnd();
            rdlInputStream.Close();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            
            return doc;
        }

        private static XmlDocument ReplaceTags(XmlDocument rdlDoc, DataRow dr, XmlDocument tagReplacementDoc)
        {            
            // replace the value of the tags in rdlDoc with those values in datarow dr. translated in tagReplacementDoc            
            if (tagReplacementDoc != null)
            {
                IEnumerator ie = tagReplacementDoc.ChildNodes[0].ChildNodes.GetEnumerator();
                while (ie.MoveNext())
                {
                    XmlNode replaceNode = (XmlNode)ie.Current;
                    string elementPath = replaceNode.Attributes["ElementPath"].Value;
                    string attributeName = replaceNode.Attributes["AttributeName"].Value;
                    string dataRowFieldName = replaceNode.Attributes["DataRowFieldName"].Value;

                    XmlNamespaceManager nm = new XmlNamespaceManager(rdlDoc.NameTable);
                    nm.AddNamespace("ns", "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition");

                    string xpath = string.Format("/ns:Report/{0}", elementPath);

                    XmlNode node = rdlDoc.DocumentElement.SelectSingleNode(xpath, nm);

                    if (node != null)
                    {                        
                        if (string.IsNullOrEmpty(attributeName))
                        {
                            node.InnerText = Convert.ToString(dr[dataRowFieldName]);
                        }
                        else
                        {
                            node.Attributes[attributeName].Value = Convert.ToString(dr[dataRowFieldName]);
                        }
                    }                                 
                }                
            }

            return rdlDoc;
        }                
    }
}
