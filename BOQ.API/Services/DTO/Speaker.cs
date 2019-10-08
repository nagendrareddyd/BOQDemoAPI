using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOQ.API.Services.DTO
{
    public class Speaker
    {
        public Speaker() { }
        public Speaker(string href, List<Data> data, List<Link> links)
        {
            this.href = href;
            this.data = data;
            this.links = links;
        }

        public string href { get; set; }
        public List<Data> data { get; set; }
        public List<Link> links { get; set; }
    }
}
