IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ReportDataSource_ReportParameter__ReportDataSource_ReportDataSourceId]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReportDataSource_ReportParameter]'))
ALTER TABLE [dbo].[ReportDataSource_ReportParameter] DROP CONSTRAINT [FK_ReportDataSource_ReportParameter__ReportDataSource_ReportDataSourceId]
GO

ALTER TABLE [dbo].[ReportDataSource_ReportParameter]  WITH CHECK ADD  CONSTRAINT [FK_ReportDataSource_ReportParameter__ReportDataSource_ReportDataSourceId] FOREIGN KEY([ReportDataSourceId])
REFERENCES [dbo].[ReportDataSource] ([ReportDataSourceId])
GO
ALTER TABLE [dbo].[ReportDataSource_ReportParameter] CHECK CONSTRAINT [FK_ReportDataSource_ReportParameter__ReportDataSource_ReportDataSourceId]