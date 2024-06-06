using BookBarn.Application.Services.Interfaces;
using BookBarn.Domain.Entities;

namespace BookBarn.Application.Services.Implementations
{
    public class BookService : IBookService
    {
        public Task AddBookAsync(Book book)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBookAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetBookByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBookAsync(Book book)
        {
            throw new NotImplementedException();
        }
    }
}
