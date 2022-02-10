using United4Admin.WebApplication.Controllers;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using United4Admin.WebApplication.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using United4Admin.UnitTests.Mocks.ApiClientFactory;
using United4Admin.WebApplication.APIClient;
using Newtonsoft.Json;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using System.Linq;

namespace United4Admin.UnitTests.Controllers
{
	[TestFixture]
	class UploadChildIdsControllerTest
	{
		private UploadChildIdsController _controller;
		private MockChildDataCacheFactory _mockChildDataCacheFactory;
		private Mock<ILogger<UploadChildIdsController>> _mockLogger;
		private Mock<IChildDataCacheForWebFactory> _mockChildDataCacheForWebFactory;
		private Mock<IFormFile> _csvFile;
		private ApiResponse _apiResponse;
		private List<ChildInternalDataVM> _testChildList;
		[SetUp]
		public void SetUp()
		{
			_mockChildDataCacheFactory = new MockChildDataCacheFactory();
			_mockLogger = new Mock<ILogger<UploadChildIdsController>>();
			_mockChildDataCacheForWebFactory = new Mock<IChildDataCacheForWebFactory>();
			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, _mockChildDataCacheForWebFactory.Object, _mockLogger.Object);

			// ApiResponse
			_apiResponse = new ApiResponse();
			_testChildList = new List<ChildInternalDataVM>()
			{
				new ChildInternalDataVM()
				{
					Id=1,
					ChildId="TST-123456-0001",
					Name="SelectedNotSponsored",
					SelectedDateTime=new DateTime(2021,7,15,15,04,30),
					SponsoredDateTime=null
				},
				new ChildInternalDataVM()
				{
					Id=2,
					ChildId="TST-123456-0002",
					Name="Sponsored",
					SelectedDateTime=new DateTime(2021,7,15,15,04,30),
					SponsoredDateTime=new DateTime(2021,7,15,15,05,30)
				},
				new ChildInternalDataVM()
				{
					Id=3,
					ChildId="TST-123456-0003",
					Name="Available",
					SelectedDateTime=null,
					SponsoredDateTime=null
				}
			};
		}

		[Test]
		public void Upload_WhenCalledWithNoFile_ReturnsError()
		{
			//arrange
			UploadCsvVM _mockUploadModel = new UploadCsvVM() { CsvFile = null };

			//act
			var actionResult = _controller.Upload(_mockUploadModel) as ViewResult;

			UploadCsvVM model = (UploadCsvVM)actionResult.Model;

			// Assert          
			Assert.IsTrue(model.Error);
		}

		[Test]
		public void Upload_WhenCalledWithInvalidFileType_ReturnsError()
		{
			//arrange
			UploadCsvVM _mockUploadModel = new UploadCsvVM();
			SetUpMockedCsvFile("test.xml", "");
			_mockUploadModel.CsvFile = _csvFile.Object;

			//act
			var actionResult = _controller.Upload(_mockUploadModel) as ViewResult;

			UploadCsvVM model = (UploadCsvVM)actionResult.Model;

			// Assert          
			Assert.IsTrue(model.Error);
		}

		[Test]
		public void Upload_WhenCalledWithEmptyFile_ReturnsError()
		{
			//arrange
			UploadCsvVM _mockUploadModel = new UploadCsvVM();
			SetUpMockedCsvFile("test.csv", "");
			_mockUploadModel.CsvFile = _csvFile.Object;

			//act
			var actionResult = _controller.Upload(_mockUploadModel) as ViewResult;

			UploadCsvVM model = (UploadCsvVM)actionResult.Model;

			// Assert          
			Assert.IsTrue(model.Error);
			Assert.AreEqual(model.ErrorMessage, "No child ids found in this file.");
		}


		[Test]
		public void Upload_WhenCalledWithInvalidContent_ReturnsError()
		{
			//arrange
			UploadCsvVM _mockUploadModel = new UploadCsvVM();
			//Arrange
			SetUpMockedCsvFile("test.csv", "Hello World,how are you,some other data");
			_mockUploadModel.CsvFile = _csvFile.Object;

			//act
			var actionResult = _controller.Upload(_mockUploadModel) as ViewResult;
			UploadCsvVM model = (UploadCsvVM)actionResult.Model;

			// Assert          
			Assert.IsTrue(model.Error);
			Assert.AreEqual(model.ErrorMessage, "the child Id 'Hello World' at line number 1 does not match the format of a valid child Id<br/>");
		}

		[Test]
		public void Upload_WhenCalledWithSomeInvalidContent_ReturnsError()
		{
			//arrange
			UploadCsvVM _mockUploadModel = new UploadCsvVM();
			//Arrange
			//mock up a csv file with 2 lines of data, where 1st column is child id but 2nd line has invalid child Id
			SetUpMockedCsvFile("test.csv", "\"XYZ-123456-1234\",\"123\",\"F\"\r\n\"ABC-9876\",\"789\",\"M\"\r\n");
			_mockUploadModel.CsvFile = _csvFile.Object;

			//act
			var actionResult = _controller.Upload(_mockUploadModel) as ViewResult;
			UploadCsvVM model = (UploadCsvVM)actionResult.Model;

			// Assert          
			Assert.IsTrue(model.Error);
			Assert.AreEqual(model.ErrorMessage, "the child Id 'ABC-9876' at line number 2 does not match the format of a valid child Id<br/>");
		}

