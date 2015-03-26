if exists (select * from sysobjects where id = object_id(N'[dbo].[TestParameterSPWithoutIsDefaultColumn]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[TestParameterSPWithoutIsDefaultColumn]
GO

CREATE PROCEDURE [dbo].[TestParameterSPWithoutIsDefaultColumn]
									
AS	 

SELECT 1 [Value], 'MyDDValue1' [Name]
UNION ALL
SELECT 2 , 'MyDDValue2'
UNION ALL
SELECT 3 , 'MyDDValue3'