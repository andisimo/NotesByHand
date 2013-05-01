using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NotesByHand.Utilities
{
    public static class ExtensionMethods
    {
        public static async Task<AuthenticationCredentials> AuthenticateAsync(
            this OrganizationServiceConfiguration orgSvcConfig, AuthenticationCredentials credentials)
        {
            var tcs = new TaskCompletionSource<AuthenticationCredentials>();
            EventHandler<AuthenticationCompletedEventArgs> handler = (o, args) =>
            {
                if (args.Error != null) tcs.TrySetException(args.Error);
                else if (args.Cancelled) tcs.TrySetCanceled();
                else tcs.TrySetResult(args.Result);
            };

            orgSvcConfig.AuthenticationComplete += handler;
            orgSvcConfig.Authenticate(credentials);
            var authenticationCredentials = await tcs.Task;
            orgSvcConfig.AuthenticationComplete -= handler;

            return authenticationCredentials;
        }

        public static async Task<Guid> CreateAsync(this OrganizationServiceProxy organizationServiceProxy, Entity entity)
        {
            var tcs = new TaskCompletionSource<Guid>();
            EventHandler<CreateCompletedEventArgs> handler = (o, args) =>
            {
                if (args.Error != null) tcs.TrySetException(args.Error);
                else if (args.Cancelled) tcs.TrySetCanceled();
                else tcs.TrySetResult(args.Result);
            };

            organizationServiceProxy.CreateCompleted += handler;
            organizationServiceProxy.Create(entity);
            var guid = await tcs.Task;
            organizationServiceProxy.CreateCompleted -= handler;

            return guid;
        }

        public static async Task<EntityCollection> RetrieveMultipleAsync(this OrganizationServiceProxy organizationServiceProxy, QueryBase query)
        {
            var tcs = new TaskCompletionSource<EntityCollection>();
            EventHandler<RetrieveMultipleCompletedEventArgs> handler = (o, args) =>
            {
                if (args.Error != null) tcs.TrySetException(args.Error);
                else if (args.Cancelled) tcs.TrySetCanceled();
                else tcs.TrySetResult(args.Result);
            };

            organizationServiceProxy.RetrieveMultipleCompleted += handler;
            organizationServiceProxy.RetrieveMultiple(query);
            var entityCollection = await tcs.Task;
            organizationServiceProxy.RetrieveMultipleCompleted -= handler;

            return entityCollection;
        }

        public static async Task<ObservableCollection<T>> RetrieveMultipleAsync<T>(this OrganizationServiceProxy organizationServiceProxy, QueryBase query) where T : Entity
        {
            var tcs = new TaskCompletionSource<ObservableCollection<T>>();
            EventHandler<RetrieveMultipleCompletedEventArgs> handler = (o, args) =>
            {
                if (args.Error != null) tcs.TrySetException(args.Error);
                else if (args.Cancelled) tcs.TrySetCanceled();
                else
                {
                    var observableCollection = new ObservableCollection<T>();
                    var entityCount = args.Result.Entities.Count;
                    for (int i = 0; i < entityCount; i++)
                    {
                        observableCollection.Add(args.Result[i].ToEntity<T>());
                    }
                    tcs.TrySetResult(observableCollection);
                }
            };

            organizationServiceProxy.RetrieveMultipleCompleted += handler;
            organizationServiceProxy.RetrieveMultiple(query);
            var collection = await tcs.Task;
            organizationServiceProxy.RetrieveMultipleCompleted -= handler;

            return collection;
        }
    }
}
