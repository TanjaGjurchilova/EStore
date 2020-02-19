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
            throw new NotImplementedException();
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
                    "SELECT * FROM public.brand b  WHERE b.\"Id\" =:id;";
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
                cmd.CommandText = "SELECT * FROM public.brand b  ORDER bY b.\"Id\" DESC";

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
            throw new NotImplementedException();
        }

        public void UpdateBrand(Brand brand)
        {
            throw new NotImplementedException();
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

            //user.Appruved = false;
            //user.CompanyUser = true;
            return brand;
        }
    }
}
