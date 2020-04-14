using DevExpress.Persistent.Base;
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
    [DefaultProperty(nameof(Title))]
	public class JobTitle : XPObject
	{
		public JobTitle(Session session) : base(session)
		{ }


        string nazwa;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Title
        {
            get => nazwa;
            set => SetPropertyValue(nameof(Title), ref nazwa, value);
        }
    }
}
