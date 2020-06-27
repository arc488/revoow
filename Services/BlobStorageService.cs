using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Options;
using Revoow.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Services
{

    public class BlobStorageService
    {
        string accessKey = string.Empty;
        string downloadPath = @"D:\local\Temp";

        public BlobStorageService(IOptions<BlobStorageOptions> options)
        {
            this.accessKey = options.Value.AccessKey;
        }

        public string UploadFileToBlob(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {

                var _task = Task.Run(() => this.UploadFileToBlobAsync(strFileName, fileData, fileMimeType));
                _task.Wait();
                string fileUrl = _task.Result;
                return fileUrl;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async void DeleteBlobData(string fileUrl)
        {
            Uri uriObj = new Uri(fileUrl);
            string BlobName = Path.GetFileName(uriObj.LocalPath);

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            string strContainerName = "uploads";
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

            string pathPrefix = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/";
            CloudBlobDirectory blobDirectory = cloudBlobContainer.GetDirectoryReference(pathPrefix);
            // get block blob refarence    
            CloudBlockBlob blockBlob = blobDirectory.GetBlockBlobReference(BlobName);
            // delete blob from container        
            await blockBlob.DeleteAsync();
        }

        public async Task<string> GetFileFromStorage(string fileName)
        {
            string tempFilePath = String.Empty;
            BlobContainerClient container = new BlobContainerClient(accessKey, "uploads");
            try
            {
                // Get a reference to a blob named "sample-file"
                BlobClient blob = container.GetBlobClient(fileName);

                // Download the blob's contents and save it to a file
                BlobDownloadInfo download = await blob.DownloadAsync();
                using (FileStream file = File.OpenWrite(downloadPath))
                {
                    await download.Content.CopyToAsync(file);
                    tempFilePath = file.Name;
                }

            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("WEBM file not found");
            }

            return tempFilePath;
        }

        private string GenerateFileName(string fileName)
        {
            string strFileName = string.Empty;
            string[] strName = fileName.Split('.');
            strFileName = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/" + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "." + strName[strName.Length - 1];
            return strFileName;
        }

        private async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                string strContainerName = "uploads";
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
                //string fileName = this.GenerateFileName(strFileName);
                string fileName = strFileName;

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                if (fileName != null && fileData != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                    cloudBlockBlob.Properties.ContentType = fileMimeType;
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                    return cloudBlockBlob.Uri.AbsoluteUri;
                }
                return "";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}

