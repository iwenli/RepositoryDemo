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
		public static string ConnectionString = File.ReadAllText(@"D:\my-file\dbCountPsw1.txt").Trim();

		//正常格式是

		//public static string ConnectionString = "server=.;uid=sa;pwd=sa;database=BlogDB"; 

		//原谅我用配置文件的形式，因为我直接调用的是我的服务器账号和密码，安全起见

	}
}
