using BasicNtierTemplate.Service.Models;

namespace BasicNtierTemplate.Web.MVC.Models
{
    public class PaginatedListViewModel<T> where T : class
    {
        public PaginatedList<T> PaginatedList;
        public string CurrentFilter;
        public string CurrentSort;
        public string SortParamOne;
        public string SortParamTwo;
        public int PageSize;

        public PaginatedListViewModel(
            PaginatedList<T> paginatedList,
            string currentFilter,
            string currentSort,
            string sortParamOne,
            string sortParamTwo,
            int pageSize
        )
        {
            this.PaginatedList = paginatedList;
            this.CurrentFilter = currentFilter;
            this.CurrentSort = currentSort;
            this.SortParamOne = sortParamOne;
            this.SortParamTwo = sortParamTwo;
            this.PageSize = pageSize;
        }
    }
}
