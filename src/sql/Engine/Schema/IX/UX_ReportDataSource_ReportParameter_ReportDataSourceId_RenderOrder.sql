IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ReportDataSource_ReportParameter]') AND name = N'UX_ReportDataSource_ReportParameter_ReportDataSourceId_ParameterOrder')
DROP INDEX [UX_ReportDataSource_ReportParameter_ReportDataSourceId_ParameterOrder] ON [dbo].[ReportDataSource_ReportParameter] WITH ( ONLINE = OFF )
GO

CREATE UNIQUE NONCLUSTERED INDEX [UX_ReportDataSource_ReportParameter_ReportDataSourceId_ParameterOrder] ON [dbo].[ReportDataSource_ReportParameter] 
(
	[ReportDataSourceId], [ParameterOrder]
)WITH (PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
