using HamroDokaan.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    [Key]
    public int Order_Id { get; set; }

    public int User_Id { get; set; }

    public User User { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;

    public string Status { get; set; } = "Pending";

    public string PaymentMethod { get; set; }

    public decimal TotalAmount { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }

    public List<OrderItem> OrderItems { get; set; }
}