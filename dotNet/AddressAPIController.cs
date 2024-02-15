using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Addresses;
using Sabio.Models.Requests.Addresses;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressAPIController : BaseApiController
    {
        private IAddressService _service = null;
        private IAuthenticationService<int> _authService = null;
        public AddressAPIController(IAddressService service
            , ILogger<AddressAPIController> logger
            , IAuthenticationService<int> authService) : base(logger)
        //here by default and does not need to be defined.
        { //needs to receive the IAddressService because the API controller is the middleman between http request and accessing the services provided by the Interface.
            _service = service;
            _authService = authService;
        }

        // GET call to api/addresses
        [HttpGet] //can also just be [HttpGet("")]
        public ActionResult<ItemsResponse<Address>> GetRandomAddress() //The ItemsResponse is the type of of response we want to receive from our endpoint.
        {
            int code = 200;
            BaseResponse response = null; //do not declare a new instance.

            try
            {
                List<Address> list = _service.GetRandomAddresses();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<Address> { Items = list};
                    
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);

        }

        //api/addresses/{id:int} => says the id that follows addresses will be an id of an int type
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Address>> Get(int id)
        {
            int iCode = 200; //integer StatusCode response like 200, 400, 500
            BaseResponse response = null;

            try 
            { 
            Address address = _service.Get(id);

            if (address == null) //if the address can't be found, a response saying so will be returned. (Not returned here, but the response will be equal to what is defined here and passed to the return StatusCode with the iCode.
            {
                iCode = 404;
                response = new ErrorResponse("Application Resource not found.");
            }
            else //when the address requested is found then it will be the response.
            {
                response = new ItemResponse<Address>{ Item = address};
            }
            }
            catch (SqlException sqlEx)//more specific error
            {
                iCode = 500;
                response = new ErrorResponse($"SqlException Errors: {sqlEx.Message}");//new ErrorResponse(sqlEx.Message);
                
            }
            catch (ArgumentException argEx) 
            {
                iCode = 500;
                response = new ErrorResponse($"ArgumentException Errors: {argEx.Message}");
                
            }
            catch (Exception ex) //general error
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Errors: {ex.Message}");
                
            }

            return StatusCode(iCode, response); //cannot be reached if all of the catches return a StatusCode.
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                _service.Delete(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }


        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(AddressAddRequest model)
        {
            //The default response code in this instance is 201. 
            //BUT we do not need it because of what we do below in the try block
            //int iCode = 201;

            //we need this instead of the BaseResponse

            ObjectResult result = null;
           
           // IUserAuthData user = _authService.GetCurrentUser();

            try
            {
                int userId = _authService.GetCurrentUserId();
                //if this operation errors, it would generate an exception and jump to the catch
                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int> { Item = id };

                //This sets the status code for us but also set Url that points back to 
                // the Get By Id endpoint. Setting a Url in the Response (for a 201 Response code) is a common practice
                return Created201(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result =  StatusCode(500, response);
            }
            return result;
        }


        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(AddressUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Update(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
    }
}



