using System;
using System.Collections.Generic;
using System.Text;

namespace schedulesUnitedHosted.Shared
{
    // This is a wrapper class so I don't have to figure out how to pull two variables from a post body
    // this may be removed once I find a better solution
    public class UserSurvey
    {
        public User user { get; set; }
        public Survey survey { get; set; }

        public UserSurvey(Survey s, User u)
        {
            user = u;
            survey = s;
        }
    }
}
