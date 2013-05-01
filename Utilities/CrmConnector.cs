using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;

namespace NotesByHand.Utilities
{
    public delegate void ConnectedEventHandler();

    internal static class CrmConnector
    {
        //private fields
        private static OrganizationServiceConfiguration _svcConfig;
        private static AuthenticationCredentials _authCreds = new AuthenticationCredentials();
        
        //properties
        internal static string OrgName { get; set; }
        public static OrganizationServiceProxy OrgServiceProxy { get; set; }
        private static AuthenticationCredentials AuthCreds 
        {
            get { return _authCreds; }
            set { _authCreds = value; }
        }
        public static bool IsConnected { get; set; }

        //events
        public static event ConnectedEventHandler Connected;

        //methods
        public static void Connect(AuthenticationCredentials authCreds)
        {
            AuthCreds = authCreds;
            OrgName = ParseOrgName(authCreds);

            _svcConfig =
                ServiceConfigurationFactory.CreateConfiguration<IOrganizationService>(
                new Uri(@"https://" + OrgName + ".crm.dynamics.com/XRMServices/2011/Organization.svc"),
                (sender, args) => MetadataLoaded(sender, args)
                ) as OrganizationServiceConfiguration;
        }

        private static string ParseOrgName(AuthenticationCredentials authCreds)
        {
            var userName = authCreds.ClientCredentials.UserName.UserName;
            int orgStartIndex = userName.IndexOf("@") + 1;
            int orgLength = userName.IndexOf(".", orgStartIndex) - orgStartIndex;
            var orgName = userName.Substring(orgStartIndex, orgLength);

            return orgName;
        }

        private static void MetadataLoaded(object sender, ServiceMetadataLoadedEventArgs e)
        {
            LogonAsync();
        }

        private static async void LogonAsync()
        {
            await _svcConfig.AuthenticateAsync(AuthCreds);
            var isvcConfig = _svcConfig as IServiceConfiguration<IOrganizationService>;

            try
            {
                OrgServiceProxy = new OrganizationServiceProxy(
                                                    isvcConfig,
                                                    AuthCreds.SecurityTokenResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            IsConnected = true;
            Connected();
        }
    }
}
