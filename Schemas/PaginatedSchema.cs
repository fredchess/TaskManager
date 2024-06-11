namespace TaskManager.Services
{
    public class PaginatedSchema<T> where T : class
    {
        public required IEnumerable<T> Datas { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; }
        public int TotalData { get; set; }
    }
}