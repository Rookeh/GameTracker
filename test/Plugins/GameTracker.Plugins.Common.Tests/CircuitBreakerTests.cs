using GameTracker.Plugins.Common.RateLimiting;
using Moq;

namespace GameTracker.Plugins.Common.Tests
{
    public class CircuitBreakerTests
    {
        [Fact]
        public async Task AttemptOperation_IfCircuitIsClosed_ShouldExecuteOperation()
        {
            // Arrange
            var testHelper = new Mock<ITestHelper>();
            var circuitBreaker = new CircuitBreaker<string>(TimeSpan.FromSeconds(5));
            var defaultReturn = string.Empty;
            var expectedReturn = "return";

            testHelper.Setup(x => x.CheckCircuitBroken())
                .Returns(false)
                .Verifiable();

            testHelper.Setup(x => x.DoSomething())
                .ReturnsAsync(expectedReturn)
                .Verifiable();

            // Act
            var result = await circuitBreaker.AttemptOperation(testHelper.Object.DoSomething, testHelper.Object.CheckCircuitBroken, defaultReturn);

            // Assert
            Assert.Equal(expectedReturn, result);
            testHelper.Verify();
            testHelper.Verify(x => x.DoSomething(), Times.Once);
        }

        [Fact]
        public async Task AttemptOperation_IfCircuitBrokenCheckTripped_OperationIsNotExecuted()
        {
            // Arrange
            var testHelper = new Mock<ITestHelper>();
            var circuitBreaker = new CircuitBreaker<string>(TimeSpan.FromSeconds(5));
            var defaultReturn = string.Empty;

            testHelper.Setup(x => x.CheckCircuitBroken())
                .Returns(true)
                .Verifiable();

            // Act
            await circuitBreaker.AttemptOperation(testHelper.Object.DoSomething, testHelper.Object.CheckCircuitBroken, defaultReturn);

            // Assert
            testHelper.Verify();
            testHelper.Verify(x => x.DoSomething(), Times.Never);
        }

        [Fact]
        public async Task AttemptOperation_IfCircuitIsOpen_ReturnsDefaultValue()
        {
            // Arrange
            var testHelper = new Mock<ITestHelper>();
            var circuitBreaker = new CircuitBreaker<string>(TimeSpan.FromSeconds(5));
            var defaultReturn = string.Empty;

            testHelper.Setup(x => x.CheckCircuitBroken())
                .Returns(true);

            // Act
            var result = await circuitBreaker.AttemptOperation(testHelper.Object.DoSomething, testHelper.Object.CheckCircuitBroken, defaultReturn);

            // Assert
            Assert.Equal(defaultReturn, result);
        }

        [Fact]
        public async Task AttemptOperation_IfCircuitIsOpenAndBackoffExpiredAndCircuitBrokenCheckIsFalse_ShouldExecuteOperation()
        {
            // Arrange
            var testHelper = new Mock<ITestHelper>();
            var circuitBreaker = new CircuitBreaker<string>(TimeSpan.FromSeconds(1));
            var defaultReturn = string.Empty;

            testHelper.SetupSequence(x => x.CheckCircuitBroken())
                .Returns(true)
                .Returns(false);

            // Act
            await circuitBreaker.AttemptOperation(testHelper.Object.DoSomething, testHelper.Object.CheckCircuitBroken, defaultReturn);
            await Task.Delay(TimeSpan.FromSeconds(1));
            await circuitBreaker.AttemptOperation(testHelper.Object.DoSomething, testHelper.Object.CheckCircuitBroken, defaultReturn);

            // Assert
            testHelper.Verify(x => x.DoSomething(), Times.Once);
        }

        public interface ITestHelper
        {
            public bool CheckCircuitBroken();
            public Task<string> DoSomething();            
        }
    }
}
