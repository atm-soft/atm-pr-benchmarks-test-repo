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

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.778 (1909/November2018Update/19H2)
Intel Core i9-9900K CPU 3.60GHz (Coffee Lake), 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=5.0.100-preview.2.20176.6
  [Host]     : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT
  Job-BILSGC : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT

PowerPlanMode=00000000-0000-0000-0000-000000000000  IterationTime=250.0000 ms  MaxIterationCount=20  
MinIterationCount=15  WarmupCount=1  

```
|     Method |     Mean |   Error |  StdDev |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------- |---------:|--------:|--------:|-------:|-------:|------:|----------:|
| TestMethod | 115.2 μs | 0.51 μs | 0.43 μs | 9.2593 | 0.9259 |     - |  79.12 KB |";

            var root = new DirectoryInfo(Path.Join(Path.GetDirectoryName(typeof(Program).Assembly.Location), "..", "..", "..")).FullName;
            var dir = Path.Join(root, ".bdn", "results");
            Directory.CreateDirectory(dir);

            var currentBranch = Read("git.exe", "branch --show-current", noEcho: true);
            var lastCommitMsg = Read("git.exe", "log -1 --pretty=%B", noEcho: true);

            var sb = new StringBuilder();
            sb.Append(reportFile);
            sb.AppendLine();
            sb.AppendLine();
            var filter = args.Length == 2 && args[0] == "--filter" ? args[1] : "";
            sb.AppendLine($"Filter: {filter}");
            sb.Append($"Branch: {currentBranch}");
            sb.Append($"Last commit message: {lastCommitMsg[..^1]}");

            File.WriteAllText(Path.Join(dir, "test-report.md"), sb.ToString());
        }
    }
}
