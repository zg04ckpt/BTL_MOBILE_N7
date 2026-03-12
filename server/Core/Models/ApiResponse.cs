namespace Core.Models
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }

        public static ApiResponse Failure(string message)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                Message = message
            };
        }

        public static ApiResponse Failure(string message, object data)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse Failure()
        {
            return new ApiResponse
            {
                IsSuccess = false,
                Message = null,
                Data = null
            };
        }

        public static ApiResponse Success()
        {
            return new ApiResponse
            {
                IsSuccess = true,
                Message = null,
                Data = null
            };
        }

        public static ApiResponse Success(string message)
        {
            return new ApiResponse
            {
                Data = null,
                Message = message,
                IsSuccess = true
            };
        }

        public static ApiResponse Success(string message, object data)
        {
            return new ApiResponse
            {
                Data = data,
                Message = message,
                IsSuccess = true
            };
        }

        public static ApiResponse Success(object data)
        {
            return new ApiResponse
            {
                Data = data,
                Message = string.Empty,
                IsSuccess = true
            };
        }
    }
}
