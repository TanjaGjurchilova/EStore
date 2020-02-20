using EStore.Infrastructure;
using EStore.Models.Product;
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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly Db _context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CategoryRepository(IConfiguration configuration)
        {
            _context = new Db();
        }
        public void DeleteCategory(Category Category)
        {
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    "DELETE FROM public.\"Category\" c  WHERE c.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", Category.Id, NpgsqlDbType.Integer);
                _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public Category FindCategoryById(long id)
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
                    "SELECT * FROM public.\"Category\" c  WHERE c.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", id, NpgsqlDbType.Integer);
                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
            return CreateCategoryObject(dt.Rows[0]);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            DataTable dt;

            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM public.\"Category\" c  ORDER bY c.\"Id\" DESC";

                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }

            List<Category> list = (from DataRow dr in dt.Rows select CreateCategoryObject(dr)).ToList();

            return list;
        }

        public void SaveCategory(Category Category)
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
                cmd.CommandText = "SELECT * FROM public.\"Category\" WHERE LOWER(name)=LOWER(:n);";
                _context.CreateParameterFunc(cmd, "@n", Category.Name, NpgsqlDbType.Text);

                dt = _context.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count == 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    cmd.CommandText = "INSERT INTO public.\"Category\"(\"CategoryStatus\", \"CreateDate\", \"IsDeleted\", \"Description\", \"MetaDescription\", \"MetaKeywords\", \"ModifiedDate\", \"Name\", \"Slug\")VALUES ( :bs, :cd, :id, :de, :md, :mk, :d, :n, :s);";

                    if (Category.CategoryStatus == CategoryStatus.Active)
                    {
                        status = 1;
                    }
                    else
                    {
                        status = 0;
                    }
                    _context.CreateParameterFunc(cmd, "@bs", status, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@id", Category.IsDeleted, NpgsqlDbType.Boolean);

                    _context.CreateParameterFunc(cmd, "@cd", Category.CreateDate.ToString(), NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@de", Category.Description, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@md", Category.MetaDescription, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@mk", Category.MetaKeywords, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@d", Category.ModifiedDate.ToString(), NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@n", Category.Name, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@s", Category.Slug, NpgsqlDbType.Text);


                    var rowsAffected = _context.ExecuteNonQuery(cmd);

                }
                throw new Exception("Category exist");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public void UpdateCategory(Category Category)
        {
            DataTable dt;
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM public.\"Category\" WHERE LOWER(name)=LOWER(:n);";
                _context.CreateParameterFunc(cmd, "@n", Category.Name, NpgsqlDbType.Text);

                dt = _context.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count == 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    cmd.CommandText = "UPDATE public.\"Category\" b SET \"CategoryStatus\"=:bs, \"CreateDate\"=:cd, \"IsDeleted\"=:id, \"Description\"=:de, \"MetaDescription\"=:md, \"MetaKeywords\"=:mk, \"ModifiedDate\"=:d, \"Name\"=:n, \"Slug\"=:s WHERE b.\"Id\" =:id ;";

                    _context.CreateParameterFunc(cmd, "@bs", Category.CategoryStatus, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@id", Category.IsDeleted, NpgsqlDbType.Integer);

                    _context.CreateParameterFunc(cmd, "@cd", Category.CreateDate, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@de", Category.Description, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@md", Category.MetaDescription, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@mk", Category.MetaKeywords, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@d", Category.ModifiedDate, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@n", Category.Name, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@s", Category.Slug, NpgsqlDbType.Text);


                    var rowsAffected = _context.ExecuteNonQuery(cmd);

                }
                throw new Exception("Category exist");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }
        private static Category CreateCategoryObject(DataRow dr)
        {
            var category = new Category
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
            if (dr["CategoryStatus"].ToString() == "0") category.CategoryStatus = CategoryStatus.InActive;
            else if (dr["CategoryStatus"].ToString() == "1") category.CategoryStatus = CategoryStatus.Active;

            return category;
        }
    }
}
