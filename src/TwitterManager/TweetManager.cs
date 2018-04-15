using System;
using System.Collections.Generic;
using log4net;
using FileReader.Interfaces;
using TwitterManager.Interfaces;

namespace TwitterManager
{
    public class TweetManager : ITweetManager
    {
        private ILog _logger;
        IUserReader _userReader;
        ITweetReader _tweetReader;

        public static string tweetFormat = "\t@{0}: {1}";

        public TweetManager(ILog logger, IUserReader userReader, ITweetReader tweetReader)
        {
            _logger = logger;
            _userReader = userReader;
            _tweetReader = tweetReader;
        }        

        public IDictionary<string, List<string>> GetTweetsOrderedByUser()
        {
            SortedDictionary<string, List<string>> userWithTweets = new SortedDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);  
              
            var users = _userReader.GetTwitterUsers();
            foreach(var user in users.Keys)
            {
                userWithTweets[user] = new List<string>();
            }       
            
            foreach (var tweet in _tweetReader.GetTwitterTweets())
            {
                if(!userWithTweets.ContainsKey(tweet.UserName))
                {
                    _logger.Warn($"Found tweet from unknown user: [{tweet.UserName}] - [{tweet.Text}] and will be skipped!!!");
                    continue;
                }

                // add tweet to user messages
                string formatedTweet = string.Format(tweetFormat, tweet.UserName, tweet.Text);
                userWithTweets[tweet.UserName].Add(formatedTweet);

                // add tweet to all users that follows this user
                foreach(var follower in users[tweet.UserName].FollowedBy)
                {
                    userWithTweets[follower.UserName].Add(formatedTweet);
                }
            }

            return userWithTweets;
        }
    }
}
