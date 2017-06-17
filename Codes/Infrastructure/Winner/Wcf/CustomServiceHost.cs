using System;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Configuration;

namespace Winner.Wcf
{
    public class CustomServiceHost : ServiceHost
    {
        #region 静态变量

        static public string ConfigFileName {get;set;}

        #endregion

        #region 构造函数
        public CustomServiceHost(object singleton, params Uri[] baseAddresses)
            : base(singleton, baseAddresses)
        {
        }
        public CustomServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }
        #endregion

        #region 重写方法
        /// <summary>
        /// override ApplyConfiguration to load config from custom file
        /// </summary>
        protected override void ApplyConfiguration()
        {
            //get custom config file name by our rule: config file name = ServiceType.Name
            //var myConfigFileName = this.Description.ServiceType.FullName;
            //get config file path
            var configFileMap = GetExeConfigurationFileMap();
            if (configFileMap == null)
                return;
            var serviceModel = GetServiceModelSectionGroup(configFileMap);
            if (serviceModel == null)
                return;
            SetServiceElement(serviceModel);
        }
        /// <summary>
        /// 得到ExeConfigurationFileMap
        /// </summary>
        /// <returns></returns>
        protected virtual ExeConfigurationFileMap GetExeConfigurationFileMap()
        {
            string dir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string myConfigFilePath = System.IO.Path.Combine(dir, ConfigFileName);
            if (!System.IO.File.Exists(myConfigFilePath))
            {
                base.ApplyConfiguration();
                return null;
            }
            var configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = myConfigFilePath;
            return configFileMap;
        }
        /// <summary>
        /// 得到ServiceModelSectionGroup
        /// </summary>
        /// <param name="configFileMap"></param>
        /// <returns></returns>
        protected virtual ServiceModelSectionGroup GetServiceModelSectionGroup(ExeConfigurationFileMap configFileMap)
        {
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            ServiceModelSectionGroup serviceModel = ServiceModelSectionGroup.GetSectionGroup(config);
            if (serviceModel == null)
            {
                base.ApplyConfiguration();
            }
            return serviceModel;
        }
        /// <summary>
        /// 设置ServiceElement
        /// </summary>
        /// <param name="serviceModel"></param>
        /// <returns></returns>
        protected virtual bool SetServiceElement(ServiceModelSectionGroup serviceModel)
        {
            foreach (ServiceElement serviceElement in serviceModel.Services.Services)
            {
                if (serviceElement.Name == Description.ServiceType.FullName)
                {
                    LoadConfigurationSection(serviceElement);
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
