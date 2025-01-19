namespace Shellscripts.OpenEHR.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Shellscripts.OpenEHR.Rest;

    public abstract class BaseRepository<T> : IRepository<T>
    {
        internal readonly IEhrClient Client;

        internal BaseRepository(IEhrClient client)
        {
            Client = client;
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

        public virtual async Task<string> UpdateAsync(T data, CancellationToken? token)
        {
            await Task.Run(() => { });
            throw new NotImplementedException();
        }

        public virtual async Task<string> CreateAsync(T data, CancellationToken? token)
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
