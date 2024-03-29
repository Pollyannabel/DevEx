USE [C134_pollyannabel21_gmail]
GO
/****** Object:  StoredProcedure [dbo].[Friends_DeleteV2]    Script Date: 3/8/2024 2:12:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[Friends_DeleteV2]
		@Id int

as

/*

Declare @Id int = 1


Execute dbo.Friends_SelectAllV2
Select *
from dbo.Images

Execute dbo.Friends_DeleteV2 @Id

Execute dbo.Friends_SelectAllV2
Select *
from dbo.Images

*/


BEGIN
--declaring the variable name PrimaryImageId that matches with the column named PrimaryImageId
Declare @PrimaryImageId int = (Select fv.PrimaryImageId
from dbo.FriendsV2 as fv
where fv.Id = @Id)


DELETE 
FROM dbo.FriendsV2 
      WHERE Id = @Id


DELETE 
FROM [dbo].[Images]
      WHERE ImageId = @PrimaryImageId
	  

END


/*
declare @PrimaryImageId int = (Select fv.PrimaryImageId
								from dbo.FriendsV2 as fv
								where fv.Id = @Id)

delete from dbo.FriendsV2
where Id = @Id

Delete from dbo.Images
where Id = @PrimaryImageId

*/
GO
