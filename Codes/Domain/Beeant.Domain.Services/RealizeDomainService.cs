using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services.Utility;
using Winner.Filter;
using Winner.Persistence;

namespace Beeant.Domain.Services
{
    public interface IUnitofworkHandle
    {
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="unitofworks"></param>
        /// <param name="info"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Handle(IList<IUnitofwork> unitofworks, BaseEntity info, object value);
    }
    public class UnitofworkHandle<TEntityType> : IUnitofworkHandle where TEntityType : BaseEntity
    {
        /// <summary>
        /// 业务实例
        /// </summary>
        public virtual IDomainService DomainService { get; set; }
        /// <summary>
        /// 存储实例
        /// </summary>
        public virtual IRepository Repository { get; set; }
        /// <summary>
        /// 是否添加
        /// </summary>
        public bool IsAppend { get; set; } = true;
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="unitofworks"></param>
        /// <param name="info"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Handle(IList<IUnitofwork> unitofworks, BaseEntity info, object value)
        {
            if (value is IEnumerable<TEntityType>)
            {
                var ies = value as IEnumerable<TEntityType>;
                IList<TEntityType> items = ies.ToList();
                if (items.Count == 0) return true;
                var units = Handle(items);
                if (units == null)
                {
                    info.Errors = info.Errors ?? new List<ErrorInfo>();
                    foreach (var item in items)
                    {
                        info.Errors.AddList(item.Errors);
                    }
                    return false;
                }
                Append(unitofworks, units);
            }
            else
            {
                var item = value as TEntityType;
                if (item == null) return true;
                var units = Handle(item);
                if (units == null)
                {
                    info.Errors = info.Errors ?? new List<ErrorInfo>();
                    info.Errors.AddList(item.Errors);
                    return false;
                }
                Append(unitofworks, units);
            }
            return true;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="unitofworks"></param>
        /// <param name="newUnitofworks"></param>
        protected virtual void Append(IList<IUnitofwork> unitofworks, IList<IUnitofwork> newUnitofworks)
        {
            if (IsAppend)
            {
                unitofworks.AddList(newUnitofworks);
                return;
            }
            for (int i = newUnitofworks.Count-1; i>=0; i--)
            {
                unitofworks.Insert(0, newUnitofworks[i]);
            }
        }
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected virtual IList<IUnitofwork> Handle(IList<TEntityType> infos)
        {
            return DomainService==null?Repository.Save(infos): DomainService.Handle(infos);
        }
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual IList<IUnitofwork> Handle(TEntityType info)
        {
            return DomainService == null ? Repository.Save(info) : DomainService.Handle(info);
        }
    }

