using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Business.Interfaces;
using Entities.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Business.Services
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly string _azureStorageConnectionString;

        public AzureBlobStorageService(IConfiguration configuration)
        {
            _azureStorageConnectionString = configuration.GetConnectionString("AzureBlobStorageConnection");
        }
        public async Task DeleteAsync(ContainerEnum container, string blobFilename)
        {
            var containerName = Enum.GetName(typeof(ContainerEnum), container).ToLower();
            var blobContainerClient = new BlobContainerClient(_azureStorageConnectionString, containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobFilename);

            try
            {
                await blobClient.DeleteAsync();
            }
            catch
            {
            }
        }


        public async Task<string> UploadAsync(IFormFile file, ContainerEnum container, string blobName = null)
        {
            if (file.Length == 0) return null;

            var containerName = Enum.GetName(typeof(ContainerEnum), container).ToLower();
            var blobContainerClient = new BlobContainerClient(_azureStorageConnectionString, containerName);

            // Trae una referencia de blob recién cargada desde el API en un contenedor desde config settings
            if (string.IsNullOrEmpty(blobName))
            {
                //Nombre del archivo
                blobName = Guid.NewGuid().ToString();
            }

            //Accedo al archivo 
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            // Indica al blob el tipo de archivo que se va a guardar en el contenedor (.FileName)(png, jpg)
            var blobHttpHeader = new BlobHttpHeaders { ContentType = file.ContentType };

            // Abre un stream para el archivo que queremos cargar
            await using (Stream stream = file.OpenReadStream())
            {
                // Carga el archivo async
                await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });
            }
            return blobName;
        }
    }
}

