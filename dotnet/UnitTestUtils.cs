using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text;

namespace dotnet
{
    public class UnitTestUtils
    {

        private readonly ITestOutputHelper output;
        public UnitTestUtils(ITestOutputHelper helper)
        {
            output = helper;
        }

        public static string GetRepositoryRoot()
        {
            string path = Path.GetDirectoryName(Assembly.GetCallingAssembly()!.Location)!;
            while (!Directory.Exists($@"{path}/.github"))
            {
                path = Directory.GetParent(path).FullName;
            }
            return path;
        }

        // バージョン管理されたファイルの一覧をフルパスで返す
        public static async Task<string[]> GetVersionedFiles()
        {
            // git ls-files コマンドでファイルの一覧を取得する
            var root = UnitTestUtils.GetRepositoryRoot();
            var psi = new ProcessStartInfo("cmd.exe", "/c git ls-files -z");
            psi.StandardOutputEncoding = Encoding.UTF8;
            psi.WorkingDirectory = root;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            var p = Process.Start(psi);
            string output = await p.StandardOutput.ReadToEndAsync();

            // 一覧をフルパスの配列にして返す
            var files = output.Split('\0');
            return files.Select(v => $"{root}/{v}").ToArray();
        }

        [Fact]
        public void GetRepositoryRootChecker()
        {
            var path = GetRepositoryRoot();
            output.WriteLine(@$"path = {path}");
            Assert.True(File.Exists(@$"{path}/README.md"));
            Assert.True(File.Exists(@$"{path}/.gitignore"));
        }

        [Fact]
        public async void GetVersionedFilesChecker()
        {
            var paths = await GetVersionedFiles();
            
            output.WriteLine(String.Join("\n", paths));

            Assert.True(1 <= paths.Where(text => text.EndsWith("README.md")).Count(), "README.md not found.");
            Assert.True(0 == paths.Where(text => text.EndsWith("AssemblyInfo.cs")).Count(), "AssemblyInfo.cs detected.");
        }
    }
}
