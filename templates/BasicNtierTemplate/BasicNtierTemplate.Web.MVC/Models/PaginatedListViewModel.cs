using BasicNtierTemplate.Service.Enums;
using BasicNtierTemplate.Service.Models;

namespace BasicNtierTemplate.Web.MVC.Models
{
    public class PaginatedListViewModel<T> where T : class
    {
        public PaginatedList<T> PaginatedList { get; }
        public string? CurrentFilter { get; }
        public int PageSize { get; }
        public CurrentSort CurrentSortCombination { get; }
        public CurrentSort SortColumnOne { get; }
        public CurrentSort SortColumnTwo { get; }

        public PaginatedListViewModel(
            PaginatedList<T> paginatedList,
            string? currentFilter,
            int pageSize,
            CurrentSort currentSortCombination,
            CurrentSort sortColumnOne,
            CurrentSort sortColumnTwo
        )
        {
            PaginatedList = paginatedList;
            CurrentFilter = currentFilter;
            PageSize = pageSize;
            CurrentSortCombination = currentSortCombination;
            SortColumnOne = sortColumnOne;
            SortColumnTwo = sortColumnTwo;
        }
    }
}
