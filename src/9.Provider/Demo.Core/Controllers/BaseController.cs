using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.Core.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	public class BaseController : Controller
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var callback = context.HttpContext.Request.Query["callback"];
			if (!string.IsNullOrWhiteSpace(callback))
			{
				//response = $"{callback}({response})";
			}

			base.OnActionExecuting(context);
		}
	}
}