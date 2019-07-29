using System;
using System.Collections.Generic;
using System.Linq;

using Disunity.Store.Exceptions;

using Newtonsoft.Json.Schema;

using AggregateException = Disunity.Store.Exceptions.AggregateException;


namespace Disunity.Store.Extensions {

    public static class ValidationErrorExtensions {

        public static SchemaException ToSchemaException(this ValidationError error) {
            return SchemaException.FromValidationError(error);
        }

        public static IEnumerable<SchemaException> ToSchemaExceptions(this IList<ValidationError> errors) {
            return errors.Select(e => e.ToSchemaException());
        }

        public static AggregateException AsAggregate(this IList<ValidationError> errors) {
            return new AggregateException(errors.ToSchemaExceptions());
        }

    }

}