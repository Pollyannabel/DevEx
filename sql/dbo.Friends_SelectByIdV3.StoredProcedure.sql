USE [C134_pollyannabel21_gmail]
GO
/****** Object:  StoredProcedure [dbo].[Friends_SelectByIdV3]    Script Date: 3/8/2024 2:12:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[Friends_SelectByIdV3]
		@Id int

/*

Declare @Id int = 25

Execute dbo.Friends_SelectByIdV3 @Id

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
	  ,i.ImageId
	  ,i.ImageTypeId
	  ,i.ImageUrl
	  ,Skills = (
				select s.Id
						,s.Name
				from dbo.Skills as s inner join dbo.FriendSkills as fs
					on s.Id = fs.SkillId
				where fv.Id = fs.FriendId
				for JSON AUTO
				)


	  ,fv.DateCreated
      ,fv.DateModified
	  ,fv.UserId
	  
  FROM [dbo].[FriendsV2] as fv inner join dbo.Images as i
	on fv.PrimaryImageId = i.ImageId
	Where fv.Id = @Id

	/*
select *
from dbo.skills

select *
from dbo.FriendSkills

select*
from dbo.friendsV2
*/

END


GO
