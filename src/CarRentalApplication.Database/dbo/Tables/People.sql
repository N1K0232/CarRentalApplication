CREATE TABLE [dbo].[People]
(
	[Id] UNIQUEIDENTIFIER NOT NULL DEFAULT newid(),
    [FirstName] NVARCHAR(256) NOT NULL,
    [LastName] NVARCHAR(256) NOT NULL,
    [BirthDate] DATE NOT NULL,
    [Gender] NVARCHAR(10) NOT NULL,
    [IdentityCardNumber] NVARCHAR(100) NOT NULL,
    [FiscalCode] NVARCHAR(50) NULL,
    [City] NVARCHAR(50) NOT NULL,
    [Province] NVARCHAR(20) NOT NULL,
    [CellphoneNumber] VARCHAR(20) NOT NULL,
    [EmailAddress] VARCHAR(100) NOT NULL,
    [CreationDate] DATETIME NOT NULL DEFAULT getutcdate(),
    [LastModifiedDate] DATETIME NULL,

    PRIMARY KEY([Id])
);

GO
    CREATE UNIQUE NONCLUSTERED INDEX [IX_CellphoneNumber]
    ON [dbo].[People]([CellphoneNumber]);

GO
    CREATE UNIQUE NONCLUSTERED INDEX [IX_EmailAddress]
    ON [dbo].[People]([EmailAddress]);