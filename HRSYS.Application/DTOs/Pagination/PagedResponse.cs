using Microsoft.EntityFrameworkCore;

namespace HRSYS.Application.DTOs.Pagination
{
    public class PagedResponse<T>
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PagedResponse(IEnumerable<T> data, int totalCount, int pageNumber, int pageSize)
        {
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
        }

        static public async Task<PagedResponse<T>> test(IQueryable<T> ali, int pageNumber, int pageSize)
        {
            var count = await ali.CountAsync();

            var pagintion = await ali.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync()
                ;

            return new PagedResponse<T>(ali.Take(pageSize), count, pageNumber, pageSize);
        }
    }
}