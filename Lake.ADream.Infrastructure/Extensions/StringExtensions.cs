
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    ///  //扩展方法必须是静态的
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// IsEmail
        /// </summary>
        /// <param name="_input"></param>
        /// <returns></returns>
        public static bool IsEmail(this string _input)
        {
            return Regex.IsMatch(_input, @"^\w+@\w+\.\w+$");
        }
        /// <summary>
        ///   带多个参数的扩展方法
        //在原始字符串前后加上指定的字符
        /// </summary>
        /// <param name="_input"></param>
        /// <param name="_quot"></param>
        /// <returns></returns>
        public static string Quot(this string _input, string _quot)
        {
            return _quot + _input + _quot;
        }

        /// <summary>
        /// 是否为Guid
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsGuid(this string input)
        {
            return Guid.TryParse(input, out Guid g);
        }

        #region 数据转换

        #region 转Int
        /// <summary>
        /// 转Int,失败返回0
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int ToInt(this string t)
        {
            int n;
            if (!int.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary>
        /// 转Int,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static int ToInt(this string t, int pReturn)
        {
            int n;
            if (!int.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary>
        /// 是否是Int true:是 false:否
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsInt(this string t)
        {
            int n;
            return int.TryParse(t, out n);
        }
        #endregion

        #region 转Int16
        /// <summary>
        /// 转Int,失败返回0
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static Int16 ToInt16(this string t)
        {
            Int16 n;
            if (!Int16.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary>
        /// 转Int,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static Int16 ToInt16(this string t, Int16 pReturn)
        {
            Int16 n;
            if (!Int16.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary>
        /// 是否是Int true:是 false:否
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsInt16(this string t)
        {
            Int16 n;
            return Int16.TryParse(t, out n);
        }
        #endregion

        #region 转byte
        /// <summary>
        /// 转byte,失败返回0
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static byte Tobyte(this string t)
        {
            byte n;
            if (!byte.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary>
        /// 转byte,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static byte Tobyte(this string t, byte pReturn)
        {
            byte n;
            if (!byte.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary>
        /// 是否是byte true:是 false:否
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Isbyte(this string t)
        {
            byte n;
            return byte.TryParse(t, out n);
        }
        #endregion

        #region 转Long
        /// <summary>
        /// 转Long,失败返回0
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static long ToLong(this string t)
        {

            long n;
            if (!long.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary>
        /// 转Long,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static long ToLong(this string t, long pReturn)
        {
            long n;
            if (!long.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary>
        /// 是否是Long true:是 false:否
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsLong(this string t)
        {
            long n;
            return long.TryParse(t, out n);
        }
        #endregion

        #region 转Double
        /// <summary>
        /// 转Int,失败返回0
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static double ToDouble(this string t)
        {
            double n;
            if (!double.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary>
        /// 转Double,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static double ToDouble(this string t, double pReturn)
        {
            double n;
            if (!double.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary>
        /// 是否是Double true:是 false:否
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsDouble(this string t)
        {
            double n;
            return double.TryParse(t, out n);
        }
        #endregion

        #region 转Decimal
        /// <summary>
        /// 转Decimal,失败返回0
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string t)
        {
            decimal n;
            if (!decimal.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary>
        /// 转Decimal,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string t, decimal pReturn)
        {
            decimal n;
            if (!decimal.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary>
        /// 是否是Decimal true:是 false:否
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsDecimal(this string t)
        {
            decimal n;
            return decimal.TryParse(t, out n);
        }
        #endregion

        #region 转DateTime
        /// <summary>
        /// 转DateTime,失败返回当前时间
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string t)
        {
            DateTime n;
            if (!DateTime.TryParse(t, out n))
                return DateTime.Now;
            return n;
        }

        /// <summary>
        /// 转DateTime,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string t, DateTime pReturn)
        {
            DateTime n;
            if (!DateTime.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary>
        /// 转DateTime,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static string ToDateTime(this string t, string pFormat, string pReturn)
        {
            DateTime n;
            if (!DateTime.TryParse(t, out n))
                return pReturn;
            return n.ToString(pFormat);
        }

        /// <summary>
        /// 转DateTime,失败返回空
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static string ToDateTime(this string t, string pFormat)
        {
            return t.ToDateTime(pFormat, string.Empty);
        }

        public static string ToShortDateTime(this string t)
        {
            return t.ToDateTime("yyyy-MM-dd", string.Empty);
        }

        /// <summary>
        /// 是否是DateTime true:是 false:否
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string t)
        {
            DateTime n;
            return DateTime.TryParse(t, out n);
        }
        #endregion

        #region 与int[]相关
        /// <summary>
        /// 转int[],字符串以逗号(,)隔开,请确保字符串内容都合法,否则会出错
        /// </summary>
        /// <param name="pStr"></param>
        /// <returns></returns>
        public static int[] ToIntArr(this string t)
        {
            return t.ToIntArr(new char[] { ',' });
        }

        /// <summary>
        /// 转int[],字符串以逗号(,)隔开,请确保字符串内容都合法,否则会出错
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pSplit">隔开的</param>
        /// <returns></returns>
        public static int[] ToIntArr(this string t, char[] pSplit)
        {
            if (t.Length == 0)
            {
                return new int[] { };
            }

            string[] ArrStr = t.Split(pSplit, StringSplitOptions.None);
            int[] iStr = new int[ArrStr.Length];

            for (int i = 0; i < ArrStr.Length; i++)
                iStr[i] = int.Parse(ArrStr[i]);

            return iStr;
        }
        public static bool IsTelephone(this string telephone)
        {
            return Regex.IsMatch(telephone, @"^(\d{3,4}-)?\d{6,8}$");
        }

        public static bool IsPhone(this string phone)
        {
            return Regex.IsMatch(phone, @"^[1]+[3,4,5,7,8]+\d{9}");
        }

        public static bool IsIDcard(this string str_idcard)
        {
            return Regex.IsMatch(str_idcard, @"(^\d{18}$)|(^\d{15}$)");
        }

        /// <summary>
        /// 是否为邮政编码
        /// </summary>
        /// <param name="str_postalcode"></param>
        /// <returns></returns>
        public static bool IsPostalcode(this string str_postalcode)
        {
            return Regex.IsMatch(str_postalcode, @"^\d{6}$");
        }
        public static bool IsChinese(this string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fbb]");
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return String.IsNullOrEmpty(str);
        }
        public static bool IsNotNullOrEmpty(this string str)
        {
            return !String.IsNullOrEmpty(str);
        }
        #endregion

        #region 过滤字符串的非int,重新组合成字符串
        /// <summary>
        /// 过滤字符串的非int,重新组合成字符串
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pSplit">分隔符</param>
        /// <returns></returns>
        public static string ClearNoInt(this string t, char pSplit)
        {
            string sStr = string.Empty;
            string[] ArrStr = t.Split(pSplit);

            for (int i = 0; i < ArrStr.Length; i++)
            {
                string lsStr = ArrStr[i];

                if (lsStr.IsInt())
                    sStr += lsStr + pSplit;
                else
                    continue;
            }

            if (sStr.Length > 0)
                sStr = sStr.TrimEnd(pSplit);

            return sStr;
        }

        /// <summary>
        /// 过滤字符串的非int,重新组合成字符串
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ClearNoInt(this string t)
        {
            return t.ClearNoInt(',');
        }
        #endregion

        #region 是否可以转换成int[]
        /// <summary>
        /// 是否可以转换成int[],true:是,false:否
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pSplit">分隔符</param>
        /// <returns></returns>
        public static bool IsIntArr(this string t, char pSplit)
        {
            string[] ArrStr = t.Split(pSplit);
            bool b = true;

            for (int i = 0; i < ArrStr.Length; i++)
            {
                if (!ArrStr[i].IsInt())
                {
                    b = false;
                    break;
                }
            }

            return b;
        }
        /// <summary>
        /// 把字符串转为同名的类型
        /// </summary>
        /// <param name="classname"></param>
        /// <param name="input"></param>
        /// <returns>T</returns>
        public static T CustomedConvert<T>(this string input, T classname) where T : class
        {
            if (classname.GetType() == typeof(string))
            {
                return input as T;
            }

            object result = null;
            result = System.ComponentModel.TypeDescriptor.GetConverter(classname.GetType()).ConvertFrom(input);
            return result as T;
        }
        /// <summary>
        /// 是否可以转换成int[],true:是,false:否
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsIntArr(this string t)
        {
            return t.IsIntArr(',');
        }
        #endregion

        #endregion

        #region 载取左字符
        /// <summary>
        /// 载取左字符
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pLen">字符个数</param>
        /// <param name="pReturn">超出时后边要加的返回的内容</param>
        /// <returns></returns>
        public static string Left(this string t, int pLen, string pReturn)
        {
            if (t == null || t.Length == 0)
                return string.Empty;
            pLen *= 2;
            int i = 0, j = 0;
            foreach (char c in t)
            {
                if (c > 127)
                {
                    i += 2;
                }
                else
                {
                    i++;
                }

                if (i > pLen)
                {
                    return t.Substring(0, j) + pReturn;
                }

                j++;
            }

            return t;
        }

        public static string Left(this string t, int pLen)
        {
            return Left(t, pLen, string.Empty);
        }

        public static string StrLeft(this string t, int pLen)
        {
            if (t == null)
            {
                return "";
            }
            if (t.Length > pLen)
            {
                return t.Substring(0, pLen);
            }
            return t;
        }
        #endregion

        #region 删除文件名或路径的特殊字符

        private class ClearPathUnsafeList
        {
            public static readonly string[] unSafeStr = { "/", "\\", ":", "*", "?", "\"", "<", ">", "|" };
        }

        /// <summary>
        /// 删除文件名或路径的特殊字符
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ClearPathUnsafe(this string t)
        {
            foreach (string s in ClearPathUnsafeList.unSafeStr)
            {
                t = t.Replace(s, "");
            }

            return t;
        }
        #endregion

        #region 字符串真实长度 如:一个汉字为两个字节
        /// <summary>
        /// 字符串真实长度 如:一个汉字为两个字节
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int LengthReal(this string s)
        {
            return Encoding.Unicode.GetBytes(s).Length;
        }
        #endregion

        #region 去除小数位最后为0的
        /// <summary>
        /// 去除小数位最后为0的
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static decimal ClearDecimal0(this string t)
        {
            decimal d;
            if (decimal.TryParse(t, out d))
            {
                return decimal.Parse(double.Parse(d.ToString("g")).ToString());
            }
            return 0;
        }
        #endregion

        #region 进制转换
        /// <summary>
        /// 16进制转二进制
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string Change16To2(this string t)
        {
            String BinOne = string.Empty;
            String BinAll = string.Empty;
            char[] nums = t.ToCharArray();
            for (int i = 0; i < nums.Length; i++)
            {
                string number = nums[i].ToString();
                int num = Int32.Parse(number, System.Globalization.NumberStyles.HexNumber);

                BinOne = Convert.ToString(num, 2).PadLeft(4, '0');
                BinAll = BinAll + BinOne;
            }
            return BinAll;
        }

        /// <summary>
        /// 二进制转十进制
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Int64 Change2To10(this string t)
        {
            char[] arrc = t.ToCharArray();
            Int64 all = 0, indexC = 1;
            for (int i = arrc.Length - 1; i >= 0; i--)
            {
                if (arrc[i] == '1')
                {
                    all += indexC;
                }
                indexC = indexC * 2;
            }

            return all;
        }

        /// <summary>
        /// 二进制转换byte[]数组
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] Change2ToBytes(this string t)
        {
            List<byte> list = new List<byte>();

            char[] arrc = t.ToCharArray();
            byte n = 0;
            char c;
            int j = 0;
            //倒序获取位
            for (int i = arrc.Length - 1; i >= 0; i--)
            {
                c = arrc[i];

                if (c == '1')
                {
                    n += Convert.ToByte(Math.Pow(2, j));
                }
                j++;

                if (j % 8 == 0)
                {
                    list.Add(n);
                    j = 0;
                    n = 0;
                }
            }

            //剩余最高位
            if (n > 0)
                list.Add(n);

            byte[] arrb = new byte[list.Count];

            int j1 = 0;
            //倒序
            for (int i = list.Count - 1; i >= 0; i--)
            {
                arrb[j1] = list[i];
                j1++;
            }
            return arrb;
        }

        /// <summary>
        /// 二进制转化为索引id数据,从右到左
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int[] Change2ToIndex(this string t)
        {
            List<int> list = new List<int>();
            char[] arrc = t.ToCharArray();
            char c;
            int j = 0;

            //倒序获取位
            for (int i = arrc.Length - 1; i >= 0; i--)
            {
                j++;
                c = arrc[i];

                if (c == '1')
                {
                    list.Add(j);
                }
            }

            return list.ToArray();
        }
        #endregion
   
    }
}
