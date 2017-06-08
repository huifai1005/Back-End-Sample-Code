using Sabio.Data;
using Sabio.Web.Domain;
using Sabio.Web.Models.Requests.Cart;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sabio.Web.Services.Cart
{
    public class CartService : BaseService, ICartService
    {

        public void Insert(CartAddRequest model,string userId)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Cart_Insert"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserId", userId);
                    paramCollection.AddWithValue("@ProductId", model.ProductId);     

                }
                , returnParameters: null);
        }

        public void Update(CartAddRequest model, string userId, int productId)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Cart_Update"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                  {
                      paramCollection.AddWithValue("@UserId", userId);
                      paramCollection.AddWithValue("@ProductId", productId);
                  }
                , returnParameters: null);
        }

        public List<CartItem> GetQuantity(string userId)
        {
            List<CartItem> cartProductQuantity = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.Cart_Get_Quantity"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection) 
                {
                    paramCollection.AddWithValue("@UserId", userId);
                    
                }
                , map: delegate (IDataReader reader,short set) 
                {

                    CartItem p = MapCartQuantity(reader);
                    if(cartProductQuantity == null)
                    {
                        cartProductQuantity = new List<CartItem>();
                    }
                    cartProductQuantity.Add(p);
                });
            return cartProductQuantity;
        }

        private CartItem MapCartQuantity(IDataReader reader)
        {
            CartItem p = new CartItem();
            int startingIndex = 0;
            p.ProductId = reader.GetSafeInt32(startingIndex++);
            p.Quantity = reader.GetSafeInt32(startingIndex++);
            p.Cost = reader.GetSafeDecimal(startingIndex++);

            return p;
        }

        public List<CartWithProducts> GetHealthProductsQuantity(int productId, string userId)//get the health products addons by associated lash girl product Id
        {
            List<CartWithProducts> cartProductQuantity = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.Cart_GetAddOnsByProductId"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserId", userId);
                    paramCollection.AddWithValue("@ProductId", productId);
                }
                , map: delegate (IDataReader reader, short set)
                {

                    CartWithProducts p = MapCartWithProducts(reader);
                    if (cartProductQuantity == null)
                    {
                        cartProductQuantity = new List<CartWithProducts>();
                    }
                    cartProductQuantity.Add(p);
                });
            return cartProductQuantity;
        }

        private CartWithProducts MapCartWithProducts(IDataReader reader)
        {
            CartWithProducts p = new CartWithProducts();
            int startingIndex = 0;
            p.Id = reader.GetSafeInt32(startingIndex++);
            p.Quantity = reader.GetSafeInt32(startingIndex++);
            p.Title = reader.GetSafeString(startingIndex++);
            p.Description = reader.GetSafeString(startingIndex++);
            p.BasePrice = reader.GetSafeDecimal(startingIndex++);
            p.Cost = reader.GetSafeDecimal(startingIndex++);
            p.ProductType = reader.GetSafeInt32(startingIndex++);
            p.MainImage = reader.GetSafeString(startingIndex++);
            p.SecondaryImage = reader.GetSafeString(startingIndex++);
            return p;
        }

        public void Delete(int productId, string userId,decimal cost)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Cart_DeleteLastOne"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserId", userId);
                    paramCollection.AddWithValue("@ProductId", productId);
                    paramCollection.AddWithValue("@Cost", cost);
                }
                , returnParameters: null);
        }
          
    }


    
}