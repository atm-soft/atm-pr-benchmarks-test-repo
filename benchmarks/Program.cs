using System.IO;
using System.Text;
using static SimpleExec.Command;

namespace benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            const string reportFile =
@"``` ini

HI

```
TABLE
";

            var root = new DirectoryInfo(Path.Join(Path.GetDirectoryName(typeof(Program).Assembly.Location), "..", "..", "..")).FullName;
            var dir = Path.Join(root, ".bdn", "results");
            Directory.CreateDirectory(dir);

            var currentBranch = Read("git.exe", "branch --show-current", noEcho: true);
            var lastCommitMsg = Read("git.exe", "log -1 --pretty=%B", noEcho: true);

            var sb = new StringBuilder();
            sb.Append(reportFile);
            sb.Append("\n");
            var filter = args.Length == 2 && args[0] == "--filter" ? args[1] : "";
            sb.Append($"Filter: {filter}\n");
            sb.Append($"Branch: {currentBranch}");
            sb.Append($"Last commit message: {lastCommitMsg[..^1]}");

            File.WriteAllText(Path.Join(dir, "test-report.md"), sb.ToString());
        }
    }
}
