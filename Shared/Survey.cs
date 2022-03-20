using System;
using System.Collections.Generic;
using System.Text;

namespace schedulesUnitedHosted.Shared
{
    public class Survey
    {
        public List<Response> Responses { get; set; }

        public DateTime start { get; set; }

        public DateTime end { get; set; }

        public int host { get; set; }

        public int id { get; set; }

        public string name { get; set; }

        public Survey(int id, string name, DateTime start, DateTime end, int host, List<Response> Responses)
        {
            this.Responses = Responses;
            this.start = start;
            this.end = end;
            this.host = host;
            this.id = id;
            this.name = name;
        }

        public Survey(string name, DateTime start, DateTime end, int host, List<Response> Responses)
        {
            this.Responses = Responses;
            this.start = start;
            this.end = end;
            this.host = host;
            this.id = 0;
            this.name = name;
        }

        public string ToString()
        {
            return $"id: {id}, name: {name}, host: {host}, start: {start}, end: {end}, Responses: {Responses}";
        }
    }
}
