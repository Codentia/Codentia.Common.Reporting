using System;
using System.Data;
using Codentia.Common.Data;
using Codentia.Common.Helper;

namespace Codentia.Common.Reporting.DL
{
    /// <summary>
    /// This class exposes methods to read data for creating reports
    /// </summary>
    public static class ReportingData
    {
        private static string _dataSourceName = string.Empty;

        /// <summary>
        /// Sets the name of the data source.
        /// </summary>
        /// <value>The name of the data source.</value>
        internal static string DataSourceName
        {
            set
            {
                _dataSourceName = value;
            }
        }

        /// <summary>
        /// Gets a data table of all reports
        /// </summary>
        /// <returns>DataTable of reports</returns>
        public static DataTable GetReports()
        {
            return DbInterface.ExecuteProcedureDataTable(_dataSourceName, "dbo.Reporting_GetReports");
        }
        
        /// <summary>
        /// Get Parameters For DataSource
        /// </summary>
        /// <param name="reportDataSourceId">Id of the report</param>
        /// <remarks>Run sp Reporting_GetParametersForDataSource and return a DataTable of Parameter Data</remarks>
        /// <returns>DataTable of parameters</returns>
        public static DataTable GetParametersForDataSource(int reportDataSourceId)
        {
            if (!ReportDataSourceExists(reportDataSourceId))
                {
                    throw new ArgumentException(string.Format("reportDataSourceId: {0} does not exist", reportDataSourceId));
                }

            DbParameter[] parameters = 
		    { 
		        new DbParameter("@ReportDataSourceId", DbType.Int16)
		    };

            parameters[0].Value = reportDataSourceId;

            return DbInterface.ExecuteProcedureDataTable(_dataSourceName, "Reporting_GetParametersForDataSource", parameters);
        }

        /// <summary>
        /// Run ReportProc
        /// </summary>
        /// <param name="procedureName">Name of Stored procedure to run</param>
        /// <param name="parameters">Parameter Collection</param>
        /// <remarks>Run sp = procedureName and return a DataTable Data</remarks>
        /// <returns>DataTable of run report procedure</returns>
        public static DataTable RunReportProc(string procedureName, DbParameter[] parameters)
        {
            ParameterCheckHelper.CheckIsValidString(procedureName, "procedureName", false);
            return DbInterface.ExecuteProcedureDataTable(_dataSourceName, procedureName, parameters);
        }

