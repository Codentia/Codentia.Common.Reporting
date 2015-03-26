 if exists (select * from sysobjects where id = object_id(N'[dbo].[TestGetImagePath]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[TestGetImagePath]
GO

CREATE PROCEDURE [dbo].[TestGetImagePath]						
AS	

SELECT Id, ShortDisplayText, URL
FROM dbo.fn_TestImageFunction() 
ORDER BY NEWID()