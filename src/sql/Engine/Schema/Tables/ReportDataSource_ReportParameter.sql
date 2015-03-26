if exists (select * from dbo.sysobjects where id = object_id(N'[ReportDataSource_ReportParameter]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [ReportDataSource_ReportParameter]
GO


CREATE TABLE [dbo].[ReportDataSource_ReportParameter](
	[ReportDataSourceReportParameterId] [int] IDENTITY(1,1) NOT NULL,
	[ReportDataSourceId] [int] NOT NULL,	
	[ParameterOrder] [int] NOT NULL,
	[ReportParameterId] [int] NOT NULL,
	[ReportParameterName] [nvarchar](50) NOT NULL DEFAULT '',
	[IsRendered]			 [BIT] NOT NULL DEFAULT 1,
	[ReportParameterCaption] [nvarchar](50) NOT NULL DEFAULT '',
	[DefaultValue]			 [nvarchar](Max) NOT NULL DEFAULT '',
	[IsNullAllowed]			 [BIT] NOT NULL DEFAULT 0,
	[ErrorMessageCaption]	 [nvarchar](Max) NOT NULL DEFAULT '',
 CONSTRAINT [PK_ReportDataSource_ReportParameter] PRIMARY KEY CLUSTERED 
(
	[ReportDataSourceReportParameterId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO