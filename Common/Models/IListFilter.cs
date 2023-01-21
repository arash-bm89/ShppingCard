namespace Common.Models;

public interface IListFilter
{
    int Offset { get; set; }
    int Count { get; set; }
}