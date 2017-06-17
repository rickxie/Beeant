using System;
using System.Collections.Generic;
using System.Linq;


namespace Winner.Search.Analysis
{
    public class StandardAnalyzer :  IAnalyzer
    {
    
        #region 属性
        /// <summary>
        /// 分组主词库
        /// </summary>
        protected IDictionary<int, string[]> MainGroupDictionaries { get; set; }
        private string[] _mainDictionaries=new string[0];
        /// <summary>
        /// 主词库
        /// </summary>
        public string[] MainDictionaries
        {
            get { return _mainDictionaries; }
            set {
                _mainDictionaries = value;
                MainGroupDictionaries = GetGroupDictionaries(value);
            }
        }
        /// <summary>
        /// 分组主词库
        /// </summary>
        protected IDictionary<int, string[]> StopGroupDictionaries { get; set; }
        private string[] _stopDictionaries=new string[0];

        /// <summary>
        /// 禁用词库
        /// </summary>
        public string[] StopDictionaries
        {
            get { return _stopDictionaries; }
            set
            {
                _stopDictionaries = value;
                StopGroupDictionaries = GetGroupDictionaries(value);
            }
        }
        /// <summary>
        /// 分组主词库
        /// </summary>
        protected IDictionary<int, KeyValuePair<string, string>[]> TransformGroupDictionaries { get; set; }
        private KeyValuePair<string, string>[] _transformDictionaries = new KeyValuePair<string, string>[0];
        /// <summary>
        /// 转换词库
        /// </summary>
        public KeyValuePair<string, string>[] TransformDictionaries
        {
            get { return _transformDictionaries; }
            set
            {
                _transformDictionaries = value;
                TransformGroupDictionaries = GetGroupDictionaries(value);
            }
        }

