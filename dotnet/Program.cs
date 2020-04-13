using System;
using System.IO;
using System.Linq;
using UnitTest;
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
        public async void JapaneseFilenameCheck()
        {
            // 日本語名のファイルが正しく取得できていることを確認する
            var path = await UnitTestUtils.GetVersionedFiles();
            Assert.True(path.Where(v=>v.EndsWith("日本語のファイル.md")).Count() == 1);
            Assert.True(path.Where(v=>v.EndsWith("日本語のファイルだみー.md")).Count() == 0);
        }
    }
}
