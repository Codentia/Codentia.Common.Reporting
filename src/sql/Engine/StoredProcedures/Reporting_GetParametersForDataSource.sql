if exists (select * from sysobjects where id = object_id(N'[dbo].[Reporting_GetParametersForDataSource]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Reporting_GetParametersForDataSource]
GO

CREATE PROCEDURE [dbo].[Reporting_GetParametersForDataSource]
						@ReportDataSourceId INT
						
AS	

/* 	
	Procedure:		Reporting_GetParametersForDataSource
	
	Description:	Get Reportparameters (if any for a given @ReportId and @ReportDataSourceId)

	Arguments:		@ReportDataSourceId

	Returns:		resultset of Report Parameters

	Temp table:		None

	History:		v1.00 -	SH 18/07/2008 - Created
					v1.01 - SH 27/10/2010 - Added ReportParameterSourceValues
						   
*/

SET NOCOUNT ON

SELECT	rp.ReportParameterId, ParameterOrder, ReportParameterSourceSP, ReportParameterSourceValues, ReportParameterCode, SqlDbTypeCode, 
		SqlDbTypeSize, ReportParameterTypeCode, ReportParameterName, IsRendered, ReportParameterCaption, DefaultValue,
		IsNullAllowed, ErrorMessageCaption
FROM dbo.ReportParameter rp
INNER JOIN dbo.ReportDataSource_ReportParameter rrp
		ON rrp.ReportDataSourceId=@ReportDataSourceId AND 
		   rrp.ReportParameterId=rp.ReportParameterId
INNER JOIN dbo.ReportParameterType rt
		ON rt.ReportParameterTypeId=rp.ReportParameterTypeId  
INNER JOIN dbo.SqlDbType sdt
		ON sdt.SqlDbTypeId=rp.SqlDbTypeId
ORDER BY ParameterOrder		


