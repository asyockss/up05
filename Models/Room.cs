using System.ComponentModel;

namespace inventory.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int? ResponsibleId { get; set; }
    }
}
