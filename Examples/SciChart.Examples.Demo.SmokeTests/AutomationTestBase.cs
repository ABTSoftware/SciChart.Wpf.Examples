using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using NUnit.Framework;

namespace SciChart.Examples.Demo.SmokeTests
{
    public class AutomationTestBase
    {
        public const double DefaultDpiX = 96.0;
        public const double DefaultDpiY = 96.0;
        public const int BigWaitTimeout = 5000;
        public const int SmallWaitTimeout = 1000;

        public static string ResourcesPath = @"SciChart\Examples\Demo\SmokeTests\Resources\Expectations\";
        public static string ExportActualPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                        "SciChart",
                        "SciChartAutomationTests_v6", // NOTE FOR MERGES: Master/v6 branch must have different folders here to avoid conflict in teamcity
                        "Actuals");

        static AutomationTestBase()
        {
            // Delete export actuals before test run 
            if (Directory.Exists(ExportActualPath))
            {
                Console.WriteLine("Deleting AutomationTestBase.ExportActualPath: " + ExportActualPath);
                Directory.Delete(ExportActualPath, true);

                Console.WriteLine("Creating AutomationTestBase.ExportActualPath: " + ExportActualPath);
                Directory.CreateDirectory(ExportActualPath);
            }
        }

        public void ExportActual(WriteableBitmap actualBitmap, string fileName)
        {
            if (!Directory.Exists(ExportActualPath))
            {
                Directory.CreateDirectory(ExportActualPath);
            }

            var pathString = Path.Combine(ExportActualPath, fileName);

            if (Path.GetExtension(fileName).ToUpper() == ".BMP")
            {
                SaveToBmp(pathString, actualBitmap);
            }
            else
            {
                SaveToPng(pathString, actualBitmap);
            }

            ProcessStartInfo startInfo = new ProcessStartInfo(pathString) { Verb = "edit" };
            Process.Start(startInfo);
        }

        public bool CompareBitmaps(string resourceName, WriteableBitmap actual, WriteableBitmap expected, double allowableErrorPercent = 1)
        {
            try
            {
                double averageError = 0.0, maxError = double.MinValue;

                Assert.That(new Size(actual.PixelWidth, actual.PixelHeight), Is.EqualTo(new Size(expected.PixelWidth, expected.PixelHeight)), "Image sizes are different!");

                var px1 = actual.ToByteArray();
                var px2 = expected.ToByteArray();

                Assert.That(px1.Length, Is.EqualTo(px2.Length), "Image pixel counts are different sizes!");

                var areEqual = true;

                for (int i = 0; i < px1.Length; i += 4)
                {
                    var b1 = px1[i];
                    var g1 = px1[i + 1];
                    var r1 = px1[i + 2];
                    var a1 = px1[i + 3];

                    var b2 = px2[i];
                    var g2 = px2[i + 1];
                    var r2 = px2[i + 2];
                    var a2 = px2[i + 3];

                    // Use Euclidean distance to find the difference
                    var error = Math.Sqrt((a1 - a2) * (a1 - a2) + (r1 - r2) * (r1 - r2) + (g1 - g2) * (g1 - g2) + (b1 - b2) * (b1 - b2));

                    averageError += error;
                    maxError = Math.Max(error, maxError);
                }

                // Compute average Euclidean distance
                averageError /= (actual.PixelWidth * actual.PixelHeight * 5.10);
                maxError /= 5.10;
                if (averageError > allowableErrorPercent)
                {
                    // Error threshold exceeded 
                    areEqual = false;
                }

                /*
                 * Image comparison by comparing hash-codes. Taken from 
                 * http://www.codeproject.com/Articles/9299/Comparing-Images-using-GDI
                 * 
                    SHA256Managed shaM = new SHA256Managed();
                    byte[] hash1 = shaM.ComputeHash(px1);
                    byte[] hash2 = shaM.ComputeHash(px2);

                    //Compare the hash values
                    for (int i = 0; i < hash1.Length && i < hash2.Length
                                      && cr == CompareResult.ciCompareOk; i++)
                    {
                        if (hash1[i] != hash2[i])
                            cr = CompareResult.ciPixelMismatch;
                    }
                 */
                string message = $"Resource: {resourceName}, Image AveError: {averageError}%, Allowable AveError: {allowableErrorPercent}%, MaxError: {maxError}%";
                Assert.That(areEqual, message);
                Console.WriteLine(message);

                return true;
            }
            catch
            {
                // Output diff file 
                SaveDiffImages(resourceName, expected, actual);

                throw;
            }
        }

