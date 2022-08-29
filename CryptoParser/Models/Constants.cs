namespace CryptoParser.Models
{
   public struct CVBData
   {
      public readonly CVBType CVB;
      public readonly string[] Banks;
      public readonly string[] Currencies;

      public CVBData(CVBType cvb, string[] banks, string[] currencies)
      {
         CVB = cvb;
         Banks = banks;
         Currencies = currencies;
      }
   }

   public static class Constants
   {
      private static readonly string[] _binanceBanksNames = { "TinkoffNew", "RosBank", "RaiffeisenBankRussia", "QIWI", "YandexMoneyNew" };
      private static readonly string[] _OKXBanksNames =     { "Tinkoff",    "Rosbank", "Raiffaizen",           "QiWi", "Yandex.Money" };
      private static readonly string[] _binanceCurrenciesNames = { "USDT", "BTC", "BUSD", "BNB", "ETH" };
      private static readonly string[] _OKXCurrenciesNames = {     "USDT", "BTC",                "ETH" };
      public static readonly string TimePos = "Описание и настройки:A:2";
      public static readonly Dictionary<string, string> SimpleTablesRanges = new()
      {
         { "Rates|Binance|",                         "Binance1:C:4"  },
         { "MarketRates|Binance|",                   "Binance1:C:11" },
         { "Convert|Binance|",                       "Binance1:K:11" },
         { "Bank|Binance|TinkoffNew",                "Binance1:K:19" },
         { "Bank|Binance|RosBank",                   "Binance1:K:27" },
         { "Bank|Binance|RaiffeisenBankRussia",      "Binance1:K:35" },
         { "Bank|Binance|QIWI",                      "Binance1:K:43" },
         { "Bank|Binance|YandexMoneyNew",            "Binance1:K:51" },
         { "Currency|Binance|USDT",                  "Binance1:C:19" },
         { "Currency|Binance|BTC",                   "Binance1:C:27" },
         { "Currency|Binance|BUSD",                  "Binance1:C:35" },
         { "Currency|Binance|BNB",                   "Binance1:C:43" },
         { "Currency|Binance|ETH",                   "Binance1:C:51" },
         
         { "Rates|OKX|",                             "OKX1:C:4"      },
         { "MarketRates|OKX|",                       "OKX1:C:9"      },
         { "Convert|OKX|",                           "OKX1:K:9"      },
         { "Bank|OKX|Tinkoff",                       "OKX1:K:15"     },
         { "Bank|OKX|Rosbank",                       "OKX1:K:21"     },
         { "Bank|OKX|Raiffaizen",                    "OKX1:K:27"     },
         { "Bank|OKX|QiWi",                          "OKX1:K:33"     },
         { "Bank|OKX|Yandex.Money",                  "OKX1:K:39"     },
         { "Currency|OKX|USDT",                      "OKX1:C:15"     },
         { "Currency|OKX|BTC",                       "OKX1:C:23"     },
         { "Currency|OKX|ETH",                       "OKX1:C:31"     },
      };

      public static readonly Dictionary<string, string> HardTablesRanges = new()
      {
         { "HardLinks|Binance|Binance|TinkoffNew|USDT",           "Binance2:D:4"      },
         { "HardLinks|Binance|Binance|TinkoffNew|BTC",            "Binance2:M:4"      },
         { "HardLinks|Binance|Binance|TinkoffNew|BUSD",           "Binance2:D:12"     },
         { "HardLinks|Binance|Binance|TinkoffNew|BNB",            "Binance2:M:12"     },
         { "HardLinks|Binance|Binance|TinkoffNew|ETH",            "Binance2:D:20"     },
         { "HardLinks|Binance|Binance|RosBank|USDT",              "Binance2:D:30"     },
         { "HardLinks|Binance|Binance|RosBank|BTC",               "Binance2:M:30"     },
         { "HardLinks|Binance|Binance|RosBank|BUSD",              "Binance2:D:38"     },
         { "HardLinks|Binance|Binance|RosBank|BNB",               "Binance2:M:38"     },
         { "HardLinks|Binance|Binance|RosBank|ETH",               "Binance2:D:46"     },
         { "HardLinks|Binance|Binance|RaiffeisenBankRussia|USDT", "Binance2:D:56"     },
         { "HardLinks|Binance|Binance|RaiffeisenBankRussia|BTC",  "Binance2:M:56"     },
         { "HardLinks|Binance|Binance|RaiffeisenBankRussia|BUSD", "Binance2:D:64"     },
         { "HardLinks|Binance|Binance|RaiffeisenBankRussia|BNB",  "Binance2:M:64"     },
         { "HardLinks|Binance|Binance|RaiffeisenBankRussia|ETH",  "Binance2:D:72"     },
         { "HardLinks|Binance|Binance|QIWI|USDT",                 "Binance2:D:82"     },
         { "HardLinks|Binance|Binance|QIWI|BTC",                  "Binance2:M:82"     },
         { "HardLinks|Binance|Binance|QIWI|BUSD",                 "Binance2:D:90"     },
         { "HardLinks|Binance|Binance|QIWI|BNB",                  "Binance2:M:90"     },
         { "HardLinks|Binance|Binance|QIWI|ETH",                  "Binance2:D:98"     },
         { "HardLinks|Binance|Binance|YandexMoneyNew|USDT",       "Binance2:D:108"    },
         { "HardLinks|Binance|Binance|YandexMoneyNew|BTC",        "Binance2:M:108"    },
         { "HardLinks|Binance|Binance|YandexMoneyNew|BUSD",       "Binance2:D:116"    },
         { "HardLinks|Binance|Binance|YandexMoneyNew|BNB",        "Binance2:M:116"    },
         { "HardLinks|Binance|Binance|YandexMoneyNew|ETH",        "Binance2:D:124"    },

         { "HardLinks|OKX|OKX|Tinkoff|USDT",                      "OKX2:D:4"          },
         { "HardLinks|OKX|OKX|Tinkoff|BTC",                       "OKX2:K:4"          },
         { "HardLinks|OKX|OKX|Tinkoff|ETH",                       "OKX2:R:4"          },
         { "HardLinks|OKX|OKX|Rosbank|USDT",                      "OKX2:D:14"         },
         { "HardLinks|OKX|OKX|Rosbank|BTC",                       "OKX2:K:14"         },
         { "HardLinks|OKX|OKX|Rosbank|ETH",                       "OKX2:R:14"         },
         { "HardLinks|OKX|OKX|Raiffaizen|USDT",                   "OKX2:D:24"         },
         { "HardLinks|OKX|OKX|Raiffaizen|BTC",                    "OKX2:K:24"         },
         { "HardLinks|OKX|OKX|Raiffaizen|ETH",                    "OKX2:R:24"         },
         { "HardLinks|OKX|OKX|QiWi|USDT",                         "OKX2:D:34"         },
         { "HardLinks|OKX|OKX|QiWi|BTC",                          "OKX2:K:34"         },
         { "HardLinks|OKX|OKX|QiWi|ETH",                          "OKX2:R:34"         },
         { "HardLinks|OKX|OKX|Yandex.Money|USDT",                 "OKX2:D:44"         },
         { "HardLinks|OKX|OKX|Yandex.Money|BTC",                  "OKX2:K:44"         },
         { "HardLinks|OKX|OKX|Yandex.Money|ETH",                  "OKX2:R:44"         },

         { "HardLinks|Binance|OKX|Tinkoff|USDT",                  "Binance->OKX:D:4"  },
         { "HardLinks|Binance|OKX|Tinkoff|BTC",                   "Binance->OKX:K:4"  },
         { "HardLinks|Binance|OKX|Tinkoff|BUSD",                  "Binance->OKX:R:4"  },
         { "HardLinks|Binance|OKX|Tinkoff|BNB",                   "Binance->OKX:D:12" },
         { "HardLinks|Binance|OKX|Tinkoff|ETH",                   "Binance->OKX:K:12" },
         { "HardLinks|Binance|OKX|Rosbank|USDT",                  "Binance->OKX:D:22" },
         { "HardLinks|Binance|OKX|Rosbank|BTC",                   "Binance->OKX:K:22" },
         { "HardLinks|Binance|OKX|Rosbank|BUSD",                  "Binance->OKX:R:22" },
         { "HardLinks|Binance|OKX|Rosbank|BNB",                   "Binance->OKX:D:30" },
         { "HardLinks|Binance|OKX|Rosbank|ETH",                   "Binance->OKX:K:30" },
         { "HardLinks|Binance|OKX|Raiffaizen|USDT",               "Binance->OKX:D:40" },
         { "HardLinks|Binance|OKX|Raiffaizen|BTC",                "Binance->OKX:K:40" },
         { "HardLinks|Binance|OKX|Raiffaizen|BUSD",               "Binance->OKX:R:40" },
         { "HardLinks|Binance|OKX|Raiffaizen|BNB",                "Binance->OKX:D:48" },
         { "HardLinks|Binance|OKX|Raiffaizen|ETH",                "Binance->OKX:K:48" },
         { "HardLinks|Binance|OKX|QiWi|USDT",                     "Binance->OKX:D:58" },
         { "HardLinks|Binance|OKX|QiWi|BTC",                      "Binance->OKX:K:58" },
         { "HardLinks|Binance|OKX|QiWi|BUSD",                     "Binance->OKX:R:58" },
         { "HardLinks|Binance|OKX|QiWi|BNB",                      "Binance->OKX:D:66" },
         { "HardLinks|Binance|OKX|QiWi|ETH",                      "Binance->OKX:K:66" },
         { "HardLinks|Binance|OKX|Yandex.Money|USDT",             "Binance->OKX:D:76" },
         { "HardLinks|Binance|OKX|Yandex.Money|BTC",              "Binance->OKX:K:76" },
         { "HardLinks|Binance|OKX|Yandex.Money|BUSD",             "Binance->OKX:R:76" },
         { "HardLinks|Binance|OKX|Yandex.Money|BNB",              "Binance->OKX:D:84" },
         { "HardLinks|Binance|OKX|Yandex.Money|ETH",              "Binance->OKX:K:84" },

         { "HardLinks|OKX|Binance|TinkoffNew|USDT",               "OKX->Binance:D:4"  },
         { "HardLinks|OKX|Binance|TinkoffNew|BTC",                "OKX->Binance:M:4"  },
         { "HardLinks|OKX|Binance|TinkoffNew|ETH",                "OKX->Binance:D:12" },
         { "HardLinks|OKX|Binance|RosBank|USDT",                  "OKX->Binance:D:22" },
         { "HardLinks|OKX|Binance|RosBank|BTC",                   "OKX->Binance:M:22" },
         { "HardLinks|OKX|Binance|RosBank|ETH",                   "OKX->Binance:D:30" },
         { "HardLinks|OKX|Binance|RaiffeisenBankRussia|USDT",     "OKX->Binance:D:40" },
         { "HardLinks|OKX|Binance|RaiffeisenBankRussia|BTC",      "OKX->Binance:M:40" },
         { "HardLinks|OKX|Binance|RaiffeisenBankRussia|ETH",      "OKX->Binance:D:48" },
         { "HardLinks|OKX|Binance|QIWI|USDT",                     "OKX->Binance:D:58" },
         { "HardLinks|OKX|Binance|QIWI|BTC",                      "OKX->Binance:M:58" },
         { "HardLinks|OKX|Binance|QIWI|ETH",                      "OKX->Binance:D:66" },
         { "HardLinks|OKX|Binance|YandexMoneyNew|USDT",           "OKX->Binance:D:76" },
         { "HardLinks|OKX|Binance|YandexMoneyNew|BTC",            "OKX->Binance:M:76" },
         { "HardLinks|OKX|Binance|YandexMoneyNew|ETH",            "OKX->Binance:D:84" },
      };

      public static CVBData GetCVBData(CVBType cvb)
      {
         switch (cvb)
         {
            case CVBType.Binance:
               return new CVBData(CVBType.Binance, _binanceBanksNames, _binanceCurrenciesNames);
            case CVBType.OKX:
               return new CVBData(CVBType.OKX, _OKXBanksNames, _OKXCurrenciesNames);
            case CVBType.Bybit:
               return new CVBData(CVBType.Bybit, _OKXBanksNames, _OKXCurrenciesNames);
            default:
               return new CVBData();
         }
      }
   }
}
