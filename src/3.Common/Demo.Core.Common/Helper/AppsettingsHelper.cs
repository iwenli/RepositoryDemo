﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Core.Common.Helper
{
	/// <summary>
	/// appsettings.json操作类
	/// </summary>
	public class AppsettingsHelper
	{
		static IConfiguration Configuration { get; set; }
		static AppsettingsHelper()
		{
			//ReloadOnChange = true 当appsettings.json被修改时重新加载
			Configuration = new ConfigurationBuilder()
				.Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
				.Build();
		}
		/// <summary>
		/// 封装要操作的字符
		/// </summary>
		/// <param name="sections"></param>
		/// <returns></returns>
		public static string Get(params string[] sections)
		{
			try
			{
				var val = string.Empty;
				for (int i = 0; i < sections.Length; i++)
				{
					val += sections[i] + ":";
				}

				return Configuration[val.TrimEnd(':')];
			}
			catch (Exception)
			{
				return "";
			}

		}
	}
}
