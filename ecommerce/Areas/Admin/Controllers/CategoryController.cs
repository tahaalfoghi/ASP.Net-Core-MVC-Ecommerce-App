using ecommerce.DataAccess.Services.UnitOfWork;
using ecommerce.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Editor")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork uow;

        public CategoryController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await uow.CategoryRepository.CreateAsync(category);
            await uow.CommitAsync();
            TempData["Create"] = "Category created successfully";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var category = await uow.CategoryRepository.GetAsync(x => x.Id == id);
            if(category is null)
            {
                return NotFound();
            } 
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await uow.CategoryRepository.UpdateAsync(category);
            await uow.CommitAsync();
            TempData["Update"] = "Category updated successfully";
            return RedirectToAction("Index");
        }
        #region APICALL
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var cateories = await uow.CategoryRepository.GetAllAsync();
            return Json(new {data= cateories});
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if(!ModelState.IsValid || id <= 0)
            {
                return Json(new { succes = "failed", message = "Failed to delete this category" });
            }
            var categoryInDb = await uow.CategoryRepository.GetByIdAsync(id);
            if (categoryInDb is null)
            {
                return Json(new { success = "Failed", message = "This category not found" });
            }
            await uow.CategoryRepository.DeleteAsync(categoryInDb);
            await uow.CommitAsync();
            TempData["Delete"] = "Category Deleted Successfully";
            return Json(new { success = true, message = "Category deleted successfully" });
        }

        #endregion
    }
}
