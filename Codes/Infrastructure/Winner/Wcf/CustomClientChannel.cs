using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Configuration;
using System.ServiceModel.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;

namespace Winner.Wcf
{

	/// <summary>
	/// Custom client channel. Allows to specify a different configuration file
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class CustomClientChannel<T> : ChannelFactory<T>
    {
        #region 属性
        /// <summary>
        /// 路径
        /// </summary>
        protected string ConfigurationPath { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        protected string EndPointConfigurationName{ get; set; }


        #endregion

        #region 构造函数

	    /// <summary>
	    /// Constructor
	    /// </summary>
	    public CustomClientChannel(string configurationPath) : base(typeof(T))
		{
            ConfigurationPath = configurationPath;
			InitializeEndpoint((string)null, null);
		}

	    /// <summary>
	    /// Constructor
	    /// </summary>
	    /// <param name="binding"></param>
	    /// <param name="configurationPath"></param>
	    public CustomClientChannel(Binding binding, string configurationPath)
            : this(binding, (EndpointAddress)null, configurationPath)

		{
		}

	    /// <summary>
	    /// Constructor
	    /// </summary>
	    /// <param name="serviceEndpoint"></param>
	    /// <param name="configurationPath"></param>
	    public CustomClientChannel(ServiceEndpoint serviceEndpoint, string configurationPath)
			: base(typeof(T))
		{
            ConfigurationPath = configurationPath;
			InitializeEndpoint(serviceEndpoint);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="endPointName"></param>
		/// <param name="configurationPath"></param>
        public CustomClientChannel(string endPointName, string configurationPath)
            : this(endPointName, null, configurationPath)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="binding"></param>
		/// <param name="endpointAddress"></param>
		/// <param name="configurationPath"></param>
        public CustomClientChannel(Binding binding, EndpointAddress endpointAddress, string configurationPath)
			: base(typeof(T))
		{
            ConfigurationPath = configurationPath;
			InitializeEndpoint(binding, endpointAddress);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="binding"></param>
		/// <param name="remoteAddress"></param>
		/// <param name="configurationPath"></param>
        public CustomClientChannel(Binding binding, string remoteAddress, string configurationPath)
            : this(binding, new EndpointAddress(remoteAddress), configurationPath)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="endPointName"></param>
		/// <param name="endpointAddress"></param>
		/// <param name="configurationPath"></param>
        public CustomClientChannel(string endPointName, EndpointAddress endpointAddress, string configurationPath)
			: base(typeof(T))
		{
            ConfigurationPath = configurationPath;
            EndPointConfigurationName = endPointName;
            InitializeEndpoint(endPointName, endpointAddress);
		}

        #endregion

        #region 重写方法

        /// <summary>
		/// Loads the serviceEndpoint description from the specified configuration file
		/// </summary>
		/// <returns></returns>
        protected override ServiceEndpoint CreateDescription()
        {
            ServiceEndpoint serviceEndpoint = base.CreateDescription();
            ExeConfigurationFileMap map = GetExeConfigurationFileMap(serviceEndpoint);
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            ServiceModelSectionGroup group = ServiceModelSectionGroup.GetSectionGroup(config);
            ChannelEndpointElement selectedEndpoint = GetSelectedEndpoint(group, serviceEndpoint);
            SetSelectedEndpoint(selectedEndpoint, group, serviceEndpoint);
            return serviceEndpoint;

        }
        /// <summary>
        /// 设置config信息
        /// </summary>
        /// <param name="serviceEndpoint"></param>
        protected virtual ExeConfigurationFileMap GetExeConfigurationFileMap(ServiceEndpoint serviceEndpoint)
        {
            if (EndPointConfigurationName != null)
                serviceEndpoint.Name = EndPointConfigurationName;
            string dir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string myConfigFilePath = System.IO.Path.Combine(dir, ConfigurationPath);
            var map = new ExeConfigurationFileMap {ExeConfigFilename = myConfigFilePath};
            return map;
        }
        /// <summary>
        /// 得到选择Endpoint
        /// </summary>
        /// <param name="group"></param>
        /// <param name="serviceEndpoint"></param>
        /// <returns></returns>
        protected virtual ChannelEndpointElement GetSelectedEndpoint(ServiceModelSectionGroup group, ServiceEndpoint serviceEndpoint)
        {
            return @group.Client.Endpoints.Cast<ChannelEndpointElement>().FirstOrDefault(endpoint => endpoint.Contract == serviceEndpoint.Contract.ConfigurationName && (EndPointConfigurationName == null || EndPointConfigurationName == endpoint.Name));
        }

	    /// <summary>
        /// 设置Endpoint
        /// </summary>
        /// <param name="selectedEndpoint"></param>
        /// <param name="group"></param>
        /// <param name="serviceEndpoint"></param>
        protected virtual void SetSelectedEndpoint(ChannelEndpointElement selectedEndpoint, ServiceModelSectionGroup group, ServiceEndpoint serviceEndpoint)
        {
	        if (selectedEndpoint == null) return;
	        if (serviceEndpoint.Binding == null) 
	            serviceEndpoint.Binding = CreateBinding(selectedEndpoint.Binding, @group);
	        if (serviceEndpoint.Address == null) 
	            serviceEndpoint.Address = new EndpointAddress(selectedEndpoint.Address, GetIdentity(selectedEndpoint.Identity), selectedEndpoint.Headers.Headers);
	        if (serviceEndpoint.Behaviors.Count == 0 && !string.IsNullOrEmpty(selectedEndpoint.BehaviorConfiguration))
	            AddBehaviors(selectedEndpoint.BehaviorConfiguration, serviceEndpoint, @group);
	        serviceEndpoint.Name = selectedEndpoint.Contract;
        }
        /// <summary>
		/// Configures the binding for the selected endpoint
		/// </summary>
		/// <param name="bindingName"></param>
		/// <param name="group"></param>
		/// <returns></returns>
        protected virtual Binding CreateBinding(string bindingName, ServiceModelSectionGroup group)
        {
            BindingCollectionElement bindingElementCollection = group.Bindings[bindingName];
            if (bindingElementCollection.ConfiguredBindings.Count > 0)
            {
                IBindingConfigurationElement be = bindingElementCollection.ConfiguredBindings[0];
                Binding binding = GetBinding(be);
                if (be != null)
                    be.ApplyConfiguration(binding);
                return binding;
            }
            return null;
        }

		/// <summary>
		/// Helper method to create the right binding depending on the configuration element
		/// </summary>
		/// <param name="configurationElement"></param>
		/// <returns></returns>
        protected  virtual Binding GetBinding(IBindingConfigurationElement configurationElement)
        {
            IDictionary<Type, Binding> container = new Dictionary<Type, Binding>
            {
                {typeof(CustomBindingElement),new CustomBinding() },  {typeof(BasicHttpBindingElement),new BasicHttpBinding() },
                {typeof(NetMsmqBindingElement),new NetMsmqBinding() },  {typeof(NetNamedPipeBindingElement),new NetNamedPipeBinding() },
                {typeof(NetPeerTcpBindingElement),new NetPeerTcpBinding() },  {typeof(NetTcpBindingElement),new NetTcpBinding() },
                {typeof(WSDualHttpBindingElement),new WSDualHttpBinding() },  {typeof(WSHttpBindingElement),new WSHttpBinding() },
                {typeof(WSFederationHttpBindingElement),new WSFederationHttpBinding() }
            };
            return container.ContainsKey(configurationElement.GetType()) ? container[configurationElement.GetType()] : null;
        }

		/// <summary>
		/// Adds the configured behavior to the selected endpoint
		/// </summary>
		/// <param name="behaviorConfiguration"></param>
		/// <param name="serviceEndpoint"></param>
		/// <param name="group"></param>
		private void AddBehaviors(string behaviorConfiguration, ServiceEndpoint serviceEndpoint, ServiceModelSectionGroup group)
		{
		    EndpointBehaviorElement behaviorElement = group.Behaviors.EndpointBehaviors[behaviorConfiguration];
		    foreach (BehaviorExtensionElement behaviorExtension in behaviorElement)
		    {
		        object extension = behaviorExtension.GetType().InvokeMember("CreateBehavior",
                                                                            BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance,
		                                                                    null, behaviorExtension, null);
		        if (extension != null)
		            serviceEndpoint.Behaviors.Add((IEndpointBehavior)extension);
		    }
		}

	    /// <summary>
		/// Gets the endpoint identity from the configuration file
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
        private EndpointIdentity GetIdentity(IdentityElement element)
        {
            PropertyInformationCollection properties = element.ElementInformation.Properties;
            EndpointIdentity rev = GetEndpointIdentity(element, properties) ??
                                   GetEndpointIdentityByCertificate(element, properties);
		    return rev;
        }
        /// <summary>
        /// 得到EndpointIdentity
        /// </summary>
        /// <param name="element"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected virtual EndpointIdentity GetEndpointIdentity(IdentityElement element, PropertyInformationCollection properties)
        {
            if (properties["userPrincipalName"]!=null && properties["userPrincipalName"].ValueOrigin != PropertyValueOrigin.Default)
                return EndpointIdentity.CreateUpnIdentity(element.UserPrincipalName.Value);
            if (properties["servicePrincipalName"] != null && properties["servicePrincipalName"].ValueOrigin != PropertyValueOrigin.Default)
                return EndpointIdentity.CreateSpnIdentity(element.ServicePrincipalName.Value);
            if (properties["dns"] != null && properties["dns"].ValueOrigin != PropertyValueOrigin.Default)
                return EndpointIdentity.CreateDnsIdentity(element.Dns.Value);
            if (properties["rsa"] != null && properties["rsa"].ValueOrigin != PropertyValueOrigin.Default)
                return EndpointIdentity.CreateRsaIdentity(element.Rsa.Value);
            return null;
        }
        /// <summary>
        /// 得到EndpointIdentity
        /// </summary>
        /// <param name="element"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected virtual EndpointIdentity GetEndpointIdentityByCertificate(IdentityElement element, PropertyInformationCollection properties)
        {
            if (properties["certificate"]!=null && properties["certificate"].ValueOrigin != PropertyValueOrigin.Default)
                return null;
            var supportingCertificates = new X509Certificate2Collection();
            supportingCertificates.Import(Convert.FromBase64String(element.Certificate.EncodedValue));
            if (supportingCertificates.Count == 0) 
                return null;
            var primaryCertificate = supportingCertificates[0];
            supportingCertificates.RemoveAt(0);
            return EndpointIdentity.CreateX509CertificateIdentity(primaryCertificate, supportingCertificates);
        }

        protected override void ApplyConfiguration(string configurationName)
        {
     
        }
        #endregion

 
    }

	

	
}
