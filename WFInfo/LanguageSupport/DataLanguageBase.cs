using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace WFInfo.LanguageSupport
{
	internal abstract class DataLanguageBase
	{
        public enum Language
        {
            DEFAULT,
            SIMPLE,
            KOREAN,
        }

        protected LocaleData localeData;
		protected CultureInfo culture => Main.culture;
		protected char itemsDelimiter => Data.ITEMS_SEPARATOR_CHAR;

        protected DataLanguageBase(LocaleData localeData)
		{
			if (string.IsNullOrEmpty(localeData.localeNameMarket))
			{
				Main.AddLog("Load default LocaleData (en)");
				localeData = LocaleDataBuilder.GetEnglish();
			}
			Main.AddLog("Set LocaleData ("
					  + localeData.localeName
					  + ") for '"
					  + GetType().Name
					  + "'"
			);
			this.localeData = localeData;
		}

		public static DataLanguageBase CreateDefault()
		{
			return new SimpleDataLanguage(default);
		}

		public static DataLanguageBase CreateLocalized(string settingsLocale)
		{
			Language type = Language.SIMPLE;
			if (settingsLocale == "ko")
			{
				type = Language.KOREAN;
			}
			LocaleData localeData = LocaleDataBuilder.ReadLocaleData(settingsLocale);
			return CreateLocalized(type, localeData);
		}

		public static DataLanguageBase CreateLocalized(Language type, LocaleData localeData)
		{
			switch (type)
			{
			case Language.SIMPLE:
				return new SimpleDataLanguage(localeData);
			case Language.KOREAN:
				return new KoreanDataLanguage(localeData);
			}
			return CreateDefault();
		}

		public virtual int CalculateLevenshteinDistance(string localizedName, string firstWord, string secondWord)
		{
			firstWord = ReplaceKeyString(firstWord, localeData.levenshteinDistanceReplaces, "");
			secondWord = ReplaceKeyString(secondWord, localeData.levenshteinDistanceReplaces, "");
			return CalculateLevenshteinDistanceDefault(firstWord, secondWord);
		}

        public virtual bool isItLanguage(string str)
        {
			LocaleData.MinMaxLocaleChars[] minMaxChars = localeData.minMaxLanguageChars;

            if (minMaxChars == null || minMaxChars.Length == 0)
            {
                return true;
            }

            char c = str[0];
            for (int i = 0; i < minMaxChars.Length; i++)
            {
                if (minMaxChars[i].min <= (int)c && (int)c <= minMaxChars[i].max)
                {
                    return true;
                }
            }

            return false;
        }

		public virtual bool PartNameValid(string name)
		{
			return name.Length >= localeData.minPartNameLenght;
		}

		public string GetMarketItemsLocale()
		{
			return localeData.localeNameMarket;
		}

		public string GetLanguageName()
		{
			return localeData.localeName;
		}

		public string GetOcrCharWhitelist()
		{
			return localeData.ocrCharWhitelist;
		}

		public string GetNameWithoutBlueprint(string name, string space = " ")
		{
			return ReplaceKeyString(name, localeData.blueprintKey, space);
		}

		public string GetNameWithBlueprint(string name, string space = " ")
		{
			if (string.IsNullOrEmpty(localeData.blueprintFormat))
			{
                string str = localeData.blueprintKey.Split(itemsDelimiter)[0];
                return name + space + str;
			}

			return string.Format(localeData.blueprintFormat, name);
		}

		public string GetNameWithPrimeBlueprint(string name, string space = " ")
		{
            if (string.IsNullOrEmpty(localeData.primeBlueprintFormat))
			{
                string blp = localeData.blueprintKey.Split(itemsDelimiter)[0];
                string prime = localeData.primeKey.Split(itemsDelimiter)[0];

                return string.Concat(new string[]
                {
                name,
                space,
                prime.ToLower(culture),
                space,
                blp
                });
            }

            return string.Format(localeData.primeBlueprintFormat, name);
        }

		public string GetSetName(string name)
		{
			string result = name.ToLower(culture);

			if (result.Contains(localeData.kavasaKey))
			{
				return localeData.kavasaSetName;
            }

			result = ReplaceKeyString(result, localeData.lowerLimbKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.upperLimbKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.neuropticsKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.chassisKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.systemsKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.carapaceKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.cerebrumKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.blueprintKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.harnessKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.bladeKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.pouchKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.headKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.barrelKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.receiverKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.stockKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.discKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.gripKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.stringKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.handleKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.ornamentKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.wingsKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.bladesKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.hiltKey.ToLower(culture), "");
			result = ReplaceKeyString(result, localeData.linkKey.ToLower(culture), "");

            result += " " + localeData.setKey;

            result = result.TrimEnd(Array.Empty<char>());
			return culture.TextInfo.ToTitleCase(result);
		}

        public bool IsNameDataIgnored(string value)
		{
			if (CheckIgnoredRule(value, localeData.ignoredForma)
			 || CheckIgnoredRule(value, localeData.ignoredExsilusWeaponAdapter)
			 || CheckIgnoredRule(value, localeData.ignoredKuva)
			 || CheckIgnoredRule(value, localeData.ignoredRivenSliver)
			 || CheckIgnoredRule(value, localeData.ignoredAyatanAmberStar))
			{
				return true;
			}

			if (localeData.ignoredAdditional != null)
			{
				for (int i = 0; i < localeData.ignoredAdditional.Length; i++)
				{
					if (CheckIgnoredRule(value, localeData.ignoredAdditional[i]))
					{
						return true;
					}
				}
			}

			return false;
		}

        public bool CheckArchwingOrWarframesBlueprintKeys(string name)
        {
            return name.Contains(localeData.neuropticsKey)
				|| name.Contains(localeData.chassisKey)
				|| name.Contains(localeData.systemsKey)
				|| name.Contains(localeData.harnessKey)
				|| name.Contains(localeData.wingsKey);
        }

        public bool CheckContainsWarframesBlueprintKeys(string name)
        {
            return name.Contains(localeData.neuropticsKey)
				|| name.Contains(localeData.chassisKey)
				|| name.Contains(localeData.systemsKey);
        }

        public bool CheckContainsArchwingBlueprintKeys(string name)
        {
            return name.Contains(localeData.systemsKey)
				|| name.Contains(localeData.harnessKey)
				|| name.Contains(localeData.wingsKey);
        }

        protected bool GroupEquals(Dictionary<int, List<int>> group, int ak, int bk)
        {
            foreach (KeyValuePair<int, List<int>> keyValuePair in group)
            {
                if (keyValuePair.Value.Contains(ak) && keyValuePair.Value.Contains(bk))
                {
                    return true;
                }
            }
            return false;
        }

        protected string ReplaceKeyString(string s, string rule, string space = "")
		{
			if (string.IsNullOrEmpty(rule) || string.IsNullOrEmpty(s))
			{
				return s;
			}

			string text = s;
			string[] ruleSplit = rule.Split(itemsDelimiter);
			for (int i = 0; i < ruleSplit.Length; i++)
            {
                text = text.Replace(space + ruleSplit[i], "");
            }

			return text;
		}

		protected int CalculateLevenshteinDistanceDefault(string s, string t)
		{
            // Levenshtein Distance determines how many character changes it takes to form a known result
            // For example: Nuvo Prime is closer to Nova Prime (2) then Ash Prime (4)
            // For more info see: https://en.wikipedia.org/wiki/Levenshtein_distance
            s = s.ToLower(culture);
            t = t.ToLower(culture);
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0 || m == 0)
                return n + m;

            d[0, 0] = 0;

            int count = 0;
            for (int i = 1; i <= n; i++)
                d[i, 0] = (s[i - 1] == ' ' ? count : ++count);

            count = 0;
            for (int j = 1; j <= m; j++)
                d[0, j] = (t[j - 1] == ' ' ? count : ++count);

            for (int i = 1; i <= n; i++)
                for (int j = 1; j <= m; j++)
                {
                    // deletion of s
                    int opt1 = d[i - 1, j];
                    if (s[i - 1] != ' ')
                        opt1++;

                    // deletion of t
                    int opt2 = d[i, j - 1];
                    if (t[j - 1] != ' ')
                        opt2++;

                    // swapping s to t
                    int opt3 = d[i - 1, j - 1];
                    if (t[j - 1] != s[i - 1])
                        opt3++;
                    d[i, j] = Math.Min(Math.Min(opt1, opt2), opt3);
                }



            return d[n, m];
        }

        private bool CheckIgnoredRule(string value, string[] ignoredRule)
        {
            if (ignoredRule == null || ignoredRule.Length < 1)
            {
                return false;
            }
            string[] array = ignoredRule[0].Split(' ');
            return array.Length >= 1 && value.ToLower(culture).Contains(array[0].ToLower(culture));
        }
	}
}
