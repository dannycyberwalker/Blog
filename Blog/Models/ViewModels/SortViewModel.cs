
namespace Blog.Models.ViewModels
{
    public class SortViewModel
    {
        public SortState DateSort { get; private set; }
        public SortState NumberOfCommentsSort { get; private set; } 
        public SortState Current { get; private set; }

        public SortViewModel(SortState sortOrder)
        {
            if (sortOrder == SortState.DateAsc)
                DateSort = SortState.DateDesc;
            else
                DateSort = SortState.DateAsc;

            if (sortOrder == SortState.NumberOfCommentsAsc)
                NumberOfCommentsSort = SortState.NumberOfCommentsDesc;
            else
                NumberOfCommentsSort = SortState.NumberOfCommentsAsc;

            Current = sortOrder;
        }
    }
}