        public void SaveToPng(string fileName, WriteableBitmap bmp)
        {
            var path = Path.GetDirectoryName(fileName);

            if (!string.IsNullOrEmpty(path))
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                //pngEncoder.Palette = new BitmapPalette(bmp, int.MaxValue);
                pngEncoder.Frames.Add(BitmapFrame.Create(bmp));

                using (var fileStream = File.Create(fileName))
                {
                    pngEncoder.Save(fileStream);
                }
            }
        }

        public void SaveDiffImages(string resourceName, WriteableBitmap expectedBitmap, WriteableBitmap actualBitmap)
        {
            string expectedPath = Path.Combine(ExportActualPath,
                Path.GetFileNameWithoutExtension(resourceName) + "-expected.png");

            string actualPath = Path.Combine(ExportActualPath,
                Path.GetFileNameWithoutExtension(resourceName) + "-actual.png");

            SaveToPng(expectedPath, expectedBitmap);
            Console.WriteLine(@"Expected bitmap saved to " + expectedPath);

            SaveToPng(actualPath, actualBitmap);
            Console.WriteLine(@"Actual bitmap saved to " + actualPath);

            var byteExpected = expectedBitmap.ToByteArray();
            var byteActual = actualBitmap.ToByteArray();
            if (byteExpected.Length == byteActual.Length)
            {
                var byteDiff = new byte[byteExpected.Length];
                for (int i = 0; i < byteExpected.Length; i++)
                {
                    // For alpha use the average of both images (otherwise pixels with the same alpha won't be visible)
                    if ((i + 1) % 4 == 0)
                        byteDiff[i] = (byte)((byteActual[i] + byteExpected[i]) / 2);
                    else
                        byteDiff[i] = (byte)Math.Abs(byteActual[i] - byteExpected[i]);
                }
                var diffBmp = new WriteableBitmap(expectedBitmap.PixelWidth, expectedBitmap.PixelHeight, expectedBitmap.DpiX,
                    expectedBitmap.DpiY, expectedBitmap.Format, expectedBitmap.Palette);
                diffBmp.WritePixels(new Int32Rect(0, 0, expectedBitmap.PixelWidth, expectedBitmap.PixelHeight), byteDiff,
                    expectedBitmap.BackBufferStride, 0);

                string diffPath = Path.Combine(ExportActualPath, Path.GetFileNameWithoutExtension(resourceName) + "-diff.png");

                SaveToPng(diffPath, diffBmp);
                Console.WriteLine(@"Difference bitmap saved to " + diffPath);
            }
        }

        public void SaveToBmp(string fileName, WriteableBitmap bmp)
        {
            BmpBitmapEncoder bmpEncoder = new BmpBitmapEncoder();
            //pngEncoder.Palette = new BitmapPalette(bmp, int.MaxValue);
            bmpEncoder.Frames.Add(BitmapFrame.Create(bmp));

            using (var fileStream = File.Create(fileName))
            {
                bmpEncoder.Save(fileStream);
            }
        }

        public WriteableBitmap LoadResource(string resourceName)
        {
            resourceName = resourceName.Replace("/", ".");
            WriteableBitmap expectedBitmap = null;
            var assembly = GetType().Assembly;

            // For testing purposes, to see all the resources available
            var resourcePath = assembly.GetManifestResourceNames().FirstOrDefault(x => x.ToUpper().Contains(resourceName.ToUpper()));

            using (var resourceStream = assembly.GetManifestResourceStream(resourcePath))
            {
                expectedBitmap = Path.GetExtension(resourceName).ToUpper() == ".BMP"
                    ? DecodeBmpStream(resourceStream)
                    : DecodePngStream(resourceStream);
            }

            return expectedBitmap;
        }

        public WriteableBitmap LoadFromPng(string fileName)
        {
            WriteableBitmap bmp;

            using (var fileStream = File.OpenRead(fileName))
            {
                bmp = DecodePngStream(fileStream);
            }

            return bmp;
        }

        public T WaitForElement<T>(Func<T> getter)
        {
            var retry = Retry.WhileNull<T>(
                getter,
                TimeSpan.FromMilliseconds(BigWaitTimeout));

            if (!retry.Success)
            {
                Assert.Fail($"Failed to get an element within a {BigWaitTimeout}ms");
            }

            return retry.Result;
        }

        public void WaitUntilClosed(AutomationElement element)
        {
            var result = Retry.WhileFalse(() => element.IsOffscreen, TimeSpan.FromMilliseconds(BigWaitTimeout));
            if (!result.Success)
            {
                Assert.Fail($"Element failed to go offscreen within {BigWaitTimeout}ms");
            }
        }

        public string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        private WriteableBitmap DecodePngStream(Stream pngStream)
        {
            var decoder = new PngBitmapDecoder(pngStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];

            return new WriteableBitmap(bitmapSource);
        }

        private WriteableBitmap DecodeBmpStream(Stream bmpStream)
        {
            var decoder = new BmpBitmapDecoder(bmpStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];

            return new WriteableBitmap(bitmapSource);
        }
    }
}
