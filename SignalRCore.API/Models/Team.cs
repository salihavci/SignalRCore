using System.Collections.Generic;

namespace SignalRCore.API.Models
{
    public class Team
    {
        public Team()
        {
            User = new List<User>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<User> User { get; set; }

    }
}
