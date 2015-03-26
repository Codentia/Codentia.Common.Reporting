using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Codentia.Common.Reporting.DL;
using Microsoft.Reporting.WebForms;

namespace Codentia.Common.Reporting.BL
{
    /// <summary>
    /// CEReport class
    /// </summary>
    public class CEReport
    {
        private string _reportCode = string.Empty;
        private int _reportId;        
        private string _reportName = string.Empty;
        private string _rdlName = string.Empty;
        private bool _isRdlc = false;
        private string _tagReplacementXml = string.Empty;
        private string _tagReplacementSP = string.Empty;
        private Dictionary<string, CEReportDataSource> _mitReportDataSources = new Dictionary<string, CEReportDataSource>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CEReport"/> class.
        /// </summary>
        /// <param name="reportCode">The report code.</param>
        public CEReport(string reportCode)
        {
            _reportCode = reportCode;
            DataTable dt = ReportingData.GetDataForReport(_reportCode);
            DataRow dr = dt.Rows[0];

            _reportId = Convert.ToInt32(dr["ReportId"]);
            _reportName = Convert.ToString(dr["ReportName"]);
            _rdlName = Convert.ToString(dr["RdlName"]);           
            _isRdlc = Convert.ToBoolean(dr["IsRdlc"]);
            _tagReplacementXml = Convert.ToString(dr["TagReplacementXml"]);
            _tagReplacementSP = Convert.ToString(dr["TagReplacementSP"]);

            LoadDataSources();
        }       

        /// <summary>
        /// Gets the ReportCode
        /// </summary>        
        public string ReportCode
        {
            get
            {
                return _reportCode;
            }
        }

        /// <summary>
        /// Gets the ReportId
        /// </summary>        
        public int ReportId
        {
            get
            {
                return _reportId;
            }
        }

        /// <summary>
        /// Gets the ReportName
        /// </summary>        
        public string ReportName
        {
            get
            {
                return _reportName;
            }
        }

        /// <summary>
        /// Gets the Rdl Name
        /// </summary>        
        public string RdlName
        {
            get
            {
                return _rdlName;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the IsRdlc flag is set
        /// </summary>        
        public bool IsRdlc
        {
            get
            {
                return _isRdlc;
            }

            set 
            {
                _isRdlc = value;
            }
        }       
 
         /// <summary>
        /// Gets the CEReportDataSources
        /// </summary>        
        public Dictionary<string, CEReportDataSource> CEReportDataSources
        {
            get
            {
                return _mitReportDataSources;
            }
        }

        /// <summary>
        /// Gets the CollatedReportParameters
        /// </summary>        
        public ReportParameter[] CollatedReportParameters
        {
            get
            {
                if (_isRdlc)
                {
                    throw new Exception("CollatedReportParameters can only be used for rdl reports");
                }

                // Collate Report parameters for all data sources
                Dictionary<int, ReportParameter[]> dict = new Dictionary<int, ReportParameter[]>();
                int paramCount = 0;

                IEnumerator<string> ieRP = _mitReportDataSources.Keys.GetEnumerator();
                while (ieRP.MoveNext())
                {
                    CEReportDataSource mrds = _mitReportDataSources[ieRP.Current];

                    if (mrds.CEReportParameters.Count > 0)
                    {
                        paramCount += mrds.CEReportParameters.Count;
                    }
                }

                ReportParameter[] repArray1 = new ReportParameter[paramCount];
                int paramCount2 = 0;
                int actualParamCount = 0;

                StringBuilder sbCheck = new StringBuilder();
                IEnumerator<string> ie2 = _mitReportDataSources.Keys.GetEnumerator();
                while (ie2.MoveNext())
                {
                    CEReportDataSource mrds = _mitReportDataSources[ie2.Current];

                    if (mrds.ReportParameters != null)
                    {                        
                        for (int i = 0; i < mrds.ReportParameters.Length; i++)
                        {        
                            // ensure Parameter has not already been added
                            if (sbCheck.ToString().IndexOf(mrds.ReportParameters[i].Name) == -1)
                            {
                                repArray1[paramCount2] = mrds.ReportParameters[i];
                                sbCheck.Append(string.Format("{0} ", mrds.ReportParameters[i].Name));
                                actualParamCount++;
                            }

                            paramCount2++;
                        }
                    }                    
                }

                // correct Array for any null
                ReportParameter[] repArray = new ReportParameter[actualParamCount];
                int j = 0;
                for (int i = 0; i  < paramCount2; i++)
                {
                    if (repArray1[i] != null)
                    {
                        repArray[j] = repArray1[i];
                        j++;
                    }
                }

                return repArray;
            }
        }

        /// <summary>
        /// Gets a value indicating whether ParametersAreRendered 
        /// </summary>        
        public bool ParametersAreRendered
        {
            get
            {
                IEnumerator<string> ieRP = _mitReportDataSources.Keys.GetEnumerator();
                while (ieRP.MoveNext())
                {
                    CEReportDataSource mrds = _mitReportDataSources[ieRP.Current];

                    if (mrds.ParametersAreRendered)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Get ReportResult DataSet
        /// Use the dictionary parameterValues to populate the parameters for each data source then return the dataset if the datatables
        /// </summary>
        /// <param name="parameterValues">parameter Values</param>
        /// <returns>DataSet from report results</returns>
        public DataSet GetReportResultDataSet(Dictionary<string, object> parameterValues)
        {
            // set values for all parameters in all data sources
            IEnumerator<string> ie = parameterValues.Keys.GetEnumerator();

            while (ie.MoveNext())
            {
                string[] arrDS = ie.Current.Split('_');
                string ds = arrDS[0];
                string paramCode = arrDS[1];

                CEReportDataSource rds = _mitReportDataSources[ds];
                rds.SetParameterValue(string.Format("{0}_{1}", rds.ReportDataSourceCode, paramCode), parameterValues[ie.Current]);
            }

            DataSet dsResult = new DataSet();

            IEnumerator<string> ieDT = _mitReportDataSources.Keys.GetEnumerator();

            while (ieDT.MoveNext())
            {
                CEReportDataSource rdsDT = _mitReportDataSources[ieDT.Current];
                dsResult.Tables.Add(rdsDT.ResultTable);
            }

            return dsResult;
        }

        /// <summary>
        /// Get Rdl StringReader
        /// </summary>
        /// <param name="applicationPath">path of application</param>
        /// <returns>StringReader for Rdl</returns>
        public StringReader GetRdlStringReader(string applicationPath)
        {
            if (_isRdlc)
            {
                throw new ArgumentException("GetRdlStringReader can only be used for rdl reports");
            }

            DataRow dr = null;

            if (!string.IsNullOrEmpty(_tagReplacementXml) && !string.IsNullOrEmpty(_tagReplacementSP))
            {
                DataTable dt = ReportingData.RunReportProc(_tagReplacementSP, null);

                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                }
            }

            return ReportEngine.GetRdlStringReader(applicationPath, _rdlName, dr, _tagReplacementXml);
        }

        private void LoadDataSources()
        {
            DataTable dt = ReportingData.GetDataSourcesForReport(_reportId);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    int reportDataSourceId = Convert.ToInt32(dr["ReportDataSourceId"]);
                    string reportDataSourceCode = Convert.ToString(dr["ReportDataSourceCode"]);
                    string reportDataSourceSP = Convert.ToString(dr["ReportDataSourceSP"]);

                    CEReportDataSource ds = new CEReportDataSource(reportDataSourceId, reportDataSourceCode, reportDataSourceSP);
                    _mitReportDataSources.Add(reportDataSourceCode, ds);
                }
            }
        }
    }
}
