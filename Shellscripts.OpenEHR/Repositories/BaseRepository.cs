namespace Shellscripts.OpenEHR.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Shellscripts.OpenEHR.Rest;

    public abstract class BaseRepository<T> : IRepository<T>
    {
        internal readonly IEhrClient Client;
        internal readonly JsonSerializerOptions SerialiserOptions;

        internal BaseRepository(IEhrClient client, JsonSerializerOptions serialiserOptions)
        {
            Client = client;
            SerialiserOptions = serialiserOptions;
        }

        #region IRepository Implementation

        public virtual async Task<IEnumerable<T>> GetCollectionAsync(IDictionary<string, string> @params, CancellationToken? token)
        {
            await Task.Run(() => { });
            throw new NotImplementedException();
        }

        public virtual async Task<T?> GetSingleAsync(IDictionary<string, string> @params, CancellationToken? token)
        {
            await Task.Run(() => { });
            throw new NotImplementedException();
        }

        public virtual async Task<string?> UpsertAsync(T data, CancellationToken? token)
        {
            await Task.Run(() => { });
            throw new NotImplementedException();
        }

        public virtual async Task<bool> DeleteAsync(IDictionary<string, string> @params, CancellationToken? token)
        {
            await Task.Run(() => { });
            throw new NotImplementedException();
        }

        #endregion

    }
}
