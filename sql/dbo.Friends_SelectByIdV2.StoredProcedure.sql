USE [C134_pollyannabel21_gmail]
GO
/****** Object:  StoredProcedure [dbo].[Friends_SelectByIdV2]    Script Date: 3/8/2024 2:12:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[Friends_SelectByIdV2]
		@Id int

/*

Declare @Id int = 2

Execute dbo.Friends_SelectByIdV2 @Id

*/

as

BEGIN


  SELECT fv.Id
      ,fv.Title
      ,fv.Bio
      ,fv.Summary
      ,fv.Headline
      ,fv.Slug
      ,fv.StatusId
      ,fv.PrimaryImageId
	  ,i.ImageTypeId
	  ,i.ImageUrl
	  ,fv.DateCreated
      ,fv.DateModified
	  ,fv.UserId
  FROM [dbo].[FriendsV2] as fv inner join dbo.Images as i
	on fv.PrimaryImageId = i.ImageId
	Where fv.Id = @Id

END
GO
