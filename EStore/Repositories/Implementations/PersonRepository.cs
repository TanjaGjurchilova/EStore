using EStore.Models.Shared;
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
    public class PersonRepository : IPersonRepository
    {
        private readonly Db _context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static object Genger { get; private set; }

        public PersonRepository(IConfiguration configuration)
        {
            _context = new Db();
        }
        public void DeletePerson(Person Person)
        {
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    "DELETE FROM public.\"Person\" b  WHERE b.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", Person.Id, NpgsqlDbType.Integer);
                _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public Person FindPersonById(long id)
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
                    "SELECT * FROM public.\"Person\" b  WHERE b.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", id, NpgsqlDbType.Integer);
                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
            return CreatePersonObject(dt.Rows[0]);
        }

        public IEnumerable<Person> GetAllPeople()
        {
            DataTable dt;

            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM public.\"Person\" b  ORDER bY b.\"Id\" DESC";

                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }

            List<Person> list = (from DataRow dr in dt.Rows select CreatePersonObject(dr)).ToList();

            return list;
        }

        public void SavePerson(Person Person)
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
                cmd.CommandText = "SELECT * FROM public.\"Person\" b WHERE LOWER(b.\"FirstName\")=LOWER(:fn) AND LOWER(b.\"MiddleName\")=LOWER(:mn) AND LOWER(b.\"LastName\")=LOWER(:ln) ;";
                _context.CreateParameterFunc(cmd, "@fn", Person.FirstName, NpgsqlDbType.Text);
                _context.CreateParameterFunc(cmd, "@mn", Person.MiddleName, NpgsqlDbType.Text);
                _context.CreateParameterFunc(cmd, "@ln", Person.LastName, NpgsqlDbType.Text);

                dt = _context.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count == 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    cmd.CommandText = "INSERT INTO public.\"Person\"(\"CreateDate\", \"ModifiedDate\", \"FirstName\", \"MiddleName\", \"LastName\", \"EmailAddress\", \"PhoneNumber\", \"DateOfBirth\", \"IsDeleted\", \"Gender\")VALUES (:cd, :md, :fn, :mn, :ln, :ea, :pn, :dob, :isd, :g);";

                    _context.CreateParameterFunc(cmd, "@fn", Person.FirstName, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@mn", Person.MiddleName, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@ln", Person.LastName, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@isd", Person.IsDeleted, NpgsqlDbType.Boolean);

                    _context.CreateParameterFunc(cmd, "@cd", Person.CreateDate.ToString(), NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@pn", Person.PhoneNumber, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@g", Person.Gender, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@dob", Person.DateOfBirth, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@md", Person.ModifiedDate.ToString(), NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@ea", Person.EmailAddress, NpgsqlDbType.Text);
                 


                    var rowsAffected = _context.ExecuteNonQuery(cmd);

                }
                else
                {
                    throw new Exception("Person exist");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public void UpdatePerson(Person Person)
        {
            DataTable dt;
            try
            {
                var cmd = _context.CreateCommand();
               
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    cmd.CommandText = "UPDATE public.\"Person\" b SET \"CreateDate\"=:cd, \"ModifiedDate\"=:md, \"FirstName\"=:fn, \"MiddleName\"=:mn, \"LastName\"=:ln, \"EmailAddress\"=:ea, \"PhoneNumber\"=:pn, \"DateOfBirth\"=:dob, \"IsDeleted\"=:isd, \"Gender\"=:g WHERE b.\"Id\" =:id ;";

                    _context.CreateParameterFunc(cmd, "@id", Person.Id, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@fn", Person.FirstName, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@mn", Person.MiddleName, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@ln", Person.LastName, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@isd", Person.IsDeleted, NpgsqlDbType.Boolean);
                    _context.CreateParameterFunc(cmd, "@cd", Person.CreateDate.ToString(), NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@pn", Person.PhoneNumber, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@g", Person.Gender, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@dob", Person.DateOfBirth, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@md", Person.ModifiedDate.ToString(), NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@ea", Person.EmailAddress, NpgsqlDbType.Text);


                    var rowsAffected = _context.ExecuteNonQuery(cmd);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }
        private static Person CreatePersonObject(DataRow dr)
        {
            var Person = new Person
            {
                Id = int.Parse(dr["Id"].ToString()),
                PhoneNumber = dr["PhoneNumber"].ToString(),
                CreateDate = DateTimeOffset.Parse(dr["CreateDate"].ToString()),
                ModifiedDate = DateTimeOffset.Parse(dr["ModifiedDate"].ToString()),
                FirstName = dr["FirstName"].ToString(),
                MiddleName = dr["MiddleName"].ToString(),
                LastName = dr["LastName"].ToString(),
                EmailAddress = dr["EmailAddress"].ToString(),
                IsDeleted = bool.Parse(dr["IsDeleted"].ToString()),
                DateOfBirth = dr["DateOfBirth"].ToString(),
            };
            if (dr["Genger"].ToString() == "0") Person.Gender = Gender.Unknown;
            else if (dr["Genger"].ToString() == "1") Person.Gender = Gender.Female;
            else if (dr["Genger"].ToString() == "2") Person.Gender = Gender.Male;


            return Person;
        }
    }
}
