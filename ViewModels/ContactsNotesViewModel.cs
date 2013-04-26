using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NotesByHand.CrmClasses;
using NotesByHand.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NotesByHand.ViewModels
{
    public delegate void LoadedEventHandler();

    class ContactsNotesViewModel
    {
        //private fields
        private ObservableCollection<Contact> _contacts;
        private List<string> _accounts = new List<string>();
        private bool isDataLoaded = false;

        public ObservableCollection<Contact> Contacts
        {
            get { return _contacts; }
            set { _contacts = value; }
        }

        public List<string> Accounts
        {
            get { return _accounts; }
            set { _accounts = value; }
        }

        public bool IsDataLoaded
        {
            get { return isDataLoaded; }
            set { isDataLoaded = value; }
        }

        //events
        public event LoadedEventHandler Loaded;

        //methods
        public async void LoadContactsAsync()
        {
            var fetch = new FetchExpression(
                @"<fetch distinct='false'>
                    <entity name='contact'>
                        <attribute name='fullname' />
                        <attribute name='parentcustomerid' />
                        <attribute name='telephone1' />
                        <attribute name='emailaddress1' />
                        <attribute name='contactid' />
                        <order attribute='fullname' descending='false' />
                        <filter type='and'>
                            <condition attribute='statecode' operator='eq' value='0' />
                        </filter>
                    </entity>
                </fetch>");

            Contacts = await CrmConnector.OrgServiceProxy.RetrieveMultipleAsync<Contact>(fetch);

            foreach (Contact c in Contacts)
            {
                Accounts.Add(c.ParentCustomerId.Name);
            }
            
            this.IsDataLoaded = true;
            Loaded();
        }

        public int GetContactById(string Id)
        {
            if (Contacts == null)
            {
                throw new InvalidOperationException(@"Cannot retrieve a contact from the 
                    ContactsNotesViewModel.Contacts property when that property is null.");
            }

            for (int i = 0; i < Contacts.Count; i++)
            {
                if (Contacts[i].ContactId.Value.ToString() == Id)
                {
                    return i;
                }
            }

            throw new InvalidOperationException(@"Cannot retrieve a contact from the 
                    ContactsNotesViewModel.Contacts property because no contact with 
                    the specified ContactId exists.");
        }

        public void RequestSingleContact(string username, string password, string orgName, string id)
        {
            
        }

        public async void PostNote(Contact contact, string noteText)
        {
            var note = new Annotation();
            note.NoteText = noteText;
            note.ObjectId = new EntityReference("contact", contact.Id);

            await CrmConnector.OrgServiceProxy.CreateAsync(note);
        }

        public async void UpdateContact(Contact contact)
        {
            CrmConnector.OrgServiceProxy.Update(contact);
        }
    }
}
