using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Core.Common.Extension
{
	/// <summary>
	/// 
	/// </summary>
	public static class UtilConvert
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="thisValue"></param>
		/// <returns></returns>
		public static int ObjToInt(this object thisValue)
		{
			if (thisValue == null) return 0;
			if (thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out var result))
			{
				return result;
			}
			return 0;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="thisValue"></param>
		/// <param name="errorValue"></param>
		/// <returns></returns>
		public static int ObjToInt(this object thisValue, int errorValue)
		{
			if (thisValue != null && thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out var result))
			{
				return result;
			}
			return errorValue;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="thisValue"></param>
		/// <returns></returns>
		public static double ObjToMoney(this object thisValue)
		{
			if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out var result))
			{
				return result;
			}
			return 0;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="thisValue"></param>
		/// <param name="errorValue"></param>
		/// <returns></returns>
		public static double ObjToMoney(this object thisValue, double errorValue)
		{
			if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out var result))
			{
				return result;
			}
			return errorValue;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="thisValue"></param>
		/// <returns></returns>
		public static string ObjToString(this object thisValue)
		{
			return thisValue != null ? thisValue.ToString().Trim() : "";
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="thisValue"></param>
		/// <param name="errorValue"></param>
		/// <returns></returns>
		public static string ObjToString(this object thisValue, string errorValue)
		{
			return thisValue != null ? thisValue.ToString().Trim() : errorValue;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="thisValue"></param>
		/// <returns></returns>
		public static Decimal ObjToDecimal(this object thisValue)
		{
			if (thisValue != null && thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out var result))
			{
				return result;
			}
			return 0;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="thisValue"></param>
		/// <param name="errorValue"></param>
		/// <returns></returns>
		public static Decimal ObjToDecimal(this object thisValue, decimal errorValue)
		{
			if (thisValue != null && thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out var result))
			{
				return result;
			}
			return errorValue;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="thisValue"></param>
		/// <returns></returns>
		public static DateTime ObjToDate(this object thisValue)
		{ 
			if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out var result))
			{
				return Convert.ToDateTime(thisValue);
			}
			return DateTime.MinValue;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="thisValue"></param>
		/// <param name="errorValue"></param>
		/// <returns></returns>
		public static DateTime ObjToDate(this object thisValue, DateTime errorValue)
		{
			if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out var result))
			{
				return result;
			}
			return errorValue;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="thisValue"></param>
		/// <returns></returns>
		public static bool ObjToBool(this object thisValue)
		{
			if (thisValue != null && thisValue != DBNull.Value && bool.TryParse(thisValue.ToString(), out var result))
			{
				return result;
			}
			return false;
		}
	}
}
