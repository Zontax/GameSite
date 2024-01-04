namespace GameSite.Data;

public class Pager
{
    public int TotalItems { get; private set; }
    public int CurrentPage { get; private set; }
    public int PageSize { get; private set; }

    public int TotalPages { get; private set; }
    public int StartPage { get; private set; }
    public int EndPage { get; private set; }
    public int RecSkip { get; private set; }

    public string Controller { get; private set; } = string.Empty;
    public string Action { get; private set; } = string.Empty;
    public string? Param1 { get; private set; }
    public string? Param2 { get; private set; }

    public Pager()
    {
    }

    public Pager(int totalItems, int? page, int pageSize = 7, string action = "Index", string controller = "Home", string? param1 = null, string? param2 = null)
    {
        int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
        if (totalPages == 0) totalPages = 1;
        int currentPage = page ?? 1;

        int startPage = currentPage - 5;
        int endPage = currentPage + 4;

        if (startPage <= 0)
        {
            endPage = endPage - (startPage - 1);
            startPage = 1;
        }

        if (endPage > totalPages)
        {
            endPage = totalPages;
            if (endPage > 10) startPage = endPage - 9;
        }

        TotalItems = totalItems;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = totalPages;
        StartPage = startPage;
        EndPage = endPage;
        Controller = controller;
        Action = action;
        Param1 = param1;
        Param2 = param2;
        RecSkip = (currentPage - 1) * pageSize;
    }
}

public static class PagerExtensions
{
    public static List<T> ToPagedList<T>(this IEnumerable<T> source, Pager pager)
    {
        return source.Skip(pager.RecSkip).Take(pager.PageSize).ToList();
    }
}