using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKXAF.Module.BusinessObjects
{
    [ImageName("BO_Invoice")]
    [DefaultClassOptions]


    // kolorowanie rekordów
   //  [Appearance("FakturyZatwierdzone", Criteria = "Status = ##Enum#Demo1.Module.BusinessObjects.StatusFaktury,Zatwierdzona#", TargetItems = "*", FontColor = "Blue")]
  //  [Appearance("FakturyAnulowane", Criteria = "Status = ##Enum#Demo1.Module.BusinessObjects.StatusFaktury,Anulowana#", TargetItems = "*", FontColor = "Gray")]
    public class Invoice : XPObject
    {
        public Invoice(Session session) : base(session)
        { }



        StatusFaktury status;
        string numerFaktry;
        Customer klient;
        DateTime dataPlatnosci;
        DateTime dataSprzedazy;
        DateTime dataFaktury;



        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleRequiredField]
        [RuleUniqueValue]
        public string NumerFaktury

        {
            get => numerFaktry;
            set => SetPropertyValue(nameof(NumerFaktury), ref numerFaktry, value);
        }
        public DateTime DataFaktury
        {
            get => dataFaktury;
            set
            {

                var modified = SetPropertyValue(nameof(DataFaktury), ref dataFaktury, value);

                if (modified && !IsLoading && !IsSaving && Klient != null)
                {
                    DataPlatnosci = DataFaktury.AddDays(Klient.TerminPlatnosci);
                }
            }
        }

        public DateTime DataSprzedazy
        {
            get => dataSprzedazy;
            set => SetPropertyValue(nameof(DataSprzedazy), ref dataSprzedazy, value);
        }

        public DateTime DataPlatnosci
        {
            get => dataPlatnosci;
            set => SetPropertyValue(nameof(DataPlatnosci), ref dataPlatnosci, value);
        }


        [Association]

        public Customer Klient
        {
            get => klient;
            set
            {
                var modified = SetPropertyValue(nameof(Klient), ref klient, value);

                if (modified && !IsLoading && !IsSaving && Klient != null)
                {
                    DataPlatnosci = DataFaktury.AddDays(Klient.TerminPlatnosci);
                }
            }
        }


        [Persistent(nameof(WartoscNetto))]
        decimal wartoscNetto;
        [PersistentAlias(nameof(wartoscNetto))]
        public decimal WartoscNetto
        {
            get => wartoscNetto;
            set => SetPropertyValue(nameof(WartoscNetto), ref wartoscNetto, value);
        }
        [Persistent("WartoscVAT")]
        decimal? wartoscVAT;
        [PersistentAlias(nameof(wartoscVAT))]
        public decimal? WartoscVAT
        {
            get => wartoscVAT;
            set => SetPropertyValue(nameof(WartoscVAT), ref wartoscVAT, value);
        }
        [Persistent("WartoscBrutto")]
        decimal? wartoscBrutto;
        [PersistentAlias(nameof(wartoscBrutto))]
        public decimal? WartoscBrutto
        {
            get => wartoscBrutto;
            set => SetPropertyValue(nameof(WartoscBrutto), ref wartoscBrutto, value);
        }

        [Association, Aggregated]
        public XPCollection<InvoiceItem> PozycjeFaktury
        {
            get
            {
                return GetCollection<InvoiceItem>(nameof(PozycjeFaktury));
            }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            // ustawiamy wartości poczatkowe
            // Opowiednik w clarionie "On prime records"
            DataFaktury = DateTime.Now;
            Status = StatusFaktury.Przygotowana;

        }

        public void PrzeliczSumy(bool forceChangeEvents)
        {
            decimal oldWartoscNetto = wartoscNetto;
            decimal? oldWartoscVAT = wartoscVAT;
            decimal? oldWartoscBrutto = wartoscBrutto;


            decimal tmpWartoscNetto = 0m;
            decimal? tmpWartoscVAT = 0m;
            decimal? tmpWartoscBrutto = 0m;

            foreach (InvoiceItem rec in PozycjeFaktury)
            {
                tmpWartoscNetto += rec.WartoscNetto;
                tmpWartoscVAT += rec.WartoscVAT;
                tmpWartoscBrutto += rec.WartoscBrutto;
            }
            wartoscNetto = tmpWartoscNetto;
            wartoscVAT = tmpWartoscVAT;
            wartoscBrutto = tmpWartoscBrutto;

            if (forceChangeEvents)
            {
                OnChanged(nameof(WartoscNetto), oldWartoscNetto, wartoscNetto);
                OnChanged(nameof(WartoscVAT), oldWartoscVAT, wartoscVAT);
                OnChanged(nameof(WartoscBrutto), oldWartoscBrutto, wartoscBrutto);
            }
        }

        
        public StatusFaktury Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }


        [Action]
        public void Zatwierdz()
        {
            Status = StatusFaktury.Zatwierdzona;

        }

        [Action]
        public void Anuluj()
        {
            Status = StatusFaktury.Anulowana;
        }
    }

    public enum StatusFaktury
    {
        Przygotowana = 0, Zatwierdzona = 1, Anulowana = 9
    }
}
