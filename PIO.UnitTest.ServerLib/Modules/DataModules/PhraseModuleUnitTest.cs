using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Models;
using PIO.ModulesLib.Exceptions;
using PIO.ServerLib.Modules;
using PIO.UnitTest.ServerLib.Mocks;

namespace PIO.UnitTest.ServerLib.Modules
{
	[TestClass]
	public class PhraseModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetPhrase()
		{
			MockedDatabase<Phrase> database;
			PhraseModule module;
			Phrase result;

			database = new MockedDatabase<Phrase>(false, 1, (t) => new Phrase() { Key = t.ToString(),CountryCode="EN" });
			module = new PhraseModule(NullLogger.Instance, database);
			result = module.GetPhrase("1","EN");
			Assert.IsNotNull(result);
			Assert.AreEqual("0", result.Key);
		}
		[TestMethod]
		public void ShouldGetPhrases()
		{
			MockedDatabase<Phrase> database;
			PhraseModule module;
			Phrase[] results;

			database = new MockedDatabase<Phrase>(false, 3, (t) => new Phrase() { Key = t.ToString(), CountryCode = "EN" });
			module = new PhraseModule(NullLogger.Instance, database);
			results = module.GetPhrases("EN");
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t.ToString(), results[t].Key);
			}
		}
		[TestMethod]
		public void ShouldNotGetPhraseAndLogError()
		{
			MockedDatabase<Phrase> database;
			PhraseModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Phrase>(true,1, (t) => new Phrase() { Key = t.ToString(), CountryCode = "EN" });
			module = new PhraseModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetPhrase("1", "EN"));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetPhrasesAndLogError()
		{
			MockedDatabase<Phrase> database;
			PhraseModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Phrase>(true, 3, (t) => new Phrase() { Key = t.ToString(), CountryCode = "EN" });
			module = new PhraseModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetPhrases("EN"));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}

		[TestMethod]
		public void ShouldCreatePhrase()
		{
			MockedDatabase<Phrase> database;
			PhraseModule module;
			Phrase result;

			database = new MockedDatabase<Phrase>(false, 1, (t) => new Phrase() { Key = t.ToString(), CountryCode = "EN" });
			module = new PhraseModule(NullLogger.Instance, database);
			result = module.CreatePhrase("1", "EN","New phrase");
			Assert.IsNotNull(result);
			Assert.AreEqual(Phrase.GenerateID("1", "EN"), result.PhraseID);
			Assert.AreEqual("1", result.Key);
			Assert.AreEqual(1, database.InsertedCount);
		}
		[TestMethod]
		public void ShouldNotCreatePhraseAndLogError()
		{
			MockedDatabase<Phrase> database;
			PhraseModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Phrase>(true, 1, (t) => new Phrase() { Key = t.ToString(), CountryCode = "EN" });
			module = new PhraseModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.CreatePhrase("1", "EN", "New phrase"));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
			Assert.AreEqual(0, database.InsertedCount);
		}


	}
}
