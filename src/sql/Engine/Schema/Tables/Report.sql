if exists (select * from dbo.sysobjects where id = object_id(N'[Report]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [Report]
GO


CREATE TABLE [dbo].[Report](
	[ReportId] [int] IDENTITY(1,1) NOT NULL,
	[ReportCode] [nvarchar] (50) NOT NULL,
	[ReportName] [nvarchar] (100) NOT NULL,
	[RdlName] [nvarchar] (100) NOT NULL,
	[IsRdlc] BIT NOT NULL DEFAULT 0,
	[OnReportPageList] [bit] NOT NULL DEFAULT 1,
	[TagReplacementXml][nvarchar] (MAX) NOT NULL DEFAULT '',
	[TagReplacementSP] [nvarchar] (100) NOT NULL DEFAULT '',
 CONSTRAINT [PK_Report] PRIMARY KEY CLUSTERED 
(
	[ReportId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO