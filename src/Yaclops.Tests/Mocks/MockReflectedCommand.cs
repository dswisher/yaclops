using System;
using System.Collections.Generic;
using Yaclops.Reflecting;

namespace Yaclops.Tests.Mocks
{
    internal class MockReflectedCommand : IReflectedCommand<object>
    {
        private static readonly Func<object> _factory = () => new object();

        private readonly List<string> _verbs = new List<string>();
        private readonly List<ReflectedNamedParameter> _namedParameters = new List<ReflectedNamedParameter>();


        public IReadOnlyList<string> Verbs { get { return _verbs; } }
        public IReadOnlyList<ReflectedNamedParameter> NamedParameters { get { return _namedParameters; } }
        public Func<object> Factory { get { return _factory; } }


        public void AddVerbs(params string[] verbs)
        {
            _verbs.AddRange(verbs);
        }


        public static MockReflectedCommand FromVerbs(params string[] verbs)
        {
            var mock = new MockReflectedCommand();
            mock.AddVerbs(verbs);
            return mock;
        }
    }
}
