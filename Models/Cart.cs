using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HamroDokaan.Models
{
    public class Cart
    {
        [Key]
        public int Cart_Id { get; set; }
        [ForeignKey("Product")]
        public int Product_Id { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }
        [ForeignKey("User")]
        public int User_Id { get; set; }
    }
}
