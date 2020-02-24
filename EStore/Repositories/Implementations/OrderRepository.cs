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
    public class OrderRepository : IOrderRepository
    {
        private readonly Db _context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public OrderRepository(IConfiguration configuration)
        {
            _context = new Db();
        }

        public IEnumerable<Order> FindOrderByOrderId(long orderId)
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
                    "SELECT * FROM public.\"Order\" b  WHERE b.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", orderId, NpgsqlDbType.Integer);
                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
            List<Order> list = (from DataRow dr in dt.Rows select CreateOrderObject(dr)).ToList();
            return list;
        }

        private static Order CreateOrderObject(DataRow dr)
        {
            var Order = new Order
            {
                Id = int.Parse(dr["Id"].ToString()),
                CreateDate = DateTimeOffset.Parse(dr["CreateDate"].ToString()),
                ModifiedDate = DateTimeOffset.Parse(dr["ModifiedDate"].ToString()),
                IsDeleted = bool.Parse(dr["IsDeleted"].ToString()),
                AddressId = int.Parse(dr["Id"].ToString()),
                CustomerId = int.Parse(dr["Id"].ToString()),
                OrderItemTotal=decimal.Parse(dr["Id"].ToString()),
                OrderTotal = decimal.Parse(dr["Id"].ToString()),
                ShippingCharge = decimal.Parse(dr["Id"].ToString()),
                
            };
            if (dr["OrderStatus"].ToString() == "0") Order.OrderStatus = OrderStatus.Canceled;
            else if (dr["OrderStatus"].ToString() == "1") Order.OrderStatus = OrderStatus.Closed;
            else if (dr["OrderStatus"].ToString() == "2") Order.OrderStatus = OrderStatus.Completed;
            else if (dr["OrderStatus"].ToString() == "3") Order.OrderStatus = OrderStatus.SuspectedFraud;
            else if (dr["OrderStatus"].ToString() == "4") Order.OrderStatus = OrderStatus.OnHold;
            else if (dr["OrderStatus"].ToString() == "5") Order.OrderStatus = OrderStatus.PaymentReview;
            else if (dr["OrderStatus"].ToString() == "6") Order.OrderStatus = OrderStatus.Pending;
            else if (dr["OrderStatus"].ToString() == "7") Order.OrderStatus = OrderStatus.PendingPayment;
            else if (dr["OrderStatus"].ToString() == "8") Order.OrderStatus = OrderStatus.Processing;
            else if (dr["OrderStatus"].ToString() == "9") Order.OrderStatus = OrderStatus.Submitted;


            return Order;
        }

        public Order FindOrderById(long id)
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
                    "SELECT * FROM public.\"Order\" b   WHERE b.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", id, NpgsqlDbType.Integer);
                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
            return CreateOrderObject(dt.Rows[0]);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            DataTable dt;

            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM public.\"Order\" b ORDER bY b.\"Id\" DESC";

                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }

            List<Order> list = (from DataRow dr in dt.Rows select CreateOrderObject(dr)).ToList();

            return list;
        }

        public void SaveOrder(Order Order)
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

                    cmd.CommandText = "INSERT INTO public.\"Order\"(\"CustomerId\", \"AddressId\", \"OrderTotal\", \"OrderItemTotal\", \"ShippingCharge\", \"OrderStatus\", \"CreateDate\", \"ModifiedDate\", \"IsDeleted\")VALUES ( :cid, :a, :ot, :oit, :sc, :os, :cd, :md, :isd);";

                _context.CreateParameterFunc(cmd, "@os", Order.OrderStatus, NpgsqlDbType.Integer);
                _context.CreateParameterFunc(cmd, "@cid", Order.CustomerId, NpgsqlDbType.Integer);
                _context.CreateParameterFunc(cmd, "@a", Order.AddressId, NpgsqlDbType.Integer);
                _context.CreateParameterFunc(cmd, "@isd", Order.IsDeleted, NpgsqlDbType.Boolean);
             
                _context.CreateParameterFunc(cmd, "@ot", Order.OrderTotal, NpgsqlDbType.Numeric);
                _context.CreateParameterFunc(cmd, "@oit", Order.OrderItemTotal, NpgsqlDbType.Numeric);
                _context.CreateParameterFunc(cmd, "@sc", Order.ShippingCharge, NpgsqlDbType.Numeric);
            
          
                _context.CreateParameterFunc(cmd, "@cd", Order.CreateDate.ToString(), NpgsqlDbType.Text);
                _context.CreateParameterFunc(cmd, "@d", Order.ModifiedDate.ToString(), NpgsqlDbType.Text);



                    var rowsAffected = _context.ExecuteNonQuery(cmd);

              
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public void UpdateOrder(Order Order)
        {

            DataTable dt;
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM public.\"Order\" b  WHERE b.\"PersonId\"=:pid";
                _context.CreateParameterFunc(cmd, "@pid", Order.PersonId, NpgsqlDbType.Text);

                dt = _context.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count == 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    cmd.CommandText = "UPDATE public.\"Order\" b SET \"CustomerId\"=:cid , \"AddressId\"=:a , \"OrderTotal\"=:ot , \"OrderItemTotal\"=:oit , \"ShippingCharge\"=:sc , \"OrderStatus\"=:os , \"CreateDate\"=:cd , \"ModifiedDate\"=:d , \"IsDeleted\"=:isd WHERE b.\"Id\" =:id ;";

                    _context.CreateParameterFunc(cmd, "@os", Order.OrderStatus, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@cid", Order.CustomerId, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@a", Order.AddressId, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@isd", Order.IsDeleted, NpgsqlDbType.Boolean);

                    _context.CreateParameterFunc(cmd, "@ot", Order.OrderTotal, NpgsqlDbType.Numeric);
                    _context.CreateParameterFunc(cmd, "@oit", Order.OrderItemTotal, NpgsqlDbType.Numeric);
                    _context.CreateParameterFunc(cmd, "@sc", Order.ShippingCharge, NpgsqlDbType.Numeric);


                    _context.CreateParameterFunc(cmd, "@cd", Order.CreateDate.ToString(), NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@d", Order.ModifiedDate.ToString(), NpgsqlDbType.Text);

                    var rowsAffected = _context.ExecuteNonQuery(cmd);

                }
                throw new Exception("Order exist");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public void DeleteOrder(Order Order)
        {
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    "DELETE FROM public.\"Order\" b  WHERE b.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", Order.Id, NpgsqlDbType.Integer);
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
