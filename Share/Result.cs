
using Share.Interfaces;

namespace Share
{

    public class Result<T> : IResult<T>
    {
        public string Message { get; set; }

        public bool Succeeded { get; set; }

        public T Data { get; set; }

        public int Code { get; set; }

        #region Non Async Methods

        #region Success Methods

        public static Result<T> Success()
        {
            return new Result<T>
            {
                Succeeded = true,
                Code = 200
            };
        }

        public static Result<T> Success(string message)
        {
            return new Result<T>
            {
                Succeeded = true,
                Message = message,
                Code = 200
            };
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>
            {
                Succeeded = true,
                Data = data,
                Code = 200
            };
        }

        public static Result<T> Success(T data, string message)
        {
            return new Result<T>
            {
                Succeeded = true,
                Message = message,
                Data = data,
                Code = 200
            };
        }

        #endregion

        #region Failure Methods

        public static Result<T> Failure()
        {
            return new Result<T>
            {
                Succeeded = false
            };
        }

        public static Result<T> Failure(string message)
        {
            return new Result<T>
            {
                Succeeded = false,
                Message = message
            };
        }

        public static Result<T> Failure(T data)
        {
            return new Result<T>
            {
                Succeeded = false,
                Data = data
            };
        }

        public static Result<T> Failure(T data, string message)
        {
            return new Result<T>
            {
                Succeeded = false,
                Message = message,
                Data = data
            };
        }

        public static Result<T> Failure(Exception exception)
        {
            return new Result<T>
            {
                Succeeded = false
                // Exception = exception 
            };
        }

        #endregion

        #endregion

        #region Async Methods

        #region Success Methods

        public static Task<Result<T>> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<Result<T>> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        public static Task<Result<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        public static Task<Result<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success(data, message));
        }

        #endregion

        #region Failure Methods

        public static Task<Result<T>> FailureAsync()
        {
            return Task.FromResult(Failure());
        }

        public static Task<Result<T>> FailureAsync(string message)
        {
            return Task.FromResult(Failure(message));
        }

        public static Task<Result<T>> FailureAsync(T data)
        {
            return Task.FromResult(Failure(data));
        }

        public static Task<Result<T>> FailureAsync(T data, string message)
        {
            return Task.FromResult(Failure(data, message));
        }


        public static Task<Result<T>> FailureAsync(Exception exception)
        {
            return Task.FromResult(Failure(exception));
        }

        #endregion

        #endregion
    }
}