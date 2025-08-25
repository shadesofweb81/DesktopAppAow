namespace WinFormsApp1.Services
{
    public class CountryService
    {
        public List<CountryOption> GetCountries()
        {
            return new List<CountryOption>
            {
                new CountryOption { Code = "US", Name = "United States", Currency = "USD" },
                new CountryOption { Code = "IN", Name = "India", Currency = "INR" },
                new CountryOption { Code = "GB", Name = "United Kingdom", Currency = "GBP" },
                new CountryOption { Code = "DE", Name = "Germany", Currency = "EUR" },
                new CountryOption { Code = "FR", Name = "France", Currency = "EUR" },
                new CountryOption { Code = "IT", Name = "Italy", Currency = "EUR" },
                new CountryOption { Code = "ES", Name = "Spain", Currency = "EUR" },
                new CountryOption { Code = "NL", Name = "Netherlands", Currency = "EUR" },
                new CountryOption { Code = "JP", Name = "Japan", Currency = "JPY" },
                new CountryOption { Code = "CN", Name = "China", Currency = "CNY" },
                new CountryOption { Code = "CA", Name = "Canada", Currency = "CAD" },
                new CountryOption { Code = "AU", Name = "Australia", Currency = "AUD" },
                new CountryOption { Code = "BR", Name = "Brazil", Currency = "BRL" },
                new CountryOption { Code = "RU", Name = "Russia", Currency = "RUB" },
                new CountryOption { Code = "KR", Name = "South Korea", Currency = "KRW" },
                new CountryOption { Code = "MX", Name = "Mexico", Currency = "MXN" },
                new CountryOption { Code = "SG", Name = "Singapore", Currency = "SGD" },
                new CountryOption { Code = "HK", Name = "Hong Kong", Currency = "HKD" },
                new CountryOption { Code = "CH", Name = "Switzerland", Currency = "CHF" },
                new CountryOption { Code = "TR", Name = "Turkey", Currency = "TRY" },
                new CountryOption { Code = "IL", Name = "Israel", Currency = "ILS" },
                new CountryOption { Code = "TH", Name = "Thailand", Currency = "THB" },
                new CountryOption { Code = "ZA", Name = "South Africa", Currency = "ZAR" },
                new CountryOption { Code = "AE", Name = "United Arab Emirates", Currency = "AED" },
                new CountryOption { Code = "SA", Name = "Saudi Arabia", Currency = "SAR" }
            };
        }

        public List<CurrencyOption> GetCurrencies()
        {
            return new List<CurrencyOption>
            {
                new CurrencyOption { Code = "USD", Name = "US Dollar", Symbol = "$" },
                new CurrencyOption { Code = "INR", Name = "Indian Rupee", Symbol = "₹" },
                new CurrencyOption { Code = "GBP", Name = "British Pound", Symbol = "£" },
                new CurrencyOption { Code = "EUR", Name = "Euro", Symbol = "€" },
                new CurrencyOption { Code = "JPY", Name = "Japanese Yen", Symbol = "¥" },
                new CurrencyOption { Code = "CNY", Name = "Chinese Yuan", Symbol = "¥" },
                new CurrencyOption { Code = "CAD", Name = "Canadian Dollar", Symbol = "C$" },
                new CurrencyOption { Code = "AUD", Name = "Australian Dollar", Symbol = "A$" },
                new CurrencyOption { Code = "BRL", Name = "Brazilian Real", Symbol = "R$" },
                new CurrencyOption { Code = "RUB", Name = "Russian Ruble", Symbol = "₽" },
                new CurrencyOption { Code = "KRW", Name = "South Korean Won", Symbol = "₩" },
                new CurrencyOption { Code = "MXN", Name = "Mexican Peso", Symbol = "$" },
                new CurrencyOption { Code = "SGD", Name = "Singapore Dollar", Symbol = "S$" },
                new CurrencyOption { Code = "HKD", Name = "Hong Kong Dollar", Symbol = "HK$" },
                new CurrencyOption { Code = "CHF", Name = "Swiss Franc", Symbol = "CHF" },
                new CurrencyOption { Code = "TRY", Name = "Turkish Lira", Symbol = "₺" },
                new CurrencyOption { Code = "ILS", Name = "Israeli Shekel", Symbol = "₪" },
                new CurrencyOption { Code = "THB", Name = "Thai Baht", Symbol = "฿" },
                new CurrencyOption { Code = "ZAR", Name = "South African Rand", Symbol = "R" },
                new CurrencyOption { Code = "AED", Name = "UAE Dirham", Symbol = "د.إ" },
                new CurrencyOption { Code = "SAR", Name = "Saudi Riyal", Symbol = "﷼" }
            };
        }

        public string GetCurrencyByCountry(string countryCode)
        {
            var country = GetCountries().FirstOrDefault(c => c.Code.Equals(countryCode, StringComparison.OrdinalIgnoreCase));
            return country?.Currency ?? "USD";
        }
    }

    public class CountryOption
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
    }

    public class CurrencyOption
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
    }
} 