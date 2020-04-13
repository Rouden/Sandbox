using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace dotnet
{
    public class Program
    {
        private readonly ITestOutputHelper output;
        public Program(ITestOutputHelper helper)
        {
            output = helper;
        }

        [Fact]
        public void GetRepositoryRootChecker()
        {
            var path = UnitTestUtils.GetRepositoryRoot();
            output.WriteLine(@$"path = {path}");
            Assert.True(File.Exists(@$"{path}/README.md"));
            Assert.True(File.Exists(@$"{path}/.gitignore"));
        }
    }
}
