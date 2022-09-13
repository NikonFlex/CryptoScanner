﻿namespace CryptoParser.Models
{
   public static class Utils
   {
      public static CVBType GetCVBTypeFrom(string cvb)
      {
         switch (cvb)
         {
            case "Binance": return CVBType.Binance;
            case "OKX":     return CVBType.OKX;
            case "Huobi":   return CVBType.Huobi;
            default:        throw new Exception($"Cannot convert {cvb} to cvb");
         }
      }

      public static SpreadType GetSpreadTypeFrom(string type)
      {
         switch (type)
         {
            case "РУБ": return SpreadType.Rub;
            case "%":   return SpreadType.Percent;
            default:    throw new Exception($"Cannot convert {type} to cvb");
         }
      }

      public static Bank GetBankTypeFrom(string bank)
      {
         switch (bank)
         {
            case "Tinkoff":     return Bank.Tinkoff;
            case "Sberbank":    return Bank.Sberbank;
            case "Raiffaisen":  return Bank.Raiffaisen;
            case "QIWI":        return Bank.QIWI;
            case "YandexMoney": return Bank.YandexMoney;
            default:            return Bank.Tinkoff;
         }
      }

      public static Currency GetCurrencyTypeFrom(string currency)
      {
         switch (currency)
         {
            case "USDT": return Currency.USDT;
            case "BTC":  return Currency.BTC;
            case "BUSD": return Currency.BUSD;
            case "BNB":  return Currency.BNB;
            case "ETH":  return Currency.ETH;
            default:     return Currency.USDT;
         }
      }

      public static string GetBankNameFrom(Bank bank, CVBType cvb)
      {
         if (cvb == CVBType.Binance)
         {
            switch (bank)
            {
               case Bank.Tinkoff:     return "TinkoffNew";
               case Bank.Sberbank:    return "RosBankNew";
               case Bank.Raiffaisen:  return "RaiffeisenBank";
               case Bank.QIWI:        return "QIWI";
               case Bank.YandexMoney: return "YandexMoneyNew";
               default:               return "No such bank";
            }
         }
         if (cvb == CVBType.OKX)
         {
            switch (bank)
            {
               case Bank.Tinkoff:     return "Tinkoff";
               case Bank.Sberbank:    return "Sberbank";
               case Bank.Raiffaisen:  return "Raiffaizen";
               case Bank.QIWI:        return "QiWi";
               case Bank.YandexMoney: return "Yandex.Money";
               default:               return "No such bank";
            }
         }
         else //huobi
         {
            switch (bank)
            {
               case Bank.Tinkoff:     return "28";
               case Bank.Sberbank:    return "29";
               case Bank.Raiffaisen:  return "36";
               case Bank.QIWI:        return "9";
               case Bank.YandexMoney: return "19";
               default:               return "No such bank";
            }
         }
      }

      public static string GetCurrencyNameFrom(Currency currency, CVBType cvb)
      {
         if (cvb == CVBType.Binance)
         {
            switch (currency)
            {
               case Currency.USDT: return "USDT";
               case Currency.BTC:  return "BTC";
               case Currency.BUSD: return "BUSD";
               case Currency.BNB:  return "BNB";
               case Currency.ETH:  return "ETH";
               default:            return "No such currency";
            }
         }
         if (cvb == CVBType.OKX)
         {
            switch (currency)
            {
               case Currency.USDT: return "USDT";
               case Currency.BTC:  return "BTC";
               case Currency.BUSD: return "No such currency";
               case Currency.BNB:  return "No such currency";
               case Currency.ETH:  return "ETH";
               default:            return "No such currency";
            }
         }
         else //huobi
         {
            switch (currency)
            {
               case Currency.USDT: return "2";
               case Currency.BTC:  return "1";
               case Currency.BUSD: return "No such currency";
               case Currency.BNB:  return "No such currency";
               case Currency.ETH:  return "3";
               default:            return "No such currency";
            }
         }
      }
   }
}