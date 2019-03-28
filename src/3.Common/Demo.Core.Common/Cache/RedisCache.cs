using Demo.Core.Common.Helper;
using StackExchange.Redis;
using System;

namespace Demo.Core.Common.Cache
{
	/// <summary>
	/// Redis 缓存实现
	/// </summary>
	public class RedisCache : IRedisCache
	{
		private readonly string _configration;
		private volatile ConnectionMultiplexer _connectionMultiplexer = null;
		private readonly object _localObject = new object();

		public RedisCache()
		{
			_configration = AppsettingsHelper.Get(new string[] { "AppSettings", "RedisCaching", "ConnectionString" });
			if (string.IsNullOrEmpty(_configration))
			{
				throw new ArgumentException("redis config is empty", nameof(_configration));
			}
		}

		/// <summary>
		/// Redis连接池
		/// </summary>
		public ConnectionMultiplexer ConnectionMultiplexer
		{
			get
			{
				if (_connectionMultiplexer == null || !_connectionMultiplexer.IsConnected)
				{
					lock (_localObject)
					{
						if (_connectionMultiplexer == null || !_connectionMultiplexer.IsConnected)
						{
							try
							{
								_connectionMultiplexer = StackExchange.Redis.ConnectionMultiplexer.Connect(_configration);
							}
							catch (Exception ex)
							{
								throw ex;
								//throw new Exception("Redis服务未启用，请开启该服务");
							}
						}
					}
				}
				return _connectionMultiplexer;
			}
		}

		public string GetValue(string key)
		{
			return ConnectionMultiplexer.GetDatabase().StringGet(key);
		}

		public TEntity Get<TEntity>(string key)
		{
			var value = ConnectionMultiplexer.GetDatabase().StringGet(key);
			return value.HasValue ? SerializeHelper.Deserialize<TEntity>(value) : default(TEntity);
		}

		public void Set(string key, object value, TimeSpan cacheTime)
		{
			if (value != null)
			{
				ConnectionMultiplexer.GetDatabase().StringSet(key, SerializeHelper.Serialize(value), cacheTime);
			}
		}

		public bool Exists(string key)
		{
			return ConnectionMultiplexer.GetDatabase().KeyExists(key);
		}

		public void Remove(string key)
		{
			ConnectionMultiplexer.GetDatabase().KeyDelete(key);
		}

		public void Clear()
		{
			foreach (var endPoint in ConnectionMultiplexer.GetEndPoints())
			{
				var server = ConnectionMultiplexer.GetServer(endPoint);
				foreach (var key in server.Keys())
				{
					ConnectionMultiplexer.GetDatabase().KeyDelete(key);
				}
			}
		}
	}
}
