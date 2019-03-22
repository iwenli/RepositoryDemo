using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Core.IService;
using Demo.Core.Model.Models;
using Demo.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Core.Controllers
{
	[Route("api/Blog")]
	public class BlogController : Controller
	{
		/// <summary>
		/// 根据id获取数据 GET: api/Blog/5
		/// </summary>
		/// <param name="id">参数id</param>
		/// <returns></returns>
		[HttpGet("{id}", Name = "Get")]
		public async Task<List<Advertisement>> Get(int id)
		{
			IAdvertisementServices advertisementServices = new AdvertisementServices();

			return await advertisementServices.Query(d => d.Id == id);
		}
	}
}