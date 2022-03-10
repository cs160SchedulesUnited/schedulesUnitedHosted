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
    }
}
