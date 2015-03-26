using NUnit.Framework;

namespace Codentia.Common.Reporting.Types.Test
{
    /// <summary>
    /// ReportParameterType Test 
    /// </summary>
    [TestFixture]
    public class ReportParameterTypeTest
    {
        /// <summary>
        /// Scenario: Check enum values are same as expected    
        /// Expected: All test pass
        /// </summary>
        [Test]
        public void _001_ParameterTypeCheck()
        {
            Assert.That((int)ReportParameterType.CheckBox == 1, Is.True);
            Assert.That((int)ReportParameterType.Date == 2, Is.True);
            Assert.That((int)ReportParameterType.DropDown == 3, Is.True);
            Assert.That((int)ReportParameterType.RadioButton == 4, Is.True);
            Assert.That((int)ReportParameterType.TextBox == 5, Is.True);
            Assert.That((int)ReportParameterType.NoControl == 6, Is.True);
        }
    }
}
