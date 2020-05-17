using AutoMapper;
using CWheelsAPI.Controllers.Resources;
using CWheelsAPI.Core;
using CWheelsAPI.Errors;
using CWheelsAPI.Filters;
using CWheelsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CWheelsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment host;

        public VehiclesController(IMapper mapper, IUnitOfWork unitOfWork,IWebHostEnvironment host)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.host = host;
        }

        /// <summary>
        /// Creates a new Vehicle.
        /// </summary>
        /// <response code="200">Successfully creates vehicle</response>
        /// <response code="401">For unauthorized requests</response>
        [HttpPost]
        [Authorize]
        [ValidationModel]
        public async Task<IActionResult> Post([FromBody] SaveVehicleResource vehicleResource)
        {
            try
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                var user = unitOfWork.Accounts.FindUserByEmail(userEmail);

                if (user == null)
                    return Unauthorized(new ApiResponse(401, "Not Authorized"));

                var vehicleModel = mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
                vehicleModel.IsHotAndNew = false;
                vehicleModel.DatePosted = DateTime.Now;
                vehicleModel.UserId = user.Id;
                vehicleModel.IsFeatured = false;

                unitOfWork.Vehicles.Add(vehicleModel);
                await unitOfWork.CompleteAsync();

                return Ok(new { status = true, message = "Vehicle Added Successfully", vehicleId = vehicleModel.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        /// <summary>
        /// Gets all hot and new ads.
        /// </summary>
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> HotAndNewAds()
        {
            var vehicles = await unitOfWork.Vehicles.GetHotAndNewVehicles();
            return Ok(mapper.Map<List<Vehicle>, List<HotAndNewVehicleResource>>(vehicles));
        }

        /// <summary>
        /// Searches vehicle by given search pattern.
        /// </summary>

        [HttpGet("[action]")]
        public async Task<IActionResult> SearchVehicles([FromQuery] string search)
        {
            var vehicles = await unitOfWork.Vehicles.SearchVehicles(search);
            return Ok(mapper.Map<List<Vehicle>, List<SearchVehicleResource>>(vehicles));
        }


        /// <summary>
        /// Gets all vehicles of a given category.
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> GetVehicles(int categoryId)
        {
            var vehicles = await unitOfWork.Vehicles.GetVehiclesByCategory(categoryId);
            return Ok(mapper.Map<List<Vehicle>, List<VehicleByCategoryResource>>(vehicles));
        }

        /// <summary>
        /// Updates a Vehicle.
        /// </summary>
        /// <response code="200">Successfully updates vehicle</response>
        /// <response code="401">For unauthorized requests</response>
        /// <response code="404">Vehicle not found</response>
        [HttpPut("{id}")]
        [Authorize]
        [ValidationModel]
        public async Task<IActionResult> UpdateVehicle(int id,[FromBody] SaveVehicleResource vehicleResource)
        {
            var vehicle = unitOfWork.Vehicles.Get(id);
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = unitOfWork.Accounts.FindUserByEmail(userEmail);

            if (vehicle == null)
                return NotFound(new ApiResponse(404,"Vehicle not found"));

            if (vehicle.UserId != user.Id)
                return Unauthorized(new ApiResponse(401, "Not Authorized"));

            mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);

            await unitOfWork.CompleteAsync();

            return Ok(new { status = true, message = "Vehicle Edited Successfully", vehicleId = vehicle.Id });
        }

        /// <summary>
        /// Deletes a specific vehicle.
        /// </summary>
        /// <param name="id"></param>  
        /// <response code="200">Successfully deletes vehicle</response>
        /// <response code="401">For unauthorized requests</response>
        /// <response code="404">If vehicle is not found</response> 
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteVehicle(int id)
        {
            var vehicle =  unitOfWork.Vehicles.GetFullVehicleDetails(id);
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = unitOfWork.Accounts.FindUserByEmail(userEmail);

            if (vehicle == null)
                return NotFound(new ApiResponse(404,"Vehicle not found"));

            if (vehicle.UserId != user.Id)
                return Unauthorized(new ApiResponse(401, "Not Authorized"));

            if (vehicle.Images.Count > 0)
            {
                var uploadsFolderPath = Path.Combine(host.WebRootPath, "images");
                foreach (var image in vehicle.Images)
                {
                    var filePath = Path.Combine(uploadsFolderPath, image.ImageUrl);
                    System.IO.File.Delete(filePath);
                }
            }
            if(vehicle.Thumbnails.Count > 0)
            {
                var uploadsFolderPath = Path.Combine(host.WebRootPath, "thumbnails");
                foreach (var thumbnail in vehicle.Thumbnails)
                {
                    var filePath = Path.Combine(uploadsFolderPath, thumbnail.ThumbnailUrl);
                    System.IO.File.Delete(filePath);
                }
            }
            unitOfWork.Vehicles.Remove(vehicle);
            unitOfWork.CompleteAsync();
            return Ok(new ApiResponse(200,"Vehicle deleted successfully"));
        }


        /// <summary>
        /// Gets a specific vehicle.
        /// </summary>
        /// <param name="id"></param>  
        /// <response code="200">Successfully gets vehicle</response>
        /// <response code="404">If vehicle is not found</response> 

        [HttpGet("{id}")]
        public IActionResult VehicleDetails(int id)
        {
            var vehicle = unitOfWork.Vehicles.GetVehicleDetails(id);
            if (vehicle == null)
                return NotFound(new ApiResponse(404,"Vehicle not found"));
          
            return Ok(mapper.Map<Vehicle,VehicleResource>(vehicle));
        }


        /// <summary>
        /// Gets all ads of logged in user.
        /// </summary>
        /// <response code="200">Successfully gets ads</response>
        /// <response code="401">For unauthorized requests</response>
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> MyAds()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = unitOfWork.Accounts.FindUserByEmail(userEmail);

            if (user == null) 
                return NotFound(new ApiResponse(404,"User not found"));

            var vehicles = await unitOfWork.Vehicles.GetVehiclesForUser(user);

            return Ok(mapper.Map<List<Vehicle>,List<MyAdsResource>>(vehicles));
        }


    }
}