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

namespace DirectorProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        private readonly MovieContext _context;

        public DirectorController(MovieContext context)
        {
            _context = context;
        }

        // GET: api/Director
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DirectorDTO>>> GetDirector()
        {
            return await _context.Director.Select(director => new DirectorDTO
            {
                Id = director.Id,
                FirstName = director.FirstName,
                LastName = director.LastName,
                Movies = director.Movies.Select(dir => new MovieDTO
                {
                    Id = dir.Id,
                    Title = dir.Title,
                    ProductionYear = dir.ProductionYear,
                    TicketsSold = dir.TicketsSold,
                    MovieGenre = dir.MovieGenre,
                }).ToList()
            }).ToListAsync();
        }

        // GET: api/Director/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DirectorDTO>> GetDirector(int id)
        {
            var director = _context.Director
                .Where(dir => dir.Id == id)
                .Include(dir => dir.Movies)
                .Single();

            if (director == null)
            {
                return NotFound();
            }

            return ConvertToDTO(director);
        }

        private DirectorDTO ConvertToDTO(Director director)
        {
            DirectorDTO directorDTO = new DirectorDTO
            {
                Id = director.Id,
                FirstName = director.FirstName,
                LastName = director.LastName,
                Movies = new List<MovieDTO>()
            };
            foreach (Movie movie in director.Movies)
            {
                directorDTO.Movies.Add(new MovieDTO
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    ProductionYear = movie.ProductionYear,
                    TicketsSold = movie.TicketsSold,
                    MovieGenre = movie.MovieGenre,
                });
            }
            return directorDTO;
        }

        // PUT: api/Director/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDirector(int id, DirectorDTO directorDTO)
        {
            if (id != directorDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(ConvertFromDTO(directorDTO)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Director.Any(anyDirector => anyDirector.Id == id))
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

        private Director ConvertFromDTO(DirectorDTO directorDTO)
        {
            var director = _context.Director
                .Where(ship => ship.Id == directorDTO.Id)
                .Include(ship => ship.Movies)
                .SingleOrDefault();

            if (director != null)
            {
                director.FirstName = directorDTO.FirstName;
                director.LastName = directorDTO.LastName;
                director.Movies.Clear();
            }
            else
            {
                director = new Director()
                {
                    Id = directorDTO.Id,
                    FirstName = directorDTO.FirstName,
                    LastName = directorDTO.LastName,
                Movies = new List<Movie>()
                };
            }
            foreach (MovieDTO movie in directorDTO.Movies)
            {
                director.Movies.Add(_context.Movie.Find(movie.Id));
            }
            return director;
        }

        // POST: api/Director
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Director>> PostDirector(DirectorDTO directorDTO)
        {
            Director director = ConvertFromDTO(directorDTO);
            _context.Director.Add(director);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDirector", new { id = directorDTO.Id }, ConvertToDTO(director));
        }

        // DELETE: api/Director/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirector(int id)
        {
            var director = await _context.Director.FindAsync(id);
            if (director == null)
            {
                return NotFound();
            }

            _context.Director.Remove(director);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
