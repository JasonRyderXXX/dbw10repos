﻿CREATE TABLE [SystemManagement].[Table1]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT newid(), 
    [user] NVARCHAR(50) NULL, 
    [password] UNIQUEIDENTIFIER NULL, 
    [profile] UNIQUEIDENTIFIER NULL
)