/* 
* FILE: Knot_P01_2CreateSP.sql
* PROJECT: Knot-P01
* PROGRAMMER: Yujung Park
* FIRST VERSION: 2024-11-26
* DESCRIPTION: This script create a stored procedure that inserts initial simulation data into tables.
* https://stackoverflow.com/questions/28786096/why-do-i-get-a-cursor-with-the-name-already-exists
*/
USE Knot
SET NOCOUNT ON
GO

IF OBJECT_ID('SP_SimulationSetup', 'P') IS NOT NULL
    DROP PROCEDURE SP_SimulationSetup;
GO

CREATE PROCEDURE SP_SimulationSetup
    @Cid nvarchar(50)
AS
BEGIN
	---------- Check parameters
	IF (SELECT ConfigurationID From Configuration WHERE ConfigurationID = @Cid) IS NULL
	BEGIN
		PRINT 'Invalid ConfigurationID: Configulation ID does not exist.';
		THROW 50001, 'Date Error', 1;
	END

	DECLARE @ConId nvarchar(50) = NULL
	DECLARE @return int = 0
	---------- Config Variables	
	DECLARE @ConfigurationID nvarchar(50) = null
	DECLARE @SimulationTime int = null
	DECLARE @Stationname nvarchar(50) = NULL
	DECLARE @OrderNumber nvarchar(50) = NULL
	DECLARE @OrderQty int = NULL
	DECLARE @OrderAmount money = NULL
	DECLARE @ProductName nvarchar(50) = NULL
	DECLARE @BatchNumber nvarchar(50) = NULL
	DECLARE @FinishedGoodsTrayMaxQty int = NULL
	DECLARE @PartName1 nvarchar(50), @PartName2 nvarchar(50), @PartName3 nvarchar(50), @PartName4 nvarchar(50), @PartName5 nvarchar(50), @PartName6 nvarchar(50)
	DECLARE @ReplQtyPart1 nvarchar(50), @ReqpQtyPart2 nvarchar(50), @ReqpQtyPart3 nvarchar(50),@ReqpQtyPart4 nvarchar(50),@ReqpQtyPart5 nvarchar(50), @ReqpQtyPart6 nvarchar(50)
	DECLARE @PartThresholdQty int = null
	DECLARE @EmployeeName nvarchar(50) = null
	DECLARE @EmployeeSkillLevel real = null

