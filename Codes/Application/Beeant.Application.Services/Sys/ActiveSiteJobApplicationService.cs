using System;
using Component.Extension;

namespace Beeant.Application.Services.Sys
{
    public class SiteAliveJobApplicationService : IJobApplicationService
    {
        public static bool IsExecute { get; set; }
        public virtual bool Execute(object[] args)
        {
            if (IsExecute)
                return true;
            IsExecute = true;
            try
            {
                if (args == null)
                    return true;
                foreach (var o in args)
                {
                    WebRequestHelper.SendPostRequest(o.Convert<string>(), null);
                }
            }
            catch (Exception ex) 
            {
                
              
            }
            IsExecute = false;
            return true;
        }


    }
}
