using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Core.Model.Models
{
	/// <summary>
	/// 
	/// </summary>
	public class RootEntity
	{
		public RootEntity()
		{
			
		}

		/// <summary>
		/// ID
		/// </summary>
		[SqlSugar.SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
		public int Id { get; set; }
	}
}
