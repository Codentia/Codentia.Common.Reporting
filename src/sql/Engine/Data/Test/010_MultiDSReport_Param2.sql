if exists (select * from sysobjects where id = object_id(N'[dbo].[MultiDSReport_Param2]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[MultiDSReport_Param2]
GO

CREATE PROCEDURE [dbo].[MultiDSReport_Param2]									 

									@P2Param1		XML
AS	 

SELECT @P2Param1 Col1

 