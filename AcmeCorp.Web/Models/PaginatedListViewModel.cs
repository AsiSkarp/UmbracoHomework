namespace AcmeCorp.Web.Models
{
    public class PaginatedListViewModel
    {
        public IEnumerable<ListEntryViewModel> Entries { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }

        public int TotalPages => (int)Math.Ceiling((decimal)TotalRecords / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
