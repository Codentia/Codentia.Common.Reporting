using System.Collections.Generic;
using Codentia.Common.Logging.BL;
using NUnit.Framework;

namespace Codentia.Common.Reporting.WebControls.Test
{
    /// <summary>
    /// This class is the unit testing fixture for the CEReport business entity.
    /// </summary>
    [TestFixture]
    public class CEReportViewerTest
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
        public void _TestPDFCreation()
        {
            CEReportViewer mrv = new CEReportViewer();
            mrv.ReportCode = "MULTIDSTEST";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict["DSPARAM1_MDSREP1PAR1"] = 2;
            dict["DSPARAM2_MDSREP2PAR1"] = "<root>THIS SHOULD APPEAR ON PDF</root>";

            mrv.CreatePDF("C:\\test.pdf", dict);
        }
    }
}
