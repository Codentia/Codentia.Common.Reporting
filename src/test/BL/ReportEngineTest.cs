using System.Data;
using System.IO;
using System.Xml;
using Codentia.Common.Data;
using Codentia.Common.Logging.BL;
using NUnit.Framework;

namespace Codentia.Common.Reporting.BL.Test
{
    /// <summary>
    /// This class is the unit testing fixture for the ReportEngineTest business entity.
    /// </summary>
    [TestFixture]
    public class ReportEngineTest
    {
        /// <summary>
        /// Ensure all data required during test is set up
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            ReportEngine.DataSourceName = "reporting_sql";
        }

        /// <summary>
        /// Ensure all data entered during test is cleared
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            LogManager.Instance.Dispose();
        }       

        /// <summary>
        /// Scenario: 
        /// Expected: 
        /// </summary>
        [Test]
        public void _001_ReplaceTags()
        {
            DataTable dt = DbInterface.ExecuteQueryDataTable("SELECT '10cm' Height, 'NEWDATASOURCE' DataSource");

            DataRow dr = dt.Rows[0];
            string docTagXml = "<root><ReplaceTag ElementPath=\"ns:InteractiveHeight\" AttributeName=\"\" DataRowFieldName=\"Height\"/>" +
                           "<ReplaceTag ElementPath=\"ns:DataSources/ns:DataSource\" AttributeName=\"Name\" DataRowFieldName=\"DataSource\"/></root>";

            StringReader sr = ReportEngine.GetRdlStringReader(System.Environment.CurrentDirectory, "TestRDL", dr, docTagXml);

            XmlDocument doc = new XmlDocument();
            doc.Load(sr);
            sr.Close();

            XmlNodeList nodeList = doc.DocumentElement.GetElementsByTagName("InteractiveHeight");
            Assert.That(nodeList[0].InnerText, Is.EqualTo("10cm"), "Expected element value of 10cm");

            nodeList = doc.DocumentElement.GetElementsByTagName("DataSource");
            Assert.That(nodeList[0].Attributes["Name"].Value, Is.EqualTo("NEWDATASOURCE"), "Expected element value of NEWDATASOURCE");
        }
    }
}
