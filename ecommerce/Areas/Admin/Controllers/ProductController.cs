using ecommerce.DataAccess.Services.UnitOfWork;
using ecommerce.Entities.Models;
using ecommerce.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ecommerce_app.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Editor")]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork uow;
        private readonly IWebHostEnvironment webenv;

        public ProductController(IUnitOfWork uow, IWebHostEnvironment webenv)
        {
            this.uow = uow;
            this.webenv = webenv;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ProductVM productVm = new ProductVM()
            {
                Product = new Product(),
                CategoryList = (await uow.CategoryRepository.GetAllAsync()).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
                .OrderBy(x => x.Text)
                .ToList()
            };
            return View(productVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVM productVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string rootPath = webenv.WebRootPath;
                if (file is not null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload = Path.Combine(rootPath, @"Images\Products");
                    var ex = Path.Combine(file.FileName);
                    using (var fileStream = new FileStream(Path.Combine(upload, filename + ex), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImgUrl = @"Images\Products\" + filename + ex;
                }
                await uow.ProductRepository.CreateAsync(productVM.Product);
                await uow.CommitAsync();
                TempData["Create"] = "Product Created Successfully";
                return RedirectToAction("Index");

            }
            TempData["error"] = "Invalid data";
            return View(productVM.Product);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0 || id == null)
            {
                NotFound();
            }

            ProductVM productVM = new ProductVM()
            {
                Product = await uow.ProductRepository.GetByIdAsync(id),
                CategoryList = (await uow.CategoryRepository.GetAllAsync()).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                })
                .OrderBy(x => x.Text)
                .ToList()
            };
            return View(productVM);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductVM productVm, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string rootPath = webenv.WebRootPath;
                if (file is not null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload = Path.Combine(rootPath, @"Images\Products");
                    var ex = Path.Combine(file.FileName);
                    if (productVm.Product.ImgUrl is not null)
                    {
                        var oldImg = Path.Combine(rootPath, productVm.Product.ImgUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImg))
                        {
                            System.IO.File.Delete(oldImg);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(upload, filename + ex), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVm.Product.ImgUrl = @"Images\Products\" + filename + ex;
                }
                await uow.ProductRepository.UpdateAsync(productVm.Product);
                await uow.CommitAsync();
                TempData["Update"] = "Product Updated Successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Invalid data to updating the product";
            return View(productVm.Product);
        }

        #region APICALL
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            IEnumerable<Product> products = await uow.ProductRepository.GetAllAsync(includes: "Category");
            return Json(new { data = products });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid || id <= 0)
            {
                return Json(new { success = "Failed", messsage = "model is invalid or id is less than or equal 0" });
            }
            Product product = await uow.ProductRepository.GetAsync(x => x.Id == id);
            if (product is null)
            {
                return Json(new { success = "Failed", messsage = "product not found" });
            }
            var oldImage = Path.Combine(webenv.WebRootPath, product.ImgUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImage))
            {
                System.IO.File.Delete(oldImage);
            }
            uow.ProductRepository.DeleteAsync(product);
            await uow.CommitAsync();
            TempData["Delete"] = "Product deleted successfully";
            return Json(new { success = true, message = "Product deleted successfully" });

        }
        #endregion
    }
}