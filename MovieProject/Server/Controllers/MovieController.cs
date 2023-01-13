using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieProject.Server;
using MovieProject.Shared;
using MovieProject.Shared.DTO;

namespace MovieProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieContext _context;

        public MovieController(MovieContext context)
        {
            _context = context;
        }

        // GET: api/Movie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovie()
        {
            return await _context.Movie.Select(movie => new MovieDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                ProductionYear = movie.ProductionYear,
                TicketsSold = movie.TicketsSold,
                MovieGenre = movie.MovieGenre,
                Directors = movie.Directors.Select(dir => new DirectorDTO
                {
                    Id = dir.Id,
                    FirstName = dir.FirstName,
                    LastName = dir.LastName
                }).ToList()
            }).ToListAsync();
        }

        // GET: api/Movie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovie(int id)
        {
            var movie = _context.Movie
                .Where(movie => movie.Id == id)
                .Include(movie => movie.Directors)
                .Include(movie => movie.MovieGenre)
                .Single();

            if (movie == null)
            {
                return NotFound();
            }

            return ConvertToDTO(movie);
        }

        private MovieDTO ConvertToDTO(Movie movie)
        {
            MovieDTO movieDTO = new MovieDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                ProductionYear = movie.ProductionYear,
                TicketsSold = movie.TicketsSold,
                MovieGenre = movie.MovieGenre,
                Directors = new List<DirectorDTO>()
            };
            foreach (Director director in movie.Directors)
            {
                movieDTO.Directors.Add(new DirectorDTO
                {
                    Id = director.Id,
                    FirstName = director.FirstName,
                    LastName = director.LastName
                });
            }
            return movieDTO;
        }

        // PUT: api/Movie/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieDTO movieDTO)
        {
            if (id != movieDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(ConvertFromDTO(movieDTO)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Movie.Any(anyMovie => anyMovie.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private Movie ConvertFromDTO(MovieDTO movieDTO)
        {
            var movie = _context.Movie
                .Where(ship => ship.Id == movieDTO.Id)
                .Include(ship => ship.Directors)
                .Include(ship => ship.MovieGenre)
                .SingleOrDefault();

            if (movie != null)
            {
                movie.Title = movieDTO.Title;
                movie.ProductionYear = movieDTO.ProductionYear;
                movie.TicketsSold = movieDTO.TicketsSold;
                movie.MovieGenre = _context.Genre.Find(movieDTO.MovieGenre.Id);
                movie.Directors.Clear();
            }
            else
            {
                movie = new Movie()
                {
                    Id = movieDTO.Id,
                    Title = movieDTO.Title,
                    ProductionYear = movieDTO.ProductionYear,
                    TicketsSold = movieDTO.TicketsSold,
                    MovieGenre = _context.Genre.Find(movieDTO.MovieGenre.Id),
                    Directors = new List<Director>()
                };
            }
            foreach (DirectorDTO director in movieDTO.Directors)
            {
                movie.Directors.Add(_context.Director.Find(director.Id));
            }
            return movie;
        }

        // POST: api/Movie
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(MovieDTO movieDTO)
        {
            Movie movie = ConvertFromDTO(movieDTO);
            _context.Movie.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movieDTO.Id }, ConvertToDTO(movie));
        }

        // DELETE: api/Movie/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
