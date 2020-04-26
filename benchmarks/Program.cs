using System.IO;
using System.Text;
using static SimpleExec.Command;

namespace benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = new DirectoryInfo(Path.Join(Path.GetDirectoryName(typeof(Program).Assembly.Location), "..", "..", "..")).FullName;
            var dir = Path.Join(root, ".bdn", "results");
            Directory.CreateDirectory(dir);

            var currentBranch = Read("git.exe", "branch --show-current");
            var lastCommitMsg = Read("git.exe", "log -1 --pretty=%B");

            var sb = new StringBuilder();
            sb.AppendLine("## Test Report");
            var filter = args.Length == 2 && args[0] == "--filter" ? args[1] : "";
            sb.AppendLine($"Filter: {filter}");
            sb.AppendLine($"Branch: {currentBranch}");
            sb.AppendLine($"Last commit message: {lastCommitMsg}");


            File.WriteAllText(Path.Join(dir, "test-report.md"), sb.ToString());
        }
    }
}
