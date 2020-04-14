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

namespace JKXAF.Module.BusinessObjects
{
    [DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]

    public class Meeting : Event
    {
     public Meeting(Session session)
            : base(session)
        {
        }


        Contact osoba;

        Customer klient;

        [Association]
        public Customer Klient
        {
            get => klient;
            set => SetPropertyValue(nameof(Klient), ref klient, value);
        }


        [DataSourceProperty("Klient.Kontakty")]
        public Contact Osoba
        {
            get => osoba;
            set => SetPropertyValue(nameof(Osoba), ref osoba, value);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

        }

    }
}