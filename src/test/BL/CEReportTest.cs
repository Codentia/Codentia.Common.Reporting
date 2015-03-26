using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;
using Codentia.Common.Data;
using Codentia.Common.Logging.BL;
using Codentia.Test.Helper;
using Microsoft.Reporting.WebForms;
using NUnit.Framework;

namespace Codentia.Common.Reporting.BL.Test
{
    /// <summary>
    /// This class is the unit testing fixture for the CEReport business entity.
    /// </summary>
    [TestFixture]
    public class CEReportTest
    {
        /// <summary>
        /// Ensure all data required during test is set up
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
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
        /// Scenario: Attempt to create CEReport with null or empty reportCode
        /// Expected: reportCode not specified Exception raised
        /// </summary>
        [Test]
        public void _001_Constructor_NullOrEmptyReportCode()
        {            
            CEReport rpt;

            // null
            Assert.That(delegate { rpt = new CEReport(null); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("reportCode is not specified"));

            // empty 
            Assert.That(delegate { rpt = new CEReport(null); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("reportCode is not specified"));
       
            // non-existant
            Assert.That(delegate { rpt = new CEReport("NONEXISTANTCODE"); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("reportCode: NONEXISTANTCODE does not exist"));
        }

        /// <summary>
        /// Scenario: Object created with valid reportCode
        /// Expected: Object created succesfully
        /// </summary>
        [Test]
        public void _002_Constructor_ReportWithADataSourceWithParameters_ValidReportCode()
        {
            DataTable dt = DbInterface.ExecuteQueryDataTable("SELECT TOP 1 ReportId, ReportCode, ReportName, RdlName, IsRdlc FROM dbo.Report WHERE ReportCode='REP1'");

            string reportCode = Convert.ToString(dt.Rows[0]["ReportCode"]);
            int reportId = Convert.ToInt32(dt.Rows[0]["ReportId"]);
            string reportName = Convert.ToString(dt.Rows[0]["ReportName"]);
            string rdlName = Convert.ToString(dt.Rows[0]["RdlName"]);
            bool isRdlc = Convert.ToBoolean(dt.Rows[0]["IsRdlc"]);

            CEReport mr = new CEReport(reportCode);

            Assert.That(mr.ReportCode, Is.EqualTo(reportCode), "ReportCode does not match");
            Assert.That(mr.ReportName, Is.EqualTo(reportName), "ReportName does not match");
            Assert.That(mr.ReportId, Is.EqualTo(reportId), "ReportId does not match");
            Assert.That(mr.RdlName, Is.EqualTo(rdlName), "rdlName does not match");
            Assert.That(mr.IsRdlc, Is.EqualTo(isRdlc), "IsRdlc does not match");    
        
            // Check Data Sources
            DataTable dtDS = DbInterface.ExecuteQueryDataTable(string.Format("SELECT ReportDataSourceId, ReportDataSourceCode, ReportDataSourceSP FROM dbo.ReportDataSource WHERE ReportId={0}", reportId));

            Assert.That(mr.CEReportDataSources.Count, Is.EqualTo(dtDS.Rows.Count), "Data source count does not match");

            for (int i = 0; i < dtDS.Rows.Count; i++)
            {
                string reportDataSourceCode = Convert.ToString(dtDS.Rows[i]["ReportDataSourceCode"]);
                string reportDataSourceSP = Convert.ToString(dtDS.Rows[i]["ReportDataSourceSP"]);
                int reportDataSourceId = Convert.ToInt32(dtDS.Rows[i]["ReportDataSourceId"]);

                CEReportDataSource rds = new CEReportDataSource(reportDataSourceId, reportDataSourceCode, reportDataSourceSP);

                Assert.That(rds.ReportDataSourceCode, Is.EqualTo(reportDataSourceCode), "ReportDataSourceCode does not match");
                Assert.That(rds.ReportDataSourceSP, Is.EqualTo(reportDataSourceSP), "ReportDataSourceSP does not match");
            }
  
            // Set Data Source Parameter Values
            IEnumerator<string> ie = mr.CEReportDataSources.Keys.GetEnumerator();

            Dictionary<string, object> reportValues = new Dictionary<string, object>();
            DateTime testDate = DateTime.Now;

            while (ie.MoveNext())
            {
                CEReportDataSource rds = mr.CEReportDataSources[ie.Current];
                DataTable dtParam = DbInterface.ExecuteQueryDataTable(string.Format("SELECT * FROM dbo.ReportDataSource_ReportParameter WHERE ReportDataSourceId={0}", rds.ReportDataSourceId));

                for (int i = 0; i < dtParam.Rows.Count; i++)
                {
                    DataRow dr = dtParam.Rows[i];
                  
                    int paramOrder = Convert.ToInt32(dr["ParameterOrder"]);
                    Assert.That(paramOrder, Is.EqualTo(i), "paramOrder does not match");
                    int reportParamId = Convert.ToInt32(dr["ReportParameterId"]);

                    DataTable dtRptParam = DbInterface.ExecuteQueryDataTable(string.Format("SELECT TOP 1 ReportParameterCode, SqlDbTypeCode  FROM dbo.ReportParameter rp INNER JOIN dbo.SqlDbType dt ON dt.SqlDbTypeId=rp.SqlDbTypeId WHERE ReportParameterId={0}", reportParamId));
                    DataRow dr2 = dtRptParam.Rows[0];
                    string paramCode = Convert.ToString(dr2["ReportParameterCode"]);

                    string fullParamCode = string.Format("{0}_{1}", rds.ReportDataSourceCode, paramCode);
  
                    Assert.That(rds.CEReportParameters[fullParamCode].ParameterCode, Is.EqualTo(paramCode), "ParameterCode does not match");
                    string sqlDBTypeCode = Convert.ToString(dr2["SqlDbTypeCode"]);
                    switch (sqlDBTypeCode)
                    {
                        case "String":
                            reportValues[fullParamCode] = "MyTestString";
                            break;
                        case "Xml":
                            reportValues[fullParamCode] = "<mytest>1</mytest>";
                            break;
                        case "Int32":
                            reportValues[fullParamCode] = 1234;
                            break;
                        case "DateTime":
                            reportValues[fullParamCode] = testDate;
                            break;
                        case "Boolean":
                            reportValues[fullParamCode] = true;
                            break;
                        default: throw new Exception("Unsupported dbTypeCode");
                    }
                }
            }

            // Test values
            DataSet ds = mr.GetReportResultDataSet(reportValues);
            int reportParameterCount = 0;

            IEnumerator<string> ieDS = mr.CEReportDataSources.Keys.GetEnumerator();
            while (ieDS.MoveNext())
            {
                CEReportDataSource rds = mr.CEReportDataSources[ieDS.Current];

                DataTable dtParam = DbInterface.ExecuteQueryDataTable(string.Format("SELECT * FROM dbo.ReportDataSource_ReportParameter WHERE ReportDataSourceId={0}", rds.ReportDataSourceId));

                Assert.That(rds.CEReportParameters.Count, Is.EqualTo(dtParam.Rows.Count), "Parameter count does not match");
     
                DataTable dtResult2 = rds.ResultTable;
                Assert.That(dtResult2.Rows.Count, Is.GreaterThan(0), "A row count greater than 0 expected");
                DataTable dtDataSetTable = ds.Tables[rds.ReportDataSourceCode];

                SqlHelper.CompareDataTables(dtDataSetTable, dtResult2);

                IEnumerator<string> ieP = rds.CEReportParameters.Keys.GetEnumerator();
                while (ie.MoveNext())
                {
                    CEReportParameter rptParam = rds.CEReportParameters[ieP.Current];
                    switch (rptParam.DbType)
                    {
                        case DbType.Int32:
                            Assert.That(rptParam.Value, Is.EqualTo(1234), "Int value does not match");
                            break;
                        case DbType.String:
                            Assert.That(rptParam.Value, Is.EqualTo("MyTestString"), "NVarChar value does not match");
                            break;
                        case DbType.Xml:
                            Assert.That(rptParam.Value, Is.EqualTo("<mytest>1</mytest>"), "Xml value does not match");
                            break;
                        case DbType.DateTime:
                            Assert.That(rptParam.Value, Is.EqualTo(testDate), "DateTime value does not match");
                            break;
                        case DbType.Boolean:
                            Assert.That(rptParam.Value, Is.True, "Boolean value does not match");
                            break;
                        default: throw new Exception("Unsupported sqlDBTypeCode");
                    }                    
                }

                // Check SqlParameter Array
                DbParameter[] sqlArray = rds.DbParameters;
                for (int j = 0; j < sqlArray.Length; j++)
                {
                    DbParameter sqlParam = sqlArray[j];
                    switch (sqlParam.DbType)
                    {
                        case DbType.Int32:
                            Assert.That(sqlParam.Value, Is.EqualTo(1234), "Int value does not match");
                            break;
                        case DbType.String:
                            Assert.That(sqlParam.Value, Is.EqualTo("MyTestString"), "NVarChar value does not match");
                            break;
                        case DbType.Xml:
                            Assert.That(sqlParam.Value, Is.EqualTo("<mytest>1</mytest>"), "Xml value does not match");
                            break;
                        case DbType.DateTime:
                            Assert.That(sqlParam.Value, Is.EqualTo(testDate), "DateTime value does not match");
                            break;
                        case DbType.Boolean:
                            Assert.That(sqlParam.Value, Is.True, "Boolean value does not match");
                            break;
                        default: throw new Exception("Unsupported DBTypeCode");
                    }
                }

                IEnumerator<string> ie2 = rds.CEReportParameters.Keys.GetEnumerator();
                ReportParameter[] repArray = rds.ReportParameters;
                Assert.That(repArray.Length, Is.EqualTo(rds.CEReportParameters.Count), "ReportParameters Array count differs");
                int k = 0;
                while (ie2.MoveNext())
                {
                    CEReportParameter rptParam = rds.CEReportParameters[ie2.Current];
                    string testDateString = Convert.ToString(testDate);
                    switch (rptParam.DbType)
                    {
                        case DbType.Int32:
                            Assert.That(rptParam.ReportParameter.Values[0], Is.EqualTo("1234"), "Int value does not match (ReportParameter)");
                            Assert.That(rptParam.ReportParameter.Values[0], Is.EqualTo(repArray[k].Values[0]), "Int value does not match (ReportParameter)");
                            break;
                        case DbType.String:
                            Assert.That(rptParam.ReportParameter.Values[0], Is.EqualTo("MyTestString"), "NVarChar value does not match (ReportParameter)");
                            Assert.That(rptParam.ReportParameter.Values[0], Is.EqualTo(repArray[k].Values[0]), "NVarChar value does not match (ReportParameter)");
                            break;
                        case DbType.Xml:
                            Assert.That(rptParam.ReportParameter.Values[0], Is.EqualTo("<mytest>1</mytest>"), "Xml value does not match (ReportParameter)");
                            Assert.That(rptParam.ReportParameter.Values[0], Is.EqualTo(repArray[k].Values[0]), "Xml value does not match (ReportParameter)");
                            break;
                        case DbType.DateTime:
                            Assert.That(rptParam.ReportParameter.Values[0], Is.EqualTo(testDateString), "DateTime value does not match (ReportParameter)");
                            Assert.That(rptParam.ReportParameter.Values[0], Is.EqualTo(repArray[k].Values[0]), "DateTime value does not match (ReportParameter)");
                            break;
                        case DbType.Boolean:
                            Assert.That(rptParam.ReportParameter.Values[0], Is.EqualTo("True"), "Boolean value does not match (ReportParameter)");
                            Assert.That(rptParam.ReportParameter.Values[0], Is.EqualTo(repArray[k].Values[0]), "Boolean value does not match (ReportParameter)");
                            break;
                        default: throw new Exception("Unsupported sqlDBTypeCode");
                    }

                    k++;
                    reportParameterCount++;
                }
             }

             Assert.That(mr.CollatedReportParameters.Length, Is.EqualTo(reportParameterCount), string.Format("CollatedReportParameters expected to be {0}", reportParameterCount));

            // test rdlc set
            mr.IsRdlc = true;
            string errorMessage = string.Empty;
            try
            {
                int i = mr.CollatedReportParameters.Length;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        /// <summary>
        /// Scenario: Object created with valid reportCode
        /// Expected: Object created succesfully
        /// </summary>
        [Test]
        public void _004_Constructor_ReportWithADataSourceWithoutParameters_ValidReportCode()
        {
            DataTable dt = DbInterface.ExecuteQueryDataTable("SELECT TOP 1 ReportId, ReportCode, ReportName, RdlName, IsRdlc FROM dbo.Report WHERE ReportId NOT IN (SELECT ReportId FROM ReportDataSource WHERE ReportDataSourceId IN (SELECT ReportDataSourceId FROM ReportDataSource_ReportParameter)) ORDER BY NEWID()");

            Assert.That(dt.Rows.Count, Is.EqualTo(1), "Expected 1 row");

            string reportCode = Convert.ToString(dt.Rows[0]["ReportCode"]);
            int reportId = Convert.ToInt32(dt.Rows[0]["ReportId"]);
            string reportName = Convert.ToString(dt.Rows[0]["ReportName"]);
            string rdlName = Convert.ToString(dt.Rows[0]["RdlName"]);
            bool isRdlc = Convert.ToBoolean(dt.Rows[0]["isRdlc"]);

            CEReport mr = new CEReport(reportCode);

            Assert.That(mr.ReportCode, Is.EqualTo(reportCode), "ReportCode does not match");
            Assert.That(mr.ReportName, Is.EqualTo(reportName), "ReportName does not match");
            Assert.That(mr.ReportId, Is.EqualTo(reportId), "ReportId does not match");
            Assert.That(mr.RdlName, Is.EqualTo(rdlName), "rdlName does not match");

            IEnumerator<string> ieDS = mr.CEReportDataSources.Keys.GetEnumerator();

            while (ieDS.MoveNext())
            {
                CEReportDataSource rds = mr.CEReportDataSources[ieDS.Current];
                Assert.That(rds.CEReportParameters.Count, Is.EqualTo(0), "Expected no parameters");
            }
        }

        /// <summary>
        /// Scenario: Run GetRdlStringReader but as an rdlc 
        /// Expected: rdl reports only exception raised
        /// </summary>
        [Test]
        public void _005_GetRdlStringReader_Rdlc()
        {
            CEReport mr = new CEReport("IMAGETEST");
            mr.IsRdlc = true;
            StringReader sr = new StringReader(string.Empty);

            Assert.That(delegate { sr = mr.GetRdlStringReader(System.Environment.CurrentDirectory); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("GetRdlStringReader can only be used for rdl reports"));
        }

        /// <summary>
        /// Scenario: Run GetRdlStringReader which will replace an tags in an rdl definition
        /// Expected: The tag to be replaced has a non-empty value
        /// </summary>
        [Test]
        public void _006_GetRdlStringReader_Rdl()
        {
            CEReport mr = new CEReport("IMAGETEST");
            StringReader sr = mr.GetRdlStringReader(System.Environment.CurrentDirectory);
            XmlDocument doc = new XmlDocument();
            doc.Load(sr);
            sr.Close();

            XmlNodeList nodeList = doc.DocumentElement.GetElementsByTagName("Value");
            Assert.That(nodeList[0].InnerText, Is.Not.EqualTo(string.Empty), "Expected element with a value");
        }

        /// <summary>
        /// Scenario: Get ParametersAreRenderedProperty where at least one parameter has IsRendered=true
        /// Expected: ParametersAreRendered is returned as true
        /// </summary>
        [Test]
        public void _007_CEReport_AtLeastOneParameterIsRendered()
        {
            CEReport mr = new CEReport("REP1");
            Assert.That(mr.ParametersAreRendered, Is.True, "true expected");
        }

        /// <summary>
        /// Scenario: Get ParametersAreRenderedProperty where all parameter have IsRendered=false
        /// Expected: ParametersAreRendered is returned as false
        /// </summary>
        [Test]
        public void _008_CEReport_AllParametersAreNotRendered()
        {
            CEReport mr = new CEReport("NOVISIBLEPARAMS");
            Assert.That(mr.ParametersAreRendered, Is.False, "false expected");
        }
    }
}
