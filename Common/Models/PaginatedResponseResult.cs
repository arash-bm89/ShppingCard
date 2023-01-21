namespace Common.Models;

public class PaginatedResponseResult<TModel>
    where TModel : class
{
    public List<TModel>? Items { get; set; }
    public int TotalCount { get; set; }

    public bool HasAnyItems()
    {
        return Items != null && Items.Count != 0;
    }
}