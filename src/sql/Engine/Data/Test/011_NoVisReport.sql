if exists (select * from sysobjects where id = object_id(N'[dbo].[NoVisReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[NoVisReport]
GO

CREATE PROCEDURE [dbo].[NoVisReport]
									@Param1		INT,
									@Param2		DATETIME=NULL
AS	 

SELECT @Param1 Col1, @Param2 Col2
 