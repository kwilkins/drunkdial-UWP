
namespace DrunkDial
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.Contacts;
    using Windows.UI.Xaml.Media.Imaging;

    public class DDContact
    {
        public static async Task<DDContact> CreateFromContact(Contact contact)
        {
            Uri contactPlaceholder = new Uri("ms-appx:///Assets/contactPlaceholder.png");
            var image = new BitmapImage(contactPlaceholder);
            if (contact.Thumbnail != null)
            {
                var imageStream = await contact.Thumbnail.OpenReadAsync();
                await image.SetSourceAsync(imageStream);
            }

            return new DDContact()
            {
                Name = contact.FirstName,
                Thumbnail = image,
                Id = contact.Id,
                Phone = contact.Phones.First(c => c.Kind == ContactPhoneKind.Mobile).Number
            };
        }

        public string Name { get; set; }
        public BitmapImage Thumbnail { get; set; }
        public string Phone { get; set; }
        public string Id { get; set; }
    }
}
