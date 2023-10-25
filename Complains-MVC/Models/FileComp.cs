namespace Complains-MVC.Models
{
    public class FileComp
    {
        public int Id { get; set; }
        public string Text { get; set; }
        
        public string ContentType { get; set; }
        //public byte[] Data { get; set; }
        public string Status { get; set; } = "Holding";
        public int UserId { get; set; }
       
    }
}
