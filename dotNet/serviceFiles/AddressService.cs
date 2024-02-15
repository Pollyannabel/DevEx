using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data;
using System.Net;
using Sabio.Models.Requests.Addresses;
using System.Reflection;
using Sabio.Models.Domain.Addresses;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class AddressService : IAddressService
    //Where the colon separates the two, we can say that the class AddressService IMPLEMENTS the IAddressService Interface.
    {

        IDataProvider _data = null; //SqlDataProvider IMPLEMENTS IDataProvider.

        //This is the constructor(below). (Constructors are methods that don't have a return type and it is named after the class it is encapsulated in. It is meant to create an instance of this class.) Takes place on the first instance of the object.
        public AddressService(IDataProvider data)  //AddressesService function receives a parameter in the shape of the IDataProvider class/type.
        {
            _data = data;

        }

        public void Delete(int target)
        {
            string procName = "[dbo].[Sabio_Addresses_DeleteById]";
            _data.ExecuteNonQuery
                (
                procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {

                    col.AddWithValue("@Id", target);


                },
                returnParameters: null);
        }

        public void Update(AddressUpdateRequest model) //updates are void because they don't return anything. Takes the model of the address.
        {
            string procName = "[dbo].[Sabio_Addresses_Update]";
            _data.ExecuteNonQuery(
                procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@Id", model.Id);

                },
                returnParameters: null);
        }

        public List<int> AddMany(AddressAddRequest model)
        {
            return null;
        }

        public int Add(AddressAddRequest model, int userId)
        {

            int id = 0; //must be null to start and then will be returned with newly created value from output.


            string procName = "[dbo].[Sabio_Addresses_Insert]";
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

        public Address Get(int Id)
        {
            //[dbo].[Sabio_Addresses_SelectById]
            //@Id int

            string procName = "[dbo].[Sabio_Addresses_SelectById]";

            Address address = null; //address will be empty until it is populated with the information it receives from the database (returned as a new class instance of an address)
            //after assigning the address as null and returning it later, include it in the Program.cs file where the Get is invoked with the actual Id number and assigned to a new variable.

            //the following .ExecuteCmd() is a function in the SqlDataProvider file that dictates the shape of the information being sent or received.
            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    // the mapper takes one shape and returns a new shape
                    //I have a number (Id )and I need to make a parameter that has a number

                    paramCollection.AddWithValue("@Id", Id);
                    //@Id is the parameter present in SQL database and the Id is the actual number that will be passed to it in the request.

                },

                singleRecordMapper: delegate (IDataReader reader, short set)
                {

                    //one shape into a second shape
                    //reader from DB and we want an Address. We're hydrating our madel with this information.

                    address = MapSingleAddress(reader);
                }
                  );

            return address;//address was originally defined as a null instance of an object until it was populated by the reader from the database.
        }

        public List<Address> GetRandomAddresses()
        {
            List<Address> list = null; //just like instance of an address was assigned null before populating it and returning it with info, the list of "all" addresses will be null until populated.
            string procName = "[dbo].[Sabio_Addresses_SelectRandom50]";
            _data.ExecuteCmd(procName,
                inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    Address anAddress = MapSingleAddress(reader);

                    //in order for the list to not be null, we must create a new instance of the list.
                    if (list == null)
                    {
                        list = new List<Address>();
                    }

                    list.Add(anAddress); //takes the initial null list and adds each of the the mapped/populated addresses to it.
                }
                );

            return list;
        }

        private static Address MapSingleAddress(IDataReader reader)
        {
            Address anAddress = new Address(); //every time the mapper runs, it will be a new instance of an Address class/type, assigned to the variable address.

            int startingIndex = 0;

            anAddress.Id = reader.GetSafeInt32(startingIndex++);
            anAddress.LineOne = reader.GetSafeString(startingIndex++);
            anAddress.SuiteNumber = reader.GetSafeInt32(startingIndex++);
            anAddress.City = reader.GetSafeString(startingIndex++);
            anAddress.State = reader.GetSafeString(startingIndex++);
            anAddress.PostalCode = reader.GetSafeString(startingIndex++);
            anAddress.IsActive = reader.GetSafeBool(startingIndex++);
            anAddress.Lat = reader.GetSafeDouble(startingIndex++);
            anAddress.Long = reader.GetSafeDouble(startingIndex++);
            return anAddress;
        }

        private static void AddCommonParams(AddressAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@LineOne", model.LineOne);
            col.AddWithValue("@SuiteNumber", model.SuiteNumber);
            col.AddWithValue("@City", model.City);
            col.AddWithValue("@State", model.State);
            col.AddWithValue("@PostalCode", model.PostalCode);
            col.AddWithValue("@IsActive", model.IsActive);
            col.AddWithValue("@Lat", model.Lat);
            col.AddWithValue("@Long", model.Long);
        }
    }
}


































//IDataProvider _data = null; //IDataProvider is inherited from the SqlDataProvider.

////This is the constructor(below). (Constructors are methods that don't have a return type and it is named after the class it is encapsulated in. It is meant to create an instance of this class.) Takes place on the first instance of the object.
//public AddressService(IDataProvider data)  //AddressesService function receives a parameter in the shape of the IDataProvider class/type.
//{
//    _data = data;

