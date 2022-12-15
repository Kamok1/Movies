namespace Models.General;

public record Filtering()
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string OrderBy { get; set; } = "";
}