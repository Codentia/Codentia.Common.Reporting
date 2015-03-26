using System;
using System.Data;
using Codentia.Common.Data;
using Codentia.Common.Helper;
using Codentia.Common.Logging;
using Codentia.Common.Logging.BL;
using Codentia.Common.Reporting.DL;
using Microsoft.Reporting.WebForms;

namespace Codentia.Common.Reporting.BL
{
    /// <summary>
    /// CEReportParameter class
    /// </summary>
    public class CEReportParameter
    {
        private string _paramCode = string.Empty;
        private string _paramName = string.Empty;
        private string _caption = string.Empty;
        private DbType _sqlDT = DbType.String;
        private int _sqlDTSize = 0;
        private ReportParameterType _rptType = ReportParameterType.CheckBox;
        private string _parameterSourceSP = string.Empty;
        private string _parameterSourceValues = string.Empty;
        private object _value = null;
        private DbParameter _sp = null;
        private string _defaultValue = string.Empty;
        private bool _isNullAllowed = false;
        private string _errorMessageCaption = string.Empty;
        private ReportParameter _rp = null;
        private bool _isRendered = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="CEReportParameter"/> class.
        /// </summary>
        /// <param name="parameterCode">database code of parameter</param>       
        /// <param name="parameterName">Name used in the stored procedure</param>
        /// <param name="caption">front end label for the parameter</param>
        /// <param name="dbTypeCode">Code that evaluates to a DbType enum</param>
        /// <param name="dbTypeSize">size of the DBType (if relevant)</param>
        /// <param name="reportParameterTypeCode">Code that evaluates to a ReportParameterType enum</param>
        /// <param name="parameterSourceSP">data source SP for parameter (can be blank)</param>
        /// <param name="parameterSourceValues">A ~~ delimited string of colon separated values eg 0:True Option~~1:False Option</param>
        /// <param name="defaultValue">value to use if default and parameter type needs a value</param>
        /// <param name="isNullAllowed">by default null is not allowed</param>
        /// <param name="errorMessageCaption">The text to display when a validation error occurs</param>
        /// <param name="isRendered">Is the parameter rendered in the report viewer</param>
        public CEReportParameter(string parameterCode, string parameterName, string caption, string dbTypeCode, int dbTypeSize, string reportParameterTypeCode, string parameterSourceSP, string parameterSourceValues, string defaultValue, bool isNullAllowed, string errorMessageCaption, bool isRendered)
        {
            ParameterCheckHelper.CheckIsValidString(parameterCode, "parameterCode", false);
            ParameterCheckHelper.CheckIsValidString(parameterName, "parameterName", false);
            if (isRendered)
            {
                ParameterCheckHelper.CheckIsValidString(caption, "caption", false);
            }

            ParameterCheckHelper.CheckIsValidString(dbTypeCode, "dbTypeCode", false);
            try
            {
                _sqlDT = (DbType)Enum.Parse(typeof(DbType), dbTypeCode);
            }
            catch (Exception ex)
            {
                string message = string.Format("dbTypeCode: {0} does not exist", dbTypeCode);
                LogManager.Instance.AddToLog(LogMessageType.FatalError, "CEReportParameter", message);
                LogManager.Instance.AddToLog(ex, "CEReportParameter");
                throw new ArgumentException(message, ex);
            }

            if (dbTypeSize < 1)
            {
                if (dbTypeCode == "String")
                {
                    string message = "dbTypeSize must be at least 1";
                    LogManager.Instance.AddToLog(LogMessageType.FatalError, "CEReportParameter", message);
                    throw new ArgumentException(message);
                }
                else
                {
                    dbTypeSize = 0;
                }
            }
            
            ParameterCheckHelper.CheckIsValidString(reportParameterTypeCode, "reportParameterTypeCode", false);
            try
            {
                _rptType = (ReportParameterType)Enum.Parse(typeof(ReportParameterType), reportParameterTypeCode);
            }
            catch (Exception ex)
            {
                string message = string.Format("reportParameterTypeCode: {0} does not exist", reportParameterTypeCode);
                LogManager.Instance.AddToLog(LogMessageType.FatalError, "CEReportParameter", message);
                LogManager.Instance.AddToLog(ex, "CEReportParameter");
                throw new ArgumentException(message, ex);
            }

            _paramCode = parameterCode;
            _paramName = parameterName;
            _sqlDTSize = dbTypeSize;            
            _parameterSourceSP = parameterSourceSP;
            _parameterSourceValues = parameterSourceValues;
            _isRendered = isRendered;
            _caption = caption;
            _defaultValue = defaultValue;
            _isNullAllowed = isNullAllowed;
            this.ErrorMessageCaption = errorMessageCaption; // use property to enforce validation
            SetDbParameter();
            SetReportParameter();
        }

        /// <summary>
        /// Gets the ParameterCode
        /// </summary>        
        public string ParameterCode
        {
            get
            {
                return _paramCode;
            }
        }

