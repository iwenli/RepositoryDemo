using Demo.Core.Model.Models;
using Demo.Core.Service.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.IRepository.Base;
using Demo.Core.IService.Blog;

namespace Demo.Core.Service.Blog
{
	public class BlogArticleServices : BaseServices<BlogArticle>, IBlogArticleServices
	{
		private readonly IBaseRepository<BlogArticle> _repository;

		public BlogArticleServices(IBaseRepository<BlogArticle> repository) : base(repository)
		{
			_repository = repository;
		}
		///// <summary>
		///// 获取博客列表
		///// </summary>
		///// <param name="id"></param>
		///// <returns></returns>
		//public async Task<List<BlogArticle>> getBlogs()
		//{
		//	var bloglist = await dal.Query(a => a.bID > 0, a => a.bID);

		//	return bloglist;

		//}
	}
}
