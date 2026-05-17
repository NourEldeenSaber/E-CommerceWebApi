namespace Shared.Results
{
    public class Result
    {
        // Errors [ Code - Description - ErrorType ]
        protected readonly List<Error> _errors = [];
        // IsSuccess
        public bool IsSuccess => _errors.Count() == 0;
        // IsFailure
        public bool Isfailure => !IsSuccess;

        public IReadOnlyList<Error> Errors => _errors;
        // Success
        protected Result()
        {
            
        }
        // Fail With Error
        protected Result(Error error)
        {
            _errors.Add(error);
        }
        // Fail With Errors
        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }

        #region  Static Factory Methods

        public static Result Ok() => new Result();
        public static Result Fail(Error error) => new Result(error);
        public static Result Fail(List<Error> errors) => new Result(errors);

        #endregion

    }

    public class Result<TValue> : Result
    {
        private readonly TValue _value;
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("Cannot Access The Value of Failed Result ");

        // Ok
        private Result(TValue value) 
        {
            _value = value;
        }
        // Fail
        private Result(Error error) : base(error) 
        {
            _value = default;
        }
        private Result(List<Error> errors) : base(errors)
        {
        }

        public static Result<TValue> Ok(TValue value)=>new Result<TValue>(value);
        public static new Result<TValue> Fail(Error error)=>new Result<TValue>(error);
        public static new Result<TValue> Fail(List<Error> errors)=>new Result<TValue>(errors);


        public static implicit operator Result<TValue>(TValue value) => Ok(value);
        public static implicit operator Result<TValue>(Error error) => Fail(error);
        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);

    }
}
