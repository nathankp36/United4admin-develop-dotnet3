using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using United4Admin.WebApplication.APIClient;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.BLL;
using United4Admin.WebApplication.ViewModels;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using United4Admin.WebApplication.Models;

namespace United4Admin.WebApplication.Controllers
{
	[Authorize]
	public class UploadChildIdsController : Controller
	{
		private readonly IChildDataCacheFactory _childDataCacheFactory;
		private readonly IChildDataCacheForWebFactory _childDataCacheForWebFactory;
		private readonly ILogger<UploadChildIdsController> _logger;
		private readonly int _pageSize = 50; //for paging the child List page
		public UploadChildIdsController(IChildDataCacheFactory childDataCacheFactory, IChildDataCacheForWebFactory childDataCacheForWebFactory, ILogger<UploadChildIdsController> logger)
		{
			_childDataCacheFactory = childDataCacheFactory;
			_childDataCacheForWebFactory = childDataCacheForWebFactory;
			_logger = logger;
		}

		public async Task<ActionResult> Upload()
		{
			ApiResponse response = new ApiResponse();
			UploadCsvVM uploadCsvVM = new UploadCsvVM() { ShowInputs = true };
			try
			{
				response = await _childDataCacheFactory.GetUpdateProgress();//selects the one in progress (no finish time)
				ProcessingUpdateVM prog = JsonConvert.DeserializeObject<ProcessingUpdateVM>(Convert.ToString(response.ResponseObject));
				if (prog != null)
				{
					uploadCsvVM.Error = true;
					uploadCsvVM.ErrorMessage = "There is already a file being processed which has not finished yet." +
											   "<br/> Processing started on " + prog.ProcessingStarted.ToShortDateString() + " at " + prog.ProcessingStarted.ToShortTimeString();
					uploadCsvVM.ShowInputs = false;
				}
			}
			catch (Exception ex)
			{
				_logger.LogInformation(ConstantMessages.UploadChildIdsFailure + "Exception Message: " + ex.Message);
				response.ResponseObject = "Error";
			}
			return View(uploadCsvVM);
		}
		public async Task<ActionResult> UploadConfirmed(int ProcessingId)
		{
			UploadCsvVM uploadCsvVM = new UploadCsvVM();
			try
			{
				ApiResponse response = await _childDataCacheFactory.GetUpdateProgressById(ProcessingId);

				ProcessingUpdateVM processingUpdate = JsonConvert.DeserializeObject<ProcessingUpdateVM>(Convert.ToString(response.ResponseObject));

				uploadCsvVM.ConfirmationMessage = "CSV data uploaded on " + processingUpdate.ProcessingStarted.ToShortDateString() + " at " + processingUpdate.ProcessingStarted.ToShortTimeString() + "<br/>";
				uploadCsvVM.ConfirmationMessage += "Child Id records submitted: " + processingUpdate.RecordsSubmitted + "<br/>";
				uploadCsvVM.ConfirmationMessage += "Child Ids from non web publishable countries: " + processingUpdate.ChildIdsNotPublishable + "<br/>";
				if (processingUpdate.ChildIdsNotPublishable > 0)
				{
					uploadCsvVM.ConfirmationMessage += "The following Child Ids were not uploaded as they are from non web-publishable countries:<br/>";
					uploadCsvVM.ConfirmationMessage += processingUpdate.Narrative;
				}
				uploadCsvVM.ConfirmationMessage += "Child Id records created: " + processingUpdate.RecordsCreated + "<br/>";
				uploadCsvVM.ConfirmationMessage += "Child Id records already in database and kept: " + processingUpdate.RecordsKept + "<br/>";
				uploadCsvVM.ConfirmationMessage += "Child Id records in database not found in submitted records: " + processingUpdate.RecordsNotInUpload + "<br/>";
				uploadCsvVM.ConfirmationMessage += "Child Id records not found in submitted records and deleted: " + processingUpdate.RecordsDeleted + "<br/>";
				uploadCsvVM.ConfirmationMessage += "Child Id records not found in submitted records and not deleted as they were in sponsorship process: " + processingUpdate.RecordsNotDeleted + "<br/>";
				uploadCsvVM.ConfirmationMessage += "Child Id records now in database: " + processingUpdate.TotalRecordsAfterUpdate + "<br/>";
				uploadCsvVM.ConfirmationMessage += "Area Programme records added to database: " + processingUpdate.ProjectRecordsCreated + "<br/>";
				uploadCsvVM.ConfirmationMessage += "Child Id records updated from Horizon and RMT: " + processingUpdate.RecordsProcessed + "<br/>";
				uploadCsvVM.ConfirmationMessage += "Area Programme records in database and updated from Horizon and RMT: " + processingUpdate.ProjectRecordsProcessed + "<br/>";
				if (!string.IsNullOrEmpty(processingUpdate.NewProjects))
				{
					uploadCsvVM.ConfirmationMessage += "</br>New Area Programmes added to database (please add data in Umbraco):</br>";
					string[] newProjects = processingUpdate.NewProjects.Split("|");
					foreach (var newProject in newProjects)
					{
						uploadCsvVM.ConfirmationMessage += newProject + "<br/>";
					}
				}
				if (processingUpdate.ProcessingFinished != null)
				{ uploadCsvVM.ConfirmationMessage += "Processing completed " + processingUpdate.ProcessingFinished.Value.ToShortDateString() + " at " + processingUpdate.ProcessingFinished.Value.ToShortTimeString() + "<br/>"; }
				if (processingUpdate.Error)
				{ uploadCsvVM.ErrorMessage = processingUpdate.ErrorMessage; }
			}
			catch (Exception ex)
			{
				_logger.LogInformation(ConstantMessages.UploadChildIdsFailure + "Exception Message: " + ex.Message);
				uploadCsvVM.ErrorMessage = "Processing data not found";
			}
			return View(uploadCsvVM);
		}

