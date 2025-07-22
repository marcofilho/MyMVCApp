using AutoMapper;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace DevIO.App.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, ISupplierRepository supplierRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsSuppliers()));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var productViewModel = await GetProductAsync(id);
            if (productViewModel == null)
            {
                return NotFound();
            }

            return View(productViewModel);
        }

        public async Task<IActionResult> Create()
        {
            return View(await SetSuppliers(new ProductViewModel()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            productViewModel = await SetSuppliers(productViewModel);
            if (!ModelState.IsValid) return View(productViewModel);

            if (productViewModel.ImageUpload == null || productViewModel.ImageUpload.Length <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please select an image for the product.");
                return View(productViewModel);
            }

            var prefixImg = Guid.NewGuid() + "_";
            if (!await FileUpload(productViewModel.ImageUpload, prefixImg)) return View(productViewModel);

            productViewModel.Image = prefixImg + productViewModel.ImageUpload.FileName;

            await _productRepository.AddAsync(_mapper.Map<Product>(productViewModel));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var productViewModel = await GetProductAsync(id);
            if (productViewModel == null)
            {
                return NotFound();
            }

            return View(productViewModel);
        }

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

            await _productRepository.UpdateAsync(_mapper.Map<Product>(updateProduct));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var productViewModel = await GetProductAsync(id);
            if (productViewModel == null) return NotFound();

            return View(productViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await GetProductAsync(id);
            if (product == null) return NotFound();

            await _productRepository.RemoveAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<ProductViewModel> GetProductAsync(Guid id)
        {
            var product = _mapper.Map<ProductViewModel>(await _productRepository.GetProductSupplier(id));
            product.Suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAllAsync());

            return product;
        }

        private async Task<ProductViewModel> SetSuppliers(ProductViewModel productViewModel)
        {
            productViewModel.Suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAllAsync());

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
