using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using United4Admin.WebApplication.ApiClientFactory.FactoryInterfaces;
using United4Admin.WebApplication.ViewModels;
using Microsoft.Azure.Storage;
using System.IO.Compression;

namespace United4Admin.WebApplication.BLL
{
    public interface IZipFileHelper
    {
        Task<byte[]> ZipUpFiles(string[] Filenames, IImageStoreFactory imageActions);
        byte[] ZipUpCSVFiles(string[] Filenames, string[] OutputFiles);
        Task<CloudBlobContainer> GetBlobImageContainer(BlobStorageVM blobStorageVM);
        CloudStorageAccount GetConnectionString(string account, string key);
    }

    public class ZipFileHelper : IZipFileHelper
    {        
        public  async Task<byte[]> ZipUpFiles(string[] fileList, IImageStoreFactory imageActions)
        {
            byte[] result;

            using (var packageStream = new MemoryStream())
            {
                BlobStorageVM blobStorageVM = await imageActions.GetBobStorageDetails();
                CloudBlobContainer cloudBlobContainer = await GetBlobImageContainer(blobStorageVM);

                using (var archive = new ZipArchive(packageStream, ZipArchiveMode.Create, true))
                {
                    foreach (var virtualFile in fileList)
                    {
                        if (!string.IsNullOrEmpty(virtualFile))
                        {
                            CloudBlockBlob thisBlob = cloudBlobContainer.GetBlockBlobReference(virtualFile);

                            //Create a zip entry for each attachment
                            var zipFile = archive.CreateEntry(virtualFile);
                            var sourceFileStream = new MemoryStream();
                            using (var zipEntryStream = zipFile.Open())
                            {

                                thisBlob.DownloadToStream(zipEntryStream);
                            }
                        }
                    }
                archive.Dispose();
                }
                result = packageStream.ToArray();
            }

            return result;
        }

        //function to convert ZipArchive bytes to download compressed
        public byte[] ZipUpCSVFiles(string[] fileList, string[] OutputFiles)
        {
            byte[] result;

            using (var packageStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(packageStream, ZipArchiveMode.Create, true))
                {
                    foreach (var virtualFile in fileList.Select((value, i) => new { i, value }))
                    {
                        //Create a zip entry for each attachment
                        var zipFile = archive.CreateEntry(virtualFile.value);
                        var zipFileValue = OutputFiles[virtualFile.i];
                        var sourceFileStream = new MemoryStream();
                        byte[] bytes = zipFileValue != null ? new UTF32Encoding().GetBytes(zipFileValue) : new UTF32Encoding().GetBytes("");
                       
                        using (var zipEntryStream = zipFile.Open())
                        {
                            zipEntryStream.Write(bytes);
                        }
                    }
                    archive.Dispose();
                }
                result = packageStream.ToArray();
            }

            return result;
        }

        public Task<CloudBlobContainer> GetBlobImageContainer(BlobStorageVM blobStorageVM)
        {
            CloudStorageAccount cloudStorageAccount = GetConnectionString(blobStorageVM.StorageAccountName, blobStorageVM.StorageAccountKey);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(blobStorageVM.StorageContainer);          
            return Task.FromResult(cloudBlobContainer);
        }

        public CloudStorageAccount GetConnectionString(string account, string key)
        {
            string connectionString = string.Format("DefaultEndpointsProtocol=https;AccountName=" + account + ";AccountKey=" + key);
            return CloudStorageAccount.Parse(connectionString);
        }
    }
}