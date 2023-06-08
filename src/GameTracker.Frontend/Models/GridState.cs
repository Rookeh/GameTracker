namespace GameTracker.Frontend.Models
{
    public class GridState
    {
        private int _itemsPerPage;

        public int CurrentPage { get; set; }
        
        public int ItemsPerPage
        {
            get
            {
                return _itemsPerPage;
            }
            set
            {
                _itemsPerPage = value;
                CurrentPage = 1;
            }
        }

        public int ItemsPerRow { get; set; }
    }
}
