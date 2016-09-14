using Dynamic;
using Dynamic.Storage.Json;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class DynamicDictionaryJsonStorageTests
    {
        [TestMethod]
        public void JsonStorageTests()
        {
            var name = "tests.json";

            var storage = new DynamicDictionaryJsonStorage(name)
            {
                JsonFormat = Newtonsoft.Json.Formatting.Indented
            };

            var source = new DynamicDictionary();

            var item1 = new List<string> { "Item1", "Item2", "Item3" };
            var item2 = new List<int> { 10, 20, 352 };
            var item3 = new List<DateTime> { DateTime.Now, DateTime.UtcNow };

            source.Storage = storage;
            source["item1"] = item1;
            source["item2"] = item2;
            source["item3"] = item3;
            source.Save();

            Assert.IsTrue(System.IO.File.Exists(name));

            DynamicDictionary target = storage.Load();

            Assert.AreEqual(source.Count, target.Count);
            Assert.AreEqual(source["item1"].Count, target["item1"].Count);
            Assert.AreEqual(source["item2"].Count, target["item2"].Count);
            Assert.AreEqual(source["item3"].Count, target["item3"].Count);

            Assert.AreEqual(source["item1"][0], target["item1"][0]);
            Assert.AreEqual(source["item1"][1], target["item1"][1]);
            Assert.AreEqual(source["item1"][2], target["item1"][2]);

            Assert.AreEqual(source["item2"][0], target["item2"][0]);
            Assert.AreEqual(source["item2"][1], target["item2"][1]);
            Assert.AreEqual(source["item2"][2], target["item2"][2]);

            Assert.AreEqual(source["item2"][0], target["item2"][0]);
            Assert.AreEqual(source["item2"][1], target["item2"][1]);
        }

        [TestMethod]
        public async Task JsonStorageTestsAsync()
        {
            var name = "testsAsync.json";

            var storage = new DynamicDictionaryJsonStorage(name)
            {
                JsonFormat = Newtonsoft.Json.Formatting.Indented
            };

            var source = new DynamicDictionary();

            var item1 = new List<string> { "Item1", "Item2", "Item3" };
            var item2 = new List<int> { 10, 20, 352 };
            var item3 = new List<DateTime> { DateTime.Now, DateTime.UtcNow };

            source.Storage = storage;
            source["item1"] = item1;
            source["item2"] = item2;
            source["item3"] = item3;
            await source.SaveAsync();

            Assert.IsTrue(System.IO.File.Exists(name));

            DynamicDictionary target = await storage.LoadAsync();

            Assert.AreEqual(source.Count, target.Count);
            Assert.AreEqual(source["item1"].Count, target["item1"].Count);
            Assert.AreEqual(source["item2"].Count, target["item2"].Count);
            Assert.AreEqual(source["item3"].Count, target["item3"].Count);

            Assert.AreEqual(source["item1"][0], target["item1"][0]);
            Assert.AreEqual(source["item1"][1], target["item1"][1]);
            Assert.AreEqual(source["item1"][2], target["item1"][2]);

            Assert.AreEqual(source["item2"][0], target["item2"][0]);
            Assert.AreEqual(source["item2"][1], target["item2"][1]);
            Assert.AreEqual(source["item2"][2], target["item2"][2]);

            Assert.AreEqual(source["item2"][0], target["item2"][0]);
            Assert.AreEqual(source["item2"][1], target["item2"][1]);
        }
    }
}
