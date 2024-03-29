USE [C134_pollyannabel21_gmail]
GO
/****** Object:  StoredProcedure [dbo].[Friends_UpdateV2]    Script Date: 3/8/2024 2:12:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[Friends_UpdateV2]
                 @Title nvarchar(50)
                ,@Bio   nvarchar(50)
                ,@Summary nvarchar(50)
                ,@Headline nvarchar(50)
                ,@Slug nvarchar(50)
                ,@StatusId int
                ,@ImageTypeId int
                ,@ImageUrl nvarchar(50)
                ,@UserId int
                ,@Id int
as

/*

Declare 
			@Title nvarchar(50) = 'Roy'
			,@Bio nvarchar(255) = 'RoysBio'
			,@Summary nvarchar(128) = 'RoysSummary'
			,@Headline nvarchar(50) = 'RoysHeadline'
			,@Slug nvarchar(50) = 'RoysSlug'
			,@StatusId int = 7
			,@ImageTypeId int = 4
			,@ImageUrl nvarchar(50) = 'ThisIsRaysUrl'
			,@UserId int = 7
			,@Id int
			
execute dbo.friends_SelectAllV2

Execute dbo.Friends_UpdateV2
	  
			@Title
			,@Bio
			,@Summary
			,@Headline
			,@Slug
			,@StatusId
			,@ImageTypeId
			,@ImageUrl
			,@UserId
			,@Id 

execute dbo.friends_SelectAllV2

*/

BEGIN
Update  dbo.Images
Set     [ImageTypeId] =@ImageTypeId
        ,[ImageUrl] =@ImageUrl
 Where ImageId = (Select PrimaryImageId
                from dbo.FriendsV2 as f
                Where f.Id = @Id)
			


Update      [dbo].FriendsV2
     SET   [Title] =@Title
           ,[Bio] =@Bio
           ,[Summary] =@Summary
           ,[Headline]=@Headline
           ,[Slug] =@Slug
           ,[StatusId]=@StatusId
            ,[UserId] =@UserId
    
    Where @Id = Id
        
        
END
GO
