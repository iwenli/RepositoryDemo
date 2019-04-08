using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Common.Middleware
{
	public class EncryptDecryptMiddleware
	{
		private readonly RequestDelegate _next;
		public EncryptDecryptMiddleware(RequestDelegate next)
		{ _next = next; }
		public async Task Invoke(HttpContext httpContext)
		{
			//判断是否为加密请求
			if (httpContext.Request.Headers.ContainsKey("aesrequest"))
			{
				//创建http的原始请求和响应流
				var reqOrigin = httpContext.Request.Body;
				var resOrigin = httpContext.Response.Body;
				try
				{
					using (var newReq = new MemoryStream())
					{
						//替换request 流
						httpContext.Request.Body = newReq;
						using (var newRes = new MemoryStream())
						{
							//替换response流
							httpContext.Response.Body = newRes;
							string reqStr;
							using (var streamReader = new StreamReader(reqOrigin))
							{ //读取原始请求的流的内容
								reqStr = streamReader.ReadToEnd();
							}
							////假设这里对读取的内容进行解密
							string writeStr = JsonConvert.SerializeObject(new { Name = "test" });
							//解密完之后把解密后内容写入新的request流去
							using (var streamWriter = new StreamWriter(newReq))
							{
								streamWriter.Write(writeStr);
								streamWriter.Flush();
								//此处一定要设置=0，否则controller的action里模型绑定不了数据
								newReq.Position = 0;
								//进入action
								await _next(httpContext);
							}
							string resStr;
							//读取action返回的结果
							using (var streamReader = new StreamReader(newRes))
							{
								newRes.Position = 0;
								resStr = streamReader.ReadToEnd();
							}
							//假设这里对action返回的结果进行加密
							string resWriteStr = "hello world";
							using (var streamWriter = new StreamWriter(resOrigin))
							{
								streamWriter.Write(resWriteStr);
							}
						}
					}
				}
				finally
				{
					//将原始的请求和响应流替换回去
					httpContext.Request.Body = reqOrigin;
					httpContext.Response.Body = resOrigin;
				}
			}
			else
			{
				await httpContext.Response.WriteAsync("don't receive non-encrpt request");
			}
		}

	}
}
