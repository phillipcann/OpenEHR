namespace Shellscripts.OpenEHR.Repositories
{
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Rest;

    public class EhrRepository
        : BaseRepository<Ehr>
    {
        public EhrRepository(IEhrClient client, JsonSerializerOptions options) 
            : base(client, options) { }

        public override async Task<Ehr?> GetSingleAsync(IDictionary<string, string> @params, CancellationToken? token)
        {
            var ehrId_exists = @params.TryGetValue("ehr_id", out string? ehr_id);
            var ehrSubjectId_exists = @params.TryGetValue("subject_id", out string? subject_id);
            var ehrSubjectNamespace_exists = @params.TryGetValue("subject_namespace", out string? subject_namespace);

            string url = $"/ehr";

            if (ehrId_exists)
                url += $"/{ehr_id}";
            else if (ehrSubjectId_exists && ehrSubjectNamespace_exists)
                url += $"?subject_id={subject_id}&subject_namespace={subject_namespace}";
            else
                throw new ArgumentException("Missing valid parameters to load Ehr Record");

            return await Client.GetAsync<Ehr>(url, token);
        }

        public override async Task<string?> UpsertAsync(Ehr data, CancellationToken? token)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            bool isUpdate = !string.IsNullOrWhiteSpace(data.EhrId?.Value);

            string url = $"/ehr";

            if (isUpdate)
            {
                url += $"/{data.EhrId?.Value}";
                return await Client.PutAsync<Ehr>(url, data, token);
            }
            else
            {
                return await Client.PostAsync<Ehr>(url, data, token);
            }            
        }
    }
}
