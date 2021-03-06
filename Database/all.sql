USE [kuzando]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 05/21/2010 01:19:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[SignupDate] [datetime] NOT NULL,
	[OpenId] [varchar](200) NOT NULL,
	[Email] [varchar](50) NULL,
	[SettingsFlags] [int] NOT NULL,
 CONSTRAINT [PK__Users__ID] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 05/21/2010 01:19:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Tasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Text] [nvarchar](2000) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[DueDate] [date] NOT NULL,
	[IsDone] [tinyint] NOT NULL,
	[Importance] [tinyint] NOT NULL,
	[PriorityInDay] [tinyint] NOT NULL,
	[Deleted] [tinyint] NOT NULL,
 CONSTRAINT [PK__Tasks__ID] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_Users_SettingsFlags]    Script Date: 05/21/2010 01:19:05 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_SettingsFlags]  DEFAULT ((0)) FOR [SettingsFlags]
GO
/****** Object:  ForeignKey [FK_User_ID]    Script Date: 05/21/2010 01:19:05 ******/
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_User_ID] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_User_ID]
GO
