USE [menuzrus]
GO
/****** Object:  Table [dbo].[Alerts]    Script Date: 3/7/2016 3:57:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP TABLE [dbo].[Alerts]
GO
CREATE TABLE [dbo].[Alerts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CheckMenuId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Alerts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 3/7/2016 3:57:11 PM ******/
DROP TABLE [dbo].[Categories]
GO
CREATE TABLE [dbo].[Categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[ImageUrl] [varchar](50) NULL,
	[Status] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CheckMenuComment]    Script Date: 3/7/2016 3:57:11 PM ******/
DROP TABLE [dbo].[CheckMenuComment]
GO
CREATE TABLE [dbo].[CheckMenuComment](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CommentId] [int] NOT NULL,
	[ParentId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_CheckMenuComment] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Checks]    Script Date: 3/7/2016 3:57:11 PM ******/
DROP TABLE [dbo].[Checks]
GO
CREATE TABLE [dbo].[Checks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[TableOrderId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Price] [decimal](6, 2) NULL,
	[Tax] [decimal](6, 2) NULL,
	[Adjustment] [decimal](6, 2) NULL,
	[UserId] [int] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_OrderChecks] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChecksMenu]    Script Date: 3/7/2016 3:57:11 PM ******/
DROP TABLE [dbo].[ChecksMenu]
GO
CREATE TABLE [dbo].[ChecksMenu](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CheckId] [int] NOT NULL,
	[MenuId] [int] NOT NULL,
	[Status] [int] NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_OrderChecksMenu] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChecksMenuProductItems]    Script Date: 3/7/2016 3:57:11 PM ******/
DROP TABLE [dbo].[ChecksMenuProductItems]
GO
CREATE TABLE [dbo].[ChecksMenuProductItems](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_OrderChecksMenuProductItems] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChecksMenuProducts]    Script Date: 3/7/2016 3:57:11 PM ******/
DROP TABLE [dbo].[ChecksMenuProducts]
GO
CREATE TABLE [dbo].[ChecksMenuProducts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CheckMenuId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_OrderChecksMenuItems] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 3/7/2016 3:57:12 PM ******/
DROP TABLE [dbo].[Comments]
GO
CREATE TABLE [dbo].[Comments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 3/7/2016 3:57:12 PM ******/
DROP TABLE [dbo].[Customers]
GO
CREATE TABLE [dbo].[Customers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
	[Address2] [nvarchar](50) NULL,
	[City] [nvarchar](50) NOT NULL,
	[State] [char](2) NOT NULL,
	[Zip] [char](5) NOT NULL,
	[Phone] [char](10) NULL,
	[ImageUrl] [varchar](50) NULL,
	[Tax] [decimal](5, 2) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Floors]    Script Date: 3/7/2016 3:57:12 PM ******/
DROP TABLE [dbo].[Floors]
GO
CREATE TABLE [dbo].[Floors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Floors] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemPrices]    Script Date: 3/7/2016 3:57:12 PM ******/
DROP TABLE [dbo].[ItemPrices]
GO
CREATE TABLE [dbo].[ItemPrices](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [int] NOT NULL,
	[Price] [decimal](6, 2) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_ItemPrices] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemProduct]    Script Date: 3/7/2016 3:57:12 PM ******/
