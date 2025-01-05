/* 
* FILE: Knot_P01_1CreateDbTable.sql
* PROJECT: Knot-P01
* PROGRAMMER: Yujung Park
* FIRST VERSION: 2024-11-17
* DESCRIPTION: This script creates a database of 'Knot' and tables and view.
* 2024-11-24 Configulation Table is changed - "EmployeeSkillLevel" REAL,
* instnwnd.sql
* adventureworks database https://learn.microsoft.com/en-us/sql/samples/adventureworks-install-configure?view=sql-server-ver16&tabs=ssms
* https://stackoverflow.com/questions/7469130/cannot-drop-database-because-it-is-currently-in-use
*/
---------- Database ----------
SET NOCOUNT ON
GO
USE master

IF EXISTS (SELECT * FROM sysdatabases WHERE name = 'Knot')
    ALTER DATABASE Knot set single_user with rollback immediate
	DROP DATABASE Knot;
GO

DECLARE @device_directory NVARCHAR(520)
SELECT @device_directory = SUBSTRING(filename, 1, CHARINDEX(N'master.mdf', LOWER(filename)) - 1)
FROM master.dbo.sysaltfiles WHERE dbid = 1 AND fileid = 1

EXECUTE (N'CREATE DATABASE Knot
  ON PRIMARY (NAME = N''Knot'', FILENAME = N''' + @device_directory + N'Knot.mdf'')
  LOG ON (NAME = N''Knot_log'',  FILENAME = N''' + @device_directory + N'Knot.ldf'')')
GO

SET QUOTED_IDENTIFIER ON
GO

USE Knot
GO


if exists (SELECT * FROM sysobjects WHERE id = object_id('view_BinsWithWorkstationjob') and sysstat & 0xf = 2)
	drop view "dbo"."view_BinsWithWorkstationjob"
GO

IF exists (select * from sysobjects where id = object_id('dbo.Configuration') and sysstat & 0xf = 4)
	DROP TABLE "dbo"."Configuration"
GO
IF exists (select * from sysobjects where id = object_id('dbo.Employees') and sysstat & 0xf = 4)
	DROP TABLE "dbo"."Employees"
GO
IF exists (select * from sysobjects where id = object_id('dbo.Station') and sysstat & 0xf = 4)
	DROP TABLE "dbo"."Stations"
GO
IF exists (select * from sysobjects where id = object_id('dbo.Products') and sysstat & 0xf = 4)
	DROP TABLE "dbo"."Products"
GO
IF exists (select * from sysobjects where id = object_id('dbo.BillOfMaterials') and sysstat & 0xf = 4)
	DROP TABLE "dbo"."BillOfMaterials"
GO
IF exists (select * from sysobjects where id = object_id('dbo.Inventory') and sysstat & 0xf = 4)
	DROP TABLE "dbo"."Inventory"
GO
IF exists (select * from sysobjects where id = object_id('dbo.MaterialTransaction') and sysstat & 0xf = 4)
	DROP TABLE "dbo"."MaterialTransaction"
GO
IF exists (select * from sysobjects where id = object_id('dbo.Bins') and sysstat & 0xf = 4)
	DROP TABLE "dbo"."Bins"
GO
IF exists (select * from sysobjects where id = object_id('dbo.Trays') and sysstat & 0xf = 4)
	DROP TABLE "dbo"."Trays"
GO
IF exists (select * from sysobjects where id = object_id('dbo.WorkstationJob') and sysstat & 0xf = 4)
	DROP TABLE "dbo"."WorkstationJob"
GO

CREATE TABLE "dbo"."Configuration"(
	"ConfigurationID" NVARCHAR(50) NOT NULL,
	"SimulationTime" INT,
	"StationName" NVARCHAR(50),
	"OrderNumber" NVARCHAR(50),
	"OrderQuantity" INT,
	"OrderAmount" MONEY,
	"ProductName" NVARCHAR(50),
	"BatchNumber" NVARCHAR(50),
	"FinishedGoodsTrayMaxQty" INT,
	"PartName1" NVARCHAR(50),
	"PartName2" NVARCHAR(50),
	"PartName3" NVARCHAR(50),
	"PartName4" NVARCHAR(50),
	"PartName5" NVARCHAR(50),
	"PartName6" NVARCHAR(50),
	"ReplQtyPart1" INT,
	"ReplQtyPart2" INT,
	"ReplQtyPart3" INT,
	"ReplQtyPart4" INT,
	"ReplQtyPart5" INT,
	"ReplQtyPart6" INT,
	"PartThresholdQty" INT,
	"EmployeeName" NVARCHAR(50),
	"EmployeeSkillLevel" REAL,
	CONSTRAINT "PK_Configuration" PRIMARY KEY CLUSTERED ("ConfigurationID")
)
GO

CREATE TABLE "dbo"."Employees"(
	"EmployeeID" INT IDENTITY(1,1) NOT NULL,
	"Name" NVARCHAR(50),
	"SkillLevel" REAL,
	CONSTRAINT "PK_Employees" PRIMARY KEY CLUSTERED ("EmployeeID")
)
GO

CREATE TABLE "dbo"."Stations"(
	"StationID" INT IDENTITY(1,1) NOT NULL,
	"StationName" NVARCHAR(50),
	CONSTRAINT "PK_Stations" PRIMARY KEY CLUSTERED ("StationID")
)
GO

CREATE TABLE "dbo"."Products"(
	"ProductID" INT IDENTITY(1,1) NOT NULL,
	"ProductName" NVARCHAR(50),
	"FinishedGoodsFlag" BIT,
	"UnitMeasurementCode" NVARCHAR(50),
	"InitialQty" INT,
	CONSTRAINT "PK_Products" PRIMARY KEY CLUSTERED ("ProductID")
)
GO

CREATE TABLE "dbo"."BillOfMaterials"(
	"BOMID" INT IDENTITY(1,1) NOT NULL,
	"ParentProductID" INT,
	"ChildProductID" INT,
	"BomQuantity" INT, -- set default 1
	CONSTRAINT "PK_BillOfMaterials" PRIMARY KEY CLUSTERED ("BOMID"),
	CONSTRAINT "FK_BillOfMaterials_ParentProductID" FOREIGN KEY ("ParentProductID") REFERENCES "dbo"."Products" ("ProductID"),
	CONSTRAINT "FK_BillOfMaterials_ChildProductID" FOREIGN KEY ("ChildProductID") REFERENCES "dbo"."Products" ("ProductID")
)
GO

-- Part product: product finishgoodsflag is false && order qty > Insert Inventory
-- FG product: when tray is maxquantity > material transaction > Insert Inventory
CREATE TABLE "dbo"."Inventory"(
	"ProductID" INT IDENTITY(1,1) NOT NULL,
	"Quantity" INT,
	"QualityPass" BIT,
	"BatchNumber" NVARCHAR(50), --added
	CONSTRAINT "FK_Inventory_Product" FOREIGN KEY ("ProductID") REFERENCES "dbo"."Products" ("ProductID")
)
GO

-- when tray is maxquantity > material transaction
CREATE TABLE "dbo"."MaterialTransaction"(
	"TransactionID" INT IDENTITY(1,1) NOT NULL,
	"ProductID" INT,
	"MovementType" NVARCHAR(50),
	"BatchNumber" NVARCHAR(50),
	"Timestamp" DATETIME,
	"Quantity" INT,
	CONSTRAINT "PK_MaterialTransaction" PRIMARY KEY CLUSTERED ("TransactionID"),
	CONSTRAINT "FK_MaterialTransaction_Product" FOREIGN KEY ("ProductID") REFERENCES "dbo"."Products" ("ProductID")
)
GO


CREATE TABLE "dbo"."Bins"(
	"BinID" INT IDENTITY(1,1) NOT NULL,
	"ProductID" INT,
	"StationID" INT,
	"PartQuantity" INT,
	"ReplenishentQty" INT,
	"ThresholdQty" INT,
	"IsReplenishmentNeeded" BIT,
	"LastRepTime" DATETIME,
	CONSTRAINT "PK_Bins" PRIMARY KEY CLUSTERED ("BinID"),
	CONSTRAINT "FK_Bins_Station" FOREIGN KEY ("StationID") REFERENCES "dbo"."Stations" ("StationID"),
	CONSTRAINT "FK_Bins_Product" FOREIGN KEY ("ProductID") REFERENCES "dbo"."Products" ("ProductID")
)
GO

CREATE TABLE "dbo"."Trays"(
	"TrayID" INT IDENTITY(1,1) NOT NULL,
	"ProductID" INT,
	"StationID" INT,
	"ProductQuantity" INT,
	"TrayMaxQuantity" INT,
	"BatchNumber" NVARCHAR(50), --added
	CONSTRAINT "PK_Trays" PRIMARY KEY CLUSTERED ("TrayID"),
	CONSTRAINT "FK_Trays_Station" FOREIGN KEY ("StationID") REFERENCES "dbo"."Stations" ("StationID"),
	CONSTRAINT "FK_Trays_Product" FOREIGN KEY ("ProductID") REFERENCES "dbo"."Products" ("ProductID")
)
GO

CREATE TABLE "dbo"."WorkstationJob" (
    "JobID" INT IDENTITY(1,1) NOT NULL,
	"ProductID" INT,
    "StationID" INT,
	"ConfigurationID" NVARCHAR(50), --added
	"StationName" NVARCHAR(50), --added
    "EmployeeID" INT,
	"TrayID" INT,
	"BatchNumber" NVARCHAR(50),
    "OrderNumber" NVARCHAR(50),
	"OrderQuantity" INT,
	"OrderAmount" Money,
    "YieldQuantity" INT,
	"QualityPassQuantity" INT,
	"QualityFailQuantity" INT,
    "JobStartDatetime" DATETIME,
    "JobEndDatetime" DATETIME,
	"Status" NVARCHAR(50)

    CONSTRAINT "PK_WorkstationJob" PRIMARY KEY CLUSTERED ("JobID"),
    CONSTRAINT "FK_WorkstationJob_Product" FOREIGN KEY ("ProductID") REFERENCES "dbo"."Products" ("ProductID"),
    CONSTRAINT "FK_WorkstationJob_Station" FOREIGN KEY ("StationID") REFERENCES "dbo"."Stations" ("StationID"),
	CONSTRAINT "FK_WorkstationJob_Employee" FOREIGN KEY ("EmployeeID") REFERENCES "dbo"."Employees" ("EmployeeID"),
	CONSTRAINT "FK_WorkstationJob_Tray" FOREIGN KEY ("TrayID") REFERENCES "dbo"."Trays" ("TrayID"),
)
GO

CREATE VIEW view_BinsWithWorkstationjob AS
-- ALTER VIEW view_BinsWithWorkstationjob AS
SELECT Bins.BinID, Bins.ProductID, Bins.StationID, Bins.PartQuantity, Bins.ReplenishentQty, Bins.ThresholdQty, Bins.IsReplenishmentNeeded, 
WorkstationJob.JobID, WorkstationJob.ProductID as FG_Id, WorkstationJob.ConfigurationID, WorkstationJob.StationName,
WorkstationJob.EmployeeID, WorkstationJob.TrayID, WorkstationJob.BatchNumber, WorkstationJob.OrderNumber, WorkstationJob. OrderQuantity,
WorkstationJob.OrderAmount, WorkstationJob.YieldQuantity, WorkstationJob.QualityPassQuantity, WorkstationJob.QualityFailQuantity,
WorkstationJob.JobStartDatetime, WorkstationJob.JobEndDatetime, WorkstationJob.Status
FROM Bins INNER JOIN WorkstationJob
ON (Bins.StationID = WorkstationJob.StationID)
GO