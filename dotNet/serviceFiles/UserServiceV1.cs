using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data;
using System.Net;
using Sabio.Models.Requests.Users;
using Sabio.Models.Requests.Addresses;
using Sabio.Models.Domain.Users;
using Sabio.Services.Interfaces;
using Sabio.Models.Domain.Friends;
using Sabio.Models;

namespace Sabio.Services
{
    public class UserServiceV1 : IUserServiceV1
    {
        IDataProvider _data = null;

        //Constructor function
        public UserServiceV1(IDataProvider data)
        {
            _data = data;
        }

        public void Delete(int target)
        {
            string procName = "[dbo].[Users_Delete]";
            _data.ExecuteNonQuery
                (
                procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {

                    col.AddWithValue("@Id", target);


                },
                returnParameters: null);
        }

        public int Add(UserAddRequest model, int userId)
        {
            int id = 0; //must be null to start and then will be returned with newly created value from output.
            string procName = "[dbo].[Users_Insert]";
            _data.ExecuteNonQuery(
               procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);
                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;
                    int.TryParse(oId.ToString(), out id);
                });
            return id;
        }

        public void Update(UserUpdateRequest model) //updates are void because they don't return anything. Takes the model of the address.
        {
            string procName = "[dbo].[Users_Update]";
            _data.ExecuteNonQuery(
                 procName,
                 inputParamMapper: delegate (SqlParameterCollection col)
                 {
                     AddCommonParams(model, col);
                     col.AddWithValue("@Id", model.Id);

                 },
                 returnParameters: null);
        }

        public User Get(int Id)
        {
            string procName = "[dbo].[Users_SelectById]";

            User user = null;

            _data.ExecuteCmd(
                procName,
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    // the mapper takes one shape and returns a new shape
                    //I have a number (Id )and I need to make a parameter that has a number

                    paramCollection.AddWithValue("@Id", Id);
                    //@Id is the parameter present in SQL database and the Id is the actual number that will be passed to it in the request.
                },

                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    user = MapSingleUser(reader);
                }
                );

            return user;
        }


        public List<User> GetAll()
        {
            List<User> list = null;
            string procName = "[dbo].[Users_SelectAll]";
            _data.ExecuteCmd(procName,
                inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    User aUser = MapSingleUser(reader);


                    if (list == null)
                    {
                        list = new List<User>();
                    }

                    list.Add(aUser);
                }
                );

            return list;
        }

        public Paged<User> GetPage(int pageIndex, int pageSize)
        {
            Paged<User> pagedList = null;
            List<User> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Users_Pagination]";

            _data.ExecuteCmd(
                procName,
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                },
                (reader, recordSetIndex) =>
                {
                    User user = MapSingleUser(reader);
                    totalCount = reader.GetSafeInt32(6);

                    if (list == null)
                    {
                        list = new List<User>();
                    }
                    list.Add(user);

                }
                );

            if (list != null)
            {
                pagedList = new Paged<User>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;

        }


        private static User MapSingleUser(IDataReader reader)
        {
            User user = new User(); //every time the mapper runs, it will be a new instance of a User class/type, assigned to the variable address.

            int startingIndex = 0;

            user.Id = reader.GetSafeInt32(startingIndex++);
            user.FirstName = reader.GetSafeString(startingIndex++);
            user.LastName = reader.GetSafeString(startingIndex++);
            user.Email = reader.GetSafeString(startingIndex++);
            user.TenantId = reader.GetSafeString(startingIndex++);
            user.AvatarUrl = reader.GetSafeString(startingIndex++);
            user.DateCreated = reader.GetSafeDateTime(startingIndex++);
            user.DateModified = reader.GetSafeDateTime(startingIndex++);
            return user;
        }

        private static void AddCommonParams(UserAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@FirstName", model.FirstName);
            col.AddWithValue("@LastName", model.LastName);
            col.AddWithValue("@Email", model.Email);
            col.AddWithValue("@Password", model.Password);
            col.AddWithValue("@AvatarUrl", model.AvatarUrl);
            col.AddWithValue("@TenantId", model.TenantId);
        }

    }
}



