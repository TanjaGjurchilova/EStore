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
    public class ProductRepository : IProductRepository
    {
        private readonly Db _context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ProductRepository(IConfiguration configuration)
        {
            _context = new Db();
        }
        public void DeleteProduct(Product Product)
        {
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText =
                    "DELETE FROM public.\"Product\" p  WHERE p.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", Product.Id, NpgsqlDbType.Integer);
                _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public Product FindProductById(long id)
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
                    "SELECT * FROM public.\"Product\" p INNER JOIN public.\"Category\" c ON c.\"Id\" = p.\"CategoryId\" INNER JOIN public.\"Brand\" b ON b.\"Id\" = p.\"BrandId\"  WHERE p.\"Id\" =:id;";
                _context.CreateParameterFunc(cmd, "@id", id, NpgsqlDbType.Integer);
                dt = _context.ExecuteSelectCommand(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
            return CreateProductObject(dt.Rows[0]);
        }
    

        public IEnumerable<Product> GetAllProducts()
        {
        DataTable dt;

        try
        {
            var cmd = _context.CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandText = "SELECT * FROM public.\"Product\" p  INNER JOIN public.\"Category\" c ON c.\"Id\" = p.\"CategoryId\" INNER JOIN public.\"Brand\" b ON b.\"Id\" = p.\"BrandId\"  ORDER bY p.\"Id\" DESC";
               
                dt = _context.ExecuteSelectCommand(cmd);
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            throw new Exception(ex.Message);
        }

        List<Product> list = (from DataRow dr in dt.Rows select CreateProductObject(dr)).ToList();

        return list;
    }

        public void SaveProduct(Product Product)
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
                cmd.CommandText = "SELECT * FROM public.\"Product\" b WHERE LOWER(b.\"Name\")=LOWER(:n);";
                _context.CreateParameterFunc(cmd, "@n", Product.Name, NpgsqlDbType.Text);

                dt = _context.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count == 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    cmd.CommandText = "INSERT INTO public.\"Product\"(\"ProductStatus\", \"CreateDate\", \"IsDeleted\", \"Description\", \"MetaDescription\", \"MetaKeywords\", \"ModifiedDate\", \"Name\", \"Slug\""
                        + ", \"BrandId\", \"CategoryId\", \"IsBestseller\", \"IsFeatured\",\"ImageUrl\", \"Model\",\"OldPrice\", \"Price\", \"SKU\", \"SalePrice\",\"QuantityInStock\""
                         + ") VALUES ( :bs, :cd, :id, :de, :md, :mk, :d, :n, :s, :bid, :cid, :isb, :isf, :iu, :m, :op, :p, :sku, :sp, :qs);";
                    if (Product.ProductStatus == ProductStatus.Active)
                    {
                        status = 1;
                    }
                    else
                    {
                        status = 0;
                    }
                    _context.CreateParameterFunc(cmd, "@bs", status, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@id", Product.IsDeleted, NpgsqlDbType.Boolean);

                    _context.CreateParameterFunc(cmd, "@cd", Product.CreateDate.ToString(), NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@de", Product.Description, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@md", Product.MetaDescription, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@mk", Product.MetaKeywords, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@d", Product.ModifiedDate.ToString(), NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@n", Product.Name, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@s", Product.Slug, NpgsqlDbType.Text);

                    _context.CreateParameterFunc(cmd, "@iu", Product.ImageUrl, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@m", Product.Model, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@sku", Product.SKU, NpgsqlDbType.Text);

                    _context.CreateParameterFunc(cmd, "@op", Product.OldPrice.ToString(), NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@p", Product.Price.ToString(), NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@sp", Product.SalePrice.ToString(), NpgsqlDbType.Text);

                    _context.CreateParameterFunc(cmd, "@bid", Product.BrandId, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@cid", Product.CategoryId, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@isb", Product.IsBestseller, NpgsqlDbType.Boolean);
                    _context.CreateParameterFunc(cmd, "@isf", Product.IsFeatured, NpgsqlDbType.Boolean);
                    _context.CreateParameterFunc(cmd, "@qs", Product.QuantityInStock, NpgsqlDbType.Integer);



                    var rowsAffected = _context.ExecuteNonQuery(cmd);

                }
                else
                {
                    throw new Exception("Product exist");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public void UpdateProduct(Product Product)
        {
            DataTable dt;
            try
            {
                var cmd = _context.CreateCommand();
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = "SELECT * FROM public.\"Product\" p WHERE LOWER(p.\"Name\")=LOWER(:n);";
                _context.CreateParameterFunc(cmd, "@n", Product.Name, NpgsqlDbType.Text);

                dt = _context.ExecuteSelectCommand(cmd);

                if (dt.Rows.Count == 0)
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    cmd.CommandText = "UPDATE public.\"Product\" b SET \"ProductStatus\"=:bs, \"CreateDate\"=:cd, \"IsDeleted\"=:id, \"Description\"=:de, \"MetaDescription\"=:md, \"MetaKeywords\"=:mk, \"ModifiedDate\"=:d, \"Name\"=:n, \"Slug\"=:s,"
                        +" \"ImageUrl\"=:iu,\"Model\"=:m,\"SKU\"=:sku,\"OldPrice\"=:op,\"Price\"=:p,\"SalePrice\"=:sp,\"BrandId\"=:bid,\"CategoryId\"=:cid,\"IsBestseller\"=:isb,IsFeatured\"=:isf,\"QuantityInStock\"=:qs"
                        +" WHERE b.\"Id\" =:id ;";
                    int status;
                    if (Product.ProductStatus == ProductStatus.Active)
                    {
                        status = 1;
                    }
                    else
                    {
                        status = 0;
                    }

                    _context.CreateParameterFunc(cmd, "@bs", status, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@id", Product.IsDeleted, NpgsqlDbType.Integer);

                    _context.CreateParameterFunc(cmd, "@cd", Product.CreateDate, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@de", Product.Description, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@md", Product.MetaDescription, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@mk", Product.MetaKeywords, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@d", Product.ModifiedDate, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@n", Product.Name, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@s", Product.Slug, NpgsqlDbType.Text);

                    _context.CreateParameterFunc(cmd, "@iu", Product.ImageUrl, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@m", Product.Model, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@sku", Product.SKU, NpgsqlDbType.Text);

                    _context.CreateParameterFunc(cmd, "@op", Product.OldPrice, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@p", Product.Price, NpgsqlDbType.Text);
                    _context.CreateParameterFunc(cmd, "@sp", Product.SalePrice, NpgsqlDbType.Text);

                    _context.CreateParameterFunc(cmd, "@bid", Product.BrandId, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@cid", Product.CategoryId, NpgsqlDbType.Integer);
                    _context.CreateParameterFunc(cmd, "@isb", Product.IsBestseller, NpgsqlDbType.Boolean);
                    _context.CreateParameterFunc(cmd, "@isf", Product.IsFeatured, NpgsqlDbType.Boolean);
                    _context.CreateParameterFunc(cmd, "@qs", Product.QuantityInStock, NpgsqlDbType.Integer);


                    var rowsAffected = _context.ExecuteNonQuery(cmd);

                }
                throw new Exception("Product exist");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }
        private static Product CreateProductObject(DataRow dr)
        {
            var product = new Product
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
                BrandId = int.Parse(dr["BrandId"].ToString()),
                CategoryId = int.Parse(dr["CategoryId"].ToString()),
                QuantityInStock = int.Parse(dr["QuantityInStock"].ToString()),
                ImageUrl= dr["ImageUrl"].ToString(),
                Model = dr["Model"].ToString(),
                SKU = dr["SKU"].ToString(),
                OldPrice = decimal.Parse(dr["OldPrice"].ToString()),
                Price= decimal.Parse(dr["Price"].ToString()),
                SalePrice=decimal.Parse(dr["SalePrice"].ToString()),
                IsBestseller = bool.Parse(dr["IsBestseller"].ToString()),
                IsFeatured = bool.Parse(dr["IsFeatured"].ToString()),
                
            };
            product.Brand = new Brand
            {
                Slug = dr["Slug2"].ToString(),
            };
            product.Category = new Category
            {
                Slug = dr["Slug1"].ToString(),
            };
            if (dr["ProductStatus"].ToString() == "0") product.ProductStatus = ProductStatus.InActive;
            else if (dr["ProductStatus"].ToString() == "1") product.ProductStatus = ProductStatus.Active;

            return product;
        }
    }
}
