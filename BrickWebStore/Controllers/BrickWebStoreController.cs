using BrickWebStore.DataAccess.DataContext;
using BrickWebStore.DataAccess.Repositories.Abstractions;
using BrickWebStore.Models;
using BrickWebStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BrickWebStore.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class BrickWebStoreController : Controller
    {
        private readonly IBrickWebStoreRepository _brickWebStoreRepository;

        public BrickWebStoreController(IBrickWebStoreRepository brickWebStoreRepository)
        {
            _brickWebStoreRepository = brickWebStoreRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<BrickWebStoreModel> stores = _brickWebStoreRepository.GetAll();

            return View(stores);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BrickWebStoreModel brickWebStoreModel)
        {
            _brickWebStoreRepository.Add(brickWebStoreModel);

            try
            {
                _brickWebStoreRepository.Save();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _brickWebStoreRepository.FirstOrDefault(x => x.Id == id);
            
            if(obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditPost(BrickWebStoreModel brickModel)
        {
            if (ModelState.IsValid)
            {
                _brickWebStoreRepository.Update(brickModel);

                try
                {
                    _brickWebStoreRepository.Save();

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(brickModel);
            }
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _brickWebStoreRepository.FirstOrDefault(x => x.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
            var obj = _brickWebStoreRepository.FirstOrDefault(x => x.Id == id);
            if(obj == null)
            {
                return NotFound();
            }

            _brickWebStoreRepository.Remove(obj);

            try
            {
                _brickWebStoreRepository.Save();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return RedirectToAction("Index");
        }
    }
}
