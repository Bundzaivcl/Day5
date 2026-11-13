
namespace Day3.Models
{
    public class ProductSearchModel
    {
        public string? Keyword { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 2;
    }
}
