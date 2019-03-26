using Demo.Core.Common.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Demo.Core.Repository
{
	/// <summary>
	/// DB连接串 配置
	/// </summary>
	public class BaseDBConfig
	{
		static string sqlServerConnection = AppsettingsHelper.Get(new string[] { "AppSettings", "SqlServer", "SqlServerConnection" });//获取连接字符串

		public static string ConnectionString = File.Exists(@"D:\my-file\dbCountPsw1.txt") ? File.ReadAllText(@"D:\my-file\dbCountPsw1.txt").Trim() : (!string.IsNullOrEmpty(sqlServerConnection) ? sqlServerConnection : "server=.;uid=sa;pwd=123456;database=WMBlogDB");
	}
}
