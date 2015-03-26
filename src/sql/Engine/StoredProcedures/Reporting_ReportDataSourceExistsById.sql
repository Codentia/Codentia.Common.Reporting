 IF EXISTS ( SELECT 1 FROM dbo.SysObjects WHERE id=OBJECT_ID('dbo.Reporting_ReportDataSourceExistsById') AND OBJECTPROPERTY(id,'IsProcedure')=1)
	BEGIN
		DROP PROCEDURE dbo.Reporting_ReportDataSourceExistsById
	END
GO

CREATE PROCEDURE dbo.Reporting_ReportDataSourceExistsById
	@ReportDataSourceId		INT,
	@Exists			BIT OUTPUT
AS
BEGIN
	SET NOCOUNT ON 

	IF EXISTS ( SELECT 1 FROM dbo.ReportDataSource WHERE ReportDataSourceId = @ReportDataSourceId )
		BEGIN
			SET @Exists = 1
		END
	ELSE
		BEGIN
			SET @Exists = 0
		END		
END
GO  