using System;
using System.Data;
using Codentia.Common.Data;
using Codentia.Test.Helper;
using NUnit.Framework;

namespace Codentia.Common.Reporting.DL.Test
{
    /// <summary>
    /// Unit testing framework for ReportingData
    /// </summary>
    [TestFixture]
    public class ReportingDataTest
    {
        /// <summary>
        /// Scenario: Check the No of Enums in the database equals the number of project enums
        /// Expected: Process completes without error
        /// </summary>
        [Test]
        public void _001_ReportParameterType_EnumCheck()
        {            
            DataTable dt = DbInterface.ExecuteQueryDataTable("SELECT * FROM ReportParameterType");
            
            // Check count
            string[] enums = Enum.GetNames(typeof(ReportParameterType));

            Assert.That(enums.Length, Is.EqualTo(dt.Rows.Count), "Total count of enums does not match database");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string code = Convert.ToString(dr["ReportParameterTypeCode"]);
                int id = Convert.ToInt32(dr["ReportParameterTypeId"]);

                Assert.That(code, Is.EqualTo(Enum.Format(typeof(ReportParameterType), id, "F")), string.Format("id for {0} incorrect", code));
            }                
        }

        /// <summary>
        /// Scenario: Get All Reports that should appear in a report list
        /// Expected: Process completes without error
        /// </summary>
        [Test]
        public void _002_GetReports()
        {
            int rowCount = DbInterface.ExecuteQueryScalar<int>("SELECT COUNT(ReportId) FROM dbo.Report WHERE OnReportPageList=1");
            DataTable dt = ReportingData.GetReports();
            Assert.That(dt.Rows.Count, Is.EqualTo(rowCount), "Row count does not match");
        }
        
