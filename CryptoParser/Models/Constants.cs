namespace CryptoParser.Models
{
   public static class Constants
   {
      public static readonly int Balance = 100000;
      private static readonly string[] _binanceBanksNames = { "Tinkoff", "RosBank", "RaiffeisenBankRussia", "QIWI", "YandexMoneyNew" };
      private static readonly string[] _OKXBanksNames = {     "Tinkoff", "Rosbank", "Raiffaizen",           "QiWi", "Yandex.Money"   };
      private static readonly string[] _binanceCurrenciesNames = { "USDT", "BTC", "BUSD", "BNB", "ETH" };
      private static readonly string[] _OKXCurrenciesNames = {     "USDT", "BTC",                "ETH" };
      public static readonly Dictionary<string, string> EngToRusDict = new()
      {
         { "Tinkoff",              "Тинькофф"  },
         { "RosBank",              "Росбанк"   },
         { "RaiffeisenBankRussia", "Райфайзен" },
         { "QIWI",                 "QIWI"      },
         { "YandexMoneyNew",       "ЮMoney"    },
         { "Rosbank",              "Росбанк"   },
         { "Raiffaizen",           "Райфайзен" },
         { "QiWi",                 "QIWI"      },
         { "Yandex.Money",         "ЮMoney"    },
      };
      public static readonly Dictionary<string, string> TablesPos = new()
      {
         { "Rates|Binance|",                         "Binance1:A:3"   },
         { "MarketRates|Binance|",                   "Binance1:H:10"  },
         { "Convert|Binance|",                       "Binance1:A:10"  },
         { "Bank|Binance|Tinkoff",                   "Binance1:A:17"  },
         { "Bank|Binance|RosBank",                   "Binance1:A:24"  },
         { "Bank|Binance|RaiffeisenBankRussia",      "Binance1:A:31"  },
         { "Bank|Binance|QIWI",                      "Binance1:A:38"  },
         { "Bank|Binance|YandexMoneyNew",            "Binance1:A:45"  },
         { "Currency|Binance|USDT",                  "Binance1:H:17"  },
         { "Currency|Binance|BTC",                   "Binance1:H:24"  },
         { "Currency|Binance|BUSD",                  "Binance1:H:31"  },
         { "Currency|Binance|BNB",                   "Binance1:H:38"  },
         { "Currency|Binance|ETH",                   "Binance1:H:45"  },
         { "BankLinks|Binance|Tinkoff",              "Binance2:A:3"   },
         { "BankLinks|Binance|RosBank",              "Binance2:A:30"  },
         { "BankLinks|Binance|RaiffeisenBankRussia", "Binance2:A:57"  },
         { "BankLinks|Binance|QIWI",                 "Binance2:A:84"  },
         { "BankLinks|Binance|YandexMoneyNew",       "Binance2:A:111" },
         { "Rates|OKX|",                             "OKX1:A:3"       },
         { "MarketRates|OKX|",                       "OKX1:F:8"       },
         { "Convert|OKX|",                           "OKX1:A:8"       },
         { "Bank|OKX|Tinkoff",                       "OKX1:A:13"      },
         { "Bank|OKX|Rosbank",                       "OKX1:A:18"      },
         { "Bank|OKX|Raiffaizen",                    "OKX1:A:23"      },
         { "Bank|OKX|QiWi",                          "OKX1:A:28"      },
         { "Bank|OKX|Yandex.Money",                  "OKX1:A:33"      },
         { "Currency|OKX|USDT",                      "OKX1:F:13"      },
         { "Currency|OKX|BTC",                       "OKX1:F:20"      },
         { "Currency|OKX|ETH",                       "OKX1:F:27"      },
         { "BankLinks|OKX|Tinkoff",                  "OKX2:A:3"       },
         { "BankLinks|OKX|Rosbank",                  "OKX2:A:20"      },
         { "BankLinks|OKX|Raiffaizen",               "OKX2:A:37"      },
         { "BankLinks|OKX|QiWi",                     "OKX2:A:54"      },
         { "BankLinks|OKX|Yandex.Money",             "OKX2:A:71"      },
      };

      public static string[] BanksNames(ExchangeType exchange)
      {
         switch (exchange)
         {
            case ExchangeType.Binance:
               return _binanceBanksNames;
            case ExchangeType.OKX:
               return _OKXBanksNames;
            default: 
               return null;
         }
      }

      public static string[] CurrenciesNames(ExchangeType exchange)
      {
         switch (exchange)
         {
            case ExchangeType.Binance:
               return _binanceCurrenciesNames;
            default:
               return _OKXCurrenciesNames;
         }
      }
   }
}