DROP TABLE [dbo].[ItemProduct]
GO
CREATE TABLE [dbo].[ItemProduct](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_ItemProduct] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemProductAssociation]    Script Date: 3/7/2016 3:57:12 PM ******/
DROP TABLE [dbo].[ItemProductAssociation]
GO
CREATE TABLE [dbo].[ItemProductAssociation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ItemProductId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_ItemProductAssociation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Items]    Script Date: 3/7/2016 3:57:12 PM ******/
DROP TABLE [dbo].[Items]
GO
CREATE TABLE [dbo].[Items](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[ImageUrl] [varchar](50) NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [datetime] NULL,
	[DateModified] [datetime] NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: 3/7/2016 3:57:12 PM ******/
DROP TABLE [dbo].[Logs]
GO
CREATE TABLE [dbo].[Logs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[SessionId] [char](24) NOT NULL,
	[LogType] [int] NOT NULL,
	[Message] [varchar](max) NOT NULL,
	[Trace] [varchar](max) NULL,
	[IP] [varchar](15) NOT NULL,
	[Route] [varchar](255) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MenuDesign]    Script Date: 3/7/2016 3:57:12 PM ******/
DROP TABLE [dbo].[MenuDesign]
GO
CREATE TABLE [dbo].[MenuDesign](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_MenuDesign] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MenuItems]    Script Date: 3/7/2016 3:57:12 PM ******/
DROP TABLE [dbo].[MenuItems]
GO
CREATE TABLE [dbo].[MenuItems](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MenuId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[Side] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_MenuItems] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menus]    Script Date: 3/7/2016 3:57:12 PM ******/
DROP TABLE [dbo].[Menus]
GO
CREATE TABLE [dbo].[Menus](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Menus] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Printouts]    Script Date: 3/7/2016 3:57:12 PM ******/
DROP TABLE [dbo].[Printouts]
GO
CREATE TABLE [dbo].[Printouts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CheckId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_KitchenOrders] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 3/7/2016 3:57:12 PM ******/
DROP TABLE [dbo].[Settings]
GO
CREATE TABLE [dbo].[Settings](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[Value] [varchar](50) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TableOrder]    Script Date: 3/7/2016 3:57:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TableOrder](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[TableId] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_TableOrder] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tables]    Script Date: 3/7/2016 3:57:12 PM ******/
DROP TABLE [dbo].[Tables]
GO
CREATE TABLE [dbo].[Tables](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[FloorId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Status] [int] NOT NULL,
	[Type] [varchar](25) NOT NULL,
	[Top] [int] NOT NULL,
	[Left] [int] NOT NULL,
	[Width] [int] NOT NULL,
	[Height] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Tables] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 3/7/2016 3:57:12 PM ******/
DROP TABLE [dbo].[Users]
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[WorkPhone] [varchar](50) NOT NULL,
	[MobilePhone] [varchar](50) NULL,
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](25) NOT NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
	[Hash] [varchar](50) NULL,
	[Type] [int] NOT NULL,
	[ImageUrl] [varchar](50) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Alerts] ADD  CONSTRAINT [DF_Alerts_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF_Category_Active]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF_Categories_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF_Categories_DateModified]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[CheckMenuComment] ADD  CONSTRAINT [DF_CheckMenuComment_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Checks] ADD  CONSTRAINT [DF_OrderChecks_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[ChecksMenu] ADD  CONSTRAINT [DF_OrderChecksMenu_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[ChecksMenuProductItems] ADD  CONSTRAINT [DF_OrderChecksMenuProductItems_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[ChecksMenuProducts] ADD  CONSTRAINT [DF_OrderChecksMenuItems_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Comments] ADD  CONSTRAINT [DF_Comments_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_Tax]  DEFAULT ((0)) FOR [Tax]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DateModified]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[Floors] ADD  CONSTRAINT [DF_Floors_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Floors] ADD  CONSTRAINT [DF_Floors_DateModified]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[ItemPrices] ADD  CONSTRAINT [DF_ItemPrices_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[ItemProduct] ADD  CONSTRAINT [DF_ItemProduct_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[ItemProduct] ADD  CONSTRAINT [DF_ItemProduct_DateModified]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[ItemProductAssociation] ADD  CONSTRAINT [DF_ItemProductAssociation_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[ItemProductAssociation] ADD  CONSTRAINT [DF_ItemProductAssociation_DateModified]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[Items] ADD  CONSTRAINT [DF_Items_Active]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Items] ADD  CONSTRAINT [DF_Items_CreatedDate]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Items] ADD  CONSTRAINT [DF_Items_ModifiedDate]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[Logs] ADD  CONSTRAINT [DF_Logs_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[MenuDesign] ADD  CONSTRAINT [DF_MenuDesign_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[MenuItems] ADD  CONSTRAINT [DF_MenuCategory_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Menus] ADD  CONSTRAINT [DF_Menus_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Menus] ADD  CONSTRAINT [DF_Menus_DateModified]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[Printouts] ADD  CONSTRAINT [DF_Printouts_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[Printouts] ADD  CONSTRAINT [DF_KitchenOrders_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_DateModified]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[TableOrder] ADD  CONSTRAINT [DF_Checks_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[TableOrder] ADD  CONSTRAINT [DF_Checks_DateModified]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[Tables] ADD  CONSTRAINT [DF_Tables_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Tables] ADD  CONSTRAINT [DF_Tables_DateModified]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Contacts_EmailConfirmed]  DEFAULT ((0)) FOR [EmailConfirmed]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Contacts_Active]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Contacts_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Contacts_DateModified]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Customer1] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Customer1]
GO
