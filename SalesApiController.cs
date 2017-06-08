using Sabio.Web.Domain;
using Sabio.Web.Domain.Products;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sabio.Web.Controllers.Api
{
    [RoutePrefix("api/sales")]
    public class SalesApiController : ApiController
    {
        private ISalesAnalyticsService _salesAnalyticsService = null;

        public SalesApiController(ISalesAnalyticsService salesAnalyticsService)
        {
            _salesAnalyticsService = salesAnalyticsService;
        }

        [Route("revenue"),HttpGet]
        public HttpResponseMessage Get([FromUri]SalesRequest model)
        {
            if(!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemsResponse<BaseProductSales> response = new ItemsResponse<BaseProductSales>();

            response.Items = _salesAnalyticsService.Get(model);

            return Request.CreateResponse(response);

        }
        [Route("addons/revenue/{productId:int}/{startDate:DateTime}/{enddate:DateTime}"), HttpGet]
        public HttpResponseMessage GetAddonByLGProductId(int productId,DateTime startDate,DateTime endDate)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemsResponse<BaseProductSales> response = new ItemsResponse<BaseProductSales>();

            response.Items = _salesAnalyticsService.GetAddonByLGProductId(productId, startDate, endDate);

            return Request.CreateResponse(response);

        }

        [Route("months/{productId:int}/{year:int}"),HttpGet]
        public HttpResponseMessage GetByMonths(int productId,int year)
        {
            ItemsResponse<ProductSales> response = new ItemsResponse<ProductSales>();
            response.Items = _salesAnalyticsService.GetByMonths(productId,year);

            return Request.CreateResponse(response);
        }
        [Route("months/allproducts/{year:int}"), HttpGet]
        public HttpResponseMessage GetAllProductsByMonths(int year)
        {
            ItemsResponse<AllProductsMonthlySales> response = new ItemsResponse<AllProductsMonthlySales>();
            response.Items = _salesAnalyticsService.GetAllProductsByMonths(year);

            return Request.CreateResponse(response);
        }
        [Route("map"),HttpGet]
        public HttpResponseMessage GetMapSales([FromUri]SalesRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemsResponse<SalesMap> response = new ItemsResponse<SalesMap>();

            response.Items = _salesAnalyticsService.GetSalesMap(model);

            return Request.CreateResponse(response);
        }

        [Route("map/{productId:int}"), HttpGet]
        public HttpResponseMessage GetMapSalesByProduct([FromUri]SalesRequest model,int productId)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemsResponse<SalesMap> response = new ItemsResponse<SalesMap>();

            response.Items = _salesAnalyticsService.GetSalesMapByProduct(model,productId);

            return Request.CreateResponse(response);
        }
    }
}
