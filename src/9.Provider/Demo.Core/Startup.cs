using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Demo.Core.Auth.JWT;
using Demo.Core.Filter;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using Demo.Core.Models;

namespace Demo.Core
{
	/// <summary>
	/// 
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// log4net 仓储库
		/// </summary>
		public static ILoggerRepository repository { get; set; }


		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;//log4net
			repository = LogManager.CreateRepository("Demo.Core");
			//指定配置文件
			XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
		}

		/// <summary>
		/// 
		/// </summary>
		public IConfiguration Configuration { get; }


		private const string ApiName = "Demo.Core";
		/// <summary>
		/// 将服务添加到容器
		/// </summary>
		/// <param name="services"></param>
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			#region swagger
			services.AddSwaggerGen(c =>
				{
					c.SwaggerDoc("v1", new Info
					{
						Version = "v0.1.0",
						Title = $"{ApiName} API",
						Description = $"{ApiName} 框架说明文档",
						TermsOfService = "None",
						Contact = new Swashbuckle.AspNetCore.Swagger.Contact
						{
							Name = ApiName,
							Email = "admin@iwenli.org",
							Url = "http://www.iwenli.org"
						}
					});

				#region 读取xml信息
				var basePath = ApplicationEnvironment.ApplicationBasePath;
					var xmlPath = Path.Combine(basePath, "Demo.Core.xml");//这个就是刚刚配置的xml文件名
				var xmlModelPath = Path.Combine(basePath, "Demo.Core.Model.xml");//这个就是Model层的xml文件名
				c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改
				c.IncludeXmlComments(xmlModelPath);
				#endregion

				#region Token绑定到ConfigureServices
				//添加header验证信息
				//c.OperationFilter<SwaggerHeader>();
				var security = new Dictionary<string, IEnumerable<string>> { { "Demo.Core", new string[] { } }, };
					c.AddSecurityRequirement(security);
				//方案名称“Demo.Core”可自定义，上下一致即可
				c.AddSecurityDefinition("Demo.Core", new ApiKeyScheme
					{
						Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入{token}\"",
						Name = "Authorization",//jwt默认的参数名称
					In = "header",//jwt默认存放Authorization信息的位置(请求头中)
					Type = "apiKey"
					});
				#endregion
			});
			#endregion

			#region Token服务注册
			services.AddSingleton<IMemoryCache>(factory =>
			{
				var cache = new MemoryCache(new MemoryCacheOptions());
				return cache;
			});
			services.AddAuthorization(options =>
			{
				options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
				options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
				options.AddPolicy("AdminOrClient", policy => policy.RequireRole("Admin,Client").Build());
			});

		    services.AddDbContext<DBContext>(options =>
		            options.UseSqlServer(Configuration.GetConnectionString("DBContext")));
			#endregion
		}

		/// <summary>
		/// 配置HTTP请求管道
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();

				#region Swagger
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
					c.RoutePrefix = "";//路径配置，设置为空，表示直接访问该文件
					//c.InjectJavascript($"/swagger_translator.js"); //注入汉化脚本
				});
				#endregion
			}
			else
			{
				app.UseHsts();
			}

			app.UseMiddleware<JwtTokenAuth>();

			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
