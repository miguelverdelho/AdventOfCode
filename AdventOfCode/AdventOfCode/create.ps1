$DAY = $args[0]
$class = @"
namespace AdventOfCode._2025;

public class Day$DAY
{
    static string projectRootPath = Directory.GetCurrentDirectory();
    static string _inputFilePath = Path.Combine(projectRootPath, "2025","inputs", "day${DAY}_sample.txt");
    public void RunPart1()
    {
       
    }

    public void RunPart2()
    {

    }
}
"@
Set-Content "2025/Day$DAY.cs" $class
New-Item -ItemType File -Path "2025/inputs/day${DAY}_sample.txt" -Force | Out-Null
New-Item -ItemType File -Path "2025/inputs/day${DAY}.txt" -Force | Out-Null