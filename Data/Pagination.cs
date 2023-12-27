public class Pagination
{
    public int PageSize;
    public int TotalCount;
    public int TotalPages;
    public int Page = 1;
    public string ActionName;
    public string? TagParametr;
    public string? SearchParametr;

    public Pagination(int count, int? page, int pageSize, string actionName, string? tagParametr = null, string? searchParametr = null)
    {
        int pageNumber = page ?? 1;
        if (count == 0) TotalCount = 1;
        else TotalCount = count;

        if (TotalCount <= 1) TotalPages = 1;
        else TotalPages = (int)Math.Ceiling((double)TotalCount / pageSize);

        Page = pageNumber;
        PageSize = pageSize;
        ActionName = actionName;
        TagParametr = tagParametr;
        SearchParametr = searchParametr;
    }
}
