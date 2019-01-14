using System;

namespace travoul.Models.ViewModels.PaginationModels
{
    public class Pager
    {
        public Pager(int totalItems, int? page, int pageSize = 9)
        {
            CurrentPage = page ?? 1;
            TotalItems = totalItems;
            PageSize = pageSize;
            StartPage = CurrentPage - 5;
            EndPage = CurrentPage + 4;

        }

        public void adjustPages()
        {
            if (StartPage <= 0)
            {
                EndPage -= (StartPage - 1);
                StartPage = 1;
            }
            if (EndPage > TotalPages)
            {
                EndPage = TotalPages;
                if (EndPage > 10)
                {
                    StartPage = EndPage - 9;
                }
            }
        }


        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int) Math.Ceiling(TotalItems / (decimal)PageSize);
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }

    }

}