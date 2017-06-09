using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sabio.Web.Controllers.Api
{
    [RoutePrefix("api/cart")]
    public class CartApiController : ApiController
    {
        private ICartService _cartService = null;
        private IUserService _userService = null;

        public CartApiController(ICartService cartService, IUserService userService)
        {
            _userService = userService;
            _cartService = cartService;
        }

        [Route, HttpPost]
        public HttpResponseMessage Add(CartAddRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            SuccessResponse response = new SuccessResponse();
            string currentUserId = _userService.GetCurrentUserId();  
            _cartService.Insert(model, currentUserId);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        
        [Route("{productId:int}"), HttpPut]
        public HttpResponseMessage Update(CartAddRequest model, int productId)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            SuccessResponse response = new SuccessResponse();
            string currentUserId = _userService.GetCurrentUserId();
            _cartService.Update(model, currentUserId, productId);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("quantity"), HttpGet]
        public HttpResponseMessage GetQuantity()
        {
            ItemsResponse<CartItem> response = new ItemsResponse<CartItem>();
            string currentUserId = _userService.GetCurrentUserId();
            response.Items = _cartService.GetQuantity(currentUserId);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("addons/{productId:int}"), HttpGet]
        public HttpResponseMessage GetHealthProductsQuantity(int productId)
        {
            ItemsResponse<CartWithProducts> response = new ItemsResponse<CartWithProducts>();
            string currentUserId = _userService.GetCurrentUserId();
            response.Items = _cartService.GetHealthProductsQuantity(productId, currentUserId);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("{productId:int}/{cost:decimal}/"),HttpDelete]
        public HttpResponseMessage Delete(int productId,decimal cost)
        {
            SuccessResponse response = new SuccessResponse();
            string currentUserId = _userService.GetCurrentUserId();
            _cartService.Delete(productId, currentUserId,cost);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
