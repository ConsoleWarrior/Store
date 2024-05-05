﻿using Store.web.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public class BookService
    {
        private readonly IBookRepository bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public IReadOnlyCollection<BookModel> GetAllByQuery(string query)
        {
            var books = Book.IsIsbn(query)
                ? bookRepository.GetAllByIsbn(query)
                : bookRepository.GetAllByTitleOrAuthor(query);
            return books.Select(Map).ToArray();
            //if (query == null) return new Book[0];
            //else if(Book.IsIsbn(query)) return bookRepository.GetAllByIsbn(query);
            //else return bookRepository.GetAllByTitleOrAuthor(query);
        }

		public BookModel GetById(int id)
		{
			var book = bookRepository.GetById(id);
            return Map(book);
		}

		private BookModel Map(Book book)
		{
            return new BookModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Isbn = book.Isbn,
                Description = book.Description,
                Price = book.Price
            };
		}
	}
}
