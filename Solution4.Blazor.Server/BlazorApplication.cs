using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;

namespace JKXAF.Blazor.Server {
    public partial class Solution4BlazorApplication : BlazorApplication {
        public Solution4BlazorApplication() {
            InitializeComponent();
            SetupAuthentication();
            ConnectionString = @"Integrated Security=SSPI;Pooling=false;Data Source=(localdb)\mssqllocaldb;Initial Catalog=Solution4";
        }
        private void SetupAuthentication() {
            this.authenticationMixed1.LogonParametersType = typeof(DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters);
            this.authenticationMixed1.IsSupportChangePassword = true;
            this.authenticationMixed1.AddAuthenticationStandardProvider(typeof(DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser));
            this.authenticationMixed1.AddIdentityAuthenticationProvider(typeof(DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser));
        }
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            args.ObjectSpaceProviders.Add(new SecuredObjectSpaceProvider((ISelectDataSecurityProvider)Security, args.ConnectionString, null, true));
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
        }
        private void Solution4BlazorApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e) {
            e.Updater.Update();
            e.Handled = true;
        }
    }
}
