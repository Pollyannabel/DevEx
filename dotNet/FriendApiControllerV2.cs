using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Requests.Friends;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/v2/friends")]
    [ApiController]
    public class FriendApiControllerV2 : BaseApiController
    {
        private IFriendService _service = null; //This is the interface that must be passed into the services.AddSingleton<IUserServiceV1, UserServiceV1>(); in "Dependency Injection".
        private IAuthenticationService<int> _authService = null;

        public FriendApiControllerV2(IFriendService service, ILogger<UserV1ApiController> logger, IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<FriendV2>> GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try


            {
                FriendV2 friend = _service.GetV2(id);

                if (friend == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<FriendV2>() { Item = friend };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet]
        public ActionResult<ItemsResponse<FriendV2>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<FriendV2> list = _service.GetAllV2();

                if (list == null)
                {
                    code = 404;

                }
                else
                {
                    response = new ItemsResponse<FriendV2> { Items = list };
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

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<FriendV2>>> GetPaginate(int pageIndex, int pageSize) 
        {
            ActionResult result = null;

            try
            {
                Paged<FriendV2> pagedList = _service.GetPageV2(pageIndex, pageSize);
                if (pagedList == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else 
                {
                    ItemResponse<Paged<FriendV2>> response = new ItemResponse<Paged<FriendV2>>();
                    response.Item = pagedList;
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            { 
                Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }

            return result;

        }

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<FriendV2>>> SearchPaginate(int pageIndex, int pageSize, string query) 
        {
            ActionResult result = null;

            try 
            {
                Paged<FriendV2> paged = _service.SearchPagedV2(pageIndex, pageSize, query);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else 
                { 
                    ItemResponse<Paged<FriendV2>> response = new ItemResponse<Paged<FriendV2>>();
                    response.Item = paged;
                    result = Ok200(response);
                }
            }
            catch (Exception ex) 
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }
            return result;
        }

    }
}
