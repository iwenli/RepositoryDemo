using Demo.Core.IRepository;
using Demo.Core.IService;
using Demo.Core.Model.Models;
using Demo.Core.Service.Base;

namespace Demo.Core.Service
{
	public class AdvertisementServices : BaseServices<Advertisement>, IAdvertisementServices
	{
		private readonly IAdvertisementRepository _advertisementRepository;
		public AdvertisementServices(IAdvertisementRepository advertisementRepository)
		{
			_advertisementRepository = advertisementRepository;
			base.baseDal = _advertisementRepository;
		}

		public void ReturnExp()
		{

			int a = 1;
			int b = 0;

			int c = a / b;
		}

		//public IAdvertisementRepository dal = new AdvertisementRepository();
		//public int Sum(int i, int j)
		//{
		//    return dal.Sum(i, j);

		//}


		//public int Add(Advertisement model)
		//{
		//    return dal.Add(model);
		//}

		//public bool Delete(Advertisement model)
		//{
		//    return dal.Delete(model);
		//}

		//public List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression)
		//{
		//    return dal.Query(whereExpression);

		//}

		//public bool Update(Advertisement model)
		//{
		//    return dal.Update(model);
		//}

	}
}
