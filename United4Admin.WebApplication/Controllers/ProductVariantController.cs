using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.BLL;
using United4Admin.WebApplication.ViewModels;

namespace United4Admin.WebApplication.Controllers
{
    [Authorize(Policy = "CreateEditDeleteEvents")]
    public class ProductVariantController : Controller
    {
        private readonly IProductVariantFactory _productVariantFactory;
        private readonly ILogger<ProductVariantController> _logger;
        private readonly string pageName = "Product Variant";
        public ProductVariantController(IProductVariantFactory productVariantFactory, IPermissionFactory permissionFactory, ILogger<ProductVariantController> logger)
        {
            _productVariantFactory = productVariantFactory;
            _logger = logger;
        }

        public async Task<ActionResult> CreateAsync(int? id=0)
        {
            ViewBag.Action = "Create";
            ProductVariantVM newProductVariant = new ProductVariantVM
            {
                Create = true
            };
            await SetDataLists(newProductVariant);
            return View("Edit", newProductVariant);
        }

        //Create Product variant
        [HttpPost]
        public async Task<ActionResult> Create(ProductVariantVM ProductVariantVM)
        {
            ViewBag.Action = "Create";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<ProductVariantVM> ProductVariantVMList = await _productVariantFactory.LoadList();
                    var productVariantExistsCount = ProductVariantVMList.Where(x => x.ddlProductTypeCodeDisplay == ProductVariantVM.ddlProductTypeCodeDisplay &&
                                      x.crmProductVariantName == ProductVariantVM.crmProductVariantName &&
                                      x.crmIncidentType == ProductVariantVM.crmIncidentType &&
                                      x.crmPurposeCode == ProductVariantVM.crmPurposeCode &&
                                      x.crmPledgeType == ProductVariantVM.crmPledgeType).Count();
                    if (productVariantExistsCount == 0)
                    {
                        var response = await _productVariantFactory.Create(ProductVariantVM);
                        _logger.LogInformation(ConstantMessages.Create.Replace("{event}", pageName));
                        return RedirectToAction("Index", "NavisionMapping", new { notification = response.Message });
                    }
                    else
                    {
                        ModelState.AddModelError("Exists", "Already Exists this salutation information");
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                        ProductVariantVM.crmProductVariantName = "";
                    }
                }
                await SetDataLists(ProductVariantVM);
                return View("Edit", ProductVariantVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                await SetDataLists(ProductVariantVM);
                return RedirectToAction("Index", "NavisionMapping");
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.Action = "Edit";

            try
            {
                ProductVariantVM ProductVariantVM = await _productVariantFactory.LoadListById(id);
                if (ProductVariantVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", pageName));
                await SetDataLists(ProductVariantVM);
                return View("Edit", ProductVariantVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        //Edit Product variant
        [HttpPost]
        public async Task<ActionResult> Edit(ProductVariantVM ProductVariantVM)
        {
            string pageAction = "Edit";
            try
            {
                if (ModelState.IsValid)
                {
                    IList<ProductVariantVM> ProductVariantVMList = await _productVariantFactory.LoadList();
                    var productVariantExistsCount = ProductVariantVMList.Where(x => x.ddlProductTypeCodeDisplay != ProductVariantVM.ddlProductTypeCodeDisplay &&
                                      x.crmProductVariantName == ProductVariantVM.crmProductVariantName &&
                                      x.crmIncidentType == ProductVariantVM.crmIncidentType &&
                                      x.crmPurposeCode == ProductVariantVM.crmPurposeCode &&
                                      x.crmPledgeType == ProductVariantVM.crmPledgeType).Count();
                    if (productVariantExistsCount == 0)
                    {
                        var response = await _productVariantFactory.Update(ProductVariantVM);
                        _logger.LogInformation(ConstantMessages.Update.Replace("{event}", pageName));
                        return RedirectToAction("Index", "NavisionMapping", new { notification = response.Message });
                    }
                    else
                    {
                        ModelState.AddModelError("Exists", "Already Exists this product variant information");
                        _logger.LogWarning(ConstantMessages.Duplicate.Replace("{event}", pageName));
                    }
                }

                ViewBag.Action = pageAction;
                await SetDataLists(ProductVariantVM);
                return View("Edit", ProductVariantVM);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ViewBag.Action = pageAction;
                ModelState.AddModelError("Error", ConstantMessages.Error);
                await SetDataLists(ProductVariantVM);
                return View("Edit", ProductVariantVM);
            }
        }

        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                ProductVariantVM ProductVariantVM = await _productVariantFactory.LoadListById(id);
                if (ProductVariantVM == null)
                {
                    _logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", pageName));
                    return NotFound();
                }
                return View(ProductVariantVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        //Delete Product variant
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var response = await _productVariantFactory.DeleteById(id);
                _logger.LogInformation(ConstantMessages.Delete.Replace("{event}", pageName));
                return RedirectToAction("Index", "NavisionMapping", new { notification = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
                ProductVariantVM cpVM = await _productVariantFactory.LoadListById(id);
                ModelState.AddModelError("Error", ConstantMessages.Error);
                return View(cpVM);
            }
        }

        // Set data list for Incident and pledge type
        private async Task SetDataLists(ProductVariantVM productVariantVM)
        {
            try
            {
                productVariantVM.IncidentTypeList = await _productVariantFactory.GetIncidentTypes();
                productVariantVM.PledgeTypeList = await _productVariantFactory.GetPledgeTypes();
                _logger.LogInformation(ConstantMessages.Load.Replace("{event}", "SetDataLists"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ConstantMessages.Error);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _productVariantFactory.Dispose();

            }
            base.Dispose(disposing);
        }
    }
}
