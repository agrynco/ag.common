using System.Collections.Generic;

namespace Services.Dtos
{
    public class PagedResultDto<TListItemDto>
    {
        public PagedResultDto(long totalCount, IReadOnlyList<TListItemDto> items)
        {
            TotalCount = totalCount;
            Items = items;
        }

        public IReadOnlyList<TListItemDto> Items { get; }

        public long TotalCount { get; }
    }
}