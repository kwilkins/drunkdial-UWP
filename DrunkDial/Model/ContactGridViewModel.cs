
namespace DrunkDial
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    using Windows.Storage;
    using Windows.ApplicationModel.Contacts;
    using Windows.UI.Xaml.Media.Imaging;

    class ContactGridViewModel
    {
        private static ContactGridViewModel instance;

        private ContactGridViewModel()
        {
            DataSource = new ObservableCollection<DDContact>();

            this.LoadSavedContacts();

            this.DeleteCommand = new DeleteContactCommand();
        }

        public static ContactGridViewModel Instance
        {
            get
            {
                return instance ?? (instance = new ContactGridViewModel());
            }
        }

        public ObservableCollection<DDContact> DataSource { get; set; }

        public ICommand DeleteCommand { get; private set; }

        private readonly ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        private const string PinnedContactIds = "PINNED_CONTACT_IDS";

        public bool RemoveContactById(string id)
        {
            var result = DataSource.Remove(DataSource.FirstOrDefault(c => c.Id.Equals(id)));
            this.SaveContacts();
            return result;

        }

        private async void LoadSavedContacts()
        {
            var pinnedContactIdsString = localSettings.Values[PinnedContactIds];
            Debug.WriteLine("Read >{0}< from settings", pinnedContactIdsString);

            if (pinnedContactIdsString == null)
            {
                //no data saved, no work to do
                return;
            }

            var contactIdList = pinnedContactIdsString.ToString().Trim().Split(';').ToList();

            if (!contactIdList.Any())
            {
                //no actual contacts saved, no work to do
                return;
            }

            var store = await ContactManager.RequestStoreAsync(ContactStoreAccessType.AllContactsReadOnly);
            DataSource.Clear();
            foreach (var s in contactIdList.Where(s => s != null && s.Trim() != String.Empty))
            {
                var contact = await store.GetContactAsync(s);
                var ddContact = await DDContact.CreateFromContact(contact);
                this.DataSource.Add(ddContact);
            }
        }

        public void SaveContacts()
        {
            var stringBuilder = new StringBuilder();

            foreach (var contact in DataSource)
            {
                stringBuilder.Append(contact.Id).Append(";");
            }

            localSettings.Values[PinnedContactIds] = stringBuilder.ToString();
        }
    }
}
