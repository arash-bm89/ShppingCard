using System.Linq.Expressions;
using Common.ExtensionMethods;
using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Implementations;

public abstract class BaseRepository<TModelBase, TFilter> : IBaseRepository<TModelBase, TFilter>
    where TModelBase : ModelBase
    where TFilter : struct, IListFilter
{
    #region Ctor

    public BaseRepository(DbContext db)
    {
        _db = db;
        _dbSet = db.Set<TModelBase>();
    }

    #endregion

    // todo: implement setupPaging


    #region Properties

    protected DbContext _db { get; set; }
    protected DbSet<TModelBase> _dbSet { get; set; }

    #endregion

    #region AbstractMethods

    protected abstract IQueryable<TModelBase> ConfigureInclude(IQueryable<TModelBase> query);
    protected abstract IQueryable<TModelBase> ConfigureListInclude(IQueryable<TModelBase> query);
    protected abstract IQueryable<TModelBase> ApplyFilter(IQueryable<TModelBase> query, TFilter filter);

    #endregion

    #region InterfaceImplementations

    public virtual async Task<TModelBase?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet
            .Apply(ConfigureInclude)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public virtual async Task<TModelBase?> GetAsync(uint seqId, CancellationToken cancellationToken)
    {
        return await _dbSet
            .Apply(ConfigureInclude)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.SeqId == seqId, cancellationToken);
    }

    public virtual async Task<TModelBase?> GetWithoutIncludeAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public virtual async Task<TModelBase?> GetWithoutIncludeAsync(uint seqId, CancellationToken cancellationToken)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.SeqId == seqId, cancellationToken);
    }

    public async Task<PaginatedResult<TModelBase>> GetListAsync(TFilter filter, CancellationToken cancellationToken)
    {
        var query = _dbSet.AsQueryable();

        query = ApplyFilter(query, filter);

        var paginatedResult = new PaginatedResult<TModelBase>
        {
            Items = await query
                .AsNoTracking()
                .Apply(ConfigureListInclude)
                .Apply(DefaultSortFunc)
                .Skip(filter.Offset)
                .Take(filter.Count)
                .ToListAsync(cancellationToken),

            TotalCount = await query
                .CountAsync(cancellationToken)
        };

        return paginatedResult;
    }

    public virtual async Task<List<TModelBase>?> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.Apply(ConfigureInclude)
            .AsNoTracking().ToListAsync(cancellationToken);
    }


    public virtual async Task CreateAsync(TModelBase model, CancellationToken cancellationToken)
    {
        if (model.Id == default) model.Id = Guid.NewGuid();

        await _dbSet.AddAsync(model, cancellationToken);

        await SaveChangesAsync(cancellationToken);
    }

    public virtual async Task CreateRangeAsync(List<TModelBase> models, CancellationToken cancellationToken)
    {
        if (!models.Any())
            throw new InvalidOperationException("no objects to add range.");

        foreach (var modelBase in models)
            if (modelBase.Id == default)
                modelBase.Id = Guid.NewGuid();

        await _dbSet.AddRangeAsync(models, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public virtual async Task UpdateAsync(TModelBase model, CancellationToken cancellationToken)
    {
        PerformUpdateWithoutSaving(model);
        await SaveChangesAsync(cancellationToken);
    }

    public virtual async Task UpdateRangeAsync(List<TModelBase> models, CancellationToken cancellationToken)
    {
        if (!models.Any())
            throw new InvalidOperationException("no objects to add range.");

        foreach (var modelBase in models) PerformUpdateWithoutSaving(modelBase);

        await SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync(TModelBase model, CancellationToken cancellationToken)
    {
        PerformDeleteWithoutSaving(model);
        await SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var model = await GetAsync(id, cancellationToken);

        if (model == null)
            throw new Exception($"Entity not found for delete. entity: {typeof(TModelBase)} by id: {id}");

        await DeleteAsync(model, cancellationToken);
    }

    public virtual async Task DeleteAsync(uint seqId, CancellationToken cancellationToken)
    {
        var model = await GetAsync(seqId, cancellationToken);

        if (model == null)
            throw new Exception($"Entity not found for delete. entity: {typeof(TModelBase)} by seqId: {seqId}");

        await DeleteAsync(model, cancellationToken);
    }

    public virtual async Task DeleteRangeAsync(List<TModelBase> models, CancellationToken cancellationToken)
    {
        foreach (var modelBase in models) PerformDeleteWithoutSaving(modelBase);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> HasAnyAsync(Expression<Func<TModelBase, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().AnyAsync(predicate, cancellationToken);
    }

    #endregion

    #region Protecteds

    protected virtual void PerformDeleteWithoutSaving(TModelBase modelBase)
    {
        modelBase.IsDeleted = true;
        modelBase.DeletedAt = DateTimeOffset.UtcNow;

        var efEntry = _db.Entry(modelBase);

        if (efEntry.State == EntityState.Detached) _db.Attach(modelBase);

        efEntry.Property(x => x.IsDeleted).IsModified = true;
        efEntry.Property(x => x.DeletedAt).IsModified = true;
    }

    protected virtual void PerformUpdateWithoutSaving(TModelBase modelBase)
    {
        modelBase.ModifiedAt = DateTimeOffset.UtcNow;
        var efEntry = _dbSet.Entry(modelBase);

        if (efEntry.State == EntityState.Detached) _db.Attach(modelBase);

        _dbSet.Update(modelBase);

        efEntry.Property(x => x.SeqId).IsModified = false;
        efEntry.Property(x => x.CreatedAt).IsModified = false;
    }

    protected virtual async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }

    protected virtual IQueryable<TModelBase> DefaultSortFunc(IQueryable<TModelBase> query)
    {
        query = query.OrderByDescending(x => x.SeqId);
        return query;
    }

    #endregion
}