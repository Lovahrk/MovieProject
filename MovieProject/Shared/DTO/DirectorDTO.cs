using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieProject.Shared.DTO;
public class DirectorDTO
{
    public DirectorDTO()
    {
        Movies = new List<MovieDTO>();
    }
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<MovieDTO>? Movies { get; set; }
    public bool IsSelected { get; set; }
}