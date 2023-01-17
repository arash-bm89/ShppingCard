using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace Common.Interfaces
{
    // todo: findout what in means in c#?
    // todo: why tfilter is a struct?

    public interface IRepository<TModel, in TFilter> 
        where TModel : ModelBase
        where TFilter : struct, IListFilter
    {
        #region AsyncMethods

        Task<TModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TModel?> GetByIdWithoutIncludeAsync(Guid id, CancellationToken cancellationToken);

        Task<TModel?> GetBySeqIdAsync(uint seqId, CancellationToken cancellationToken);
        Task<TModel?> GetBySeqIdWithoutIncludeAsync(uint seqId, CancellationToken cancellationToken);

        Task<PaginatedResult<TModel>> GetListAsync(TFilter filter, CancellationToken cancellationToken);
        Task<List<TModel>?> GetAllAsync(CancellationToken cancellationToken);
        Task<List<TModel>?> GetByFilterAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken);
        Task CreateAsync(TModel model, CancellationToken cancellationToken);
        Task CreateRangeAsync(List<TModel> models, CancellationToken cancellationToken);
        Task UpdateAsync(TModel model, CancellationToken cancellationToken);
        Task UpdateRangeAsync(List<TModel> models, CancellationToken cancellationToken);
        Task DeleteAsync(TModel model, CancellationToken cancellationToken);
        Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
        Task DeleteBySeqIdAsync(uint seqId, CancellationToken cancellationToken);
        Task DeleteRangeAsync(List<TModel> models, CancellationToken cancellationToken);
        

        #endregion

    }
}
