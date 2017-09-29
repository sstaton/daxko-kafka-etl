
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shaun Staton
-- Create date: 9/28/17
-- Description:	Writes a checkin message to appropriate tables
-- =============================================
CREATE PROCEDURE [dbo].[CheckinMessageToInsertStmts]
	-- Add the parameters for the stored procedure here
	@chk_id UNIQUEIDENTIFIER,
	@mem_id UNIQUEIDENTIFIER,
	@loc_id UNIQUEIDENTIFIER,
	@chk_completed DATETIME,
	@first_name nvarchar(50),
	@last_name nvarchar(50),
	@loc_name nvarchar(50)
	--@chk_created DATETIME,
	--@mem_created DATETIME,
	--@loc_created DATETIME,
	--@Phone nvarchar(50),
	--@Email nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	MERGE [dbo].[dim_location] AS T
	USING (SELECT @loc_name AS [name], @loc_id AS [key]) AS S 
	ON T.[location_alt_key] = S.[key]
	WHEN MATCHED THEN  
		UPDATE SET T.[name] = S.[name]
	WHEN NOT MATCHED THEN  
		INSERT ([name],[location_alt_key]) VALUES (S.[name],S.[key]);

	MERGE [dbo].[dim_member] AS T
	USING (SELECT @first_name AS [first_name], @last_name AS [last_name], @mem_id AS [key]) AS S 
	ON T.[member_alt_key] = S.[key]
	WHEN MATCHED THEN  
		UPDATE SET T.[first_name] = S.[first_name], T.[last_name] = S.[last_name]
	WHEN NOT MATCHED THEN  
		INSERT ([first_name],[last_name],[member_alt_key]) VALUES (S.[first_name],S.[last_name],S.[key]);

	INSERT INTO [dbo].[fact_location_member_checkin]
				([location_key]
				,[member_key]
				,[checkin_completed]
				,[checkin_alt_key])
			VALUES
				((SELECT TOP 1 [location_key] FROM [dbo].dim_location WHERE location_alt_key=@loc_id)
				,(SELECT TOP 1 [member_key] FROM [dbo].dim_member WHERE member_alt_key=@mem_id)
				,@chk_completed
				,@chk_id);


END
GO
