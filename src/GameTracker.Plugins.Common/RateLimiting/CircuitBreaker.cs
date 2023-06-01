namespace GameTracker.Plugins.Common.RateLimiting
{
    internal class CircuitBreaker<TResult>
    {
        private TimeSpan _backOff;
        private CircuitBreakerState _state;
        private DateTime _updatedAt;

        public CircuitBreaker(TimeSpan backOff)
        {
            _backOff = backOff;
            _updatedAt = DateTime.MaxValue;
            _state = CircuitBreakerState.Closed;
        }

        internal async Task<TResult> AttemptOperation(Func<Task<TResult>> operation, Func<bool> circuitBrokenCheck, TResult defaultValue)
        {
            try
            {
                if (_state != CircuitBreakerState.Open || (DateTime.Now - _updatedAt) >= _backOff)
                {
                    _state = circuitBrokenCheck() ? CircuitBreakerState.Open : CircuitBreakerState.Closed;
                    _updatedAt = DateTime.Now;
                }

                return _state == CircuitBreakerState.Closed
                    ? await operation().ConfigureAwait(false)
                    : defaultValue;
            }
            catch (Exception)
            {
                _state = CircuitBreakerState.Open;
                _updatedAt = DateTime.Now;
                return defaultValue;
            }
        }
    }
}