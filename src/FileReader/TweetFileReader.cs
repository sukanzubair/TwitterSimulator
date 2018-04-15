using System.Linq;
using System.Collections.Generic;
using log4net;
using FileReader.Entities;
using FileReader.Properties;
using FileReader.Interfaces;
using Utilities;

namespace FileReader
{
    public class TweetFileReader : BaseFileReader, ITweetReader
    {
        private ILog _logger;
        public TweetFileReader(ILog logger)
            : base(logger)
        {
            _logger = logger;
        }

        public IEnumerable<Tweet> GetTwitterTweets()
        {
            var tweetData = GetFileSplit(Settings.Default.tweetsFileFullName, Settings.Default.tweetDelimeterString);

            return tweetData.Select(x => new Tweet(x.Item1, x.Item2.Truncate(Settings.Default.maxTweetLength)));            
        }        
    }
}
