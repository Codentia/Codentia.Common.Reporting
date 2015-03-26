 if exists (select * from sysobjects where id = object_id(N'[dbo].[TestSPForImagePaths]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[TestSPForImagePaths]
GO

CREATE PROCEDURE [dbo].[TestSPForImagePaths]
									
AS	 

SELECT Id, ShortDisplayText
FROM dbo.fn_TestImageFunction()