
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/06/2016 18:34:54
-- Generated from EDMX file: d:\benutzerdaten\emma\documents\visual studio 2015\Projects\Katalog\Katalog\KatalogDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [KatalogDatabase];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_DepartementDorf]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DorfSet] DROP CONSTRAINT [FK_DepartementDorf];
GO
IF OBJECT_ID(N'[dbo].[FK_SprachgruppeDorf]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DorfSet] DROP CONSTRAINT [FK_SprachgruppeDorf];
GO
IF OBJECT_ID(N'[dbo].[FK_DorfObjekt]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ObjektSet] DROP CONSTRAINT [FK_DorfObjekt];
GO
IF OBJECT_ID(N'[dbo].[FK_KategorieObjekt]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ObjektSet] DROP CONSTRAINT [FK_KategorieObjekt];
GO
IF OBJECT_ID(N'[dbo].[FK_KategorieKategorie]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KategorieSet] DROP CONSTRAINT [FK_KategorieKategorie];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[DepartementSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartementSet];
GO
IF OBJECT_ID(N'[dbo].[SprachgruppeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SprachgruppeSet];
GO
IF OBJECT_ID(N'[dbo].[DorfSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DorfSet];
GO
IF OBJECT_ID(N'[dbo].[KategorieSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KategorieSet];
GO
IF OBJECT_ID(N'[dbo].[ObjektSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ObjektSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'DepartementSet'
CREATE TABLE [dbo].[DepartementSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SprachgruppeSet'
CREATE TABLE [dbo].[SprachgruppeSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'DorfSet'
CREATE TABLE [dbo].[DorfSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Departement_Id] int  NOT NULL,
    [Sprachgruppe_Id] int  NOT NULL
);
GO

-- Creating table 'KategorieSet'
CREATE TABLE [dbo].[KategorieSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Oberkategorie_Id] int  NOT NULL
);
GO

-- Creating table 'ObjektSet'
CREATE TABLE [dbo].[ObjektSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ObjektNummer] nvarchar(max)  NOT NULL,
    [ObjektName] nvarchar(max)  NOT NULL,
    [Bilder] nvarchar(max)  NOT NULL,
    [Herkunft] nvarchar(max)  NOT NULL,
    [BeschreibungMaterial] nvarchar(max)  NOT NULL,
    [BeschreibungHerstellung] nvarchar(max)  NOT NULL,
    [Zustand] nvarchar(max)  NOT NULL,
    [Masse] nvarchar(max)  NOT NULL,
    [ErworbenBei] nvarchar(max)  NOT NULL,
    [Datierung] nvarchar(max)  NOT NULL,
    [Versicherungswert] nvarchar(max)  NOT NULL,
    [Dorf_Id] int  NOT NULL,
    [Kategorie_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'DepartementSet'
ALTER TABLE [dbo].[DepartementSet]
ADD CONSTRAINT [PK_DepartementSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SprachgruppeSet'
ALTER TABLE [dbo].[SprachgruppeSet]
ADD CONSTRAINT [PK_SprachgruppeSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DorfSet'
ALTER TABLE [dbo].[DorfSet]
ADD CONSTRAINT [PK_DorfSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'KategorieSet'
ALTER TABLE [dbo].[KategorieSet]
ADD CONSTRAINT [PK_KategorieSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ObjektSet'
ALTER TABLE [dbo].[ObjektSet]
ADD CONSTRAINT [PK_ObjektSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Departement_Id] in table 'DorfSet'
ALTER TABLE [dbo].[DorfSet]
ADD CONSTRAINT [FK_DepartementDorf]
    FOREIGN KEY ([Departement_Id])
    REFERENCES [dbo].[DepartementSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartementDorf'
CREATE INDEX [IX_FK_DepartementDorf]
ON [dbo].[DorfSet]
    ([Departement_Id]);
GO

-- Creating foreign key on [Sprachgruppe_Id] in table 'DorfSet'
ALTER TABLE [dbo].[DorfSet]
ADD CONSTRAINT [FK_SprachgruppeDorf]
    FOREIGN KEY ([Sprachgruppe_Id])
    REFERENCES [dbo].[SprachgruppeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SprachgruppeDorf'
CREATE INDEX [IX_FK_SprachgruppeDorf]
ON [dbo].[DorfSet]
    ([Sprachgruppe_Id]);
GO

-- Creating foreign key on [Dorf_Id] in table 'ObjektSet'
ALTER TABLE [dbo].[ObjektSet]
ADD CONSTRAINT [FK_DorfObjekt]
    FOREIGN KEY ([Dorf_Id])
    REFERENCES [dbo].[DorfSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DorfObjekt'
CREATE INDEX [IX_FK_DorfObjekt]
ON [dbo].[ObjektSet]
    ([Dorf_Id]);
GO

-- Creating foreign key on [Kategorie_Id] in table 'ObjektSet'
ALTER TABLE [dbo].[ObjektSet]
ADD CONSTRAINT [FK_KategorieObjekt]
    FOREIGN KEY ([Kategorie_Id])
    REFERENCES [dbo].[KategorieSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_KategorieObjekt'
CREATE INDEX [IX_FK_KategorieObjekt]
ON [dbo].[ObjektSet]
    ([Kategorie_Id]);
GO

-- Creating foreign key on [Oberkategorie_Id] in table 'KategorieSet'
ALTER TABLE [dbo].[KategorieSet]
ADD CONSTRAINT [FK_KategorieKategorie]
    FOREIGN KEY ([Oberkategorie_Id])
    REFERENCES [dbo].[KategorieSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_KategorieKategorie'
CREATE INDEX [IX_FK_KategorieKategorie]
ON [dbo].[KategorieSet]
    ([Oberkategorie_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------