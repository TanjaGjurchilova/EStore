using EStore.Models.Customer;
using EStore.Infrastructure;
using EStore.Models.Product;
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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly Db _context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CustomerRepository(IConfiguration configuration)
        {
            _context = new Db();
        }
        public void DeleteCustomer(Customer Customer)
        {
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    "DELETE FROM public.\"Customer\" b  WHERE b.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", Customer.Id, NpgsqlDbType.Integer);
                _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public Customer FindCustomerById(long id)
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
                    "SELECT * FROM public.\"Customer\" b  INNER JOIN  public.\"People\" c ON c.\"PersonId\" = b.\"Id\"  WHERE b.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", id, NpgsqlDbType.Integer);
                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
            return CreateCustomerObject(dt.Rows[0]);
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            DataTable dt;

            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM public.\"Customer\" b INNER JOIN  public.\"People\" c ON c.\"PersonId\" = b.\"Id\"  ORDER bY b.\"Id\" DESC";

                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }

            List<Customer> list = (from DataRow dr in dt.Rows select CreateCustomerObject(dr)).ToList();

            return list;
        }

        public void SaveCustomer(Customer Customer)
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
                cmd.CommandText = "SELECT * FROM public.\"Customer\" b  WHERE b.\"PersonId\"=:pid";
                _context.CreateParameterFunc(cmd, "@pid", Customer.PersonId, NpgsqlDbType.Text);

                dt = _context.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count == 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    cmd.CommandText = "INSERT INTO public.\"Customer\"(\"CreateDate\", \"ModifiedDate\", \"IsDeleted\", \"PersonId\")VALUES ( :cd, :d, :isd, :pid);";
                
                    _context.CreateParameterFunc(cmd, "@pid", Customer.PersonId, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@isd", Customer.IsDeleted, NpgsqlDbType.Boolean);
                    _context.CreateParameterFunc(cmd, "@cd", Customer.CreateDate.ToString(), NpgsqlDbType.Text);                
                    _context.CreateParameterFunc(cmd, "@d", Customer.ModifiedDate.ToString(), NpgsqlDbType.Text);
                  


                    var rowsAffected = _context.ExecuteNonQuery(cmd);

                }
                else
                {
                    throw new Exception("Customer exist");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public void UpdateCustomer(Customer Customer)
        {
            DataTable dt;
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM public.\"Customer\" b  WHERE b.\"PersonId\"=:pid";
                _context.CreateParameterFunc(cmd, "@pid", Customer.PersonId, NpgsqlDbType.Text);

                dt = _context.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count == 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    cmd.CommandText = "UPDATE public.\"Customer\" b SET \"CreateDate\"=:cd, \"ModifiedDate\"=:d, \"IsDeleted\"=:isd, \"PersonId\"=:pid WHERE b.\"Id\" =:id ;";

                    _context.CreateParameterFunc(cmd, "@id", Customer.Id, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@p", Customer.PersonId, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@isd", Customer.IsDeleted, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@cd", Customer.CreateDate, NpgsqlDbType.Text);                  
                    _context.CreateParameterFunc(cmd, "@d", Customer.ModifiedDate, NpgsqlDbType.Text);
                 


                    var rowsAffected = _context.ExecuteNonQuery(cmd);

                }
                throw new Exception("Customer exist");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }
        private static Customer CreateCustomerObject(DataRow dr)
        {
            var Customer = new Customer
            {
                Id = int.Parse(dr["Id"].ToString()),
                PersonId = int.Parse(dr["Id"].ToString()),             
                CreateDate = DateTimeOffset.Parse(dr["CreateDate"].ToString()),
                ModifiedDate = DateTimeOffset.Parse(dr["ModifiedDate"].ToString()),            
                IsDeleted = bool.Parse(dr["IsDeleted"].ToString()),
               
            };
         

            return Customer;
        }
    }
}
