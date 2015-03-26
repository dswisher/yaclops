using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using Yaclops.Parsing;

namespace Yaclops.Tests.Parsing
{
    [TestFixture]
    public class ParserConfigurationTests
    {
        private ParserConfiguration _config;


        [SetUp]
        public void BeforeEachTest()
        {
            _config = new ParserConfiguration();
        }



        [Test]
        public void SettingDefaultCommandToUnknownCommandThrows()
        {
            _config.AddCommand("foo");
            Should.Throw<ParserConfigurationException>(() => _config.DefaultCommand = "bar");
        }


        [Test]
        public void AddingDuplicateCommandThrows()
        {
            _config.AddCommand("foo");
            Should.Throw<ParserConfigurationException>(() => _config.AddCommand("foo"));
        }
    }
}
