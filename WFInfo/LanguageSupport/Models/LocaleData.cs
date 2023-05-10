using System;

namespace WFInfo.LanguageSupport
{
    [Serializable]
	public struct LocaleData
	{
        [Serializable]
        public struct MinMaxLocaleChars
		{
			public int min;
			public int max;

            public MinMaxLocaleChars(int min, int max)
            {
                this.min = min;
                this.max = max;
            }
        }

		public string localeName;
		public string localeNameMarket;

		public string trainedDataChecksum;
		public string regexSymbols;
		public string ocrCharWhitelist;
		public string levenshteinDistanceReplaces;

		public MinMaxLocaleChars[] minMaxLanguageChars;

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
		// Need remove it?
		public string stringKey;
		public string handleKey;
		public string ornamentKey;
		public string bladesKey;
		public string hiltKey;
		public string linkKey;
		public string starsKey;

		public string kavasaKey;
		public string kavasaSetName;
		public string setKey;

        /// <summary>
        /// "{0} ***" {0} - partName
        /// </summary>
        public string blueprintFormat;
        /// <summary>
        /// "{0} ***" {0} - partName
        /// </summary>
        public string primeBlueprintFormat;

		public LocaleDataInfo GetLocaleInfo()
		{
			return new LocaleDataInfo()
			{
                key = localeNameMarket,
                trainedDataChecksum = trainedDataChecksum,
                name = localeName
            };
		}
	}
}
