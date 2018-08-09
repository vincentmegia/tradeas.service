namespace Tradeas.Colfinancial.Provider
{
    internal static class Constants
    {
        public const string HeaderFrameId = "headern";
        public const string MainFrameId = "main";
        public const string TradeTabSelector = "a[onclick='ital(3);clickTable(3,1);getwin(3);'][onmouseover='displayTable(3);'][onmouseout='hideTable();']";
        public const string PortfolioTabSelector = "a[onclick='ital(44);clickTable(3,2);getwin(44);'][onmouseover='displayTable(3);'][onmouseout='hideTable();']";
        public const string InputId = "ebrokerno";
        public const string DateFromSelectName = "cbDateFrom";
        public const string DateToSelectName = "cbDateTo";
        public const string User1TextboxName = "txtUser1";
        public const string User2TextboxName = "txtUser2";
        public const string PasswordTextboxName = "txtPassword";
        public const string SubmitButtonSelector = "input[value = 'LOG IN'][type = 'button']";
        public const string LoginPageUrl = "https://www.colfinancial.com/ape/Final2/home/HOME_NL_MAIN.asp?p=0";
        public const string HomePageLikeUrl = "ape/FINAL2_STARTER/HOME/HOME.asp";
        public const string QuoteTabSelector = "a[onclick='ital(2);clickTable(2,1);getwin(2);'][onmouseover='displayTable(2);'][onmouseout='hideTable();']";
        public const string BrokerSubTabSelector = "a[onclick='ital(24);clickSubTable1(3);getwin(24);'][onmouseover='delaySubTable1(3);'][onmouseout='hideSubTable1();']";
    }
}