using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dynamic;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class DynamicDictionaryTests
    {
        [TestMethod]
        public void DynamicDictionary_StrongTypedTests()
        {
            DynamicDictionary artist = new DynamicDictionary();

            string mainName = "Pink Floyd";
            List<string> aliases = new List<string>
            {
                "The Pink Floyd",
                "Floyd",
                "핑크 플로이드",
            };

            string popularAlbum = "The Dark Side of the Moon";
            List<string> Albums = new List<string>
            {
                "Wish You Were Here",
                "Meddle",
                "Atom Heart Mother",
                "Animals",
                "The Wall"
            };

            artist["Name"] = mainName;
            artist["Name"].AddRange(aliases);

            artist["Album"] = Albums;
            artist["Album"].Value = popularAlbum;

            Assert.AreEqual<string>(mainName, artist["name"]);
            Assert.AreEqual(aliases.Count + 1, artist["name"].Count);

            Assert.AreEqual<string>(popularAlbum, artist["album"][0]);
            Assert.AreEqual(Albums.Count + 1, artist["album"].Count);

            artist["album"] -= popularAlbum;
            Assert.AreEqual<string>(Albums[0], artist["album"][0]);
        }

        [TestMethod]
        public void DynamicDictionary_DynamicTypedTests()
        {
            dynamic artist = new DynamicDictionary();

            string mainName = "Pink Floyd";
            List<string> aliases = new List<string>
            {
                "The Pink Floyd",
                "Floyd",
                "핑크 플로이드",
            };

            string popularAlbum = "The Dark Side of the Moon";
            List<string> Albums = new List<string>
            {
                "Wish You Were Here",
                "Meddle",
                "Atom Heart Mother",
                "Animals",
                "The Wall"
            };

            artist.name = mainName;
            artist.name.AddRange(aliases);

            artist.album = Albums;
            artist.album.Value = popularAlbum;

            Assert.AreEqual<string>(mainName, artist.name);
            Assert.AreEqual(aliases.Count + 1, artist.name.Count);

            Assert.AreEqual<string>(popularAlbum, artist.album[0]);
            Assert.AreEqual(Albums.Count + 1, artist.album.Count);

            artist["album"] -= popularAlbum;
            Assert.AreEqual<string>(Albums[0], artist.album[0]);
        }

        [TestMethod]
        public void DynamicDictionary_EventsTests()
        {
            DynamicDictionary d = new DynamicDictionary();

            int countAdd = 0;
            int countRemove = 0;
            int countChanged = 0;
            int countClear = 0;
            int countOther = 0;

            d.OnChange += (sender, e) =>
            {
                switch (e.EventType)
                {
                    case DynamicDictionaryChangedType.AddedValue:
                        countAdd++;
                        break;

                    case DynamicDictionaryChangedType.RemovedValue:
                        countRemove++;
                        break;

                    case DynamicDictionaryChangedType.ChangedValue:
                        countChanged++;
                        break;

                    case DynamicDictionaryChangedType.Clear:
                        countClear++;
                        break;

                    default:
                        countOther++;
                        break;
                }
            };

            d["1"] = "Main";
            Assert.AreEqual(1, countAdd);

            d["1"] = "Main";
            Assert.AreEqual(0, countChanged);

            d["2"] = "New";
            Assert.AreEqual(2, countAdd);

            d["2"] = "Test";
            Assert.AreEqual(1, countChanged);

            d.Add("3", "Name");
            Assert.AreEqual(3, countAdd);

            d.Remove("3");
            Assert.AreEqual(1, countRemove);

            d.Clear();
            Assert.AreEqual(1, countClear);
            Assert.AreEqual(0, countOther);
        }
    }
}
