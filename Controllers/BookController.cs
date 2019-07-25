using System.Collections.Generic;
using basicWebApi.Models.Param;
using Microsoft.AspNetCore.Mvc;
using System.Linq; //language integrated query
using basicWebApi.Models.View;
using System;
using basicWebApi.Models;
using basicWebApi.Models.Entity;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace basicWebApi.Controllers //namaProject.namaFolder (filepath)
{
    [Route("api/[controller]")] //kasih endpoint
    //biar dia tahu ini controller, kasih atribut
    [ApiController]
    public class BookController : ControllerBase //namaKelas = namaFile
    {
        private readonly BasicDbContext dbContext;

        public BookController(BasicDbContext dbContext) //constructor
        {
            this.dbContext = dbContext; //untuk inisialisasi object BasicDbContext
            if (this.dbContext.Books.Count() == 0)
            {
                this.dbContext.Books.Add(new Book
                {
                    Id = 1,
                    Date = DateTime.UtcNow,
                    Title = "Harry Potter"
                });
                this.dbContext.Books.Add(new Book
                {
                    Id = 2,
                    Date = DateTime.UtcNow,
                    Title = "Sherlock Holmes"
                });
                this.dbContext.Books.Add(new Book
                {
                    Id = 3,
                    Date = DateTime.UtcNow,
                    Title = "The Lord of the Ring"
                });
                this.dbContext.Books.Add(new Book
                {
                    Id = 4,
                    Date = DateTime.UtcNow,
                    Title = "Assassins Creed"
                });
                this.dbContext.SaveChanges();
            }
        }
        //mendapatkan daftar buku
        //public bisa diakses di kelas lain
        [HttpGet] //dikasih tau kalo ini get(atribut)
        public async Task<ActionResult<IEnumerable<BookView>>> Get([FromQuery]GetBookParam param) //API endpoint
        {
            var dataSource = await dbContext.Books
                .Select(book => new BookView
                {
                    Title = book.Title,
                    Date = book.Date
                }) //SELECT Title, Date FROM Books
                .ToListAsync();
            //ToList => ToListAsync
            //ToListAsync => untuk operation asynchronous, tidak bisa dipakai untuk variabel
            if (string.IsNullOrWhiteSpace(param.Title)) //validasi parameter
            {
                return dataSource;
            }
            //object.method(callback function).tolist()
            return dataSource
                .Where(item => item.Title.ToLower().Contains(param.Title.ToLower()))
                .ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<BookView>>> Get([FromRoute]int Id)
        {
            var dataSource = dbContext.Books;
            var selectedBook = await dataSource
                .Where(book => book.Id == Id)
                .Select(book => new BookView
                {
                    Title = book.Title,
                    Date = book.Date
                })
                .ToListAsync();
            if (await dataSource.SingleOrDefaultAsync(book => book.Id == Id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return selectedBook;
        }

        [HttpPost] //dikasih tau kalo ini post(atribut)
        public async Task<ActionResult<IEnumerable<BookView>>> Post([FromBody]BookView param) //menerima body dalam bentuk JSON secara default
        {
            //SELECT COUNT(*)
            var newId = await dbContext.Books.CountAsync() + 1;
            dbContext.Add(new Book
            {
                Id = newId,
                Title = param.Title,
                Date = param.Date
            });
            await dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);//return biasa untuk post/patch => 201
        }

        //localhost/api/book/{id}
        [HttpPut("{id}")] //dikasih tau kalo ini put(atribut)
        public async Task<ActionResult<IEnumerable<BookView>>> Put([FromRoute]int id, [FromBody]BookView param) //menerima body dalam bentuk JSON secara default
        {
            var selectedBook = await dbContext.Books.SingleOrDefaultAsync(book => book.Id == id);
            //singleordefault => untuk data unique => kalo gak ketemu error(single), firstordefault => yang ketemu pertama, kalo gak ada gak error
            if (selectedBook == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            selectedBook.Title = param.Title;
            selectedBook.Date = param.Date;
            await dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}