        /// <summary>
        /// Gets the ParameterName
        /// </summary>        
        public string ParameterName
        {
            get
            {
                return _paramName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsRendered
        /// </summary>        
        public bool IsRendered
        {
            get
            {
                return _isRendered;
            }
        }

        /// <summary>
        /// Gets the Caption
        /// </summary>        
        public string Caption
        {
            get
            {
                return _caption;
            }
        }

        /// <summary>
        /// Gets the DbType
        /// </summary>        
        public DbType DbType
        {
            get
            {
                return _sqlDT;
            }
        }

        /// <summary>
        /// Gets the DbTypeSize
        /// </summary>        
        public int DbTypeSize
        {
            get
            {
                return _sqlDTSize;
            }
        }

        /// <summary>
        /// Gets the ReportParameterType
        /// </summary>        
        public ReportParameterType ReportParameterType
        {
            get
            {
                return _rptType;
            }
        }

        /// <summary>
        /// Gets the ParameterSourceSP
        /// </summary>        
        public string ParameterSourceSP
        {
            get
            {
                return _parameterSourceSP;
            }
        }

        /// <summary>
        /// Gets the ParameterSourceValues
        /// </summary>        
        public string ParameterSourceValues
        {
            get
            {
                return _parameterSourceValues;
            }
        }

        /// <summary>
        /// Gets or sets the Value
        /// </summary>        
        public object Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
                SetDbParameter();
                SetReportParameter();
            }
        }

        /// <summary>
        /// Gets DbParameter
        /// </summary>        
        public DbParameter DbParameter
        {
            get
            {
                return _sp;
            }
        }

        /// <summary>
        /// Gets DefaultValue
        /// if from 
        /// </summary>        
        public string DefaultValue
        {
            get
            {
                return _defaultValue;
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsNullAllowed
        /// if from 
        /// </summary>        
        public bool IsNullAllowed
        {
            get
            {
                return _isNullAllowed;
            }
        }

        /// <summary>
        /// Gets or sets ErrorMessageCaption
        /// if from 
        /// </summary>        
        public string ErrorMessageCaption
        {
            get
            {
                return _errorMessageCaption;
            }

            set
            {
                if (!_isNullAllowed && string.IsNullOrEmpty(value))
                {
                    throw new Exception("ErrorMessageCaption must be specified if Null value is not allowed");
                }

                _errorMessageCaption = value;
            }
        }

        /// <summary>
        /// Gets ReportParameter
        /// </summary>        
        public ReportParameter ReportParameter
        {
            get
            {
                return _rp;
            }
        }
       
        /// <summary>
        /// Return a Data View for the ParameterSourceSP
        /// note: this is currently parameterless
        /// </summary>
        /// <returns>DataView from ParameterSourceSP</returns>
        public DataView GetParameterDataView()
        {
            DataView dv = null;
            DataTable dt = null;
            if (!string.IsNullOrEmpty(_parameterSourceValues))
            {
                dt = ReportingData.GetFixedValueDataTable(_parameterSourceValues);
                dt.TableName = "FixedData";   
            }
            else
            {
                if (!string.IsNullOrEmpty(_parameterSourceSP))
                {
                    dt = ReportingData.RunReportProc(_parameterSourceSP, null);
                    dt.TableName = _parameterSourceSP;
                }
            }
            
            if (dt != null)
            {                
                dv = new DataView();
                if (dt.Rows.Count > 0)
                {
                    dv.Table = dt;                        
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        bool isDefault = false;
                        try
                        {
                            DataRow dr = dt.Rows[i];                  
                            isDefault = Convert.ToBoolean(dr["IsDefault"]);
                            if (isDefault)
                            {
                                _defaultValue = Convert.ToString(dr["Value"]);
                            }
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
            }

            return dv;
        }

        private void SetDbParameter()
        {
            // check db type is supported
            switch (_sqlDT)
            {
                case DbType.String: break;
                case DbType.Xml: break;
                case DbType.DateTime: break;
                case DbType.Int32: break;
                case DbType.Boolean: break;
                default:
                    string message = string.Format("DbType: {0} not yet supported", _sqlDT.ToString());
                    LogManager.Instance.AddToLog(LogMessageType.FatalError, "CEReportParameter", message);
                    throw new NotSupportedException(message);
            }

            _sp = new DbParameter(string.Format("@{0}", _paramName), _sqlDT);
            _sp.Size = _sqlDTSize;
            _sp.Value = GetConvertedValue();
        }

        private object GetConvertedValue()
        {
            if (_value != null)
            {
                switch (_sqlDT)
                {
                    case DbType.String:
                        return Convert.ToString(_value);
                    case DbType.Xml:
                        return Convert.ToString(_value);
                    case DbType.DateTime:
                        return Convert.ToDateTime(_value);
                    case DbType.Int32:
                        return Convert.ToInt32(_value);
                    case DbType.Boolean:
                        return Convert.ToBoolean(_value);
                }
            }

            return null;
        }

        private void SetReportParameter()
        {
            // check db type is supported
            switch (_sqlDT)
            {
                case DbType.String: break;
                case DbType.Xml: break;
                case DbType.DateTime: break;
                case DbType.Int32: break;
                case DbType.Boolean: break;
            }

            string sqlValue = string.Empty;
            if (_sqlDT == DbType.Boolean)
            {
                sqlValue = "0";
                if (_value != null)
                {
                    sqlValue = (bool)GetConvertedValue() ? "True" : "False";
                }
            }
            else
            {
                sqlValue = Convert.ToString(GetConvertedValue());
            }

            _rp = new ReportParameter(_paramName, sqlValue);
        }
    }
}
