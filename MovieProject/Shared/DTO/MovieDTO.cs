using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieProject.Shared.DTO;
public class MovieDTO
{
    public MovieDTO() { 
        Directors = new List<DirectorDTO>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public int ProductionYear { get; set; }
    public int TicketsSold { get; set; }
    public Genre MovieGenre { get; set; }
    public List<DirectorDTO>? Directors { get; set; }
    public bool IsSelected { get; set; }
}
