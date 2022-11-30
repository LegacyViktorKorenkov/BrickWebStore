using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BrickWebStore.Models;

public class BrickWebStoreModel
{
    [Key]
    public int Id { get; set; }

    [DisplayName("Shop Name"), Required, StringLength(50, MinimumLength =2, ErrorMessage = "You have to type 2 or more symbols")]
    public string ShopName { get; set; }

    [DisplayName("Shop Address"), Required, StringLength(200, MinimumLength = 2, ErrorMessage = "You have to type 2 or more symbols")]
    public string Address { get; set; }
}
