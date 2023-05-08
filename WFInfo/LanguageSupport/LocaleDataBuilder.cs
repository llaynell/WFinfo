using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WFInfo.LanguageSupport
{
	internal static class LocaleDataBuilder
	{
		private static string dirPath
		{
			get
			{
				return CustomEntrypoint.appdata_customlocales_folder;
			}
		}

		public static string GetLocaleJsonPath(string locale)
		{
			return Path.Combine(dirPath, locale + "-locale.json");
		}

		public static string ReadRegexSymbols(string settingsLocale)
		{
			string dest = "";
			dest = AddRegexSymbols(GetEnglish().regexSymbols, dest);
			return AddRegexSymbols(ReadLocaleFile(settingsLocale).regexSymbols, dest);
		}

		public static string ReadAllRegexSymbols()
		{
			string[][] array = FindLocales();
			string text = "";
			text = AddRegexSymbols(GetEnglish().regexSymbols, text);
			text = AddRegexSymbols(GetKorean().regexSymbols, text);
			int length = array.GetLength(0);
			for (int i = 0; i < length; i++)
			{
				text = AddRegexSymbols(ReadLocaleFile(array[i][0]).regexSymbols, text);
			}
			return text;
		}

		private static string AddRegexSymbols(string symbols, string dest)
		{
			if (symbols.Contains("|"))
			{
				Array.ForEach<string>(symbols.Split(new char[]
				{
					'|'
				}), delegate(string s)
				{
					if (!dest.Contains(s))
					{
						dest += s;
					}
				});
			}
			else
			{
				dest += symbols;
			}
			return dest;
		}

		public static string[][] FindLocales()
		{
			string[] files = Directory.GetFiles(dirPath);
			List<string[]> list = new List<string[]>();
			for (int i = 0; i < files.Length; i++)
			{
				string text = File.ReadAllText(files[i]);
				LocaleData localeData = default(LocaleData);
				try
				{
					localeData = JsonConvert.DeserializeObject<LocaleData>(text);
				}
				catch
				{
					Main.AddLog("FAILED read localeData for '" + files[i] + "'!");
				}
				if (!string.IsNullOrEmpty(localeData.localeNameMarket)
				 && !(localeData.localeNameMarket == GetEnglish().localeNameMarket)
				 && !(localeData.localeNameMarket == GetKorean().localeNameMarket))
				{
					list.Add(new string[]
					{
						localeData.localeNameMarket,
						localeData.traineddataChecksum,
						localeData.localeName
					});
					Main.AddLog("Founded custom locale for '" + localeData.localeName + "'");
				}
			}
			return list.ToArray();
		}

		public static LocaleData ReadLocaleFile(string locale)
		{
			if (locale == GetEnglish().localeNameMarket)
			{
				return GetEnglish();
			}
			if (locale == GetKorean().localeNameMarket)
			{
				return GetKorean();
			}
			string localeJsonPath = GetLocaleJsonPath(locale);
			if (!File.Exists(localeJsonPath))
			{
				Main.AddLog("Cant find file at path '" + localeJsonPath + "', load english");
				return GetEnglish();
			}
			return JsonConvert.DeserializeObject<LocaleData>(File.ReadAllText(localeJsonPath));
		}

		public static void BuildDefault()
		{
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}
			foreach (LocaleData localeData in new LocaleData[0])
			{
				string localeJsonPath = GetLocaleJsonPath(localeData.localeNameMarket);
				if (!File.Exists(localeJsonPath))
				{
					Main.AddLog(string.Concat(new string[]
					{
						"Create localeData for '",
						localeData.localeNameMarket,
						"' at path '",
						localeJsonPath,
						"'"
					}));
					string contents = JsonConvert.SerializeObject(localeData, Formatting.Indented);
					File.WriteAllText(localeJsonPath, contents);
				}
			}
		}

		public static LocaleData GetEnglish()
		{
			return new LocaleData
			{
				localeName = "English",
				localeNameMarket = "en",
				traineddataChecksum = "7af2ad02d11702c7092a5f8dd044d52f",
				regexSymbols = "a-z",
				ocrCharWhitelist = " ABCDEFGHIJKLMNOPQRSTUVWXYZ&",
				levenshteinDistanceReplaces = "",
				minLanguageChars = null,
				maxLanguageChars = null,
				minPartNameLenght = 13,
				ignoredForma = new string[]
				{
					"Forma Blueprint"
				},
				ignoredExsilusWeaponAdapter = new string[]
				{
					"Exilus Weapon Adapter Blueprint"
				},
				ignoredKuva = new string[]
				{
					"Kuva"
				},
				ignoredRivenSliver = new string[]
				{
					"Riven Sliver"
				},
				ignoredAyatanAmberStar = new string[]
				{
					"Ayatan Amber Star"
				},
				ignoredAdditional = null,
				primeKey = "Prime",
				blueprintKey = "Blueprint",
				neuropticsKey = "Neuroptics",
				chassisKey = "Chassis",
				systemsKey = "Systems",
				harnessKey = "Harness",
				wingsKey = "Wings",
				lowerLimbKey = "lower limb",
				upperLimbKey = "upper limb",
				carapaceKey = "carapace",
				cerebrumKey = "cerebrum",
				bladeKey = "blade",
				pouchKey = "pouch",
				headKey = "head",
				barrelKey = "barrel",
				receiverKey = "receiver",
				stockKey = "stock",
				discKey = "disc",
				gripKey = "grip",
				stringKey = "string",
				handleKey = "handle",
				ornamentKey = "ornament",
				bladesKey = "blades",
				hiltKey = "hilt",
				starsKey = "stars"
			};
		}

		public static LocaleData GetKorean()
		{
			return new LocaleData
			{
				localeName = "한국어",
				localeNameMarket = "ko",
				traineddataChecksum = "c776744205668b7e76b190cc648765da",
				regexSymbols = "가-힣",
				ocrCharWhitelist = " ABCDEFGHIJKLMNOPQRSTUVWXYZ&",
				levenshteinDistanceReplaces = "설계도| ",
				minLanguageChars = new int[]
				{
					4352,
					12592,
					44032
				},
				maxLanguageChars = new int[]
				{
					4607,
					12687,
					55203
				},
				minPartNameLenght = 6,
				ignoredForma = null,
				ignoredExsilusWeaponAdapter = null,
				ignoredKuva = null,
				ignoredRivenSliver = null,
				ignoredAyatanAmberStar = null,
				ignoredAdditional = null,
				primeKey = "Prime",
				blueprintKey = "Blueprint",
				neuropticsKey = "Neuroptics",
				chassisKey = "Chassis",
				systemsKey = "Systems",
				harnessKey = "Harness",
				wingsKey = "Wings",
				lowerLimbKey = "lower limb",
				upperLimbKey = "upper limb",
				carapaceKey = "carapace",
				cerebrumKey = "cerebrum",
				bladeKey = "blade",
				pouchKey = "pouch",
				headKey = "head",
				barrelKey = "barrel",
				receiverKey = "receiver",
				stockKey = "stock",
				discKey = "disc",
				gripKey = "grip",
				stringKey = "string",
				handleKey = "handle",
				ornamentKey = "ornament",
				bladesKey = "blades",
				hiltKey = "hilt",
				starsKey = "stars"
			};
		}
	}
}
