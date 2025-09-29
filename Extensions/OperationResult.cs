namespace Automotive_Project.Extensions
{
    public class OperationResult<T>
    {
        public bool Succeeded { get; private set; }
        public List<string> Errors { get; private set; } = new();
        public T? Value { get; private set; }

        private OperationResult(bool succeeded, T? value = default, IEnumerable<string>? errors = null)
        {
            Succeeded = succeeded;
            Value = value;
            if (errors != null)
                Errors.AddRange(errors);
        }

        public static OperationResult<T> Success(T value)
            => new OperationResult<T>(true, value);

        public static OperationResult<T> Failed(params string[] errors)
            => new OperationResult<T>(false, default, errors);
    }

}
