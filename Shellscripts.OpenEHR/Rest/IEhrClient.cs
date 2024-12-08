namespace Shellscripts.OpenEHR.Rest
{
    public interface IEhrClient
    {
        Task<TResult> ExecuteAsync<TResult>(Func<HttpClient, CancellationToken, Task<TResult>> action, CancellationToken cancellationToken = default);
    }
}