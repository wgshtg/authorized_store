using System;

namespace AuthorizedStore
{
    public class StoreCriteria
    {
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public DateTime? ContractStartDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
