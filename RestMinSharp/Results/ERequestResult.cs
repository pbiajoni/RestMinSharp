namespace RestMinSharp.Results
{
    public class ERequestResult<T, E>: RequestResult<T>
    {
        public bool HasError { get => (Error is not null); }
        public E Error { get; set; }
    }
}
