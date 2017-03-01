using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Practices.Unity;
using System;
using SciChart.Wpf.UI.Bootstrap;

namespace SciChart.Examples.Demo.Helpers.UsageTracking
{
    [ExportType(typeof(ISyncUsageHelper), CreateAs.Singleton)]
    public class SyncUsageHelper : ISyncUsageHelper
    {
        private string userId;
        private DateTime lastSent = DateTime.MinValue;
        private readonly IUsageCalculator _usageCalculator;
        private readonly IUsageServiceClient _client;
        private bool _enabled = true;

        public event EventHandler<EventArgs> EnabledChanged;

        private readonly string _password = "vankNBVDavigfuakbvja";

        public SyncUsageHelper(IUsageCalculator usageCalculator, IUsageServiceClient client)
        {
            _usageCalculator = usageCalculator;
            _client = client;
            userId = System.Guid.NewGuid().ToString();
        }

        public bool Enabled { get { return _enabled; } set { _enabled = value; } }

        public void LoadFromIsolatedStorage()
        {
#if !SILVERLIGHT
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
#else
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForSite())
#endif
            {
                if (isf.FileExists("Usage.xml"))
                {
                    try
                    {
                        using (var stream = new IsolatedStorageFileStream("Usage.xml", FileMode.Open, isf))
                        {
                            string usageXml = "";
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                var encryptedUsage = reader.ReadToEnd();
                                usageXml = Abt.Licensing.Core.EncryptionUtil.Decrypt(encryptedUsage, _password);
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
#if !SILVERLIGHT
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
#else
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForSite())
#endif
            {
                using (var stream = new IsolatedStorageFileStream("Usage.xml", FileMode.Create, isf))
                {
                    XDocument xml = SerializationUtil.Serialize(_usageCalculator.Usages.Values.Where(e => e.VisitCount > 0));
                    xml.Root.Add(new XAttribute("UserId", userId));
                    xml.Root.Add(new XAttribute("Enabled", Enabled));
                    xml.Root.Add(new XAttribute("LastSent", lastSent.ToString("o")));
                    
                    using (var stringWriter = new StringWriter())
                    {
                        xml.Save(stringWriter);
                        var encryptedUsage = Abt.Licensing.Core.EncryptionUtil.Encrypt(stringWriter.ToString(), _password);
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(encryptedUsage);
                        }
                    }
                }
            }
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
#if !SILVERLIGHT
            //var ratings = _client.GetGlobalUsage();
            //ratings.ContinueWith(r =>
            //{
            //if (r.Result != null)
            //    _usageCalculator.Ratings = r.Result.ToDictionary<ExampleRating, string>(x => x.ExampleID);
            //});
#else
            //_client.GetGlobalUsageAsync();

            //_client.GetGlobalUsageCompleted += (sender, args) =>
            //{
            //    if (args.Result.Any())
            //    {
            //        _usageCalculator.Ratings = args.Result;
            //    }
            //};
#endif
        }

        public void SetUsageOnExamples()
        {
            IModule module = ServiceLocator.Container.Resolve<IModule>();
            foreach (var example in module.Examples.Values)
            {
                example.Usage = _usageCalculator.GetUsage(example);
            }
        }
    }
}