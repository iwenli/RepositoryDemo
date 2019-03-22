using Demo.Core.IService.Base;
using Demo.Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Core.IService
{
	public interface IAdvertisementServices : IBaseServices<Advertisement>
	{
		//int Sum(int i, int j);
		//int Add(Advertisement model);
		//bool Delete(Advertisement model);
		//bool Update(Advertisement model);
		//List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression);

		void ReturnExp();
	}
}
