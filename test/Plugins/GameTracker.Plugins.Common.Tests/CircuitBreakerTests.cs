using GameTracker.Plugins.Common.RateLimiting;

namespace GameTracker.Plugins.Common.Tests
{
    public class CircuitBreakerTests
    {
        [Fact]
        public async Task AttemptOperation_IfCircuitIsClosed_ShouldExecuteOperation()
        {
            // Arrange
            var testHelper = Substitute.For<ITestHelper>();
            var circuitBreaker = new CircuitBreaker<string>(TimeSpan.FromSeconds(5));
            var defaultReturn = string.Empty;
            var expectedReturn = "return";

            testHelper.CheckCircuitBroken()
                .Returns(false);
            testHelper.DoSomething()
                .Returns(expectedReturn);

            // Act
            var result = await circuitBreaker.AttemptOperation(testHelper.DoSomething, testHelper.CheckCircuitBroken, defaultReturn);

            // Assert
            Assert.Equal(expectedReturn, result);
            testHelper.Received(1).CheckCircuitBroken();
            await testHelper.Received(1).DoSomething();
        }

        [Fact]
        public async Task AttemptOperation_IfCircuitBrokenCheckTripped_OperationIsNotExecuted()
        {
            // Arrange
            var testHelper = Substitute.For<ITestHelper>();
            var circuitBreaker = new CircuitBreaker<string>(TimeSpan.FromSeconds(5));
            var defaultReturn = string.Empty;

            testHelper.CheckCircuitBroken()
                .Returns(true);

            // Act
            await circuitBreaker.AttemptOperation(testHelper.DoSomething, testHelper.CheckCircuitBroken, defaultReturn);

            // Assert
            testHelper.Received(1).CheckCircuitBroken();
            await testHelper.Received(0).DoSomething();
        }

        [Fact]
        public async Task AttemptOperation_IfCircuitIsOpen_ReturnsDefaultValue()
        {
            // Arrange
            var testHelper = Substitute.For<ITestHelper>();
            var circuitBreaker = new CircuitBreaker<string>(TimeSpan.FromSeconds(5));
            var defaultReturn = string.Empty;

            testHelper.CheckCircuitBroken()
                .Returns(true);

            // Act
            var result = await circuitBreaker.AttemptOperation(testHelper.DoSomething, testHelper.CheckCircuitBroken, defaultReturn);

            // Assert
            Assert.Equal(defaultReturn, result);
        }

        [Fact]
        public async Task AttemptOperation_IfCircuitIsOpenAndBackoffExpiredAndCircuitBrokenCheckIsFalse_ShouldExecuteOperation()
        {
            // Arrange
            var testHelper = Substitute.For<ITestHelper > ();
            var circuitBreaker = new CircuitBreaker<string>(TimeSpan.FromSeconds(1));
            var defaultReturn = string.Empty;

            testHelper.CheckCircuitBroken()
                .Returns(true, false);

            // Act
            await circuitBreaker.AttemptOperation(testHelper.DoSomething, testHelper.CheckCircuitBroken, defaultReturn);
            await Task.Delay(TimeSpan.FromSeconds(1));
            await circuitBreaker.AttemptOperation(testHelper.DoSomething, testHelper.CheckCircuitBroken, defaultReturn);

            // Assert
            testHelper.Received(2).CheckCircuitBroken();
            await testHelper.Received(1).DoSomething();
        }

        public interface ITestHelper
        {
            public bool CheckCircuitBroken();
            public Task<string> DoSomething();            
        }
    }
}