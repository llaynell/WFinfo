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
			firstWord = ReplaceKeyString(firstWord, localeData.levenshteinDistanceReplaces, "").Trim();
			secondWord = ReplaceKeyString(secondWord, localeData.levenshteinDistanceReplaces, "").Trim();
			return CalculateLevenshteinDistanceDefault(firstWord, secondWord);
		}

        //public override bool PartNameValid(string name)
        //{
        //    return base.PartNameValid(name.Replace(" ", ""));
        //}

        protected string ReplaceIfIgnored(string word)
		{
			word = ReplaceIfIgnoredMatch(word, localeData.ignoredForma);
			word = ReplaceIfIgnoredMatch(word, localeData.ignoredExsilusWeaponAdapter);
			word = ReplaceIfIgnoredMatch(word, localeData.ignoredKuva);
			word = ReplaceIfIgnoredMatch(word, localeData.ignoredRivenSliver);
			word = ReplaceIfIgnoredMatch(word, localeData.ignoredAyatanAmberStar);
			if (localeData.ignoredAdditional != null)
			{
				for (int i = 0; i < localeData.ignoredAdditional.Length; i++)
				{
					word = ReplaceIfIgnoredMatch(word, localeData.ignoredAdditional[i]);
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
