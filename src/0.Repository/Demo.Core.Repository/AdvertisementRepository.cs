using Demo.Core.IRepository;
using Demo.Core.Model.Models;
using Demo.Core.Repository.Base;

namespace Demo.Core.Repository
{
	public class AdvertisementRepository : BaseRepository<Advertisement>, IAdvertisementRepository
	{
	}
}
