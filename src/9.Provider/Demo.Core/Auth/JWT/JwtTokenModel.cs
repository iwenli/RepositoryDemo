using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Core.Auth.JWT
{
	/// <summary>
	/// 
	/// </summary>
	public class JwtTokenModel
	{
		/// <summary>
		/// Id
		/// </summary>
		public long Uid { get; set; }
		/// <summary>
		/// 角色
		/// </summary>
		public string Role { get; set; }
		/// <summary>
		/// 职能
		/// </summary>
		public string Work { get; set; }
	}
}
