using System;
using System.Collections.Generic;
using System.Text;

namespace schedulesUnitedHosted.Shared
{
    public class Utilities
    {
        public List<intDate> getAvailablilityList(List<Response> availabilities, int OwnerId)
        {
            List<intDate> list = new List<intDate>();
            foreach (Response r in availabilities)
            {
                if (r.AccId == OwnerId)
                {
                    list.Add(new intDate(1, r.Availability.AddHours(r.Hour)));
                }
            }
            foreach (Response r in availabilities)
            {
                if(r.AccId != OwnerId)
                {
                    DateTime a = r.Availability.AddHours(r.Hour);
                    Predicate<intDate> test = b => b.date == a.Date;
                    intDate match = list.Find(test);
                    int index = list.IndexOf(match);
                    list[index].num++;
                }
            }
            return list;
        }

        public int getNumberResponders(List<Response> availabilities)
        {
            int num = 0;
            List<int> ids = new List<int>();
            foreach (Response r in availabilities)
            {
                if(!ids.Contains(r.AccId))
                {
                    ids.Add(r.AccId);
                    num++;
                }
            }
            return num;
        }
    }

    public class intDate
    {
        public int num;
        public DateTime date;
        public intDate(int num, DateTime date)
        {
            this.num = num;
            this.date = date;
        }
    }
}
