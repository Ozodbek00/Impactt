namespace Impactt.Service.Exceptions
{
    public sealed class ImpacttException : Exception
    {
        public int Code { get; set; }

        public ImpacttException(int code, string message) : base(message)
        {
            this.Code = code;
        }
    }
}
