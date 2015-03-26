IF EXISTS ( SELECT 1 FROM dbo.SysObjects WHERE id=OBJECT_ID('dbo.Reporting_ReportExistsByCode') AND OBJECTPROPERTY(id,'IsProcedure')=1)
	BEGIN
		DROP PROCEDURE dbo.Reporting_ReportExistsByCode
	END
GO

CREATE PROCEDURE dbo.Reporting_ReportExistsByCode
	@ReportCode	NVARCHAR(50),
	@Exists			BIT OUTPUT
AS
BEGIN
	SET NOCOUNT ON 

	IF EXISTS ( SELECT 1 FROM dbo.Report WHERE ReportCode = @ReportCode )
		BEGIN
			SET @Exists = 1
		END
	ELSE
		BEGIN
			SET @Exists = 0
		END		
END
GO   