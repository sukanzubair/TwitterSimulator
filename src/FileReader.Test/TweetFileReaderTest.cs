using Moq;
using log4net;
using System.Linq;
using FileReader.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace FileReader.Test
{
    [TestClass]
    public class TweetFileReaderTest
    {
        private static TweetFileReader _fileReader;
        private static Mock<ILog> _logger = new Mock<ILog>();

        public TweetFileReaderTest()
        {
        }

        [TestInitialize]
        public void Initialize()
        {
            _fileReader = new TweetFileReader(_logger.Object);

            // force a reload and lazy execution on App settings
            Settings.Default.Reload();
            var dummy = Settings.Default.tweetsFileFullName;

            Settings.Default.PropertyValues["maxTweetLength"].PropertyValue = 140;
            Settings.Default.PropertyValues["tweetDelimeterString"].PropertyValue = "> ";
        }

        [TestMethod]
        [DeploymentItem(Constants.TEST_FILE_PATH + Constants.GOOD_FILE)]
        public void GetTwitterTweetsTestGoodFile()
        {
            Settings.Default.PropertyValues["tweetsFileFullName"].PropertyValue = @".\" + Constants.GOOD_FILE;
            var results = _fileReader.GetTwitterTweets().ToList();

            Assert.IsTrue(results.Count() == 3);
            Assert.IsTrue(results.Exists(x => x.UserName == "Ward" &&
                                              x.Text == "There are only two hard things in Computer Science: cache invalidation, naming things and off-by-1 errors."));

        }

        [TestMethod]
        [DeploymentItem(Constants.TEST_FILE_PATH + Constants.BAD_NO_MARKER_FILE)]
        public void GetTwitterTweetsTestBadFile()
        {
            Settings.Default.PropertyValues["tweetsFileFullName"].PropertyValue = @".\" + Constants.BAD_NO_MARKER_FILE;
            var results = _fileReader.GetTwitterTweets().ToList();
            Assert.IsTrue(results.Count() == 4);
            Assert.IsTrue(results.Exists(x => x.UserName == "Zubair" && x.Text == "Test"));
        }

        [TestMethod]
        [DeploymentItem(Constants.TEST_FILE_PATH + Constants.TWEET_TRUCATE_FILE)]
        public void GetTwitterTweetsTestTweetTruncate()
        {
            Settings.Default.PropertyValues["tweetsFileFullName"].PropertyValue = @".\" + Constants.TWEET_TRUCATE_FILE;
            var results = _fileReader.GetTwitterTweets().ToList();
            Assert.IsTrue(results.Count() == 1);
            Assert.IsTrue(results.Exists(x => x.UserName == "Alan" && x.Text.Length == 140));
        }
    }
}
