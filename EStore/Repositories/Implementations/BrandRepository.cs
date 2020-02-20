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
    public class BrandRepository : IBrandRepository
    {
        private readonly Db _context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public BrandRepository(IConfiguration configuration)
        {
            _context = new Db();
        }
        public void DeleteBrand(Brand brand)
        {
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    "DELETE FROM public.\"Brand\" b  WHERE b.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", brand.Id, NpgsqlDbType.Integer);
                _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public Brand FindBrandById(long id)
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
                    "SELECT * FROM public.\"Brand\" b  WHERE b.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", id, NpgsqlDbType.Integer);
                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
            return CreateBrandObject(dt.Rows[0]);
        }

        public IEnumerable<Brand> GetAllBrands()
        {
            DataTable dt;

            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM public.\"Brand\" b  ORDER bY b.\"Id\" DESC";

                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }

            List<Brand> list = (from DataRow dr in dt.Rows select CreateBrandObject(dr)).ToList();

            return list;
        }

        public void SaveBrand(Brand brand)
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
                cmd.CommandText = "SELECT * FROM public.\"Brand\" b WHERE LOWER(b.\"Name\")=LOWER(:n);";
                _context.CreateParameterFunc(cmd, "@n", brand.Name, NpgsqlDbType.Text);

                dt = _context.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count == 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    cmd.CommandText = "INSERT INTO public.\"Brand\"(\"BrandStatus\", \"CreateDate\", \"IsDeleted\", \"Description\", \"MetaDescription\", \"MetaKeywords\", \"ModifiedDate\", \"Name\", \"Slug\")VALUES ( :bs, :cd, :id, :de, :md, :mk, :d, :n, :s);";
                    if (brand.BrandStatus == BrandStatus.Active)
                    {
                        status = 1;
                    }
                    else
                    {
                        status = 0;
                    }
                    _context.CreateParameterFunc(cmd, "@bs", status, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@id", brand.IsDeleted, NpgsqlDbType.Boolean);

                    _context.CreateParameterFunc(cmd, "@cd", brand.CreateDate.ToString(), NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@de", brand.Description, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@md", brand.MetaDescription, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@mk", brand.MetaKeywords, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@d", brand.ModifiedDate.ToString(), NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@n", brand.Name, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@s", brand.Slug, NpgsqlDbType.Text);


                    var rowsAffected = _context.ExecuteNonQuery(cmd);

                }
                else
                {
                    throw new Exception("Brand exist");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public void UpdateBrand(Brand brand)
        {
            DataTable dt;
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM public.\"Brand\" b WHERE LOWER(b.\"Name\")=LOWER(:n);";
                _context.CreateParameterFunc(cmd, "@n", brand.Name, NpgsqlDbType.Text);

                dt = _context.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count == 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    cmd.CommandText = "UPDATE public.\"Brand\" b SET \"BrandStatus\"=:bs, \"CreateDate\"=:cd, \"IsDeleted\"=:id, \"Description\"=:de, \"MetaDescription\"=:md, \"MetaKeywords\"=:mk, \"ModifiedDate\"=:d, \"Name\"=:n, \"Slug\"=:s WHERE b.\"Id\" =:id ;";

                    _context.CreateParameterFunc(cmd, "@bs", brand.BrandStatus, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@id", brand.IsDeleted, NpgsqlDbType.Integer);

                    _context.CreateParameterFunc(cmd, "@cd", brand.CreateDate, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@de", brand.Description, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@md", brand.MetaDescription, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@mk", brand.MetaKeywords, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@d", brand.ModifiedDate, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@n", brand.Name, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@s", brand.Slug, NpgsqlDbType.Text);


                    var rowsAffected = _context.ExecuteNonQuery(cmd);

                }
                throw new Exception("Brand exist");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }
        private static Brand CreateBrandObject(DataRow dr)
        {
            var brand = new Brand
            {
                Id = int.Parse(dr["Id"].ToString()),
                Description = dr["Description"].ToString(),
                MetaDescription = dr["MetaDescription"].ToString(),
                MetaKeywords = dr["MetaKeywords"].ToString(),
                CreateDate = DateTimeOffset.Parse(dr["CreateDate"].ToString()),
                ModifiedDate = DateTimeOffset.Parse(dr["ModifiedDate"].ToString()),
                Name = dr["Name"].ToString(),
                Slug = dr["Slug"].ToString(),
                IsDeleted = bool.Parse(dr["IsDeleted"].ToString()),              
            };
            if (dr["BrandStatus"].ToString() == "0") brand.BrandStatus=BrandStatus.InActive;
            else if (dr["BrandStatus"].ToString() == "1") brand.BrandStatus = BrandStatus.Active;

            return brand;
        }
    }
}
