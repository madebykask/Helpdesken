﻿namespace DH.Helpdesk.Web.Models
{
    public interface IConfigurableFieldModel
    {
        string Caption { get; }

        bool IsRequired { get; }

        bool Show { get; }
    }
}