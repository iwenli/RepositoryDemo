using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Core.Common.Attributes
{
	/// <summary>
	/// 是否开启缓存
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public class CachingAttribute : Attribute
	{
		/// <summary>
		/// 缓存绝对过期时间（分钟）
		/// </summary>
		public int AbsoluteExpiration { get; set; } = 30; 
	}
}
