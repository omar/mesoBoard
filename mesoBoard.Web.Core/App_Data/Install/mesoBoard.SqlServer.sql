/*************************************************************************************************************************************************************************************************************
 Table Create
*************************************************************************************************************************************************************************************************************/

/****** Object:  FullTextCatalog [mesoboard_catalog]    Script Date: 12/29/2010 06:52:34 ******/
CREATE FULLTEXT CATALOG [mesoboard_catalog] WITH ACCENT_SENSITIVITY = OFF
AS DEFAULT
AUTHORIZATION [dbo]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](250) NOT NULL,
	[PasswordSalt] [nvarchar](100) NOT NULL,
	[ActivationCode] [nvarchar](50) NULL,
	[Status] [bit] NOT NULL,
	[RegisterDate] [datetime] NOT NULL,
	[RegisterIP] [nvarchar](50) NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LastLoginIP] [nvarchar](50) NULL,
	[LastLogoutDate] [datetime] NOT NULL,
	[LastPostDate] [datetime] NULL,
	[UsernameLower] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FileTypes]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileTypes](
	[FileTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Extension] [nvarchar](50) NOT NULL,
	[Image] [nvarchar](200) NULL,
 CONSTRAINT [PK_FileTypes] PRIMARY KEY CLUSTERED 
(
	[FileTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Configs]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Configs](
	[ConfigID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Value] [nvarchar](150) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Group] [nvarchar](50) NULL,
	[Note] [nvarchar](100) NULL,
	[Options] [nvarchar](150) NULL,
 CONSTRAINT [PK_Configs] PRIMARY KEY CLUSTERED 
(
	[ConfigID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Order] [int] NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BBCodes]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BBCodes](
	[BBCodeID] [int] IDENTITY(1,1) NOT NULL,
	[Tag] [nvarchar](50) NOT NULL,
	[Parse] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_BBCodes] PRIMARY KEY CLUSTERED 
