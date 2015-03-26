if exists (select * from dbo.sysobjects where id = object_id(N'[ReportParameter]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [ReportParameter]
GO


CREATE TABLE [dbo].[ReportParameter](
	[ReportParameterId] [int] IDENTITY(1,1) NOT NULL,
	[ReportParameterCode] [nvarchar] (30) NOT NULL,
	[ReportParameterSourceSP] [nvarchar] (100) NOT NULL,
	[ReportParameterSourceValues] [nvarchar] (MAX) NOT NULL,
	[ReportParameterTypeId] [int] NOT NULL,	
	[SqlDbTypeId] [int] NOT NULL,
	[SqlDbTypeSize] [int] NOT NULL DEFAULT 0,
 CONSTRAINT [PK_ReportParameter] PRIMARY KEY CLUSTERED 
(
	[ReportParameterId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO