/****** Object:  Table [dbo].[FileUpload]    Script Date: 28/12/2018 16:06:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FileUpload](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Path] [nvarchar](2000) NOT NULL,
	[Type] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](150) NOT NULL,
	[ProcessedOn] [datetime] NULL,
 CONSTRAINT [PK_FileUpload] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO