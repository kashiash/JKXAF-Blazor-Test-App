﻿using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKXAF.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(Symbol))]
    public class Currency : XPCustomObject
	{
		public Currency(Session session) : base(session)
		{ }


        Country kraj;
        string symbol;
        string nazwa;


        [Size(3)]
        [Key]
        public string Symbol
        {
            get => symbol;
            set => SetPropertyValue(nameof(Symbol), ref symbol, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Nazwa
        {
            get => nazwa;
            set => SetPropertyValue(nameof(Nazwa), ref nazwa, value);
        }

        public Country Kraj
        {
            get => kraj;
            set => SetPropertyValue(nameof(Kraj), ref kraj, value);
        }

        string lokalnaNazwa;
        string lokalnySymbol;
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LokalnySymbol
        {
            get => lokalnySymbol;
            set => SetPropertyValue(nameof(LokalnySymbol), ref lokalnySymbol, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string LokalnaNazwa
        {
            get => lokalnaNazwa;
            set => SetPropertyValue(nameof(LokalnaNazwa), ref lokalnaNazwa, value);
        }
    }
}
