-- <Migration ID="c6112f3d-9a49-409a-acc2-7badd8cbeaa8" TransactionHandling="Custom" />
GO

PRINT N'Creating table [dbo].[DemoDatas]'
GO

IF OBJECT_ID('[dbo].[DemoDatas]') IS NOT NULL
	DROP TABLE [dbo].[DemoDatas];

CREATE TABLE [dbo].[DemoDatas] (
    [Id] uniqueidentifier NOT NULL,
    [Date] datetime2 NOT NULL
    CONSTRAINT [PK_DemoData_Id] PRIMARY KEY NONCLUSTERED HASH ([Id]) WITH (BUCKET_COUNT = 512),
    INDEX [CCI_DemoData] CLUSTERED COLUMNSTORE
)
WITH (
    MEMORY_OPTIMIZED = ON,
    DURABILITY = SCHEMA_AND_DATA
)
GO