namespace Codentia.Common.Reporting
{   
    /// <summary>
    /// Types of Parameters which can be used in reports
    /// </summary>
    public enum ReportParameterType
    {
        /// <summary>
        /// CheckBox Parameter
        /// </summary>
        CheckBox = 1,

        /// <summary>
        /// Date Parameter
        /// </summary>
        Date = 2,

        /// <summary>
        /// DropDown Parameter
        /// </summary>
        DropDown = 3,

        /// <summary>
        /// RadioButton Parameter
        /// </summary>
        RadioButton = 4,

        /// <summary>
        /// TextBox Parameter
        /// </summary>
        TextBox = 5,

        /// <summary>
        /// No Control
        /// </summary>
        NoControl = 6
    }
}