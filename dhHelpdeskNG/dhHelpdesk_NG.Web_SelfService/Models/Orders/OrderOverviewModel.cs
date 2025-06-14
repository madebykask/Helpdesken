﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Common.ValidationAttributes;
using DH.Helpdesk.SelfService.Models.Common;

namespace DH.Helpdesk.SelfService.Models.Orders
{
    public class OrderOverviewModel
    {
        public OrderOverviewModel(
                int id,
                string orderType,
                List<NewGridRowCellValueModel> fieldValues,
                bool caseIsFinished = false)
        {
            this.FieldValues = fieldValues;
            this.Id = id;
            OrderType = orderType;
            CaseIsFinished = caseIsFinished;
        }

        [IsId]
        public int Id { get; private set; }

        public string OrderType { get; private set; }

        public bool CaseIsFinished { get; private set; }

        [NotNull]
        public List<NewGridRowCellValueModel> FieldValues { get; private set; }
    }
}