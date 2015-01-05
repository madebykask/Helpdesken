namespace DH.Helpdesk.Dal.SearchRequestBuilders.Notifiers
{
    using System;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Server;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Enums.Inventory.Shared;
    using DH.Helpdesk.Domain.Servers;

    public sealed class ServersSearchRequestBuilder
    {
        private IQueryable<Server> entities;

        public ServersSearchRequestBuilder(IQueryable<Server> entities)
        {
            this.entities = entities;
        }

        public IQueryable<Server> Build()
        {
            return this.entities;
        }

        public void OrderByDefault()
        {
            this.entities = this.entities.OrderBy(n => n.ServerName);
        }

        public void OrderBy(SortField field)
        {
            switch (field.SortBy)
            {
                case SortBy.Ascending:
                    if (field.Name == GeneralFields.Name)
                    {
                        this.entities = this.entities.OrderBy(it => it.ServerName);
                    }
                    else if (field.Name == GeneralFields.Manufacturer)
                    {
                        this.entities = this.entities.OrderBy(it => it.Manufacturer);
                    }
                    else if (field.Name == GeneralFields.Description)
                    {
                        this.entities = this.entities.OrderBy(it => it.ServerDescription);
                    }
                    else if (field.Name == GeneralFields.Model)
                    {
                        this.entities = this.entities.OrderBy(it => it.ServerModel);
                    }
                    else if (field.Name == GeneralFields.SerialNumber)
                    {
                        this.entities = this.entities.OrderBy(it => it.SerialNumber);
                    }
                    else if (field.Name == ChassisFields.Chassis)
                    {
                        this.entities = this.entities.OrderBy(it => it.ChassisType);
                    }
                    else if (field.Name == InventoryFields.BarCode)
                    {
                        this.entities = this.entities.OrderBy(it => it.BarCode);
                    }
                    else if (field.Name == InventoryFields.PurchaseDate)
                    {
                        this.entities = this.entities.OrderBy(it => it.PurchaseDate);
                    }
                    else if (field.Name == OperatingSystemFields.OperatingSystem)
                    {
                        this.entities = this.entities.OrderBy(it => it.OperatingSystem.Name);
                    }
                    //                    else if (field.Name == OperatingSystemFields.Version)
                    //                    {
                    //                        this.entities = this.entities.OrderBy(it => it.RAM.);
                    //                    }
                    //                    else if (field.Name == GeneralFields.Email)
                    //                    {
                    //                        this.entities = this.entities.OrderBy(it => it.Email);
                    //                    }
                    //                    else if (field.Name == GeneralFields.Code)
                    //                    {
                    //                        this.entities = this.entities.OrderBy(it => it.UserCode);
                    //                    }
                    //                    else if (field.Name == AddressField.PostalAddress)
                    //                    {
                    //                        this.entities = this.entities.OrderBy(it => it.PostalAddress);
                    //                    }
                    //                    else if (field.Name == AddressField.PostalCode)
                    //                    {
                    //                        this.entities = this.entities.OrderBy(it => it.Postalcode);
                    //                    }
                    //                    else if (field.Name == AddressField.City)
                    //                    {
                    //                        this.entities = this.entities.OrderBy(it => it.City);
                    //                    }
                    //                    else
                    //                    {
                    //                        throw new NotImplementedException();
                    //                    }
                    //
                    //                    break;
                    //                case SortBy.Descending:
                    //                    if (field.Name == GeneralFields.UserId)
                    //                    {
                    //                        this.entities = this.entities.OrderByDescending(n => n.UserId);
                    //                    }
                    //                    else if (field.Name == GeneralFields.Domain)
                    //                    {
                    //                        this.entities = this.entities.OrderByDescending(n => n.Domain.Name);
                    //                    }
                    //                    else if (field.Name == GeneralFields.LoginName)
                    //                    {
                    //                        this.entities = this.entities.OrderByDescending(n => n.LogonName);
                    //                    }
                    //                    else if (field.Name == GeneralFields.FirstName)
                    //                    {
                    //                        this.entities = this.entities.OrderByDescending(it => it.FirstName);
                    //                    }
                    //                    else if (field.Name == GeneralFields.Initials)
                    //                    {
                    //                        this.entities = this.entities.OrderByDescending(it => it.Initials);
                    //                    }
                    //                    else if (field.Name == GeneralFields.LastName)
                    //                    {
                    //                        this.entities = this.entities.OrderByDescending(it => it.SurName);
                    //                    }
                    //                    else if (field.Name == GeneralFields.DisplayName)
                    //                    {
                    //                        this.entities = this.entities.OrderByDescending(it => it.DisplayName);
                    //                    }
                    //                    else if (field.Name == GeneralFields.Place)
                    //                    {
                    //                        this.entities = this.entities.OrderByDescending(it => it.Location);
                    //                    }
                    //                    else if (field.Name == GeneralFields.Phone)
                    //                    {
                    //                        this.entities = this.entities.OrderByDescending(it => it.Phone);
                    //                    }
                    //                    else if (field.Name == GeneralFields.CellPhone)
                    //                    {
                    //                        this.entities = this.entities.OrderByDescending(it => it.Cellphone);
                    //                    }
                    //                    else if (field.Name == GeneralFields.Email)
                    //                    {
                    //                        this.entities = this.entities.OrderByDescending(it => it.Email);
                    //                    }
                    //                    else if (field.Name == GeneralFields.Code)
                    //                    {
                    //                        this.entities = this.entities.OrderByDescending(it => it.UserCode);
                    //                    }
                    //                    else if (field.Name == AddressField.PostalAddress)
                    //                    {
                    //                        this.entities = this.entities.OrderByDescending(it => it.PostalAddress);
                    //                    }
                    //                    else if (field.Name == AddressField.PostalCode)
                    //                    {
                    //                        this.entities = this.entities.OrderByDescending(it => it.Postalcode);
                    //                    }
                    //                    else if (field.Name == AddressField.City)
                    //                    {
                    //                        this.entities = this.entities.OrderByDescending(it => it.City);
                    //                    }
                    //                    else
                    //                    {
                    //                        throw new NotImplementedException();
                    //                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


    }
}
