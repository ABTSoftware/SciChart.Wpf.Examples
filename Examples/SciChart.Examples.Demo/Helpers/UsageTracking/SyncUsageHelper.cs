using SciChart.UI.Bootstrap;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Unity;

namespace SciChart.Examples.Demo.Helpers.UsageTracking
{
    [ExportType(typeof(ISyncUsageHelper), CreateAs.Singleton)]
    public class SyncUsageHelper : ISyncUsageHelper
    {
        private class DataEncryptionHelper
        {
            string _namespace = "Abt.Licensing.New";
            string _class = "SodiumSymmetricEncryption";
            string _decodingMethod = "Decrypt";
            string _encodingMethod = "Encrypt";

            public string Decrypt(string encrypted)
            {
                var decrypted = String.Empty;
                var decodingMethod = GetEncoderType()?
                    .GetMethod(_decodingMethod);
                if (decodingMethod != null)
                {
                    decrypted = decodingMethod.Invoke(null, new object[] { encrypted }) as string;
                }

                return decrypted;
            }

            private Type GetEncoderType()
            {
                var type = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .FirstOrDefault(assemblyType => assemblyType.Name == _class &&
                                                    assemblyType.Namespace == _namespace);

                return type;
            }

            public string Encrypt(string raw)
            {
                var encrypted = String.Empty;
                var encodingMethod = GetEncoderType()?
                    .GetMethod(_encodingMethod);
                if (encodingMethod != null)
                {
                    encrypted = encodingMethod.Invoke(null, new object[] { raw }) as string;
                }

                return encrypted;
            }
        }

        private string userId;
        private DateTime lastSent = DateTime.MinValue;
        private readonly IUsageCalculator _usageCalculator;
        private readonly IUsageServiceClient _client;
        private readonly DataEncryptionHelper _encryptionHelper;
        private bool _enabled = true;

        public event EventHandler<EventArgs> EnabledChanged;

        public SyncUsageHelper(IUsageCalculator usageCalculator, IUsageServiceClient client)
        {
            _encryptionHelper = new DataEncryptionHelper();
            _usageCalculator = usageCalculator;
            _client = client;
            userId = System.Guid.NewGuid().ToString();
        }

        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        public void LoadFromIsolatedStorage()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                if (isf.FileExists("Usage.xml"))
                {
                    try
                    {
                        using (var stream = new IsolatedStorageFileStream("Usage.xml", FileMode.Open, isf))
                        {
                            var usageXml = "";
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                var encryptedUsage = reader.ReadToEnd();
                                try
                                {
                                    usageXml = _encryptionHelper.Encrypt(encryptedUsage);
                                }
                                catch
                                {
                                    // Old file contents will not decrypt due to encryption changes.  We don't care.
                                }
                            }

                            using (var textReader = new StringReader(usageXml))
                            {
                                var xDocument = XDocument.Load(textReader);

                                _usageCalculator.Usages = SerializationUtil.Deserialize(xDocument);
                                var userIdNode = xDocument.Root.Attributes("UserId").FirstOrDefault();
                                if (userIdNode != null)
                                {
                                    userId = userIdNode.Value;
                                }
                                var lastSentNode = xDocument.Root.Attributes("LastSent").FirstOrDefault();
                                if (lastSentNode != null)
                                {
                                    lastSent = DateTime.ParseExact(lastSentNode.Value, "o", null);
                                }
                                var enabledNode = xDocument.Root.Attributes("Enabled").FirstOrDefault();
                                if (enabledNode != null)
                                {
                                    _enabled = bool.Parse(enabledNode.Value);
                                }
                            }
                        }

                        var handler = EnabledChanged;
                        if (handler != null)
                        {
                            handler(this, EventArgs.Empty);
                        }
                    }
                    catch
                    {
                        // If something goes wrong, delete the local file
                        try { isf.DeleteFile("Usage.xml"); }
                        catch { }
                    }
                }
            }
        }

        public void WriteToIsolatedStorage()
        {
            try
            {
                using (var isf = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly,
                    null, null))
                {
                    using (var stream = new IsolatedStorageFileStream("Usage.xml", FileMode.Create, isf))
                    {
                        var xml = SerializationUtil.Serialize(
                            _usageCalculator.Usages.Values.Where(e => e.VisitCount > 0));
                        xml.Root.Add(new XAttribute("UserId", userId));
                        xml.Root.Add(new XAttribute("Enabled", Enabled));
                        xml.Root.Add(new XAttribute("LastSent", lastSent.ToString("o")));

                        using (var stringWriter = new StringWriter())
                        {
                            xml.Save(stringWriter);

                            var encryptedUsage = _encryptionHelper.Encrypt(stringWriter.ToString());
                            using (StreamWriter writer = new StreamWriter(stream))
                            {
                                writer.Write(encryptedUsage);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        public void SendUsagesToServer()
        {
            if (!_enabled)
                return;
            // Send only recently updated usage
            _client.SendLocalUsage(userId, _usageCalculator.Usages.Values.Where(e => e.VisitCount > 0 && e.LastUpdated > lastSent).ToList());
            lastSent = DateTime.UtcNow;
        }

        public void GetRatingsFromServer()
        {
            if (!_enabled)
                return;

            //var ratings = _client.GetGlobalUsage();
            //ratings.ContinueWith(r =>
            //{
            //if (r.Result != null)
            //    _usageCalculator.Ratings = r.Result.ToDictionary<ExampleRating, string>(x => x.ExampleID);
            //});
        }

        public void SetUsageOnExamples()
        {
            var module = ServiceLocator.Container.Resolve<IModule>();
            foreach (var example in module.Examples.Values)
            {
                example.Usage = _usageCalculator.GetUsage(example);
            }
        }
    }
}