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
    }
}