        private string[] _splitDictionaries = new[]
            {
               "\r\n", "\r", "\n", "！", "@", "#", "￥", "%", "……", "&", "*", "（", "）", "——", "【",
                 "】",  "；", "“", "‘", "《", "，", "》", "。", "？"
                 ,"~","`","!","#","$","%","^","&","*","(",")",":",";","'","\"",",","<",">",".","?","/","|","\\"
            };
        /// <summary>
        /// 禁用词库
        /// </summary>
        public string[] SplitDictionaries
        {
            get { return _splitDictionaries; }
            set
            {
                _splitDictionaries = value;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 无参数
        /// </summary>
        public StandardAnalyzer()
        {
          
        }

        /// <summary>
        /// 拆分字符串,词库实例,拼音实例
        /// </summary>
        /// <param name="mainDictionaries"></param>
        /// <param name="stopDictionaries"></param>
        /// <param name="rootDictionaries"></param>
        public StandardAnalyzer(string[] mainDictionaries, string[] stopDictionaries, KeyValuePair<string, string>[] rootDictionaries)
        {
            MainDictionaries = mainDictionaries;
            StopDictionaries = stopDictionaries;
            TransformDictionaries = rootDictionaries;
        }
        #endregion

        #region 接口的实现
        /// <summary>
        /// 分词
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual IList<TermInfo> Resolve(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            input = input.ToLower();
            var keys = SplitEnglishOrNumber(input);
            var tokens = Tokenize(keys);
            var terms = Linguisticize(tokens);
            terms = FilterStop(terms);
            return terms;
        }
        #endregion

        #region 过滤禁用词
        /// <summary>
        /// 过滤禁用词
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        protected virtual IList<TermInfo> FilterStop(IList<TermInfo> terms)
        {
            return terms.Where(term => !IsStopWord(term.Name)).ToList();
        }

        /// <summary>
        /// 是否为禁用词
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual bool IsStopWord(string name)
        {
            if (!StopGroupDictionaries.ContainsKey(name.Length))
                return false;
            var dictionaries = StopGroupDictionaries[name.Length];
            int low = 0, high = dictionaries.Length - 1;
            while (low <= high)
            {
                int mid = low + ((high - low) / 2);
                if (dictionaries[mid].Equals(name))//找到该词
                    return true;
                if (dictionaries[mid].CompareTo(name) > 0)
                    high = mid - 1;
                else
                    low = mid + 1;
            }
            return false;
        }
        #endregion

        #region 转换元词
        /// <summary>
        /// 转换元词
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        protected virtual IList<TermInfo> Linguisticize(IList<TokenInfo> tokens)
        {
            var terms = new List<TermInfo>();
            foreach (var token in tokens)
            {
                var term = new TermInfo {Name = token.Name, Token = token};
                Lemmatize(term);
                terms.Add(term);
            }
            return terms;
        }
        /// <summary>
        /// 转换词根
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        protected virtual bool Lemmatize(TermInfo term)
        {
            if (!TransformGroupDictionaries.ContainsKey(term.Name.Length))
                return false;
            var dictionaries = TransformGroupDictionaries[term.Name.Length];
            int low = 0, high = dictionaries.Length - 1;
            while (low <= high)
            {
                int mid = low + ((high - low) / 2);
                if (dictionaries[mid].Key.Equals(term.Token.Name)) //找到该词
                {
                    term.Name = dictionaries[mid].Value;
                    return true;  
                }

                if (dictionaries[mid].Key.CompareTo(term.Name) > 0)
                    high = mid - 1;
                else
                    low = mid + 1;
            }
            return false;
        }

        #endregion

        #region 得到Token
        
        /// <summary>
        /// 得到词源
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        protected virtual IList<TokenInfo> Tokenize(IList<string> keys)
        {
            var tokens = new List<TokenInfo>();
            if (MainGroupDictionaries.Count > 0)
            {
                var maxLength = MainGroupDictionaries.Keys.Max(it => it);
                var minLength = MainGroupDictionaries.Keys.Min(it => it);
                foreach (var key in keys)
                {
                    AddTokensByForward(tokens, key, maxLength, minLength);
                }
            }
            return tokens;
        }

        /// <summary>
        /// 正向最大匹配,返回切片
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="key"></param>
        /// <param name="maxLength"></param>
        /// <param name="minLength"></param>
        /// <returns></returns>
        protected virtual void AddTokensByForward(IList<TokenInfo> tokens, string key, int maxLength, int minLength)
        {
            int index = 0;
            while (index < key.Length)
            {
                var length = index + maxLength > key.Length ? key.Length - index : maxLength;
                if (length < minLength)
                    break;
                var name = key.Substring(index, length);
                int i = name.Length;
                for (; i > 0; i--)
                {
                    if (i < minLength)
                    {
                        i = 0;
                        break;
                    }
                    string word = name.Substring(0, i);
                    if (IsMainWord(word))
                    {
                        tokens.Add(new TokenInfo {Name = word});
                        break;
                    }
                }
                index += (i == 0 ? 1 : i);
            }
        }

        /// <summary>
        /// 反向最大匹配,返回切片
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="key"></param>
        /// <param name="maxLength"></param>
        /// <param name="minLength"></param>
        /// <returns></returns>
        protected virtual void AddTokensByOpposite(IList<TokenInfo> tokens, string key, int maxLength, int minLength)
        {
            int index = key.Length;
            while (index >= 0)
            {
                var length = index - maxLength < 0 ? index : maxLength;
                if (length < minLength)
                    break;
                var name = key.Substring(0, length);
                int i = 0;
                for (; i < name.Length; i++)
                {
                    if (name.Length - i < minLength)
                    {
                        i = index;
                        break;
                    }
                    string word = name.Substring(i, name.Length - i);
                    if (IsMainWord(word))
                    {
                        tokens.Add(new TokenInfo {Name = word});
                        break;
                    }
                }
                index = (i == index ? index-1 : i);
            }
        }

     

        /// <summary>
        /// 是否为词
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual bool IsMainWord(string name)
        {
            if (!MainGroupDictionaries.ContainsKey(name.Length))
                return false;
            var dictionaries = MainGroupDictionaries[name.Length];
            int low = 0, high = dictionaries.Length - 1;
            while (low <= high)
            {
                int mid = low + ((high - low) / 2);
                if (dictionaries[mid].Equals(name))//找到该词
                    return true;
                if (dictionaries[mid].CompareTo(name) > 0)
                    high = mid - 1;
                else
                    low = mid + 1;
            }
            return false;
        }
        #endregion

        #region 拆分

        /// <summary>
        /// 把英文或者数字当作一个整体保留并把前后的中文切开
        /// </summary>
        /// <param name="input"></param>
        protected virtual IList<string> SplitEnglishOrNumber(string input)
        {
            //IList<string> keys = new List<string>();
            var array = input.Split(SplitDictionaries, StringSplitOptions.None);
            //foreach (var arr in array)
            //{
            //    if (string.IsNullOrEmpty(arr)) continue;
            //    for (int i = 0; i < arr.Length; i++)
            //    {
            //        if (arr[i] <= 127 && !IsEnglishOrNumber(arr[i])) continue;
            //        var builder = new StringBuilder();
            //        while (i < arr.Length && IsEnglishOrNumber(arr[i]))
            //        {
            //            builder.Append(arr[i]);
            //            i++;
            //        }
            //        if (builder.Length > 0) keys.Add(builder.ToString());
            //        if (i >= arr.Length || arr[i] <= 127 && !IsEnglishOrNumber(arr[i])) continue;
            //        builder = new StringBuilder();
            //        while (i < arr.Length && arr[i] > 127)
            //        {
            //            builder.Append(arr[i]);
            //            i++;
            //        }
            //        if (builder.Length > 0)
            //        {
            //            keys.Add(builder.ToString());
            //            i--;
            //        }
            //    }
            //}
            return array;
        }

        /// <summary>
        /// 是否是英文或者数字
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected virtual bool IsEnglishOrNumber(char c)
        {
            return c <= 127;
        }
        #endregion

        #region 根据词长度分组
        /// <summary>
        /// 根据词分组
        /// </summary>
        /// <param name="dictionaries"></param>
        /// <returns></returns>
        protected virtual IDictionary<int, string[]> GetGroupDictionaries(string[] dictionaries)
        {
            var tempGroupDictionary = new Dictionary<int, IList<string>>();
            foreach (var key in dictionaries)
            {
                if (!tempGroupDictionary.ContainsKey(key.Length))
                {
                    tempGroupDictionary.Add(key.Length, new List<string>());
                }
                tempGroupDictionary[key.Length].Add(key);
            }
            var groupDictionary = new Dictionary<int, string[]>();
            foreach (var temp in tempGroupDictionary)
            {
                groupDictionary.Add(temp.Key,temp.Value.ToArray());
            }
            return groupDictionary;
        }
        /// <summary>
        /// 根据词分组
        /// </summary>
        /// <param name="dictionaries"></param>
        /// <returns></returns>
        protected virtual IDictionary<int, KeyValuePair<string, string>[]> GetGroupDictionaries(KeyValuePair<string, string>[] dictionaries)
        {
            var tempGroupDictionary = new Dictionary<int, IList<KeyValuePair<string, string>>>();
            foreach (var dic in dictionaries)
            {
                if (!tempGroupDictionary.ContainsKey(dic.Key.Length))
                {
                    tempGroupDictionary.Add(dic.Key.Length, new List<KeyValuePair<string, string>>());
                }
                tempGroupDictionary[dic.Key.Length].Add(new KeyValuePair<string, string>(dic.Key,dic.Value));
            }
            var groupDictionary = new Dictionary<int, KeyValuePair<string, string>[]>();
            foreach (var temp in tempGroupDictionary)
            {
                groupDictionary.Add(temp.Key, temp.Value.ToArray());
            }
            return groupDictionary;
        }
        #endregion
    }
}
