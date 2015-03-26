if exists (select * from sysobjects where id = object_id(N'[dbo].[TestReportSPWithParameters]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[TestReportSPWithParameters]
GO

CREATE PROCEDURE [dbo].[TestReportSPWithParameters]
									@Param1		INT,
									@Param2		DATETIME=NULL,
									@Param3		XML,
									@Param4		INT,
									@Param5		BIT
AS	 

SELECT @Param1 Col1, @Param2 Col2, @Param3 Col3, @Param4 Col4, @Param5 Col5
 