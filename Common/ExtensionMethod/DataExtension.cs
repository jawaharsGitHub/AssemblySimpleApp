using System;
using System.Collections.Generic;
using System.Globalization;

namespace Common.ExtensionMethod
{
    public static class DataExtension
    {
        // TODO: Need to merge ToMOney and TokFormat!!
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string ToMoney(this int number)
        {
            NumberFormatInfo nfo = new NumberFormatInfo();
            nfo.CurrencyGroupSeparator = ",";
            nfo.CurrencyNegativePattern = Convert.ToInt16(number < 0);
            // you are interested in this part of controlling the group sizes
            nfo.CurrencyGroupSizes = new int[] { 3, 2 };
            nfo.CurrencySymbol = ""; // "Rs.";

            return number.ToString("c0", nfo); // prints 1,50,00,000

        }

        public static string ToMoney(this decimal number)
        {
            var no = Convert.ToInt32(number);
            return ToMoney(no);
        }

        public static double RoundMoney(this double number)
        {
            return Math.Round(number, 2);
        }

        public static double RoundMoneyOnly(this double? number)
        {
            return Math.Round(number.Value);
        }

        public static double RoundMoney(this double? number)
        {
            return RoundMoney(number.Value);
        }

        public static double RoundPoints(this double number)
        {
            return Math.Round(number, 1);
        }
    }
}
