 IF EXISTS ( SELECT 1 FROM dbo.SysObjects WHERE id=OBJECT_ID('dbo.Reporting_ReportExistsById') AND OBJECTPROPERTY(id,'IsProcedure')=1)
	BEGIN
		DROP PROCEDURE dbo.Reporting_ReportExistsById
	END
GO

CREATE PROCEDURE dbo.Reporting_ReportExistsById
	@ReportId		INT,
	@Exists			BIT OUTPUT
AS
BEGIN
	SET NOCOUNT ON 

	IF EXISTS ( SELECT 1 FROM dbo.Report WHERE ReportId = @ReportId )
		BEGIN
			SET @Exists = 1
		END
	ELSE
		BEGIN
			SET @Exists = 0
		END		
END
GO  