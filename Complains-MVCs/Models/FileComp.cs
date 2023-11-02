using System.ComponentModel.DataAnnotations.Schema;

namespace Complains_MVCs.Models
{
    public class FileComp
    {
        public int Id { get; set; }
        public string Text { get; set; }
        
        public string ContentType { get; set; }
        [NotMapped]
        public IFormFile? fileUp { get; set; }
        public string fileName { get; set; }
        public string Status { get; set; } = "Holding";
        public int UserId { get; set; }
        public List<Demand> Demands { get; set; }
    }
}
