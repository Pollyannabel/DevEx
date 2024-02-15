using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Addresses;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System.Data.SqlClient;
using System;
using Sabio.Models.Domain.Users;
using System.Collections.Generic;
using Sabio.Models.Requests.Users;
using Sabio.Models.Domain.Friends;
using Sabio.Models;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserV1ApiController : BaseApiController
    {
        private IUserServiceV1 _service = null; //This is the interface that must be passed into the services.AddSingleton<IUserServiceV1, UserServiceV1>(); in "Dependency Injection".
        private IAuthenticationService<int> _authService = null;

        public UserV1ApiController(IUserServiceV1 service, ILogger<UserV1ApiController> logger, IAuthenticationService<int> authService) : base(logger)
        { //when this controller is instantiated will require it be provided with the Interface service.
            _service = service;
            _authService = authService;
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<User>>> GetPage(int pageIndex, int pageSize)//pageIndex is the number of the page and size is how many displayed on a page
                                                                                             //    //Sql parameters with @Query
        {
            int code = 200;
            BaseResponse response = null;//do not declare an instance.

            try
            {
                Paged<User> page = _service.GetPage(pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<User>> { Item = page };//it's ONE Item and not many because it's ONE page with multiple friends on it.
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
        public ActionResult<ItemResponse<User>> Get(int id)
        {
            int code = 200; //integer StatusCode response like 200, 400, 500
            BaseResponse response = null;

            try
            {
                User user = _service.Get(id);

                if (user == null) //if the user can't be found, a response saying so will be returned. (Not returned here, but the response will be changed to equal what is defined here and passed to the return StatusCode with the iCode.
                {
                    code = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else //when the user requested is found then it will be the response.
                {
                    response = new ItemResponse<User> { Item = user };
                }
            }
            
            catch (Exception ex) //general error
            {
                code = 404;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Errors: {ex.Message}");
            }

            return StatusCode(code, response); //cannot be reached if all of the catches return a StatusCode.
        }

        [HttpGet]
        public ActionResult<ItemsResponse<User>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;//do not declare an instance.

            try
            {
                List<User> list = _service.GetAll();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<User> { Items = list };
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

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(UserUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;//

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

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(UserAddRequest model)
        {

            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
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














    }
}