BEGIN TRY
	SET @ConId = @Cid
	
	DECLARE c_cursor CURSOR LOCAL FOR
		SELECT ConfigurationID, SimulationTime, StationName, OrderNumber, OrderQuantity, OrderAmount, ProductName, BatchNumber, FinishedGoodsTrayMaxQty,
		PartName1, PartName2, PartName3, PartName4, PartName5, PartName6, ReplQtyPart1, ReplQtyPart2, ReplQtyPart3, ReplQtyPart4, ReplQtyPart5, ReplQtyPart6,
		PartThresholdQty, EmployeeName, EmployeeSkillLevel
		FROM Configuration WHERE ConfigurationID = @ConId

	OPEN c_cursor

	FETCH NEXT FROM c_cursor
	INTO @ConfigurationID, @SimulationTime, @Stationname, @OrderNumber, @OrderQty, @OrderAmount, @ProductName, @BatchNumber, @FinishedGoodsTrayMaxQty, 
	@PartName1, @PartName2, @PartName3, @PartName4, @PartName5, @PartName6, @ReplQtyPart1, @ReqpQtyPart2, @ReqpQtyPart3, @ReqpQtyPart4, @ReqpQtyPart5, @ReqpQtyPart6,
	@PartThresholdQty, @EmployeeName, @EmployeeSkillLevel
	
	WHILE (@@FETCH_STATUS = 0)
		BEGIN
			FETCH NEXT FROM c_cursor
			INTO @ConfigurationID, @SimulationTime, @Stationname, @OrderNumber, @OrderQty, @OrderAmount, @ProductName, @BatchNumber, @FinishedGoodsTrayMaxQty, 
			@PartName1, @PartName2, @PartName3, @PartName4, @PartName5, @PartName6, @ReplQtyPart1, @ReqpQtyPart2, @ReqpQtyPart3, @ReqpQtyPart4, @ReqpQtyPart5, @ReqpQtyPart6,
			@PartThresholdQty, @EmployeeName, @EmployeeSkillLevel
		END
	CLOSE c_cursor
	DEALLOCATE c_cursor
	
	---------- Config Variables
	DECLARE @FG_ProdId int = null
	DECLARE @P1_ProdId int = null
	DECLARE @P2_ProdId int = null
	DECLARE @P3_ProdId int = null
	DECLARE @P4_ProdId int = null
	DECLARE @P5_ProdId int = null
	DECLARE @P6_ProdId int = null
	DECLARE @StId int = null
	DECLARE @EmpId int = null
	DECLARE @TrId int = null
	DECLARE @start_time DATETIME = null
	DECLARE @end_time DATETIME = null

	---------- INSERT	
	IF ((SELECT ProductName FROM Products WHERE ProductName=@ProductName) IS NULL)
		BEGIN
		ALTER TABLE Products NOCHECK CONSTRAINT ALL
		INSERT INTO Products VALUES (@ProductName, 1, 'ea', 0)
		INSERT INTO Products VALUES (@PartName1, 0, 'ea', 0)
		INSERT INTO Products VALUES (@PartName2, 0, 'ea', 0)
		INSERT INTO Products VALUES (@PartName3, 0, 'ea', 0)
		INSERT INTO Products VALUES (@PartName4, 0, 'ea', 0)
		INSERT INTO Products VALUES (@PartName5, 0, 'ea', 0)
		INSERT INTO Products VALUES (@PartName6, 0, 'ea', 0)
		END

	ALTER TABLE Employees NOCHECK CONSTRAINT ALL
	INSERT INTO Employees VALUES (@EmployeeName, @EmployeeSkillLevel)

	ALTER TABLE Stations NOCHECK CONSTRAINT ALL
	INSERT INTO Stations VALUES (@Stationname)

	SELECT @FG_ProdId = ProductID FROM Products WHERE ProductName=@ProductName
	SELECT @P1_ProdId = ProductID FROM Products WHERE ProductName=@PartName1
	SELECT @P2_ProdId = ProductID FROM Products WHERE ProductName=@PartName2
	SELECT @P3_ProdId = ProductID FROM Products WHERE ProductName=@PartName3
	SELECT @P4_ProdId = ProductID FROM Products WHERE ProductName=@PartName4
	SELECT @P5_ProdId = ProductID FROM Products WHERE ProductName=@PartName5
	SELECT @P6_ProdId = ProductID FROM Products WHERE ProductName=@PartName6
	
	SELECT @EmpId = EmployeeID FROM Employees WHERE Name=@EmployeeName	
	SELECT @StId = StationID FROM Stations WHERE StationName=@Stationname

	SET @start_time = GETDATE()
	SET @end_time = DATEADD(minute, @SimulationTime, @start_time)
	
	ALTER TABLE Trays NOCHECK CONSTRAINT ALL
	INSERT INTO Trays (ProductID, StationID, ProductQuantity, TrayMaxQuantity, BatchNumber) VALUES (@FG_ProdId, @StId, 0, @FinishedGoodsTrayMaxQty, @BatchNumber)

	ALTER TABLE MaterialTransaction NOCHECK CONSTRAINT ALL
	INSERT INTO MaterialTransaction (ProductID, MovementType, BatchNumber, "Timestamp", Quantity) VALUES (@P1_ProdId, 'PartIn', 'na', GETDATE(), @OrderQty)
	INSERT INTO MaterialTransaction (ProductID, MovementType, BatchNumber, "Timestamp", Quantity) VALUES (@P2_ProdId, 'PartIn', 'na', GETDATE(), @OrderQty)
	INSERT INTO MaterialTransaction (ProductID, MovementType, BatchNumber, "Timestamp", Quantity) VALUES (@P3_ProdId, 'PartIn', 'na', GETDATE(), @OrderQty)
	INSERT INTO MaterialTransaction (ProductID, MovementType, BatchNumber, "Timestamp", Quantity) VALUES (@P4_ProdId, 'PartIn', 'na', GETDATE(), @OrderQty)
	INSERT INTO MaterialTransaction (ProductID, MovementType, BatchNumber, "Timestamp", Quantity) VALUES (@P5_ProdId, 'PartIn', 'na', GETDATE(), @OrderQty)
	INSERT INTO MaterialTransaction (ProductID, MovementType, BatchNumber, "Timestamp", Quantity) VALUES (@P6_ProdId, 'PartIn', 'na', GETDATE(), @OrderQty)

	ALTER TABLE Bins NOCHECK CONSTRAINT ALL
	INSERT INTO Bins (ProductID, StationID, PartQuantity, ReplenishentQty, ThresholdQty, IsReplenishmentNeeded, LastRepTime) 
	VALUES (@P1_ProdId, @StId, @ReplQtyPart1, @ReplQtyPart1, @PartThresholdQty, 0, @start_time)
	INSERT INTO Bins (ProductID, StationID, PartQuantity, ReplenishentQty, ThresholdQty, IsReplenishmentNeeded, LastRepTime) 
	VALUES (@P2_ProdId, @StId, @ReqpQtyPart2, @ReqpQtyPart2, @PartThresholdQty, 0, @start_time)
	INSERT INTO Bins (ProductID, StationID, PartQuantity, ReplenishentQty, ThresholdQty, IsReplenishmentNeeded, LastRepTime) 
	VALUES (@P3_ProdId, @StId, @ReqpQtyPart3, @ReqpQtyPart3, @PartThresholdQty, 0, @start_time)
	INSERT INTO Bins (ProductID, StationID, PartQuantity, ReplenishentQty, ThresholdQty, IsReplenishmentNeeded, LastRepTime) 
	VALUES (@P4_ProdId, @StId, @ReqpQtyPart4, @ReqpQtyPart4, @PartThresholdQty, 0, @start_time)
	INSERT INTO Bins (ProductID, StationID, PartQuantity, ReplenishentQty, ThresholdQty, IsReplenishmentNeeded, LastRepTime) 
	VALUES (@P5_ProdId, @StId, @ReqpQtyPart5, @ReqpQtyPart5, @PartThresholdQty, 0, @start_time)
	INSERT INTO Bins (ProductID, StationID, PartQuantity, ReplenishentQty, ThresholdQty, IsReplenishmentNeeded, LastRepTime) 
	VALUES (@P6_ProdId, @StId, @ReqpQtyPart6, @ReqpQtyPart6, @PartThresholdQty, 0, @start_time)

	SELECT @TrId = TrayID FROM Trays WHERE StationID=@StId

	ALTER TABLE WorkstationJob NOCHECK CONSTRAINT ALL
	INSERT INTO WorkstationJob (ProductID, StationID, ConfigurationID, StationName, EmployeeID, TrayID, BatchNumber, 
	OrderNumber, OrderQuantity, OrderAmount, YieldQuantity, QualityPassQuantity, QualityFailQuantity, JobStartDatetime, JobEndDatetime, "Status")
	VALUES (@FG_ProdId,	@StId, @ConfigurationID, @Stationname, @EmpId, @TrId, @BatchNumber, 
	@OrderNumber, @OrderQty, @OrderAmount, 0, 0, 0, @start_time, @end_time, 'InProgress')	

	---------- DELETE LATER?
	IF ((SELECT ProductName FROM Products WHERE ProductName=@ProductName) IS NULL)
		BEGIN
		ALTER TABLE BillOfMaterials NOCHECK CONSTRAINT ALL
		INSERT INTO BillOfMaterials (ParentProductID, ChildProductID, BomQuantity) VALUES (@FG_ProdId,@P1_ProdId, 1)
		INSERT INTO BillOfMaterials (ParentProductID, ChildProductID, BomQuantity) VALUES (@FG_ProdId,@P2_ProdId, 1)
		INSERT INTO BillOfMaterials (ParentProductID, ChildProductID, BomQuantity) VALUES (@FG_ProdId,@P3_ProdId, 1)
		INSERT INTO BillOfMaterials (ParentProductID, ChildProductID, BomQuantity) VALUES (@FG_ProdId,@P4_ProdId, 1)
		INSERT INTO BillOfMaterials (ParentProductID, ChildProductID, BomQuantity) VALUES (@FG_ProdId,@P5_ProdId, 1)
		INSERT INTO BillOfMaterials (ParentProductID, ChildProductID, BomQuantity) VALUES (@FG_ProdId,@P6_ProdId, 1)
		END

	PRINT 'DEBUG: ' + CAST(@TrId AS nvarchar) + ' '
	+ CAST(@P1_ProdId AS nvarchar) + ' Line' 
	+ CAST(@P2_ProdId AS nvarchar) + ' ' 
	+ CAST(@P3_ProdId AS nvarchar);

END TRY

BEGIN CATCH
	DECLARE @ErrorNumber int = ERROR_NUMBER()
	DECLARE @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
	DECLARE @ErrorState int = ERROR_STATE()
	DECLARE @ErrorLine int = ERROR_LINE()

	SET @return = @ErrorState
	PRINT CAST(@ErrorNumber AS nvarchar) + ' '
	+ CAST(@ErrorMessage AS nvarchar) + ' Line' 
	+ CAST(@ErrorLine AS nvarchar) + ' ' 
	+ CAST(@ErrorState AS nvarchar);
END CATCH
	RETURN @return
END
GO
