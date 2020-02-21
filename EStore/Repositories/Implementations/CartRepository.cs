using EStore.Infrastructure;
using EStore.Models.Cart;
using Microsoft.Extensions.Configuration;
using NLog;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Repositories.Implementations
{
    public class CartRepository : ICartRepository
    {
        private readonly Db _context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CartRepository(IConfiguration configuration)
        {
            _context = new Db();
        }
        public void DeleteCart(Cart cart)
        {
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    "DELETE FROM public.\"Cart\" ci  WHERE ci.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", cart.Id, NpgsqlDbType.Integer);
                _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public Cart FindCartById(long id)
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
                    "SELECT * FROM public.\"Cart\" ci  WHERE ci.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", id, NpgsqlDbType.Integer);
                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
            return CreateCartObject(dt.Rows[0]);
        }

        public IEnumerable<Cart> GetAllCarts()
        {
            DataTable dt;

            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM public.\"Cart\" c  ORDER bY c.\"Id\" DESC";

                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }

            List<Cart> list = (from DataRow dr in dt.Rows select CreateCartObject(dr)).ToList();
            return list;
        }

        public void SaveCart(Cart cart)
        {
            try
            {
                var cmd = _context.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText = "INSERT INTO public.\"Cart\"(\"UniqueCartId\", \"IsDeleted\", \"CreateDate\", \"ModifiedDate\", \"CartStatus\")VALUES ( :ucid, :isd, :cd, :d, :cs);";
                int status;
                if (cart.CartStatus == CartStatus.Open)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                _context.CreateParameterFunc(cmd, "@cs", status, NpgsqlDbType.Integer);
                _context.CreateParameterFunc(cmd, "@ucid", cart.UniqueCartId, NpgsqlDbType.Text);
                _context.CreateParameterFunc(cmd, "@isd", cart.IsDeleted, NpgsqlDbType.Boolean);
                _context.CreateParameterFunc(cmd, "@cd", cart.CreateDate.ToString(), NpgsqlDbType.Text);
                _context.CreateParameterFunc(cmd, "@d", cart.ModifiedDate.ToString(), NpgsqlDbType.Text);

                var rowsAffected = _context.ExecuteNonQuery(cmd);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public void UpdateCart(Cart cart)
        {
            try
            {
                var cmd = _context.CreateCommand();

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText = "UPDATE public.\"Cart\" c SET \"UniqueCartId\":=ucid, \"IsDeleted\":=isd, \"CreateDate\":=cd, \"ModifiedDate\":=d, \"CardStatus\":=cs WHERE c.\"Id\":=id;";
                int status;
                if (cart.CartStatus == CartStatus.Open)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                _context.CreateParameterFunc(cmd, "@id", cart.Id, NpgsqlDbType.Integer);
                _context.CreateParameterFunc(cmd, "@cs", status, NpgsqlDbType.Integer);
                _context.CreateParameterFunc(cmd, "@ucid", cart.UniqueCartId, NpgsqlDbType.Text);
                _context.CreateParameterFunc(cmd, "@isd", cart.IsDeleted, NpgsqlDbType.Boolean);
                _context.CreateParameterFunc(cmd, "@cd", cart.CreateDate.ToString(), NpgsqlDbType.Text);
                _context.CreateParameterFunc(cmd, "@d", cart.ModifiedDate.ToString(), NpgsqlDbType.Text);

                var rowsAffected = _context.ExecuteNonQuery(cmd);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }
        private static Cart CreateCartObject(DataRow dr)
        {
            var cart = new Cart
            {
                Id = int.Parse(dr["Id"].ToString()),
                UniqueCartId = dr["UniqueCartId"].ToString(),
                CreateDate = DateTimeOffset.Parse(dr["CreateDate"].ToString()),
                ModifiedDate = DateTimeOffset.Parse(dr["ModifiedDate"].ToString()),
                IsDeleted = bool.Parse(dr["IsDeleted"].ToString()),
              
            };
            if (dr["CartStatus"].ToString() == "0") cart.CartStatus = CartStatus.Open;
            else if (dr["CartStatus"].ToString() == "1") cart.CartStatus = CartStatus.CheckedOut;
            return cart;
        }
    }
}
