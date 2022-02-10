using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Moq;

namespace United4Admin.UnitTests.Mocks.ApiClientFactory
{
    public class MockProductVariantFactory:Mock<IProductVariantFactory>
    {
        public async Task<MockProductVariantFactory> MockLoad(ProductVariantVM result)
        {
            Setup(x => x.LoadListById(It.IsAny<string>()))
                .Returns(Task.Run(() => result));
            return await Task.FromResult(this);
        }

        public async Task<MockProductVariantFactory> MockLoadList(List<ProductVariantVM> result)
        {
            if (result != null)
            {
                Setup(x => x.LoadList()).Returns(Task.Run(() => new List<ProductVariantVM>(result)));
            }
            else
            {
                Setup(x => x.LoadList()).Returns(Task.Run(() => new List<ProductVariantVM>()));
            }

            return await Task.FromResult(this);
        }

        public async Task<MockProductVariantFactory> MockCreate(ApiResponse result, IList<ProductVariantVM> objList, ProductVariantVM objCreate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<ProductVariantVM>(objList)));
            // Allows us to test saving a product
            Setup(x => x.Create(It.IsAny<ProductVariantVM>())).Returns(Task.Run(() => result));
            if (objCreate != null && Convert.ToBoolean(result.ResponseObject))
            {
                objList.Add(objCreate);
            }
            return await Task.FromResult(this);
        }
        public async Task<MockProductVariantFactory> MockUpdate(ApiResponse result, IList<ProductVariantVM> objList, ProductVariantVM objUpdate)
        {
            Setup(x => x.LoadList()).Returns(Task.Run(() => new List<ProductVariantVM>(objList)));
            // Allows us to test saving a product
            Setup(x => x.Update(It.IsAny<ProductVariantVM>())).Returns(Task.Run(() => result));
            if (objUpdate != null && Convert.ToBoolean(result.ResponseObject))
            {
                var objDelete = objList.Where(x => x.ddlProductTypeCodeDisplay == objUpdate.ddlProductTypeCodeDisplay).FirstOrDefault();
                objList.Remove(objDelete);
                objList.Add(objUpdate);
            }
            return await Task.FromResult(this);
        }

        public async Task<MockProductVariantFactory> MockDeleteLoad(ProductVariantVM result, string id)
        {
            Setup(x => x.LoadListById(It.IsAny<string>()))
               .Returns(Task.Run(() => result));

            return await Task.FromResult(this); ;
        }

        public async Task<MockProductVariantFactory> MockDelete(ApiResponse result, IList<ProductVariantVM> objList, string id)
        {
            Setup(x => x.DeleteById(It.IsAny<string>())).Returns(Task.Run(() => result));
            if (!String.IsNullOrEmpty(id) && Convert.ToBoolean(result.ResponseObject))
            {
                var objDelete = objList.Where(x => x.ddlProductTypeCodeDisplay == id).FirstOrDefault();
                objList.Remove(objDelete);
            }
            return await Task.FromResult(this); ;
        }

        public ProductVariantVM MockFindById(IList<ProductVariantVM> ProductVariantVMModels, string id)
        {
            var result = ProductVariantVMModels.Where(x => x.ddlProductTypeCodeDisplay == id).SingleOrDefault();
            return result;
        }
        public ProductVariantVM MockFindByName(List<ProductVariantVM> ProductVariantVMModels, string name)
        {
            var result = ProductVariantVMModels.Where(x => x.crmProductVariantName == name).SingleOrDefault();
            return result;
        }
    }
}
