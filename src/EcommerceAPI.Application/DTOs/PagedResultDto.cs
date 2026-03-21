namespace ecommerceAPI.src.EcommerceAPI.Application.DTOs
{
     public class PagedResultDto<T>
    {
        public IReadOnlyCollection<T> Items { get; set; } =  Array.Empty<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    }
}