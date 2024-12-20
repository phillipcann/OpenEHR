using Shellscripts.OpenEHR.Models.Ehr;

namespace Shellscripts.OpenEHR.Rest
{
    public interface IEhrClient
    {
        Task<TResult> ExecuteAsync<TResult>(Func<HttpClient, CancellationToken, Task<TResult>> action, CancellationToken cancellationToken = default);


        Task<Ehr?> GetEhrAsync(string ehrId, CancellationToken cancellationToken);
        Task<VersionedEhrStatus?> GetVersionedEhrStatusAsync(string ehrId, CancellationToken cancellationToken);
        Task<Composition?> GetCompositionAsync(string ehrId, string compositionId, CancellationToken cancellationToken);
        Task<VersionedComposition?> GetVersionedCompositionAsync(string ehrId, string compositionId, CancellationToken cancellationToken);

    }
}