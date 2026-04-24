namespace CNLib.Api.Models
{
    public class ApiSuccess<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class ApiSuccess
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
    }
}
