using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarsRoverApi.Core.Handlers.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using AutoMapper;
using MarsRoverApi.Core.AutoMapper;
using MarsRoverMessages;

namespace MarsRoverApi.Core.Handlers.CQRS.Command.Tests
{
    [TestClass()]
    public class MoveRoverHandlerTests
    {
        private static IMapper _mapper;
        private static Mock<ILogger<MoveRoverHandler>> _mockLogger = new Mock<ILogger<MoveRoverHandler>>();

        public MoveRoverHandlerTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MoveRoverProfile());
            });
            _mapper = mappingConfig.CreateMapper();
        }

        [TestMethod()]
        public async Task HandleTest()
        {
            var testableSession = new TestableMessageSession();

            var moveRoverHandler = new MoveRoverHandler(_mockLogger.Object, _mapper, testableSession);

            var moveCommand = new MoveRoverCommand
            {
                MoveRoverCommandSteps = new List<MoveRoverCommandStep>()
                {
                    new MoveRoverCommandStep {Direction = Direction.Forward, Milliseconds = 1 },
                    new MoveRoverCommandStep {Direction = Direction.Right, Milliseconds = 1 },
                    new MoveRoverCommandStep {Direction = Direction.Forward, Milliseconds = 1 }
                }
            };
            var result = await moveRoverHandler.Handle(moveCommand, CancellationToken.None);

            Assert.AreEqual(1, testableSession.SentMessages.Length);
            var sentMessage = testableSession.SentMessages[0].Message;
            Assert.IsInstanceOfType<MoveRoverRequestMessage>(sentMessage);
            MoveRoverRequestMessageStep step0 = ((MoveRoverRequestMessage)sentMessage).MoveRoverRequestMessageSteps[0];
            MoveRoverRequestMessageStep step1 = ((MoveRoverRequestMessage)sentMessage).MoveRoverRequestMessageSteps[1];
            MoveRoverRequestMessageStep step2 = ((MoveRoverRequestMessage)sentMessage).MoveRoverRequestMessageSteps[2];

            Assert.AreEqual(MarsRoverMessages.Direction.Forward, step0.Direction);
            Assert.AreEqual(1, step0.Milliseconds);
            Assert.AreEqual(MarsRoverMessages.Direction.Right, step1.Direction);
            Assert.AreEqual(1, step1.Milliseconds);
            Assert.AreEqual(MarsRoverMessages.Direction.Forward, step2.Direction);
            Assert.AreEqual(1, step2.Milliseconds);

            Assert.IsTrue(result);
        }
    }
}