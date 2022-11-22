using BrickWebStore.Models;

namespace BrickWebStore.ViewModels
{
    public class DetailsViewModel
    {
        public Product Product { get; set; }

        public bool IsInCard { get; set; }

        public DetailsViewModel() 
        { 
            Product = new Product();
        }
    }
}
