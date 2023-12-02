using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConversionService.Dollar
{
    public class DollarConversion
    {
        public DollarConversion()
        {
        }        

        internal string GetData(string enteredValue)
        {
            string centPart = string.Empty;
            string centValue = string.Empty;
            string dollarPart = string.Empty;
            string dollarValue = string.Empty;
            string convertedValue = string.Empty;
            StringBuilder sbCompleteValue = new StringBuilder();

            string[] strArray = new string[2];// cannot be more than 2 indexes, 0-dollar,1-cents
            try
            {
                //Split the string or complete string is required
                strArray = SplitString(enteredValue);

                dollarPart = strArray[0];

                if (!string.IsNullOrEmpty(strArray[1]))
                    centPart = strArray[1];

                //call the method in for loop for 2 instance or directly pass if 100% sure
                dollarValue = DollarPartConversion(dollarPart, sbCompleteValue);
                centValue = CentPartConversion(centPart, centValue, sbCompleteValue);

                convertedValue = sbCompleteValue.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                //Reset Array with Default values
                strArray[0] = "\0";
                strArray[1] = "\0";
            }

            //return converted Value
            return convertedValue;
        }

        /// <summary>
        /// Method to take care of Dollar Part Conversion
        /// </summary>
        private string CentPartConversion(string centPart, string centValue, StringBuilder sbCompleteValue)
        {
            if (!string.IsNullOrEmpty(centPart))
            {
                sbCompleteValue.Append(" and ");
                centValue = ConvertMethod(centPart);
                if (centValue.Equals("one"))
                    sbCompleteValue.Append(centValue + " cent");
                else
                    sbCompleteValue.Append(centValue + " cents");
            }

            return centValue;
        }

        /// <summary>
        /// Method to take care of Cent Part Conversion
        /// </summary>      
        private string DollarPartConversion(string dollarPart, StringBuilder sbCompleteValue)
        {
            string dollarValue = ConvertMethod(dollarPart);
            if (dollarValue.Equals("one"))
                sbCompleteValue.Append(dollarValue + " dollar");
            else
                sbCompleteValue.Append(dollarValue + " dollars");
            return dollarValue;
        }

        /// <summary>
        /// Split string on the basis of character ','
        /// </summary>        
        private string[] SplitString(string enteredValue)
        {
            string[] strArray = new string[2];
            if (enteredValue.Contains(","))
            {
                strArray = enteredValue.Split(',');
                if (!string.IsNullOrEmpty(strArray[1]))
                {
                    if (strArray[1].Equals("1"))
                        strArray[1] = "10";
                }
                else
                {
                    strArray[1] = "0";
                }
                if (string.IsNullOrEmpty(strArray[0]))
                    strArray[0] = "0";
            }
            else
            {
                strArray[0] = enteredValue;
            }

            return strArray;
        }

        /// <summary>
        /// Method called for dollars and cents separately
        /// </summary>
        /// <param name="strValue">value to convert</param>
        /// <returns></returns>
        private string ConvertMethod(string strValue)
        {
            string ConversionString = string.Empty;
            try
            {
                ArrayList arrylist = new ArrayList();

                //split into combination of 3, reverse the order and save in arraylist, fill empty indexes with \0/null value
                arrylist = SplitAndReverse(strValue);

                //If digit 0 - nothing
                //if digit 1 - thousand
                //if digit 2 - million
                ConversionString = CalculatedValue(arrylist);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ConversionString;
        }

        /// <summary>
        /// Method to do the conversion by taking each array from arraylist
        /// </summary>        
        /// <param name="arrylist">arraylist contains spilited groups</param>
        /// <returns>complete string for either dollar/cents</returns>
        private string CalculatedValue(ArrayList arrylist)
        {
            int digCount = 0;
            string ConversionString = string.Empty;
            string[] strNumbers = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            string[] strTeens = new string[] { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "ninteen" };
            string[] str10Numbers = new string[] { "default", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninty" };
            string[] str100Numbers = new string[] { "Hundred", "Thousand", "Million" };

            if (arrylist.Count > 0)
                foreach (string item in arrylist)
                {
                    char[] chararray = new char[3];
                    chararray = item.ToCharArray();
                    int noOfDigits = item.Length;
                    string tempo = string.Empty;
                    string retrieveVal = string.Empty;

                    for (int i = 0; i < noOfDigits; i++)
                    {
                        if (noOfDigits == 1)
                        {
                            char chartemp0 = item[0];
                            int specificInt0 = Convert.ToInt32(chartemp0.ToString());
                            string temp0 = strNumbers[specificInt0];

                            retrieveVal = temp0;
                            break;
                        }
                        else if (noOfDigits == 2)
                        {
                            if (item[0] == '0')
                            {
                                char chartemp1 = item[1];
                                int specificInt1 = Convert.ToInt32(chartemp1.ToString());
                                retrieveVal = str10Numbers[specificInt1];//10,20,30,40,50,60,70,80,90
                            }
                            else
                            {
                                if (item[1] == '1')
                                {
                                    char chartemp0 = item[0];
                                    int specificInt0 = Convert.ToInt32(chartemp0.ToString());
                                    string temp0 = strTeens[specificInt0 - 1];//11,12,13,14,15,16,17,18,19

                                    retrieveVal = temp0;
                                }
                                else
                                {
                                    char chartemp1 = item[1];
                                    int specificInt1 = Convert.ToInt32(chartemp1.ToString());
                                    string temp1 = str10Numbers[specificInt1];

                                    char chartemp0 = item[0];
                                    int specificInt0 = Convert.ToInt32(chartemp0.ToString());
                                    string temp0 = strNumbers[specificInt0];

                                    if (!temp1.Equals(str10Numbers[0]))
                                        retrieveVal = temp1 + "-" + temp0;
                                    else
                                    {
                                        retrieveVal = temp0;
                                    }
                                }
                            }
                            break;
                        }
                        else if (noOfDigits == 3)
                        {
                            string temp0 = string.Empty;
                            string temp1 = string.Empty;
                            string temp2 = string.Empty;

                            if (item[2] != '0')
                            {
                                char chartemp2 = item[2];
                                int specificInt2 = Convert.ToInt32(chartemp2.ToString());
                                temp2 = strNumbers[specificInt2];
                            }

                            if (item[0] == '0')
                            {
                                char chartemp1 = item[1];
                                int specificInt1 = Convert.ToInt32(chartemp1.ToString());
                                temp1 = str10Numbers[specificInt1];//d, 10,20,30,40,50,60,70,80,90
                                if (temp1 == str10Numbers[0])
                                    temp1 = string.Empty;
                            }
                            else
                            {
                                if (item[1] == '1')
                                {
                                    char chartemp0 = item[0];
                                    int specificInt0 = Convert.ToInt32(chartemp0.ToString());
                                    temp0 = strTeens[specificInt0 - 1];//11,12,13,14,15,16,17,18,19                                    
                                }
                                else
                                {
                                    if (item[1] != '0')
                                    {
                                        char chartemp1 = item[1];
                                        int specificInt1 = Convert.ToInt32(chartemp1.ToString());
                                        temp1 = str10Numbers[specificInt1];
                                        if (temp1 == str10Numbers[0])
                                            temp1 = string.Empty;
                                    }

                                    if (item[0] != '0')
                                    {
                                        char chartemp0 = item[0];
                                        int specificInt0 = Convert.ToInt32(chartemp0.ToString());
                                        temp0 = strNumbers[specificInt0];
                                    }
                                }
                            }

                            //Get all the combinations 000,001,010,011,100,101,110,111
                            retrieveVal = CombineStrings(temp0, temp1, temp2);

                            break;
                        }
                    }

                    if (digCount == 0)
                        ConversionString = retrieveVal;
                    else if (digCount == 1)
                        ConversionString = retrieveVal + " thousand " + ConversionString;
                    else if (digCount == 2)
                        ConversionString = retrieveVal + " million " + ConversionString;

                    digCount++;
                }

            return ConversionString;
        }

        /// <summary>
        /// Method created to set Truth Table for 3 bits combinations.
        /// </summary>      
        private static string CombineStrings(string temp0, string temp1, string temp2)
        {
            string retrieveVal;
            if (string.IsNullOrEmpty(temp2) && string.IsNullOrEmpty(temp1))//001
                retrieveVal = temp0;
            else if (string.IsNullOrEmpty(temp2) && string.IsNullOrEmpty(temp0))//010
                retrieveVal = temp1;
            else if (string.IsNullOrEmpty(temp2))//011
                retrieveVal = temp1 + "-" + temp0;
            else if (string.IsNullOrEmpty(temp1) && string.IsNullOrEmpty(temp0))//100
                retrieveVal = temp2 + " hundred ";
            else if (string.IsNullOrEmpty(temp1))//101
                retrieveVal = temp2 + " hundred " + temp0;
            else if (string.IsNullOrEmpty(temp0))//110
                retrieveVal = temp2 + " hundred " + temp1;
            else //111
                retrieveVal = temp2 + " hundred " + temp1 + "-" + temp0;
            return retrieveVal;
        }

        /// <summary>
        /// split into combination of 3, reverse the order and save in arraylist, fill empty indexes with \0/null value
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private ArrayList SplitAndReverse(string strValue)
        {
            ArrayList arrylist = new ArrayList();
            char[] arraychar = new char[strValue.Length];
            arraychar = strValue.ToCharArray();
            char[] splitedchars = new char[3];
            int count = 0; bool blFlagFilled = true;
            for (int i = arraychar.Length - 1; i >= 0; i--)
            {
                char character = arraychar[i];
                count++;

                if (count == 3)
                {
                    splitedchars[count - 1] = arraychar[i];

                    string s = new string(splitedchars);
                    s = s.Replace("\0", string.Empty);
                    arrylist.Add(s);
                    count = 0; blFlagFilled = true;
                    splitedchars[0] = splitedchars[1] = splitedchars[2] = '\0';
                }
                else
                {
                    splitedchars[count - 1] = arraychar[i]; blFlagFilled = false;
                }
            }
            if (!blFlagFilled)
            {
                string s1 = new string(splitedchars);
                s1 = s1.Replace("\0", string.Empty);
                arrylist.Add(s1);
            }

            return arrylist;
        }
    }
}
