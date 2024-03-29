USE [C134_pollyannabel21_gmail]
GO
/****** Object:  StoredProcedure [dbo].[Friends_Insert]    Script Date: 3/8/2024 2:12:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE proc [dbo].[Friends_Insert]


@Title nvarchar(50)
,@Bio nvarchar(255)
,@Summary nvarchar(128)
,@Headline nvarchar(50)
,@Slug nvarchar(50)
,@StatusId int
,@PrimaryImageUrl nvarchar(255)
,@UserId int
,@Id int OUTPUT

/*-----------Test Code---------------------


	Declare @Title nvarchar(50) = 'Grace'
			,@Bio nvarchar(255) = 'Loves chocolate'
			,@Summary nvarchar(128) = 'Lady eats chocolate'
			,@Headline nvarchar(50) = 'Chocolate lover'
			,@Slug nvarchar(50) = 'Grace's slug'
			,@StatusId int = 1
			,@PrimaryImageUrl nvarchar(255) = 'picture'
			,@UserId int = 7
			,@Id int = 0;

	Execute dbo.Friends_Insert 
					@Title
					,@Bio
					,@Summary
					,@Headline
					,@Slug
					,@StatusId
					,@PrimaryImageUrl
					,@UserId
					,@Id OUTPUT

	Execute dbo.Friends_SelectAll
	
*/

as

BEGIN



INSERT INTO [dbo].[Friends]
           (
           [Title]
           ,[Bio]
           ,[Summary]
		   ,[Headline]
           ,[Slug]
           ,[StatusId]
           ,[PrimaryImageUrl]
           ,[UserId]
		   )
     VALUES
         (
		@Title
		,@Bio
		,@Summary
		,@Headline
		,@Slug
		,@StatusId
		,@PrimaryImageUrl
		,@UserId)

	SET @Id = SCOPE_IDENTITY()



END
GO
