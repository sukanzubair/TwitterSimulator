using System;
using System.Linq;
using System.Collections.Generic;
using log4net;
using FileReader.Entities;
using FileReader.Properties;
using FileReader.Interfaces;


namespace FileReader
{
    public class UserFileReader : BaseFileReader, IUserReader
    {
        private ILog _logger;

        public UserFileReader(ILog logger)
            :base(logger)
        {
            _logger = logger;
        }

        public IDictionary<string, User> GetTwitterUsers()
        {            
            var userLines = GetFileSplit(Settings.Default.userFileFullName, Settings.Default.userDelimeterString);

            Dictionary<string, User> users = new Dictionary<string, User>(StringComparer.OrdinalIgnoreCase);
            foreach (var userLine in userLines)
            {
                var followers = userLine.Item2.Split(',').Select(x=> x.Trim()).ToList();
                var removed = followers.RemoveAll(x => x.Length == 0);
                if (removed > 0)
                {
                    _logger.Error($"Removed {removed} empty followers from - { userLine.Item2 } ");
                }

                User currentUser;
                if (!users.TryGetValue(userLine.Item1, out currentUser))  
                {
                    currentUser = new User(userLine.Item1);
                    users.Add(userLine.Item1, currentUser);
                }

                PopulateRelationships(users, followers, currentUser);                
            }
            return users;               
        }
                
        private void PopulateRelationships(IDictionary<string, User> users, IList<string> followers, User user)
        {
            User twitterUserFollowed;
            foreach (var follower in followers)
            {
                // create twitter user if follower was not created before                
                if (!users.TryGetValue(follower, out twitterUserFollowed))
                {
                    twitterUserFollowed = new User(follower);
                    users.Add(follower, twitterUserFollowed);                    
                }

                //set relationships for both  parties  
                twitterUserFollowed.FollowedBy.Add(user);    //followed user knows who is following him
                user.Follows.Add(twitterUserFollowed);       //follower knows who he is following  
            }            
        }        
       
    }
}
