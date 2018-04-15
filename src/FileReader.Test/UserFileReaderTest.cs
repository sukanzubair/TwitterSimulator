using Moq;
using log4net;
using System.Linq;
using FileReader.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace FileReader.Test
{
    [TestClass]
    public class UserFileReaderTest
    {
        private static UserFileReader _fileReader;
        private static Mock<ILog> _logger = new Mock<ILog>();

        public UserFileReaderTest()
        {
        }

        [TestInitialize]
        public void Initialize()
        {
            _fileReader = new UserFileReader(_logger.Object);

            // force a reload and lazy execution on App settings
            Settings.Default.Reload();
            var dummy = Settings.Default.userFileFullName;
            Settings.Default.PropertyValues["userDelimeterString"].PropertyValue = " follows ";
        }

        [TestMethod]
        [DeploymentItem(Constants.TEST_FILE_PATH + Constants.GOOD_USER_FILE)]
        public void GetTwitterUsersTestGoodFile()
        {
            Settings.Default.PropertyValues["userFileFullName"].PropertyValue = @".\" + Constants.GOOD_USER_FILE;
            var results = _fileReader.GetTwitterUsers();

            Assert.IsTrue(results.Keys.Count == 3);
            Assert.IsTrue(results.Keys.Contains("Alan"));
            Assert.IsTrue(results.Keys.Contains("Ward"));

            var user = results["Ward"];
            Assert.IsTrue(user.Follows.Count == 2);
            Assert.IsTrue(user.FollowedBy.Count == 0);

            user = results["Alan"];
            Assert.IsTrue(user.Follows.Count == 1);
            Assert.IsTrue(user.FollowedBy.Count == 1);
            Assert.IsTrue(user.FollowedBy.First().UserName == "Ward");
        }

        [TestMethod]
        [DeploymentItem(Constants.TEST_FILE_PATH + Constants.BAD_USER_FILE)]
        public void GetTwitterUsersTestBadFile()
        {
            Settings.Default.PropertyValues["userFileFullName"].PropertyValue = @".\" + Constants.BAD_USER_FILE;
            _logger.ResetCalls();
            var results = _fileReader.GetTwitterUsers();

            _logger.Verify(x => x.Error(It.IsAny<string>()), Times.Exactly(3));
            Assert.IsTrue(results.Keys.Count == 4);
            Assert.IsTrue(results.Keys.Contains("Alan"));
            Assert.IsTrue(results.Keys.Contains("Martin"));
            Assert.IsTrue(results.Keys.Contains("Test"));

            var user = results["Martin"];
            Assert.IsTrue(user.Follows.Count == 1);
            Assert.IsTrue(user.Follows.First().UserName == "Test");
            Assert.IsTrue(user.FollowedBy.Count == 2);

            user = results["Test"];
            Assert.IsTrue(user.Follows.Count == 0);
            Assert.IsTrue(user.FollowedBy.First().UserName == "Martin");
            Assert.IsTrue(user.FollowedBy.Count == 1);
        }

        [TestMethod]
        [DeploymentItem(Constants.TEST_FILE_PATH + Constants.CASE_IGNORE_FILE)]
        public void GetTwitterUsersTestIgnoreCase()
        {
            Settings.Default.PropertyValues["userFileFullName"].PropertyValue = @".\" + Constants.CASE_IGNORE_FILE;
            var results = _fileReader.GetTwitterUsers();

            Assert.IsTrue(results.Keys.Count == 3);
            Assert.IsTrue(results.Keys.Contains("Alan"));
            Assert.IsTrue(results.Keys.Contains("Martin"));
            Assert.IsTrue(results.Keys.Contains("Ward"));
            Assert.IsTrue(results.Keys.FirstOrDefault(x=> x == "ALAN") == null );
            Assert.IsTrue(results.Keys.FirstOrDefault(x => x == "ALan") == null);
        }
    }
}
