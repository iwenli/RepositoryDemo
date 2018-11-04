using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Demo.Core.Filter;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Demo.Core
{
	public class Startup
	{
		/// <summary>
		/// log4net 仓储库
		/// </summary>
		public static ILoggerRepository repository { get; set; }


		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;//log4net
			repository = LogManager.CreateRepository("Blog.Core");
			//指定配置文件
			XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
		}

		public IConfiguration Configuration { get; }


		private const string ApiName = "Demo.Core";
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc(m=> {
				m.Filters.Add(typeof(GlobalExceptionsFilter))
			}).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
