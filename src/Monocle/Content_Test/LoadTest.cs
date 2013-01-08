using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Monocle.Content.Serialization;
using Monocle.Content;
using System.IO;

namespace Content_Test
{
    [TestFixture]
    class LoadTest
    {
        IResourceManager manager;

        [SetUp]
        public void Setup()
        {
            var readerFactory = new TypeReaderFactory();
            var writerFactory = new TypeWriterFactory();
            var loader = new ResourceLoader();
            loader.AddImportersAndProcessors(typeof(IImporter).Assembly);

            manager = new ResourceManager(readerFactory, writerFactory, loader, Environment.CurrentDirectory + "\\Assets", Environment.CurrentDirectory);
        }

        [Test]
        public void CanLoadTextFileToString()
        {
            string text = manager.LoadAsset<string>("Test Assets\\LoadTextTest.txt");
            Assert.AreEqual("This is a Test!", text);


            File.Delete("Assets\\LoadTextTest.asset");
        }


        [Test]
        public void CanLoadAssetFileToString()
        {
            string text = manager.LoadAsset<string>("Test Assets\\LoadTextTest.asset");
            Assert.AreEqual("This is a Test!", text);


            File.Delete("Assets\\LoadTextTest.asset");
        }

        [Test]
        public void CanSaveAsset()
        {
            string text = "Saving a string can be done here!";
            manager.SaveAsset("Test Assets\\SaveTestString", text);

            string loaded = manager.LoadAsset<string>("Test Assets\\SaveTestString.asset");
            Assert.AreEqual(text, loaded);

            File.Delete("Test Assets\\SaveTestString.asset");
            File.Delete("Assets\\SaveTestString.asset");
        }
    }
}