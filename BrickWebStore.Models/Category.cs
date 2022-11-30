using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BrickWebStore.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "You have to type 2 or more symbols")]
    [DisplayName("Category name")]
    public string CategoryName { get; set; }

    [Required]
    [Range(1, 50, ErrorMessage = @"You have to type more then ""0""")]
    [DisplayName("Display order")]
    public int DisplayOrder { get; set; }
}
