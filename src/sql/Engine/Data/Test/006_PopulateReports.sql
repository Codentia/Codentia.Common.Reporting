  INSERT INTO dbo.Report (ReportCode, ReportName, RdlName, OnReportPageList, TagReplacementXml, TagReplacementSP)
 SELECT 'REP1', 'My Report 1', 'Report_TestReportSPWithParameters', 1, '', ''
 UNION ALL
 SELECT 'REP2', 'My Report 2', 'RDLC2', 1, '', ''
 UNION ALL
 SELECT 'REP3', 'My Report 3', 'RDLC3', 0, '', ''
 UNION ALL
 SELECT 'IMAGETEST', 'Image Test Report', 'IMAGETEST', 0, '<root><ReplaceTag ElementPath="ns:Body/ns:ReportItems/ns:Image/ns:Value" AttributeName="" DataRowFieldName="URL"/></root>', 'TestGetImagePath'
 UNION ALL
 SELECT 'MULTIDSTEST', 'Multiple Data Sources Test Report', 'MULTIDSTEST', 0, '', ''
 UNION ALL
 SELECT 'NOVISIBLEPARAMS', 'No Visible Parameters Test Report', 'NOVISPARAMTEST', 0, '', ''
 
 
EXEC DataLoad__Report_AddDataSource @ReportCode='REP1', @ReportDataSourceCode='TESTDATASOURCE', @ReportDataSourceSP='TestReportSPWithParameters'
 
 
EXEC DataLoad__ReportDataSource_AddParameter	@ReportCode='REP1', @ReportDataSourceCode='TESTDATASOURCE', @ReportParameterCode='PAR1', @ReportParameterName='Param1', 
												@ReportParameterCaption='Param DD Caption', 
												@ReportParameterSourceSP='TestParameterSPDropDown', @ReportParameterTypeCode='DropDown', 
												@SqlDbTypeCode='Int32', @ParameterOrder=0
EXEC DataLoad__ReportDataSource_AddParameter	@ReportCode='REP1', @ReportDataSourceCode='TESTDATASOURCE', @ReportParameterCode='PAR2', @ReportParameterName='Param2', 
												@ReportParameterCaption='Param Date Caption', @ReportParameterSourceSP='', 
												@ReportParameterTypeCode='Date', @SqlDbTypeCode='DateTime', @ParameterOrder=1, @ErrorMessageCaption='TestDate'
EXEC DataLoad__ReportDataSource_AddParameter	@ReportCode='REP1', @ReportDataSourceCode='TESTDATASOURCE', @ReportParameterCode='PAR3', @ReportParameterName='Param3', 
												@ReportParameterCaption='Param CheckBox Caption', 
												@ReportParameterSourceSP='TestParameterSPCheckBox', @ReportParameterTypeCode='CheckBox', 
												@SqlDbTypeCode='Xml', @ParameterOrder=2
EXEC DataLoad__ReportDataSource_AddParameter	@ReportCode='REP1', @ReportDataSourceCode='TESTDATASOURCE', @ReportParameterCode='PAR4', @ReportParameterName='Param4', 
												@ReportParameterCaption='Param RadioButton Caption', 
												@ReportParameterSourceSP='TestParameterSPRadioButton', 
												@ReportParameterTypeCode='RadioButton', @SqlDbTypeCode='Int32', @ParameterOrder=3
EXEC DataLoad__ReportDataSource_AddParameter	@ReportCode='REP1', @ReportDataSourceCode='TESTDATASOURCE', @ReportParameterCode='PAR5', @ReportParameterName='Param5', 
												@ReportParameterCaption='Param RadioButton Boolean Caption', 
												@ReportParameterSourceSP='', 
												@ReportParameterSourceValues='-1:True Option~~0:False Option', 
												@ReportParameterTypeCode='RadioButton', @SqlDbTypeCode='Boolean', @ParameterOrder=4

EXEC DataLoad__Report_AddDataSource @ReportCode='REP2', @ReportDataSourceCode='MITTEST', @ReportDataSourceSP='TestReportSP'

EXEC DataLoad__Report_AddDataSource @ReportCode='REP3', @ReportDataSourceCode='MITTEST', @ReportDataSourceSP='REPSP3'
 
EXEC DataLoad__Report_AddDataSource @ReportCode='IMAGETEST', @ReportDataSourceCode='TESTDATASOURCE', @ReportDataSourceSP='TestImageReport'
 
EXEC DataLoad__Report_AddDataSource @ReportCode='MULTIDSTEST', @ReportDataSourceCode='DSNOPARAM', @ReportDataSourceSP='MultiDSReport_NoParam'

EXEC DataLoad__Report_AddDataSource @ReportCode='MULTIDSTEST', @ReportDataSourceCode='DSPARAM1', @ReportDataSourceSP='MultiDSReport_Param1'

EXEC DataLoad__ReportDataSource_AddParameter	@ReportCode='MULTIDSTEST', @ReportDataSourceCode='DSPARAM1', @ReportParameterCode='MDSREP1PAR1', @ReportParameterName='P1Param1', 
												@ReportParameterCaption='Param DD Caption', 
												@ReportParameterSourceSP='TestParameterSPDropDown', @ReportParameterTypeCode='DropDown', 
												@SqlDbTypeCode='Int32', @ParameterOrder=0

EXEC DataLoad__Report_AddDataSource @ReportCode='MULTIDSTEST', @ReportDataSourceCode='DSPARAM2', @ReportDataSourceSP='MultiDSReport_Param2'

EXEC DataLoad__ReportDataSource_AddParameter	@ReportCode='MULTIDSTEST', @ReportDataSourceCode='DSPARAM2', @ReportParameterCode='MDSREP2PAR1', @ReportParameterName='P2Param1', 
												@ReportParameterCaption='Param CheckBox Caption', 
												@ReportParameterSourceSP='TestParameterSPCheckBox', @ReportParameterTypeCode='CheckBox', 
												@SqlDbTypeCode='Xml', @ParameterOrder=0  
												
												
EXEC DataLoad__Report_AddDataSource @ReportCode='NOVISIBLEPARAMS', @ReportDataSourceCode='NOVISDS', @ReportDataSourceSP='NoVisReport'
												
EXEC DataLoad__ReportDataSource_AddParameter	@ReportCode='NOVISIBLEPARAMS', @ReportDataSourceCode='NOVISDS', @ReportParameterCode='PAR1', @ReportParameterName='Param1', 												
												@IsRendered=0, @ReportParameterTypeCode='NoControl', 
												@SqlDbTypeCode='Int32', @ParameterOrder=0
EXEC DataLoad__ReportDataSource_AddParameter	@ReportCode='NOVISIBLEPARAMS', @ReportDataSourceCode='NOVISDS', @ReportParameterCode='PAR2', @ReportParameterName='Param2', 												 
												@IsRendered=0, @ReportParameterTypeCode='NoControl', 
												@SqlDbTypeCode='DateTime', @ParameterOrder=1
												