using System.Linq.Expressions;
using Common.Models;

namespace Common.Interfaces
{
    // todo: findout what in means in c#?

    public interface IBaseRepository<TModelBase, in TFilter> 
        where TModelBase : ModelBase
        where TFilter : struct, IListFilter
    {
        #region AsyncMethods

        Task<TModelBase?> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<TModelBase?> GetWithoutIncludeAsync(Guid id, CancellationToken cancellationToken);

        Task<TModelBase?> GetAsync(uint seqId, CancellationToken cancellationToken);
        Task<TModelBase?> GetWithoutIncludeAsync(uint seqId, CancellationToken cancellationToken);

        Task<PaginatedResult<TModelBase>> GetListAsync(TFilter filter, CancellationToken cancellationToken);
        Task CreateAsync(TModelBase model, CancellationToken cancellationToken);
        Task CreateRangeAsync(List<TModelBase> models, CancellationToken cancellationToken);
        Task UpdateAsync(TModelBase model, CancellationToken cancellationToken);
        Task UpdateRangeAsync(List<TModelBase> models, CancellationToken cancellationToken);
        Task DeleteAsync(TModelBase model, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seqId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(uint seqId, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteRangeAsync(List<TModelBase> models, CancellationToken cancellationToken);
        
        Task<bool> HasAnyAsync(Expression<Func<TModelBase, bool>> predicate, CancellationToken cancellationToken);

        #endregion

    }
}
