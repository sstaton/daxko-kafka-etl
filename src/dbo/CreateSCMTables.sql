        CREATE TABLE [dbo].[CustomReportSettingsTBL] (
              [id] int IDENTITY(1 ,1) NOT NULL,
              [admin_user_name] varchar(500) Not NULL,
              [admin_password] varchar(500) NOT NULL,
              [sso_password] varchar(1000) NOT NULL,
              [space_id] varchar(1000) NOT NULL,
			  [basic_group] varchar(500) NOT NULL,
			  [dashboard] varchar(500) NOT NULL,
			  [dashboard_page] varchar(500) NOT NULL
        )
