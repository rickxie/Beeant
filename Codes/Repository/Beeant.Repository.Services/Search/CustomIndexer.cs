using System;
using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Winner.Search;

namespace Beeant.Repository.Services.Search
{
    public class CustomIndexer : XmlIndexer
    {
        readonly string[] _keys = new[] { "StartCost", "EndCost", "StartPrice", "EndPrice", "IsCustom", "Seqence", "Sku" };
        /// <summary>
        /// 得到搜索编号
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="result"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        protected override IList<long> GetSearchDocumentIds(StoreIndexInfo storeIndex, SearchResultInfo result, SearchQueryInfo searchQuery)
        {
            var documentIds= base.GetSearchDocumentIds(storeIndex, result, searchQuery);
            
            if (documentIds != null && searchQuery.Conditions != null &&
               searchQuery.Conditions.Count(it => _keys.Contains(it.Key)) > 0)
            {
                FilterDocuments(documentIds, storeIndex, result, searchQuery);
                OrderbyDocuments(documentIds, storeIndex, result, searchQuery);
                if (result.Documents != null)
                    return result.Documents.Select(it => it.Id).ToList();
            }
            return documentIds;
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="documentIds"></param>
        /// <param name="storeIndex"></param>
        /// <param name="result"></param>
        /// <param name="searchQuery"></param>
        protected virtual void OrderbyDocuments(IList<long> documentIds, StoreIndexInfo storeIndex,
                                                SearchResultInfo result, SearchQueryInfo searchQuery)
        {
            if (searchQuery.Conditions.ContainsKey("Seqence"))
            {
                switch (searchQuery.Conditions["Seqence"].ToString())
                {
                    case "costasc":
                        if (searchQuery.PageSize > 0)
                        {
                            result.Documents =
                                result.Documents.OrderBy(it => it.Feilds[4].Text.Convert<decimal>())
                                      .Skip(searchQuery.PageIndex * searchQuery.PageSize)
                                      .Take(searchQuery.PageSize)
                                      .ToList();
                        }
                        else
                        {
                            result.Documents = result.Documents.OrderBy(it => it.Feilds[4].Text.Convert<decimal>()).ToList();
                        }
                        break;
                    case "costdesc":
                        if (searchQuery.PageSize > 0)
                        {
                            result.Documents =
                                result.Documents.OrderByDescending(it => it.Feilds[4].Text.Convert<decimal>())
                                      .Skip(searchQuery.PageIndex * searchQuery.PageSize)
                                      .Take(searchQuery.PageSize)
                                      .ToList();
                        }
                        else
                        {
                            result.Documents = result.Documents.OrderByDescending(it => it.Feilds[4].Text.Convert<decimal>()).ToList();
                        }
                        break;
                    case "prasc":
                        if (searchQuery.PageSize > 0)
                        {
                            result.Documents =
                                result.Documents.OrderBy(it => it.Feilds[5].Text.Convert<decimal>())
                                      .Skip(searchQuery.PageIndex * searchQuery.PageSize)
                                      .Take(searchQuery.PageSize)
                                      .ToList();
                        }
                        else
                        {
                            result.Documents = result.Documents.OrderBy(it => it.Feilds[5].Text.Convert<decimal>()).ToList();
                        }
                        break;
                    case "prdesc":
                        if (searchQuery.PageSize > 0)
                        {
                            result.Documents =
                                result.Documents.OrderByDescending(it => it.Feilds[5].Text.Convert<decimal>())
                                      .Skip(searchQuery.PageIndex * searchQuery.PageSize)
                                      .Take(searchQuery.PageSize)
                                      .ToList();
                        }
                        else
                        {
                            result.Documents = result.Documents.OrderByDescending(it => it.Feilds[5].Text.Convert<decimal>()).ToList();
                        }
                        break;
                    case "ptdesc":
                        if (searchQuery.PageSize > 0)
                        {
                            result.Documents =
                                result.Documents.OrderByDescending(it => it.Feilds[7].Text.Convert<DateTime>())
                                      .Skip(searchQuery.PageIndex * searchQuery.PageSize)
                                      .Take(searchQuery.PageSize)
                                      .ToList();
                        }
                        else
                        {
                            result.Documents = result.Documents.OrderByDescending(it => it.Feilds[7].Text.Convert<DateTime>()).ToList();
                        }
                        break;
                    case "scasc":
                        if (searchQuery.PageSize > 0)
                        {
                            result.Documents =
                                result.Documents.OrderByDescending(it => it.Feilds[8].Text.Convert<int>())
                                      .Skip(searchQuery.PageIndex * searchQuery.PageSize)
                                      .Take(searchQuery.PageSize)
                                      .ToList();
                        }
                        else
                        {
                            result.Documents = result.Documents.OrderByDescending(it => it.Feilds[8].Text.Convert<int>()).ToList();
                        }
                        break;
                }
            }
            else if (searchQuery.PageSize > 0)
                result.Documents =
                    result.Documents.Skip(searchQuery.PageIndex * searchQuery.PageSize)
                          .Take(searchQuery.PageSize)
                          .ToList();
        }
        /// <summary>
        /// 过滤条件
        /// </summary>
        /// <param name="documentIds"></param>
        /// <param name="storeIndex"></param>
        /// <param name="result"></param>
        /// <param name="searchQuery"></param>
        protected virtual void FilterDocuments(IList<long>  documentIds,StoreIndexInfo storeIndex, SearchResultInfo result, SearchQueryInfo searchQuery)
        {
            foreach (var documentId in documentIds)
            {
                var document = Documentor.GetInfo(storeIndex, documentId);
                if (document == null || document.Feilds == null) continue;
                if (searchQuery.Conditions.ContainsKey("IsCustom") && document.Feilds.Count > 6 &&
                    !document.Feilds[6].Text.Convert<bool>() != searchQuery.Conditions["IsCustom"].Convert<bool>())
                {
                    continue;
                }
                if (searchQuery.Conditions.ContainsKey("StartCost") && document.Feilds.Count > 4 &&
                    document.Feilds[4].Text.Convert<decimal>() <
                    searchQuery.Conditions["StartCost"].Convert<decimal>())
                {
                    continue;
                }
                if (searchQuery.Conditions.ContainsKey("EndCost") && document.Feilds.Count > 4 &&
                    document.Feilds[4].Text.Convert<decimal>() >
                    searchQuery.Conditions["EndCost"].Convert<decimal>())
                {
                    continue;
                }
                if (searchQuery.Conditions.ContainsKey("StartPrice") && document.Feilds.Count > 5 &&
                    document.Feilds[5].Text.Convert<decimal>() <
                    searchQuery.Conditions["StartPrice"].Convert<decimal>())
                {
                    continue;
                }
                if (searchQuery.Conditions.ContainsKey("EndPrice") && document.Feilds.Count > 5 &&
                    document.Feilds[5].Text.Convert<decimal>() >
                    searchQuery.Conditions["EndPrice"].Convert<decimal>())
                {
                    continue;
                }
                if (searchQuery.Conditions.ContainsKey("Sku") && document.Feilds.Count > 3 &&
                    !string.IsNullOrEmpty(document.Feilds[3].Text))
                {
                    var texts = searchQuery.Conditions["Sku"].ToString().Split(',');
                    var values = document.Feilds[3].Text.Split(',');
                    var rev = texts.All(text => CheckValue(values, text));
                    if (rev)
                    {
                        continue;
                    }
                }
                result.Documents.Add(document);
            }
        }
        /// <summary>
        /// 重写
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="searchQuery"></param>
        /// <param name="result"></param>
        /// <param name="documentIds"></param>
        protected override void AddSearchDocuments(StoreIndexInfo storeIndex, SearchQueryInfo searchQuery, SearchResultInfo result, IList<long> documentIds)
        {
            if (searchQuery.Conditions != null && searchQuery.Conditions.Count(it => _keys.Contains(it.Key)) > 0)
                return;
            base.AddSearchDocuments(storeIndex,searchQuery,result,documentIds);
        }
      

        /// <summary>
        /// 根据词得到索引下标
        /// </summary>
        /// <param name="values"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual bool CheckValue(string[] values, string value)
        {
            if (values == null || values.Length == 0) 
                return false;
            int low = 0, high = values.Length - 1;
            while (low <= high)
            {
                int mid = low + ((high - low) / 2);
                if (values[mid].Equals(value))
                    return true;
                if (values[mid].CompareTo(value) > 0)
                    high = mid - 1;
                else
                    low = mid + 1;
            }
            return false;
        }
    }
}
