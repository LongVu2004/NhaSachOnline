namespace NhaSachOnline.Models.DTO
{
    public class BookDislayModel
    {
        public List<BookDTO> Books { get; set; }
        public List<GenreDTO> Genres { get; set; }
        public int? GenreId { get; set; }
        public string KeySearch { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
    }
}
