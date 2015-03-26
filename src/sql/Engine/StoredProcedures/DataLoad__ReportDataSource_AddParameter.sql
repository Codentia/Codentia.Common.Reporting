if exists (select * from sysobjects where id = object_id(N'[dbo].[DataLoad__ReportDataSource_AddParameter]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DataLoad__ReportDataSource_AddParameter]
GO

CREATE PROCEDURE [dbo].[DataLoad__ReportDataSource_AddParameter]
						@ReportCode NVARCHAR(50),
						@ReportDataSourceCode NVARCHAR(50),
						@ReportParameterCode NVARCHAR(30),
						@ReportParameterName NVARCHAR(50),
						@IsRendered BIT=1,
						@ReportParameterCaption NVARCHAR(50)='',
						@ReportParameterSourceSP NVARCHAR (100)='',
						@ReportParameterSourceValues NVARCHAR (MAX)='',
						@ReportParameterTypeCode NVARCHAR (30)='',
						@SqlDbTypeCode NVARCHAR (100),
						@SqlDbTypeSize INT=0,
						@ParameterOrder INT,
						@DefaultValue NVARCHAR(MAX)='',
						@IsNullAllowed BIT=0,
						@ErrorMessageCaption NVARCHAR(MAX)=''
						
AS	

/* 	
	Procedure:		Reporting_AddParameter
	
	Description:	Add report and reportparameter to ReportReportParameter

	Arguments:		@ReportCode
					@ReportDataSourceCode
					@ReportParameterCode
					@ReportParameterName
					@IsRendered
					@ReportParameterCaption
					@ReportParameterSourceSP
					@ReportParameterSourceValues
					@ReportParameterTypeCode
					@SqlDbTypeCode
					@SqlDbTypeSize
					@ParameterOrder
					@DefaultValue
					@IsNullAllowed
					@ErrorMessageCaption

	Returns:		Nothing

	Temp table:		None

	Called by:		data preparation scripts

	History:		v1.00 -	SH 18/07/2008 - Created
					v1.01 - SH 27/10/2010 - Added ReportParameterSourceValues
						   
*/

SET NOCOUNT ON

DECLARE @ReportId INT
DECLARE @ReportParameterId INT
DECLARE @ReportParameterTypeId INT
DECLARE @SqlDbTypeId INT
DECLARE @ReportDataSourceId INT

SELECT @ReportId=ReportId FROM dbo.Report WHERE ReportCode=@ReportCode
SELECT @ReportParameterTypeId=ReportParameterTypeId FROM dbo.ReportParameterType WHERE ReportParameterTypeCode=@ReportParameterTypeCode
SELECT @SqlDbTypeId=SqlDbTypeId FROM dbo.SqlDbType WHERE SqlDbTypeCode=@SqlDbTypeCode

SELECT @ReportDataSourceId=ReportDataSourceId FROM dbo.ReportDataSource WHERE ReportId=@ReportId AND ReportDataSourceCode=@ReportDataSourceCode

IF NOT EXISTS (SELECT 1 FROM dbo.ReportParameter WHERE ReportParameterCode=@ReportParameterCode)
	BEGIN
			INSERT INTO dbo.ReportParameter (ReportParameterCode, ReportParameterSourceSP, ReportParameterSourceValues, ReportParameterTypeId, SqlDbTypeId, SqlDbTypeSize)
			VALUES (@ReportParameterCode, @ReportParameterSourceSP, @ReportParameterSourceValues, @ReportParameterTypeId, @SqlDbTypeId, @SqlDbTypeSize)
			
			SET @ReportParameterId=SCOPE_IDENTITY()
	END
ELSE
	BEGIN
			SELECT @ReportParameterId=ReportParameterId FROM dbo.ReportParameter WHERE ReportParameterCode=@ReportParameterCode
	END	

INSERT INTO dbo.ReportDataSource_ReportParameter (ReportDataSourceId, ReportParameterId, ReportParameterName, IsRendered, ReportParameterCaption, 
										ParameterOrder, DefaultValue, IsNullAllowed, ErrorMessageCaption)
SELECT @ReportDataSourceId, @ReportParameterId, @ReportParameterName, @IsRendered, @ReportParameterCaption, 
		@ParameterOrder, @DefaultValue, @IsNullAllowed, @ErrorMessageCaption
WHERE NOT EXISTS (SELECT 1 FROM dbo.ReportDataSource_ReportParameter WHERE ReportDataSourceId=@ReportDataSourceId AND ReportParameterId=@ReportParameterId)



