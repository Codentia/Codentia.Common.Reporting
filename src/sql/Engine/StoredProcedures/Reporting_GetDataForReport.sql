if exists (select * from sysobjects where id = object_id(N'[dbo].[Reporting_GetDataForReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Reporting_GetDataForReport]
GO

CREATE PROCEDURE [dbo].[Reporting_GetDataForReport]
						@ReportCode NVARCHAR(50)
						
						
AS	

/* 	
	Procedure:		Reporting_GetDataForReport
	
	Description:	Get Report resultset for a given @ReportCode

	Arguments:		@ReportCode

	Returns:		Resultset
	
	Temp table:		None

	History:		v1.00 -	SH 18/07/2008 - Created
						   
*/

SET NOCOUNT ON

SELECT ReportId, ReportName, RdlName, IsRdlc,
TagReplacementXml, TagReplacementSP
FROM dbo.Report WHERE ReportCode=@ReportCode


