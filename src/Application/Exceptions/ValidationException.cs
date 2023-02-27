using FluentValidation.Results;
using System.Runtime.Serialization;

namespace Application.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationException(IReadOnlyCollection<ValidationFailure> failures)
            : this(CreateMessage(failures))
        {
            var failureGroups = failures
                ?.GroupBy(e => e.PropertyName, e => e.ErrorMessage);

            foreach (var failureGroup in failureGroups)
            {
                var propertyName = failureGroup.Key;
                var propertyFailures = failureGroup.ToArray();

                Errors.Add(propertyName, propertyFailures);
            }
        }

        public static string CreateMessage(IEnumerable<ValidationFailure> failures)
        {
            return $"ValidationException: {Environment.NewLine} { string.Join($",{Environment.NewLine}", failures.Select(x => x.PropertyName + ":" + string.Join(", ", x.ErrorMessage))) }";
        }

        public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ValidationException(SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public ValidationException(string[] resultErrors) : base(resultErrors.FirstOrDefault())
        {
            Errors.Add("General", resultErrors);
        }
    }
}