		public async Task<ActionResult> UploadProcessing()
		{
			ApiResponse response = new ApiResponse();
			UploadCsvVM uploadCsvVM = new UploadCsvVM();
			try
			{
				response = await _childDataCacheFactory.GetUpdateProgress();//selects the one in progress (no finish time)
				ProcessingUpdateVM prog = JsonConvert.DeserializeObject<ProcessingUpdateVM>(Convert.ToString(response.ResponseObject));
				if (prog != null)
				{
					uploadCsvVM.ProgressId = prog.Id;
					uploadCsvVM.ConfirmationMessage = prog.ProcessingStarted.ToShortTimeString();
				}
				else
				{
					//not found - show error
					uploadCsvVM.Error = true;
					uploadCsvVM.ErrorMessage = "Searching for processing updates; please refresh this screen or press F5";
				}
			}
			catch (Exception ex)
			{
				_logger.LogInformation(ConstantMessages.UploadChildIdsFailure + "Exception Message: " + ex.Message);
				uploadCsvVM.Error = true;
				uploadCsvVM.ErrorMessage = "There has been an error accessing the progress information on this upload.";
			}
			return View(uploadCsvVM);
		}


		[HttpPost]
		public ActionResult Upload(UploadCsvVM UploadCsvVM)
		{
			if (ModelState.IsValid)
			{
				UploadCsvVM.ErrorMessage = "The file submitted does not appear to be a .csv file.";
				UploadCsvVM.Error = true;
				//validate - is this a csv file?
				try
				{
					string fileExtension = UploadCsvVM.CsvFile.FileName.Substring(UploadCsvVM.CsvFile.FileName.Length - 4, 4);
					if (fileExtension.ToLower() == ".csv")
					{
						UploadCsvVM.Error = false;
						UploadCsvVM.ErrorMessage = string.Empty;
					}
				}
				catch
				{
				}
				if (!UploadCsvVM.Error)
				{
					//process     
					List<string> childIdList = new List<string>();
					int lineNumber = 0;
					StreamReader csvReader = new StreamReader(UploadCsvVM.CsvFile.OpenReadStream());
					while (!csvReader.EndOfStream)
					{
						lineNumber++;
						var line = csvReader.ReadLine();
						if (UploadCsvVM.HeaderRow && lineNumber == 1)
						{//do nothing - this is just the header row
						}
						else
						{
							string[] columns = line.Split(',');
							string childId = columns[0].Replace("\"", "");
							Regex rgx = new Regex("^[A-Z]{3}[-][0-9]{6}[-][0-9]{4}$");
							if (!rgx.IsMatch(childId))
							{
								UploadCsvVM.Error = true;
								UploadCsvVM.ErrorMessage += "the child Id '" + childId + "' at line number " + lineNumber + " does not match the format of a valid child Id<br/>";
							}
							else
							{
								childIdList.Add(childId);
							}
						}
					}

					if (!UploadCsvVM.Error)
					{
						if (childIdList.Count == 0)
						{
							UploadCsvVM.Error = true;
							UploadCsvVM.ErrorMessage = "No child ids found in this file.";
						}
						else
						{
							try
							{
								_childDataCacheFactory.UploadChildIds(childIdList);

								_logger.LogInformation(ConstantMessages.UploadChildIds);
								return RedirectToAction("UploadProcessing");

							}
							catch (Exception ex)
							{
								_logger.LogInformation(ConstantMessages.UploadChildIdsFailure + "Exception Message: " + ex.Message);
								UploadCsvVM.Error = true;
								UploadCsvVM.ErrorMessage = "There was an error uploading the child ids to the database.";
							}

						}
					}
				}

			}

			return View(UploadCsvVM);

		}

