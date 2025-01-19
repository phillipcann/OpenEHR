namespace Shellscripts.OpenEHR.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Rest;

    public class EhrRepository
        : BaseRepository<Ehr>
    {
        public EhrRepository(IEhrClient client) 
            : base(client) { }

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

            return await Client.GetAsync<Ehr>(url, token ?? CancellationToken.None);
        }
    }
}
