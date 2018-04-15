using Moq;
using log4net;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitterManager;
using FileReader.Interfaces;
using FileReader.Entities;

namespace FileReader.Test
{
    [TestClass]
    public class TweetManagerTest
    {
        public static string tweetFormat = "\t@{0}: {1}";
        private static TweetManager _tweetManager;
        private static Mock<ILog> _logger = new Mock<ILog>();
        private static Mock<IUserReader> _userReader = new Mock<IUserReader>();
        private static Mock<ITweetReader> _tweetReader = new Mock<ITweetReader>();


        //mock data
        private  User _alan = new User("Alan");
        private  User _ward = new User("Ward");
        private  User _martin = new User("Martin");                       
       
        private Dictionary<string, User> _users;
        private List<Tweet> _tweets;
        private List<string> _formattedTweet;

        public TweetManagerTest()            
        {            
        }

        [TestInitialize]
        public void Initialize()
        {
            _tweetManager = new TweetManager(_logger.Object, _userReader.Object, _tweetReader.Object);
             
            //setup mock data
            _users = new Dictionary<string, User>
            {
                { _alan.UserName,  _alan },
                { _ward.UserName,  _ward },
                { _martin.UserName,  _martin },
            };

            _tweets = new List<Tweet>
            {
                new Tweet("Alan", "If you have a procedure with 10 parameters, you probably missed some." ),
                new Tweet("Ward", "There are only two hard things in Computer Science: cache invalidation, naming things and off-by-1 errors." ),
                new Tweet("Alan", "Random numbers should not be generated with a method chosen at random." )
            };

            _formattedTweet = new List<string>
            {
                string.Format(tweetFormat, _tweets[0].UserName, _tweets[0].Text),
                string.Format(tweetFormat, _tweets[1].UserName, _tweets[1].Text),
                string.Format(tweetFormat, _tweets[2].UserName, _tweets[2].Text)
            };

            //setup
            _alan.Follows.Add(_martin);
            _martin.FollowedBy.Add(_alan);

            _ward.Follows.Add(_alan);
            _alan.FollowedBy.Add(_ward);

            _ward.Follows.Add(_martin);
            _ward.Follows.Add(_alan);
            _martin.Follows.Add(_alan);
            _martin.Follows.Add(_martin);
        }

        [TestMethod]        
        public void GetTwitterUsersTestGood()
        {
            _userReader.Setup(x => x.GetTwitterUsers()).Returns(() => _users);
            _tweetReader.Setup(x => x.GetTwitterTweets()).Returns(() => _tweets);

            var result = _tweetManager.GetTweetsOrderedByUser();

            CheckStandardMessages(result);
        }

        [TestMethod]
        public void GetTwitterUsersTestUnknowTweet()
        {
            var unknowTweet = new Tweet("unknownUser", "tweet form unknown user");
            _tweets.Add(unknowTweet);

            _userReader.Setup(x => x.GetTwitterUsers()).Returns(() => _users);
            _tweetReader.Setup(x => x.GetTwitterTweets()).Returns(() => _tweets);

            _logger.ResetCalls();
            var result = _tweetManager.GetTweetsOrderedByUser();

            CheckStandardMessages(result);

            _logger.Verify(x => x.Warn(It.IsAny<string>()), Times.Exactly(1));
            Assert.IsFalse(result.Keys.Contains(unknowTweet.UserName));
        }

        public void CheckStandardMessages(IDictionary<string, List<string>> result)
        {
            Assert.IsTrue(result.Keys.Count == 3);

            var first = result.First();
            Assert.IsTrue(first.Key == "Alan");
            Assert.IsTrue(first.Value.Count == 2);
            Assert.IsTrue(first.Value[0] == _formattedTweet[0]);
            Assert.IsTrue(first.Value[1] == _formattedTweet[2]);

            var last = result.Last();
            Assert.IsTrue(last.Key == "Ward");
            Assert.IsTrue(last.Value.Count == 3);
            Assert.IsTrue(last.Value[0] == _formattedTweet[0]);
            Assert.IsTrue(last.Value[1] == _formattedTweet[1]);
            Assert.IsTrue(last.Value[2] == _formattedTweet[2]);
        }
    }
}
