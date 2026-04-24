using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNLib.Api.Models
{
    public class ApiError
    {
        public bool Success { get; set; } = false;
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
    }

    public class ApiErrorCodes {
        public const string InvalidUrl = "InvalidUrl";
        public const string DownloadFailed = "DownloadFailed";
        public const string UnsupportedFormat = "UnsupportedFormat";
        public const string InternalError = "InternalError";
    }
}
