using Sabio.Data.Providers;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Requests.Friends;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data;
using Sabio.Services.Interfaces;
using Sabio.Models;

namespace Sabio.Services
{
    public class FriendService : IFriendService//The Interface communicates with the Controller.
    {
        //IDataProvider allows us to execute commands and bring back data from the SQL database.
        IDataProvider _data = null;

        //Constructor function will automatically be using the data from the database whenever the FriendService is instantiated.
        public FriendService(IDataProvider data)
        {
            _data = data;
        }

        //The Add function is public and it returns and integer. It requires the model and the userId to be passed to it.
        public int Add(FriendAddRequest model, int userId)
        {
            //The id will be returned when this method is fully executed. It starts out as  zero so it saves a place in memory.
            int id = 0;

            //The procName is the location where the command will be carried out in SQL. It is the first argument passed into the command below.
            string procName = "[dbo].[Friends_Insert]";

            //With the data we have access to in the database, we execute a NonQuery because we are not expecting an object to be returned to us in an Add. 
            _data.ExecuteNonQuery(
                procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    //The inputParamMapper takes the information we're adding and creates a space within the database with the specified value.
                    //The SqlParameterCollection allows us to use the AddWithValue method.
                    AddCommonParams(model, col); //The AddCommonParams is a private function that takes the params that are used within several models (add and update)
                                                 //and adds the values to the collection.
                    col.AddWithValue("@UserId", userId);//The userId is not shared in several models, so it is added separately from the CommonParams.

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);//The id that is created from the scope_identity in sql is an OUTPUT and must
                                                                                //therefore be captured and specified with a value of int for .NET
                    idOut.Direction = ParameterDirection.Output;//We are now taking that captured value and assigning it the direction of OUT as is specified by SQL.

                    col.Add(idOut);//Now that the id has been captured, it can now be added to the collection with its value.
                },
                returnParameters: delegate (SqlParameterCollection returnCollection)//We are utilizing the returnParameters because even though we are not returning
                                                                                    //an object (like in a Get), we are receiving the Id that we need to be an integer.
                {
                    object oId = returnCollection["@Id"].Value;//the Id will be returned as an object, but we don't want it to be an object, so we are grabbing the
                                                               //value that is within the object.
                    int.TryParse(oId.ToString(), out id);//the value of the object is then broken down into string form which is then turned into an integer, which is the correct form we want.
                });
            return id;//The only thing this command is returning is an id that is now an integer (like the signature specifies).
        }

        public void Update(FriendUpdateRequest model, int userId)//The Update function does not return anything so it is a public void.
            //It uses the FriendUpdateRequest model to shape its information and we use the userId to add to the collection.
        {
            string procName = "[dbo].[Friends_Update]";//The string procName is the location where the command will be carried out in SQL.
            _data.ExecuteNonQuery(//It is a NonQuery (and void) because we are not expecting to receive information back.
                procName,//sql proc
                inputParamMapper: delegate (SqlParameterCollection col) //Because we are sending information to the database, we utilize the SqlParameterColelction to add values.
                {
                    AddCommonParams(model, col);//the AddCommonParams uses the model data to add to the collection.
                    col.AddWithValue("@UserId", userId);//The userId is not part of the AddCommonParams because it is being passed as an argument to complete the command.
                    col.AddWithValue("@Id", model.Id);//The Id is not part of the AddCommonParams, but it's in the model and needs a value.

                },
                returnParameters: null);//Nothing is being returned.
        }

        public void Delete(int target)//Just like the Update, Delete is not returning anything and is therefore void.
            //The target is the integer value (could also be called id) that is associated with the record (located at the "@Id" parameter in sql).
        {
            string procName = "[dbo].[Friends_Delete]";//the procName is what we are using to carry out the command on the specified table.
            _data.ExecuteNonQuery//We are not expecting anything to be returned, so it is a NonQuery just like Update and Add(add gets an integer, not an object)
                (
                procName,
                inputParamMapper: delegate (SqlParameterCollection col)//We must utilize the SqlParameterCollection because it needs to know which record to perform the command on.
                {
                    col.AddWithValue("@Id", target);//The target is the integer value (aka the id) of the record we wish to delete from the database.
                },
                returnParameters: null);//Nothing is being returned.
        }


        #region GetAll
        public List<Friend> GetAll()
        {
            List<Friend> list = null;
            string procName = "[dbo].[Friends_SelectAll]";
            _data.ExecuteCmd(procName,
                inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex  = 0;
                    Friend aFriend = MapSingleFriend(reader, ref startingIndex);

                    if (list == null)
                    {
                        list = new List<Friend>();
                    }

                    list.Add(aFriend);
                }
                );

            return list;
        }

        public List<FriendV2> GetAllV2()
        {
            List<FriendV2> list = null;
            string procName = "[dbo].[Friends_SelectAllV2]";
            _data.ExecuteCmd(procName,
                inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    FriendV2 aFriend = MapSingleFriendV2(reader, ref startingIndex);


                    if (list == null)
                    {
                        list = new List<FriendV2>();
                    }

                    list.Add(aFriend);
                }
                );

            return list;
        }

        public List<FriendV3> GetAllV3()//Because we are expecting more than one record, it will be a List type of the specified model type. No parameters/arguments are needed.
        {
            List<FriendV3> list = null;//The list starts out null because we are creating a place for it in memory.
            string procName = "[dbo].[Friends_SelectAllV3]";

            _data.ExecuteCmd//This is an ExecuteCmd because we are telling the database we want something to come back, in this case, a list of Friend objects.
                (procName,
                inputParamMapper: null,//because this is a GetAll and requires no params/arguments, the inputParamMapper is null and does not "Add Value" to the collection.
                singleRecordMapper: delegate (IDataReader reader, short set)//Now that we are receiving an object back, we need it to be mapped. The IDataReader receives the information
                                                                            //from the database and the mapper assigns the values to the properties in the model.
                                                                            //We are using one table, so it is a short set.
                {
                    int startingIndex = 0;
                    FriendV3 aFriend = MapSingleFriendV3(reader, ref startingIndex);//as the mapper assigns values to the properties, the index will increment through the columns in the
                                                                                    //database table's info that the Reader has.
                                                                                    //Every time the mapper runs through one friend it is assigned to the variable friend that is
                                                                                    //of FriendV3 model type.


                    if (list == null)//If there is no information in the list, we will create a new List of FriendV3.
                    {
                        list = new List<FriendV3>();
                    }

                    list.Add(aFriend);//If the list has values, add the newly mapped friend to the existing list. If we didn't first do the If statement, we would constantly
                                      //have a list of only one friend because it would be replaced each time the mapper runs.
                }
                );

            return list;//We are returning a list.
        }
        #endregion

        #region GetById
        public Friend Get(int Id)
        {
            string procName = "[dbo].[Friends_SelectById]";

            Friend friend = null;

            _data.ExecuteCmd(
                procName,
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", Id);
                },

                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    friend = MapSingleFriend(reader, ref startingIndex);
                }
                );

            return friend;
        }//GetById

        public FriendV2 GetV2(int Id)
        {
            string procName = "[dbo].[Friends_SelectByIdV2]";

            FriendV2 friend = null;

            _data.ExecuteCmd(
                procName,
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", Id);
                },

                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    friend = MapSingleFriendV2(reader, ref startingIndex);
                }
                );

            return friend;
        }//GetByIdV2

        public FriendV3 GetV3(int Id)
        {//We are returning only one object that will take the shape of the FriendV3 model. The Id of that specific friend is the parameter/integer.
            FriendV3 friend = null;//An empty object is created to save a space in memeory.

            string procName = "[dbo].[Friends_SelectByIdV3]";

            _data.ExecuteCmd//We are expecting an object back, so we will be executing a command.
                (procName
                , inputParamMapper: delegate (SqlParameterCollection col)//Because we require an integer Id as an argument, it must be added to the SqlParameterCollection.
                {
                    col.AddWithValue("@Id", Id);//The value of the argument is added to the collection to be returned in the object model.
                }
            , singleRecordMapper: delegate (IDataReader reader, short set)//Because we are receiving an object, it must be mapped.The reader taked the data and increments
                                                                          //through the columns and assigns the values to the properties in the model.
            {
                int startingIndex = 0;
                friend = MapSingleFriendV3(reader, ref startingIndex);
            }
            );

            return friend;//A friend object is returned.
        }//GetByIdV3
        #endregion

        #region Pagination
        public Paged<Friend> GetPage(int pageIndex, int pageSize)
        {
            Paged<Friend> pagedList = null;
            List<Friend> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Friends_Pagination]";

            _data.ExecuteCmd(
                procName,
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    Friend friend = MapSingleFriend(reader, ref startingIndex);
                    totalCount = reader.GetSafeInt32(startingIndex++);

                    if (list == null)
                    {
                        list = new List<Friend>();
                    }
                    list.Add(friend);

                }
                );

            if (list != null)
            {
                pagedList = new Paged<Friend>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;

        }//Pagination

        public Paged<FriendV2> GetPageV2(int pageIndex, int pageSize)//PaginationV2
        {
            Paged<FriendV2> pagedList = null;
            List<FriendV2> friendList = null;
            int totalCount = 0;
            string procName = "[dbo].[Friends_PaginationV2]";

            _data.ExecuteCmd(
                procName,
                inputParamMapper: delegate (SqlParameterCollection col)

                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    FriendV2 friend = MapSingleFriendV2(reader, ref startingIndex);
                    totalCount = reader.GetSafeInt32(startingIndex++);

                    if (friendList == null)
                    {
                        friendList = new List<FriendV2>();
                    }
                    friendList.Add(friend);

                }
                );

            if (friendList != null)
            {
                pagedList = new Paged<FriendV2>(friendList, pageIndex, pageSize, totalCount);
            }
            return pagedList;

        } 

        public Paged<FriendV3> GetPageV3 (int pageIndex, int pageSize) //We expect a page full of Friend Objects. It is a list on a page.
                                                                       //We must receive the pageIndex and pageSize.
        { 
            Paged<FriendV3> pagedList = null;//The page is empty.
            List<FriendV3> friendList = null;//The list is empty.
            int totalCount = 0;

            string procName = "[dbo].[Friends_PaginationV3]";

            _data.ExecuteCmd//We are expecting a pagedList returned to us so we are executing a command to the database.
                (procName
                , inputParamMapper: delegate (SqlParameterCollection col)//The parameters in the function are added to the collection to create the specific list of data.
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);

            }
            , singleRecordMapper: delegate (IDataReader reader, short set)//Because we have an object, it needs to be mapped.
                                                                          //The information from the reader will be assigned to the properties of the model.
           
            {
                int startingIndex = 0;
                FriendV3 model = MapSingleFriendV3(reader, ref startingIndex);

                if (totalCount == 0)//if the count is 0 (the default value at top),
                                    //go into the reader and grab the value from the specified index of the object and assign it to the variable.
                                    //total count is not currently in the mapping function, so it must be manually added.
                {
                    totalCount = reader.GetSafeInt32(startingIndex++);
                }
                if (friendList == null)//if the friendList is null (the default value at top),
                                       //create a new instance of a List of FriendV3 type. 
                {
                    friendList = new List<FriendV3>();
                }
                friendList.Add(model);//add the newly mapped friend (called model in this case) to the friendList.
            });
            if (friendList != null) //If the friend list has something in it, pass it into a new Paged list of friends and assign it to the previously null
                                    //pagedList along with the page index, size, and count.
            {
                pagedList = new Paged<FriendV3>(friendList, pageIndex, pageSize, totalCount);
            }
            return pagedList;//Return the paged list of friends.
        }//PaginationV3
        #endregion

        #region Search_Pagination
        public Paged<Friend> SearchPaged(int pageIndex, int pageSize, string query)//Search_Pagination
        {
            Paged<Friend> pagedList = null;
            List<Friend> friendList = null;
            int totalCount = 0;

            string procName = "[dbo].[Friends_Search_Pagination]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
                col.AddWithValue("@Query", query);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                Friend friend = MapSingleFriend(reader, ref startingIndex);
                totalCount = reader.GetSafeInt32(startingIndex++);

                if (friendList == null)
                {
                    friendList = new List<Friend>();
                }
                friendList.Add(friend);
            }
            );

            if (friendList != null)
            {
                pagedList = new Paged<Friend>(friendList, pageIndex, pageSize, totalCount);
            }

            return pagedList;

        }

        public Paged<FriendV2> SearchPagedV2(int pageIndex, int pageSize, string query)//Search_PaginationV2
        {
            Paged<FriendV2> pagedList = null;
            List<FriendV2> friendList = null;
            int totalCount = 0;
            string procName = "[dbo].[Friends_Search_PaginationV2]";

            _data.ExecuteCmd(
                procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                    col.AddWithValue("@Query", query);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    FriendV2 friend = MapSingleFriendV2(reader, ref startingIndex);
                    totalCount = reader.GetSafeInt32(startingIndex++);

                    if (friendList == null)
                    {
                        friendList = new List<FriendV2>();
                    }
                    friendList.Add(friend);

                }
                );

            if (friendList != null)
            {
                pagedList = new Paged<FriendV2>(friendList, pageIndex, pageSize, totalCount);
            }
            return pagedList;

        } 

        public Paged<FriendV3> SearchPagedV3(int pageIndex, int pageSize, string query)//Search_PaginationV3
        {
            Paged<FriendV3> pagedList = null;//the paged list will be null to start.
            List<FriendV3> friendList = null;//the list of friends will be null.
            int totalCount = 0;//the total count will be null until it is returned from the reader.
            string procName = "[dbo].[Friends_SearchPaginationV3]";

            _data.ExecuteCmd
                (procName
                , inputParamMapper: delegate (SqlParameterCollection col)//Because we have parameters that are passed, we need to add the values to the collection.
            {
                col.AddWithValue("@PageIndex", pageIndex);//the parameters/arguments are added to the specified columns within the sql collection.
                col.AddWithValue("@PageSize", pageSize);
                col.AddWithValue("@Query", query);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                FriendV3 friend = MapSingleFriendV3(reader, ref startingIndex);

                if (totalCount == 0)//if the totalCount is 0, go in an grab the indexed value and assign it to the variable.
                {
                    totalCount = reader.GetSafeInt32(startingIndex++);
                }
                if (friendList == null)//if the friendList is empty, create a new list of the friend object model.
                {
                    friendList = new List<FriendV3>();//if we didn't do this, the list we ould replaced every time it was mapped and it would be a page of one friend.
                }
                friendList.Add(friend);//add the newly mapped friend to the existing list.
            });

            if(friendList != null) //if there is a list of friends object, pass it into a new instance of the Paged Friend along with the other parameters and total count.
            { 
                pagedList = new Paged<FriendV3>(friendList,pageIndex,pageSize, totalCount);
            }
            return pagedList;//return the paged list of friends.
        }
        #endregion

        #region Mappers
        private static Friend MapSingleFriend(IDataReader reader, ref int startingIndex)
        {
            Friend friend = new Friend();

            friend.Id = reader.GetSafeInt32(startingIndex++);
            friend.Title = reader.GetSafeString(startingIndex++);
            friend.Bio = reader.GetSafeString(startingIndex++);
            friend.Summary = reader.GetSafeString(startingIndex++);
            friend.Headline = reader.GetSafeString(startingIndex++);
            friend.Slug = reader.GetSafeString(startingIndex++);
            friend.StatusId = reader.GetSafeInt32(startingIndex++);
            friend.PrimaryImageUrl = reader.GetSafeString(startingIndex++);
            friend.DateCreated = reader.GetSafeDateTime(startingIndex++);
            friend.DateModified = reader.GetSafeDateTime(startingIndex++);
            friend.UserId = reader.GetSafeInt32(startingIndex++);

            return friend;
        }

        private static FriendV2 MapSingleFriendV2(IDataReader reader, ref int startingIndex)
        {
            FriendV2 friend = new FriendV2();
            friend.PrimaryImage = new Image();


            friend.Id = reader.GetSafeInt32(startingIndex++);
            friend.Title = reader.GetSafeString(startingIndex++);
            friend.Bio = reader.GetSafeString(startingIndex++);
            friend.Summary = reader.GetSafeString(startingIndex++);
            friend.Headline = reader.GetSafeString(startingIndex++);
            friend.Slug = reader.GetSafeString(startingIndex++);
            friend.StatusId = reader.GetSafeInt32(startingIndex++);
            friend.PrimaryImage.Id = reader.GetSafeInt32(startingIndex++);
            friend.PrimaryImage.TypeId = reader.GetSafeInt32(startingIndex++);
            friend.PrimaryImage.Url = reader.GetSafeString(startingIndex++);
            friend.DateCreated = reader.GetSafeDateTime(startingIndex++);
            friend.DateModified = reader.GetSafeDateTime(startingIndex++);
            friend.UserId = reader.GetSafeInt32(startingIndex++);

            return friend;
        }

        private static FriendV3 MapSingleFriendV3(IDataReader reader, ref int startingIndex)
        {
            FriendV3 friend = new FriendV3();
            friend.PrimaryImage = new Image();//A new instance of Image is needed in order to access the properties.


            friend.Id = reader.GetSafeInt32(startingIndex++);
            friend.Title = reader.GetSafeString(startingIndex++);
            friend.Bio = reader.GetSafeString(startingIndex++);
            friend.Summary = reader.GetSafeString(startingIndex++);
            friend.Headline = reader.GetSafeString(startingIndex++);
            friend.Slug = reader.GetSafeString(startingIndex++);
            friend.StatusId = reader.GetSafeInt32(startingIndex++);
            friend.PrimaryImage.Id = reader.GetSafeInt32(startingIndex++);
            friend.PrimaryImage.TypeId = reader.GetSafeInt32(startingIndex++);
            friend.PrimaryImage.Url = reader.GetSafeString(startingIndex++);;
            friend.DateCreated = reader.GetSafeDateTime(startingIndex++);
            friend.DateModified = reader.GetSafeDateTime(startingIndex++);
            friend.UserId = reader.GetSafeInt32(startingIndex++);
            friend.Skills = reader.DeserializeObject<List<Skill>>(startingIndex++);

            return friend;
        }
        #endregion

        #region CommonParams
        private static void AddCommonParams(FriendAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Title", model.Title);
            col.AddWithValue("@Bio", model.Bio);
            col.AddWithValue("@Summary", model.Summary);
            col.AddWithValue("@Headline", model.Headline);
            col.AddWithValue("@Slug", model.Slug);
            col.AddWithValue("@StatusId", model.StatusId);
            col.AddWithValue("@PrimaryImageUrl", model.PrimaryImageUrl);

        }

        private static void AddCommonParamsV2(FriendV2AddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Title", model.Title);
            col.AddWithValue("@Bio", model.Bio);
            col.AddWithValue("@Summary", model.Summary);
            col.AddWithValue("@Headline", model.Headline);
            col.AddWithValue("@Slug", model.Slug);
            col.AddWithValue("@StatusId", model.StatusId);
            col.AddWithValue("@PrimaryImageUrl", model.PrimaryImageUrl);
            col.AddWithValue("@ImageTypeId", model.ImageTypeId);

        }

        private static void AddCommonParamsV3(FriendV3AddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Title", model.Title);
            col.AddWithValue("@Bio", model.Bio);
            col.AddWithValue("@Summary", model.Summary);
            col.AddWithValue("@Headline", model.Headline);
            col.AddWithValue("@Slug", model.Slug);
            col.AddWithValue("@StatusId", model.StatusId);
            col.AddWithValue("@PrimaryImageUrl", model.PrimaryImageUrl);
            col.AddWithValue("@ImageTypeId", model.ImageTypeId);

        } 
        #endregion

    }
}


