IF EXISTS ( SELECT 1 FROM dbo.SysObjects WHERE id=OBJECT_ID('dbo.fn_TestImageFunction') AND OBJECTPROPERTY(id,'IsTableFunction')=1)
	BEGIN
		DROP FUNCTION dbo.fn_TestImageFunction
	END
GO

CREATE FUNCTION dbo.fn_TestImageFunction
	(
	)
	RETURNS TABLE
AS
	RETURN
	(					
		SELECT 1 Id, 'Google' ShortDisplayText, 'http://www.google.co.uk/intl/en_uk/images/logo.gif' URL
		UNION ALL
		SELECT 2, 'Office', 'http://office.microsoft.com/_Services/Ont/images/logooffice.gif'
		UNION ALL
		SELECT 3, 'Mattched', 'http://www.mattchedit.com/Images/MIT_Logo3_small.PNG'
	) 