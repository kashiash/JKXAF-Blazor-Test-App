using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using Bogus;

namespace JKXAF.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Customer")]
    [DefaultProperty(nameof(Skrot))]
 public class Customer : BaseObject
    { 
      public Customer(Session session)
            : base(session)
        {
        }



        string test;
        Employee consultant;
        int terminPlatnosci;
        string miejscowosc;
        string kodPocztowy;
        string ulica;
        string telefon;
        string email;
        string skrot;
        string nazwa;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Nazwa
        {
            get => nazwa;
            set => SetPropertyValue(nameof(Nazwa), ref nazwa, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]

        [RuleUniqueValue]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Skrot
        {
            get => skrot;
            set => SetPropertyValue(nameof(Skrot), ref skrot, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Email
        {
            get => email;
            set => SetPropertyValue(nameof(Email), ref email, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Telefon
        {
            get => telefon;
            set => SetPropertyValue(nameof(Telefon), ref telefon, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Ulica
        {
            get => ulica;
            set => SetPropertyValue(nameof(Ulica), ref ulica, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string KodPocztowy
        {
            get => kodPocztowy;
            set => SetPropertyValue(nameof(KodPocztowy), ref kodPocztowy, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Miejscowosc
        {
            get => miejscowosc;
            set => SetPropertyValue(nameof(Miejscowosc), ref miejscowosc, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Test
        {
            get => test;
            set => SetPropertyValue(nameof(Test), ref test, value);
        }

        public int TerminPlatnosci
        {
            get => terminPlatnosci;
            set => SetPropertyValue(nameof(TerminPlatnosci), ref terminPlatnosci, value);
        }

        [Association]
        public Employee Consultant
        {
            get => consultant;
            set => SetPropertyValue(nameof(Consultant), ref consultant, value);
        }


        [XafDisplayName("Kontakty")]
        [Association("Klient-Kontakty"), DevExpress.Xpo.Aggregated]
        public XPCollection<Contact> Kontakty
        {
            get
            {
                return GetCollection<Contact>(nameof(Kontakty));
            }
        }

        [Association]
        public XPCollection<Meeting> Spotkania
        {
            get
            {
                return GetCollection<Meeting>(nameof(Spotkania));
            }
        }

        [Association]
        public XPCollection<Invoice> Faktury
        {
            get
            {
                return GetCollection<Invoice>(nameof(Faktury));
            }
        }

        [Association]
        public XPCollection<Payment> Wplaty
        {
            get
            {
                return GetCollection<Payment>(nameof(Wplaty));
            }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            TerminPlatnosci = 14;
        }



    }
}