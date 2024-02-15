using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Addresses;
using Sabio.Models.Requests.Addresses;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Requests.Friends;
using Sabio.Models;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/friends")]
    [ApiController]
    public class FriendApiController : BaseApiController
    {
        private IFriendService _service = null; //This is the interface that must be passed into the services.AddSingleton<IUserServiceV1, UserServiceV1>(); in "Dependency Injection".
        private IAuthenticationService<int> _authService = null;

        public FriendApiController(IFriendService service, ILogger<UserV1ApiController> logger, IAuthenticationService<int> authService) : base(logger)
        { //when this controller is instantiated will require it be provided with the Interface service.
            _service = service; //IFriendService is injected into the controller
            _authService = authService;
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Friend>>> GetPage(int pageIndex, int pageSize)//pageIndex is the number of the page and size is how many displayed on a page
                                                                                             //    //Sql parameters with @Query
        {
            int code = 200;
            BaseResponse response = null;//do not declare an instance.

            try
            {
                Paged<Friend> page = _service.GetPage(pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Friend>> { Item = page };//it's ONE Item and not many because it's ONE page with multiple friends on it.
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

        [HttpGet]
        public ActionResult<ItemsResponse<Friend>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<Friend> list = _service.GetAll();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<Friend> { Items = list };

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

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Friend>> Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Friend friend = _service.Get(id);

                if (friend == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Friend> { Item = friend };
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
        public ActionResult<ItemResponse<int>> Create(FriendAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
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

                result = StatusCode(500, response);
            }
            return result;
        }


        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(FriendUpdateRequest model, int userId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Update(model, userId);

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
