using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EStore.Messages.Response
{
    public class ResponseBase
    {
        public ResponseBase()
        {
            Messages = new List<string>;  
        }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Messages { get; set; }
    }
}
