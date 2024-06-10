using AutoMapper;
using MarsRoverApi.Core.AutoMapper;
using MarsRoverApi.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NServiceBus.Testing;

namespace MarsRoverApi.Core.Handlers.CQRS.Command.Tests
{
    [TestClass()]
    public class MoveRoverHandlerTests
    {
        private static readonly IMapper _mapper = new MapperConfiguration(mc=>mc.AddProfile(new MoveRoverProfile())).CreateMapper();
        private static readonly Mock<ILogger<MoveRoverHandler>> _mockLogger = new();

        [TestMethod()]
        public async Task HandleTest()
        {
            var testableSession = new TestableMessageSession();

            var moveRoverHandler = new MoveRoverHandler(_mockLogger.Object, _mapper, testableSession);

            var moveCommand = new MoveRoverCommand { Direction = Direction.Forward, Milliseconds = 1 };
            var result = await moveRoverHandler.Handle(moveCommand, CancellationToken.None);

            Assert.AreEqual(1, testableSession.SentMessages.Length);
            var sentMessage = testableSession.SentMessages[0].Message;
            Assert.IsInstanceOfType<MoveRoverRequestMessage>(sentMessage);

            Assert.AreEqual(MarsRoverApi.Messages.Direction.Forward, ((MoveRoverRequestMessage)sentMessage).Direction);
            Assert.AreEqual(1, ((MoveRoverRequestMessage)sentMessage).Milliseconds);

            Assert.IsTrue(result);
        }
    }
}