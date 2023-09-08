namespace Sibe.API.Models
{
    public class ServiceResponse<T>
    {
        private readonly string ErrorPrefix = "[ERROR] ";

        public T? Data { get; set; }

        public bool Success { get; set; } = true;

        public string Message { get; set; } = string.Empty;

        public void SetError(string errorMessage)
        {
            Success = false;
            Message = ErrorPrefix + errorMessage;
        }
        public void SetSuccess(string successMessage)
        {
            Message = successMessage;
        }
    }
}