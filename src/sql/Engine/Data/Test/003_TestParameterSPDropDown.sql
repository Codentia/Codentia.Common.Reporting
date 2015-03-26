if exists (select * from sysobjects where id = object_id(N'[dbo].[TestParameterSPDropDown]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[TestParameterSPDropDown]
GO

CREATE PROCEDURE [dbo].[TestParameterSPDropDown]
									
AS	 

SELECT 1 [Value], 'MyDDValue1' [Name], 0 IsDefault
UNION ALL
SELECT 2 , 'MyDDValue2', 1
UNION ALL
SELECT 3 , 'MyDDValue3', 0