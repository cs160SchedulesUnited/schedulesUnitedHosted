using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace schedulesUnitedHosted.Shared
{
    public class Utilities
    {
        public List<intDate> getOwnerAvailablilityList(List<Response> availabilities, int OwnerId)
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

        public List<intDate> getTimesOpen(List<Response> availabilities)
        {
            List<intDate> res = new List<intDate>();
            foreach(Response r in availabilities)
            {
                DateTime a = r.Availability.AddHours(r.Hour);
                Predicate<intDate> test = b => b.date == a.Date;
                intDate match = res.Find(test);
                if(match == null)
                {
                    int length = res.Count - 1;
                    res[length] = new intDate(1, a);
                }
                else
                {
                    int index = res.IndexOf(match);
                    res[index].num++;
                }
            }
            return res;
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

        public String hashPass(String password)
        {
            String hashed;
            Rfc2898DeriveBytes RFCDB = new Rfc2898DeriveBytes(password, 8, 10000);
            byte[] hashedPW = RFCDB.GetBytes(20);
            byte[] salt = RFCDB.Salt;
            hashed = Encoding.GetEncoding("iso-8859-1").GetString(salt) + Encoding.ASCII.GetString(hashedPW);
            return hashed;
        }

        public int saltLength(String password)
        {
            String hashed;
            Rfc2898DeriveBytes RFCDB = new Rfc2898DeriveBytes(password, 8, 10000);
            byte[] hashedPW = RFCDB.GetBytes(20);
            byte[] salt = RFCDB.Salt;
            String saltString = Encoding.GetEncoding("iso-8859-1").GetString(salt);
            return saltString.Length;
        }

        public String hashPass(String password, byte[] salt)
        {
            String hashed;
            Rfc2898DeriveBytes RFCDB = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hashedPW = RFCDB.GetBytes(20);
            hashed = Encoding.ASCII.GetString(salt) + Encoding.GetEncoding("iso-8859-1").GetString(hashedPW);
            return hashed;
        }

        public Boolean verifyPass(String pass, String hash)
        {
            byte[] salt = Encoding.GetEncoding("iso-8859-1").GetBytes(hash.Substring(0, 8));
            Rfc2898DeriveBytes RFCDB = new Rfc2898DeriveBytes(pass, salt, 10000);
            String comp = Encoding.GetEncoding("iso-8859-1").GetString(salt) + Encoding.ASCII.GetString(RFCDB.GetBytes(20));
            
            if (comp == hash) return true;
            else return false;
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
