using System.Collections.Generic;

namespace FileReader.Entities
{
    public class User
    {
        public User(string userName)            
        {
            UserName = userName;
            FollowedBy = new HashSet<User>();
            Follows = new HashSet<User>();
        }
        
        public string UserName { get; private set; }

        public HashSet<User> FollowedBy { get; private set; }

        public HashSet<User> Follows { get; private set; }
    }
}
