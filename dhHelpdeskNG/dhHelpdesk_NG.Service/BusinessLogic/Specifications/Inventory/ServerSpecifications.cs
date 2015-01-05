namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Inventory
{
    using System;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Inventory.Fields;
    using DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Server;
    using DH.Helpdesk.BusinessData.Enums.Inventory.Fields.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Servers;

    public static class ServerSpecifications
    {
        public static IQueryable<Server> GetByText(this IQueryable<Server> query, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return query;
            }

            var search = text.Trim().ToLower();
            query =
                query.Where(
                    it =>
                    it.ServerName.ToLower().Contains(search) 
                    || it.ServerModel.ToLower().Contains(search)
                    || it.ServerDescription.ToLower().Contains(search) 
                    || it.Manufacturer.ToLower().Contains(search)
                    || it.SerialNumber.ToLower().Contains(search));
            return query;
        }

        public static IQueryable<Server> Search(
                        this IQueryable<Server> query,
                        int customerId,
                        string searchFor,
                        SortField sort,
                        int selectCount = 0)
        {
            query = query
                        .GetByCustomer(customerId)
                        .GetByText(searchFor)
                        .Sort(sort);

            if (selectCount > 0)
            {
                query = query.Take(selectCount);
            }

            return query;
        }


        public static IQueryable<Server> Sort(this IQueryable<Server> query, SortField sort)
        {
            if (sort == null)
            {
                return query;
            }

            switch (sort.SortBy)
            {
                case SortBy.Ascending:
                    // GeneralFields
                    if (sort.Name == GeneralFields.Name)
                    {
                        query = query.OrderBy(it => it.ServerName);
                    }
                    else if (sort.Name == GeneralFields.Manufacturer)
                    {
                        query = query.OrderBy(it => it.Manufacturer);
                    }
                    else if (sort.Name == GeneralFields.Description)
                    {
                        query = query.OrderBy(it => it.ServerDescription);
                    }
                    else if (sort.Name == GeneralFields.Model)
                    {
                        query = query.OrderBy(it => it.ServerModel);
                    }
                    else if (sort.Name == GeneralFields.SerialNumber)
                    {
                        query = query.OrderBy(it => it.SerialNumber);
                    }
   
                    // OtherFields
                    else if (sort.Name == OtherFields.Info)
                    {
                        query = query.OrderBy(it => it.Info);
                    }
                    else if (sort.Name == OtherFields.Other)
                    {
                        query = query.OrderBy(it => it.Miscellaneous);
                    }
                    else if (sort.Name == OtherFields.URL)
                    {
                        query = query.OrderBy(it => it.URL);
                    }
                    else if (sort.Name == OtherFields.URL2)
                    {
                        query = query.OrderBy(it => it.URL2);
                    }
                    else if (sort.Name == OtherFields.Owner)
                    {
                        query = query.OrderBy(it => it.Owner);
                    }

                    // StateFields
                    else if (sort.Name == StateFields.SyncChangeDate)
                    {
                        query = query.OrderBy(it => it.SyncChangedDate);
                    }

                    // StorageFields
                    else if (sort.Name == StorageFields.Capasity)
                    {
                        query = query.OrderBy(it => it.Harddrive);
                    }

                    // ChassisFields
                    else if (sort.Name == ChassisFields.Chassis)
                    {
                        query = query.OrderBy(it => it.ChassisType);
                    }
                 
                    // InventoryFields
                    else if (sort.Name == InventoryFields.BarCode)
                    {
                        query = query.OrderBy(it => it.BarCode);
                    }
                    else if (sort.Name == InventoryFields.PurchaseDate)
                    {
                        query = query.OrderBy(it => it.PurchaseDate);
                    }

                    // MemoryFields
                    else if (sort.Name == MemoryFields.RAM)
                    {
                        query = query.OrderBy(it => it.RAM.Name);
                    }

                    // OperatingSystemFields
                    else if (sort.Name == OperatingSystemFields.OperatingSystem)
                    {
                        query = query.OrderBy(it => it.OperatingSystem.Name);
                    }
                    else if (sort.Name == OperatingSystemFields.Version)
                    {
                        query = query.OrderBy(it => it.Version);
                    }
                    else if (sort.Name == OperatingSystemFields.ServicePack)
                    {
                        query = query.OrderBy(it => it.SP);
                    }
                    else if (sort.Name == OperatingSystemFields.RegistrationCode)
                    {
                        query = query.OrderBy(it => it.RegistrationCode);
                    }
                    else if (sort.Name == OperatingSystemFields.ProductKey)
                    {
                        query = query.OrderBy(it => it.ProductKey);
                    }

                    // ProcessorFields 
                    else if (sort.Name == ProcessorFields.ProccesorName)
                    {
                        query = query.OrderBy(it => it.Processor.Name);
                    }

                    // PlaceFields 
                    else if (sort.Name == PlaceFields.Room)
                    {
                        query = query.OrderBy(it => it.Room.Name);
                    } 
                    else if (sort.Name == PlaceFields.Location)
                    {
                        query = query.OrderBy(it => it.Location);
                    }

                    // CommunicationFields 
                    else if (sort.Name == CommunicationFields.NetworkAdapter)
                    {
                        query = query.OrderBy(it => it.NIC.Name);
                    }
                    else if (sort.Name == CommunicationFields.IPAddress)
                    {
                        query = query.OrderBy(it => it.IPAddress);
                    }
                    else if (sort.Name == CommunicationFields.MacAddress)
                    {
                        query = query.OrderBy(it => it.MACAddress);
                    }

                    break;
                case SortBy.Descending:
                    // GeneralFields
                    if (sort.Name == GeneralFields.Name)
                    {
                        query = query.OrderByDescending(it => it.ServerName);
                    }
                    else if (sort.Name == GeneralFields.Manufacturer)
                    {
                        query = query.OrderByDescending(it => it.Manufacturer);
                    }
                    else if (sort.Name == GeneralFields.Description)
                    {
                        query = query.OrderByDescending(it => it.ServerDescription);
                    }
                    else if (sort.Name == GeneralFields.Model)
                    {
                        query = query.OrderByDescending(it => it.ServerModel);
                    }
                    else if (sort.Name == GeneralFields.SerialNumber)
                    {
                        query = query.OrderByDescending(it => it.SerialNumber);
                    }

                    // OtherFields
                    else if (sort.Name == OtherFields.Info)
                    {
                        query = query.OrderByDescending(it => it.Info);
                    }
                    else if (sort.Name == OtherFields.Other)
                    {
                        query = query.OrderByDescending(it => it.Miscellaneous);
                    }
                    else if (sort.Name == OtherFields.URL)
                    {
                        query = query.OrderByDescending(it => it.URL);
                    }
                    else if (sort.Name == OtherFields.URL2)
                    {
                        query = query.OrderByDescending(it => it.URL2);
                    }
                    else if (sort.Name == OtherFields.Owner)
                    {
                        query = query.OrderByDescending(it => it.Owner);
                    }

                    // StateFields
                    else if (sort.Name == StateFields.SyncChangeDate)
                    {
                        query = query.OrderByDescending(it => it.SyncChangedDate);
                    }

                    // StorageFields
                    else if (sort.Name == StorageFields.Capasity)
                    {
                        query = query.OrderByDescending(it => it.Harddrive);
                    }

                    // ChassisFields
                    else if (sort.Name == ChassisFields.Chassis)
                    {
                        query = query.OrderByDescending(it => it.ChassisType);
                    }

                    // InventoryFields
                    else if (sort.Name == InventoryFields.BarCode)
                    {
                        query = query.OrderByDescending(it => it.BarCode);
                    }
                    else if (sort.Name == InventoryFields.PurchaseDate)
                    {
                        query = query.OrderByDescending(it => it.PurchaseDate);
                    }

                    // MemoryFields
                    else if (sort.Name == MemoryFields.RAM)
                    {
                        query = query.OrderByDescending(it => it.RAM.Name);
                    }

                    // OperatingSystemFields
                    else if (sort.Name == OperatingSystemFields.OperatingSystem)
                    {
                        query = query.OrderByDescending(it => it.OperatingSystem.Name);
                    }
                    else if (sort.Name == OperatingSystemFields.Version)
                    {
                        query = query.OrderByDescending(it => it.Version);
                    }
                    else if (sort.Name == OperatingSystemFields.ServicePack)
                    {
                        query = query.OrderByDescending(it => it.SP);
                    }
                    else if (sort.Name == OperatingSystemFields.RegistrationCode)
                    {
                        query = query.OrderByDescending(it => it.RegistrationCode);
                    }
                    else if (sort.Name == OperatingSystemFields.ProductKey)
                    {
                        query = query.OrderByDescending(it => it.ProductKey);
                    }

                    // ProcessorFields 
                    else if (sort.Name == ProcessorFields.ProccesorName)
                    {
                        query = query.OrderByDescending(it => it.Processor.Name);
                    }

                    // PlaceFields 
                    else if (sort.Name == PlaceFields.Room)
                    {
                        query = query.OrderByDescending(it => it.Room.Name);
                    }
                    else if (sort.Name == PlaceFields.Location)
                    {
                        query = query.OrderByDescending(it => it.Location);
                    }

                    // CommunicationFields 
                    else if (sort.Name == CommunicationFields.NetworkAdapter)
                    {
                        query = query.OrderByDescending(it => it.NIC.Name);
                    }
                    else if (sort.Name == CommunicationFields.IPAddress)
                    {
                        query = query.OrderByDescending(it => it.IPAddress);
                    }
                    else if (sort.Name == CommunicationFields.MacAddress)
                    {
                        query = query.OrderByDescending(it => it.MACAddress);
                    }

                    break;
            }

            return query;
        }
    }
}