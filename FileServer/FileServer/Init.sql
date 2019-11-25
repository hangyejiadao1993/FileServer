if exists (select * from sys.databases where name='myapp')
begin
drop database myapp
end
create database myapp


USE myapp
GO
/****** Object:  Table [dbo].[FileDatas]    Script Date: 2019/11/24 16:15:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileDatas](
	[Id] [uniqueidentifier] NOT NULL,
	[FilePath] [nvarchar](max) NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[FileName] [nvarchar](max) NULL,
	[IP] [nvarchar](max) NULL,
 CONSTRAINT [PK_FileDatas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
