IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ReportParameter__ReportParameterType_ReportParameterTypeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReportParameter]'))
ALTER TABLE [dbo].[ReportParameter] DROP CONSTRAINT [FK_ReportParameter__ReportParameterType_ReportParameterTypeId]
GO

ALTER TABLE [dbo].[ReportParameter]  WITH CHECK ADD  CONSTRAINT [FK_ReportParameter__ReportParameterType_ReportParameterTypeId] FOREIGN KEY([ReportParameterTypeId])
REFERENCES [dbo].[ReportParameterType] ([ReportParameterTypeId])
GO
ALTER TABLE [dbo].[ReportParameter] CHECK CONSTRAINT [FK_ReportParameter__ReportParameterType_ReportParameterTypeId]