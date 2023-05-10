using System;
using System.Collections.Generic;

namespace WFInfo.LanguageSupport
{
	internal class KoreanDataLanguage : DataLanguageBase
	{
		internal KoreanDataLanguage(LocaleData localeData) : base(localeData)
		{
		}

		public override int CalculateLevenshteinDistance(string localizedName, string s, string t)
		{
            // NameData s 를 한글명으로 가져옴
            s = localizedName;

            // i18n korean edit distance algorithm
            s = " " + ReplaceKeyString(s, this.localeData.levenshteinDistanceReplaces, "");
            t = " " + ReplaceKeyString(t, this.localeData.levenshteinDistanceReplaces, "");

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0 || m == 0)
                return n + m;
            int i, j;

            for (i = 1; i < s.Length; i++) d[i, 0] = i * 9;
            for (j = 1; j < t.Length; j++) d[0, j] = j * 9;

            int s1, s2;

            for (i = 1; i < s.Length; i++)
            {
                for (j = 1; j < t.Length; j++)
                {
                    s1 = 0;
                    s2 = 0;

                    char cha = s[i];
                    char chb = t[j];
                    int[] a = new int[3];
                    int[] b = new int[3];
                    a[0] = (((cha - 0xAC00) - (cha - 0xAC00) % 28) / 28) / 21;
                    a[1] = (((cha - 0xAC00) - (cha - 0xAC00) % 28) / 28) % 21;
                    a[2] = (cha - 0xAC00) % 28;

                    b[0] = (((chb - 0xAC00) - (chb - 0xAC00) % 28) / 28) / 21;
                    b[1] = (((chb - 0xAC00) - (chb - 0xAC00) % 28) / 28) % 21;
                    b[2] = (chb - 0xAC00) % 28;

                    if (a[0] != b[0] && a[1] != b[1] && a[2] != b[2])
                    {
                        s1 = 9;
                    }
                    else
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            if (a[k] != b[k])
                            {
                                if (GroupEquals(_korean[k], a[k], b[k]))
                                {
                                    s2 += 1;
                                }
                                else
                                {
                                    s1 += 1;
                                }
                            }

                        }
                        s1 *= 3;
                        s2 *= 2;
                    }

                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 9, d[i, j - 1] + 9), d[i - 1, j - 1] + s1 + s2);
                }
            }

            return d[s.Length - 1, t.Length - 1];
        }

		public override bool isItLanguage(string str)
		{
			bool baseResult = localeData.minMaxLanguageChars != null && localeData.minMaxLanguageChars.Length > 0
				? base.isItLanguage(str)
				: false;

            char c = str[0];
			return ('ᄀ' <= c && c <= 'ᇿ')
				|| ('㄰' <= c && c <= '㆏')
				|| ('가' <= c && c <= '힣')
				|| baseResult;
		}

        public override bool PartNameValid(string name)
        {
			return base.PartNameValid(name.Replace(" ", ""));
        }

        private static readonly List<Dictionary<int, List<int>>> _korean = new List<Dictionary<int, List<int>>>
		{
            new Dictionary<int, List<int>>() {
                { 0, new List<int>{ 6, 7, 8, 16 } }, // ㅁ, ㅂ, ㅃ, ㅍ
                { 1, new List<int>{ 2, 3, 4, 16, 5, 9, 10 } }, // ㄴ, ㄷ, ㄸ, ㅌ, ㄹ, ㅅ, ㅆ
                { 2, new List<int>{ 12, 13, 14 } }, // ㅈ, ㅉ, ㅊ
                { 3, new List<int>{ 0, 1, 15, 11, 18 } } // ㄱ, ㄲ, ㅋ, ㅇ, ㅎ
            },
            new Dictionary<int, List<int>>() {
                { 0, new List<int>{ 20, 5, 1, 7, 3, 19 } }, // ㅣ, ㅔ, ㅐ, ㅖ, ㅒ, ㅢ
                { 1, new List<int>{ 16, 11, 15, 10 } }, // ㅟ, ㅚ, ㅞ, ㅙ
                { 2, new List<int>{ 4, 0, 6, 2, 14, 9 } }, // ㅓ, ㅏ, ㅕ, ㅑ, ㅝ, ㅘ
                { 3, new List<int>{ 18, 13, 8, 17, 12 } } // ㅡ, ㅜ, ㅗ, ㅠ, ㅛ
            },
            new Dictionary<int, List<int>>() {
                { 0, new List<int>{ 16, 17, 18, 26 } }, // ㅁ, ㅂ, ㅄ, ㅍ
                { 1, new List<int>{ 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 19, 20, 25 } }, // ㄴ, ㄵ, ㄶ, ㄷ, ㄹ, ㄺ, ㄻ, ㄼ, ㄽ, ㄾ, ㄿ, ㅀ, ㅅ, ㅆ, ㅌ
                { 2, new List<int>{ 22, 23 } }, // ㅈ, ㅊ
                { 3, new List<int>{ 1, 2, 3, 24, 21, 27 } }, // ㄱ, ㄲ, ㄳ, ㅋ, ㅑ, ㅎ
                { 4, new List<int>{ 0 } }, // 
            }
        };
	}
}
