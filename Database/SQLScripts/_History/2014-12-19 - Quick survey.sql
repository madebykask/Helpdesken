/****** Object:  Table [dbo].[tblSurvey]    Script Date: 19.12.2014 16:10:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblSurvey](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[caseId] [int] NOT NULL,
	[VoteResult] [int] NOT NULL,
 CONSTRAINT [PK_tblSurvey_1] PRIMARY KEY CLUSTERED 
(
	[caseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

insert into [dbo].[tblReport] values(22);