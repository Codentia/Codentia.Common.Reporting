using System;
using System.Collections.Generic;
using System.Data;
using Codentia.Common.Data;
using Codentia.Common.Reporting.DL;
using Microsoft.Reporting.WebForms;

namespace Codentia.Common.Reporting.BL
{
    /// <summary>
    /// CEReportDataSource class
    /// </summary>
    public class CEReportDataSource
    {
        private string _reportDataSourceCode = string.Empty;
        private string _reportDataSourceSP = string.Empty;
        private int _reportDataSourceId;
        private Dictionary<string, CEReportParameter> _mitReportParameters = new Dictionary<string, CEReportParameter>();
        private DbParameter[] _sqlParams = null;
        private ReportParameter[] _repParams = null;
        private bool _isDirty = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="CEReportDataSource"/> class.
        /// </summary>
        /// <param name="reportDataSourceId">The report data source id.</param>
        /// <param name="reportDataSourceCode">The report data source code.</param>
        /// <param name="reportDataSourceSP">The report data source SP.</param>
        public CEReportDataSource(int reportDataSourceId, string reportDataSourceCode, string reportDataSourceSP)
        {
            _reportDataSourceId = reportDataSourceId;    
            _reportDataSourceCode = reportDataSourceCode;
            _reportDataSourceSP = reportDataSourceSP;

            LoadParameters();
        }

        /// <summary>
        /// Gets the CEReportParameters
        /// </summary>        
        public Dictionary<string, CEReportParameter> CEReportParameters
        {
            get
            {
                return _mitReportParameters;
            }
        }

        /// <summary>
        /// Gets the ResultTable        
        /// </summary>
        public DataTable ResultTable
        {
            get
            {
                UpdateArrays();
                DataTable dt = ReportingData.RunReportProc(_reportDataSourceSP, _sqlParams);
                dt.TableName = _reportDataSourceCode;
                return dt;
            }
        }

        /// <summary>
        /// Gets the DbParameters
        /// </summary>        
        public DbParameter[] DbParameters
        {
            get
            {
                return _sqlParams;
            }
        }

        /// <summary>
        /// Gets the ReportParameter
        /// </summary>        
        public ReportParameter[] ReportParameters
        {
            get
            {
                return _repParams;
            }
        }

        /// <summary>
        /// Gets the ReportDataSourceCode
        /// </summary>        
        public string ReportDataSourceCode
        {
            get
            {
                return _reportDataSourceCode;
            }
        }

        /// <summary>
        /// Gets the ReportDataSourceSP
        /// </summary>        
        public string ReportDataSourceSP
        {
            get
            {
                return _reportDataSourceSP;
            }
        }

        /// <summary>
        /// Gets the ReportDataSourceId
        /// </summary>        
        public int ReportDataSourceId
        {
            get
            {
                return _reportDataSourceId;
            }
        }

        /// <summary>
        /// Gets a value indicating whether ParametersAreRendered
        /// </summary>        
        public bool ParametersAreRendered
        {
            get
            {
                IEnumerator<string> ieRP = _mitReportParameters.Keys.GetEnumerator();
                while (ieRP.MoveNext())
                {
                    CEReportParameter rp = _mitReportParameters[ieRP.Current];

                    if (_mitReportParameters.Count > 0)
                    {
                        if (rp.IsRendered)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Set Parameter Value        
        /// </summary>
        /// <param name="paramCode">code of Param</param>
        /// <param name="paramValue">value of Param</param>
        public void SetParameterValue(string paramCode, object paramValue)
        {
            CEReportParameter rp = _mitReportParameters[paramCode];
            rp.Value = paramValue;
            _isDirty = true;
        }        

        private void LoadParameters()
        {
            DataTable dt = ReportingData.GetParametersForDataSource(_reportDataSourceId);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    string paramCode = Convert.ToString(dr["ReportParameterCode"]);
                    string paramName = Convert.ToString(dr["ReportParameterName"]);
                    string paramCaption = Convert.ToString(dr["ReportParameterCaption"]);
                    string sourceSP = Convert.ToString(dr["ReportParameterSourceSP"]);
                    string sourceValues = Convert.ToString(dr["ReportParameterSourceValues"]);                    
                    string rptParamTypeCode = Convert.ToString(dr["ReportParameterTypeCode"]);
                    string sqlDBTypeCode = Convert.ToString(dr["SqlDbTypeCode"]);
                    int sqlDBTypeSize = Convert.ToInt32(dr["SqlDbTypeSize"]);
                    string defaultValue = Convert.ToString(dr["DefaultValue"]);
                    bool isNullAllowed = Convert.ToBoolean(dr["IsNullAllowed"]);
                    string errorMessageCaption = Convert.ToString(dr["ErrorMessageCaption"]);
                    bool isRendered = Convert.ToBoolean(dr["IsRendered"]);

                    if (!isNullAllowed && string.IsNullOrEmpty(errorMessageCaption))
                    {
                        errorMessageCaption = "Parameter value is incorrect";
                    }

                    CEReportParameter param = new CEReportParameter(paramCode, paramName, paramCaption, sqlDBTypeCode, sqlDBTypeSize, rptParamTypeCode, sourceSP, sourceValues, defaultValue, isNullAllowed, errorMessageCaption, isRendered);
                    _mitReportParameters.Add(string.Format("{0}_{1}", _reportDataSourceCode, paramCode), param);
                }
            }
        }

        /// <summary>
        /// Update Arrays        
        /// </summary>
        private void UpdateArrays()
        {
            if (_isDirty)
            {
                // Set or refresh parameter arrays
                DbParameter[] spArray = new DbParameter[_mitReportParameters.Count];
                ReportParameter[] repArray = new ReportParameter[_mitReportParameters.Count];

                IEnumerator<string> ie = _mitReportParameters.Keys.GetEnumerator();
                int i = 0;
                while (ie.MoveNext())
                {
                    spArray[i] = _mitReportParameters[ie.Current].DbParameter;
                    repArray[i] = _mitReportParameters[ie.Current].ReportParameter;
                    i++;
                }

                _sqlParams = spArray;
                _repParams = repArray;

                _isDirty = false;
            }
        }      
    }
}
