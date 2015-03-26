 if exists (select * from sysobjects where id = object_id(N'[dbo].[TestParameterSPCheckBox]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[TestParameterSPCheckBox]
GO

CREATE PROCEDURE [dbo].[TestParameterSPCheckBox]
									
AS	 

SELECT 1 [Value], 'MyCBValue1' [Name]
UNION ALL
SELECT 2 , 'MyCBValue2' 
UNION ALL
SELECT 3 , 'MyCBValue3'