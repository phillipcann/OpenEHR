namespace Shellscripts.OpenEHR.Repositories
{
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Rest;

    public class VersionedEhrStatusRepository
        : BaseRepository<VersionedEhrStatus>
    {
        public VersionedEhrStatusRepository(IEhrClient client, JsonSerializerOptions options)
            : base(client, options) { }

        public override async Task<VersionedEhrStatus?> GetSingleAsync(IDictionary<string, string> @params, CancellationToken? token)
        {
            if (!@params.TryGetValue("ehrId", out string? ehrId))
                throw new ArgumentException("Missing ehrId parameter");

            string url = $"/ehr/{ehrId}/versioned_ehr_status";
            return await Client.GetAsync<VersionedEhrStatus>(url, token ?? CancellationToken.None);
        }
    }
}