using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Content.Serialization;
using System.IO;

namespace Monocle.Content
{
    public interface IResourceManager
    {
        /// <summary>
        /// Loads a object.
        /// </summary>
        /// <typeparam name="T">The type of the object to load.</typeparam>
        /// <param name="name">The path of the file to load.</param>
        /// <param name="importer">A special importer or null to use the default importer.</param>
        /// <param name="processor">A special processor or null to use the default processor.</param>
        /// <returns>A loaded object of type T.</returns>
        T LoadAsset<T>(string relativePath, IImporter importer = null, IProcessor processor = null);

        /// <summary>
        /// Saves an asset to the location specified.
        /// </summary>
        /// <param name="location">A path relative to the RootDirectory. (without extention)</param>
        /// <param name="asset">The asset to save. (cannot be null)</param>
        void SaveAsset(string relativePath, object asset);

        /// <summary>
        /// Unloads the asset.
        /// </summary>
        /// <param name="relativePath">The path to the asset to unload.</param>
        void UnloadAsset(string relativePath);

        /// <summary>
        /// Unloads the asset.
        /// </summary>
        /// <param name="asset">The asset to unload.</param>
        void UnloadAsset(object asset);

        /// <summary>
        /// The root directory that resources are managed in.
        /// </summary>
        string RootDirectory { get; }
    }


    class ResourceManager : IResourceManager
    {
        private const string ASSET_EXT = ".asset";

        private readonly Dictionary<string, object> loaded;
        private readonly ITypeReaderFactory trfactory;
        private readonly ITypeWriterFactory twfactory;
        private readonly IResourceLoader resourceLoader;

        private readonly string assetDirectory;

        public string RootDirectory
        {
            get;
            private set;
        }


        public ResourceManager(ITypeReaderFactory trfactory, ITypeWriterFactory twfactory, IResourceLoader loader, string assetDir, string rootDir)
        {
            this.loaded = new Dictionary<string, object>();
            this.trfactory = trfactory;
            this.twfactory = twfactory;
            this.resourceLoader = loader;
            this.assetDirectory = assetDir;
            this.RootDirectory = rootDir;
        }


        public T LoadAsset<T>(string relativePath, IImporter importer = null, IProcessor processor = null)
        {
            if (loaded.ContainsKey(relativePath))
                return (T)loaded[relativePath];

            string filePath = Path.GetFullPath(Path.Combine(this.RootDirectory, relativePath));
            string assetPath = Path.GetFullPath(Path.Combine(this.assetDirectory, Path.GetFileNameWithoutExtension(relativePath) + ASSET_EXT));

            T result;

            if (Path.GetExtension(relativePath).ToLower() == ASSET_EXT)
            {
                result = LoadAssetAsset<T>(filePath, assetPath);
            }
            else
            {
                result = ImportAndLoadAsset<T>(filePath, assetPath, importer, processor);
            }

            this.loaded.Add(relativePath, result);
            return result;
        }

        private T ImportAndLoadAsset<T>(string filePath, string assetPath, IImporter importer, IProcessor processor)
        {
            object asset;
            using (Stream stream = new FileStream(filePath, FileMode.Open))
            {
                asset = this.resourceLoader.ImportContent(stream, Path.GetExtension(filePath), importer);
            }
            asset = this.resourceLoader.ProcessContent(asset, processor);

            using (Stream stream = new FileStream(assetPath, FileMode.Create,FileAccess.ReadWrite, FileShare.None))
            {
                AssetWriter.WriteAsset(stream, asset, this.twfactory);
                stream.Position = 0;
                asset = AssetReader.ReadAsset<T>(stream, this.trfactory);
            }

            return (T)asset;
        }

        private T LoadAssetAsset<T>(string filePath, string assetPath)
        {
            if(filePath != assetPath)
                File.Copy(filePath, assetPath, true);

            T result;
            using (Stream stream = new FileStream(assetPath, FileMode.Open))
            {
                result = AssetReader.ReadAsset<T>(stream, this.trfactory);
            }
            return result;
        }

        public void SaveAsset(string relativePath, object asset)
        {
            string filePath = Path.GetFullPath(Path.Combine(this.RootDirectory, relativePath + ASSET_EXT));

            using (Stream stream = new FileStream(filePath, FileMode.CreateNew))
            {
                AssetWriter.WriteAsset(stream, asset, this.twfactory);
            }
        }

        public void UnloadAsset(string relativePath)
        {
            //TODO have more complex unloading functionallity!

            this.loaded.Remove(relativePath);
        }

        public void UnloadAsset(object asset)
        {
            string key = null;
            foreach (var item in this.loaded)
            {
                if (asset == item.Value)
                {
                    key = item.Key;
                    break;
                }
            }

            this.loaded.Remove(key);
        }
    }
}
