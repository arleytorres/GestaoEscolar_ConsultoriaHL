namespace BlazorApp1.ContextResponse
{
    public class IEscolaServiceResponse
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }

        public IEscolaServiceResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
