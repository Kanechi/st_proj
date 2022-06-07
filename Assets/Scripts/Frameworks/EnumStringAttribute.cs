using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Reflection;



	/// <summary>
	/// enum に文字列属性付与
	/// 
	/// 例＞
	///		enum test {
	///			[EnumString("/test/debug/")]
	///			Enemy1,
	///		}
	///		
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
	public class EnumStringAttribute : Attribute{
		private string value_;
		public string Value => value_;
		public EnumStringAttribute(string v) =>value_ = v;
	}

	/// <summary>
	/// enum に設定された属性の抽出
	/// </summary>
	static public class EnumStringUtility {

        /// <summary>
        /// enum の値毎に属性を取得する
        /// </summary>
        /// <typeparam name="Type">enumの型</typeparam>
        /// <typeparam name="Attr">取得する属性</typeparam>
        /// <param name="callback">コールバック</param>
        static public void ForeachEnumAttribute<Type, Attr>(Action<Type, Attr> callback) where Attr : System.Attribute {
            var type = typeof(Type);
            foreach (var e in Enum.GetValues(type) as IEnumerable<Type>) {
                var info = type.GetField(e.ToString());
                var attr = CustomAttributeExtensions.GetCustomAttribute<Attr>(info);
                if (attr != null)
                    callback(e, attr);
            }
            return;
        }
    }

    /// <summary>
    /// 例
    /// </summary>
#if false
    public enum ePictureBookCategory {

        [EnumString("General/GE_Dishes")]
        Cooking,

        [EnumString("General/GE_Beitkuma")]
        Item,
    }

    static class ePictureBookCategoryExtention {
        static readonly private Dictionary<ePictureBookCategory, string> s_dic_ = new Dictionary<ePictureBookCategory, string>();
        static ePictureBookCategoryExtention() => EnumStringUtility.ForeachEnumAttribute<ePictureBookCategory, EnumStringAttribute>((e, attr) => { s_dic_.Add(e, attr.Value); });
        static public string ToEnumString(this ePictureBookCategory e) => s_dic_[e];
    }
#endif


