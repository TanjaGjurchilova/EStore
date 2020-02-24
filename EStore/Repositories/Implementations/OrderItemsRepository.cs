using EStore.Models.Order;
using EStore.Infrastructure;
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
    public class OrderItemsRepository : IOrderItemsRepository
    {
        private readonly Db _context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public OrderItemsRepository(IConfiguration configuration)
        {
            _context = new Db();
        }

        public IEnumerable<OrderItems> FindOrderItemsByOrderId(long orderId)
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
                    "SELECT * FROM public.\"OrderItems\" b  INNER JOIN  public.\"Order\" c ON c.\"Id\" = b.\"OrderId\"  WHERE c.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", orderId, NpgsqlDbType.Integer);
                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
            List<OrderItems> list = (from DataRow dr in dt.Rows select CreateOrderItemsObject(dr)).ToList();
            return list;
        }
    
        private static OrderItems CreateOrderItemsObject(DataRow dr)
        {
            var OrderItems = new OrderItems
            {
                Id = int.Parse(dr["Id"].ToString()),
                ProductId = int.Parse(dr["ProductId"].ToString()),
                OrderId = int.Parse(dr["OrderId"].ToString()),
                Quantity = int.Parse(dr["Quantity"].ToString()),
                CreateDate = DateTimeOffset.Parse(dr["CreateDate"].ToString()),
                ModifiedDate = DateTimeOffset.Parse(dr["ModifiedDate"].ToString()),            
                IsDeleted = bool.Parse(dr["IsDeleted"].ToString()),
               
            };
         

            return OrderItems;
        }

        public OrderItems FindOrderItemById(long id)
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
                    "SELECT * FROM public.\"OrderItems\" b  INNER JOIN  public.\"People\" c ON c.\"PersonId\" = b.\"Id\"  WHERE b.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", id, NpgsqlDbType.Integer);
                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
            return CreateOrderItemsObject(dt.Rows[0]);
        }

        public IEnumerable<OrderItems> FindOrderItemByOrderId(long orderId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrderItems> GetAllOrderItems()
        {
            DataTable dt;

            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM public.\"OrderItems\" b INNER JOIN  public.\"People\" c ON c.\"PersonId\" = b.\"Id\"  ORDER bY b.\"Id\" DESC";

                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }

            List<OrderItems> list = (from DataRow dr in dt.Rows select CreateOrderItemsObject(dr)).ToList();

            return list;
        }

        public void SaveOrderItem(OrderItems orderItem)
        {
            DataTable dt;
            int status;
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM public.\"OrderItems\" b  WHERE b.\"PersonId\"=:pid";
                _context.CreateParameterFunc(cmd, "@pid", OrderItems.PersonId, NpgsqlDbType.Text);

                dt = _context.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count == 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    cmd.CommandText = "INSERT INTO public.\"OrderItems\"(\"CreateDate\", \"ModifiedDate\", \"IsDeleted\", \"PersonId\")VALUES ( :cd, :d, :isd, :pid);";

                    _context.CreateParameterFunc(cmd, "@pid", OrderItems.PersonId, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@isd", OrderItems.IsDeleted, NpgsqlDbType.Boolean);
                    _context.CreateParameterFunc(cmd, "@cd", OrderItems.CreateDate.ToString(), NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@d", OrderItems.ModifiedDate.ToString(), NpgsqlDbType.Text);



                    var rowsAffected = _context.ExecuteNonQuery(cmd);

                }
                else
                {
                    throw new Exception("OrderItems exist");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public void UpdateOrderItem(OrderItems orderItem)
        {

            DataTable dt;
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM public.\"OrderItems\" b  WHERE b.\"PersonId\"=:pid";
                _context.CreateParameterFunc(cmd, "@pid", OrderItems.PersonId, NpgsqlDbType.Text);

                dt = _context.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count == 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    cmd.CommandText = "UPDATE public.\"OrderItems\" b SET \"CreateDate\"=:cd, \"ModifiedDate\"=:d, \"IsDeleted\"=:isd, \"PersonId\"=:pid WHERE b.\"Id\" =:id ;";

                    _context.CreateParameterFunc(cmd, "@id", OrderItems.Id, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@p", OrderItems.PersonId, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@isd", OrderItems.IsDeleted, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@cd", OrderItems.CreateDate, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@d", OrderItems.ModifiedDate, NpgsqlDbType.Text);



                    var rowsAffected = _context.ExecuteNonQuery(cmd);

                }
                throw new Exception("OrderItems exist");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public void DeleteOrderItem(OrderItems orderItem)
        {
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    "DELETE FROM public.\"OrderItems\" b  WHERE b.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", OrderItems.Id, NpgsqlDbType.Integer);
                _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }
    }
    
}
