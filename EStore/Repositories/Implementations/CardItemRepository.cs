using EStore.Infrastructure;
using EStore.Models.Cart;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Repositories.Implementations
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly Db _context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CartItemRepository(IConfiguration configuration)
        {
            _context = new Db();
        }
        public void DeleteCartItem(CartItem cartItem)
        {
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    "DELETE FROM public.\"CartItems\" ci  WHERE ci.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", cartItem.Id, NpgsqlDbType.Integer);
                _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public CartItem FindCartItemById(long id)
        {
            DataTable dt;

            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    "SELECT * FROM public.\"CartItems\" ci  WHERE ci.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", id, NpgsqlDbType.Integer);
                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
            return CreateCartItemObject(dt.Rows[0]);
        }

        public IEnumerable<CartItem> FindCartItemsByCartId(long cartId)
        {
            DataTable dt;

            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    "SELECT * FROM public.\"CartItems\" ci INNER JOIN  public.\"Cart\" c ON c.\"Id\" = ci.\"Id\" WHERE c.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", cartId, NpgsqlDbType.Integer);
                dt = _context.ExecuteSelectCommand(cmd);
               
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
            List<CartItem> list = (from DataRow dr in dt.Rows select CreateCartItemObject(dr)).ToList();
            return list;
        }

        public void SaveCartItem(CartItem cartItem)
        {
                  
            try
            {
                var cmd = _context.CreateCommand();
              
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    cmd.CommandText = "INSERT INTO public.\"CartItems\"(\"CartId\", \"ProductId\", \"Quantity\", \"IsDeleted\", \"CreateDate\", \" ModifiedDate\")VALUES ( :cid, :pid, :q, :isd, :cd, :d);";

                    _context.CreateParameterFunc(cmd, "@cid", cartItem.CartId, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@pid", cartItem.ProductId, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@q", cartItem.Quantity, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@isd", cartItem.IsDeleted, NpgsqlDbType.Boolean);
                    _context.CreateParameterFunc(cmd, "@cd", cartItem.CreateDate.ToString(), NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@d", cartItem.ModifiedDate.ToString(), NpgsqlDbType.Text);
                 
                    var rowsAffected = _context.ExecuteNonQuery(cmd);
             
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public void UpdateCartItem(CartItem cartItem)
        {

            try
            {
                var cmd = _context.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText = "UPDATE public.\"CartItems\" ci SET ci.\"CartId\"=:cid, ci.\"ProductId\":=pid, ci.\"Quantity\":=q, ci.\"IsDeleted\":=isd, ci.\"CreateDate\":=cd, ci.\" ModifiedDate\":=d WHERE ci.\"Id\" =:id ;;";

                _context.CreateParameterFunc(cmd, "@id", cartItem.Id, NpgsqlDbType.Integer);
                _context.CreateParameterFunc(cmd, "@cid", cartItem.CartId, NpgsqlDbType.Integer);
                _context.CreateParameterFunc(cmd, "@pid", cartItem.ProductId, NpgsqlDbType.Integer);
                _context.CreateParameterFunc(cmd, "@q", cartItem.Quantity, NpgsqlDbType.Integer);
                _context.CreateParameterFunc(cmd, "@isd", cartItem.IsDeleted, NpgsqlDbType.Boolean);
                _context.CreateParameterFunc(cmd, "@cd", cartItem.CreateDate.ToString(), NpgsqlDbType.Text);
                _context.CreateParameterFunc(cmd, "@d", cartItem.ModifiedDate.ToString(), NpgsqlDbType.Text);

                var rowsAffected = _context.ExecuteNonQuery(cmd);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }
        private static CartItem CreateCartItemObject(DataRow dr)
        {
            var cartItem = new CartItem
            {
                Id=int.Parse(dr["Id"].ToString()),
                CartId= int.Parse(dr["CartId"].ToString()),
                ProductId= int.Parse(dr["ProductId"].ToString()),
                Quantity= int.Parse(dr["Quantity"].ToString()),
                CreateDate = DateTimeOffset.Parse(dr["CreateDate"].ToString()),
                ModifiedDate = DateTimeOffset.Parse(dr["ModifiedDate"].ToString()),
                IsDeleted = bool.Parse(dr["IsDeleted"].ToString()),
                
            };
            return cartItem;
        }
    }
}