        /// <summary>
        /// Scenario: Run GetParametersForDataSource with an invalid id
        /// Expected: reportId is not valid Exception raised
        /// </summary>
        [Test]
        public void _003_GetParametersForDataSource_InvalidReportId()
        {
            // 0
            Assert.That(delegate { ReportingData.GetParametersForDataSource(0); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("reportDataSourceId: 0 is not valid"));

            // -1
            Assert.That(delegate { ReportingData.GetParametersForDataSource(-1); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("reportDataSourceId: -1 is not valid"));
       
            // mon-existant
            int reportDataSourceIdNonExistant = SqlHelper.GetUnusedIdFromTable("reporting_sql", "ReportDataSource");
            Assert.That(delegate { ReportingData.GetParametersForDataSource(reportDataSourceIdNonExistant); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo(string.Format("reportDataSourceId: {0} does not exist", reportDataSourceIdNonExistant)));
        }

        /// <summary>
        /// Scenario: Get All Parameters for a ReportDataSource
        /// Expected: Process completes without error
        /// </summary>
        [Test]
        public void _005_GetParametersForDataSource_ValidReportDataSourceId()
        {
            int reportDataSourceId = DbInterface.ExecuteQueryScalar<int>("SELECT TOP 1 ReportDataSourceId FROM dbo.ReportDataSource ORDER BY NEWID()");
            int rowCount = DbInterface.ExecuteQueryScalar<int>(string.Format("SELECT COUNT(*) FROM dbo.ReportDataSource_ReportParameter WHERE ReportDataSourceId={0}", reportDataSourceId));

            DataTable dt = ReportingData.GetParametersForDataSource(reportDataSourceId);

            Assert.That(dt.Rows.Count, Is.EqualTo(rowCount), string.Format("Expected row count does not match (reportDataSourceId: {0})", reportDataSourceId));
        }

        /// <summary>
        /// Scenario: RunReportProc with a null or empty string for procedureName
        /// Expected: ProcedureName not specified exception raised
        /// </summary>
        [Test]
        public void _006_RunReportProc_NullOrEmptyProcedureName()
        {
            // null
            Assert.That(delegate { ReportingData.RunReportProc(null, null); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("procedureName is not specified"));

            // empty 
            Assert.That(delegate { ReportingData.RunReportProc(string.Empty, null); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("procedureName is not specified"));
        }

        /// <summary>
        /// Scenario: RunReportProc with null parameters object
        /// Expected: Process runs succesfully
        /// </summary>
        [Test]
        public void _007_RunReportProc_NullParameters()
        {
            DataTable dt = ReportingData.RunReportProc("TestReportSP", null);
            Assert.That(dt.Rows.Count, Is.GreaterThan(0), "Expected row count greater than 0");
        }

        /// <summary>
        /// Scenario: RunReportProc with parameters
        /// Expected: Process runs succesfully
        /// </summary>
        [Test]
        public void _008_RunReportProc_WithParameters()
        {
            DbParameter[] parameters =
                {
                    new DbParameter("@Param1", DbType.Int32),
                    new DbParameter("@Param2", DbType.DateTime),
                    new DbParameter("@Param3", DbType.Xml),
                    new DbParameter("@Param4", DbType.Int32),
                    new DbParameter("@Param5", DbType.Boolean)
                };

            parameters[0].Value = 0;         
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = "<mytest>blahblah</mytest>";         
            parameters[3].Value = 10000;
            parameters[4].Value = true;  
            
            DataTable dt = ReportingData.RunReportProc("TestReportSPWithParameters", parameters);
            Assert.That(dt.Rows.Count, Is.GreaterThan(0), "Expected row count greater than 0");
        }
      
        /// <summary>
        /// Scenario: ReportExists (Id) Negative Tests
        /// Expected: Exceptions raised
        /// </summary>
        [Test]
        public void _009_ReportExists_Id_NegativeTests()
        {
            NUnitHelper.DoNegativeTests("reporting_sql", "Codentia.Common.Reporting.dll", "Codentia.Common.Reporting.DL.ReportingData", "ReportExists", "System.Int32", string.Empty, string.Empty);
        }

        /// <summary>
        /// Scenario: ReportExists (String) Negative Tests
        /// Expected: Exceptions raised
        /// </summary>
        [Test]
        public void _010_ReportExists_String_NegativeTests()
        {
            NUnitHelper.DoNegativeTests("reporting_sql", "Codentia.Common.Reporting.dll", "Codentia.Common.Reporting.DL.ReportingData", "ReportExists", "System.String", string.Empty, string.Empty);            
        }

        /// <summary>
        /// Scenario: Call ReportExists with an invalid Id
        /// Expected: ReportExists returns false
        /// </summary>
        [Test]
        public void _011_ReportExists_InvalidId()
        {
           int reportId = SqlHelper.GetUnusedIdFromTable("reporting_sql", "Report");
           Assert.That(ReportingData.ReportExists(reportId), Is.False, "false expected");
        }

        /// <summary>
        /// Scenario: Call ReportExists with a valid Id
        /// Expected: ReportExists returns true
        /// </summary>
        [Test]
        public void _012_ReportExists_ValidId()
        {
            int reportId = DbInterface.ExecuteQueryScalar<int>("SELECT TOP 1 ReportId FROM dbo.Report ORDER BY NEWID()");
            Assert.That(ReportingData.ReportExists(reportId), Is.True, "true expected");           
        }

        /// <summary>
        /// Scenario: Call ReportExists with a valid code
        /// Expected: ReportExists returns true
        /// </summary>
        [Test]
        public void _013_ReportExists_ValidCode()
        {
            string reportCode = DbInterface.ExecuteQueryScalar<string>("SELECT TOP 1 ReportCode FROM dbo.Report ORDER BY NEWID()");
            Assert.That(ReportingData.ReportExists(reportCode), Is.True, "true expected");           
        }

        /// <summary>
        /// Scenario: GetDataForReport (String) Negative Tests
        /// Expected: Exceptions raised
        /// </summary>
        [Test]
        public void _014_GetDataForReport_NegativeTests()
        {
            NUnitHelper.DoNegativeTests("reporting_sql", "Codentia.Common.Reporting.dll", "Codentia.Common.Reporting.DL.ReportingData", "GetDataForReport", "System.String", "reportId=[Report]", string.Empty);
        }

        /// <summary>
        /// Scenario: Call GetDataForReport with a valid code
        /// Expected: Process runs succesfully
        /// </summary>
        [Test]
        public void _015_GetDataForReport_ValidCode()
        {
            DataTable dt = DbInterface.ExecuteQueryDataTable("SELECT TOP 1 ReportCode, ReportId, ReportName, rdlName, isRdlc, TagReplacementXml, TagReplacementSP FROM dbo.Report ORDER BY NEWID()");
            string reportCode = Convert.ToString(dt.Rows[0]["ReportCode"]);
            int reportId = Convert.ToInt32(dt.Rows[0]["ReportId"]);
            string reportName = Convert.ToString(dt.Rows[0]["ReportName"]);
            string rdlName = Convert.ToString(dt.Rows[0]["RdlName"]);
            bool isRdlc = Convert.ToBoolean(dt.Rows[0]["isRdlc"]);
            string tagReplacementXml = Convert.ToString(dt.Rows[0]["TagReplacementXml"]);
            string tagReplacementSP = Convert.ToString(dt.Rows[0]["TagReplacementSP"]);

            DataTable dtResult = ReportingData.GetDataForReport(reportCode);

            int testReportId = Convert.ToInt32(dtResult.Rows[0]["ReportId"]);
            string testReportName = Convert.ToString(dtResult.Rows[0]["ReportName"]);
            string testRdlName = Convert.ToString(dtResult.Rows[0]["RdlName"]);
            bool testIsRdlc = Convert.ToBoolean(dtResult.Rows[0]["isRdlc"]);
            string testTagReplacementXml = Convert.ToString(dtResult.Rows[0]["TagReplacementXml"]);
            string testTagReplacementSP = Convert.ToString(dtResult.Rows[0]["TagReplacementSP"]);

            Assert.That(reportId, Is.EqualTo(testReportId), "testReportId does not match");
            Assert.That(reportName, Is.EqualTo(testReportName), "testReportName does not match");            
            Assert.That(rdlName, Is.EqualTo(testRdlName), "testRdlName does not match");
            Assert.That(isRdlc, Is.EqualTo(testIsRdlc), "testIsRdlc does not match");
            Assert.That(tagReplacementXml, Is.EqualTo(testTagReplacementXml), "testTagReplacementXml does not match");
            Assert.That(tagReplacementSP, Is.EqualTo(testTagReplacementSP), "testTagReplacementSP does not match");
        }

        /// <summary>
        /// Scenario: Run GetDataSourcesForReport with an invalid id
        /// Expected: reportId is not valid Exception raised
        /// </summary>
        [Test]
        public void _017_GetDataSourcesForReport_InvalidReportId()
        {
            NUnitHelper.DoNegativeTests("reporting_sql", "Codentia.Common.Reporting.dll", "Codentia.Common.Reporting.DL.ReportingData", "GetDataSourcesForReport", "reportId=[Report]", string.Empty);           
        }
       
        /// <summary>
        /// Scenario: Get All DataSources for a Report
        /// Expected: Process completes without error
        /// </summary>
        [Test]
        public void _018_GetDataSourcesForReport_ValidReportDataSourceId()
        {
            int reportId = DbInterface.ExecuteQueryScalar<int>("SELECT TOP 1 ReportId FROM dbo.Report ORDER BY NEWID()");
            int rowCount = DbInterface.ExecuteQueryScalar<int>(string.Format("SELECT COUNT(*) FROM dbo.ReportDataSource WHERE ReportId={0}", reportId));

            DataTable dt = ReportingData.GetDataSourcesForReport(reportId);

            Assert.That(dt.Rows.Count, Is.EqualTo(rowCount), string.Format("Expected row count does not match (reportId: {0})", reportId));
        }
    }
}
