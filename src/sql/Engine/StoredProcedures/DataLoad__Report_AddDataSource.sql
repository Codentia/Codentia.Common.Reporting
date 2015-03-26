if exists (select * from sysobjects where id = object_id(N'[dbo].[DataLoad__Report_AddDataSource]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DataLoad__Report_AddDataSource]
GO

CREATE PROCEDURE [dbo].[DataLoad__Report_AddDataSource]
						@ReportCode NVARCHAR(50),
						@ReportDataSourceCode NVARCHAR(50),
						@ReportDataSourceSP NVARCHAR(100)
		
						
AS	

/* 	
	Procedure:		DataLoad__Report_AddDataSource
	
	Description:	Add report data source

	Arguments:		@ReportCode
					@ReportDataSourceCode
					@ReportDataSourceSP
										
	Returns:		nothing

	Temp table:		None

	Called by:		data preparation scripts

	History:		v1.00 -	SH 10/08/2008 - Created
						   
*/

SET NOCOUNT ON

DECLARE @ReportId INT
DECLARE @ReportDataSourceId INT

SELECT @ReportId=ReportId FROM dbo.Report WHERE Reportcode=@ReportCode

IF NOT EXISTS (SELECT 1 FROM dbo.ReportDataSource WHERE ReportId=@ReportId AND ReportDataSourceCode=@ReportDataSourceCode)
	BEGIN
			INSERT INTO dbo.ReportDataSource (ReportId, ReportDataSourceCode, ReportDataSourceSP)
			VALUES (@ReportId, @ReportDataSourceCode, @ReportDataSourceSP)
	END
