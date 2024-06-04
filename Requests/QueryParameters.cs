using System.ComponentModel.DataAnnotations;

namespace TaskManager.Requests
{
    public class QueryParameters
    {
        const int _maxLimit = 100;
        private int _limit = 20;
        private int _page = 1;
        public string SortBy { get; set; } = "Id";
        [AllowedValues("asc", "desc")]
        public string SortOrder { get; set;} = "asc";

        public int Page 
        {
            get { return _page; }
            set {
                if (value < 0) _page = 1;
                else _page = value;
            }
        }
        public int Limit
        {
            get { return _limit; }
            set {
                if (value < 0) _limit = 0;

                else _limit = Math.Min(value, _maxLimit);
            }
        }
    }
}