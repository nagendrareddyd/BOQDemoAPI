using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOQ.API.Services.DTO
{
    public class RootObject
    {
        public Collection collection { get; set; }
    }

    public class Template
    {
        public List<object> data { get; set; }
    }
}
