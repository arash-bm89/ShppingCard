using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace Common.Interfaces
{
    public interface IRepository<TModel> where TModel : ModelBase
    {
        #region AsyncMethods

        Task<TModel> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TModel> GetBySeqIdAsync(uint seqId, CancellationToken cancellationToken);
        Task<List<TModel>> GetAllAsync(CancellationToken cancellationToken);
        Task<List<TModel>> GetByFilterAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken);
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
