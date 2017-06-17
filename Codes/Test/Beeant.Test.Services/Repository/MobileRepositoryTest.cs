using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services.Utility;
using Beeant.Repository.Services.Utility;
using Component.Extension;
using Dependent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Winner;

namespace Beeant.Test.Services.Repository
{
    [TestClass]
    public class MobileRepositoryTest : TestBase
    {
        [TestMethod]
        public void AliDayuTest()
        {
            var dy = Ioc.Resolve<IMobileRepository>();
            var result = dy.Send(new MobileEntity()
            {
                Body = "123456",
                ToMobiles = new[] {"18806871573"}
            });
            var rt = result.DeserializeJson<dynamic>();
            var success = rt?.IsError != null && (bool) rt?.IsError;
            Assert.IsFalse(success);
        }
    }
}
