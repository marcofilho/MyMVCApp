using AutoMapper;
using DevIO.App.Extensions;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.App.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService,
                                  ISupplierService supplierService,
                                  IMapper mapper,
                                  INotificator notificator) : base(notificator)
        {
            _productService = productService;
            _supplierService = supplierService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("products-list")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProductViewModel>>(await _productService.GetProductsSuppliers()));
        }

        [AllowAnonymous]
        [Route("product-details/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var productViewModel = await GetProductAsync(id);
            if (productViewModel == null)
            {
                return NotFound();
            }

            return View(productViewModel);
        }

        [ClaimsAuthorize("Product", "Add")]
        [Route("new-product")]
        public async Task<IActionResult> Create()
        {
            return View(await SetSuppliers(new ProductViewModel()));
        }

        [ClaimsAuthorize("Product", "Add")]
        [Route("new-product")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            productViewModel = await SetSuppliers(productViewModel);
            if (!ModelState.IsValid) return View(productViewModel);

            var prefixImg = Guid.NewGuid() + "_";
            if (!await FileUpload(productViewModel.ImageUpload, prefixImg)) return View(productViewModel);

            productViewModel.Image = prefixImg + productViewModel.ImageUpload.FileName;
            await _productService.Add(_mapper.Map<Product>(productViewModel));

            if (!IsValidOperation()) return View(productViewModel);

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Product", "Edit")]
        [Route("edit-product/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var productViewModel = await GetProductAsync(id);
            if (productViewModel == null)
            {
                return NotFound();
            }

            return View(productViewModel);
        }

        [ClaimsAuthorize("Product", "Edit")]
        [Route("edit-product/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id) return NotFound();

            var updateProduct = await GetProductAsync(id);
            productViewModel.Supplier = updateProduct.Supplier;
            productViewModel.Image = updateProduct.Image;
            if (!ModelState.IsValid) return View(productViewModel);

            if (productViewModel.ImageUpload != null)
            {
                var prefixImg = Guid.NewGuid() + "_";
                if (!await FileUpload(productViewModel.ImageUpload, prefixImg)) return View(productViewModel);

                updateProduct.Image = prefixImg + productViewModel.ImageUpload.FileName;
            }

            updateProduct.Name = productViewModel.Name;
            updateProduct.Description = productViewModel.Description;
            updateProduct.Price = productViewModel.Price;
            updateProduct.Active = productViewModel.Active;

            await _productService.Update(_mapper.Map<Product>(updateProduct));

            if (!IsValidOperation()) return View(productViewModel);

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Product", "Delete")]
        [Route("delete-product/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var productViewModel = await GetProductAsync(id);
            if (productViewModel == null) return NotFound();

            return View(productViewModel);
        }

        [ClaimsAuthorize("Product", "Delete")]
        [Route("delete-product/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await GetProductAsync(id);
            if (product == null) return NotFound();

            await _productService.Remove(id);

            if (!IsValidOperation()) return View(product);

            TempData["Success"] = "Product removed with success!";

            return RedirectToAction(nameof(Index));
        }

        private async Task<ProductViewModel> GetProductAsync(Guid id)
        {
            var product = _mapper.Map<ProductViewModel>(await _productService.GetProductSupplier(id));
            product.Suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierService.GetAll());

            return product;
        }

        private async Task<ProductViewModel> SetSuppliers(ProductViewModel productViewModel)
        {
            productViewModel.Suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierService.GetAll());

            return productViewModel;
        }

        private async Task<bool> FileUpload(IFormFile file, string prefixImg)
        {
            if (file == null || file.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", prefixImg + file.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "An image with this name already exists.");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return true;
        }
    }
}
