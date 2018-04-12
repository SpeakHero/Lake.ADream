//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Lake.ADream.Infrastructure.Utilities
//{
//    class ObjectExtensions
//    {
//        /// <summary>
//        /// 将 json 字符串转换为指定类型的对象表示形式。
//        /// </summary>
//        /// <typeparam name="T">要转换成的对象类型。</typeparam>
//        /// <param name="json">json 字符串。</param>
//        /// <returns>转换完后的 JSON 对象。</returns>
//        public static T ToJson<T>(this object json)
//        {
//            #region 参数校验

//            if (string.IsNullOrEmpty(json))
//                throw new StringNullOrEmptyException(nameof(json));

//            #endregion

//            ISerializer serializer = GlobalConfig.SerializerFactory.GetJsonSerializer();
//            return serializer.Deserialize<T>(json);
//        }
//    }
//}
