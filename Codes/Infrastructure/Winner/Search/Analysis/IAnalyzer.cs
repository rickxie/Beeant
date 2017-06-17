using System.Collections.Generic;

namespace Winner.Search.Analysis
{

    public interface IAnalyzer
    {
        /// <summary>
        /// 分词信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        IList<TermInfo> Resolve(string input);


    }
}
