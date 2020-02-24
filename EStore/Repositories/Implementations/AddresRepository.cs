using EStore.Infrastructure;
using EStore.Models.Address;
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
    public class AddressRepository : IAddressRepository
    {
        private readonly Db _context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AddressRepository(IConfiguration configuration)
        {
            _context = new Db();
        }
        public void DeleteAddress(Address Address)
        {
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    "DELETE FROM public.\"Address\" b  WHERE b.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", Address.Id, NpgsqlDbType.Integer);
                _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public Address FindAddressById(long id)
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
                    "SELECT * FROM public.\"Address\" b  WHERE b.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", id, NpgsqlDbType.Integer);
                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
            return CreateAddressObject(dt.Rows[0]);
        }

        public IEnumerable<Address> GetAllAddresss()
        {
            DataTable dt;

            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM public.\"Address\" b  ORDER bY b.\"Id\" DESC";

                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }

            List<Address> list = (from DataRow dr in dt.Rows select CreateAddressObject(dr)).ToList();

            return list;
        }

        public void SaveAddress(Address Address)
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
                cmd.CommandText = "SELECT * FROM public.\"Address\" b WHERE LOWER(b.\"Name\")=LOWER(:n);";
                _context.CreateParameterFunc(cmd, "@n", Address.Name, NpgsqlDbType.Text);

                dt = _context.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count == 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    cmd.CommandText = "INSERT INTO public.\"Address\"(\"Name\", \"AddressLine1\", \"AddressLine2\", \"City\", \"Country\", \"State\", \"ZipCode\", \"CreateDate\", \"ModifiedDate\", \"CustomerId\", \"IsDeleted\")VALUES ( :n, :a1, :a2, :c, :co, :s, :zc, :cd, :md, :cid, :isd);";

                    _context.CreateParameterFunc(cmd, "@isd", Address.IsDeleted, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@cid", Address.CostumerId, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@zc", Address.ZipCode, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@cd", Address.CreateDate, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@md", Address.ModifiedDate, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@a1", Address.AddressLine1, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@a2", Address.AddressLine2, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@c", Address.City, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@co", Address.Country, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@n", Address.Name, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@s", Address.State, NpgsqlDbType.Text);


                    var rowsAffected = _context.ExecuteNonQuery(cmd);

                }
                else
                {
                    throw new Exception("Address exist");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public void UpdateAddress(Address Address)
        {
            DataTable dt;
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM public.\"Address\" b WHERE LOWER(b.\"Name\")=LOWER(:n);";
                _context.CreateParameterFunc(cmd, "@n", Address.Name, NpgsqlDbType.Text);

                dt = _context.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count == 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    cmd.CommandText = "UPDATE public.\"Address\" b SET \"Name\"=:n, \"AddressLine1\"=:a1, \"AddressLine2\"=:a2, \"City\"=:c, \"Country\"=:co, \"State\"=:s, \"ZipCode\"=:zc, \"CreateDate\"=:cd, \"ModifiedDate\"=:md, \"CustomerId\"=:cid, \"IsDeleted\"=:isd WHERE b.\"Id\" =:id ;";

                    _context.CreateParameterFunc(cmd, "@id", Address.Id, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@cid", Address.CostumerId, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@isd", Address.IsDeleted, NpgsqlDbType.Boolean);
                    _context.CreateParameterFunc(cmd, "@s", Address.ZipCode, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@cd", Address.CreateDate, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@cd", Address.ModifiedDate, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@a1", Address.AddressLine1, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@a2", Address.AddressLine2, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@c", Address.City, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@co", Address.Country, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@n", Address.Name, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@s", Address.State, NpgsqlDbType.Text);


                    var rowsAffected = _context.ExecuteNonQuery(cmd);

                }
                throw new Exception("Address exist");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }
        private static Address CreateAddressObject(DataRow dr)
        {
            var Address = new Address
            {
                Id = int.Parse(dr["Id"].ToString()),
                CreateDate = DateTimeOffset.Parse(dr["CreateDate"].ToString()),
                ModifiedDate = DateTimeOffset.Parse(dr["ModifiedDate"].ToString()),
                Name = dr["Name"].ToString(),
                IsDeleted = bool.Parse(dr["IsDeleted"].ToString()),
                AddressLine1 = dr["AddressLine1"].ToString(),
                AddressLine2 = dr["AddressLine2"].ToString(),
                City = dr["City"].ToString(),
                Country = dr["Country"].ToString(),
                State = dr["State"].ToString(),
                ZipCode = dr["ZipCode"].ToString(),
                CostumerId= int.Parse(dr["CostumerId"].ToString()),

            };
         
            return Address;
        }
    }
}
