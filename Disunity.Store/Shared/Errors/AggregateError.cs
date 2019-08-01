using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;


namespace Disunity.Store.Errors {

    public class AggregateError : ApiError, IEnumerable<ApiError> {

        private readonly IEnumerable<ApiError> _innerErrors;

        public AggregateError(IEnumerable<ApiError> innerErrors) : base(null) {
            _innerErrors = innerErrors;
        }

        public IEnumerator<ApiError> GetEnumerator() {
            foreach (var error in _innerErrors) {
                if (error is AggregateError innerAggregate) {
                    foreach (var aggregateError in innerAggregate) {
                        yield return aggregateError;
                    }
                } else {
                    yield return error;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public override ObjectResult ToObjectResult() {
            return new ObjectResult(new { errors = _innerErrors}) {
                StatusCode = (int) StatusCode
            };
        }

    }

}