USE [Kafka-dw]
GO

DROP TABLE [dbo].[fact_location_member_checkin]
GO
DROP TABLE [dbo].[dim_member]
GO
DROP TABLE [dbo].[dim_location]
GO



CREATE TABLE [dbo].[dim_member] (
    [member_key]  INT	IDENTITY (100001, 1)	NOT NULL,
    [first_name]   NVARCHAR (50)      NULL,
    [last_name]    NVARCHAR (50)      NULL,
    [member_alt_key]     UNIQUEIDENTIFIER   NULL,
    CONSTRAINT [PK_member] PRIMARY KEY CLUSTERED ([member_key] ASC)
);





CREATE TABLE [dbo].[dim_location] (
    [location_key]   INT	IDENTITY (100001, 1)	NOT NULL,
    [name]	NVARCHAR (50)      NULL,
    [location_alt_key]      UNIQUEIDENTIFIER   NULL,
    CONSTRAINT [PK_location] PRIMARY KEY CLUSTERED ([location_key] ASC)
);





CREATE TABLE [dbo].[fact_location_member_checkin] (
    [location_member_checkin_key]	INT	IDENTITY (100001, 1)	NOT NULL,
    [member_key]  INT	NOT NULL,
    [location_key]   INT	NOT NULL,
    [checkin_completed]  DATETIMEOFFSET (7) NULL,
    [checkin_alt_key]	UNIQUEIDENTIFIER	NOT NULL,
    CONSTRAINT [PK_checkin] PRIMARY KEY CLUSTERED ([location_member_checkin_key] ASC)
);





