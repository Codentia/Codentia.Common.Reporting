using System;
using System.Data;
using Codentia.Common.Data;
using Codentia.Common.Logging.BL;
using Microsoft.Reporting.WebForms;
using NUnit.Framework;

namespace Codentia.Common.Reporting.BL.Test
{
    /// <summary>
    /// This class is the unit testing fixture for the CEReport business entity.
    /// </summary>
    [TestFixture]
    public class CEReportParameterTest
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
        /// Scenario: Object created with null or empty parameterCode
        /// Expected: parameterCode not specified Exception raised
        /// </summary>
        [Test]
        public void _001_Constructor_NullorEmptyParameterCode()
        {           
            CEReportParameter rp;

            // null
            Assert.That(delegate { rp = new CEReportParameter(null, string.Empty, string.Empty, string.Empty, 0, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, false); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("parameterCode is not specified"));

            // empty 
            Assert.That(delegate { rp = new CEReportParameter(null, string.Empty, string.Empty, string.Empty, 0, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, false); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("parameterCode is not specified"));
        }

        /// <summary>
        /// Scenario: Object created with null or empty parameterName
        /// Expected: parameterName not specified Exception raised
        /// </summary>
        [Test]
        public void _002_Constructor_NullorEmptyParameterName()
        {            
            CEReportParameter rp;

            // null
            Assert.That(delegate { rp = new CEReportParameter("blah", null, string.Empty, string.Empty, 0, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, false); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("parameterName is not specified"));

            // empty 
            Assert.That(delegate { rp = new CEReportParameter("blah", string.Empty, string.Empty, string.Empty, 0, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, false); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("parameterName is not specified"));
        }

        /// <summary>
        /// Scenario: Object created with null or empty caption
        /// Expected: caption not specified Exception raised
        /// </summary>
        [Test]
        public void _003_Constructor_NullorEmptyCaption()
        {            
            CEReportParameter rp;

            // null
            Assert.That(delegate { rp = new CEReportParameter("blah", "blah", null, string.Empty, 0, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, true); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("caption is not specified"));

            // empty 
            Assert.That(delegate { rp = new CEReportParameter("blah", "blah", string.Empty, string.Empty, 0, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, true); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("caption is not specified"));
        }

        /// <summary>
        /// Scenario: Object created with null or empty DbTypeCode
        /// Expected: DbTypeCode not specified Exception raised
        /// </summary>
        [Test]
        public void _004_Constructor_NullorEmptyDbTypeCode()
        {            
            CEReportParameter rp;
            
            // null
            Assert.That(delegate { rp = new CEReportParameter("blah", "blah", "blah", null, 0, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, false); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("dbTypeCode is not specified"));

            // empty 
            Assert.That(delegate { rp = new CEReportParameter("blah", "blah", "blah", string.Empty, 0, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, false); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("dbTypeCode is not specified"));        
        
            // non-existant
            Assert.That(delegate { rp = new CEReportParameter("blah", "blah", "blah", "NONEXISTANTTYPE", 0, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, false); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("dbTypeCode: NONEXISTANTTYPE does not exist"));                    
        }

        /// <summary>
        /// Scenario: Object created with an unsupported dbTypeCode
        /// Expected: DbType not yet supported Exception raised
        /// </summary>
        [Test]
        public void _005_Constructor_UnsupportedDbTypeCode()
        {                        
            Assert.That(delegate { CEReportParameter rp = new CEReportParameter("blah", "blah", "blah", "Int16", 0, "CheckBox", string.Empty, string.Empty, string.Empty, false, "test message", false); }, Throws.InstanceOf<NotSupportedException>().With.Message.EqualTo("DbType: Int16 not yet supported"));                    
        }

        /// <summary>
        /// Scenario: Object created with a non existant dbTypeCode
        /// Expected: dbTypeCode does not exist Exception raised
        /// </summary>
        [Test]
        public void _006_Constructor_ExistantDBTypeCodeInvalidSize()
        {            
            CEReportParameter rp;
            
            // 0
            Assert.That(delegate { rp = new CEReportParameter("blah", "blah", "blah", "String", 0, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, false); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("dbTypeSize must be at least 1"));

            // -1
            Assert.That(delegate { rp = new CEReportParameter("blah", "blah", "blah", "String", -1, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, false); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("dbTypeSize must be at least 1"));        
        }

        /// <summary>
        /// Scenario: Object created with null or empty reportParameterTypeCode
        /// Expected: reportParameterTypeCode not specified Exception raised
        /// </summary>
        [Test]
        public void _007_Constructor_NullorEmptyReportParameterTypeCode()
        {            
            CEReportParameter rp;

            // null
            Assert.That(delegate { rp = new CEReportParameter("blah", "blah", "blah", "Int32", 0, null, string.Empty, string.Empty, string.Empty, false, string.Empty, false); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("reportParameterTypeCode is not specified"));

            // empty 
            Assert.That(delegate { rp = new CEReportParameter("blah", "blah", "blah", "Int32", 0, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, false); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("reportParameterTypeCode is not specified"));

            // non-existant
            Assert.That(delegate { rp = new CEReportParameter("blah", "blah", "blah", "Int32", 0, "NONEXISTANTTYPE", string.Empty, string.Empty, string.Empty, false, string.Empty, false); }, Throws.InstanceOf<ArgumentException>().With.Message.EqualTo("reportParameterTypeCode: NONEXISTANTTYPE does not exist"));                    
        }       

        /// <summary>
        /// Scenario: CEReportParameter Object created with valid parameters
        /// Expected: Object is created successfully
        /// </summary>
        [Test]
        public void _008_Constructor_ValidParams()
        {
            CEReportParameter rp = new CEReportParameter("blah", "ParamName1", "MyCaption", "Int32", 0, "CheckBox", "AnySP", "0:True Option;1:False Option", "MyDefault", false, "test message", false);
           
            // check properties
            Assert.That(rp.ParameterCode, Is.EqualTo("blah"), "ParameterCode does not match");
            Assert.That(rp.ParameterName, Is.EqualTo("ParamName1"), "ParameterName does not match");
            Assert.That(rp.Caption, Is.EqualTo("MyCaption"), "Caption does not match");
            Assert.That(rp.DbType, Is.EqualTo(DbType.Int32), "DbType does not match");
            Assert.That(rp.DbTypeSize, Is.EqualTo(0), "DbTypeSize does not match");
            Assert.That(rp.ReportParameterType, Is.EqualTo(ReportParameterType.CheckBox), "ReportParameterType does not match");
            Assert.That(rp.ParameterSourceSP, Is.EqualTo("AnySP"), "ParameterSourceSP does not match");
            Assert.That(rp.ParameterSourceValues, Is.EqualTo("0:True Option;1:False Option"), "ParameterSourceValues does not match");
            Assert.That(rp.DefaultValue, Is.EqualTo("MyDefault"), "DefaultValue does not match");
            Assert.That(rp.IsNullAllowed, Is.False, "IsNullAllowed does not match");
            Assert.That(rp.ErrorMessageCaption, Is.EqualTo("test message"), "ErrorMessageCaption does not match");
            Assert.That(rp.IsRendered, Is.False, "IsRendered does not match");

            CEReportParameter rp2 = new CEReportParameter("blah", "ParamName2", "MyCaption2", "String", 10, "CheckBox", "AnySP", string.Empty, string.Empty, true, "MyTestDate", true);

            // check properties
            Assert.That(rp2.ParameterCode, Is.EqualTo("blah"), "ParameterCode does not match");
            Assert.That(rp2.ParameterName, Is.EqualTo("ParamName2"), "ParameterName does not match");
            Assert.That(rp2.Caption, Is.EqualTo("MyCaption2"), "Caption does not match");
            Assert.That(rp2.DbType, Is.EqualTo(DbType.String), "DbType does not match");
            Assert.That(rp2.DbTypeSize, Is.EqualTo(10), "DbTypeSize does not match");
            Assert.That(rp2.ReportParameterType, Is.EqualTo(ReportParameterType.CheckBox), "ReportParameterType does not match");
            Assert.That(rp2.ParameterSourceSP, Is.EqualTo("AnySP"), "ParameterSourceSP does not match");
            Assert.That(rp2.DefaultValue, Is.EqualTo(string.Empty), "DefaultValue does not match");
            Assert.That(rp2.IsNullAllowed, Is.True, "IsNullAllowed does not match");
            Assert.That(rp2.ErrorMessageCaption, Is.EqualTo("MyTestDate"), "ErrorMessageCaption does not match");
            Assert.That(rp2.IsRendered, Is.True, "IsRendered does not match");
        }

        /// <summary>
        /// Scenario: CEReportParameter Object created with valid parameters and check DbParameter (String)
        /// Expected: Object is created successfully and DbParameter properties match
        /// </summary>
        [Test]
        public void _009_DbParameter_String()
        {
            CEReportParameter rp = new CEReportParameter("blah", "MyParam", "blah", "String", 10, "CheckBox", "AnySP", string.Empty, string.Empty, false, "test message", true);
            rp.Value = "My String";
            DbParameter sp = new DbParameter("@MyParam", DbType.String, 10, "My String");

            Assert.That(sp.DbType, Is.EqualTo(rp.DbParameter.DbType), "DbParameter does not match (DbType)");
            Assert.That(sp.ParameterName, Is.EqualTo(rp.DbParameter.ParameterName), "DbParameter does not match (ParameterName)");
            Assert.That(sp.Size, Is.EqualTo(rp.DbParameter.Size), "DbParameter does not match (Size)");
            Assert.That(sp.Value, Is.EqualTo(rp.DbParameter.Value), "DbParameter does not match (DbParameter Value)");
            Assert.That(sp.Value, Is.EqualTo(rp.Value), "DbParameter does not match (CEReportParameter Value)");
        }

        /// <summary>
        /// Scenario: CEReportParameter Object created with valid parameters and check DbParameter (Int32)
        /// Expected: Object is created successfully and DbParameter properties match
        /// </summary>
        [Test]
        public void _010_DbParameter_Int32()
        {
            CEReportParameter rp = new CEReportParameter("blah", "MyParam", "blah", "Int32", 10, "CheckBox", "AnySP", string.Empty, string.Empty, false, "test message", true);
            rp.Value = 100;
            DbParameter sp = new DbParameter("@MyParam", DbType.Int32, 10, 100);            

            Assert.That(sp.DbType, Is.EqualTo(rp.DbParameter.DbType), "DbParameter does not match (DbType)");
            Assert.That(sp.ParameterName, Is.EqualTo(rp.DbParameter.ParameterName), "DbParameter does not match (ParameterName)");
            Assert.That(sp.Size, Is.EqualTo(rp.DbParameter.Size), "DbParameter does not match (Size)");
            Assert.That(sp.Value, Is.EqualTo(rp.DbParameter.Value), "DbParameter does not match (DbParameter Value)");
            Assert.That(sp.Value, Is.EqualTo(rp.Value), "DbParameter does not match (CEReportParameter Value)");
        }

        /// <summary>
        /// Scenario: CEReportParameter Object created with valid parameters and check DbParameter (DateTime)
        /// Expected: Object is created successfully and DbParameter properties match
        /// </summary>
        [Test]
        public void _011_DbParameter_DateTime()
        {
            CEReportParameter rp = new CEReportParameter("blah", "MyParam", "blah", "DateTime", 10, "CheckBox", "AnySP", string.Empty, string.Empty, false, "test message", true);
            rp.Value = DateTime.Now;
            DbParameter sp = new DbParameter("@MyParam", DbType.DateTime, 10, rp.Value);

            Assert.That(sp.DbType, Is.EqualTo(rp.DbParameter.DbType), "DbParameter does not match (DbType)");
            Assert.That(sp.ParameterName, Is.EqualTo(rp.DbParameter.ParameterName), "DbParameter does not match (ParameterName)");
            Assert.That(sp.Size, Is.EqualTo(rp.DbParameter.Size), "DbParameter does not match (Size)");
            Assert.That(sp.Value, Is.EqualTo(rp.DbParameter.Value), "DbParameter does not match (DbParameter Value)");
            Assert.That(sp.Value, Is.EqualTo(rp.Value), "DbParameter does not match (CEReportParameter Value)");        
        }

         /// <summary>
        /// Scenario: Object created with valid parameters including an empty and null ParameterSourceSP and GetParameterDataView run 
        /// Expected: Object is created successfully but GetParameterDataView returns null
        /// </summary>
        [Test]
        public void _012_GetParameterDataView_EmptyParameterSourceSP()
        {
            CEReportParameter rp = new CEReportParameter("blah", "ParamName1", "MyCaption", "Int32", 0, "CheckBox", string.Empty, string.Empty, string.Empty, false, "test message", true);
            DataView dv = rp.GetParameterDataView();
            Assert.That(dv, Is.Null, "null result expected");
            rp = new CEReportParameter("blah", "ParamName1", "MyCaption", "Int32", 0, "CheckBox", null, string.Empty, string.Empty, false, "test message", true);
            dv = rp.GetParameterDataView();
            Assert.That(dv, Is.Null, "null result expected");
        }

        /// <summary>
        /// Scenario: CEReportParameter Object created with valid parameters including a real ParameterSourceSP and GetParameterDataView run 
        ///           However, parameter SP has no IsDefaultColumn
        /// Expected: Object is created successfully and GetParameterDataView returns a populated dataview
        ///           The default value should be same as constructor default value
        /// </summary>
        [Test]
        public void _013_GetParameterDataView_ParameterSPHasNoIsDefaultColumn()
        {
            // assumes TestParameterSPWithoutIsDefaultColumn exists in database
            CEReportParameter rp = new CEReportParameter("blah", "ParamName1", "MyCaption", "Int32", 0, "CheckBox", "TestParameterSPWithoutIsDefaultColumn", string.Empty, "MyDefaultValue", false, "test message", true);

            DataView dv = rp.GetParameterDataView();            

            Assert.That(dv.Table.Rows.Count, Is.GreaterThan(0), "Count greater than 0 expected");
            Assert.That(dv.Table.Rows.Count, Is.EqualTo(3), "Count of 3 expected");

            // test default value of 2 is returned
            Assert.That(rp.DefaultValue, Is.EqualTo("MyDefaultValue"), "Default Value of MyDDValue2 expected");
        }        

        /// <summary>
        /// Scenario: CEReportParameter Object created with valid parameters including a real ParameterSourceSP and GetParameterDataView run 
        /// Expected: Object is created successfully and GetParameterDataView returns a populated dataview
        /// </summary>
        [Test]
        public void _014_GetParameterDataView_Actual()
        {
            // assumes TestParameterSPDropDown exists in database
            CEReportParameter rp = new CEReportParameter("blah", "ParamName1", "MyCaption", "Int32", 0, "CheckBox", "TestParameterSPDropDown", string.Empty, string.Empty, false, "test message", true);

            DataView dv = rp.GetParameterDataView();            

            Assert.That(dv.Table.Rows.Count, Is.GreaterThan(0), "Count greater than 0 expected");
            Assert.That(dv.Table.Rows.Count, Is.EqualTo(3), "Count of 3 expected");

            // test default value of 2 is returned
            Assert.That(rp.DefaultValue, Is.EqualTo("2"), "Default Value of MyDDValue2 expected");

            // test Setting ErrorMessageCaption
            rp.ErrorMessageCaption = "MyErrorTestMessageCaption";
        }

        /// <summary>
        /// Scenario: Property set, IsNullAllowed = false, invalid value
        /// Expected: Exception
        /// </summary>
        [Test]
        public void _015_ErrorMessageCaption_Set_Invalid()
        {
            CEReportParameter rp = new CEReportParameter("blah", "ParamName1", "MyCaption", "Int32", 0, "CheckBox", "TestParameterSPDropDown", string.Empty, string.Empty, false, "test", true);

            // null
            Assert.That(delegate { rp.ErrorMessageCaption = null; }, Throws.InstanceOf<Exception>().With.Message.EqualTo("ErrorMessageCaption must be specified if Null value is not allowed"));

            // empty
            Assert.That(delegate { rp.ErrorMessageCaption = string.Empty; }, Throws.InstanceOf<Exception>().With.Message.EqualTo("ErrorMessageCaption must be specified if Null value is not allowed"));
        }

        /// <summary>
        /// Scenario: CEReportParameter Object created with valid parameters and check ReportParameter (String)
        /// Expected: Object is created successfully and ReportParameter properties match
        /// </summary>
        [Test]
        public void _016_ReportParameter_String()
        {
            CEReportParameter mrp = new CEReportParameter("blah", "MyParam", "blah", "String", 10, "CheckBox", "AnySP", string.Empty, string.Empty, false, "test message", true);
            mrp.Value = "My String";
            ReportParameter rp = new ReportParameter("MyParam", "My String");

            Assert.That(rp.Name, Is.EqualTo("MyParam"), "ReportParameter does not match (Name)");
            Assert.That(rp.Values[0], Is.EqualTo("My String"), "ReportParameter does not match (Value)");
        }

        /// <summary>
        /// Scenario: CEReportParameter Object created with valid parameters and check ReportParameter (Int32)
        /// Expected: Object is created successfully and ReportParameter properties match
        /// </summary>
        [Test]
        public void _017_ReportParameter_Int32()
        {
            CEReportParameter mrp = new CEReportParameter("blah", "MyParam", "blah", "Int32", 10, "CheckBox", "AnySP", string.Empty, string.Empty, false, "test message", true);
            mrp.Value = 100;
            ReportParameter rp = new ReportParameter("MyParam", "100");

            Assert.That(rp.Name, Is.EqualTo("MyParam"), "ReportParameter does not match (Name)");
            Assert.That(rp.Values[0], Is.EqualTo("100"), "ReportParameter does not match (Value)");
        }

        /// <summary>
        /// Scenario: CEReportParameter Object created with valid parameters and check ReportParameter (DateTime)
        /// Expected: Object is created successfully and ReportParameter properties match
        /// </summary>
        [Test]
        public void _018_ReportParameter_DateTime()
        {
            CEReportParameter mrp = new CEReportParameter("blah", "MyParam", "blah", "DateTime", 10, "CheckBox", "AnySP", string.Empty, string.Empty, false, "test message", true);
            string bl = Convert.ToString(true);
            ReportParameter rp = new ReportParameter("MyParam", bl);

            Assert.That(rp.Name, Is.EqualTo("MyParam"), "ReportParameter does not match (Name)");
            Assert.That(rp.Values[0], Is.EqualTo(bl), "ReportParameter does not match (Value)");
        }

        /// <summary>
        /// Scenario: CEReportParameter Object created with valid parameters and check ReportParameter (Boolean)
        /// Expected: Object is created successfully and ReportParameter properties match
        /// </summary>
        [Test]
        public void _019_ReportParameter_Boolean()
        {
            CEReportParameter mrp = new CEReportParameter("blah", "MyParam", "blah", "Boolean", 1, "RadioButton", "AnySP", "-1:True Option~~0:False Option", "-1", false, "test message", true);
            bool bl = true;
            string bls = Convert.ToString(bl);
            ReportParameter rp = new ReportParameter("MyParam", bls);

            Assert.That(rp.Name, Is.EqualTo("MyParam"), "ReportParameter does not match (Name)");
            Assert.That(rp.Values[0], Is.EqualTo(bls), "ReportParameter does not match (Value)");        
        }

        /// <summary>
        /// Scenario: Object created with valid parameters including an empty and null EmptyParameterSourceValues and GetParameterDataView run 
        /// Expected: Object is created successfully but GetParameterDataView returns null
        /// </summary>
        [Test]
        public void _020_GetParameterDataView_EmptyParameterSourceValues()
        {
            CEReportParameter rp = new CEReportParameter("blah", "ParamName1", "MyCaption", "Int32", 0, "CheckBox", string.Empty, string.Empty, string.Empty, false, "test message", true);
            DataView dv = rp.GetParameterDataView();
            Assert.That(dv, Is.Null, "null result expected");
            rp = new CEReportParameter("blah", "ParamName1", "MyCaption", "Int32", 0, "CheckBox", null, null, string.Empty, false, "test message", true);
            dv = rp.GetParameterDataView();
            Assert.That(dv, Is.Null, "null result expected");
        }

        /// <summary>
        /// Scenario: CEReportParameter Object created with valid parameters with ParameterSourceValues and GetParameterDataView run         
        /// Expected: Object is created successfully and GetParameterDataView returns a populated dataview
        ///           The default value should be same as constructor default value
        /// </summary>
        [Test]
        public void _021_GetParameterDataView_ParameterSourceValues()
        {
            CEReportParameter rp = new CEReportParameter("blah", "ParamName1", "MyCaption", "Boolean", 1, "RadioButton", string.Empty, "1:True Option~~0:False Option", "1", false, "test message", true);

            DataView dv = rp.GetParameterDataView();

            Assert.That(dv.Table.Rows.Count, Is.GreaterThan(0), "Count greater than 0 expected");
            Assert.That(dv.Table.Rows.Count, Is.EqualTo(2), "Count of 2 expected");

            // test default value of 2 is returned
            Assert.That(rp.DefaultValue, Is.EqualTo("1"), "Default Value of MyDDValue2 expected");
        }
    }
}
