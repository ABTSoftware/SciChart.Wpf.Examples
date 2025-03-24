param (
    [Parameter(Mandatory=$true)][string]$ExamplesDir,
    [Parameter(Mandatory=$true)][string]$PackageVersion,
    [Parameter(Mandatory=$true)][string[]]$PackageNames
)

$code = 
	@"
	using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
	
	public class NuGetCodeTools
	{
		public static void AddReferences(string examplesPath, string packageVersion, string[] packageNames)
        {           
            var files = Directory.GetFiles(examplesPath, "*.*", SearchOption.AllDirectories).Where(file => file.EndsWith(".csproj"));
												
			Console.WriteLine("AddReferences: Found {0} file(s) in directory {1}", files.Count(), examplesPath);
			
            foreach(string file in files)
            {
                List<string> lines = new List<string>(File.ReadAllLines(file));

                int startIndex = -1;
                int endIndex = -1;

                for (int i = 0; i < lines.Count; i++ )
                {
                    string collapsed = lines[i].Replace(" ", "");

                    if (startIndex == -1 && collapsed.Contains("<PackageReferenceInclude=\"SciChart"))
                    {
                        lines[i] = string.Empty;
                        System.Console.WriteLine("File: {0}, removed <PackageReference>, line {1}", file, i);
                    }

                    if (startIndex == -1 && collapsed.Contains("<!--ScriptReferences-->"))
                    {
                        startIndex = i;
                    }

                    if (startIndex != -1 && collapsed.Contains("<!--/ScriptReferences-->"))
                    {
                        endIndex = i;
                        System.Console.WriteLine("File: {0}, cleared <ScriptReferences> section, lines {1}-{2}", file, startIndex, endIndex);

                        for (int j = endIndex; j >= startIndex; j--)
                        {
                            lines[j] = string.Empty;
                        }                        
                        startIndex = -1;
                    }
                }

                lines[lines.Count - 1] = string.Empty;
                lines.Add("\t<!--ScriptReferences-->");
                lines.Add("\t<ItemGroup>");
                
                foreach (string packageName in packageNames)
                {
                    lines.Add(string.Format("\t\t<PackageReference Include=\"{0}\" Version=\"{1}\"/>", packageName, packageVersion));
					System.Console.WriteLine("File: {0}, added <PackageReference> to {1} package, version {2}", file, packageName, packageVersion);
                }

                lines.Add("\t</ItemGroup>");
                lines.Add("\t<!--/ScriptReferences-->");
                lines.Add("</Project>");
                
                using (StreamWriter writer = File.CreateText(file))
                {
                    foreach (string line in lines)
                    {
                        if (line != string.Empty)
                        {
                            writer.WriteLine(line);
                        }
                    }
                }               
            }
        }
	}
"@

if (-not ("NuGetCodeTools" -as [type])) {

    Add-Type -TypeDefinition $code -Language CSharp  
}

[NuGetCodeTools]::AddReferences($ExamplesDir, $PackageVersion, $PackageNames)