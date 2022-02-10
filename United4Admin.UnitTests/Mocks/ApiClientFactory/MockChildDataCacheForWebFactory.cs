using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Moq;
using Newtonsoft.Json;

namespace United4Admin.UnitTests.Mocks.ApiClientFactory
{


	public class MockChildDataCacheForWebFactory : Mock<IChildDataCacheForWebFactory>
	{


		public async Task<MockChildDataCacheForWebFactory> MockGetChildDataPaged(ChildInternalDataPagedVM result, int pageNumber, int pageSize, int totalNumber)
		{
			if (pageSize <= totalNumber)
			{ result.ChildInternalDataModels = GenerateMockChildData(pageSize); }
			else
			{ result.ChildInternalDataModels = GenerateMockChildData(totalNumber); }
			result.CurrentCount = result.ChildInternalDataModels.Count;
			result.CurrentPage = pageNumber;
			result.TotalPages = (int)Math.Ceiling(totalNumber / (double)pageSize);
			result.TotalNumber = totalNumber;
			result.PreviousPage = result.CurrentPage > 1;
			result.NextPage = result.CurrentPage < result.TotalPages;
			result.PageSize = pageSize;
			Setup(x => x.GetAllChildren(pageNumber, pageSize)).Returns(Task.Run(() => result));
			return await Task.FromResult(this);
		}

		public async Task<MockChildDataCacheForWebFactory> MockGetChildDataPagedThrowsException(int pageNumber, int pageSize)
		{
			Setup(x => x.GetAllChildren(pageNumber, pageSize)).Throws<Exception>();
			return await Task.FromResult(this);
		}

		public List<ChildInternalDataVM> GenerateMockChildData(int TotalRecordCount)
		{
			List<ChildInternalDataVM> mockChildData = new List<ChildInternalDataVM>();
			for (int i = 1; i <= TotalRecordCount; i++)
			{
				mockChildData.Add(new ChildInternalDataVM() { ChildId = "TST-123456-" + i.ToString().PadLeft(4, '0'), Id = i, Name = "Child" + i.ToString() });
			}
			return mockChildData;
		}

		public async Task<MockChildDataCacheForWebFactory> MockGetChildLoad(List<ChildInternalDataVM> childData, int id)
		{
			ChildInternalDataVM selectedChild = childData.FirstOrDefault(x => x.Id == id);
			Setup(x => x.Load(id)).Returns(Task.Run(() => selectedChild));
			return await Task.FromResult(this);
		}
		public async Task<MockChildDataCacheForWebFactory> MockLoadThrowsException(int id)
		{
			Setup(x => x.Load(id)).Throws<Exception>();
			return await Task.FromResult(this);
		}

		public async Task<MockChildDataCacheForWebFactory> MockDeleteChildRecord(List<ChildInternalDataVM> childData, int id)
		{
			ChildInternalDataVM selectedChild = childData.FirstOrDefault(x => x.Id == id && x.SelectedDateTime == null && x.SponsoredDateTime == null);
			ApiResponse response = new ApiResponse();

			if (selectedChild != null)
			{
				childData.Remove(selectedChild);
				response.Success = true;
				response.Message = "Deletion successful";
			}
			else
			{
				response.Success = false;
				response.Message = "Error on deletion";
			}
			Setup(x => x.DeleteChildRecord(id)).Returns(Task.Run(() => response));
			Setup(x => x.Load(id)).Returns(Task.Run(() => childData.FirstOrDefault(x => x.Id == id)));
			return await Task.FromResult(this);
		}

		public async Task<MockChildDataCacheForWebFactory> MockSponsorChildRecord(List<ChildInternalDataVM> childData, int id)
		{
			ChildInternalDataVM selectedChild = childData.FirstOrDefault(x => x.Id == id && x.SelectedDateTime == null && x.SponsoredDateTime == null);
			Setup(x => x.Load(id)).Returns(Task.Run(() => childData.FirstOrDefault(x => x.Id == id)));
			ApiResponse response = new ApiResponse();

			if (selectedChild != null)
			{
				//selectedChild.SponsoredDateTime = DateTime.Now;
				response.Success = true;
				response.Message = "Child Sponsored successfully";
			}
			else
			{
				response.Success = false;
				response.Message = "Error on updating to sponsored";
			}
			Setup(x => x.ChangeStatusSponsored(It.IsAny<UpdateChildStatusViewModel>())).Returns(Task.Run(() => response));
			
			return await Task.FromResult(this);
		}

		public async Task<MockChildDataCacheForWebFactory> MockDeleteConfirmThrowsException(List<ChildInternalDataVM> childData, int id)
		{
			Setup(x => x.DeleteChildRecord(id)).Throws<Exception>();
			Setup(x => x.Load(id)).Returns(Task.Run(() => childData.FirstOrDefault(x => x.Id == id)));
			return await Task.FromResult(this);
		}
		public async Task<MockChildDataCacheForWebFactory> MockChangeStatusConfirmThrowsException(List<ChildInternalDataVM> childData, int id)
		{
			UpdateChildStatusViewModel updateChildStatusVM = new UpdateChildStatusViewModel() { Id = id };
			Setup(x => x.ChangeStatusSponsored(updateChildStatusVM)).Throws<Exception>();
			Setup(x => x.Load(id)).Returns(Task.Run(() => childData.FirstOrDefault(x => x.Id == id)));
			return await Task.FromResult(this);
		}
	}
}
