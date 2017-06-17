using System;
using System.Collections.Generic;
using System.Text;


namespace Winner.Search.Analysis
{
    public class BinaryAnalyzer : StandardAnalyzer
    {

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override IList<TermInfo> Resolve(string input)
        {
            var terms=new List<TermInfo>();
            if (string.IsNullOrEmpty(input))
                return null;
            var splits = SplitEnglishOrNumber(input);
            foreach (var split in splits)
            {
                Binary(terms, split);
            }
            return terms;
        }

        #region 拆分
  
        /// <summary>
        /// 二分
        /// </summary>
        /// <param name="words"></param>
        /// <param name="input"></param>
        protected virtual void Binary(IList<TermInfo> words, string input)
        {
            input = input.Trim();
            if(string.IsNullOrWhiteSpace(input))
                return;
            if(IsEnglishOrNumber(input[0]) || input.Length < 2)
                words.Add(new TermInfo {Name=input,Token = new TokenInfo {Name=input} });
            for (int i = 0; i < input.Length-1; i++)
            {
                var key = input.Substring(i, 2);
                words.Add(new TermInfo { Name = key, Token = new TokenInfo { Name = key } });
            }
        }

        /// <summary>
        /// 把英文或者数字当作一个整体保留并把前后的中文切开
        /// </summary>
        /// <param name="input"></param>
        protected override IList<string> SplitEnglishOrNumber(string input)
        {
            IList<string> keys = new List<string>();
            var array = input.Split(SplitDictionaries, StringSplitOptions.None);
            foreach (var arr in array)
            {
                if (string.IsNullOrEmpty(arr)) continue;
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] <= 127 && !IsEnglishOrNumber(arr[i])) continue;
                    var builder = new StringBuilder();
                    while (i < arr.Length && IsEnglishOrNumber(arr[i]))
                    {
                        builder.Append(arr[i]);
                        i++;
                    }
                    if (builder.Length > 0) keys.Add(builder.ToString());
                    if (i >= arr.Length || arr[i] <= 127 && !IsEnglishOrNumber(arr[i])) continue;
                    builder = new StringBuilder();
                    while (i < arr.Length && arr[i] > 127)
                    {
                        builder.Append(arr[i]);
                        i++;
                    }
                    if (builder.Length > 0)
                    {
                        keys.Add(builder.ToString());
                        i--;
                    }
                }
            }
            return array;
        }

        
        #endregion

    }
}
