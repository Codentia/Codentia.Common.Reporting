 if exists (select * from sysobjects where id = object_id(N'[dbo].[TestParameterSPRadioButton]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[TestParameterSPRadioButton]
GO

CREATE PROCEDURE [dbo].[TestParameterSPRadioButton]
									
AS	 

SELECT 1 [Value], 'MyRBValue1' [Name], 0 IsDefault
UNION ALL
SELECT 2 , 'MyRBValue2', 0
UNION ALL
SELECT 3 , 'MyRBValue3', 0
UNION ALL
SELECT 4 , 'MyRBValue4', 1