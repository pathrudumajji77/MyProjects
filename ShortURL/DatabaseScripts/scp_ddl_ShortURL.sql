USE [ShortURL]
GO
/****** Object:  Table [dbo].[URLInfo]    Script Date: 20/10/2019 11:49:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[URLInfo](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[LongURL] [nvarchar](max) NOT NULL,
	[ShortURLCode] [nvarchar](100) NOT NULL,
	[Active] [bit] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[ClicksCount] [int] NULL,
 CONSTRAINT [PK_URLInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[ShortURLCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[scp_mydb_get_LongURL]    Script Date: 20/10/2019 11:49:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[scp_mydb_get_LongURL]
	@ShortCode varchar(100)
	
AS
BEGIN
	
	
	SELECT LongURL FROM [URLInfo]
	WHERE [ShortURLCode] = @ShortCode

	UPDATE [URLInfo]
	SET [ClicksCount] = ISNULL([ClicksCount],0) + 1 
	WHERE [ShortURLCode] = @ShortCode



END


GO
/****** Object:  StoredProcedure [dbo].[scp_mydb_get_URLInfo]    Script Date: 20/10/2019 11:49:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[scp_mydb_get_URLInfo]
	@ShortCode varchar(100)
	
AS
BEGIN
	
	
	SELECT ID, LongURL,ShortURLCode,ClicksCount FROM [URLInfo]
	WHERE [ShortURLCode] = @ShortCode and Active = 1

	



END


GO
/****** Object:  StoredProcedure [dbo].[scp_mydb_ins_URLInfo]    Script Date: 20/10/2019 11:49:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[scp_mydb_ins_URLInfo]
	@LongURL varchar(max)
	
AS
BEGIN
	
	INSERT INTO [URLInfo](LongURL, ShortURLCode, Active, UpdatedOn, CreatedOn)
	OUTPUT Inserted.ID
	VALUES(@LongURL, '', 1, GETDATE(), GETDATE())

	--SELECT ID FROM [URLInfo] WHERE [LongURL] = @LongURL

END


GO
/****** Object:  StoredProcedure [dbo].[scp_mydb_upd_URLInfo]    Script Date: 20/10/2019 11:49:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[scp_mydb_upd_URLInfo]
	@Id bigint,
	@ShortCode varchar(100),
	@LongURL nvarchar(max)
	
AS
BEGIN
	
	Declare @Inserted bit

	IF NOT EXISTS( SELECT * FROM [URLInfo] WHERE [ShortURLCode] = @ShortCode)
	BEGIN
		UPDATE [URLInfo]
		SET [ShortURLCode] = @ShortCode, [UpdatedOn] = GetDate()
		WHERE [ID] = @Id

		SET @Inserted = 1
	END

	ELSE
	BEGIN
		SET @Inserted = 0
	END

	SELECT @Inserted

END


GO
