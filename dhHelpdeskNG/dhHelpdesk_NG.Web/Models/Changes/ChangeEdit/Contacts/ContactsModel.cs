namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit.Contacts
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class ContactsModel
    {
        public ContactsModel()
        {
        }

        public ContactsModel(
            ContactModel contactOne,
            ContactModel contactTwo,
            ContactModel contactThree,
            ContactModel contactFourth,
            ContactModel contactFive,
            ContactModel contactSix)
        {
            this.ContactOne = contactOne;
            this.ContactTwo = contactTwo;
            this.ContactThree = contactThree;
            this.ContactFourth = contactFourth;
            this.ContactFive = contactFive;
            this.ContactSix = contactSix;
        }

        [NotNull]
        [LocalizedDisplay("Contact 1")]
        public ContactModel ContactOne { get; set; }

        [NotNull]
        [LocalizedDisplay("Contact 2")]
        public ContactModel ContactTwo { get; set; }

        [NotNull]
        [LocalizedDisplay("Contact 3")]
        public ContactModel ContactThree { get; set; }

        [NotNull]
        [LocalizedDisplay("Contact 4")]
        public ContactModel ContactFourth { get; set; }

        [NotNull]
        [LocalizedDisplay("Contact 5")]
        public ContactModel ContactFive { get; set; }

        [NotNull]
        [LocalizedDisplay("Contact 6")]
        public ContactModel ContactSix { get; set; }

        public bool IsUnshowable()
        {
            return this.ContactOne.IsUnshowable() &&
                this.ContactTwo.IsUnshowable() &&
                this.ContactThree.IsUnshowable() &&
                this.ContactFourth.IsUnshowable() &&
                this.ContactFive.IsUnshowable() &&
                this.ContactSix.IsUnshowable();
        }
    }
}