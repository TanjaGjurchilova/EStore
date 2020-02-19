using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Messages.Request.Brand
{
    public class FetchBrandRequest
    {
        public int PageNumber { get; set; }
        public int BrandsPerPage { get; set; }
    }
}
