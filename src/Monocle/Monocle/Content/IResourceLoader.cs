using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Content.Serialization;
using System.Reflection;
using System.IO;

namespace Monocle.Content
{
    public interface IResourceLoader 
    {
        object ImportContent(Stream stream, string fileExt, IImporter importer = null);
        object ProcessContent(object content, IProcessor processor = null);
    }

    public class ResourceLoader : IResourceLoader
    {
        private readonly Dictionary<string, IImporter> defaultImporters;
        private readonly Dictionary<Type, IProcessor> defaultProcessors;
        
        public ResourceLoader() 
        {
            defaultImporters = new Dictionary<string, IImporter>();
            defaultProcessors = new Dictionary<Type, IProcessor>();
        }

        public void AddImportersAndProcessors(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsDefined(typeof(ImporterAttribute), false))
                        this.AddImporter(type);
                    else if (type.IsDefined(typeof(ProcessorAttribute), false))
                        this.AddProcessor(type);
                }
            }
        }

        private void AddProcessor(Type processorType)
        {
            var attrib = (ProcessorAttribute)processorType.GetCustomAttributes(typeof(ProcessorAttribute), false)[0];
            if (!attrib.IsDefault)
                return;

            var processor = (IProcessor)Activator.CreateInstance(processorType);

            this.defaultProcessors.Add(attrib.InputType, processor);
        }

        private void AddImporter(Type importerType)
        {
            var attrib = (ImporterAttribute)importerType.GetCustomAttributes(typeof(ImporterAttribute), false)[0];
            if (!attrib.IsDefault)
                return;

            var importer = (IImporter)Activator.CreateInstance(importerType);

            foreach (var fileEnding in attrib.FileEndings)
            {
                this.defaultImporters.Add(fileEnding, importer);
            }
        }

        private IImporter GetDefaultImporter(string fileExt)
        {
            if (this.defaultImporters.ContainsKey(fileExt))
                return this.defaultImporters[fileExt];

            throw new ResourceException(string.Format("The resource loader cannot find any importer capable of importing a {0} file.", fileExt));
        }

        private IProcessor GetDefaultProcessor(Type type)
        {
            if (this.defaultProcessors.ContainsKey(type))
                return this.defaultProcessors[type];

            throw new ResourceException(string.Format("The resource loader cannot find any processor capable of processing an object of type {0}.", type));
        }

        public object ImportContent(Stream stream, string fileExt, IImporter importer = null)
        {
            if (importer == null)
            {
                importer = GetDefaultImporter(fileExt);
            }

            return importer.Import(stream);
        }

        public object ProcessContent(object content, IProcessor processor = null)
        {
            if (processor == null)
                processor = GetDefaultProcessor(content.GetType());

            return processor.Process(content);
        }
    }
}