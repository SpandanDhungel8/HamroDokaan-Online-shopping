using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HamroDokaan.Models
{
    public class Order
    {
        [Key]
        public int Order_Id { get; set; }

        [ForeignKey("User")]
        public int User_Id { get; set; }
        public User User { get; set; }  

        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        public Product Product { get; set; }  

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending";
    }
}