    public class RealizeDomainService<TEntityType> : DomainService where TEntityType : BaseEntity
    {
        /// <summary>
        /// 存储实例
        /// </summary>
        public virtual IFileRepository FileRepository { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public virtual IList<FileEntity> FileProperties { get; set; }

        #region 重写
        /// <summary>
        /// 重写添加上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public override IList<IUnitofwork> Handle<T>(T info)
        {
            if (info.HandleResult.HasValue)
                return new IUnitofwork[0];
            return Handle(info as TEntityType);
        }
   
 
        /// <summary>
        /// 重写验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool Validate<T>(T info)
        {
            return Validate(info as TEntityType);
        }
        /// <summary>
        /// 设置委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        public override void SetItemLoaders<T>(T info)
        {
            info.SetItemLoaders(ItemLoaders);
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public virtual bool Validate(IList<TEntityType> infos)
        {
           return ValidateBatch(infos) && infos.Aggregate(true, (current, info) => Validate(info) & current);
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual bool Validate(TEntityType info)
        {
            var rev = base.Validate(info);
            if (rev)
                rev = ValidateBusiness(info);
            return rev;
        }
    
        #endregion

        #region 验证
        /// <summary>
        /// 验证集合
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected virtual bool ValidateBatch(IList<TEntityType> infos)
        {
            return true;
        }

        /// <summary>
        /// 验证业务
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateBusiness(TEntityType info)
        {
            switch (info.SaveType)
            {
                case SaveType.Add:
                    return ValidateAdd(info);
                case SaveType.Modify:
                    return ValidateModify(info);
                case SaveType.Remove:
                    return ValidateRemove(info);
            }
            return true;
        }
        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAdd(TEntityType info)
        {
            return true;
        }
        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateModify(TEntityType info)
        {
            return true;
        }
        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateRemove(TEntityType info)
        {
            return true;
        }
        #endregion

        #region 处理业务
    
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual IList<IUnitofwork> Handle(TEntityType info)
        {
            SetFileProperty(info);
            var unitofworks = base.Handle(info);
            if (unitofworks == null) return null;
            if (!SetBusiness(unitofworks, info))
                return null;
            AddFileUnitofworks(unitofworks, info);
            return unitofworks;
        }
        /// <summary>
        /// 处理
        /// </summary>
        protected virtual IDictionary<string, ItemLoader<TEntityType>> ItemLoaders { get; set; }
     

        /// <summary>
        /// 加载对象
        /// </summary>
        protected virtual IDictionary<string, IUnitofworkHandle> ItemHandles { get; set; }
        /// <summary>
        /// 设置业务
        /// </summary>
        /// <param name="unitofworks"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool SetBusiness(IList<IUnitofwork> unitofworks, TEntityType info)
        {
            info.SetBusiness(ItemLoaders);
            var rev= InvokeItemHandles(unitofworks, ItemHandles, info);
            return rev;
        }

        /// <summary>
        /// 调用相关业务
        /// </summary>
        /// <param name="unitofworks"></param>
        /// <param name="itemHandles"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool InvokeItemHandles(IList<IUnitofwork> unitofworks,IDictionary<string, IUnitofworkHandle> itemHandles, TEntityType info)
        {
            if (itemHandles == null || itemHandles.Count == 0)
                return true;
            foreach (var handle in itemHandles)
            {
                var value = Winner.Creator.Get<Winner.Base.IProperty>().GetValue<object>(info, handle.Key);
                if (value == null) continue;
                if (!handle.Value.Handle(unitofworks, info, value))
                    return false;
            }
            return true;
        }

     

        #region 文件

        /// <summary>
        /// 设置文件名
        /// </summary>
        /// <param name="info"></param>
        protected virtual void SetFileProperty(TEntityType info)
        {
            if (FileProperties == null || FileProperties.Count == 0 || FileRepository == null)
                return;
            var rev = FileProperties.Aggregate(false,
                                               (current, fileProperty) =>
                                               current || info.HasSaveProperty(fileProperty.FilePropertyName));
            if (!rev)
                return;
            var server = Winner.Creator.Get<Winner.Base.IProperty>();
            var fileroute = Winner.Creator.Get<Winner.Storage.IFile>();
            foreach (var fileProperty in FileProperties)
            {
                if (!info.HasSaveProperty(fileProperty.FilePropertyName))
                    continue;
                var fileName = server.GetValue<string>(info, fileProperty.FilePropertyName);
                if (string.IsNullOrEmpty(fileName))
                    continue;
                var fileByte = server.GetValue<byte[]>(info, fileProperty.BytePropertyName);
                if (fileByte == null || fileByte.Length == 0)
                    continue;
                string setValue;
                if (info.SaveType == SaveType.Add)
                {
                    setValue = fileroute.CreateFileName(fileName);

                }
                else
                {
                    var dataEntity = Repository.Get<TEntityType>(info.Id);
                    setValue = server.GetValue<string>(dataEntity, fileProperty.FilePropertyName);
                    if (dataEntity == null || string.IsNullOrEmpty(setValue))
                    {
                        setValue = fileroute.CreateFileName(fileName);
                    }
                    else
                    {
                        var exetion = System.IO.Path.GetExtension(setValue);
                        if (!string.IsNullOrEmpty(exetion))
                        {
                            setValue = setValue.Replace(exetion, System.IO.Path.GetExtension(fileName));
                        }
                    }
                }
                setValue = setValue ?? "";
                server.SetValue(info, fileProperty.FilePropertyName, setValue);
            }
        }

        /// <summary>
        /// 得到文件接口
        /// </summary>
        /// <param name="unitofworks"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual void AddFileUnitofworks(IList<IUnitofwork> unitofworks, TEntityType info)
        {
            if (FileProperties == null || FileProperties.Count == 0 || FileRepository == null)
                return;
            if (unitofworks != null)
            {
                var fileUnitofworks = FileRepository.GetSaveUnitofwork(info, FileProperties);
                if (fileUnitofworks != null) unitofworks.Insert(0, fileUnitofworks);
            }
        }
        #endregion
        #endregion
    }
}
