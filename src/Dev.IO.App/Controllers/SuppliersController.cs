﻿using AutoMapper;
using DevIO.App.Extensions;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.App.Controllers
{
    [Authorize]
    public class SuppliersController : BaseController
    {
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public SuppliersController(ISupplierService supplierService,
                                   IMapper mapper,
                                   INotificator notificator) : base(notificator)
        {
            _supplierService = supplierService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("suppliers-list")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierService.GetAll()));
        }

        [AllowAnonymous]
        [Route("suppliers-details/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var supplierViewModel = await GetSupplierAddressAsync(id);

            if (supplierViewModel == null) return NotFound();

            return View(supplierViewModel);
        }

        [ClaimsAuthorize("Supplier", "Add")]
        [Route("new-supplier")]
        public IActionResult Create()
        {
            return View();
        }

        [ClaimsAuthorize("Supplier", "Add")]
        [Route("new-supplier")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid) return View(supplierViewModel);

            var supplier = _mapper.Map<Supplier>(supplierViewModel);
            await _supplierService.Add(supplier);

            if (!IsValidOperation()) return View(supplierViewModel);

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Supplier", "Edit")]
        [Route("edit-supplier/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var supplierViewModel = await GetSupplierProductsAddressAsync(id);

            if (supplierViewModel == null) return NotFound();

            return View(supplierViewModel);
        }

        [ClaimsAuthorize("Supplier", "Edit")]
        [Route("edit-supplier/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, SupplierViewModel supplierViewModel)
        {
            if (id != supplierViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(supplierViewModel);

            var supplier = _mapper.Map<Supplier>(supplierViewModel);
            await _supplierService.Update(supplier);

            if (!IsValidOperation()) return View(await GetSupplierProductsAddressAsync(id));

            return RedirectToAction(nameof(Index));

        }

        [ClaimsAuthorize("Supplier", "Delete")]
        [Route("delete-supplier/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplierViewModel = await GetSupplierAddressAsync(id);

            if (supplierViewModel == null) return NotFound();

            return View(supplierViewModel);
        }

        [ClaimsAuthorize("Supplier", "Delete")]
        [Route("delete-supplier/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var supplierViewModel = await GetSupplierAddressAsync(id);

            if (supplierViewModel == null) return NotFound();

            await _supplierService.Remove(id);

            if (!IsValidOperation()) return View(supplierViewModel);

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [Route("get-supplier-address/{id:guid}")]
        public async Task<IActionResult> GetAddress(Guid id)
        {
            var supplier = await GetSupplierAddressAsync(id);
            if (supplier == null) return NotFound();

            return PartialView("_AddressDetails", supplier);
        }

        [ClaimsAuthorize("Supplier", "Edit")]
        [Route("update-supplier-address/{id:guid}")]
        public async Task<IActionResult> UpdateAddress(Guid id)
        {
            var supplier = await GetSupplierAddressAsync(id);
            if (supplier == null) return NotFound();

            return PartialView("_UpdateAddress", new SupplierViewModel { Address = supplier.Address });
        }

        [ClaimsAuthorize("Supplier", "Edit")]
        [Route("update-supplier-address/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAddress(SupplierViewModel supplierViewModel)
        {
            ModelState.Remove("Name");
            ModelState.Remove("Document");

            if (!ModelState.IsValid) return PartialView("_UpdateAddress", supplierViewModel);

            await _supplierService.UpdateAddress(_mapper.Map<Address>(supplierViewModel.Address));

            if (!IsValidOperation()) return PartialView("_UpdateAddress", supplierViewModel);

            var url = Url.Action("GetAddress", "Suppliers", new { id = supplierViewModel.Address.SupplierId });
            return Json(new { success = true, url });
        }

        private async Task<SupplierViewModel> GetSupplierAddressAsync(Guid id)
        {
            return _mapper.Map<SupplierViewModel>(await _supplierService.GetSupplierAddress(id));
        }

        private async Task<SupplierViewModel> GetSupplierProductsAddressAsync(Guid id)
        {
            return _mapper.Map<SupplierViewModel>(await _supplierService.GetSupplierProductsAddress(id));
        }
    }
}
