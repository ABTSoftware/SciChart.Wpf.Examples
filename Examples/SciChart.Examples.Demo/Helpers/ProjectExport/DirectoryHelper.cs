using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace SciChart.Examples.Demo.Helpers.ProjectExport
{
    public static class DirectoryHelper
    {
        public static string GetFileNameFromPath(string path)
        {
            var index = path.LastIndexOf("/", StringComparison.InvariantCulture);

            if (index > 0)
            {
                return path.Substring(index + 1, path.Length - index - 1);
            }

            return null;
        }

        public static bool IsValidPath(string path, out string error)
        {
            error = null;

            if (string.IsNullOrEmpty(path))
            {
                error = "Path cannot be empty";
                return false;
            }

            if (path.Length < 3)
            {
                error = "Please set drive at least";
                return false;
            }

            var drive = path.Substring(0, 3); // e.g. C:\
            var driveCheck = new Regex(@"^[a-zA-Z]:\\$");

            if (!driveCheck.IsMatch(drive))
            {
                error = @"Drive should be set in the following format: ""_:\"", where ""_"" is a drive letter";
                return false;
            }

            if (!Directory.Exists(drive))
            {
                error = $"Drive {drive} is not found or inaccessible";
                return false;
            }

            var strTheseAreInvalidFileNameChars = new string(Path.GetInvalidPathChars());
            strTheseAreInvalidFileNameChars += @":/?*" + "\"";

            var containsBadCharacter = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");
            if (containsBadCharacter.IsMatch(path.Substring(3, path.Length - 3)))
            {
                error = "The given path's format is not supported";
                return false;
            }

            var containsDotCharacter = new Regex(@"^.*\\(.*\.+|\.{2,}.*)($|\\)");
            if (containsDotCharacter.IsMatch(path))
            {
                error = @"The given path with ""."" in folder name is not supported";
                return false;
            }

            return true;
        }

        public static bool HasWriteAccessToFolder(string folderPath, out string error)
        {
            error = null;
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    // We cannot verify write access to a non-existent folder
                    return true;
                }

                string fullPath = Path.Combine(folderPath, "file.temp");

                using (var fs = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write))
                {
                    fs.WriteByte(0xff);
                }

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                error = "This location is not accessible for writing.";
                return false;
            }
            catch
            {
                error = "This location is not found or inaccessible.";
                return false;
            }
        }

        public static string GetPathForExport(string defaultPath)
        {
            var isValidPath = false;

            string selectedPath = null;

            while (!isValidPath)
            {
                var dialog = new FolderBrowserDialog { SelectedPath = defaultPath };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    isValidPath = HasWriteAccessToFolder(dialog.SelectedPath, out string error);

                    if (isValidPath)
                    {
                        selectedPath = dialog.SelectedPath;
                    }
                    else
                    {
                        MessageBox.Show(error, "SciChart", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    isValidPath = true;
                }
            }

            return selectedPath;
        }
    }
}