(
	[BBCodeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Plugins]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Plugins](
	[PluginID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[AssemblyName] [nvarchar](50) NOT NULL,
	[Installed] [bit] NOT NULL,
	[Version] [nvarchar](50) NULL,
	[Author] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Website] [nvarchar](50) NULL,
 CONSTRAINT [PK_Plugins] PRIMARY KEY CLUSTERED 
(
	[PluginID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PluginConfigs]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PluginConfigs](
	[PluginConfigID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Value] [nvarchar](150) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[PluginGroup] [nvarchar](50) NULL,
	[Note] [nvarchar](100) NULL,
	[Options] [nvarchar](150) NULL,
 CONSTRAINT [PK_PluginConfigs] PRIMARY KEY CLUSTERED 
(
	[PluginConfigID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Themes]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Themes](
	[ThemeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[DisplayName] [nvarchar](150) NOT NULL,
	[VisibleToUsers] [bit] NOT NULL,
	[FolderName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Themes] PRIMARY KEY CLUSTERED 
(
	[ThemeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OnlineGuests]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OnlineGuests](
	[OnlineGuestID] [int] IDENTITY(1,1) NOT NULL,
	[IP] [nvarchar](20) NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_OnlineGuests] PRIMARY KEY CLUSTERED 
(
	[OnlineGuestID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ranks]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ranks](
	[RankID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[PostCount] [int] NOT NULL,
	[Image] [nvarchar](50) NULL,
	[Color] [nvarchar](50) NULL,
	[IsRoleRank] [bit] NOT NULL,
 CONSTRAINT [PK_Ranks] PRIMARY KEY CLUSTERED 
(
	[RankID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Smilies]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Smilies](
	[SmileyID] [int] IDENTITY(1,1) NOT NULL,
	[ImageURL] [nvarchar](150) NOT NULL,
	[Code] [nvarchar](100) NOT NULL,
	[Title] [nvarchar](100) NULL,
 CONSTRAINT [PK_Smilies] PRIMARY KEY CLUSTERED 
(
	[SmileyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[RankID] [int] NULL,
	[IsGroup] [bit] NOT NULL,
	[SpecialPermissions] [tinyint] NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Messages]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Messages](
	[MessageID] [int] IDENTITY(1,1) NOT NULL,
	[FromUserID] [int] NULL,
	[ToUserID] [int] NULL,
	[Subject] [nvarchar](100) NULL,
	[Text] [nvarchar](max) NOT NULL,
	[IsRead] [bit] NOT NULL,
	[DateSent] [datetime] NOT NULL,
	[ToDelete] [bit] NOT NULL,
	[FromDelete] [bit] NOT NULL,
 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PasswordResetRequests]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PasswordResetRequests](
	[UserID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Token] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_PasswordResetRequests] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OnlineUsers]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OnlineUsers](
	[UserID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_OnlineUsers] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Forums]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Forums](
	[ForumID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Order] [int] NOT NULL,
	AllowGuestDownloads bit NOT NULL,
	VisibleToGuests bit NOT NULL
 CONSTRAINT [PK_Forums] PRIMARY KEY CLUSTERED 
(
	[ForumID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ForumPermissions]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ForumPermissions](
	[ForumPermissionID] [int] IDENTITY(1,1) NOT NULL,
	[ForumID] [int] NOT NULL,
	[Visibility] [int] NOT NULL,
	[Posting] [int] NOT NULL,
	[Polling] [int] NOT NULL,
	[Attachments] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_ForumPermissions] PRIMARY KEY CLUSTERED 
(
	[ForumPermissionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Threads]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Threads](
	[ThreadID] [int] IDENTITY(1,1) NOT NULL,
	[ForumID] [int] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Type] [int] NOT NULL,
	[HasPoll] [bit] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[Image] [nvarchar](50) NULL,
 CONSTRAINT [PK_Threads] PRIMARY KEY CLUSTERED 
(
	[ThreadID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE FULLTEXT INDEX ON [dbo].[Threads](
[Title] LANGUAGE [English])
KEY INDEX [PK_Threads]ON ([mesoboard_catalog], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)
GO
/****** Object:  Table [dbo].[InRoles]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InRoles](
	[InRoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_InRoles] PRIMARY KEY CLUSTERED 
(
	[InRoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfiles]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfiles](
	[UserID] [int] NOT NULL,
	[AlwaysSubscribeToThread] [bit] NOT NULL,
	[AlwaysShowSignature] [bit] NOT NULL,
	[ThemeID] [int] NULL,
	[AvatarType] [nvarchar](50) NOT NULL,
	[Avatar] [nvarchar](100) NULL,
	[Location] [nvarchar](100) NULL,
	[DefaultRole] [int] NULL,
	[AIM] [nvarchar](200) NULL,
	[ICQ] [int] NULL,
	[MSN] [nvarchar](200) NULL,
	[Website] [nvarchar](max) NULL,
	[Birthdate] [datetime] NULL,
	[Signature] [nvarchar](max) NULL,
	[ParsedSignature] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserProfiles] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThreadViewStamps]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThreadViewStamps](
	[ViewID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ThreadID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_ThreadViewStamps] PRIMARY KEY CLUSTERED 
(
	[ViewID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThreadViews]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThreadViews](
	[ThreadViewID] [int] IDENTITY(1,1) NOT NULL,
	[ThreadID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_ThreadViews] PRIMARY KEY CLUSTERED 
(
	[ThreadViewID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subscriptions]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscriptions](
	[SubscriptionID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ThreadID] [int] NOT NULL,
 CONSTRAINT [PK_Subscriptions] PRIMARY KEY CLUSTERED 
(
	[SubscriptionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Posts]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Posts](
	[PostID] [int] IDENTITY(1,1) NOT NULL,
	[ThreadID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[Date] [datetime] NOT NULL,
	[UseSignature] [bit] NOT NULL,
	[ParsedText] [nvarchar](max) NOT NULL,
	[TextOnly] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED 
(
	[PostID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE FULLTEXT INDEX ON [dbo].[Posts](
[TextOnly] LANGUAGE [English])
KEY INDEX [PK_Posts]ON ([mesoboard_catalog], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)
GO
/****** Object:  Table [dbo].[Polls]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polls](
	[PollID] [int] NOT NULL,
	[Question] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Polls] PRIMARY KEY CLUSTERED 
(
	[PollID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PollOptions]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PollOptions](
	[PollOptionID] [int] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[PollID] [int] NOT NULL,
 CONSTRAINT [PK_PollOptions] PRIMARY KEY CLUSTERED 
(
	[PollOptionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Attachments]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attachments](
	[AttachmentID] [int] IDENTITY(1,1) NOT NULL,
	[PostID] [int] NOT NULL,
	[DownloadName] [nvarchar](250) NOT NULL,
	[SavedName] [nvarchar](250) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Size] [int] NOT NULL,
	[Downloaded] [int] NOT NULL,
 CONSTRAINT [PK_Attachments] PRIMARY KEY CLUSTERED 
(
	[AttachmentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportedPosts]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportedPosts](
	[PostID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_ReportedPosts] PRIMARY KEY CLUSTERED 
(
	[PostID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PollVotes]    Script Date: 12/29/2010 06:52:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PollVotes](
	[PollVoteID] [int] IDENTITY(1,1) NOT NULL,
	[PollOptionID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_PollVotes] PRIMARY KEY CLUSTERED 
(
	[PollVoteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_Attachments_Posts]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[Attachments]  WITH CHECK ADD  CONSTRAINT [FK_Attachments_Posts] FOREIGN KEY([PostID])
REFERENCES [dbo].[Posts] ([PostID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Attachments] CHECK CONSTRAINT [FK_Attachments_Posts]
GO
/****** Object:  ForeignKey [FK_ForumPermissions_Forums]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[ForumPermissions]  WITH CHECK ADD  CONSTRAINT [FK_ForumPermissions_Forums] FOREIGN KEY([ForumID])
REFERENCES [dbo].[Forums] ([ForumID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ForumPermissions] CHECK CONSTRAINT [FK_ForumPermissions_Forums]
GO
/****** Object:  ForeignKey [FK_Permissions_Roles]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[ForumPermissions]  WITH CHECK ADD  CONSTRAINT [FK_Permissions_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ForumPermissions] CHECK CONSTRAINT [FK_Permissions_Roles]
GO
/****** Object:  ForeignKey [FK_Forums_Categories]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[Forums]  WITH CHECK ADD  CONSTRAINT [FK_Forums_Categories] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Forums] CHECK CONSTRAINT [FK_Forums_Categories]
GO
/****** Object:  ForeignKey [FK_InRoles_Roles]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[InRoles]  WITH CHECK ADD  CONSTRAINT [FK_InRoles_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InRoles] CHECK CONSTRAINT [FK_InRoles_Roles]
GO
/****** Object:  ForeignKey [FK_InRoles_Users]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[InRoles]  WITH CHECK ADD  CONSTRAINT [FK_InRoles_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InRoles] CHECK CONSTRAINT [FK_InRoles_Users]
GO
/****** Object:  ForeignKey [FK_Messages_FromUser]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[Messages]  WITH NOCHECK ADD  CONSTRAINT [FK_Messages_FromUser] FOREIGN KEY([FromUserID])
REFERENCES [dbo].[Users] ([UserID])
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[Messages] NOCHECK CONSTRAINT [FK_Messages_FromUser]
GO
/****** Object:  ForeignKey [FK_Messages_ToUser]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[Messages]  WITH NOCHECK ADD  CONSTRAINT [FK_Messages_ToUser] FOREIGN KEY([ToUserID])
REFERENCES [dbo].[Users] ([UserID])
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[Messages] NOCHECK CONSTRAINT [FK_Messages_ToUser]
GO
/****** Object:  ForeignKey [FK_OnlineUsers_Users]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[OnlineUsers]  WITH CHECK ADD  CONSTRAINT [FK_OnlineUsers_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OnlineUsers] CHECK CONSTRAINT [FK_OnlineUsers_Users]
GO
/****** Object:  ForeignKey [FK_PasswordResetRequests_Users]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[PasswordResetRequests]  WITH CHECK ADD  CONSTRAINT [FK_PasswordResetRequests_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PasswordResetRequests] CHECK CONSTRAINT [FK_PasswordResetRequests_Users]
GO
/****** Object:  ForeignKey [FK_PollOptions_Polls]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[PollOptions]  WITH CHECK ADD  CONSTRAINT [FK_PollOptions_Polls] FOREIGN KEY([PollID])
REFERENCES [dbo].[Polls] ([PollID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PollOptions] CHECK CONSTRAINT [FK_PollOptions_Polls]
GO
/****** Object:  ForeignKey [FK_Polls_Threads]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[Polls]  WITH CHECK ADD  CONSTRAINT [FK_Polls_Threads] FOREIGN KEY([PollID])
REFERENCES [dbo].[Threads] ([ThreadID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Polls] CHECK CONSTRAINT [FK_Polls_Threads]
GO
/****** Object:  ForeignKey [FK_PollVotes_PollOptions]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[PollVotes]  WITH CHECK ADD  CONSTRAINT [FK_PollVotes_PollOptions] FOREIGN KEY([PollOptionID])
REFERENCES [dbo].[PollOptions] ([PollOptionID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PollVotes] CHECK CONSTRAINT [FK_PollVotes_PollOptions]
GO
/****** Object:  ForeignKey [FK_PollVotes_Users]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[PollVotes]  WITH CHECK ADD  CONSTRAINT [FK_PollVotes_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PollVotes] CHECK CONSTRAINT [FK_PollVotes_Users]
GO
/****** Object:  ForeignKey [FK_Posts_Threads]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_Posts_Threads] FOREIGN KEY([ThreadID])
REFERENCES [dbo].[Threads] ([ThreadID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_Posts_Threads]
GO
/****** Object:  ForeignKey [FK_Posts_Users]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_Posts_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_Posts_Users]
GO
/****** Object:  ForeignKey [FK_ReportedPosts_Posts]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[ReportedPosts]  WITH CHECK ADD  CONSTRAINT [FK_ReportedPosts_Posts] FOREIGN KEY([PostID])
REFERENCES [dbo].[Posts] ([PostID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ReportedPosts] CHECK CONSTRAINT [FK_ReportedPosts_Posts]
GO
/****** Object:  ForeignKey [FK_Roles_Ranks]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[Roles]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Ranks] FOREIGN KEY([RankID])
REFERENCES [dbo].[Ranks] ([RankID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Roles] CHECK CONSTRAINT [FK_Roles_Ranks]
GO
/****** Object:  ForeignKey [FK_Subscriptions_Threads]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[Subscriptions]  WITH CHECK ADD  CONSTRAINT [FK_Subscriptions_Threads] FOREIGN KEY([ThreadID])
REFERENCES [dbo].[Threads] ([ThreadID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Subscriptions] CHECK CONSTRAINT [FK_Subscriptions_Threads]
GO
/****** Object:  ForeignKey [FK_Subscriptions_Users]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[Subscriptions]  WITH CHECK ADD  CONSTRAINT [FK_Subscriptions_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Subscriptions] CHECK CONSTRAINT [FK_Subscriptions_Users]
GO
/****** Object:  ForeignKey [FK_Threads_Forums]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[Threads]  WITH CHECK ADD  CONSTRAINT [FK_Threads_Forums] FOREIGN KEY([ForumID])
REFERENCES [dbo].[Forums] ([ForumID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Threads] CHECK CONSTRAINT [FK_Threads_Forums]
GO
/****** Object:  ForeignKey [FK_ThreadViews_Threads]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[ThreadViews]  WITH CHECK ADD  CONSTRAINT [FK_ThreadViews_Threads] FOREIGN KEY([ThreadID])
REFERENCES [dbo].[Threads] ([ThreadID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThreadViews] CHECK CONSTRAINT [FK_ThreadViews_Threads]
GO
/****** Object:  ForeignKey [FK_ThreadViews_Users]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[ThreadViews]  WITH CHECK ADD  CONSTRAINT [FK_ThreadViews_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThreadViews] CHECK CONSTRAINT [FK_ThreadViews_Users]
GO
/****** Object:  ForeignKey [FK_ThreadViewStamp_Threads]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[ThreadViewStamps]  WITH CHECK ADD  CONSTRAINT [FK_ThreadViewStamp_Threads] FOREIGN KEY([ThreadID])
REFERENCES [dbo].[Threads] ([ThreadID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThreadViewStamps] CHECK CONSTRAINT [FK_ThreadViewStamp_Threads]
GO
/****** Object:  ForeignKey [FK_Views_Users]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[ThreadViewStamps]  WITH CHECK ADD  CONSTRAINT [FK_Views_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThreadViewStamps] CHECK CONSTRAINT [FK_Views_Users]
GO
/****** Object:  ForeignKey [FK_UserProfiles_Roles]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[UserProfiles]  WITH CHECK ADD  CONSTRAINT [FK_UserProfiles_Roles] FOREIGN KEY([DefaultRole])
REFERENCES [dbo].[Roles] ([RoleID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[UserProfiles] CHECK CONSTRAINT [FK_UserProfiles_Roles]
GO
/****** Object:  ForeignKey [FK_UserProfiles_Themes]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[UserProfiles]  WITH CHECK ADD  CONSTRAINT [FK_UserProfiles_Themes] FOREIGN KEY([ThemeID])
REFERENCES [dbo].[Themes] ([ThemeID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[UserProfiles] CHECK CONSTRAINT [FK_UserProfiles_Themes]
GO
/****** Object:  ForeignKey [FK_UserProfiles_Users]    Script Date: 12/29/2010 06:52:34 ******/
ALTER TABLE [dbo].[UserProfiles]  WITH CHECK ADD  CONSTRAINT [FK_UserProfiles_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserProfiles] CHECK CONSTRAINT [FK_UserProfiles_Users]
GO


/*************************************************************************************************************************************************************************************************************
 Stored Procedures
*************************************************************************************************************************************************************************************************************/

/****** Object:  StoredProcedure [dbo].[Get_Inactive_OnlineGuests]    Script Date: 11/15/2010 13:31:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_Inactive_OnlineGuests]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM OnlineGuests
    WHERE Date NOT BETWEEN DATEADD(MINUTE, -5, GETUTCDATE()) AND GETUTCDATE()
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Inactive_OnlineUsers]    Script Date: 11/15/2010 13:31:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_Inactive_OnlineUsers]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM OnlineUsers
    WHERE Date NOT BETWEEN DATEADD(MINUTE, -5, GETUTCDATE()) AND GETUTCDATE()
END
GO

/*************************************************************************************************************************************************************************************************************
 Data Inserts
*************************************************************************************************************************************************************************************************************/

SET IDENTITY_INSERT [Themes] ON
INSERT [Themes] ([ThemeID], [Name], [DisplayName], [VisibleToUsers], [FolderName]) VALUES (1, N'Default', N'Default', 1, N'Default')
INSERT [Themes] ([ThemeID], [Name], [DisplayName], [VisibleToUsers], [FolderName]) VALUES (2, N'Dark', N'Dark', 1, N'Dark')
SET IDENTITY_INSERT [Themes] OFF
SET IDENTITY_INSERT [Users] ON
INSERT [Users] ([UserID], [Username], [Email], [Password], [PasswordSalt], [ActivationCode], [Status], [RegisterDate], [RegisterIP], [LastLoginDate], [LastLoginIP], [LastLogoutDate], [LastPostDate], [UsernameLower]) VALUES (1, N'admin', N'admin@yourdomain.com', N'4126A2A4DEA2DD79CBC434AB5FAF23ED', N'4e4001865d7b47049864e09ebe0790d8', NULL, 1, GETUTCDATE(), N'127.0.0.1', GETUTCDATE(), N'127.0.0.1', GETUTCDATE(), GETUTCDATE(), N'admin')
SET IDENTITY_INSERT [Users] OFF
SET IDENTITY_INSERT [FileTypes] ON
INSERT [FileTypes] ([FileTypeID], [Extension], [Image]) VALUES (1, N'.zip', N'compress_file.png')
INSERT [FileTypes] ([FileTypeID], [Extension], [Image]) VALUES (2, N'.png', N'image_file.png')
INSERT [FileTypes] ([FileTypeID], [Extension], [Image]) VALUES (3, N'.gif', N'image_file.png')
INSERT [FileTypes] ([FileTypeID], [Extension], [Image]) VALUES (4, N'.jpg', N'image_file.png')
INSERT [FileTypes] ([FileTypeID], [Extension], [Image]) VALUES (5, N'.jpeg', N'image_file.png')
INSERT [FileTypes] ([FileTypeID], [Extension], [Image]) VALUES (6, N'.bmp', N'image_file.png')
INSERT [FileTypes] ([FileTypeID], [Extension], [Image]) VALUES (7, N'.rar', N'compress_file.png')
INSERT [FileTypes] ([FileTypeID], [Extension], [Image]) VALUES (8, N'.txt', N'text_file.png')
SET IDENTITY_INSERT [FileTypes] OFF
SET IDENTITY_INSERT [Configs] ON
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (1, N'BoardName', N'Board Name', N'string', N'Board', N'Board name used throughout', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (2, N'BoardURL', N'localhost', N'string', N'Board', N'URL to the folder where mesoBoard was installed, include http:// ', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (3, N'AutomatedFromEmail', N'no-reply@yourdomain.com', N'string', N'Mail', N'From email used for automated emails', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (5, N'PageTitlePhrase', N' - Board Name', N'string', N'Board', N'Suffix to use for page titles', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (6, N'UsernameMax', N'20', N'int', N'Registration', N'Maximum username characters', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (7, N'UsernameMin', N'4', N'int', N'Registration', N'Minimum username characters', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (8, N'PasswordMin', N'5', N'int', N'Registration', N'Maximum password characters', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (9, N'Offline', N'false', N'bool', N'Board', N'Set board offline', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (10, N'TimeLimitToEditPost', N'500', N'int', N'Posting', N'Maximum timelimit (in seconds) for users to edit their posts', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (11, N'PostsPerPage', N'10', N'int', N'Posting', N'Number of posts to display per page', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (12, N'ThreadsPerPage', N'25', N'int', N'Posting', N'Number of posts to display per page', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (14, N'AvatarWidth', N'100', N'int', N'Upload', N'Maximum avatar width (in pixels)', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (15, N'AvatarHeight', N'100', N'int', N'Upload', N'Maximum avatar height (in pixels)', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (16, N'BoardCreateDate', GETUTCDATE(), N'string', N'Board', NULL, NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (17, N'MaxFileSize', N'6000', N'int', N'Upload', N'Maximum attachment filesize (in kilobytes; KB)', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (18, N'Language', N'English', N'string[]', N'Board', N'Board language', N'English')
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (19, N'SyntaxHighlighting', N'CSharp,Css,JScript,Php,Sql', N'bool[]', N'Posting', N'Allowed languages for syntax highlighting', N'Cpp,CSharp,Css,Delphi,Java,JScript,Php,Python,Ruby,Sql,Vb,Xml')
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (20, N'AccountActivation', N'None', N'string[]', N'Registration', N'Account activation used for new accounts', N'None,Email,Admin')
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (21, N'BoardTheme', N'1', N'int', N'Theme', N'Board theme', N'')
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (22, N'OverrideUserTheme', N'true', N'bool', N'Theme', N'Override user theme selection with board theme', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (23, N'TimeBetweenPosts', N'120', N'int', N'Posting', N'Minimum time (in seconds) required between creation of new posts', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (24, N'AllowThreadImage', N'true', N'bool', N'Posting', N'Allow users to choose an image for the thread', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (25, N'TimeOffset', N'6', N'int', N'Board', N'Time zone offset relative to GMT (ex. US-Central is 6)', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (26, N'CaptchaFontColor', N'#1b5790', N'string', N'Captcha', N'Hexadecimal code for the captcha font color', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (27, N'CaptchaBackgroundColor', N'#eef2f9', N'string', N'Captcha', N'Hexadecimal code for the captcha background color', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (28, N'CaptchaWidth', N'130', N'int', N'Captcha', N'Captcha image width', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (29, N'CaptchaHeight', N'40', N'int', N'Captcha', N'Captcha image width', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (30, N'CaptchaFontFamily', N'Verdana', N'string', N'Captcha', N'Captcha image font family', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (31, N'CaptchaWarpFactor', N'5', N'int', N'Captcha', N'Amount of warp in the captcha image (higher numbers increase warp)', NULL)
INSERT [Configs] ([ConfigID], [Name], [Value], [Type], [Group], [Note], [Options]) VALUES (32, N'RegistrationRole', N'1', N'int', N'Registration', N'Default role assigned to new registered users', NULL)

SET IDENTITY_INSERT [Configs] OFF
SET IDENTITY_INSERT [Categories] ON
INSERT [Categories] ([CategoryID], [Name], [Description], [Order]) VALUES (1, N'First Category', N'First category description', 1)
SET IDENTITY_INSERT [Categories] OFF
SET IDENTITY_INSERT [BBCodes] ON
INSERT [BBCodes] ([BBCodeID], [Tag], [Parse]) VALUES (1, N'b', N'<b>{1}</b>')
INSERT [BBCodes] ([BBCodeID], [Tag], [Parse]) VALUES (2, N'url', N'<a href="{1}">{2}</a>')
INSERT [BBCodes] ([BBCodeID], [Tag], [Parse]) VALUES (3, N'u', N'<span style="text-decoration:underline">{1}</span>')
INSERT [BBCodes] ([BBCodeID], [Tag], [Parse]) VALUES (4, N'img', N'<img src="{1}" alt="{1}" title="{1}"/>')
INSERT [BBCodes] ([BBCodeID], [Tag], [Parse]) VALUES (5, N'url', N'<a href="{1}">{2}</a>')
INSERT [BBCodes] ([BBCodeID], [Tag], [Parse]) VALUES (6, N'small', N'<span style="font-size: 8px">{1}</span>')
INSERT [BBCodes] ([BBCodeID], [Tag], [Parse]) VALUES (7, N'normal', N'<span style="font-size: 11px">{1}</span>')
INSERT [BBCodes] ([BBCodeID], [Tag], [Parse]) VALUES (8, N'big', N'<span style="font-size: 16px">{1}</span>')
INSERT [BBCodes] ([BBCodeID], [Tag], [Parse]) VALUES (9, N'i', N'<span style="font-style:italic;">{1}</span>')
INSERT [BBCodes] ([BBCodeID], [Tag], [Parse]) VALUES (10, N'quote', N'<div class="bb_quote"><div><b>{1}</b> said:</div><p>{2}</p></div>')
INSERT [BBCodes] ([BBCodeID], [Tag], [Parse]) VALUES (11, N'*', N'<li>{1}</li>')
INSERT [BBCodes] ([BBCodeID], [Tag], [Parse]) VALUES (12, N'list', N'<ul>{1}</ul>')
INSERT [BBCodes] ([BBCodeID], [Tag], [Parse]) VALUES (13, N'color', N'<span style="color:{1}">{2}</span>')
INSERT [BBCodes] ([BBCodeID], [Tag], [Parse]) VALUES (14, N'code', N'<pre class="brush: {1};">{2}</pre>')
SET IDENTITY_INSERT [BBCodes] OFF
SET IDENTITY_INSERT [Ranks] ON
INSERT [Ranks] ([RankID], [Title], [PostCount], [Image], [Color], [IsRoleRank]) VALUES (1, N'New Member', 0, N'square.png', NULL, 0)
INSERT [Ranks] ([RankID], [Title], [PostCount], [Image], [Color], [IsRoleRank]) VALUES (2, N'Member', 50, N'square.png', NULL, 0)
INSERT [Ranks] ([RankID], [Title], [PostCount], [Image], [Color], [IsRoleRank]) VALUES (3, N'Contributor', 300, N'square.png', NULL, 0)
INSERT [Ranks] ([RankID], [Title], [PostCount], [Image], [Color], [IsRoleRank]) VALUES (4, N'Board Addict', 500, N'square.png', N'#bd6044', 0)
INSERT [Ranks] ([RankID], [Title], [PostCount], [Image], [Color], [IsRoleRank]) VALUES (5, N'Administrator', 0, NULL, N'#20a106', 1)
SET IDENTITY_INSERT [Ranks] OFF
SET IDENTITY_INSERT [Smilies] ON
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (1, N'icon_biggrin.gif', N':biggrin:', N'Big Grin')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (2, N'icon_cheesygrin.gif', N':cheeseygrin:', N'Cheesey Grin')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (3, N'icon_confused.gif', N':confused:', N'Confused')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (4, N'icon_cool.gif', N':cool:', N'Cool')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (5, N'icon_cry.gif', N':cry:', N'Cry')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (6, N'icon_eek.gif', N':eek:', N'Eek')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (7, N'icon_evil.gif', N':evil:', N'Evil')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (8, N'icon_exclaim.gif', N':!:', N'Exclaim')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (9, N'icon_frown.gif', N':frown:', N'Frown')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (10, N'icon_idea.gif', N':idea:', N'Idea')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (11, N'icon_lol.gif', N':lol:', N'LOL')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (12, N'icon_mad.gif', N':mad:', N'Mad')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (13, N'icon_mrgreen.gif', N':mrgreen:', N'Mr. Green')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (14, N'icon_neutral.gif', N':neutral:', N'Neutral')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (15, N'icon_question.gif', N':?:', N'Question')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (16, N'icon_razz.gif', N':razz:', N'Razz')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (17, N'icon_redface.gif', N':redface:', N'Red Face')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (18, N'icon_rolleyes.gif', N':rolleyes:', N'Roll Eyes')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (19, N'icon_sad.gif', N':sad:', N'Sad')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (20, N'icon_smile.gif', N':smile:', N'Smile')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (21, N'icon_surprised.gif', N':surprised:', N'Surprised')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (22, N'icon_twisted.gif', N':twisted:', N'Twisted')
INSERT [Smilies] ([SmileyID], [ImageURL], [Code], [Title]) VALUES (23, N'icon_wink.gif', N':wink:', N'Wink')
SET IDENTITY_INSERT [Smilies] OFF
SET IDENTITY_INSERT [Roles] ON
INSERT [Roles] ([RoleID], [Name], [RankID], [IsGroup], [SpecialPermissions]) VALUES (1, N'Member', NULL, 0, 0)
INSERT [Roles] ([RoleID], [Name], [RankID], [IsGroup], [SpecialPermissions]) VALUES (2, N'Administrator', 5, 0, 2)
SET IDENTITY_INSERT [Roles] OFF
SET IDENTITY_INSERT [Forums] ON
INSERT [Forums] ([ForumID], [CategoryID], [Name], [Description], [Order], [AllowGuestDownloads], [VisibleToGuests]) VALUES (1, 1, N'First Forum', N'First forum description', 0, 0, 1)
SET IDENTITY_INSERT [Forums] OFF
SET IDENTITY_INSERT [InRoles] ON
INSERT [InRoles] ([InRoleID], [RoleID], [UserID]) VALUES (1, 2, 1)
SET IDENTITY_INSERT [InRoles] OFF
SET IDENTITY_INSERT [ForumPermissions] ON
INSERT [ForumPermissions] ([ForumPermissionID], [ForumID], [Visibility], [Posting], [Polling], [Attachments], [RoleID]) VALUES (1, 1, 1, 2, 2, 2, 1)
INSERT [ForumPermissions] ([ForumPermissionID], [ForumID], [Visibility], [Posting], [Polling], [Attachments], [RoleID]) VALUES (2, 1, 1, 4, 2, 2, 2)
SET IDENTITY_INSERT [ForumPermissions] OFF
SET IDENTITY_INSERT [Threads] ON
INSERT [Threads] ([ThreadID], [ForumID], [Title], [Type], [HasPoll], [IsLocked], [Image]) VALUES (1, 1, N'mesoBoard Installation Successful!', 1, 0, 0, N'emoticon_happy.png')
SET IDENTITY_INSERT [Threads] OFF
INSERT [UserProfiles] ([UserID], [AlwaysSubscribeToThread], [AlwaysShowSignature], [ThemeID], [AvatarType], [Avatar], [Location], [DefaultRole], [AIM], [ICQ], [MSN], [Website], [Birthdate], [Signature], [ParsedSignature]) VALUES (1, 1, 1, 1, N'None', N'', NULL, 2, NULL, NULL, NULL, NULL, NULL, N'', NULL)
SET IDENTITY_INSERT [Posts] ON
INSERT [Posts] ([PostID], [ThreadID], [UserID], [Text], [Date], [UseSignature], [ParsedText], [TextOnly]) VALUES (1, 1, 1, N'[big][b]mesoBoard has been installed[/b][/big] :!:

To edit this forum and modify other options, login with the admin account you created during the installation and go to the the [url=/Admin/Admin]Admin Control Panel[/url].

For updates and support, visit the [url=http://mesoboard.com]mesoBoard website[/url].', GETUTCDATE(), 0, N'<span style="font-size: 16px"><b>mesoBoard has been installed</b></span> <img alt="Exclaim" src="/Images/Smilies/icon_exclaim.gif" title="Exclaim"></img><br /><br />To edit this forum and modify other options, login with the admin account you created during the installation and go to the the <a href="/Admin/Admin">Admin Control Panel</a>.<br /><br />For updates and support, visit the <a href="http://mesoboard.com">mesoBoard website</a>.', N'mesoBoard has been installed To edit this forum and modify other options, login with the admin account you created during the installation and go to the the Admin Control Panel.For updates and support, visit the mesoBoard website.')
SET IDENTITY_INSERT [Posts] OFF