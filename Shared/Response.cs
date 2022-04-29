using System;
using System.Collections.Generic;
using System.Text;

namespace schedulesUnitedHosted.Shared
{
    public class Response
    {
        public int AccId { get; set; }

        public int EventId { get; set; }

        public DateTime Availability { get; set; }

        public int Hour { get; set; }

        public Response()
        {

        }

        public Response(int AccId, int EventId, DateTime Availability, int Hour)
        {
            this.AccId = AccId;
            this.EventId = EventId;
            this.Availability = Availability;
            this.Hour = Hour;
        }

        public Response() {
        }
        public string ToString()
        {
            return $"event: {EventId}, account: {AccId}, availability: {Availability}, hour: {Hour}";
        }

    }
}
