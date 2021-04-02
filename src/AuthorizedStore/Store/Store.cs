using System;

namespace AuthorizedStore
{
    public class Store
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Website { get; set; }

        public string ContractContent { get; set; }

        public DateTime ContractStartDate { get; set; }

        public DateTime ContractEndDate { get; set; }
    }
}
