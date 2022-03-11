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

        public Response(string AccId, string EventId, DateTime[] Availabilities)
        {
            this.AccId = AccId;
            this.EventId = EventId;
            this.Availabilities = Availabilities;
        }

    }
}
