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
      private static readonly string[] _binanceBanksNames = { "TinkoffNew", "RosBankNew", "RaiffeisenBank", "QIWI", "YandexMoneyNew" };
      private static readonly string[] _OKXBanksNames =     { "Tinkoff",    "Rosbank",    "Raiffaizen",     "QiWi", "Yandex.Money" };
      private static readonly string[] _binanceCurrenciesNames = { "USDT", "BTC", "BUSD", "BNB", "ETH" };
      private static readonly string[] _OKXCurrenciesNames = {     "USDT", "BTC",                "ETH" };
      public static readonly string TimePos = "Общая информация:E:4";
      public static readonly string SettingsPos = "Общая информация:F:4";
      public static readonly Dictionary<string, string> SimpleTablesRanges = new()
      { 
         { "Rates|Binance|",                                  "Общая информация:B:10"       },
         { "MarketRates|Binance|",                            "Общая информация:B:17"       },
         { "Convert|Binance|",                                "Общая информация:B:20"       },
         { "Rates|OKX|",                                      "Общая информация:B:30"       },
         { "MarketRates|OKX|",                                "Общая информация:B:35"       },
         { "Convert|OKX|",                                    "Общая информация:B:38"       },

         { "BankOnlyMaker|Binance|TinkoffNew",                "Binance простые связки:K:4"  },
         { "BankOnlyMaker|Binance|RosBankNew",                "Binance простые связки:K:12" },
         { "BankOnlyMaker|Binance|RaiffeisenBank",      "Binance простые связки:K:20" },
         { "BankOnlyMaker|Binance|QIWI",                      "Binance простые связки:K:28" },
         { "BankOnlyMaker|Binance|YandexMoneyNew",            "Binance простые связки:K:36" },
         { "CurrencyOnlyMaker|Binance|USDT",                  "Binance простые связки:C:4"  },
         { "CurrencyOnlyMaker|Binance|BTC",                   "Binance простые связки:C:12" },
         { "CurrencyOnlyMaker|Binance|BUSD",                  "Binance простые связки:C:20" },
         { "CurrencyOnlyMaker|Binance|BNB",                   "Binance простые связки:C:28" },
         { "CurrencyOnlyMaker|Binance|ETH",                   "Binance простые связки:C:36" },

         { "BankWithTaker|Binance|TinkoffNew",                "Binance простые связки:K:45" },
         { "BankWithTaker|Binance|RosBankNew",                "Binance простые связки:K:53" },
         { "BankWithTaker|Binance|RaiffeisenBank",      "Binance простые связки:K:61" },
         { "BankWithTaker|Binance|QIWI",                      "Binance простые связки:K:69" },
         { "BankWithTaker|Binance|YandexMoneyNew",            "Binance простые связки:K:77" },
         { "CurrencyWithTaker|Binance|USDT",                  "Binance простые связки:C:45" },
         { "CurrencyWithTaker|Binance|BTC",                   "Binance простые связки:C:53" },
         { "CurrencyWithTaker|Binance|BUSD",                  "Binance простые связки:C:61" },
         { "CurrencyWithTaker|Binance|BNB",                   "Binance простые связки:C:69" },
         { "CurrencyWithTaker|Binance|ETH",                   "Binance простые связки:C:77" },

         { "BankOnlyMaker|OKX|Tinkoff",                       "OKX простые связки:K:4"      },
         { "BankOnlyMaker|OKX|Rosbank",                       "OKX простые связки:K:10"     },
         { "BankOnlyMaker|OKX|Raiffaizen",                    "OKX простые связки:K:16"     },
         { "BankOnlyMaker|OKX|QiWi",                          "OKX простые связки:K:22"     },
         { "BankOnlyMaker|OKX|Yandex.Money",                  "OKX простые связки:K:28"     },
         { "CurrencyOnlyMaker|OKX|USDT",                      "OKX простые связки:C:4"      },
         { "CurrencyOnlyMaker|OKX|BTC",                       "OKX простые связки:C:12"     },
         { "CurrencyOnlyMaker|OKX|ETH",                       "OKX простые связки:C:20"     },
      };

      public static readonly Dictionary<string, string> HardTablesRanges = new()
      {
         { "HardLinksOnlyMaker|Binance|Binance|TinkoffNew|USDT",           "Binance сложные связки:D:4"   },
         { "HardLinksOnlyMaker|Binance|Binance|TinkoffNew|BTC",            "Binance сложные связки:M:4"   },
         { "HardLinksOnlyMaker|Binance|Binance|TinkoffNew|BUSD",           "Binance сложные связки:D:12"  },
         { "HardLinksOnlyMaker|Binance|Binance|TinkoffNew|BNB",            "Binance сложные связки:M:12"  },
         { "HardLinksOnlyMaker|Binance|Binance|TinkoffNew|ETH",            "Binance сложные связки:D:20"  },
         { "HardLinksOnlyMaker|Binance|Binance|RosBankNew|USDT",           "Binance сложные связки:D:30"  },
         { "HardLinksOnlyMaker|Binance|Binance|RosBankNew|BTC",            "Binance сложные связки:M:30"  },
         { "HardLinksOnlyMaker|Binance|Binance|RosBankNew|BUSD",           "Binance сложные связки:D:38"  },
         { "HardLinksOnlyMaker|Binance|Binance|RosBankNew|BNB",            "Binance сложные связки:M:38"  },
         { "HardLinksOnlyMaker|Binance|Binance|RosBankNew|ETH",            "Binance сложные связки:D:46"  },
         { "HardLinksOnlyMaker|Binance|Binance|RaiffeisenBank|USDT",       "Binance сложные связки:D:56"  },
         { "HardLinksOnlyMaker|Binance|Binance|RaiffeisenBank|BTC",        "Binance сложные связки:M:56"  },
         { "HardLinksOnlyMaker|Binance|Binance|RaiffeisenBank|BUSD",       "Binance сложные связки:D:64"  },
         { "HardLinksOnlyMaker|Binance|Binance|RaiffeisenBank|BNB",        "Binance сложные связки:M:64"  },
         { "HardLinksOnlyMaker|Binance|Binance|RaiffeisenBank|ETH",        "Binance сложные связки:D:72"  },
         { "HardLinksOnlyMaker|Binance|Binance|QIWI|USDT",                 "Binance сложные связки:D:82"  },
         { "HardLinksOnlyMaker|Binance|Binance|QIWI|BTC",                  "Binance сложные связки:M:82"  },
         { "HardLinksOnlyMaker|Binance|Binance|QIWI|BUSD",                 "Binance сложные связки:D:90"  },
         { "HardLinksOnlyMaker|Binance|Binance|QIWI|BNB",                  "Binance сложные связки:M:90"  },
         { "HardLinksOnlyMaker|Binance|Binance|QIWI|ETH",                  "Binance сложные связки:D:98"  },
         { "HardLinksOnlyMaker|Binance|Binance|YandexMoneyNew|USDT",       "Binance сложные связки:D:108" },
         { "HardLinksOnlyMaker|Binance|Binance|YandexMoneyNew|BTC",        "Binance сложные связки:M:108" },
         { "HardLinksOnlyMaker|Binance|Binance|YandexMoneyNew|BUSD",       "Binance сложные связки:D:116" },
         { "HardLinksOnlyMaker|Binance|Binance|YandexMoneyNew|BNB",        "Binance сложные связки:M:116" },
         { "HardLinksOnlyMaker|Binance|Binance|YandexMoneyNew|ETH",        "Binance сложные связки:D:124" },

         { "HardLinksWithTaker|Binance|Binance|TinkoffNew|USDT",           "Binance сложные связки:D:133" },
         { "HardLinksWithTaker|Binance|Binance|TinkoffNew|BTC",            "Binance сложные связки:M:133" },
         { "HardLinksWithTaker|Binance|Binance|TinkoffNew|BUSD",           "Binance сложные связки:D:141" },
         { "HardLinksWithTaker|Binance|Binance|TinkoffNew|BNB",            "Binance сложные связки:M:141" },
         { "HardLinksWithTaker|Binance|Binance|TinkoffNew|ETH",            "Binance сложные связки:D:149" },
         { "HardLinksWithTaker|Binance|Binance|RosBankNew|USDT",           "Binance сложные связки:D:159" },
         { "HardLinksWithTaker|Binance|Binance|RosBankNew|BTC",            "Binance сложные связки:M:159" },
         { "HardLinksWithTaker|Binance|Binance|RosBankNew|BUSD",           "Binance сложные связки:D:167" },
         { "HardLinksWithTaker|Binance|Binance|RosBankNew|BNB",            "Binance сложные связки:M:167" },
         { "HardLinksWithTaker|Binance|Binance|RosBankNew|ETH",            "Binance сложные связки:D:175" },
         { "HardLinksWithTaker|Binance|Binance|RaiffeisenBank|USDT",       "Binance сложные связки:D:185" },
         { "HardLinksWithTaker|Binance|Binance|RaiffeisenBank|BTC",        "Binance сложные связки:M:185" },
         { "HardLinksWithTaker|Binance|Binance|RaiffeisenBank|BUSD",       "Binance сложные связки:D:193" },
         { "HardLinksWithTaker|Binance|Binance|RaiffeisenBank|BNB",        "Binance сложные связки:M:193" },
         { "HardLinksWithTaker|Binance|Binance|RaiffeisenBank|ETH",        "Binance сложные связки:D:201" },
         { "HardLinksWithTaker|Binance|Binance|QIWI|USDT",                 "Binance сложные связки:D:211" },
         { "HardLinksWithTaker|Binance|Binance|QIWI|BTC",                  "Binance сложные связки:M:211" },
         { "HardLinksWithTaker|Binance|Binance|QIWI|BUSD",                 "Binance сложные связки:D:219" },
         { "HardLinksWithTaker|Binance|Binance|QIWI|BNB",                  "Binance сложные связки:M:219" },
         { "HardLinksWithTaker|Binance|Binance|QIWI|ETH",                  "Binance сложные связки:D:227" },
         { "HardLinksWithTaker|Binance|Binance|YandexMoneyNew|USDT",       "Binance сложные связки:D:237" },
         { "HardLinksWithTaker|Binance|Binance|YandexMoneyNew|BTC",        "Binance сложные связки:M:237" },
         { "HardLinksWithTaker|Binance|Binance|YandexMoneyNew|BUSD",       "Binance сложные связки:D:245" },
         { "HardLinksWithTaker|Binance|Binance|YandexMoneyNew|BNB",        "Binance сложные связки:M:245" },
         { "HardLinksWithTaker|Binance|Binance|YandexMoneyNew|ETH",        "Binance сложные связки:D:253" },

         { "HardLinksOnlyMaker|OKX|OKX|Tinkoff|USDT",                      "OKX сложные связки:D:4"       },
         { "HardLinksOnlyMaker|OKX|OKX|Tinkoff|BTC",                       "OKX сложные связки:K:4"       },
         { "HardLinksOnlyMaker|OKX|OKX|Tinkoff|ETH",                       "OKX сложные связки:R:4"       },
         { "HardLinksOnlyMaker|OKX|OKX|Rosbank|USDT",                      "OKX сложные связки:D:14"      },
         { "HardLinksOnlyMaker|OKX|OKX|Rosbank|BTC",                       "OKX сложные связки:K:14"      },
         { "HardLinksOnlyMaker|OKX|OKX|Rosbank|ETH",                       "OKX сложные связки:R:14"      },
         { "HardLinksOnlyMaker|OKX|OKX|Raiffaizen|USDT",                   "OKX сложные связки:D:24"      },
         { "HardLinksOnlyMaker|OKX|OKX|Raiffaizen|BTC",                    "OKX сложные связки:K:24"      },
         { "HardLinksOnlyMaker|OKX|OKX|Raiffaizen|ETH",                    "OKX сложные связки:R:24"      },
         { "HardLinksOnlyMaker|OKX|OKX|QiWi|USDT",                         "OKX сложные связки:D:34"      },
         { "HardLinksOnlyMaker|OKX|OKX|QiWi|BTC",                          "OKX сложные связки:K:34"      },
         { "HardLinksOnlyMaker|OKX|OKX|QiWi|ETH",                          "OKX сложные связки:R:34"      },
         { "HardLinksOnlyMaker|OKX|OKX|Yandex.Money|USDT",                 "OKX сложные связки:D:44"      },
         { "HardLinksOnlyMaker|OKX|OKX|Yandex.Money|BTC",                  "OKX сложные связки:K:44"      },
         { "HardLinksOnlyMaker|OKX|OKX|Yandex.Money|ETH",                  "OKX сложные связки:R:44"      },

         { "HardLinksOnlyMaker|Binance|OKX|Tinkoff|USDT",                  "Binance->OKX:D:4"             },
         { "HardLinksOnlyMaker|Binance|OKX|Tinkoff|BTC",                   "Binance->OKX:K:4"             },
         { "HardLinksOnlyMaker|Binance|OKX|Tinkoff|BUSD",                  "Binance->OKX:R:4"             },
         { "HardLinksOnlyMaker|Binance|OKX|Tinkoff|BNB",                   "Binance->OKX:D:12"            },
         { "HardLinksOnlyMaker|Binance|OKX|Tinkoff|ETH",                   "Binance->OKX:K:12"            },
         { "HardLinksOnlyMaker|Binance|OKX|Rosbank|USDT",                  "Binance->OKX:D:22"            },
         { "HardLinksOnlyMaker|Binance|OKX|Rosbank|BTC",                   "Binance->OKX:K:22"            },
         { "HardLinksOnlyMaker|Binance|OKX|Rosbank|BUSD",                  "Binance->OKX:R:22"            },
         { "HardLinksOnlyMaker|Binance|OKX|Rosbank|BNB",                   "Binance->OKX:D:30"            },
         { "HardLinksOnlyMaker|Binance|OKX|Rosbank|ETH",                   "Binance->OKX:K:30"            },
         { "HardLinksOnlyMaker|Binance|OKX|Raiffaizen|USDT",               "Binance->OKX:D:40"            },
         { "HardLinksOnlyMaker|Binance|OKX|Raiffaizen|BTC",                "Binance->OKX:K:40"            },
         { "HardLinksOnlyMaker|Binance|OKX|Raiffaizen|BUSD",               "Binance->OKX:R:40"            },
         { "HardLinksOnlyMaker|Binance|OKX|Raiffaizen|BNB",                "Binance->OKX:D:48"            },
         { "HardLinksOnlyMaker|Binance|OKX|Raiffaizen|ETH",                "Binance->OKX:K:48"            },
         { "HardLinksOnlyMaker|Binance|OKX|QiWi|USDT",                     "Binance->OKX:D:58"            },
         { "HardLinksOnlyMaker|Binance|OKX|QiWi|BTC",                      "Binance->OKX:K:58"            },
         { "HardLinksOnlyMaker|Binance|OKX|QiWi|BUSD",                     "Binance->OKX:R:58"            },
         { "HardLinksOnlyMaker|Binance|OKX|QiWi|BNB",                      "Binance->OKX:D:66"            },
         { "HardLinksOnlyMaker|Binance|OKX|QiWi|ETH",                      "Binance->OKX:K:66"            },
         { "HardLinksOnlyMaker|Binance|OKX|Yandex.Money|USDT",             "Binance->OKX:D:76"            },
         { "HardLinksOnlyMaker|Binance|OKX|Yandex.Money|BTC",              "Binance->OKX:K:76"            },
         { "HardLinksOnlyMaker|Binance|OKX|Yandex.Money|BUSD",             "Binance->OKX:R:76"            },
         { "HardLinksOnlyMaker|Binance|OKX|Yandex.Money|BNB",              "Binance->OKX:D:84"            },
         { "HardLinksOnlyMaker|Binance|OKX|Yandex.Money|ETH",              "Binance->OKX:K:84"            },

         { "HardLinksWithTaker|Binance|OKX|Tinkoff|USDT",                  "Binance->OKX:D:93"            },
         { "HardLinksWithTaker|Binance|OKX|Tinkoff|BTC",                   "Binance->OKX:K:93"            },
         { "HardLinksWithTaker|Binance|OKX|Tinkoff|BUSD",                  "Binance->OKX:R:93"            },
         { "HardLinksWithTaker|Binance|OKX|Tinkoff|BNB",                   "Binance->OKX:D:101"           },
         { "HardLinksWithTaker|Binance|OKX|Tinkoff|ETH",                   "Binance->OKX:K:101"           },
         { "HardLinksWithTaker|Binance|OKX|Rosbank|USDT",                  "Binance->OKX:D:111"           },
         { "HardLinksWithTaker|Binance|OKX|Rosbank|BTC",                   "Binance->OKX:K:111"           },
         { "HardLinksWithTaker|Binance|OKX|Rosbank|BUSD",                  "Binance->OKX:R:111"           },
         { "HardLinksWithTaker|Binance|OKX|Rosbank|BNB",                   "Binance->OKX:D:119"           },
         { "HardLinksWithTaker|Binance|OKX|Rosbank|ETH",                   "Binance->OKX:K:119"           },
         { "HardLinksWithTaker|Binance|OKX|Raiffaizen|USDT",               "Binance->OKX:D:129"           },
         { "HardLinksWithTaker|Binance|OKX|Raiffaizen|BTC",                "Binance->OKX:K:129"           },
         { "HardLinksWithTaker|Binance|OKX|Raiffaizen|BUSD",               "Binance->OKX:R:129"           },
         { "HardLinksWithTaker|Binance|OKX|Raiffaizen|BNB",                "Binance->OKX:D:137"           },
         { "HardLinksWithTaker|Binance|OKX|Raiffaizen|ETH",                "Binance->OKX:K:137"           },
         { "HardLinksWithTaker|Binance|OKX|QiWi|USDT",                     "Binance->OKX:D:147"           },
         { "HardLinksWithTaker|Binance|OKX|QiWi|BTC",                      "Binance->OKX:K:147"           },
         { "HardLinksWithTaker|Binance|OKX|QiWi|BUSD",                     "Binance->OKX:R:147"           },
         { "HardLinksWithTaker|Binance|OKX|QiWi|BNB",                      "Binance->OKX:D:155"           },
         { "HardLinksWithTaker|Binance|OKX|QiWi|ETH",                      "Binance->OKX:K:155"           },
         { "HardLinksWithTaker|Binance|OKX|Yandex.Money|USDT",             "Binance->OKX:D:165"           },
         { "HardLinksWithTaker|Binance|OKX|Yandex.Money|BTC",              "Binance->OKX:K:165"           },
         { "HardLinksWithTaker|Binance|OKX|Yandex.Money|BUSD",             "Binance->OKX:R:165"           },
         { "HardLinksWithTaker|Binance|OKX|Yandex.Money|BNB",              "Binance->OKX:D:173"           },
         { "HardLinksWithTaker|Binance|OKX|Yandex.Money|ETH",              "Binance->OKX:K:173"           },

         { "HardLinksOnlyMaker|OKX|Binance|TinkoffNew|USDT",               "OKX->Binance:D:4"             },
         { "HardLinksOnlyMaker|OKX|Binance|TinkoffNew|BTC",                "OKX->Binance:M:4"             },
         { "HardLinksOnlyMaker|OKX|Binance|TinkoffNew|ETH",                "OKX->Binance:D:12"            },
         { "HardLinksOnlyMaker|OKX|Binance|RosBankNew|USDT",               "OKX->Binance:D:22"            },
         { "HardLinksOnlyMaker|OKX|Binance|RosBankNew|BTC",                "OKX->Binance:M:22"            },
         { "HardLinksOnlyMaker|OKX|Binance|RosBankNew|ETH",                "OKX->Binance:D:30"            },
         { "HardLinksOnlyMaker|OKX|Binance|RaiffeisenBank|USDT",           "OKX->Binance:D:40"            },
         { "HardLinksOnlyMaker|OKX|Binance|RaiffeisenBank|BTC",            "OKX->Binance:M:40"            },
         { "HardLinksOnlyMaker|OKX|Binance|RaiffeisenBank|ETH",            "OKX->Binance:D:48"            },
         { "HardLinksOnlyMaker|OKX|Binance|QIWI|USDT",                     "OKX->Binance:D:58"            },
         { "HardLinksOnlyMaker|OKX|Binance|QIWI|BTC",                      "OKX->Binance:M:58"            },
         { "HardLinksOnlyMaker|OKX|Binance|QIWI|ETH",                      "OKX->Binance:D:66"            },
         { "HardLinksOnlyMaker|OKX|Binance|YandexMoneyNew|USDT",           "OKX->Binance:D:76"            },
         { "HardLinksOnlyMaker|OKX|Binance|YandexMoneyNew|BTC",            "OKX->Binance:M:76"            },
         { "HardLinksOnlyMaker|OKX|Binance|YandexMoneyNew|ETH",            "OKX->Binance:D:84"            },
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
