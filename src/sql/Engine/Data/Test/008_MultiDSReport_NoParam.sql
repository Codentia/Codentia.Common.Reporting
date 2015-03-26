if exists (select * from sysobjects where id = object_id(N'[dbo].[MultiDSReport_NoParam]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[MultiDSReport_NoParam]
GO

CREATE PROCEDURE [dbo].[MultiDSReport_NoParam]
									
AS	 

SELECT 'This comes from a no parameter data source SP' MainText

 