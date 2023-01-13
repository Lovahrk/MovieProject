using MovieProject.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieProject.Shared;
public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int ProductionYear { get; set; }
    public int TicketsSold { get; set; }
    public Genre MovieGenre { get; set; }
    public ICollection<Director> Directors { get; set; }
}