		public async Task<ActionResult> GetProgress(int Id)
		{
			ApiResponse response = new ApiResponse();
			try
			{
				response = await _childDataCacheFactory.GetUpdateProgressById(Id);
			}
			catch (Exception ex)
			{
				_logger.LogInformation(ConstantMessages.UploadChildIdsFailure + "Exception Message: " + ex.Message);
				response.ResponseObject = "Error";
			}
			return Json(JsonConvert.SerializeObject(response.ResponseObject));
		}

		public async Task<ActionResult> ChildList(int? page, string notification)
		{
			var pageNumber = page ?? 1; //if no page specified, default to 1
			ViewBag.Notification = !string.IsNullOrEmpty(notification) ? notification : "";

			ChildInternalDataPagedVM childrenPaged = new ChildInternalDataPagedVM();
			try
			{
				childrenPaged = await _childDataCacheForWebFactory.GetAllChildren(pageNumber, _pageSize);
				_logger.LogInformation(ConstantMessages.Load.Replace("{event}", "Child records"));
			}
			catch (Exception ex)
			{
				_logger.LogInformation(ex, ConstantMessages.Error);
				return RedirectToAction("Error", "Home");
			}
			return View(childrenPaged);
		}

		public async Task<ActionResult> Delete(int id)
		{
			try
			{
				ChildInternalDataVM childToDelete = await _childDataCacheForWebFactory.Load(id);
				if (childToDelete == null)
				{
					_logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", "Children"));
					return NotFound();
				}
				if (childToDelete.SponsoredDateTime != null || childToDelete.SelectedDateTime != null)
				{
					ModelState.AddModelError("", "You cannot delete this child as they have been selected or sponsored on the website");
					childToDelete.CannotDelete = true;
					_logger.LogWarning(ConstantMessages.ChildSelectedSponsored.Replace("{childid}", childToDelete.ChildId));
				}
				return View(childToDelete);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ConstantMessages.Error);
				return RedirectToAction("Error", "Home");
			}
		}

