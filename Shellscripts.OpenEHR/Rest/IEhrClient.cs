namespace Shellscripts.OpenEHR.Rest
{
    public interface IEhrClient
    {
        Task<TResult> ExecuteAsync<TResult>(Func<HttpClient, CancellationToken?, Task<TResult>> action, CancellationToken? cancellationToken = default);
        Task<IEnumerable<TR>> QueryAsync<TR>(string query, CancellationToken? cancellationToken);


        Task<TResult?> GetAsync<TResult>(string url, CancellationToken? cancellationToken);
        Task<string?> PostAsync<TModelObject>(string url, TModelObject data, CancellationToken? cancellationToken);
        Task<string?> PostAsync(string url, string data, CancellationToken? cancellationToken);
        Task<string?> PutAsync<TModelObject>(string url, TModelObject data, CancellationToken? cancellationToken);
        Task<string?> PutAsync(string url, string data, CancellationToken? cancellationToken);
    }
}