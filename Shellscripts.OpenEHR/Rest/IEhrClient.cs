using Shellscripts.OpenEHR.Models.Ehr;

namespace Shellscripts.OpenEHR.Rest
{
    public interface IEhrClient : IEhrServiceModel
    {
        Task<TResult> ExecuteAsync<TResult>(Func<HttpClient, CancellationToken, Task<TResult>> action, CancellationToken cancellationToken = default);


        Task<TResult?> GetAsync<TResult>(string url, CancellationToken cancellationToken);
        Task<string?> PostAsync<TModelObject>(string url, TModelObject data, CancellationToken cancellationToken);
        Task<string?> PostAsync(string url, string data, CancellationToken cancellationToken);

    }

    public interface IEhrServiceModel
    {
        Task<Ehr?> GetEhrAsync(string ehrId, CancellationToken cancellationToken);
        Task<VersionedEhrStatus?> GetVersionedEhrStatusAsync(string ehrId, CancellationToken cancellationToken);
        Task<Composition?> GetCompositionAsync(string ehrId, string compositionId, CancellationToken cancellationToken);
        Task<VersionedComposition?> GetVersionedCompositionAsync(string ehrId, string compositionId, CancellationToken cancellationToken);
        Task<IEnumerable<TR>> PostQueryAqlAsync<TR>(object body, CancellationToken cancellationToken);
    }
}