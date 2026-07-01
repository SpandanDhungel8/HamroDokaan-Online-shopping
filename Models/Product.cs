using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HamroDokaan.Models
{
    public class Product
    {
        [Key]
        public int Product_Id { get; set; }

        public string Product_Name { get; set; }
        public string Product_Price { get; set; }

        public List<Order> Orders { get; set; }
        [ForeignKey("Category")]
        public int Category_Id { get; set; }
        public Category Category { get; set; }
        public string Product_img { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
