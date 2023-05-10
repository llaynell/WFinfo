using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WFInfo.LanguageSupport
{
	internal static class LocaleDataBuilder
	{
		private const string LOCALE_FILE_FROMAT = "{0}-locale.json";

        private static string _dirPath
		{
			get
			{
				return CustomEntrypoint.appdata_customlocales_folder;
			}
		}

		private static LocaleData[] _defaultLocales = new LocaleData[]
		{
			GetEnglish(),
			GetKorean(),
			GetRussian(),
		};


        public static void InitializeCustomLocaleDir()
        {
            if (!Directory.Exists(_dirPath))
            {
                Directory.CreateDirectory(_dirPath);
            }
        }

        //public static void CreateCustomLocalesFromDefault()
        //{
        //    foreach (var localeData in _defaultLocales)
        //    {
        //        string localeKey = localeData.localeNameMarket;
        //        string localeJsonPath = GetLocaleJsonPath(localeKey);
        //        if (!File.Exists(localeJsonPath))
        //        {
        //            Main.AddLog("Create localeData for '"
        //                      + localeKey
        //                      + "' at path '"
        //                      + localeJsonPath
        //                      + "'"
        //            );
        //            string contents = JsonConvert.SerializeObject(localeData, Formatting.Indented);
        //            File.WriteAllText(localeJsonPath, contents);
        //        }
        //    }
        //}

        public static string GetLocaleJsonPath(string localeKey)
		{
			return Path.Combine(_dirPath, string.Format(LOCALE_FILE_FROMAT, localeKey));
		}

		public static string ReadRegexSymbols(string settingsLocale)
		{
			string dest = "";
			dest = AddRegexSymbols(GetEnglish().regexSymbols, dest);
			return AddRegexSymbols(ReadLocaleData(settingsLocale).regexSymbols, dest);
		}

		public static string ReadAllRegexSymbols()
		{
			string result = "";

			LocaleData[] customLocales = FindCustomLocales();
			for (int i = 0; i < customLocales.Length; i++)
			{
				result = AddRegexSymbols(customLocales[i].regexSymbols, result);
			}

			return result;
		}

        public static LocaleData ReadLocaleData(string localeKey)
		{
			for (int i = 0; i < _defaultLocales.Length; i++)
			{
				LocaleData check = _defaultLocales[i];
                if (localeKey == check.localeNameMarket)
                {
                    return check;
                }
            }

            string localeJsonPath = GetLocaleJsonPath(localeKey);
            int overridedFedaultI = Array.FindIndex(_defaultLocales, check =>
                localeKey == check.localeNameMarket
            );
			if (!File.Exists(localeJsonPath) && overridedFedaultI == -1)
			{
				Main.AddLog("Cant find file at path '" + localeJsonPath + "', load english");
				return GetEnglish();
			}

			return JsonConvert.DeserializeObject<LocaleData>(File.ReadAllText(localeJsonPath));
		}

        public static LocaleDataInfo[] FindLocaleInfos()
        {
            LocaleData[] locales = FindAllLocales();
            LocaleDataInfo[] result = new LocaleDataInfo[locales.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = locales[i].GetLocaleInfo();
            }
            return result;
        }

        private static LocaleData[] FindAllLocales()
        {
            List<LocaleData> result = new List<LocaleData>();

            result.AddRange(_defaultLocales);

            LocaleData[] customLocales = FindCustomLocales();
            for (int i = 0; i < customLocales.Length; i++)
            {
                LocaleData localeData = customLocales[i];

                int overridedFedaultI = Array.FindIndex(_defaultLocales, check =>
                    localeData.localeNameMarket == check.localeNameMarket
                );

                if (!string.IsNullOrEmpty(localeData.localeNameMarket) && overridedFedaultI == -1)
                {
                    result.Add(localeData);
                    Main.AddLog("Founded custom locale for '" + localeData.localeName + "'");
                }
                else
                {
                    result[overridedFedaultI] = _defaultLocales[overridedFedaultI];
                    Main.AddLog("Override default locale for '" + localeData.localeName + "' from custom locale");
                }
            }

            return result.ToArray();
        }

        private static LocaleData[] FindCustomLocales()
        {
            List<LocaleData> result = new List<LocaleData>();

            string[] files = Directory.GetFiles(_dirPath);
            for (int i = 0; i < files.Length; i++)
            {
                LocaleData localeData = default;

                try
                {
                    string text = File.ReadAllText(files[i]);
                    localeData = JsonConvert.DeserializeObject<LocaleData>(text);
                }
                catch
                {
                    Main.AddLog("FAILED read localeData for '" + files[i] + "'!");
                    continue;
                }

                result.Add(localeData);
            }

            return result.ToArray();
        }

        private static string AddRegexSymbols(string symbols, string dest)
        {
            if (symbols.Contains("|"))
            {
                Array.ForEach(symbols.Split(Data.ITEMS_SEPARATOR_CHAR), s =>
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

        #region LocaleData Init

        public static LocaleData GetEnglish()
		{
            return new LocaleData
            {
                localeName = "English",
                localeNameMarket = "en",

                trainedDataChecksum = "7af2ad02d11702c7092a5f8dd044d52f",
                regexSymbols = "a-z",
                ocrCharWhitelist = " ABCDEFGHIJKLMNOPQRSTUVWXYZ&",
                levenshteinDistanceReplaces = "",

                minMaxLanguageChars = null,

                minPartNameLenght = 13,

                ignoredForma = new string[]
                {
                    "Forma Blueprint",
                },
                ignoredExsilusWeaponAdapter = new string[]
                {
                    "Exilus Weapon Adapter Blueprint",
                },
                ignoredKuva = new string[]
                {
                    "Kuva",
                },
                ignoredRivenSliver = new string[]
                {
                    "Riven Sliver",
                },
                ignoredAyatanAmberStar = new string[]
                {
                    "Ayatan Amber Star",
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
                starsKey = "stars",

                kavasaKey = "kavasa",
                kavasaSetName = "Kavasa Prime Kubrow Collar Set",
                setKey = "Set",

                blueprintFormat = "{0} Blueprint",
                primeBlueprintFormat = "{0} prime Blueprint",
        };
		}

        public static LocaleData GetKorean()
        {
            return new LocaleData
            {
                localeName = "한국어",
                localeNameMarket = "ko",

                trainedDataChecksum = "c776744205668b7e76b190cc648765da",
                regexSymbols = "가-힣",
                ocrCharWhitelist = " ABCDEFGHIJKLMNOPQRSTUVWXYZ&",
                levenshteinDistanceReplaces = "설계도| ",

                minMaxLanguageChars = new LocaleData.MinMaxLocaleChars[]
                {
                    new LocaleData.MinMaxLocaleChars(4352, 4607),
                    new LocaleData.MinMaxLocaleChars(12592, 12687),
                    new LocaleData.MinMaxLocaleChars(44032, 55203),
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
                starsKey = "stars",

                kavasaKey = "kavasa",
                kavasaSetName = "Kavasa Prime Kubrow Collar Set",
                setKey = "Set",

                blueprintFormat = "{0} Blueprint",
                primeBlueprintFormat = "{0} prime Blueprint",
            };
        }

        public static LocaleData GetRussian()
        {
            return new LocaleData
            {
                localeName = "Russian",
                localeNameMarket = "ru",

                trainedDataChecksum = "2e2022eddce032b754300a8188b41419",
                regexSymbols = "а-я",
                ocrCharWhitelist = " ABCDEFGHIJKLMNOPQRSTUVWXYZ&АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ",
                levenshteinDistanceReplaces = "(Чертеж)|(Чертёж)|Чертеж|Чертёж|Чертж|:",

                minMaxLanguageChars = new LocaleData.MinMaxLocaleChars[]
                {
                    new LocaleData.MinMaxLocaleChars(0x0400, 0x045F),
                },

                minPartNameLenght = 13,

                ignoredForma = new string[]
                {
                    "Forma Blueprint",
                    "Форма",
                },
                ignoredExsilusWeaponAdapter = new string[]
				{
                    "Exilus Weapon Adapter Blueprint",
					"Адаптер Эксилус Для оружия",
                },
                ignoredKuva = new string[]
				{
                    "Kuva",
					"Кува",
                },
                ignoredRivenSliver = new string[]
				{
                    "Riven Sliver",
                    "Осколок Разлома",
                },
                ignoredAyatanAmberStar = new string[]
				{
                    "Ayatan Amber Star",
					"Звезда аятан",
                },
                ignoredAdditional = null,

                primeKey = "Прайм",
                blueprintKey = "Чертеж|Чертёж|Чертж",
                neuropticsKey = "Нейрооптика",
                chassisKey = "Каркас",
                systemsKey = "Система",
                harnessKey = "Упряж",
                wingsKey = "Крылья",
                lowerLimbKey = "Нижнее плечо",
                upperLimbKey = "Верхнее плечо",
                carapaceKey = "Панцирь",
                cerebrumKey = "Мозг",
                bladeKey = "Лезвие",
                pouchKey = "Кисет",
                headKey = "Голова",
                barrelKey = "Ствол",
                receiverKey = "Приемник|Приёмник|Примник",
                stockKey = "Приклад",
                discKey = "Диск",
                gripKey = "Рукоять",
                stringKey = "string",
                handleKey = "Рукоять",
                ornamentKey = "Орнамент",
                bladesKey = "Лезвия",
                hiltKey = "Рукоятка",
                starsKey = "Сюрикены",

                kavasaKey = "каваса",
                kavasaSetName = "Ошейник Каваса Прайм Set",
                setKey = "Set",

                blueprintFormat = "Чертеж: {0}",
                primeBlueprintFormat = "Чертеж: {0} прайм",
            };
        }

        #endregion
    }
}
