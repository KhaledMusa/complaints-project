using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace project_comp.Models
{
    public class FileComp
    {
        public int Id { get; set; }
        public string Text { get; set; }
        
        public string ContentType { get; set; }
        
        public string fileName { get; set; }
        public string Status { get; set; } = "Holding";
        public int UserId { get; set; }
        //public int DemandId { get; set; }
        //public Demand Demand { get; set; }
    }
}
