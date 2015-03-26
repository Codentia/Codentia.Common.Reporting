IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Report]') AND name = N'UX_Report_ReportCode')
DROP INDEX [UX_Report_ReportCode] ON [dbo].[Report] WITH ( ONLINE = OFF )
GO

CREATE UNIQUE NONCLUSTERED INDEX [UX_Report_ReportCode] ON [dbo].[Report] 
(
	[ReportCode] ASC
)WITH (PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
