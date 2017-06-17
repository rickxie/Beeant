namespace Beeant.Basic.Services.Mvc.Uploader
{
    public class UploaderModel
    {
        public string Id { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 验证名称
        /// </summary>
        public string FileNameValidateName { get; set; }
        /// <summary>
        /// 验证名称
        /// </summary>
        public string FileByteValidateName { get; set; }
        /// <summary>
        /// 是否隐藏图片
        /// </summary>
        public bool IsHideImage { get; set; }

        public UploaderModel()
        {
            IsMultiple = false;
        }

        /// <summary>
        /// 是否多选
        /// </summary>
        public bool IsMultiple { get; set; }
        /// <summary>
        /// 接受类型
        /// </summary>
        public string Accept { get; set; }
        /// <summary>
        /// 值类型
        /// </summary>
        public string ValueName { get; set; }
    }
}