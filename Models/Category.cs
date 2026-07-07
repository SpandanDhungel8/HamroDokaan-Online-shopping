using System.ComponentModel.DataAnnotations;

namespace HamroDokaan.Models
{
    public class Category
    {
        [Key]
        public int Category_Id { get; set; }

        public string Category_Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
