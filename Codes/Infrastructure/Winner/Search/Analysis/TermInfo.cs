using System;

namespace Winner.Search.Analysis
{
    /// <summary>
    /// 词元
    /// </summary>
    [Serializable]
    public class TermInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 词源
        /// </summary>
        public TokenInfo Token { get; set; }
    }
}
