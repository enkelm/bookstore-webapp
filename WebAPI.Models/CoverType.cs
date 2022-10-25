namespace API.Models
{
    public class CoverType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IList<Product> Products { get; set; }
    }
}
