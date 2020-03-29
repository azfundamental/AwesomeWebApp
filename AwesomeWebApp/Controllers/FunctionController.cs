using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using AwesomeWebApp.Models;
using System.Configuration;
using BlobStorageDemo;

namespace AwesomeWebApp.Controllers
{
    public class FunctionController : Controller
    {
        ImageService imageService = new ImageService();



        const string blobContainerName = "webappstoragedotnet-imagecontainer";

        readonly CloudStorageAccount CloudStorageAccount = ConnectionString.GetConnectionString();

        public async Task<ActionResult> Index()
        {
            try
            {
                // Create a blob client for interacting with the blob service.

                CloudBlobClient cloudBlobClient = CloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("sampleimage");

                // To view the uploaded blob in a browser, you have two options. The first option is to use a Shared Access Signature (SAS) token to delegate  
                // access to the resource. See the documentation links at the top for more information on SAS. The second approach is to set permissions  
                // to allow public access to blobs in this container. Comment the line below to not use this approach and to use SAS. Then you can view the image  
                // using: https://[InsertYourStorageAccountNameHere].blob.core.windows.net/webappstoragedotnet-imagecontainer/FileName 
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                // Gets all Cloud Block Blobs in the blobContainerName and passes them to teh view
                List<Uri> allBlobs = new List<Uri>();
                foreach (IListBlobItem blob in cloudBlobContainer.ListBlobs())
                {
                    if (blob.GetType() == typeof(CloudBlockBlob))
                        allBlobs.Add(blob.Uri);
                }
                ViewBag.allBlobsimage = allBlobs;
                
                CloudBlobContainer cloudBlobContainer2 = cloudBlobClient.GetContainerReference("miniatures");
                await cloudBlobContainer2.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                // Gets all Cloud Block Blobs in the blobContainerName and passes them to teh view
                List<Uri> allBlobs2 = new List<Uri>();
                foreach (IListBlobItem blob in cloudBlobContainer2.ListBlobs())
                {
                    if (blob.GetType() == typeof(CloudBlockBlob))
                        allBlobs2.Add(blob.Uri);
                }
                ViewBag.allBlobsimage2 = allBlobs2;

                return View();
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }
    }
}