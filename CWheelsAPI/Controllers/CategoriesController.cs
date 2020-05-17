using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CWheelsAPI.Controllers.Resources;
using CWheelsAPI.Core;
using CWheelsAPI.Models;
using CWheelsAPI.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CWheelsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CategoriesController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get all categories of vehicles.
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            var categories = unitOfWork.Categories.GetAll().ToList();
            return Ok(mapper.Map<List<Category>,List<CategoryResource>>(categories));
        }
    }
}