if exists (select * from dbo.sysobjects where id = object_id(N'[ReportDataSource]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [ReportDataSource]
GO


CREATE TABLE [dbo].[ReportDataSource](
	[ReportDataSourceId] [int] IDENTITY(1,1) NOT NULL,
	[ReportId] [int],
	[ReportDataSourceCode] [nvarchar] (50) NOT NULL,
	[ReportDataSourceSP] [nvarchar] (100) NOT NULL,
 CONSTRAINT [PK_ReportDataSource] PRIMARY KEY CLUSTERED 
(
	[ReportDataSourceId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO