USE [menuzrus]
GO

DELETE FROM [dbo].[Categories]
GO

DELETE FROM [dbo].[Floors]
GO

DELETE FROM [dbo].[ItemPrices]
GO

DELETE FROM [dbo].[ItemProduct]
GO

DELETE FROM [dbo].[ItemProductAssociation]
GO

DELETE FROM [dbo].[Items]
GO

DELETE FROM [dbo].[Logs]
GO

DELETE FROM [dbo].[Menus]
GO

DELETE FROM [dbo].[OrderChecks]
GO

DELETE FROM [dbo].[OrderChecksMenu]
GO

DELETE FROM [dbo].[OrderChecksMenuProductItems]
GO

DELETE FROM [dbo].[OrderChecksMenuProducts]
GO

DELETE FROM [dbo].[Settings]
GO

DELETE FROM [dbo].[TableOrder]
GO

DELETE FROM [dbo].[Tables]
GO

DELETE FROM [dbo].[Users]
GO

DELETE FROM [dbo].[Customers]
GO

DELETE FROM [dbo].[Comments]
GO

DELETE FROM [dbo].[CheckMenuComment]
GO

DBCC CHECKIDENT ([Categories], reseed, 0)
GO
DBCC CHECKIDENT ([Customers], reseed, 0)
GO
DBCC CHECKIDENT ([Floors], reseed, 0)
GO
DBCC CHECKIDENT ([ItemPrices], reseed, 0)
GO
DBCC CHECKIDENT ([ItemProduct], reseed, 0)
GO
DBCC CHECKIDENT ([ItemProductAssociation], reseed, 0)
GO
DBCC CHECKIDENT ([Items], reseed, 0)
GO
DBCC CHECKIDENT ([Logs], reseed, 0)
GO
DBCC CHECKIDENT ([Menus], reseed, 0)
GO
DBCC CHECKIDENT ([OrderChecks], reseed, 0)
GO
DBCC CHECKIDENT ([OrderChecksMenu], reseed, 0)
GO
DBCC CHECKIDENT ([OrderChecksMenuProductItems], reseed, 0)
GO
DBCC CHECKIDENT ([OrderChecksMenuProducts], reseed, 0)
GO
DBCC CHECKIDENT ([Settings], reseed, 0)
GO
DBCC CHECKIDENT ([TableOrder], reseed, 0)
GO
DBCC CHECKIDENT ([Tables], reseed, 0)
GO
DBCC CHECKIDENT ([Users], reseed, 0)
GO
DBCC CHECKIDENT ([CheckMenuComment], reseed, 0)
GO
DBCC CHECKIDENT ([Comments], reseed, 0)
GO