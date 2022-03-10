using System;
using System.Collections.Generic;
using System.Text;

namespace schedulesUnitedHosted.Shared
{
    public class Response
    {
        public string AccId { get; set; }

        public string EventId { get; set; }

        public DateTime[] Availabilities { get; set; }

    }
}
