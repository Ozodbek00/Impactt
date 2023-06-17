namespace Impactt.Service.Exceptions
{
    public class ImpacttException : Exception
    {
        public int Code { get; set; }

        public ImpacttException(int code, string message) : base(message)
        {
            this.Code = code;
        }
    }
}
