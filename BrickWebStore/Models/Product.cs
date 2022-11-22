using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrickWebStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        public string ShortDesk { get; set; }

        public string ProductDescription { get; set; }

        [Required]
        [Range(0.1d, double.MaxValue)]
        public double ProductPrice { get; set; }

        public string ProductImage { get; set; }

        [Display(Name = "Category Type")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [DisplayName("In Shop")]
        public int StoreAddressId { get; set; }

        [ForeignKey("StoreAddressId")]
        public virtual BrickWebStoreModel BrickStore { get; set; }
    }
}
