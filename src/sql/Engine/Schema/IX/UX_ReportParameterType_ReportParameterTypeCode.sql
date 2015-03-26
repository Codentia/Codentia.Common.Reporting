IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ReportParameterType]') AND name = N'UX_ReportParameterType_ReportParameterTypeCode')
DROP INDEX [UX_ReportParameterType_ReportParameterTypeCode] ON [dbo].[ReportParameterType] WITH ( ONLINE = OFF )
GO

CREATE UNIQUE NONCLUSTERED INDEX [UX_ReportParameterType_ReportParameterTypeCode] ON [dbo].[ReportParameterType] 
(
	[ReportParameterTypeCode] ASC
)WITH (PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
