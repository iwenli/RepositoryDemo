using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Core.Auth.JWT;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Core.Controllers
{
	/// <summary>
	/// 登录
	/// </summary>
    public class LoginController : BaseController
	{
		/// <summary>
		/// 获取JWT的重写方法，推荐这种，注意在文件夹OverWrite下
		/// </summary>
		/// <param name="id">id</param>
		/// <param name="sub">角色</param>
		/// <returns></returns>
		[HttpGet]
		[Route("Token2")]
		public JsonResult GetJWTStr(long id = 1, string sub = "Admin")
		{
			//这里就是用户登陆以后，通过数据库去调取数据，分配权限的操作
			var tokenModel = new JwtTokenModel();
			tokenModel.Uid = id;
			tokenModel.Role = sub;

			string jwtStr = JwtHelper.IssueJWT(tokenModel);
			return Json(jwtStr);
		}
	}
}