param (
    [Parameter(Mandatory=$true)][string]$ExamplesDir,
    [Parameter(Mandatory=$true)][string]$DllsDir,
    [Parameter(Mandatory=$true)][string[]]$DllFiles
)

$code = 
	@"
	using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
	
	public class LibCodeTools
	{
		public static void AddReferences(string examplesPath, string dllsPath, string[] dllFiles)
        {           
            var files = Directory.GetFiles(examplesPath, "*.*", SearchOption.AllDirectories).Where(file => file.EndsWith(".csproj"));
			
			var frameworks = new string[] {"net462", "net6.0-windows", "netcoreapp3.1"};
												
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
				
				foreach(string framework in frameworks)
				{
					lines.Add(string.Format("\t<ItemGroup Condition=\"'`$(TargetFramework)' == '{0}'\">", framework));
					
					foreach (string dll in dllFiles)
					{
						lines.Add(string.Format("\t\t<Reference Include=\"{0}\" HintPath=\"{1}\\{2}\\{0}.dll\"/>", dll, dllsPath, framework));
						System.Console.WriteLine("File: {0}, added <Reference> to {1}.dll", file, dll);
					}
					
					lines.Add("\t</ItemGroup>");
				}
				
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

if (-not ("LibCodeTools" -as [type])) {

    Add-Type -TypeDefinition $code -Language CSharp  
}

[LibCodeTools]::AddReferences($ExamplesDir, $DllsDir, $DllFiles)