		[Test]
		public void Upload_WhenCalledWithSomeInvalidContentAndHeaderRow_ReturnsError()
		{
			//arrange
			UploadCsvVM _mockUploadModel = new UploadCsvVM() { HeaderRow = true };
			//Arrange
			//mock up a csv file with header row and 2 lines of data, where 1st column is child id but 2nd line has invalid child Id
			SetUpMockedCsvFile("test.csv", "\"Child_Id\",\"Code\",\"Gender\"\r\n\"XYZ-123456-1234\",\"123\",\"F\"\r\n\"ABC-9876\",\"789\",\"M\"\r\n");
			_mockUploadModel.CsvFile = _csvFile.Object;

			//act
			var actionResult = _controller.Upload(_mockUploadModel) as ViewResult;
			UploadCsvVM model = (UploadCsvVM)actionResult.Model;

			// Assert          
			Assert.IsTrue(model.Error);
			Assert.AreEqual(model.ErrorMessage, "the child Id 'ABC-9876' at line number 3 does not match the format of a valid child Id<br/>");
		}

		[Test]
		public void Upload_WhenCalledWithValidContent_ReturnsConfirmation()
		{
			//arrange
			UploadCsvVM _mockUploadModel = new UploadCsvVM();
			//Arrange
			//mock up a csv file with 2 lines of data, where 1st column is child id in the correct format
			SetUpMockedCsvFile("test.csv", "\"XYZ-123456-1234\",\"123\",\"F\"\r\n\"ABC-987654-9876\",\"789\",\"M\"\r\n");
			_mockUploadModel.CsvFile = _csvFile.Object;

			List<string> mockChildIdList = new List<string>() { "XYZ-123456-1234", "ABC-987654-9876" };

			var mockChildDataCacheFactory = new MockChildDataCacheFactory().MockUploadChildIds(mockChildIdList);

			_controller = new UploadChildIdsController(mockChildDataCacheFactory.Result.Object, _mockChildDataCacheForWebFactory.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.Upload(_mockUploadModel);
			var actionResponse = actionResult as RedirectToActionResult;

			// Assert          
			Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response              
			Assert.AreEqual(actionResponse.ActionName, "UploadProcessing");
			Assert.AreEqual(_mockLogger.Invocations.Count, 1);//ensure logging called
			Assert.AreEqual(mockChildDataCacheFactory.Result.Invocations.Count, 1);//ensure method called
		}

		[Test]
		public void Upload_WhenCalledWithValidContentAndHeaderRow_ReturnsConfirmation()
		{
			//arrange
			UploadCsvVM _mockUploadModel = new UploadCsvVM() { HeaderRow = true };
			//Arrange
			//mock up a csv file with header row and 2 lines of data
			SetUpMockedCsvFile("test.csv", "\"Child_Id\",\"Code\",\"Gender\"\r\n\"XYZ-123456-1234\",\"123\",\"F\"\r\n\"ABC-987654-1234\",\"789\",\"M\"\r\n");
			_mockUploadModel.CsvFile = _csvFile.Object;

			List<string> mockChildIdList = new List<string>() { "XYZ-123456-1234", "ABC-987654-1234" };

			var mockChildDataCacheFactory = new MockChildDataCacheFactory().MockUploadChildIds(mockChildIdList);

			_controller = new UploadChildIdsController(mockChildDataCacheFactory.Result.Object, _mockChildDataCacheForWebFactory.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.Upload(_mockUploadModel);
			var actionResponse = actionResult as RedirectToActionResult;

			// Assert          
			Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response              
			Assert.AreEqual(actionResponse.ActionName, "UploadProcessing");
			Assert.AreEqual(_mockLogger.Invocations.Count, 1);//ensure logging called
			Assert.AreEqual(mockChildDataCacheFactory.Result.Invocations.Count, 1);//ensure method called
		}

		[Test]
		public void Upload_WhenExceptionThrown_HandlesResult()
		{
			//arrange
			UploadCsvVM _mockUploadModel = new UploadCsvVM() { HeaderRow = true };
			//Arrange
			//mock up a csv file with header row and 2 lines of data
			SetUpMockedCsvFile("test.csv", "\"Child_Id\",\"Code\",\"Gender\"\r\n\"XYZ-123456-1234\",\"123\",\"F\"\r\n\"ABC-987654-1234\",\"789\",\"M\"\r\n");
			_mockUploadModel.CsvFile = _csvFile.Object;
			List<string> mockChildIdList = new List<string>() { "XYZ-123456-1234", "ABC-987654-1234" };

			var mockChildDataCacheFactory = new MockChildDataCacheFactory().MockUploadChildIdsThrowsException(mockChildIdList);

			_controller = new UploadChildIdsController(mockChildDataCacheFactory.Result.Object, _mockChildDataCacheForWebFactory.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.Upload(_mockUploadModel) as ViewResult;
			UploadCsvVM model = (UploadCsvVM)actionResult.Model;

			// Assert                      
			Assert.AreEqual(model.ErrorMessage, "There was an error uploading the child ids to the database.");
			Assert.IsTrue(model.Error);
		}

		[Test]
		public void UploadConfirmation_WhenCalled_ReturnsConfirmationMessage()
		{
			//arrange           
			ProcessingUpdateVM processingUpdateVM = new ProcessingUpdateVM()
			{
				Id = 1,
				ProcessingStarted = new DateTime(2020, 09, 14, 10, 5, 0),
				RecordsSubmitted = 10,
				RecordsCreated = 7,
				RecordsKept = 3,
				RecordsNotInUpload = 4,
				RecordsDeleted = 3,
				RecordsNotDeleted = 1,
				TotalRecordsAfterUpdate = 10,
				RecordsProcessed = 10,
				ProcessingFinished = new DateTime(2020, 09, 14, 10, 10, 0),
				ChildIdsNotPublishable = 0,
				ProjectRecordsCreated = 2,
				ProjectRecordsProcessed = 2,
				Narrative = String.Empty,
				Error = false
			};
			ApiResponse response = new ApiResponse() { ResponseObject = processingUpdateVM };


			var mockChildDataCacheFactory = new MockChildDataCacheFactory().MockGetProgressUpdateById(response, 1);

			_controller = new UploadChildIdsController(mockChildDataCacheFactory.Result.Object, _mockChildDataCacheForWebFactory.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.UploadConfirmed(1).Result as ViewResult;
			UploadCsvVM model = (UploadCsvVM)actionResult.Model;

			// Assert                      
			Assert.AreEqual(model.ConfirmationMessage, "CSV data uploaded on 14/09/2020 at 10:05<br/>" +
													   "Child Id records submitted: 10<br/>" +
													   "Child Ids from non web publishable countries: 0<br/>" +
													   "Child Id records created: 7<br/>" +
													   "Child Id records already in database and kept: 3<br/>" +
													   "Child Id records in database not found in submitted records: 4<br/>" +
													   "Child Id records not found in submitted records and deleted: 3<br/>" +
													   "Child Id records not found in submitted records and not deleted as they were in sponsorship process: 1<br/>" +
													   "Child Id records now in database: 10<br/>" +
													   "Area Programme records added to database: 2<br/>" +
													   "Child Id records updated from Horizon and RMT: 10<br/>" +
													   "Area Programme records in database and updated from Horizon and RMT: 2<br/>" +
													   "Processing completed 14/09/2020 at 10:10<br/>");
		}

		[Test]
		public void UploadConfirmation_WhenCalledWithErrors_ReturnsConfirmationMessageAndErrorMsg()
		{
			//arrange           
			ProcessingUpdateVM processingUpdateVM = new ProcessingUpdateVM()
			{
				Id = 3,
				ProcessingStarted = new DateTime(2020, 09, 14, 10, 5, 0),
				RecordsSubmitted = 10,
				RecordsCreated = 0,
				RecordsKept = 0,
				RecordsNotInUpload = 0,
				RecordsDeleted = 0,
				RecordsNotDeleted = 0,
				TotalRecordsAfterUpdate = 0,
				RecordsProcessed = 0,
				ProcessingFinished = new DateTime(2020, 09, 14, 10, 10, 0),
				ChildIdsNotPublishable = 0,
				Narrative = String.Empty,
				Error = true,
				ErrorMessage = "TEST ERROR"
			};
			ApiResponse response = new ApiResponse() { ResponseObject = processingUpdateVM };


			var mockChildDataCacheFactory = new MockChildDataCacheFactory().MockGetProgressUpdateById(response, 3);

			_controller = new UploadChildIdsController(mockChildDataCacheFactory.Result.Object, _mockChildDataCacheForWebFactory.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.UploadConfirmed(3).Result as ViewResult;
			UploadCsvVM model = (UploadCsvVM)actionResult.Model;

			// Assert                      
			Assert.AreEqual(model.ConfirmationMessage, "CSV data uploaded on 14/09/2020 at 10:05<br/>" +
													   "Child Id records submitted: 10<br/>" +
													   "Child Ids from non web publishable countries: 0<br/>" +
													   "Child Id records created: 0<br/>" +
													   "Child Id records already in database and kept: 0<br/>" +
													   "Child Id records in database not found in submitted records: 0<br/>" +
													   "Child Id records not found in submitted records and deleted: 0<br/>" +
													   "Child Id records not found in submitted records and not deleted as they were in sponsorship process: 0<br/>" +
													   "Child Id records now in database: 0<br/>" +
													   "Area Programme records added to database: 0<br/>" +
													   "Child Id records updated from Horizon and RMT: 0<br/>" +
													   "Area Programme records in database and updated from Horizon and RMT: 0<br/>" +
													   "Processing completed 14/09/2020 at 10:10<br/>");
			Assert.AreEqual(model.ErrorMessage, "TEST ERROR");
		}

		[Test]
		public void UploadConfirmation_WhenCalledIfProblems_ReturnsConfirmationMessageWithProblems()
		{
			//arrange           
			ProcessingUpdateVM processingUpdateVM = new ProcessingUpdateVM()
			{
				Id = 1,
				ProcessingStarted = new DateTime(2020, 09, 14, 10, 5, 0),
				RecordsSubmitted = 10,
				RecordsCreated = 7,
				RecordsKept = 3,
				RecordsNotInUpload = 4,
				RecordsDeleted = 3,
				RecordsNotDeleted = 1,
				TotalRecordsAfterUpdate = 10,
				RecordsProcessed = 10,
				ProcessingFinished = new DateTime(2020, 09, 14, 10, 10, 0),
				ChildIdsNotPublishable = 3,
				ProjectRecordsCreated = 2,
				ProjectRecordsProcessed = 2,
				Narrative = "Could not import Child ID LAO-123456-1234 as it is from a non web-publishable country.<br/>Could not import Child ID CHI-123456-1234 as it is from a non web-publishable country.<br/>Could not import Child ID JWG-123456-1234 as it is from a non web-publishable country.<br/>"
			};

			ApiResponse response = new ApiResponse() { ResponseObject = processingUpdateVM };

			var mockChildDataCacheFactory = new MockChildDataCacheFactory().MockGetProgressUpdateById(response, 1);

			_controller = new UploadChildIdsController(mockChildDataCacheFactory.Result.Object, _mockChildDataCacheForWebFactory.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.UploadConfirmed(1).Result as ViewResult;
			UploadCsvVM model = (UploadCsvVM)actionResult.Model;

			// Assert                      
			Assert.AreEqual(model.ConfirmationMessage, "CSV data uploaded on 14/09/2020 at 10:05<br/>" +
													   "Child Id records submitted: 10<br/>" +
													   "Child Ids from non web publishable countries: 3<br/>" +
													   "The following Child Ids were not uploaded as they are from non web-publishable countries:<br/>" +
													   "Could not import Child ID LAO-123456-1234 as it is from a non web-publishable country.<br/>" +
													   "Could not import Child ID CHI-123456-1234 as it is from a non web-publishable country.<br/>" +
													   "Could not import Child ID JWG-123456-1234 as it is from a non web-publishable country.<br/>" +
													   "Child Id records created: 7<br/>" +
													   "Child Id records already in database and kept: 3<br/>" +
													   "Child Id records in database not found in submitted records: 4<br/>" +
													   "Child Id records not found in submitted records and deleted: 3<br/>" +
													   "Child Id records not found in submitted records and not deleted as they were in sponsorship process: 1<br/>" +
													   "Child Id records now in database: 10<br/>" +
													   "Area Programme records added to database: 2<br/>" +
													   "Child Id records updated from Horizon and RMT: 10<br/>" +
													   "Area Programme records in database and updated from Horizon and RMT: 2<br/>" +
													   "Processing completed 14/09/2020 at 10:10<br/>");
		}

		[Test]
		public void UploadConfirmation_WhenCalledWithNewProjects_ReturnsConfirmationMessageWithNewProjects()
		{
			//arrange           
			ProcessingUpdateVM processingUpdateVM = new ProcessingUpdateVM()
			{
				Id = 1,
				ProcessingStarted = new DateTime(2020, 09, 14, 10, 5, 0),
				RecordsSubmitted = 10,
				RecordsCreated = 7,
				RecordsKept = 3,
				RecordsNotInUpload = 4,
				RecordsDeleted = 3,
				RecordsNotDeleted = 1,
				TotalRecordsAfterUpdate = 10,
				RecordsProcessed = 10,
				ProcessingFinished = new DateTime(2020, 09, 14, 10, 10, 0),
				ChildIdsNotPublishable = 0,
				ProjectRecordsCreated = 2,
				ProjectRecordsProcessed = 2,
				NewProjects = "178422, Domue Sponsorship Management Project|188040,Kazuzo Sponsorship Management Project|"
			};

			ApiResponse response = new ApiResponse() { ResponseObject = processingUpdateVM };

			var mockChildDataCacheFactory = new MockChildDataCacheFactory().MockGetProgressUpdateById(response, 1);

			_controller = new UploadChildIdsController(mockChildDataCacheFactory.Result.Object, _mockChildDataCacheForWebFactory.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.UploadConfirmed(1).Result as ViewResult;
			UploadCsvVM model = (UploadCsvVM)actionResult.Model;

			// Assert                      
			Assert.AreEqual(model.ConfirmationMessage, "CSV data uploaded on 14/09/2020 at 10:05<br/>" +
													   "Child Id records submitted: 10<br/>" +
													   "Child Ids from non web publishable countries: 0<br/>" +
													   "Child Id records created: 7<br/>" +
													   "Child Id records already in database and kept: 3<br/>" +
													   "Child Id records in database not found in submitted records: 4<br/>" +
													   "Child Id records not found in submitted records and deleted: 3<br/>" +
													   "Child Id records not found in submitted records and not deleted as they were in sponsorship process: 1<br/>" +
													   "Child Id records now in database: 10<br/>" +
													   "Area Programme records added to database: 2<br/>" +
													   "Child Id records updated from Horizon and RMT: 10<br/>" +
													   "Area Programme records in database and updated from Horizon and RMT: 2<br/>" +
													   "</br>New Area Programmes added to database (please add data in Umbraco):</br>" +
													   "178422, Domue Sponsorship Management Project<br/>" +
													   "188040,Kazuzo Sponsorship Management Project<br/><br/>" +
													   "Processing completed 14/09/2020 at 10:10<br/>");
		}

		[Test]
		public void GetProgress_WhenCalled_ReturnsProcessingInfo()
		{
			//arrange    
			_apiResponse = new ApiResponse()
			{
				Message = "",
				ResponseObject = new ProcessingUpdateVM()
				{
					Id = 2,
					ProcessingStarted = new DateTime(2020, 09, 20, 07, 55, 30),
					RecordsSubmitted = 50,
					RecordsCreated = 30,
					RecordsKept = 20,
					RecordsNotInUpload = 10,
					RecordsDeleted = 7,
					RecordsNotDeleted = 3,
					TotalRecordsAfterUpdate = 50,
					RecordsProcessed = 50,
					ProcessingFinished = new DateTime(2020, 09, 14, 08, 00, 45)
				},
				Success = true
			};

			var mockChildDataCacheFactory = new MockChildDataCacheFactory().MockGetProgressUpdateById(_apiResponse, 2);

			_controller = new UploadChildIdsController(mockChildDataCacheFactory.Result.Object, _mockChildDataCacheForWebFactory.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.GetProgress(2).Result as JsonResult;
			var processingUpdate = JsonConvert.DeserializeObject<ProcessingUpdateVM>(JsonConvert.DeserializeObject<string>(actionResult.Value.ToString()));

			// Assert                      
			Assert.AreEqual(processingUpdate.ProcessingStarted, new DateTime(2020, 09, 20, 07, 55, 30));
			Assert.AreEqual(processingUpdate.RecordsSubmitted, 50);
			Assert.AreEqual(processingUpdate.RecordsCreated, 30);
			Assert.AreEqual(processingUpdate.RecordsKept, 20);
			Assert.AreEqual(processingUpdate.RecordsNotInUpload, 10);
			Assert.AreEqual(processingUpdate.RecordsDeleted, 7);
			Assert.AreEqual(processingUpdate.RecordsNotDeleted, 3);
			Assert.AreEqual(processingUpdate.TotalRecordsAfterUpdate, 50);
			Assert.AreEqual(processingUpdate.RecordsProcessed, 50);
			Assert.AreEqual(processingUpdate.ProcessingFinished, new DateTime(2020, 09, 14, 08, 00, 45));
		}

		[Test]
		public void GetProgress_WhenExceptionThrown_HandlesResult()
		{
			//arrange                     
			var mockChildDataCacheFactory = new MockChildDataCacheFactory().MockGetProgressUpdateByIdThrowsException(1);

			_controller = new UploadChildIdsController(mockChildDataCacheFactory.Result.Object, _mockChildDataCacheForWebFactory.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.GetProgress(1).Result as JsonResult;
			var jsonResult = JsonConvert.DeserializeObject(Convert.ToString(actionResult.Value));

			// Assert                      
			Assert.AreEqual(_mockLogger.Invocations.Count, 1);//ensure error has been logged
			Assert.AreEqual(jsonResult, "Error");
		}

		[Test]
		public void Upload_WhenAlreadyProcessing_InformsUser()
		{
			//arrange                 
			_apiResponse = new ApiResponse()
			{
				Message = "",
				ResponseObject = new ProcessingUpdateVM()
				{
					Id = 2,
					ProcessingStarted = new DateTime(2020, 09, 20, 07, 55, 30),
					RecordsSubmitted = 50,
					RecordsCreated = 30,
					RecordsKept = 20,
					RecordsNotInUpload = 10,
					RecordsDeleted = 7,
					RecordsNotDeleted = 3,
					TotalRecordsAfterUpdate = 50,
					RecordsProcessed = 50
				},
				Success = true
			};
			var mockChildDataCacheFactory = new MockChildDataCacheFactory().MockGetProgressUpdate(_apiResponse);

			_controller = new UploadChildIdsController(mockChildDataCacheFactory.Result.Object, _mockChildDataCacheForWebFactory.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.Upload().Result as ViewResult;
			UploadCsvVM model = (UploadCsvVM)actionResult.Model;

			// Assert          
			Assert.IsTrue(model.Error);
			Assert.AreEqual(model.ErrorMessage, "There is already a file being processed which has not finished yet."+
											   "<br/> Processing started on 20-09-2020 at 07:55");
			Assert.IsFalse(model.ShowInputs);
		}

		[Test]
		public void Upload_WhenNotAlreadyProcessing_AllowsInputs()
		{
			//arrange                 
			_apiResponse = new ApiResponse()
			{
				Message = "",
				ResponseObject = null,
				Success = true
			};
			var mockChildDataCacheFactory = new MockChildDataCacheFactory().MockGetProgressUpdate(_apiResponse);

			_controller = new UploadChildIdsController(mockChildDataCacheFactory.Result.Object, _mockChildDataCacheForWebFactory.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.Upload().Result as ViewResult;
			UploadCsvVM model = (UploadCsvVM)actionResult.Model;

			// Assert          
			Assert.False(model.Error);
			Assert.IsTrue(model.ShowInputs);
		}

		[Test]
		public void UploadProcessing_WhenFindsRecordInProgress_SetsProgressInfo()
		{
			//arrange                 
			_apiResponse = new ApiResponse()
			{
				Message = "",
				ResponseObject = new ProcessingUpdateVM()
				{
					Id = 2,
					ProcessingStarted = new DateTime(2020, 09, 20, 07, 55, 30),
					RecordsSubmitted = 50,
					RecordsCreated = 30,
					RecordsKept = 20,
					RecordsNotInUpload = 10,
					RecordsDeleted = 7,
					RecordsNotDeleted = 3,
					TotalRecordsAfterUpdate = 50,
					RecordsProcessed = 50
				},
				Success = true
			};
			var mockChildDataCacheFactory = new MockChildDataCacheFactory().MockGetProgressUpdate(_apiResponse);

			_controller = new UploadChildIdsController(mockChildDataCacheFactory.Result.Object, _mockChildDataCacheForWebFactory.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.UploadProcessing().Result as ViewResult;
			UploadCsvVM model = (UploadCsvVM)actionResult.Model;

			// Assert          
			Assert.IsNotNull(model);
			Assert.AreEqual(model.ConfirmationMessage, "07:55");//displays when the processing started
			Assert.AreEqual(model.ProgressId, 2);//this is the id that enables the recurring get to be sent from the jquery
		}

		[Test]
		public void UploadProcessing_WhenCantFindRecordInProgress_ShowsError()
		{
			//arrange                 
			_apiResponse = new ApiResponse()
			{
				Message = "",
				ResponseObject = null,
				Success = true
			};
			var mockChildDataCacheFactory = new MockChildDataCacheFactory().MockGetProgressUpdate(_apiResponse);

			_controller = new UploadChildIdsController(mockChildDataCacheFactory.Result.Object, _mockChildDataCacheForWebFactory.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.UploadProcessing().Result as ViewResult;
			UploadCsvVM model = (UploadCsvVM)actionResult.Model;

			// Assert          
			Assert.IsNotNull(model);
			Assert.IsTrue(model.Error);
			Assert.AreEqual(model.ErrorMessage, "Searching for processing updates; please refresh this screen or press F5");
		}

		[Test]
		public void UploadProcessing_WhenExceptionThrown_ShowsError()
		{
			//arrange                             
			var mockChildDataCacheFactory = new MockChildDataCacheFactory().MockGetProgressUpdateThrowsException();

			_controller = new UploadChildIdsController(mockChildDataCacheFactory.Result.Object, _mockChildDataCacheForWebFactory.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.UploadProcessing().Result as ViewResult;
			UploadCsvVM model = (UploadCsvVM)actionResult.Model;

			// Assert          
			Assert.IsNotNull(model);
			Assert.IsTrue(model.Error);
			Assert.AreEqual(model.ErrorMessage, "There has been an error accessing the progress information on this upload.");
		}

		private void SetUpMockedCsvFile(string FileName, string FileContent)
		{
			_csvFile = new Mock<IFormFile>();
			//Setup mock file using a memory stream
			var content = FileContent;
			var fileName = FileName;
			var ms = new MemoryStream();
			var writer = new StreamWriter(ms);
			writer.Write(content);
			writer.Flush();
			ms.Position = 0;
			_csvFile.Setup(_ => _.OpenReadStream()).Returns(ms);
			_csvFile.Setup(_ => _.FileName).Returns(fileName);
			_csvFile.Setup(_ => _.Length).Returns(ms.Length);
		}

		[Test]
		public void ChildList_WhenCalledWithNoPageNumber_ReturnsPageExpectedViewModel()
		{
			//arrange                 
			ChildInternalDataPagedVM childInternalDataPagedVM = new ChildInternalDataPagedVM();
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockGetChildDataPaged(childInternalDataPagedVM, 1, 50, 347);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.ChildList(null, "").Result as ViewResult;
			ChildInternalDataPagedVM model = (ChildInternalDataPagedVM)actionResult.Model;

			// Assert          
			Assert.IsNotNull(model);
			Assert.IsInstanceOf(typeof(ChildInternalDataPagedVM), actionResult.Model);
			Assert.AreEqual(50, model.ChildInternalDataModels.Count);
			Assert.AreEqual(50, model.CurrentCount);
			Assert.AreEqual(50, model.PageSize);
			Assert.AreEqual(1, model.CurrentPage);
			Assert.AreEqual(347, model.TotalNumber);
			Assert.AreEqual(7, model.TotalPages);
			Assert.IsFalse(model.PreviousPage);
			Assert.IsTrue(model.NextPage);
		}

		[Test]
		public void ChildList_WhenCalledWithSpecifiedPageNumber_ReturnsPageExpectedViewModel()
		{
			//arrange                 
			ChildInternalDataPagedVM childInternalDataPagedVM = new ChildInternalDataPagedVM();
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockGetChildDataPaged(childInternalDataPagedVM, 5, 50, 347);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.ChildList(5, "").Result as ViewResult;
			ChildInternalDataPagedVM model = (ChildInternalDataPagedVM)actionResult.Model;

			// Assert          
			Assert.IsNotNull(model);
			Assert.IsInstanceOf(typeof(ChildInternalDataPagedVM), actionResult.Model);
			Assert.AreEqual(50, model.ChildInternalDataModels.Count);
			Assert.AreEqual(50, model.CurrentCount);
			Assert.AreEqual(50, model.PageSize);
			Assert.AreEqual(5, model.CurrentPage);
			Assert.AreEqual(347, model.TotalNumber);
			Assert.AreEqual(7, model.TotalPages);
			Assert.IsTrue(model.PreviousPage);
			Assert.IsTrue(model.NextPage);
		}

		[Test]
		public void ChildList_WhenCalledWithLastPageNumber_ReturnsPageExpectedViewModel()
		{
			//arrange                 
			ChildInternalDataPagedVM childInternalDataPagedVM = new ChildInternalDataPagedVM();
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockGetChildDataPaged(childInternalDataPagedVM, 7, 50, 347);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.ChildList(7, "").Result as ViewResult;
			ChildInternalDataPagedVM model = (ChildInternalDataPagedVM)actionResult.Model;

			// Assert          
			Assert.IsNotNull(model);
			Assert.IsInstanceOf(typeof(ChildInternalDataPagedVM), actionResult.Model);
			Assert.AreEqual(50, model.ChildInternalDataModels.Count);
			Assert.AreEqual(50, model.CurrentCount);
			Assert.AreEqual(50, model.PageSize);
			Assert.AreEqual(7, model.CurrentPage);
			Assert.AreEqual(347, model.TotalNumber);
			Assert.AreEqual(7, model.TotalPages);
			Assert.IsTrue(model.PreviousPage);
			Assert.IsFalse(model.NextPage);
		}

		[Test]
		public void ChildList_WhenCalledWithNoChildRecords_ReturnsPageExpectedViewModel()
		{
			//arrange                 
			ChildInternalDataPagedVM childInternalDataPagedVM = new ChildInternalDataPagedVM();
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockGetChildDataPaged(childInternalDataPagedVM, 1, 50, 0);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.ChildList(1, "").Result as ViewResult;
			ChildInternalDataPagedVM model = (ChildInternalDataPagedVM)actionResult.Model;

			// Assert          
			Assert.IsNotNull(model);
			Assert.IsInstanceOf(typeof(ChildInternalDataPagedVM), actionResult.Model);
			Assert.AreEqual(0, model.ChildInternalDataModels.Count);
			Assert.AreEqual(0, model.CurrentCount);
			Assert.AreEqual(50, model.PageSize);
			Assert.AreEqual(1, model.CurrentPage);
			Assert.AreEqual(0, model.TotalNumber);
			Assert.AreEqual(0, model.TotalPages);
			Assert.IsFalse(model.PreviousPage);
			Assert.IsFalse(model.NextPage);
		}

		[Test]
		public void ChildList_WhenCalledWithChildRecordCountLessThanPageSize_ReturnsPageExpectedViewModel()
		{
			//arrange                 
			ChildInternalDataPagedVM childInternalDataPagedVM = new ChildInternalDataPagedVM();
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockGetChildDataPaged(childInternalDataPagedVM, 1, 50, 16);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.ChildList(1, "").Result as ViewResult;
			ChildInternalDataPagedVM model = (ChildInternalDataPagedVM)actionResult.Model;

			// Assert          
			Assert.IsNotNull(model);
			Assert.IsInstanceOf(typeof(ChildInternalDataPagedVM), actionResult.Model);
			Assert.AreEqual(16, model.ChildInternalDataModels.Count);
			Assert.AreEqual(16, model.CurrentCount);
			Assert.AreEqual(50, model.PageSize);
			Assert.AreEqual(1, model.CurrentPage);
			Assert.AreEqual(16, model.TotalNumber);
			Assert.AreEqual(1, model.TotalPages);
			Assert.IsFalse(model.PreviousPage);
			Assert.IsFalse(model.NextPage);
		}

		[Test]
		public void ChildList_WhenExceptionThrown_HandlesResult()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockGetChildDataPagedThrowsException(1, 50);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.ChildList(null, "");
			var actionResponse = actionResult.Result as RedirectToActionResult;

			// Assert                      
			Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response   
			Assert.AreEqual(1, _mockLogger.Invocations.Count);//ensure logging called
			Assert.AreEqual("Home", actionResponse.ControllerName);
			Assert.AreEqual("Error", actionResponse.ActionName);
		}

		[Test]
		public void DeleteLoad_WhenCalledWithAvailableId_DoesNotLockDelete()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockGetChildLoad(_testChildList, 3);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.Delete(3).Result as ViewResult;
			ChildInternalDataVM model = (ChildInternalDataVM)actionResult.Model;

			// Assert                      
			Assert.IsInstanceOf(typeof(ChildInternalDataVM), actionResult.Model); //Test if returns ok response   
			Assert.IsNotNull(model);
			Assert.IsFalse(model.CannotDelete);
			Assert.AreEqual(0, actionResult.ViewData.ModelState.ErrorCount);
		}

		[Test]
		public void DeleteLoad_WhenCalledWithSelectedId_DoesLockDelete()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockGetChildLoad(_testChildList, 1);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.Delete(1).Result as ViewResult;
			ChildInternalDataVM model = (ChildInternalDataVM)actionResult.Model;

			// Assert                      
			Assert.IsInstanceOf(typeof(ChildInternalDataVM), actionResult.Model); //Test if returns ok response   
			Assert.IsNotNull(model);
			Assert.IsTrue(model.CannotDelete);
			Assert.AreEqual(1, actionResult.ViewData.ModelState.ErrorCount);
		}

		[Test]
		public void DeleteLoad_WhenCalledWithSponsoredId_DoesLockDelete()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockGetChildLoad(_testChildList, 2);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.Delete(2).Result as ViewResult;
			ChildInternalDataVM model = (ChildInternalDataVM)actionResult.Model;

			// Assert                      
			Assert.IsInstanceOf(typeof(ChildInternalDataVM), actionResult.Model); //Test if returns ok response   
			Assert.IsNotNull(model);
			Assert.IsTrue(model.CannotDelete);
			Assert.AreEqual(1, actionResult.ViewData.ModelState.ErrorCount);
		}

		[Test]
		public void DeleteLoad_WhenCalledWithUnknownId_ReturnsNotFound()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockGetChildLoad(_testChildList, 9);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.Delete(9).Result as NotFoundResult;

			// Assert                      
			Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);
		}

		[Test]
		public void DeleteLoad_WhenExceptionThrown_HandlesResult()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockLoadThrowsException(2);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.Delete(2);
			var actionResponse = actionResult.Result as RedirectToActionResult;

			// Assert                      
			Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response   
			Assert.AreEqual(1, _mockLogger.Invocations.Count);//ensure logging called
			Assert.AreEqual("Home", actionResponse.ControllerName);
			Assert.AreEqual("Error", actionResponse.ActionName);
		}

		[Test]
		public void DeleteConfirmed_WhenCalledWithAvailableId_DeletesAndRedirects()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockDeleteChildRecord(_testChildList, 3);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.DeleteConfirmed(3).Result as RedirectToActionResult;

			// Assert                      
			Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResult); //Test if returns ok response   
			Assert.AreEqual("ChildList", actionResult.ActionName);
			Assert.IsTrue(actionResult.RouteValues.Keys.Contains("notification"));
			Assert.AreEqual("Deletion successful", actionResult.RouteValues.GetValueOrDefault("notification"));
			Assert.AreEqual(2, _testChildList.Count);
		}


		[Test]
		public void DeleteConfirmed_WhenCalledWithSelectedId_DoesNotDeleteShowsError()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockDeleteChildRecord(_testChildList, 1);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.DeleteConfirmed(1).Result as ViewResult;
			ChildInternalDataVM model = (ChildInternalDataVM)actionResult.Model;

			// Assert                      
			Assert.IsInstanceOf(typeof(ChildInternalDataVM), actionResult.Model); //Test if returns ok response   
			Assert.IsNotNull(model);
			Assert.IsTrue(model.CannotDelete);
			Assert.AreEqual(1, actionResult.ViewData.ModelState.ErrorCount);
		}

		[Test]
		public void DeleteConfirmed_WhenCalledWithSponsoredId_DoesNotDeleteShowsError()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockDeleteChildRecord(_testChildList, 2);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.DeleteConfirmed(2).Result as ViewResult;
			ChildInternalDataVM model = (ChildInternalDataVM)actionResult.Model;

			// Assert                      
			Assert.IsInstanceOf(typeof(ChildInternalDataVM), actionResult.Model); //Test if returns ok response   
			Assert.IsNotNull(model);
			Assert.IsTrue(model.CannotDelete);
			Assert.AreEqual(1, actionResult.ViewData.ModelState.ErrorCount);
		}

		[Test]
		public void DeleteConfirmed_WhenCalledWithUnknownId_DoesNotDeleteShowsError()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockDeleteChildRecord(_testChildList, 9);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.DeleteConfirmed(9).Result as ViewResult;
			ChildInternalDataVM model = (ChildInternalDataVM)actionResult.Model;

			// Assert                      			
			Assert.IsNull(model);
			Assert.AreEqual(1, actionResult.ViewData.ModelState.ErrorCount);
		}

		[Test]
		public void DeleteConfirm_WhenExceptionThrown_HandlesResult()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockDeleteConfirmThrowsException(_testChildList, 2);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.DeleteConfirmed(2).Result as ViewResult;
			ChildInternalDataVM model = (ChildInternalDataVM)actionResult.Model;

			// Assert                      
			Assert.IsInstanceOf(typeof(ChildInternalDataVM), actionResult.Model); //Test if returns ok response   
			Assert.AreEqual(1, _mockLogger.Invocations.Count);//ensure logging called
			Assert.AreEqual(1, actionResult.ViewData.ModelState.ErrorCount);
		}

		[Test]
		public void ChangeStatusLoad_WhenCalledWithAvailableId_DoesNotLockRecord()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockGetChildLoad(_testChildList, 3);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.ChangeStatus(3).Result as ViewResult;
			ChildInternalDataVM model = (ChildInternalDataVM)actionResult.Model;

			// Assert                      
			Assert.IsInstanceOf(typeof(ChildInternalDataVM), actionResult.Model); //Test if returns ok response   
			Assert.IsNotNull(model);
			Assert.IsFalse(model.CannotChangeStatus);
			Assert.AreEqual(0, actionResult.ViewData.ModelState.ErrorCount);
		}

