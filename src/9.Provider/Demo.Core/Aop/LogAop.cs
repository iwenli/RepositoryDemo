using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Demo.Core.Log;

namespace Demo.Core.Aop
{
	public class LogAop : IInterceptor
	{
		private readonly ILoggerHelper _log;

		public LogAop(ILoggerHelper log)
		{
			_log = log;
		}
		/// <summary>
		/// 实例化IInterceptor唯一方法
		/// </summary>
		/// <param name="invocation">包含被拦截的方法的信息</param>
		public void Intercept(IInvocation invocation)
		{
			//记录被拦截方法的日志信息
			Exception exception = null;
			var logMsg = $"{DateTime.Now:yyyyMMddHHmmss}=>当前执行方法：{invocation.TargetType.Name}_{invocation.Method.Name} 参数是：{string.Join(",", invocation.Arguments.Select(m => (m ?? "").ToString()).ToArray())}{Environment.NewLine}";
			try
			{
				invocation.Proceed();
			}
			catch (Exception e)
			{
				exception = e;
				   logMsg += $"方法执行异";
			}
			logMsg += $"方法执行完毕，返回结果：{invocation.ReturnValue}";
			if (exception != null)
			{
				_log.Error(typeof(LogAop), logMsg, exception);
			}
			else
			{
				_log.Debug(typeof(LogAop), logMsg);
			}

		}
	}
}
