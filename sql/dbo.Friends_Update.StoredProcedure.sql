USE [C134_pollyannabel21_gmail]
GO
/****** Object:  StoredProcedure [dbo].[Friends_Update]    Script Date: 3/8/2024 2:12:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Friends_Update]
			@Title nvarchar(50)
			,@Bio nvarchar(255)
			,@Summary nvarchar(128)
			,@Headline nvarchar(50)
			,@Slug nvarchar(50)
			,@StatusId int
			,@PrimaryImageUrl nvarchar(255)
			,@UserId int
			,@Id int

as

/*

Declare
@Title nvarchar(50) = 'Gordy',
@Bio nvarchar(255) = 'Bio',
@Summary nvarchar(128) = 'Summary',
@Headline nvarchar(50) = 'Headline',
@Slug nvarchar(50) = 'Slug',
@StatusId int = 1,
@PrimaryImageUrl nvarchar(255) = 'PrimaryImg',
@UserId int = 4,
@Id int = 2

Select *
From dbo.Friends
Where Id = @Id

Execute dbo.Friends_Update
@Title,
@Bio,
@Summary,
@Headline,
@Slug,
@StatusId,
@PrimaryImageUrl,
@UserId,
@Id

Select *
From dbo.Friends
Where Id = @Id

*/


BEGIN


UPDATE [dbo].[Friends]
   SET [Title] = @Title
      ,[Bio] = @Bio
      ,[Summary] = @Summary
      ,[Headline] = @Headline
      ,[Slug] = @Slug
      ,[StatusId] = @StatusId
      ,[PrimaryImageUrl] = @PrimaryImageUrl
      ,[UserId] = @UserId
 WHERE Id = @Id


END
GO
