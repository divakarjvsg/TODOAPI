namespace ToDoApi.Database.Models
{
    public class PageParmeters
    {
        const int maxPageSixe = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSixe) ? maxPageSixe : value;
            }
        }
    }
}
