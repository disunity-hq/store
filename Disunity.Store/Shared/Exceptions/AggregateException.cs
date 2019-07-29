using System;
using System.Collections;
using System.Collections.Generic;


namespace Disunity.Store.Exceptions {

    public class AggregateException : Exception, IEnumerable<Exception> {

        private readonly IEnumerable<Exception> _innerExceptions;

        public AggregateException(IEnumerable<Exception> innerExceptions) {
            _innerExceptions = innerExceptions;
        }

        public IEnumerator<Exception> GetEnumerator() {
            foreach (var innerException in _innerExceptions) {
                if (innerException is AggregateException innerAggregate) {
                    foreach (var aggregatedException in innerAggregate) {
                        yield return aggregatedException;
                    }
                } else {
                    yield return innerException;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

    }

}