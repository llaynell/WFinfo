using System;

namespace WFInfo.LanguageSupport
{
	internal class SimpleDataLanguage : DataLanguageBase
	{
		internal SimpleDataLanguage(LocaleData localeData) : base(localeData)
		{
		}

		public override int CalculateLevenshteinDistance(string localizedName, string firstWord, string secondWord)
		{
			string text = ReplaceIfIgnored(firstWord);
			firstWord = ((text != firstWord) ? text : localizedName);
			firstWord = ReplaceKeyString(firstWord, _localeData.levenshteinDistanceReplaces, "").Trim();
			secondWord = ReplaceKeyString(secondWord, _localeData.levenshteinDistanceReplaces, "").Trim();
			return CalculateLevenshteinDistanceDefault(firstWord, secondWord);
		}

		protected string ReplaceIfIgnored(string word)
		{
			word = ReplaceIfIgnoredMatch(word, _localeData.ignoredForma);
			word = ReplaceIfIgnoredMatch(word, _localeData.ignoredExsilusWeaponAdapter);
			word = ReplaceIfIgnoredMatch(word, _localeData.ignoredKuva);
			word = ReplaceIfIgnoredMatch(word, _localeData.ignoredRivenSliver);
			word = ReplaceIfIgnoredMatch(word, _localeData.ignoredAyatanAmberStar);
			if (_localeData.ignoredAdditional != null)
			{
				for (int i = 0; i < _localeData.ignoredAdditional.Length; i++)
				{
					word = ReplaceIfIgnoredMatch(word, _localeData.ignoredAdditional[i]);
				}
			}
			return word;
		}

		protected string ReplaceIfIgnoredMatch(string word, string[] ignoredRule)
		{
			if (ignoredRule == null || ignoredRule.Length != 2)
			{
				return word;
			}
			string b = ignoredRule[0];
			string result = ignoredRule[1];
			if (word == b)
			{
				return result;
			}
			return word;
		}
	}
}
