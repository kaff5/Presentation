namespace Presentation1.ViewModels
{
    public class BookViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; } = 100;
        public string Type { get; set; }
    }
    public class BooksViewModel
    {
        public List<BookViewModel> Books { get; set; }
    }
}
