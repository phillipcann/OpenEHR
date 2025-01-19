namespace Shellscripts.OpenEHR.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepository<T>        
    {
        Task<string> CreateAsync(T data, CancellationToken? token);

        Task<T?> GetSingleAsync(IDictionary<string, string> @params, CancellationToken? token);
        Task<IEnumerable<T>> GetCollectionAsync(IDictionary<string, string> @params, CancellationToken? token);


        Task<string> UpdateAsync(T data, CancellationToken? token);

        Task<bool> DeleteAsync(IDictionary<string, string> @params, CancellationToken? token);
    }
}
