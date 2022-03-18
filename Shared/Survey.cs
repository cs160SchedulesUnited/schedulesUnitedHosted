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

        public int host { get; set; }

        public int id { get; set; }

        public string name { get; set; }

        public Survey(int id, string name, DateTime start, DateTime end, int host, Response[] Responses)
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
