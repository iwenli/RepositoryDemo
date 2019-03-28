using Demo.Core.IRepository.Base;
using Demo.Core.IService.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.Model.Models;
using Demo.Core.Common.Attributes;

namespace Demo.Core.Service.Base
{
	public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : RootEntity
	{
		private readonly IBaseRepository<TEntity> _baseRepository;

		public BaseServices(IBaseRepository<TEntity> baseRepository)
		{
			_baseRepository = baseRepository;
		}

		/// <summary>
		/// 根据ID查询一条数据
		/// </summary>
		/// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
		/// <param name="blnUseCache">是否使用缓存</param>
		/// <returns>数据实体</returns>
		public async Task<TEntity> QueryByID(object objId, bool blnUseCache = false)
		{
			return await _baseRepository.QueryById(objId, blnUseCache);
		}

		/// <summary>
		/// 根据ID查询数据
		/// </summary>
		/// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
		/// <returns>数据实体列表</returns>
		public async Task<List<TEntity>> QueryByIDs(object[] lstIds)
		{
			return await _baseRepository.QueryByIDs(lstIds);
		}

		/// <summary>
		/// 写入实体数据
		/// </summary>
		/// <param name="entity">实体类</param>
		/// <returns></returns>
		public async Task<long> Add(TEntity entity)
		{
			return await _baseRepository.Add(entity);
		}

		/// <summary>
		/// 更新实体数据
		/// </summary>
		/// <param name="entity">实体</param>
		/// <param name="lstColumns">更新列</param>
		/// <param name="lstIgnoreColumns">忽略列</param>
		/// <param name="strWhere">更新条件</param>
		/// <returns></returns>
		public async Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null,
			string strWhere = "")
		{
			return await _baseRepository.Update(entity, lstColumns, lstIgnoreColumns, strWhere);
		}

		/// <summary>
		/// 根据实体删除一条数据
		/// </summary>
		/// <param name="entity">实体类</param>
		/// <returns></returns>
		public async Task<bool> Delete(TEntity entity)
		{
			return await _baseRepository.Delete(entity);
		}

		/// <summary>
		/// 删除指定ID的数据
		/// </summary>
		/// <param name="id">主键ID</param>
		/// <returns></returns>
		public async Task<bool> DeleteById(object id)
		{
			return await _baseRepository.DeleteById(id);
		}

		/// <summary>
		/// 删除指定ID集合的数据(批量删除)
		/// </summary>
		/// <param name="ids">主键ID集合</param>
		/// <returns></returns>
		public async Task<bool> DeleteByIds(object[] ids)
		{
			return await _baseRepository.DeleteByIds(ids);
		}



		/// <summary>
		/// 查询所有数据
		/// </summary>
		/// <returns>数据列表</returns>
		public async Task<List<TEntity>> Query()
		{
			return await _baseRepository.Query();
		}

		/// <summary>
		/// 查询数据列表
		/// </summary>
		/// <param name="strWhere">条件</param>
		/// <returns>数据列表</returns>
		public async Task<List<TEntity>> Query(string strWhere)
		{
			return await _baseRepository.Query(strWhere);
		}

		/// <summary>
		/// 查询数据列表
		/// </summary>
		/// <param name="whereExpression">whereExpression</param>
		/// <returns>数据列表</returns>
		[Caching(AbsoluteExpiration = 10)]
		public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
		{
			return await _baseRepository.Query(whereExpression);
		}

		/// <summary>
		/// 查询一个列表
		/// </summary>
		/// <param name="whereExpression">条件表达式</param>
		/// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
		/// <returns>数据列表</returns>
		public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
		{
			return await _baseRepository.Query(whereExpression, strOrderByFileds);
		}
		/// <summary>
		/// 查询一个列表
		/// </summary>
		/// <param name="whereExpression">条件表达式</param>
		/// <param name="orderByExpression">排序表达式</param>
		/// <param name="isAsc"></param>
		/// <returns></returns>
		public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
		{
			return await _baseRepository.Query(whereExpression, orderByExpression, isAsc);
		}

		/// <summary>
		/// 查询一个列表
		/// </summary>
		/// <param name="strWhere">条件</param>
		/// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
		/// <returns>数据列表</returns>
		public async Task<List<TEntity>> Query(string strWhere, string strOrderByFileds)
		{
			return await _baseRepository.Query(strWhere, strOrderByFileds);
		}


		/// <summary>
		/// 查询前N条数据
		/// </summary>
		/// <param name="whereExpression">条件表达式</param>
		/// <param name="intTop">前N条</param>
		/// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
		/// <returns>数据列表</returns>
		public async Task<List<TEntity>> Query(
			Expression<Func<TEntity, bool>> whereExpression,
			int intTop,
			string strOrderByFileds)
		{
			return await _baseRepository.Query(whereExpression, intTop, strOrderByFileds);
		}

		/// <summary>
		/// 查询前N条数据
		/// </summary>
		/// <param name="strWhere">条件</param>
		/// <param name="intTop">前N条</param>
		/// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
		/// <returns>数据列表</returns>
		public async Task<List<TEntity>> Query(
			string strWhere,
			int intTop,
			string strOrderByFileds)
		{
			return await _baseRepository.Query(strWhere, intTop, strOrderByFileds);
		}



		/// <summary>
		/// 分页查询
		/// </summary>
		/// <param name="whereExpression">条件表达式</param>
		/// <param name="intPageIndex">页码（下标0）</param>
		/// <param name="intPageSize">页大小</param>
		/// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
		/// <returns>数据列表</returns>
		public async Task<List<TEntity>> QueryPage(
			Expression<Func<TEntity, bool>> whereExpression,
			int intPageIndex = 0,
			int intPageSize = 20,
			string strOrderByFileds = null)
		{
			return await _baseRepository.QueryPage(whereExpression, intPageIndex, intPageSize, strOrderByFileds);
		}

		/// <summary>
		/// 分页查询
		/// </summary>
		/// <param name="strWhere">条件</param>
		/// <param name="intPageIndex">页码（下标0）</param>
		/// <param name="intPageSize">页大小</param>
		/// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
		/// <returns>数据列表</returns>
		public async Task<List<TEntity>> QueryPage(
		  string strWhere,
		  int intPageIndex = 0,
		  int intPageSize = 20,
		  string strOrderByFileds = null)
		{
			return await _baseRepository.QueryPage(strWhere, intPageIndex, intPageSize, strOrderByFileds);
		}
	}
}
