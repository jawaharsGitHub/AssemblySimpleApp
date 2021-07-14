using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExtensionMethod
{
    public class AdangalFn
    {
        public static string GetSumThreeDotNo(List<string> nos)
        {
            try
            {
                var decimalList = new List<decimal>();
                var intList = new List<int>();

                nos.ForEach(fe =>
                {
                    decimalList.Add(Convert.ToDecimal(fe.Substring(fe.IndexOf(".") + 1).Trim()));
                    intList.Add(Convert.ToInt32(fe.Split('.')[0]));
                });

                var addedData = decimalList.Sum();
                var intAddedData = intList.Sum();

                var finalData = addedData + (intAddedData * 100);

                var firstPart = Convert.ToInt32(finalData.ToString().Split('.')[0]) / 100;
                var secondData = Convert.ToInt32(finalData.ToString().Split('.')[0]) % 100;
                var secondPart = secondData.ToString().PadLeft(2, '0');
                var thirdPart = finalData.ToString().Split('.')[1].PadLeft(2, '0');

                var result = $"{firstPart}.{secondPart}.{thirdPart}";

                return result;

                //var finalData = addedData + (intAddedData * 100);

                //if (finalData >= 100)
                //{
                //    var firstPart = Convert.ToDecimal(Convert.ToInt32(finalData.ToString().Split('.')[0])) / Convert.ToDecimal(100);
                //    var secPart = $"{finalData.ToString().Split('.')[1]}";
                //    return $"{firstPart}.{secPart}";
                //}

                //return $"0.{finalData}";

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public static string GetSumThreeDotNo_Old(List<string> nos)
        {
            try
            {
                var decimalList = new List<decimal>();
                var intList = new List<int>();

                nos.ForEach(fe =>
                {
                    decimalList.Add(Convert.ToDecimal(fe.Substring(fe.IndexOf(".") + 1).Trim()));
                    intList.Add(Convert.ToInt32(fe.Split('.')[0]));
                });

                var addedData = decimalList.Sum();
                var intAddedData = intList.Sum();

                var finalData = addedData + (intAddedData * 100);

                if (finalData >= 100)
                {
                    var firstPart = Convert.ToDecimal(Convert.ToInt32(finalData.ToString().Split('.')[0])) / Convert.ToDecimal(100);
                    var secPart = $"{finalData.ToString().Split('.')[1]}";
                    return $"{firstPart}.{secPart}";
                }

                return $"0.{finalData}";

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
