using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Presentation1.Models;
using Presentation1.ViewModels;
using smtp_sender.Data;
using System;

namespace Presentation1.Services
{
    public interface IBookService
    {
        BooksViewModel GetAll();
        Task CreateBookGuidAsync(BookCreateViewModel bookCreateViewModel);
        Task CreateBookObjectIdAsync(BookCreateViewModel bookCreateViewModel);
        void CreateTwoBooksObjectIdAtOnceAsync();
        BookViewModel GetBookGuid(string id);
        BookViewModel GetBookObjectId(string id);
        Task CreateTwoBooksGuidAtOnceAsync();
        Task RemoveBookGuidAsync(string id);
        Task RemoveBookObjectIdAsync(string id);
        Task EditBookGuid(BookViewModel model);
        Task EditBookObjectId(BookViewModel model);


    }
    public class BookService: IBookService
    {
        private readonly IMongoCollection<BookGuid> booksGuidCollection;
        private readonly IMongoCollection<BookObjectId> booksObjectidCollection;
        public BookService(IOptions<MongoDbConfig> mongoDbConfig) 
        {
            var mongoClient = new MongoClient(
                mongoDbConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                mongoDbConfig.Value.DatabaseName);

            booksGuidCollection = mongoDatabase.GetCollection<BookGuid>("BookGuid");
            booksObjectidCollection = mongoDatabase.GetCollection<BookObjectId>("BookObjectId");
        }

        public BooksViewModel GetAll()
        {
            var booksGuid = booksGuidCollection.Find(_ => true).ToList();
            var booksViewModel = new BooksViewModel()
            {
                Books = new List<BookViewModel>()
            };

            foreach(var book in booksGuid)
            {
                booksViewModel.Books.Add(new BookViewModel
                {
                    Description= book.Description,
                    Id = book.Id.ToString(),
                    Name= book.Name,
                    Price = book.Price,
                    Title= book.Title,
                    Type= book.Type,
                });
            }
            var booksObjectId = booksObjectidCollection.Find(_ => true).ToList();
            foreach (var book in booksObjectId)
            {
                booksViewModel.Books.Add(new BookViewModel
                {
                    Description = book.Description,
                    Id = book.Id.ToString(),
                    Name = book.Name,
                    Price = book.Price,
                    Title = book.Title,
                    Type= book.Type,
                });
            }
            return booksViewModel;
        }
        public async Task CreateBookGuidAsync(BookCreateViewModel bookCreateViewModel)
        {
            BookGuid bookGuid = new BookGuid()
            {
                Description = bookCreateViewModel.Description,
                Name = bookCreateViewModel.Name,
                Price = bookCreateViewModel.Price,
                Title = bookCreateViewModel.Title,
            };

            await booksGuidCollection.InsertOneAsync(bookGuid);
        }
        public async Task CreateBookObjectIdAsync(BookCreateViewModel bookCreateViewModel)
        {
            BookObjectId bookObjectId = new BookObjectId()
            {
                Description = bookCreateViewModel.Description,
                Name = bookCreateViewModel.Name,
                Price = bookCreateViewModel.Price,
                Title = bookCreateViewModel.Title,
            };

            await booksObjectidCollection.InsertOneAsync(bookObjectId);
        }

        public void CreateTwoBooksObjectIdAtOnceAsync()
        {
            Thread myThread = new Thread(CreateBookObjectIdForDublicate);
            myThread.Start(); //запускаем поток


            //основной поток
            BookObjectId bookObjectId = new BookObjectId()
            {
                Description = "",
                Name = "Дубликат",
                Price = 100,
                Title = "",
            };

            booksObjectidCollection.InsertOneAsync(bookObjectId);
        }

        //Функция запускаемая из другого потока
        public void CreateBookObjectIdForDublicate()
        {
            BookObjectId bookObjectId = new BookObjectId()
            {
                Description = "",
                Name = "Дубликат",
                Price = 100,
                Title = "",
            };

            booksObjectidCollection.InsertOneAsync(bookObjectId);
        }

        /*public BookViewModel GetBookGuid(string id)
        {
            var bookGuid = booksGuidCollection.Find(x=> x.Id == Guid.Parse(id)).FirstOrDefault();
            BookViewModel bookViewModel = new BookViewModel()
            {
                Description = bookGuid.Description,
                Id = bookGuid.Id.ToString(),
                Name = bookGuid.Name,
                Price = bookGuid.Price,
                Title = bookGuid.Title,
                Type = bookGuid.Type,
            };

            return bookViewModel;
        }*/
        public BookViewModel GetBookGuid(string id)
        {
            var filter = Builders<BookGuid>.Filter.Eq("_id", Guid.Parse(id));
            var bookGuid = booksGuidCollection.Find(filter).FirstOrDefault();
            BookViewModel bookViewModel = new BookViewModel()
            {
                Description = bookGuid.Description,
                Id = bookGuid.Id.ToString(),
                Name = bookGuid.Name,
                Price = bookGuid.Price,
                Title = bookGuid.Title,
                Type = bookGuid.Type,
            };

            return bookViewModel;
        }

        public BookViewModel GetBookObjectId(string id)
        {
            var bookObjectId = booksObjectidCollection.Find(x => x.Id == id).FirstOrDefault();
            BookViewModel bookViewModel = new BookViewModel()
            {
                Description = bookObjectId.Description,
                Id = bookObjectId.Id.ToString(),
                Name = bookObjectId.Name,
                Price = bookObjectId.Price,
                Title = bookObjectId.Title,
                Type = bookObjectId.Type,
            };

            return bookViewModel;
        }


        public async Task CreateTwoBooksGuidAtOnceAsync()
        {
            Thread myThread = new Thread(CreateBookGuidForDublicate);
            myThread.Start(); //запускаем поток


            //основной поток
            BookGuid bookGuid = new BookGuid()
            {
                Description = "",
                Name = "Дубликат",
                Price = 100,
                Title = "",
            };

            booksGuidCollection.InsertOneAsync(bookGuid);
        }

        //Функция запускаемая из другого потока
        public void CreateBookGuidForDublicate()
        {
            BookGuid bookGuid = new BookGuid()
            {
                Description = "",
                Name = "Дубликат",
                Price = 100,
                Title = "",
            };

            booksGuidCollection.InsertOneAsync(bookGuid);
        }

        public async Task RemoveBookGuidAsync(string id)
        {
            await booksGuidCollection.DeleteOneAsync(x => x.Id == Guid.Parse(id));
        }

        public async Task RemoveBookObjectIdAsync(string id)
        {
            await booksObjectidCollection.DeleteOneAsync(x => x.Id == id);
        }


        public async Task EditBookGuid(BookViewModel model)
        {
            //var filter = Builders<BookGuid>.Filter.Eq(s => s.Id, Guid.Parse(model.Id));
            var update = Builders<BookGuid>.Update
                            .Set(s => s.Description, model.Description).Set(s => s.Name, model.Name);
            await booksGuidCollection.UpdateOneAsync(x=>x.Id == Guid.Parse(model.Id), update);
        }

        public async Task EditBookObjectId(BookViewModel model)
        {
            //var filter = Builders<BookObjectId>.Filter.Eq(s => s.Id, model.Id);
            var update = Builders<BookObjectId>.Update
                            .Set(s => s.Description, model.Description).Set(s => s.Name, model.Name);
            await booksObjectidCollection.UpdateOneAsync(x => x.Id == model.Id, update);
        }

    }
}
