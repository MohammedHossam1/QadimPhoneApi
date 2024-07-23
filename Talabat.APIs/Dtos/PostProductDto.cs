namespace Talabat.APIs.Dtos
{
    public class PostProductDto
    {
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? ProductTypeId { get; set; }
    
    }
}
