USE [C134_pollyannabel21_gmail]
GO
/****** Object:  StoredProcedure [dbo].[Friends_SearchPaginationV3]    Script Date: 3/8/2024 2:12:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[Friends_SearchPaginationV3]  
		@PageIndex int 
        ,@PageSize int
		,@Query nvarchar(100)

/*

Declare @PageIndex int = 0
,@PageSize int = 10
, @Query nvarchar(100) = 'Yada'

Execute dbo.Friends_SearchPaginationV3 
@PageIndex
,@PageSize
,@Query

execute dbo.friends_selectAllV2


*/


AS

BEGIN

Declare @offset int = @PageIndex * @PageSize
Select fv.Id
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
	  ,TotalCount = COUNT(1) OVER()
  FROM [dbo].[FriendsV2] as fv inner join dbo.Images as i
	on fv.PrimaryImageId = i.ImageId
	WHERE (Title LIKE '%' + @Query + '%')
	ORDER BY fv.Id

	OFFSET @offSet Rows
	Fetch Next @PageSize Rows ONLY


END
GO
