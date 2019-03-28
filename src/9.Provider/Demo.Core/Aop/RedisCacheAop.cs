using Castle.DynamicProxy;
using Demo.Core.Common.Attributes;
using Demo.Core.Common.Cache;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Core.Aop
{
	public class RedisCacheAop : IInterceptor
	{
		private readonly IRedisCache _cache;

		public RedisCacheAop(IRedisCache cache)
		{
			_cache = cache;
		}
		public void Intercept(IInvocation invocation)
		{
			var cacheKey = "";
			object response;
			var returnType = invocation.Method.ReturnType;
			if (returnType.FullName == "System.Void")
			{
				return;
			}
			var method = invocation.MethodInvocationTarget ?? invocation.Method; 

			if (method.GetCustomAttributes(true).Any(m => m.GetType() == typeof(CachingAttribute)))
			{
				//获取自定义缓存键
				cacheKey = CustomCacheKey(invocation);
				//根据key获取相应的缓存值
				var cacheValue = _cache.GetValue(cacheKey);
				if (cacheValue != null)
				{
					var returnTypeArguments = returnType.GenericTypeArguments;
					if (typeof(Task).IsAssignableFrom(returnType))
					{
						//返回Task<T>
						if (returnTypeArguments.Length > 0)
						{
							var returnTypeArgument = returnTypeArguments.FirstOrDefault();
							// 核心1，直接获取 dynamic 类型
							dynamic temp = Newtonsoft.Json.JsonConvert.DeserializeObject(cacheValue, returnTypeArgument);
							//dynamic temp = System.Convert.ChangeType(cacheValue, resultType);
							// System.Convert.ChangeType(Task.FromResult(temp), type);
							response = Task.FromResult(temp);
						}
						else
						{
							response = Task.Yield();
						}
					}
					else
					{
						// 核心2，要进行 ChangeType
						response = System.Convert.ChangeType(_cache.Get<object>(cacheKey), returnType);
					}

					//将当前获取到的缓存值，赋值给当前执行方法
					invocation.ReturnValue = response;
					return;
				} 
			}
			//去执行当前的方法
			invocation.Proceed();
			//存入缓存
			if (!string.IsNullOrWhiteSpace(cacheKey))
			{
				if (typeof(Task).IsAssignableFrom(returnType))
				{
					var resultProperty = returnType.GetProperty("Result");
					response = resultProperty.GetValue(invocation.ReturnValue);
				}
				else
				{
					response = invocation.ReturnValue;
				}
				if (response == null) response = string.Empty;
				var abs = ((CachingAttribute)method.GetCustomAttributes(true)
					          .FirstOrDefault(m => m.GetType() == typeof(CachingAttribute)))?.AbsoluteExpiration ?? 10;
				_cache.Set(cacheKey, response, TimeSpan.FromMinutes(abs));
			}
		}

		/// <summary>
		/// 自定义缓存键
		/// </summary>
		/// <param name="invocation"></param>
		/// <returns></returns>
		private string CustomCacheKey(IInvocation invocation)
		{
			var typeName = invocation.TargetType.Name;
			var methodName = invocation.Method.Name;
			//获取参数列表，我最多需要三个即可
			var methodArguments = invocation.Arguments.Select(m=>GetArgumentValue(m)).Take(3).ToList();
			string key = $"{typeName}:{methodName}:";
			foreach (var param in methodArguments)
			{
				key += $"{param}:";
			}

			return key.TrimEnd(':');
		}
		/// <summary>
		/// object 转 string
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		private string GetArgumentValue(object arg)
		{
			if (arg is int || arg is long || arg is string)
				return arg.ToString();

			if (arg is DateTime)
				return ((DateTime)arg).ToString("yyyyMMddHHms");

			return "";
		}
	}
}
