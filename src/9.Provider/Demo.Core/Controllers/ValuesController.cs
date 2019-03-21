using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Core.Model.Demo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Core.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize(Policy = "Admin")]
	public class ValuesController : ControllerBase
	{
		// GET api/values
		/// <summary>
		/// 获取列表
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult<IEnumerable<string>> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/values/5
		/// <summary>
		/// 获取一条记录
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public ActionResult<string> Get(int id)
		{
			return "value";
		}

		//// POST api/values
		///// <summary>
		///// 添加一条记录
		///// </summary>
		///// <param name="value"></param>
		//[HttpPost]
		//public void Post([FromBody] string value)
		//{
		//}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}

		/// <summary>
		/// post
		/// </summary>
		/// <param name="entity">model实体类参数</param>
		[HttpPost]
		public void Post(DemoInfo entity)
		{
		}
	}
}
