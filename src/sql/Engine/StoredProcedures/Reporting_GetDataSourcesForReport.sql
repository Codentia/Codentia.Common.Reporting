if exists (select * from sysobjects where id = object_id(N'[dbo].[Reporting_GetDataSourcesForReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Reporting_GetDataSourcesForReport]
GO

CREATE PROCEDURE [dbo].[Reporting_GetDataSourcesForReport]
						@ReportId INT
						
						
AS	

/* 	
	Procedure:		Reporting_GetDataSourcesForReport
	
	Description:	Get ReportDataSource resultset for a given @ReportId

	Arguments:		@ReportId

	Returns:		Resultset
	
	Temp table:		None	

	History:		v1.00 -	SH 10/08/2008 - Created
						   
*/

SET NOCOUNT ON

SELECT ReportDataSourceId, ReportDataSourceCode, ReportDataSourceSP
FROM dbo.ReportDataSource 
WHERE ReportId=@ReportId
