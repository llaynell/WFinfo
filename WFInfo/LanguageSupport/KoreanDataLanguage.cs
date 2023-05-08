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
			s = localizedName;
			s = " " + ReplaceKeyString(s, this._localeData.levenshteinDistanceReplaces, "");
			t = " " + ReplaceKeyString(t, this._localeData.levenshteinDistanceReplaces, "");
			int length = s.Length;
			int length2 = t.Length;
			int[,] array = new int[length + 1, length2 + 1];

			if (length == 0 || length2 == 0)
			{
				return length + length2;
			}
			for (int i = 1; i < s.Length; i++)
			{
				array[i, 0] = i * 9;
			}
			for (int j = 1; j < t.Length; j++)
			{
				array[0, j] = j * 9;
			}

			for (int i = 1; i < s.Length; i++)
			{
				for (int j = 1; j < t.Length; j++)
				{
					int num = 0;
					int num2 = 0;
					char c = s[i];
					char c2 = t[j];
					int[] array2 = new int[3];
					int[] array3 = new int[3];
					array2[0] = (int)((c - '가' - (c - '가') % '\u001c') / '\u001c' / '\u0015');
					array2[1] = (int)((c - '가' - (c - '가') % '\u001c') / '\u001c' % '\u0015');
					array2[2] = (int)((c - '가') % '\u001c');
					array3[0] = (int)((c2 - '가' - (c2 - '가') % '\u001c') / '\u001c' / '\u0015');
					array3[1] = (int)((c2 - '가' - (c2 - '가') % '\u001c') / '\u001c' % '\u0015');
					array3[2] = (int)((c2 - '가') % '\u001c');
					if (array2[0] != array3[0] && array2[1] != array3[1] && array2[2] != array3[2])
					{
						num = 9;
					}
					else
					{
						for (int k = 0; k < 3; k++)
						{
							if (array2[k] != array3[k])
							{
								if (GroupEquals(_korean[k], array2[k], array3[k]))
								{
									num2++;
								}
								else
								{
									num++;
								}
							}
						}
						num *= 3;
						num2 *= 2;
					}
					array[i, j] = Math.Min(Math.Min(array[i - 1, j] + 9, array[i, j - 1] + 9), array[i - 1, j - 1] + num + num2);
				}
			}

			return array[s.Length - 1, t.Length - 1];
		}

		public override bool isItLanguage(string str)
		{
			char c = str[0];
			return ('ᄀ' <= c && c <= 'ᇿ') || ('㄰' <= c && c <= '㆏') || ('가' <= c && c <= '힣');
		}

		private static List<Dictionary<int, List<int>>> _korean = new List<Dictionary<int, List<int>>>
		{
			new Dictionary<int, List<int>>
			{
				{
					0,
					new List<int>
					{
						6,
						7,
						8,
						16
					}
				},
				{
					1,
					new List<int>
					{
						2,
						3,
						4,
						16,
						5,
						9,
						10
					}
				},
				{
					2,
					new List<int>
					{
						12,
						13,
						14
					}
				},
				{
					3,
					new List<int>
					{
						0,
						1,
						15,
						11,
						18
					}
				}
			},
			new Dictionary<int, List<int>>
			{
				{
					0,
					new List<int>
					{
						20,
						5,
						1,
						7,
						3,
						19
					}
				},
				{
					1,
					new List<int>
					{
						16,
						11,
						15,
						10
					}
				},
				{
					2,
					new List<int>
					{
						4,
						0,
						6,
						2,
						14,
						9
					}
				},
				{
					3,
					new List<int>
					{
						18,
						13,
						8,
						17,
						12
					}
				}
			},
			new Dictionary<int, List<int>>
			{
				{
					0,
					new List<int>
					{
						16,
						17,
						18,
						26
					}
				},
				{
					1,
					new List<int>
					{
						4,
						5,
						6,
						7,
						8,
						9,
						10,
						11,
						12,
						13,
						14,
						15,
						19,
						20,
						25
					}
				},
				{
					2,
					new List<int>
					{
						22,
						23
					}
				},
				{
					3,
					new List<int>
					{
						1,
						2,
						3,
						24,
						21,
						27
					}
				},
				{
					4,
					new List<int>
					{
						0
					}
				}
			}
		};
	}
}
