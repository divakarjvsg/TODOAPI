using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.Models
{
    public class PageParmeters
    {
        const int maxPageSixe = 50;
        public int pageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int pageSize {
            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSixe) ? maxPageSixe : value;
            } 
        }

    }
}
