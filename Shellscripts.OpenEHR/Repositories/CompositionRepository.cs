namespace Shellscripts.OpenEHR.Repositories
{
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Rest;

    public class CompositionRepository
        : BaseRepository<Composition>
    {
        public CompositionRepository(IEhrClient client, JsonSerializerOptions options) 
            : base(client, options) { }

        public override async Task<Composition?> GetSingleAsync(IDictionary<string, string> @params, CancellationToken? token)
        {
            if (!@params.TryGetValue("ehrId", out string? ehrId))
                throw new ArgumentException("Missing ehrId parameter");

            if (!@params.TryGetValue("compositionId", out string? compositionId))
                throw new ArgumentException("Missing compositionId parameter");

            string url = $"/ehr/{ehrId}/composition/{compositionId}";
            return await Client.GetAsync<Composition>(url, token ?? CancellationToken.None);
        }
    }
}