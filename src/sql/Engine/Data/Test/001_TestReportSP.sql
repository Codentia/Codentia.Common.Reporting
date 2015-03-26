 if exists (select * from sysobjects where id = object_id(N'[dbo].[TestReportSP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[TestReportSP]
GO

CREATE PROCEDURE [dbo].[TestReportSP]
											
AS	 

SELECT 1 Col1, 'MyString1' Col2, 1 IsTrue
UNION ALL
SELECT 2 Col1, 'MyString2' Col2, 1 IsTrue
UNION ALL
SELECT 3 Col1, 'MyString3' Col3, 0 IsTrue 
