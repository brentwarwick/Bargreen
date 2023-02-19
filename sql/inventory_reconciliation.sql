--NOTE: The decimal data type was changed to decimal(10,2) to prevent rounding

--Create a table to hold inventory balances:
declare @inventory table (
    ItemNumber varchar(50) not null,
    WarehouseLocation varchar(50) not null,
    QuantityOnHand int not null,
    PricePerItem decimal(10,2) not null
)

--Create a table to hold accounting balances: 
declare @accounting table (
    ItemNumber varchar(50) not null,
    TotalInventoryValue decimal(10,2) not null
)

--Mock up some inventory balances
INSERT INTO @inventory VALUES ('ABC123', 'WLA1', 312, 7.5)
INSERT INTO @inventory VALUES ('ABC123', 'WLA2', 146, 7.5)
INSERT INTO @inventory VALUES ('ZZZ99', 'WLA3', 47, 13.99)
INSERT INTO @inventory VALUES ('zzz99', 'WLA4', 91, 13.99)
INSERT INTO @inventory VALUES ('xxccM', 'WLA5', 32, 245.25)
INSERT INTO @inventory VALUES ('xxddM', 'WLA6', 15, 747.47)

--Mock up some accounting balances
INSERT INTO @accounting VALUES ('ABC123', 3435)
INSERT INTO @accounting VALUES ('ZZZ99', 1930.62)
INSERT INTO @accounting VALUES ('xxccM', 7602.75)
INSERT INTO @accounting VALUES ('fbr77', 17.99)

SELECT inv.ItemNumber as InvItemNumber, acc.ItemNumber as AccItemNumber, InvTotal, TotalInventoryValue
	FROM (SELECT ItemNumber Collate SQL_Latin1_General_CP1_CS_AS AS ItemNumber, Sum(QuantityOnHand * PricePeritem) AS InvTotal FROM @inventory GROUP BY ItemNumber Collate SQL_Latin1_General_CP1_CS_AS) inv
	FULL JOIN @accounting acc ON inv.ItemNumber = acc.ItemNumber
	WHERE (InvTotal != TotalInventoryValue) OR (InvTotal IS NULL AND TotalInventoryValue IS NOT NULL) OR (InvTotal IS NOT NULL AND TotalInventoryValue IS NULL);