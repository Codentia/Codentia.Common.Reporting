INSERT INTO dbo.ReportParameterType (ReportParameterTypeId, ReportParameterTypeCode)
SELECT 1, 'CheckBox'
UNION ALL
SELECT 2, 'Date'
UNION ALL
SELECT 3, 'DropDown'
UNION ALL
SELECT 4, 'RadioButton'
UNION ALL
SELECT 5, 'TextBox'
UNION ALL
SELECT 6, 'NoControl'


INSERT INTO dbo.SqlDbType (SqlDbTypeId, SqlDbTypeCode)
SELECT 1, 'Int32'
UNION ALL
SELECT 2, 'String'
UNION ALL
SELECT 3, 'DateTime'
UNION ALL
SELECT 4, 'Xml'
UNION ALL
SELECT 5, 'Boolean'



