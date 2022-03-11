using System;
using System.Collections.Generic;
using System.Text;

namespace schedulesUnitedHosted.Shared
{
    public class Survey
    {
        public Response[] Responses { get; set; }

        public DateTime start { get; set; }

        public DateTime end { get; set; }

        public string host { get; set; }

        public int id { get; set; }

        public string name { get; set; }

        public Survey(Response[] Responses, DateTime start, DateTime end, string host, int id, string name)
        {
            this.Responses = Responses;
            this.start = start;
            this.end = end;
            this.host = host;
            this.id = id;
            this.name = name;
        }
    }
}
