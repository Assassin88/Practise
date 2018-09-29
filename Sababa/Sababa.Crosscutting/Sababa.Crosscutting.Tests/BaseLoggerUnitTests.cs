using NLog;
using NLog.Targets;
using NUnit.Framework;
using Sababa.Crosscutting.Logging;

namespace Sababa.Logic.Tests
{
    [TestFixture]
    public class BaseLoggerUnitTests
    {
        private BaseLogger _logger;
        private DebugTarget _target;
        private string _testMessage = "Test_message";

        [SetUp]
        public void SetUp()
        {
            _target = LogManager.Configuration.FindTargetByName("debug") as DebugTarget;
            _logger = new BaseLogger("Test");
        }

        [Test]
        public void LogTrace_WriteCorrectMessage_ReturnLognameLevelMessage()
        {
            _logger.LogTrace(_testMessage);

            Assert.That(_target.LastMessage, Is.EqualTo("Test|Trace|" + _testMessage));
        }

        [Test]
        public void LogDebug_WriteCorrectMessage_ReturnLognameLevelMessage()
        {
            _logger.LogDebug(_testMessage);

            Assert.That(_target.LastMessage, Is.EqualTo("Test|Debug|" + _testMessage));
        }

        [Test]
        public void LogInfo_WriteCorrectMessage_ReturnLognameLevelMessage()
        {
            _logger.LogInfo(_testMessage);

            Assert.That(_target.LastMessage, Is.EqualTo("Test|Info|" + _testMessage));
        }

        [Test]
        public void LogWarn_WriteCorrectMessage_ReturnLognameLevelMessage()
        {
            _logger.LogWarn(_testMessage);

            Assert.That(_target.LastMessage, Is.EqualTo("Test|Warn|" + _testMessage));
        }

        [Test]
        public void LogError_WriteCorrectMessage_ReturnLognameLevelMessage()
        {
            _logger.LogError(_testMessage);

            Assert.That(_target.LastMessage, Is.EqualTo("Test|Error|" + _testMessage));
        }

        [Test]
        public void LogFatal_WriteCorrectMessage_ReturnLognameLevelMessage()
        {
            _logger.LogFatal(_testMessage);

            Assert.That(_target.LastMessage, Is.EqualTo("Test|Fatal|" + _testMessage));
        }
    }
}
