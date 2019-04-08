using Demo.Core.IService;
using Demo.Core.Model.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Core.Controllers
{
	[Route("api/Blog")]
	public class BlogController : BaseController
	{
		private readonly IAdvertisementServices _advertisementServices;

		public BlogController(IAdvertisementServices advertisementServices)
		{
			_advertisementServices = advertisementServices;
		}

		/// <summary>
		/// 根据id获取数据 GET: api/Blog/5
		/// </summary>
		/// <param name="id">参数id</param>
		/// <returns></returns>
		[HttpGet("{id}", Name = "Get")]
		public async Task<List<Advertisement>> Get(int id)
		{
			return await _advertisementServices.Query(d => d.Id == id);
		}
	}
}