//}

//public void Delete(int target)
//{
//    string procName = "[dbo].[Sabio_Addresses_DeleteById]";
//    _data.ExecuteNonQuery
//        (
//        procName, 
//        inputParamMapper: delegate (SqlParameterCollection col)
//        {

//        col.AddWithValue("@Id", target);


//    }, 
//        returnParameters: null);
//}

//public void Update(AddressUpdateRequest model) //updates are void because they don't return anything. Takes the model of the address.
//{
//    string procName = "[dbo].[Sabio_Addresses_Update]";
//    _data.ExecuteNonQuery(
//        procName,
//        inputParamMapper: delegate (SqlParameterCollection col)
//        {
//            AddCommonParams(model, col);
//            col.AddWithValue("@Id", model.Id);

//        },
//        returnParameters: null);
//}

//public int Add(AddressAddRequest model)
//{

//    int id = 0; //must be null to start and then will be returned with newly created value from output.


//    string procName = "[dbo].[Sabio_Addresses_Insert]";
//    _data.ExecuteNonQuery(
//        procName,
//        inputParamMapper: delegate (SqlParameterCollection col)
//        {
//            AddCommonParams(model, col);

//            SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
//            idOut.Direction = ParameterDirection.Output;

//            col.Add(idOut);
//        },
//        returnParameters: delegate (SqlParameterCollection returnCollection)
//        {
//            object oId = returnCollection["@Id"].Value;

//            int.TryParse(oId.ToString(), out id);


//        });
//    return id;
//}


//public Address Get(int Id)
//{
//    //[dbo].[Sabio_Addresses_SelectById]
//    //@Id int

//    string procName = "[dbo].[Sabio_Addresses_SelectById]";

//    Address address = null; //address will be empty until it is populated with the information it receives from the database (returned as a new class instance of an address)
//    //after assigning the address as null and returning it later, include it in the Program.cs file where the Get is invoked with the actual Id number and assigned to a new variable.

//    //the following .ExecuteCmd() is a function in the SqlDataProvider file that dictates the shape of the information being sent or received.
//    _data.ExecuteCmd(procName,
//        inputParamMapper: delegate (SqlParameterCollection paramCollection)
//        {
//            // the mapper takes one shape and returns a new shape
//            //I have a number (Id )and I need to make a parameter that has a number

//            paramCollection.AddWithValue("@Id", Id);
//            //@Id is the parameter present in SQL database and the Id is the actual number that will be passed to it in the request.

//        },

//        singleRecordMapper: delegate (IDataReader reader, short set)
//        {

//            //one shape into a second shape
//            //reader from DB and we want an Address. We're hydrating our madel with this information.

//            address = MapSingleAddress(reader);
//        }
//          );

//    return address;//address was originally defined as a null instance of an object until it was populated by the reader from the database.
//}

//public List<Address> GetRandomAddresses()
//{
//    List<Address> list = null; //just like instance of an address was assigned null before populating it and returning it with info, the list of "all" addresses will be null until populated.
//    string procName = "[dbo].[Sabio_Addresses_SelectRandom50]";
//    _data.ExecuteCmd(procName,
//        inputParamMapper: null,
//        singleRecordMapper: delegate (IDataReader reader, short set)
//        {
//            Address anAddress = MapSingleAddress(reader);

//            //in order for the list to not be null, we must create a new instance of the list.
//            if (list == null)
//            {
//                list = new List<Address>();
//            }

//            list.Add(anAddress); //takes the initial null list and adds each of the the mapped/populated addresses to it.
//        }
//        );

//    return list;
//}

//private static Address MapSingleAddress(IDataReader reader)
//{
//    Address anAddress = new Address(); //every time the mapper runs, it will be a new instance of an Address class/type, assigned to the variable address.

//    int startingIndex = 0;

//    anAddress.Id = reader.GetSafeInt32(startingIndex++);
//    anAddress.LineOne = reader.GetSafeString(startingIndex++);
//    anAddress.SuiteNumber = reader.GetSafeInt32(startingIndex++);
//    anAddress.City = reader.GetSafeString(startingIndex++);
//    anAddress.State = reader.GetSafeString(startingIndex++);
//    anAddress.PostalCode = reader.GetSafeString(startingIndex++);
//    anAddress.IsActive = reader.GetSafeBool(startingIndex++);
//    anAddress.Lat = reader.GetSafeDouble(startingIndex++);
//    anAddress.Long = reader.GetSafeDouble(startingIndex++);
//    return anAddress;
//}

//private static void AddCommonParams(AddressAddRequest model, SqlParameterCollection col)
//{
//    col.AddWithValue("@LineOne", model.LineOne);
//    col.AddWithValue("@SuiteNumber", model.SuiteNumber);
//    col.AddWithValue("@City", model.City);
//    col.AddWithValue("@State", model.State);
//    col.AddWithValue("@PostalCode", model.PostalCode);
//    col.AddWithValue("@IsActive", model.IsActive);
//    col.AddWithValue("@Lat", model.Lat);
//    col.AddWithValue("@Long", model.Long);
//}