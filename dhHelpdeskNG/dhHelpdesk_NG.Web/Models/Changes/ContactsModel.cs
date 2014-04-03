namespace DH.Helpdesk.Web.Models.Changes
{
    using DH.Helpdesk.Common.ValidationAttributes;

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
        public ContactModel ContactOne { get; set; }

        [NotNull]
        public ContactModel ContactTwo { get; set; }

        [NotNull]
        public ContactModel ContactThree { get; set; }

        [NotNull]
        public ContactModel ContactFourth { get; set; }

        [NotNull]
        public ContactModel ContactFive { get; set; }

        [NotNull]
        public ContactModel ContactSix { get; set; }
    }
}