		[Test]
		public void ChangeStatusLoad_WhenCalledWithSelectedId_DoesLockRecord()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockGetChildLoad(_testChildList, 1);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.ChangeStatus(1).Result as ViewResult;
			ChildInternalDataVM model = (ChildInternalDataVM)actionResult.Model;

			// Assert                      
			Assert.IsInstanceOf(typeof(ChildInternalDataVM), actionResult.Model); //Test if returns ok response   
			Assert.IsNotNull(model);
			Assert.IsTrue(model.CannotChangeStatus);
			Assert.AreEqual(1, actionResult.ViewData.ModelState.ErrorCount);
		}

		[Test]
		public void ChangeStatusLoad_WhenCalledWithSponsoredId_DoesLockRecord()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockGetChildLoad(_testChildList, 2);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.ChangeStatus(2).Result as ViewResult;
			ChildInternalDataVM model = (ChildInternalDataVM)actionResult.Model;

			// Assert                      
			Assert.IsInstanceOf(typeof(ChildInternalDataVM), actionResult.Model); //Test if returns ok response   
			Assert.IsNotNull(model);
			Assert.IsTrue(model.CannotChangeStatus);
			Assert.AreEqual(1, actionResult.ViewData.ModelState.ErrorCount);
		}

		[Test]
		public void ChangeStatusLoad_WhenCalledWithUnknownId_ReturnsNotFound()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockGetChildLoad(_testChildList, 9);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.ChangeStatus(9).Result as NotFoundResult;

			// Assert                      
			Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);
		}

		[Test]
		public void ChangeStatusLoad_WhenExceptionThrown_HandlesResult()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockLoadThrowsException(2);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.ChangeStatus(2);
			var actionResponse = actionResult.Result as RedirectToActionResult;

			// Assert                      
			Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResponse); //Test if returns ok response   
			Assert.AreEqual(1, _mockLogger.Invocations.Count);//ensure logging called
			Assert.AreEqual("Home", actionResponse.ControllerName);
			Assert.AreEqual("Error", actionResponse.ActionName);
		}

		[Test]
		public void ChangeStatusConfirmed_WhenCalledWithAvailableId_ChangeStatusAndRedirects()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockSponsorChildRecord(_testChildList, 3);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.ChangeStatusConfirmed(3).Result as RedirectToActionResult;

			// Assert                      
			Assert.IsInstanceOf(typeof(RedirectToActionResult), actionResult); //Test if returns ok response   
			Assert.AreEqual("ChildList", actionResult.ActionName);
			Assert.IsTrue(actionResult.RouteValues.Keys.Contains("notification"));
			Assert.AreEqual("Child Sponsored successfully", actionResult.RouteValues.GetValueOrDefault("notification"));
		}


		[Test]
		public void ChangeStatusConfirmed_WhenCalledWithSelectedId_DoesNotChangeStatusShowsError()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockSponsorChildRecord(_testChildList, 1);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.ChangeStatusConfirmed(1).Result as ViewResult;
			ChildInternalDataVM model = (ChildInternalDataVM)actionResult.Model;

			// Assert                      
			Assert.IsInstanceOf(typeof(ChildInternalDataVM), actionResult.Model); //Test if returns ok response   
			Assert.IsNotNull(model);
			Assert.IsTrue(model.CannotChangeStatus);
			Assert.AreEqual(1, actionResult.ViewData.ModelState.ErrorCount);
		}

		[Test]
		public void ChangeStatusConfirmed_WhenCalledWithSponsoredId_DoesNotChangeStatusShowsError()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockSponsorChildRecord(_testChildList, 2);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.ChangeStatusConfirmed(2).Result as ViewResult;
			ChildInternalDataVM model = (ChildInternalDataVM)actionResult.Model;

			// Assert                      
			Assert.IsInstanceOf(typeof(ChildInternalDataVM), actionResult.Model); //Test if returns ok response   
			Assert.IsNotNull(model);
			Assert.IsTrue(model.CannotChangeStatus);
			Assert.AreEqual(1, actionResult.ViewData.ModelState.ErrorCount);
		}

		[Test]
		public void ChangeStatusConfirmed_WhenCalledWithUnknownId_DoesNotChangeStatusShowsError()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockSponsorChildRecord(_testChildList, 9);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.ChangeStatus(9).Result as NotFoundResult;

			// Assert                      			
			Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);
		}

		[Test]
		public void ChangeStatusConfirm_WhenExceptionThrown_HandlesResult()
		{
			//Arrange		
			var mockChildDataCacheForWebFactory = new MockChildDataCacheForWebFactory().MockChangeStatusConfirmThrowsException(_testChildList, 2);

			_controller = new UploadChildIdsController(_mockChildDataCacheFactory.Object, mockChildDataCacheForWebFactory.Result.Object, _mockLogger.Object);
			//act
			var actionResult = _controller.ChangeStatusConfirmed(2).Result as ViewResult;
			ChildInternalDataVM model = (ChildInternalDataVM)actionResult.Model;

			// Assert                      
			Assert.IsInstanceOf(typeof(ChildInternalDataVM), actionResult.Model); //Test if returns ok response   
			Assert.AreEqual(1, _mockLogger.Invocations.Count);//ensure logging called
			Assert.AreEqual(1, actionResult.ViewData.ModelState.ErrorCount);
		}
	}
}