        /// <summary>
        /// Get Fixed Value DataTable
        /// </summary>
        /// <param name="values">~~ delimited string with name/value pairs delimited by :</param>        
        /// <remarks>Return a table of values in fixed format (as if it had been run as an sp by RunReportProc)</remarks>
        /// <returns>DataTable of fixed values</returns>
        public static DataTable GetFixedValueDataTable(string values)
        {
            ParameterCheckHelper.CheckIsValidString("values", values, false);

            DataTable dt = new DataTable();

            DataColumn col1 = new DataColumn("Value", typeof(int));
            dt.Columns.Add(col1);
            DataColumn col2 = new DataColumn("Name", typeof(string));
            dt.Columns.Add(col2);
            DataColumn col3 = new DataColumn("IsDefault", typeof(bool));
            dt.Columns.Add(col3);

            char[] chars = { '~', '~' };
            string[] valueArray = values.Split(chars);
            for (int i = 0; i < valueArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(valueArray[i]))
                {
                    DataRow dr = dt.NewRow();
                    string[] currentArray = valueArray[i].Split(':');
                    dr["Value"] = Convert.ToInt32(currentArray[0]);
                    dr["Name"] = Convert.ToString(currentArray[1]);
                    dr["IsDefault"] = false;
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        /// <summary>
        /// Get Data For Report
        /// </summary>
        /// <param name="reportCode">code of the report</param>       
        /// <remarks>Run sp Reporting_GetDataForReport and return resultset</remarks> 
        /// <returns>DataTable of report data</returns>
        public static DataTable GetDataForReport(string reportCode)
        {
            ParameterCheckHelper.CheckIsValidString("reportCode", reportCode, 50, false);

            if (!ReportExists(reportCode))
            {
                throw new ArgumentException(string.Format("reportCode: {0} does not exist", reportCode));
            }

            DbParameter[] parameters = 
		    { 
                new DbParameter("@ReportCode", DbType.String)                        
		    };

            parameters[0].Value = reportCode;

            return DbInterface.ExecuteProcedureDataTable(_dataSourceName, "Reporting_GetDataForReport", parameters);            
        }

        /// <summary>
        /// Verify if a given Report exists, based on its id
        /// </summary>
        /// <param name="reportId">Id to be checked for existence</param>
        /// <returns>bool - true if reports exists, otherwise false</returns>
        public static bool ReportExists(int reportId)
        {
            ParameterCheckHelper.CheckIsValidId(reportId, "reportId");

            DbParameter[] spParams =
                {
                    new DbParameter("@ReportId", DbType.Int32),
                    new DbParameter("@Exists", DbType.Boolean)
                };

            spParams[0].Value = reportId;
            spParams[1].Direction = ParameterDirection.Output;

            DbInterface.ExecuteProcedureNoReturn(_dataSourceName, "dbo.Reporting_ReportExistsById", spParams);

            return Convert.ToBoolean(spParams[1].Value);
        }

        /// <summary>
        /// Verify if a given Report exists, based on its code
        /// </summary>
        /// <param name="reportCode">code to be checked for existence</param>
        /// <returns>bool - true if reports exists, otherwise false</returns>
        public static bool ReportExists(string reportCode)
        {
            ParameterCheckHelper.CheckIsValidString(reportCode, "reportCode", 30, false);

            DbParameter[] spParams =
                {
                    new DbParameter("@ReportCode", DbType.String),
                    new DbParameter("@Exists", DbType.Boolean)
                };

            spParams[0].Value = reportCode;
            spParams[1].Direction = ParameterDirection.Output;

            DbInterface.ExecuteProcedureNoReturn(_dataSourceName, "dbo.Reporting_ReportExistsByCode", spParams);

            return Convert.ToBoolean(spParams[1].Value);
        }

        /// <summary>
        /// Verify if a given ReportDataSource exists, based on its id
        /// </summary>
        /// <param name="reportDataSourceId">Id to be checked for existence</param>
        /// <returns>bool - true if report datasource exists, otherwise false</returns>
        public static bool ReportDataSourceExists(int reportDataSourceId)
        {
            ParameterCheckHelper.CheckIsValidId(reportDataSourceId, "reportDataSourceId");

            DbParameter[] spParams =
                {
                    new DbParameter("@ReportDataSourceId", DbType.Int32),
                    new DbParameter("@Exists", DbType.Boolean)
                };

            spParams[0].Value = reportDataSourceId;
            spParams[1].Direction = ParameterDirection.Output;

            DbInterface.ExecuteProcedureNoReturn(_dataSourceName, "dbo.Reporting_ReportDataSourceExistsById", spParams);

            return Convert.ToBoolean(spParams[1].Value);
        }

        /// <summary>
        /// Verify if a given Report exists, based on its id
        /// </summary>
        /// <param name="reportId">Id to be checked for existence</param>
        /// <returns>DataTable of DataSources</returns>
        public static DataTable GetDataSourcesForReport(int reportId)
        {
            if (!ReportExists(reportId))
            {
                throw new ArgumentException(string.Format("reportId: {0} does not exist", reportId));
            }

            DbParameter[] spParams =
                {
                    new DbParameter("@ReportId", DbType.Int32)                  
                };

            spParams[0].Value = reportId;

            return DbInterface.ExecuteProcedureDataTable(_dataSourceName, "dbo.Reporting_GetDataSourcesForReport", spParams);
        }       
    }
}
