using System;

namespace WFInfo.LanguageSupport
{
	[Serializable]
	public struct LocaleData
	{
		public string localeName;
		public string localeNameMarket;

		public string traineddataChecksum;
		public string regexSymbols;
		public string ocrCharWhitelist;
		public string levenshteinDistanceReplaces;

		public int[] minLanguageChars;
		public int[] maxLanguageChars;

		public int minPartNameLenght;

		public string[] ignoredForma;
		public string[] ignoredExsilusWeaponAdapter;
		public string[] ignoredKuva;
		public string[] ignoredRivenSliver;
		public string[] ignoredAyatanAmberStar;
		public string[][] ignoredAdditional;

		public string primeKey;
		public string blueprintKey;
		public string neuropticsKey;
		public string chassisKey;
		public string systemsKey;
		public string harnessKey;
		public string wingsKey;
		public string lowerLimbKey;
		public string upperLimbKey;
		public string carapaceKey;
		public string cerebrumKey;
		public string bladeKey;
		public string pouchKey;
		public string headKey;
		public string barrelKey;
		public string receiverKey;
		public string stockKey;
		public string discKey;
		public string gripKey;
		public string stringKey;
		public string handleKey;
		public string ornamentKey;
		public string bladesKey;
		public string hiltKey;
		public string starsKey;
	}
}
