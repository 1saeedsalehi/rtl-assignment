namespace Rtl.MazeScrapper.Domain.Dtos;

public class PagedResponse<T> 
{
    public T Result { get; init; }
        
    public int TotalCount { get; init; }

    public PagedResponse(T result, int totalCount)
    {
        Result = result;
        TotalCount = totalCount;
    }
}
