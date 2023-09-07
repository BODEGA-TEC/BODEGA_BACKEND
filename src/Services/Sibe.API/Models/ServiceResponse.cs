namespace Sibe.API.Models
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }

        public bool Success { get; set; } = true;

        public string Message { get; set; } = string.Empty;

        public void SetError(string errorMessage)
        {
            Success = false;
            Message = errorMessage;
        }
    }
}