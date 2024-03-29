USE [C134_pollyannabel21_gmail]
GO
/****** Object:  StoredProcedure [dbo].[Friends_InsertV2]    Script Date: 3/8/2024 2:12:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[Friends_InsertV2]
			@Title nvarchar(50)
			,@Bio nvarchar(255)
			,@Summary nvarchar(128)
			,@Headline nvarchar(50)
			,@Slug nvarchar(50)
			,@StatusId int
			,@ImageTypeId int
            ,@ImageUrl nvarchar(50)
			,@UserId int
			,@Id int OUTPUT

/*

Declare		@Title nvarchar(50) = 'Blob'
			,@Bio nvarchar(255) = 'BlobBio'
			,@Summary nvarchar(128) = 'BlobSummary'
			,@Headline nvarchar(50) = 'BlobHeadline'
			,@Slug nvarchar(50) = 'BlobSlug'
			,@StatusId int =  1
			,@ImageTypeId int = 1
			,@ImageUrl nvarchar(50) = 'BlobImageUrl'
			,@UserId int = 1
			,@Id int
			
    

Execute dbo.Friends_InsertV2
			@Title
			,@Bio
			,@Summary
			,@Headline
			,@Slug
			,@StatusId
			,@ImageTypeId
			,@ImageUrl
			,@UserId
			,@Id OUTPUT


Select*
from dbo.FriendsV2

Select*
From dbo.Images
*/
as


BEGIN



		INSERT INTO [dbo].[Images]
			(ImageTypeId
			,ImageUrl)

		 VALUES
			(@ImageTypeId
			,@ImageUrl)

		SET @Id = SCOPE_IDENTITY()

		INSERT INTO [dbo].[FriendsV2] 
				(Title
				,Bio
				,Summary
				,Headline
				,Slug
				,StatusId
				,PrimaryImageId
				,UserId)


			VALUES
           		(@Title
				,@Bio
				,@Summary
				,@Headline
				,@Slug
				,@StatusId
				,@Id
				,@UserId
				)


		SET @Id = SCOPE_IDENTITY()


END
GO
