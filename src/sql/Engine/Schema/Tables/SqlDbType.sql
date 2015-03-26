if exists (select * from dbo.sysobjects where id = object_id(N'[SqlDbType]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [SqlDbType]
GO


CREATE TABLE [dbo].[SqlDbType](
	[SqlDbTypeId] [int] NOT NULL,
	[SqlDbTypeCode] [nvarchar] (30) NOT NULL,
 CONSTRAINT [PK_SqlDbType] PRIMARY KEY CLUSTERED 
(
	[SqlDbTypeId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO