using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Utility {

    public class CardReader {

        #region Public Properties

        public readonly String CardExpiration;
        public readonly String CardNumber;
        public readonly CreditCardTypeType CardType;
        public readonly String PersonName;

        private const string cardRegex = @"^(?:(?<Visa>4\\d{3})|
            (?<MasterCard>5[1-5]\\d{2})|(?<Discover>6011)|(?<DinersClub>
            (?:3[68]\\d{2})|(?:30[0-5]\\d))|(?<Amex>3[47]\\d{2}))([ -]?)
            (?(DinersClub)(?:\\d{6}\\1\\d{4})|(?(Amex)(?:\\d{6}\\1\\d{5})
            |(?:\\d{4}\\1\\d{4}\\1\\d{4})))$";

        public enum CreditCardTypeType {
            Visa,
            MasterCard,
            Discover,
            Amex,
            Switch,
            Solo
        }

        #endregion Public Properties

        #region Public Constructors

        public CardReader(String cardReader) {
            Boolean CaretPresent = false;
            Boolean EqualPresent = false;

            CaretPresent = cardReader.Contains("^");
            EqualPresent = cardReader.Contains("=");

            if (CaretPresent) {
                String[] CardData = cardReader.Split('^');

                PersonName = FormatName(CardData[1]);
                CardNumber = FormatCardNumber(CardData[0]);
                CardExpiration = CardData[2].Substring(2, 2) + "/" + CardData[2].Substring(0, 2);
            }
            else if (EqualPresent) {
                String[] CardData = cardReader.Split('=');

                CardNumber = FormatCardNumber(CardData[0]);
                CardExpiration = CardData[1].Substring(2, 2) + "/" + CardData[1].Substring(0, 2);
            }
            //Determine the card type based on the number
            CreditCardTypeType? CardType = GetCardTypeFromNumber(CardNumber);
        }

        #endregion Public Constructors

        #region Private Methods

        private string FormatCardNumber(string o) {
            string result = string.Empty;

            result = Regex.Replace(o, "[^0-9]", string.Empty);

            return result;
        }

        private string FormatName(string o) {
            string result = string.Empty;

            if (o.Contains("/")) {
                string[] NameSplit = o.Split('/');

                result = NameSplit[1] + " " + NameSplit[0];
            }
            else {
                result = o;
            }

            return result;
        }

        private CreditCardTypeType? GetCardTypeFromNumber(String cardNum) {
            //Create new instance of Regex comparer with our
            //credit card regex patter
            Regex cardTest = new Regex(cardRegex);

            //Compare the supplied card number with the regex
            //pattern and get reference regex named groups
            GroupCollection gc = cardTest.Match(cardNum).Groups;

            //Compare each card type to the named groups to
            //determine which card type the number matches
            if (gc[CreditCardTypeType.Amex.ToString()].Success) {
                return CreditCardTypeType.Amex;
            }
            else if (gc[CreditCardTypeType.MasterCard.ToString()].Success) {
                return CreditCardTypeType.MasterCard;
            }
            else if (gc[CreditCardTypeType.Visa.ToString()].Success) {
                return CreditCardTypeType.Visa;
            }
            else if (gc[CreditCardTypeType.Discover.ToString()].Success) {
                return CreditCardTypeType.Discover;
            }
            else {
                //Card type is not supported by our system, return null
                //(You can modify this code to support more (or less)
                // card types as it pertains to your application)
                return null;
            }
        }

        private bool IsValidNumber(string cardNum, CreditCardTypeType? cardType) {
            //Create new instance of Regex comparer with our
            //credit card regex pattern
            Regex cardTest = new Regex(cardRegex);

            //Make sure the supplied number matches the supplied
            //card type
            if (cardTest.Match(cardNum).Groups[cardType.ToString()].Success) {
                //If the card type matches the number, then run it
                //through Luhn's test to make sure the number appears correct
                if (PassesLuhnTest(cardNum))
                    return true;
                else
                    //The card fails Luhn's test
                    return false;
            }
            else
                //The card number does not match the card type
                return false;
        }

        private bool IsValidNumber(string cardNum) {
            Regex cardTest = new Regex(cardRegex);

            //Determine the card type based on the number
            CreditCardTypeType? cardType = GetCardTypeFromNumber(cardNum);

            //Call the base version of IsValidNumber and pass the
            //number and card type
            if (IsValidNumber(cardNum, cardType))
                return true;
            else
                return false;
        }

        private bool PassesLuhnTest(string cardNumber) {
            //Clean the card number- remove dashes and spaces
            cardNumber = cardNumber.Replace("-", "").Replace(" ", "");

            //Convert card number into digits array
            int[] digits = new int[cardNumber.Length];
            for (int len = 0; len < cardNumber.Length; len++) {
                digits[len] = Int32.Parse(cardNumber.Substring(len, 1));
            }

            //Luhn Algorithm
            //Adapted from code available on Wikipedia at
            //http://en.wikipedia.org/wiki/Luhn_algorithm
            int sum = 0;
            bool alt = false;
            for (int i = digits.Length - 1; i >= 0; i--) {
                int curDigit = digits[i];
                if (alt) {
                    curDigit *= 2;
                    if (curDigit > 9) {
                        curDigit -= 9;
                    }
                }
                sum += curDigit;
                alt = !alt;
            }

            //If Mod 10 equals 0, the number is good and this will return true
            return sum % 10 == 0;
        }

        #endregion Private Methods
    }
}