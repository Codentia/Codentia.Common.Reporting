if exists (select * from sysobjects where id = object_id(N'[dbo].[MultiDSReport_Param1]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[MultiDSReport_Param1]
GO

CREATE PROCEDURE [dbo].[MultiDSReport_Param1]									 

									@P1Param1		INT
AS	 

SELECT @P1Param1 Col1

 