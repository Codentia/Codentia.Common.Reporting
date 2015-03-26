if exists (select * from dbo.sysobjects where id = object_id(N'[ReportParameterType]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [ReportParameterType]
GO


CREATE TABLE [dbo].[ReportParameterType](
	[ReportParameterTypeId] [int] NOT NULL,
	[ReportParameterTypeCode] [nvarchar] (30) NOT NULL,
 CONSTRAINT [PK_ReportParameterType] PRIMARY KEY CLUSTERED 
(
	[ReportParameterTypeId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO