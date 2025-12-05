namespace BasicNtierTemplate.Service.Enums
{
    public class CurrentSort
    {
        public CurrentSort()
        {
            SortParameter = SortParameter.None;
            SortOrder = SortOrder.Ascending;
        }

        public CurrentSort(
            SortParameter sortParameter,
            SortOrder sortOrder)
        {
            SortParameter = sortParameter;
            SortOrder = sortOrder;
        }

        public SortParameter SortParameter { get; set; }
        public SortOrder SortOrder { get; set; }
    }

    public enum SortOrder
    {
        Ascending = 1,
        Descending = 2
    }

    public enum SortParameter
    {
        None = 0,
        Name = 1,
        Surname = 2,
        Username = 3,
        Date = 4,
    }

}
