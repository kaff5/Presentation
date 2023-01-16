using Microsoft.AspNetCore.Mvc;
using Presentation1.Services;
using Presentation1.ViewModels;

namespace Presentation1.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var books = _bookService.GetAll();
            return View(books);
        }

        [HttpGet]
        public IActionResult CreateBookGuid() 
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateBookObjectid()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBookGuid(BookCreateViewModel model)
        {

            await _bookService.CreateBookGuidAsync(model);
            return RedirectToAction("Index", "Book");
        }

        [HttpPost]
        public async Task<IActionResult> CreateBookObjectId(BookCreateViewModel model)
        {

            await _bookService.CreateBookObjectIdAsync(model);
            return RedirectToAction("Index", "Book");
        }

        [HttpGet]
        public IActionResult CreateTwoBooksObjectIdAtOnce()
        {
            _bookService.CreateTwoBooksObjectIdAtOnceAsync();
            return RedirectToAction("Index", "Book");
        }

        [HttpGet]
        public IActionResult CreateTwoBooksGuidAtOnce()
        {
            _bookService.CreateTwoBooksGuidAtOnceAsync();
            return RedirectToAction("Index", "Book");
        }

        [HttpGet]
        public IActionResult BookGuid(string Id)
        {
            var model = _bookService.GetBookGuid(Id);
            return View(model);
        }

        [HttpGet]
        public IActionResult BookObjectId(string Id)
        {
            var model = _bookService.GetBookObjectId(Id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBookObjectId(string id)
        {

            await _bookService.RemoveBookObjectIdAsync(id);
            return RedirectToAction("Index", "Book");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBookGuid(string id)
        {

            await _bookService.RemoveBookGuidAsync(id);
            return RedirectToAction("Index", "Book");
        }

        [HttpGet]
        public async Task<IActionResult> EditBook(string id)
        {
            BookViewModel model;
            if (id.Length ==36)
            {
                model = _bookService.GetBookGuid(id);
            }
            else
            {
                model =  _bookService.GetBookObjectId(id);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditBook(BookViewModel model)
        {

            if (model.Type == "Guid")
            {
                await _bookService.EditBookGuid(model);
            }
            else
            {
                await _bookService.EditBookObjectId(model);
            }


            return RedirectToAction("Index", "Book");
        }
    }
}
