using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using Sabio.Models.Domain.Friends;
using System;
using System.Collections.Generic;
using Sabio.Models;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/v3/friends")]
    [ApiController]
    public class FriendApiControllerV3 : BaseApiController
    {
        private IFriendService _service = null; 
        private IAuthenticationService<int> _authService = null;

        public FriendApiControllerV3
            (IFriendService service //DependencyInjection singleton
            , ILogger<FriendApiControllerV3> logger
            , IAuthenticationService<int> authService) 
            : base(logger)
        { 
            _service = service; 
            _authService = authService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<FriendV3>> GetV3(int id)//GetByIdV3
        {
            int code = 200;
            BaseResponse response = null;

            try 
            { 
                FriendV3 friend = _service.GetV3(id);

                if(friend == null) 
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<FriendV3>() { Item = friend };
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
        public ActionResult<ItemsResponse<FriendV3>> GetAll()//GetAllV3
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<FriendV3> friendList = _service.GetAllV3();

                if(friendList == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");
                }
                else 
                {
                    response = new ItemsResponse<FriendV3> { Items = friendList};
                }
            }
            catch(Exception ex) 
            { 
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpGet("paginate")]
        public ActionResult<Paged<FriendV3>> GetPaged(int pageIndex, int pageSize) 
        { 
            ActionResult result = null;

            try
            {
                Paged<FriendV3> paged = _service.GetPageV3(pageIndex, pageSize);
                if(paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else 
                { 
                    ItemResponse<Paged<FriendV3>> response = new ItemResponse<Paged<FriendV3>>();
                    response.Item = paged;
                    result = Ok200(response);
                }
            }
            catch(Exception ex) 
            { 
                
                Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.ToString()));
            }
            return result;
        }

        [HttpGet("search")]
        public ActionResult<Paged<FriendV3>> SearchPaged(int pageIndex, int pageSize, string query) 
        {
            ActionResult result= null;

            try
            {
                Paged<FriendV3> paged = _service.SearchPagedV3(pageIndex, pageSize, query);
                if (paged == null) 
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else 
                { 
                    ItemResponse<Paged<FriendV3>> response = new ItemResponse<Paged<FriendV3>>();
                    response.Item = paged;
                    result = Ok200(response);
                }
            }
            catch(Exception ex) 
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }

            return result;

        }





    }
}
