using AutoMapper;
using CWheelsAPI.Controllers.Resources;
using CWheelsAPI.Core;
using CWheelsAPI.Errors;
using CWheelsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace CWheelsAPI.Controllers
{
    [Route("api/vehicles/{vehicleId}/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment host;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly PhotoSettings photoSettings;

        public ImagesController(IWebHostEnvironment host, IOptionsSnapshot<PhotoSettings> options, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.host = host;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.photoSettings = options.Value;
        }

        /// <summary>
        /// returns thumbnails of the images of a vehicle
        /// </summary>
        /// <returns>Thumbnails of the images added to the vehicle</returns>
        [HttpGet]
        public async Task<IActionResult> GetThumbnails(int vehicleId)
        {
            var thumbnails = await unitOfWork.Thumbnails.GetThumbnails(vehicleId);
            return Ok(mapper.Map<IEnumerable<Thumbnail>, IEnumerable<ThumbnailResource>>(thumbnails));
        }

        /// <summary>
        /// Add an image to a vehicle.
        /// </summary>
        /// <returns>Thumbnail of the image added</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(int vehicleId,IFormFile file)
        {
            var vehicle = unitOfWork.Vehicles.Get(vehicleId);

            if (vehicle == null)
                return BadRequest(new ApiResponse(400, "Vehicle does not exists"));

            if (file == null)
                return BadRequest(new ApiResponse(400, "File cannot be null"));

            if (file.Length == 0)
                return BadRequest(new ApiResponse(400, "Empty File"));

            if (file.Length > photoSettings.MaxBytes)
                return BadRequest(new ApiResponse(400, "Maximum File Size exceeded"));

            if (!photoSettings.IsSupported(file.FileName))
                return BadRequest(new ApiResponse(400, "Only Images allowed"));


            var uploadsFolderPath = Path.Combine(host.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            };
            Stream fileStream = file.OpenReadStream();
            Image newImage = photoSettings.GetReducedImage(150, 150, fileStream);

            var thumbnailFolderPath = Path.Combine(host.WebRootPath, "thumbnails");
            if (!Directory.Exists(thumbnailFolderPath))
                Directory.CreateDirectory(thumbnailFolderPath);

            var thumbnailFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var thumbnailFilePath = Path.Combine(thumbnailFolderPath, thumbnailFileName);

            newImage.Save(thumbnailFilePath);

            var newPhoto = new Photo { ImageUrl = fileName };
            var newThumbnail = new Thumbnail { ThumbnailUrl = thumbnailFileName };

            vehicle.Images.Add(newPhoto);
            vehicle.Thumbnails.Add(newThumbnail);
            await unitOfWork.CompleteAsync();

            return Ok(mapper.Map<Thumbnail,ThumbnailResource>(newThumbnail));
        }

       

    }
}