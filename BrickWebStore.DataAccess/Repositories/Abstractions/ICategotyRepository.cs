using BrickWebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickWebStore.DataAccess.Repositories.Abstractions
{
    public interface ICategotyRepository : IRepository<Category>
    {
        void Update(Category category);
    }
}
