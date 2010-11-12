using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using Raven.Client.Document;
using Raven.Client;
using System.Security.Cryptography;

namespace AppVisum.Sys
{
    [Flags]
    public enum AppPluginStatus : short
    {
        New = 1,
        Instance = 2,
        Enabled = 4,
        Model = 8,
        Install = 16,
        SetupIncomplete = 32
    }

    public sealed class AppSys
    {
        private static volatile AppSys instance = null;
        private static readonly object instanceLock = new object();
        public static AppSys Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                        instance = new AppSys();
                }
                return instance;
            }
        }

        private PluginSettingsProvider pluginSettingsProvider;
        private DocumentStore db;
        private List<Plugin> installedPluggins = new List<Plugin>();

        internal IEnumerable<Plugin> InstalledPlugins
        {
            get
            {
                foreach (var p in installedPluggins)
                    yield return p;
                yield break;
            }
        }

        [ImportMany]
        private IEnumerable<IAppProvider> Providers { get; set; }

        private AppSys()
        {
            Compose();
            LoadDb();
            Setup();
        }

        private void Setup()
        {
            pluginSettingsProvider = new PluginSettingsProvider(this);

            foreach (var p in PluginConfigs)
            {
                Plugin pl = new Plugin();
                AppPluginStatus status = (AppPluginStatus)0;
                if (p.Status == Configuration.PluginStatus.Enabled)
                    status |= AppPluginStatus.Enabled;

                Type t = Type.GetType(p.PluginTypeName);
                pl.PluginObj = (IAppProvider)Activator.CreateInstance(t);
                if (t.HasInterface(typeof(IAppModelProvider)))
                    status |= AppPluginStatus.Model;
                if (t.HasInterface(typeof(IAppInstancedProvider)))
                    status |= AppPluginStatus.Instance;
                if (t.HasInterface(typeof(IAppProviderInstallWizzard)))
                {
                    status |= AppPluginStatus.Install;
                    if (p.InstallStep < pl.P<IAppProviderInstallWizzard>().InstallationStepsCount)
                        status |= AppPluginStatus.SetupIncomplete;
                }

                pl.Status = status;
                pl.InstanceName = p.InstanceName;
                pl.Id = Plugin.GetPluginId(p.Id);

                installedPluggins.Add(pl);
            }
        }

        private void Compose()
        {
            //var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var directory = Path.GetDirectoryName(@"C:\Users\Alxandr\Documents\Visual Studio 2010\Projects\AppVisum\AppVisum\bin\");
            var container = new CompositionContainer(new DirectoryCatalog(directory));
            container.ComposeParts(this);
        }

        private void LoadDb()
        {
            db = new DocumentStore { Url = "http://localhost:8080" };
            db.Initialize();
        }

        public IEnumerable<Configuration.Plugin> PluginConfigs
        {
            get
            {
                using (IDocumentSession session = db.OpenSession())
                {
                    return session.Query<Configuration.Plugin>();
                }
            }
        }

        public IEnumerable<Plugin> GetTypeMatchingPlugins(Type type)
        {
            if (type.IsInterface)
                return installedPluggins.Where(p => p.PluginObj.GetType().HasInterface(type));
            else
                return installedPluggins.Where(p => p.PluginObj.GetType() == type);
        }

        public IDocumentSession GetDBSession()
        {
            return db.OpenSession();
        }

        public PluginSettingsProvider PluginSettings
        {
            get { return pluginSettingsProvider; }
        }

        public IEnumerable<Plugin> Plugins
        {
            get
            {
                ISet<Guid> plugins = new SortedSet<Guid>();
                foreach (var p in installedPluggins)
                {
                    plugins.Add(p.PluginObj.GetType().GUID);
                    yield return p;
                }

                foreach (var p in Providers)
                {
                    if (plugins.Contains(p.GetType().GUID) && !p.GetType().HasInterface(typeof(IAppInstancedProvider)))
                        continue;

                    Plugin pl = new Plugin();
                    AppPluginStatus status = AppPluginStatus.New;

                    Type t = p.GetType();
                    pl.PluginObj = p;
                    if (t.HasInterface(typeof(IAppModelProvider)))
                        status |= AppPluginStatus.Model;
                    if (t.HasInterface(typeof(IAppInstancedProvider)))
                        status |= AppPluginStatus.Instance;
                    if (t.HasInterface(typeof(IAppProviderInstallWizzard)))
                        status |= AppPluginStatus.Install;

                    pl.Status = status;
                    pl.InstanceName = "";
                    pl.Id = p.GetType().GUID;

                    yield return pl;
                }

                yield break;
            }
        }

        public string HashString(string input)
        {
            var hasher = new SHA256CryptoServiceProvider();
            byte[] hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
            hasher.Clear();

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        public string EncryptString(string input, string key)
        {
            byte[] result;

            // Generate key
            var hasher = new MD5CryptoServiceProvider();
            byte[] TDESKey = hasher.ComputeHash(Encoding.UTF8.GetBytes(key));

            // Create a TDES provider
            var algorithm = new TripleDESCryptoServiceProvider();

            // Setup TDES
            algorithm.Key = TDESKey;
            algorithm.Mode = CipherMode.ECB;
            algorithm.Padding = PaddingMode.PKCS7;

            // Get input byte-data
            byte[] data = Encoding.UTF8.GetBytes(input);

            // Attempt to encrypt
            try
            {
                var encryptor = algorithm.CreateEncryptor();
                result = encryptor.TransformFinalBlock(data, 0, data.Length);
            }
            finally
            {
                algorithm.Clear();
                hasher.Clear();
            }

            // Return base64 encoding of data
            return Convert.ToBase64String(result);
        }

        public string DecryptString(string input, string key)
        {
            byte[] result;

            // Generate key
            var hasher = new MD5CryptoServiceProvider();
            byte[] TDESKey = hasher.ComputeHash(Encoding.UTF8.GetBytes(key));

            // Create a TDES provider
            var algorithm = new TripleDESCryptoServiceProvider();

            // Setup TDES
            algorithm.Key = TDESKey;
            algorithm.Mode = CipherMode.ECB;
            algorithm.Padding = PaddingMode.PKCS7;

            // Get input byte-data
            byte[] data = Encoding.UTF8.GetBytes(input);

            // Attempt to encrypt
            try
            {
                var decryptor = algorithm.CreateDecryptor();
                result = decryptor.TransformFinalBlock(data, 0, data.Length);
            }
            finally
            {
                algorithm.Clear();
                hasher.Clear();
            }

            // Return base64 encoding of data
            return Encoding.UTF8.GetString(result);
        }

        public Plugin GetPluginById(Guid id)
        {
            return installedPluggins.SingleOrDefault(p => p.Id == id);
        }

        public void Restart()
        {
            try
            {
                System.Web.HttpRuntime.UnloadAppDomain();
            }
            catch
            {

            }
        }
    }
}
