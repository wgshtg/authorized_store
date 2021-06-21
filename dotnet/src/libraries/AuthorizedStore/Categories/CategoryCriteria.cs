namespace AuthorizedStore
{
    public class CategoryCriteria
    {
        public string Name { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        /// <summary>
        /// Internal usage.
        /// </summary>
        public int? NotId { get; set; }

        /// <summary>
        /// Internal usage.
        /// </summary>
        public string FullName { get; set; }
    }
}
