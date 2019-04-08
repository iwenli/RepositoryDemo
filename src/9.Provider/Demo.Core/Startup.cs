using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Demo.Core.Aop;
using Demo.Core.Common.Cache;
using Demo.Core.Common.Extension;
using Demo.Core.Common.Helper;
using Demo.Core.Filter;
using Demo.Core.Log;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using static Demo.Core.CustomApiVersion.CustomApiVersion;

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
			Configuration = configuration;
			//log4net
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
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			#region 部分服务注入-netcore自带方法
			//缓存注入
			services.AddSingleton<IMemoryCache>(factory => new MemoryCache(new MemoryCacheOptions()));
			//redis
			services.AddSingleton<IRedisCache, RedisCache>();
			//log日志注入
			services.AddSingleton<ILoggerHelper, LogHelper>();
			#endregion

			#region 初始化DB
			//services.AddScoped<DbContext>();
			#endregion

			#region CORS
			services.AddCors(c =>
			{
				////↓↓↓↓↓↓↓注意正式环境不要使用这种全开放的处理↓↓↓↓↓↓↓↓↓↓
				//c.AddPolicy("AllRequests", policy =>
				//{
				//	policy
				//		.AllowAnyOrigin()//允许任何源
				//		.AllowAnyMethod()//允许任何方式
				//		.AllowAnyHeader()//允许任何头
				//		.AllowCredentials();//允许cookie
				//});
				////↑↑↑↑↑↑↑注意正式环境不要使用这种全开放的处理↑↑↑↑↑↑↑↑↑↑

				//一般采用这种方法
				c.AddPolicy("LimitRequests", policy =>
				{
					policy
						.WithOrigins("http://localhost:63139", "http://blog.iwenli.org")//支持多个域名端口
						.AllowAnyHeader()//Ensures that the policy allows any header.
						.AllowAnyMethod();//标头添加到策略
				});

			});
			#endregion

			#region Swagger UI Service

			var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;

			services.AddSwaggerGen(c =>
			{
				//遍历出全部的版本，做文档信息展示
				typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
				{
					c.SwaggerDoc(version, new Info
					{
						Version = version,
						Title = $"{ApiName} API",
						Description = $"{ApiName} HTTP API " + version,
						TermsOfService = "None",
						Contact = new Contact { Name = ApiName, Email = "admin@iwenli.org", Url = "http://www.iwenli.org" }
					});
					c.OrderActionsBy(m => m.RelativePath);
				});

				#region 读取xml信息
				var xmlPath = Path.Combine(basePath, "Demo.Core.xml");//这个就是刚刚配置的xml文件名
				c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

				var xmlModelPath = Path.Combine(basePath, "Demo.Core.Model.xml");//这个就是Model层的xml文件名
				c.IncludeXmlComments(xmlModelPath);
				#endregion

				#region Token绑定到ConfigureServices
				//添加header验证信息
				//c.OperationFilter<SwaggerHeader>();

				var issuerName = (Configuration.GetSection("Audience"))["Issuer"];
				var security = new Dictionary<string, IEnumerable<string>> { { issuerName, new string[] { } }, };
				c.AddSecurityRequirement(security);
				c.AddSecurityDefinition(issuerName, new ApiKeyScheme
				{
					Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
					Name = "Authorization",//jwt默认的参数名称
					In = "header",//jwt默认存放Authorization信息的位置(请求头中)
					Type = "apiKey"
				});
				#endregion
			});
			#endregion

			#region MVC + 注入全局异常捕获

			services.AddMvc(m =>
				{
					//全局异常过滤
					m.Filters.Add(typeof(GlobalExceptionsFilter));
				})
				.SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2)
				//全局配置Json序列化处理
				.AddJsonOptions(options =>
				{
					//忽略循环引用
					options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
					//不使用驼峰样式的key
					//options.SerializerSettings.ContractResolver = new DefaultContractResolver();
					//设置时间格式
					options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
				});
			#endregion

			#region Token服务注册
			services.AddSingleton((Func<IServiceProvider, IMemoryCache>)(factory =>
			{
				var cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());
				return cache;
			}));
			services.AddAuthorization(options =>
			{
				options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
				options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
				options.AddPolicy("AdminOrClient", policy => policy.RequireRole("Admin,Client").Build());
			});
			#endregion

			#region AutoFac
			//实例化 AutoFac 容器
			var builder = new ContainerBuilder();
			//注册要通过反射创建的组件
			builder.RegisterType<LogAop>();
			builder.RegisterType<MemoryCacheAop>();
			builder.RegisterType<RedisCacheAop>();
			//builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();

			try
			{
				#region 不引用项目，通过FileLoad
				var serviceDllFile = Path.Combine(basePath, "Demo.Core.Service.dll");
				var serviceAssembly = Assembly.LoadFile(serviceDllFile);
				// AOP 开关，如果想要打开指定的功能，只需要在 appsettigns.json 对应对应 true 就行。
				var cacheType = new List<Type>();
				if (AppsettingsHelper.Get(new string[] { "AppSettings", "RedisCaching", "Enabled" }).ObjToBool())
				{
					cacheType.Add(typeof(RedisCacheAop));
				}
				if (AppsettingsHelper.Get(new string[] { "AppSettings", "MemoryCachingAop", "Enabled" }).ObjToBool())
				{
					cacheType.Add(typeof(MemoryCacheAop));
				}
				if (AppsettingsHelper.Get(new string[] { "AppSettings", "LogAop", "Enabled" }).ObjToBool())
				{
					cacheType.Add(typeof(LogAop));
				}

				builder.RegisterAssemblyTypes(serviceAssembly)
					.AsImplementedInterfaces()
					.InstancePerLifetimeScope()
					.EnableInterfaceInterceptors() //引用Autofac.Extras.DynamicProxy;
												   //如果你想注入两个，就这么写  InterceptedBy(typeof(BlogCacheAOP), typeof(BlogLogAOP));
												   //如果想使用Redis缓存，请必须开启 redis 服务，端口号我的是6319，如果不一样还是无效，否则请使用memory缓存 BlogCacheAOP
					.InterceptedBy(cacheType.ToArray())//允许将拦截器服务的列表分配给注册。 
					;

				var repositoryDllFile = Path.Combine(basePath, "Demo.Core.Repository.dll");
				var repositoryAssembly = Assembly.LoadFile(repositoryDllFile);
				builder.RegisterAssemblyTypes(repositoryAssembly).AsImplementedInterfaces();

				#endregion

				#region 直接引用项目
				//var servicveAssembly = Assembly.Load("Demo.Core.Service");
				//builder.RegisterAssemblyTypes(servicveAssembly).AsImplementedInterfaces();  //指定已扫描程序集中的类型注册为提供所有其实现的接口。
				//var repositoryAssembly = Assembly.Load("Demo.Core.Repository");
				//builder.RegisterAssemblyTypes(repositoryAssembly).AsImplementedInterfaces(); 
				#endregion

				#endregion
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			//将 services 填充 AutoFac 容器生成器
			builder.Populate(services);
			//使用已进行的组件等级创建新容器
			var applicationContainer = builder.Build();
			//第三方ico接管
			return new AutofacServiceProvider(applicationContainer);
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

			}
			else
			{
				app.UseExceptionHandler("/Error");
				// 在非开发环境中，使用HTTP严格安全传输(or HSTS) 对于保护web安全是非常重要的。
				// 强制实施 HTTPS 在 ASP.NET Core，配合 app.UseHttpsRedirection
				//app.UseHsts();
			}

			#region Swagger
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				//根据版本名称倒序 遍历展示
				typeof(ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
				{
					c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{ApiName} {version}");
				});
				c.RoutePrefix = ""; //路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉
			});
			#endregion


			#region 跨域callback中间件

			//app.Use(async (context, next) =>
			//{
			//	var originResponse = context.Response.Body;
			//	try
			//	{
			//		using (var memoryStream = new MemoryStream())
			//		{
			//			context.Response.Body = memoryStream;
			//			await next.Invoke();
			//			var callback = context.Request.Query["callback"];
			//			if (!string.IsNullOrWhiteSpace(callback))
			//			{
			//				//替换response流
			//				string response;
			//				using (var streamReader = new StreamReader(memoryStream))
			//				{
			//					memoryStream.Position = 0;
			//					response = streamReader.ReadToEnd();
			//				}

			//				response = $"{callback}({response})";
			//				using (var streamWriter = new StreamWriter(originResponse))
			//				{
			//					streamWriter.Write(response);
			//				}
			//			}
			//		}
			//	}
			//	catch (Exception ex)
			//	{
			//	}
			//	finally
			//	{
			//		//将原始的请求和响应流替换回去
			//		context.Response.Body = originResponse;
			//	}
			//});

			#endregion

			#region Authen

			//此授权认证方法已经放弃，请使用下边的官方验证方法。但是如果你还想传User的全局变量，还是可以继续使用中间件
			//app.UseMiddleware<JwtTokenAuth>();

			//如果你想使用官方认证，必须在上边ConfigureService 中，配置JWT的认证服务 (.AddAuthentication 和 .AddJwtBearer 二者缺一不可)
			app.UseAuthentication();
			#endregion

			#region CORS
			#region 跨域第一种版本
			//跨域第一种版本，请要ConfigureService中配置服务 services.AddCors();
			//    app.UseCors(options => options.WithOrigins("http://localhost:8021").AllowAnyHeader()
			//.AllowAnyMethod());  
			#endregion

			#region 跨域第二种方法
			//使用策略，详细策略信息在ConfigureService中
			app.UseCors("LimitRequests");//将 CORS 中间件添加到 web 应用程序管线中, 以允许跨域请求。 
			#endregion
			#endregion

			//app.UseMiddleware<JwtTokenAuth>();


			//跳转https
			app.UseHttpsRedirection();
			// 使用静态文件
			app.UseStaticFiles();
			// 使用cookie
			app.UseCookiePolicy();
			//把错误码返回给前台
			app.UseStatusCodePages();

			app.UseMvc();
		}
	}
}
