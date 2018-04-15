using Moq;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.IO;
using System.Linq;

namespace FileReader.Test
{
    [TestClass]
    public class FileReaderBaseTest : BaseFileReader
    {
        private static Mock<ILog> _logger = new Mock<ILog>();

        public FileReaderBaseTest()
            : base(_logger.Object)
        {
        }

        [TestMethod]        
        public void GetFileSplitTestNoFileExisit()
        {
            _logger.ResetCalls();
            var result = GetFileSplit(@".\tweet.txt_filenotfound", "> ");
            Assert.ThrowsException<FileNotFoundException>(() => result.GetEnumerator().MoveNext());
            _logger.Verify(x => x.Error(It.IsAny<string>()), Times.Exactly(1));
        }

        [TestMethod]
        [DeploymentItem(Constants.TEST_FILE_PATH + Constants.GOOD_FILE)]
        public void GetFileSplitTestGood()
        {            
            _logger.ResetCalls();
            var goodTest = GetFileSplit(@".\" + Constants.GOOD_FILE, "> ").ToList();
            Assert.IsTrue(goodTest.Count == 3);
            Assert.IsTrue(goodTest[0].Item1 == "Alan");
            Assert.IsTrue(goodTest[0].Item2 == "If you have a procedure with 10 parameters, you probably missed some.");
            _logger.Verify(x => x.Error(It.IsAny<string>()), Times.Exactly(0));                  
        }

        [TestMethod]
        [DeploymentItem(Constants.TEST_FILE_PATH + Constants.BAD_NO_MARKER_FILE)]
        public void GetFileSplitTestBadNoMarker()
        {
            _logger.ResetCalls();
            var badTest = GetFileSplit(@".\" + Constants.BAD_NO_MARKER_FILE, "> ").ToList();
            Assert.IsTrue(badTest.Count == 4);
            Assert.IsTrue(badTest[0].Item1 == "Alan");
            Assert.IsTrue(badTest[0].Item2 == "If you have a procedure with 10 parameters, you probably missed some.");
            Assert.IsTrue(badTest[0].Item2 == "If you have a procedure with 10 parameters, you probably missed some.");
            _logger.Verify(x => x.Error(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        [DeploymentItem(Constants.TEST_FILE_PATH + Constants.BAD_NO_USERNAME_FILE)]
        public void GetFileSplitTestBadUserName()
        {
            _logger.ResetCalls();
            var badTest = GetFileSplit(@".\" + Constants.BAD_NO_USERNAME_FILE, "> ").ToList();
            Assert.IsTrue(badTest.Count == 3);
            Assert.IsTrue(badTest[0].Item1 == "Alan");
            Assert.IsTrue(badTest[0].Item2 == "If you have a procedure with 10 parameters, you probably missed some.");
            Assert.IsTrue(badTest[0].Item2 == "If you have a procedure with 10 parameters, you probably missed some.");
            _logger.Verify(x => x.Error(It.IsAny<string>()), Times.Exactly(1));
        }
    }
}