		public async Task<ActionResult> ChangeStatus(int id)
		{
			try
			{
				ChildInternalDataVM childToChange = await _childDataCacheForWebFactory.Load(id);
				if (childToChange == null)
				{
					_logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", "Children"));
					return NotFound();
				}
				if (childToChange.SponsoredDateTime != null || childToChange.SelectedDateTime != null)
				{
					ModelState.AddModelError("", "You cannot change the status of this child as they have been selected or sponsored on the website");
					childToChange.CannotChangeStatus = true;
					_logger.LogWarning(ConstantMessages.ChildSelectedSponsored.Replace("{childid}", childToChange.ChildId));
				}
				return View(childToChange);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ConstantMessages.Error);
				return RedirectToAction("Error", "Home");
			}
		}


		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int Id)
		{
			try
			{
				var response = await _childDataCacheForWebFactory.DeleteChildRecord(Id);
				if (response.Success)
				{
					_logger.LogInformation(ConstantMessages.Delete.Replace("{event}", "Child record"));
					return RedirectToAction("ChildList", new { notification = response.Message });
				}
				else
				{
					//child not deleted - this could be because the child is now selected or sponsored
					_logger.LogWarning(response.Message + " Child Id: " + Id.ToString());
					ChildInternalDataVM childVM = await _childDataCacheForWebFactory.Load(Id);
					if (childVM != null && childVM.SponsoredDateTime == null && childVM.SelectedDateTime == null)
					{
						//do nothing - it is ok to delete this
					}
					else
					{ if (childVM != null) { childVM.CannotDelete = true; } }
					ModelState.AddModelError("Error", response.Message);
					return View(childVM);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ConstantMessages.Error);
				ChildInternalDataVM childVM = await _childDataCacheForWebFactory.Load(Id);
				ModelState.AddModelError("Error", ConstantMessages.Error);
				return View(childVM);
			}
		}

		[HttpPost, ActionName("ChangeStatus")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ChangeStatusConfirmed(int Id)
		{
			try
			{
				//check that the child has not been selected/sponsored during this time
				ChildInternalDataVM childToChange = await _childDataCacheForWebFactory.Load(Id);
				if (childToChange == null)
				{
					_logger.LogWarning(ConstantMessages.NoRecordsFound.Replace("{event}", "Children"));
					return NotFound();
				}
				if (childToChange.SponsoredDateTime != null || childToChange.SelectedDateTime != null)
				{
					ModelState.AddModelError("", "You cannot change the status of this child as they have been selected or sponsored on the website");
					childToChange.CannotChangeStatus = true;
					_logger.LogWarning(ConstantMessages.ChildSelectedSponsored.Replace("{childid}", childToChange.ChildId));
					return View(childToChange);
				}
				else
				{
					UpdateChildStatusViewModel updateChildStatusVM = new UpdateChildStatusViewModel() { Id = Id };
					var response = await _childDataCacheForWebFactory.ChangeStatusSponsored(updateChildStatusVM);
					if (response.Success)
					{
						_logger.LogInformation(ConstantMessages.Update.Replace("{event}", "Child record"));
						return RedirectToAction("ChildList", new { notification = response.Message });
					}
					else
					{
						//error from api
						_logger.LogWarning(response.Message + " Child Id: " + Id.ToString());
						ChildInternalDataVM childVM = await _childDataCacheForWebFactory.Load(Id);
						if (childVM != null && childVM.SponsoredDateTime == null && childVM.SelectedDateTime == null)
						{
							//do nothing - it is ok to sponsor this child
						}
						else
						{ if (childVM != null) { childVM.CannotChangeStatus = true; } }
						ModelState.AddModelError("Error", response.Message);
						return View(childVM);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ConstantMessages.Error);
				ChildInternalDataVM childVM = await _childDataCacheForWebFactory.Load(Id);
				ModelState.AddModelError("Error", ConstantMessages.Error);
				return View(childVM);
			}
		}

	}
}
