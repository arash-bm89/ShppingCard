using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Common.ExtensionMethods;
using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Implementations
{
    public abstract class Repository<TModelBase, TFilter>: IRepository<TModelBase, TFilter> 
        where TModelBase : ModelBase
        where TFilter : struct, IListFilter
    {
        // for now repository implemented using eager loading

        // todo: ask about eager one and implement the class using this pattern
        // todo: ask about filter & pagination
        // todo: see the pagination section of the course

        // todo: is it better to handle notfound element in db inside the application or inside the stack?

        // todo: differences of configureInclude and ConfigureListInclude


        #region Properties

        protected DbContext _db { get; set; }
        protected DbSet<TModelBase> _dbSet { get; set; }

        #endregion

        #region Ctor

        public Repository(DbContext db)
        {
            _db = db;
            _dbSet = db.Set<TModelBase>();
        }

        #endregion

        #region AbstractMethods

        protected abstract IQueryable<TModelBase> ConfigureInclude(IQueryable<TModelBase> query);
        protected abstract IQueryable<TModelBase> ConfigureListInclude(IQueryable<TModelBase> query);
        protected abstract IQueryable<TModelBase> ApplyFilter(IQueryable<TModelBase> query, TFilter filter);

        #endregion

        #region InterfaceImplementations

        public virtual async Task<TModelBase?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            // todo: ask what the hell is this?!
            return await _dbSet.Apply<TModelBase, TModelBase>(new Func<IQueryable<TModelBase>, IQueryable<TModelBase>>(this.ConfigureInclude))
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public virtual async Task<TModelBase?> GetBySeqIdAsync(uint seqId, CancellationToken cancellationToken)
        {
            return await _dbSet.Apply<TModelBase, TModelBase>(new Func<IQueryable<TModelBase>, IQueryable<TModelBase>>(this.ConfigureInclude))
                .AsNoTracking().FirstOrDefaultAsync(x => x.SeqId == seqId, cancellationToken);
        }

        public virtual async Task<TModelBase?> GetByIdWithoutIncludeAsync(Guid id, CancellationToken cancellationToken)
        {
            // todo: ask what the hell is this?!
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public virtual async Task<TModelBase?> GetBySeqIdWithoutIncludeAsync(uint seqId, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.SeqId == seqId, cancellationToken);
        }

        public async Task<PaginatedResult<TModelBase>> GetListAsync(TFilter filter, CancellationToken cancellationToken)
        {
            var query = _dbSet.AsQueryable();
            query = this.ApplyFilter(query, filter);

            PaginatedResult<TModelBase> paginatedResult = new PaginatedResult<TModelBase>();
            paginatedResult.Items = await _dbSet
                .AsNoTracking()
                .Apply(this.ConfigureListInclude)
                .Skip(filter.Offset)
                .Take(filter.Count)
                .ToListAsync(cancellationToken);
            paginatedResult.TotalCount = await _dbSet.CountAsync(cancellationToken);
            
            return paginatedResult;
        }


        public virtual async Task<List<TModelBase>?> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.Apply<TModelBase, TModelBase>(new Func<IQueryable<TModelBase>, IQueryable<TModelBase>>(this.ConfigureInclude))
                .AsNoTracking().ToListAsync(cancellationToken);
        }


        [Obsolete]
        public virtual async Task<List<TModelBase>?> GetByFilterAsync(Expression<Func<TModelBase, bool>> filter, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().Where(filter).ToListAsync(cancellationToken);
        }

        public virtual async Task CreateAsync(TModelBase model, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(model, cancellationToken);
            await SaveChangesAsync(cancellationToken);
        }

        public virtual async Task CreateRangeAsync(List<TModelBase> models, CancellationToken cancellationToken)
        {
            await _dbSet.AddRangeAsync(models, cancellationToken);
            await SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(TModelBase model, CancellationToken cancellationToken)
        {
            model.ModifiedAt = DateTimeOffset.Now;
            _dbSet.Update(model);
            await SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateRangeAsync(List<TModelBase> models, CancellationToken cancellationToken)
        {
            foreach (TModelBase modelBase in models)
            {
                modelBase.ModifiedAt = DateTimeOffset.Now;
            }
            _dbSet.UpdateRange(models);
            await SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(TModelBase model, CancellationToken cancellationToken)
        {
            PerformDeleteWithoutSaving(model);
            await SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var model = await GetByIdAsync(id, cancellationToken);
            await DeleteAsync(model, cancellationToken);
        }

        public virtual async Task DeleteBySeqIdAsync(uint seqId, CancellationToken cancellationToken)
        {
            var model = await GetBySeqIdAsync(seqId, cancellationToken);
            await DeleteAsync(model, cancellationToken);
        }

        public virtual async Task DeleteRangeAsync(List<TModelBase> models, CancellationToken cancellationToken)
        {
            foreach (TModelBase modelBase in models)
            {
                PerformDeleteWithoutSaving(modelBase);
            }
            await SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region Protecteds

        protected virtual void PerformDeleteWithoutSaving(TModelBase modelBase)
        {
            modelBase.IsDeleted = true;
            modelBase.DeletedAt = DateTimeOffset.UtcNow;

            var efEntry = _db.Entry(modelBase);

            if (efEntry.State == EntityState.Detached)
            {
                _db.Attach(modelBase);
            }

            efEntry.Property(x => x.IsDeleted).IsModified = true;
            efEntry.Property(x => x.DeletedAt).IsModified = true;
        }

        protected virtual async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _db.SaveChangesAsync(cancellationToken);
        }

        #endregion

    }
}
