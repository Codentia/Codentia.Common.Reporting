if exists (select * from sysobjects where id = object_id(N'[dbo].[Reporting_GetReports]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Reporting_GetReports]
GO

CREATE PROCEDURE [dbo].[Reporting_GetReports]
						
						
AS	

/* 	
	Procedure:		Report_GetReports
	
	Description:	Get Report Display Name and Id 

	Arguments:		@iReportId

	Returns:		resultset of Report Parameters

	Temp table:		None

	Called by:		Report.DAL

	History:		v1.00 -	SH 18/07/2008 - Created
						   
*/

SET NOCOUNT ON

SELECT ReportId, ReportName 
FROM dbo.Report 
WHERE OnReportPageList=1
ORDER BY ReportName
