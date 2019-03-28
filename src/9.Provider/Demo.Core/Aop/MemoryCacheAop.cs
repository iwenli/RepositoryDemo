using Castle.DynamicProxy;
using Demo.Core.Common.Attributes;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;

namespace Demo.Core.Aop
{
	public class MemoryCacheAop : IInterceptor
	{
		private readonly IMemoryCache _cache;

		public MemoryCacheAop(IMemoryCache cache)
		{
			_cache = cache;
		}
		public void Intercept(IInvocation invocation)
		{
			var cacheKey = "";
			var method = invocation.MethodInvocationTarget ?? invocation.Method;
			if (method.GetCustomAttributes(true).Any(m => m.GetType() == typeof(CachingAttribute)))
			{
				//获取自定义缓存键
				cacheKey = CustomCacheKey(invocation);
				//根据key获取相应的缓存值
				var cacheValue = _cache.Get(cacheKey);
				if (cacheValue != null)
				{
					//将当前获取到的缓存值，赋值给当前执行方法
					invocation.ReturnValue = cacheValue;
					return;
				} 
			}
			//去执行当前的方法
			invocation.Proceed();
			//存入缓存
			if (!string.IsNullOrWhiteSpace(cacheKey))
			{
				_cache.Set(cacheKey, invocation.ReturnValue);
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
			var methodArguments = invocation.Arguments.Select(GetArgumentValue).Take(3).ToList();
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
