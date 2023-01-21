namespace Common.Models;

public class PaginatedResult<TModelBase>
    where TModelBase : ModelBase
{
    public List<TModelBase>? Items { get; set; }
    public int TotalCount { get; set; }

    public bool HasAnyItems()
    {
        return Items != null && Items.Count != 0;
    }
}