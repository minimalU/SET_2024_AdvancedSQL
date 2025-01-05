/* 
* FILE: Knot_P01_3CreateTrigger.sql
* PROJECT: Knot-P01
* PROGRAMMER: Yujung Park
* FIRST VERSION: 2024-12-08
* DESCRIPTION: This script creates Triggers for Bins, Trays tables.
*/

USE Knot
GO

IF (OBJECT_ID(N'dbo.trigger_WorkstationJob_YieldQuantity') IS NOT NULL)
BEGIN
      DROP TRIGGER dbo.trigger_WorkstationJob_YieldQuantity
END
GO
CREATE TRIGGER dbo.trigger_WorkstationJob_YieldQuantity
ON  dbo.Trays
AFTER UPDATE 
AS
IF UPDATE(ProductQuantity)
BEGIN
	UPDATE WorkstationJob
	SET    YieldQuantity = Trays.ProductQuantity
	FROM   Trays
	WHERE WorkstationJob.BatchNumber = Trays.BatchNumber
END
GO

IF (OBJECT_ID(N'dbo.trigger_Bin_IsReplenishmentNeeded') IS NOT NULL)
BEGIN
      DROP TRIGGER dbo.trigger_Bin_IsReplenishmentNeeded
END
GO
CREATE TRIGGER dbo.trigger_Bin_IsReplenishmentNeeded
ON  dbo.Bins
AFTER INSERT, UPDATE AS
	UPDATE dbo.Bins
	SET IsReplenishmentNeeded = 1
	FROM dbo.Bins
	WHERE dbo.Bins.PartQuantity <= dbo.Bins.ThresholdQty

	UPDATE dbo.Bins
	SET IsReplenishmentNeeded = 0
	FROM dbo.Bins
	WHERE dbo.Bins.PartQuantity > dbo.Bins.ThresholdQty
GO