using Bogus;
using JKXAF.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKXAF.Module.Utils
{
  public static  class DataGeneratorHelper
    {

       public static void GenerateCountries(IObjectSpace ObjectSpace)
        {

            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.FrameworkCultures))

            {

                RegionInfo ri = null;

                try

                {

                    ri = new RegionInfo(ci.Name);

                }

                catch

                {

                    // If a RegionInfo object could not be created we don't want to use the CultureInfo

                    //    for the country list.

                    continue;

                }

                // var kraj =    os.CreateObject<Kraj>();



                var a = ri.EnglishName;
                var a1 = ri.NativeName;
                var a2 = ri.ThreeLetterISORegionName;
                var a3 = ri.CurrencyEnglishName;
                var a4 = ri.CurrencyNativeName;
                var a5 = ri.CurrencySymbol;
                var a6 = ri.ISOCurrencySymbol;

                var waluta = ObjectSpace.FindObject<Currency>(new BinaryOperator("Symbol", ri.ISOCurrencySymbol));
                if (waluta == null)
                {
                    waluta = ObjectSpace.CreateObject<Currency>();
                    waluta.Symbol = ri.ISOCurrencySymbol;
                    waluta.Nazwa = ri.CurrencyEnglishName;
                    waluta.LokalnaNazwa = ri.CurrencyNativeName;
                    waluta.LokalnySymbol = ri.CurrencySymbol;
                }

                var kraj = ObjectSpace.FindObject<BusinessObjects.Country>(new BinaryOperator("Symbol", ri.ThreeLetterISORegionName));
                if (kraj == null)
                {
                    kraj = ObjectSpace.CreateObject<BusinessObjects.Country>();
                    kraj.Symbol = ri.ThreeLetterISORegionName;
                    kraj.Nazwa = ri.EnglishName;
                    kraj.LokalnySymbol = ri.TwoLetterISORegionName;
                    kraj.LokalnaNazwa = ri.NativeName;
                    kraj.GeoId = ri.GeoId;
                    kraj.Waluta = waluta;
                    kraj.IsMetric = ri.IsMetric;

                }
                waluta.Kraj = kraj;
            }
        }



        public static void GenerateCustomers(IObjectSpace ObjectSpace, int records)
        {

         var   consultants = ObjectSpace.GetObjectsQuery<Employee>().ToList();

            var cusFaker = new Faker<Customer>("pl")
              .CustomInstantiator(f => ObjectSpace.CreateObject<Customer>())
              .RuleFor(o => o.Telefon, f => f.Person.Phone)
              .RuleFor(o => o.Skrot, f => f.Company.CompanyName())
              .RuleFor(o => o.Nazwa, f => f.Company.CompanyName())
              .RuleFor(o => o.Email, (f, u) => f.Internet.Email())
              .RuleFor(o => o.Miejscowosc, f => f.Address.City())
              .RuleFor(o => o.KodPocztowy, f => f.Address.ZipCode())
              .RuleFor(o => o.Consultant, f => f.PickRandom(consultants))
              .RuleFor(o => o.Ulica, f => f.Address.StreetName());
            var customers = cusFaker.Generate(records);
            ObjectSpace.CommitChanges();

            var conFaker = new Faker<Contact>("pl")
            .CustomInstantiator(f => ObjectSpace.CreateObject<Contact>())
            .RuleFor(o => o.Imie, f => f.Person.FirstName)
            .RuleFor(o => o.Nazwisko, f => f.Person.LastName)
            .RuleFor(o => o.Klient, f => f.PickRandom(cusFaker))
            .RuleFor(o => o.Email, (f, u) => f.Internet.Email())
            .RuleFor(o => o.Telefon, f => f.Person.Phone);

            var contacts = conFaker.Generate(records);

            var stawki = new List<VATRate>();
            stawki.Add(NewRate(ObjectSpace, "23%", 23M));
            stawki.Add(NewRate(ObjectSpace, "0%", 0M));
            stawki.Add(NewRate(ObjectSpace, "7%", 7M));
            stawki.Add(NewRate(ObjectSpace, "ZW", 0M));



            var prodFaker = new Faker<Product>("pl")
             .CustomInstantiator(f => ObjectSpace.CreateObject<Product>())
             .RuleFor(o => o.Nazwa, f => f.Commerce.ProductName())
             .RuleFor(o => o.StawkaVAT, f => f.PickRandom(stawki))
             .RuleFor(o => o.Cena, f => f.Random.Decimal(0.01M, 1000M));

            var products = prodFaker.Generate(100);

        }



        private static VATRate NewRate(IObjectSpace ObjectSpace, string symbol, decimal wartosc)
        {

            var stawka = ObjectSpace.FindObject<VATRate>(new BinaryOperator("Symbol", symbol));
            if (stawka == null)
            {
                stawka = ObjectSpace.CreateObject<VATRate>();
                stawka.Symbol = symbol;
                stawka.Stawka = wartosc;

            }
            return stawka;
        }

        public static void GenerateInvoices(IObjectSpace ObjectSpace, int liczbaFaktur = 1000 , IList<Customer> customers = null, IList<Product> products = null)
        {
            if (customers is null)
            {
                customers = ObjectSpace.GetObjectsQuery<Customer>().ToList();
            }

            var orderFaker = new Faker<Invoice>("pl")
            .CustomInstantiator(f => ObjectSpace.CreateObject<Invoice>())
            .RuleFor(o => o.NumerFaktury, f => f.Random.Int().ToString())
            .RuleFor(o => o.DataFaktury, f => f.Date.Past(2))
            .RuleFor(o => o.DataSprzedazy, f => f.Date.Past(20))
            .RuleFor(o => o.Klient, f => f.PickRandom(customers));
            var orders = orderFaker.Generate(liczbaFaktur);
            if (products == null)
            {
                products = ObjectSpace.GetObjectsQuery<Product>().ToList();
            }

            var itemsFaker = new Faker<InvoiceItem>()
            .CustomInstantiator(f => ObjectSpace.CreateObject<InvoiceItem>())
            .RuleFor(o => o.Faktura, f => f.PickRandom(orders))
            .RuleFor(o => o.Produkt, f => f.PickRandom(products))

            .RuleFor(o => o.Ilosc, f => f.Random.Decimal(0.01M, 100M));

            var items = itemsFaker.Generate(liczbaFaktur * 10);
        }
    }
}
