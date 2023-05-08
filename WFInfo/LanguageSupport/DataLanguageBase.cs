using System;
using System.Collections.Generic;

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

        protected LocaleData _localeData;

        protected DataLanguageBase(LocaleData localeData)
		{
			if (string.IsNullOrEmpty(localeData.localeNameMarket))
			{
				Main.AddLog("Load default LocaleData (en)");
				localeData = LocaleDataBuilder.GetEnglish();
			}
			Main.AddLog(string.Concat(new string[]
			{
				"Set LocaleData (",
				localeData.localeName,
				") for '",
				base.GetType().Name,
				"'"
			}));
			_localeData = localeData;
		}

		public static DataLanguageBase CreateDefault()
		{
			return new SimpleDataLanguage(default(LocaleData));
		}

		public static DataLanguageBase CreateLocalized(string settingsLocale)
		{
			DataLanguageBase.Language type = DataLanguageBase.Language.SIMPLE;
			if (settingsLocale == "ko")
			{
				type = DataLanguageBase.Language.KOREAN;
			}
			LocaleData localeData = LocaleDataBuilder.ReadLocaleFile(settingsLocale);
			return DataLanguageBase.CreateLocalized(type, localeData);
		}

		public static DataLanguageBase CreateLocalized(DataLanguageBase.Language type, LocaleData localeData)
		{
			switch (type)
			{
			case DataLanguageBase.Language.SIMPLE:
				return new SimpleDataLanguage(localeData);
			case DataLanguageBase.Language.KOREAN:
				return new KoreanDataLanguage(localeData);
			}
			return DataLanguageBase.CreateDefault();
		}

		public virtual int CalculateLevenshteinDistance(string localizedName, string firstWord, string secondWord)
		{
			firstWord = ReplaceKeyString(firstWord, _localeData.levenshteinDistanceReplaces, "");
			secondWord = ReplaceKeyString(secondWord, _localeData.levenshteinDistanceReplaces, "");
			return CalculateLevenshteinDistanceDefault(firstWord, secondWord);
		}

        public virtual bool isItLanguage(string str)
        {
            if (_localeData.minLanguageChars == null || _localeData.minLanguageChars.Length == 0)
            {
                return true;
            }
            if (_localeData.maxLanguageChars == null || _localeData.maxLanguageChars.Length == 0)
            {
                return true;
            }
            if (_localeData.minLanguageChars.Length != _localeData.maxLanguageChars.Length)
            {
                return true;
            }

            char c = str[0];
            for (int i = 0; i < _localeData.minLanguageChars.Length; i++)
            {
                if (_localeData.minLanguageChars[i] <= (int)c && (int)c <= _localeData.maxLanguageChars[i])
                {
                    return true;
                }
            }

            return false;
        }

        public int GetMininunLenght()
		{
			return _localeData.minPartNameLenght;
		}

		public string GetMarketItemsLocale()
		{
			return _localeData.localeNameMarket;
		}

		public string GetLanguageName()
		{
			return _localeData.localeName;
		}

		public string GetOcrCharWhitelist()
		{
			return _localeData.ocrCharWhitelist;
		}

		public string GetNameWithoutBlueprint(string name, string space = " ")
		{
			return ReplaceKeyString(name, _localeData.blueprintKey, space);
		}

		public string GetNameWithBlueprint(string name, string space = " ")
		{
			string str = ParseBlueprintKey();
			return name + space + str;
		}

		public string GetNameWithPrimeBlueprint(string name, string space = " ")
		{
			string text = ParseBlueprintKey();
			return string.Concat(new string[]
			{
				name,
				space,
				_localeData.primeKey.ToLower(Main.culture),
				space,
				text
			});
		}

		public string GetSetName(string name)
		{
			string text = name.ToLower(Main.culture);
			text = ReplaceKeyString(text, _localeData.lowerLimbKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.upperLimbKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.neuropticsKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.chassisKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.systemsKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.carapaceKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.cerebrumKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.blueprintKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.harnessKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.bladeKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.pouchKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.headKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.barrelKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.receiverKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.stockKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.discKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.gripKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.stringKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.handleKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.ornamentKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.wingsKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.bladesKey.ToLower(Main.culture), "");
			text = ReplaceKeyString(text, _localeData.hiltKey.ToLower(Main.culture), "");
			text = text.TrimEnd(Array.Empty<char>());
			return Main.culture.TextInfo.ToTitleCase(text);
		}

        public bool IsNameDataIgnored(string value)
		{
			if (CheckIgnoredRule(value, _localeData.ignoredForma)
			 || CheckIgnoredRule(value, _localeData.ignoredExsilusWeaponAdapter)
			 || CheckIgnoredRule(value, _localeData.ignoredKuva)
			 || CheckIgnoredRule(value, _localeData.ignoredRivenSliver)
			 || CheckIgnoredRule(value, _localeData.ignoredAyatanAmberStar))
			{
				return true;
			}

			if (_localeData.ignoredAdditional != null)
			{
				for (int i = 0; i < _localeData.ignoredAdditional.Length; i++)
				{
					if (CheckIgnoredRule(value, _localeData.ignoredAdditional[i]))
					{
						return true;
					}
				}
			}

			return false;
		}

        public bool CheckArchwingOrWarframesBlueprintKeys(string name)
        {
            return name.Contains(_localeData.neuropticsKey)
				|| name.Contains(_localeData.chassisKey)
				|| name.Contains(_localeData.systemsKey)
				|| name.Contains(_localeData.harnessKey)
				|| name.Contains(_localeData.wingsKey);
        }

        public bool CheckContainsWarframesBlueprintKeys(string name)
        {
            return name.Contains(_localeData.neuropticsKey)
				|| name.Contains(_localeData.chassisKey)
				|| name.Contains(_localeData.systemsKey);
        }

        public bool CheckContainsArchwingBlueprintKeys(string name)
        {
            return name.Contains(_localeData.systemsKey)
				|| name.Contains(_localeData.harnessKey)
				|| name.Contains(_localeData.wingsKey);
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
			if (rule.Contains("|"))
			{
				string[] array = rule.Split(new char[] { '|' });
				for (int i = 0; i < array.Length; i++)
				{
					text = text.Replace(space + array[i], "");
				}
			}
			else
			{
				text = text.Replace(space + rule, "");
			}

			return text;
		}

		protected int CalculateLevenshteinDistanceDefault(string s, string t)
		{
			s = s.ToLower(Main.culture);
			t = t.ToLower(Main.culture);
			int length = s.Length;
			int length2 = t.Length;
			int[,] array = new int[length + 1, length2 + 1];

			if (length == 0 || length2 == 0)
			{
				return length + length2;
			}

			array[0, 0] = 0;
			int num = 0;
			for (int i = 1; i <= length; i++)
			{
				array[i, 0] = ((s[i - 1] == ' ') ? num : (++num));
			}
			num = 0;
			for (int j = 1; j <= length2; j++)
			{
				array[0, j] = ((t[j - 1] == ' ') ? num : (++num));
			}
			for (int k = 1; k <= length; k++)
			{
				for (int l = 1; l <= length2; l++)
				{
					int num2 = array[k - 1, l];
					if (s[k - 1] != ' ')
					{
						num2++;
					}
					int num3 = array[k, l - 1];
					if (t[l - 1] != ' ')
					{
						num3++;
					}
					int num4 = array[k - 1, l - 1];
					if (t[l - 1] != s[k - 1])
					{
						num4++;
					}
					array[k, l] = Math.Min(Math.Min(num2, num3), num4);
				}
			}
			return array[length, length2];
		}

        private bool CheckIgnoredRule(string value, string[] ignoredRule)
        {
            if (ignoredRule == null || ignoredRule.Length < 1)
            {
                return false;
            }
            string[] array = ignoredRule[0].Split(new char[]
            {
                ' '
            });
            return array.Length >= 1 && value.ToLower(Main.culture).Contains(array[0].ToLower(Main.culture));
        }

        private string ParseBlueprintKey()
		{
			if (!_localeData.blueprintKey.Contains("|"))
			{
				return _localeData.blueprintKey;
			}
			return _localeData.blueprintKey.Split(new char[] { '|' })[0];
		}
	}
}
