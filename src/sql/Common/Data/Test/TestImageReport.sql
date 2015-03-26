 if exists (select * from sysobjects where id = object_id(N'[dbo].[TestImageReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[TestImageReport]
GO

CREATE PROCEDURE [dbo].[TestImageReport]						
AS	

SELECT 'TestText' TestText