using Entities.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAzureBlobStorageService
    {
        //string AzureStorageConnectionString { get; }
        /// <summary>
        /// Carga un archivo entregado con el request
        /// </summary>
        /// <param name="file">Archivo para cargar</param>
        /// <param name="container">Contenedor donde se hace el submit del archivo</param>
        /// <param name="blobName">Blob name a actualizar</param>
        /// <returns>Nombre del archivo guardado en Blob Container</returns>
        Task<string> UploadAsync(IFormFile file, ContainerEnum container, string blobName = null);

        /// <summary>
        /// Elimina un archivo con un filename especificado
        /// </summary>
        /// <param name="container"></param>
        /// <param name="blobFilename"></param>
        /// <returns></returns>
        Task DeleteAsync(ContainerEnum container, string blobFilename);
    }
}
