using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Demo.Core.Common.Cache.ExpressionCache
{
	public interface IExpressionCache<T> where T : class
	{
		T Get(Expression key, Func<Expression, T> creator);
	}
}
