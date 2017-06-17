using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration;

namespace Beeant.Test.Services
{
    public class TestBase
    {
        static TestBase()
        {
            ConfigurationManager.Initialize(@"Beeant.Test.Services");
        }
    }
}
