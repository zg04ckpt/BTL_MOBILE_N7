using CNLib.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNLib.Api
{
    public class ApiResponse
    {
        public static ApiSuccess<T> Success<T>(T data, string message = null)
        {
            return new ApiSuccess<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiSuccess Success(string message = null)
        {
            return new ApiSuccess
            {
                Success = true,
                Message = message
            };
        }

        public static ApiError Error(string errorMessage, string errorCode = null)
        {
            return new ApiError
            {
                Success = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }

        public static ApiError InternalServerError(string errorMessage = null)
        {
            return new ApiError
            {
                Success = false,
                ErrorMessage = errorMessage ?? "Lỗi chưa xác định",
                ErrorCode = ApiErrorCodes.InternalError
            };
        }
    }
}
