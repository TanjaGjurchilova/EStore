using EStore.Messages.DataTransferObjects.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Messages.Request.Brand
{
    public class CreateBrandRequest
    {
        public BrandDto Brand { get; set; }
    }
}
