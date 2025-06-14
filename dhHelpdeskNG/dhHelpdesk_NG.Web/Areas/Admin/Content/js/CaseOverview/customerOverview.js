﻿'use strict';

var overviewParameters = window.parameters;
$(function () {  
    
    if (!overviewParameters.Seperator) {
        overviewParameters.Seperator = ',';
    }

    var tableBody = "#" + overviewParameters.CaseFieldsTableId + " tbody";

    $(tableBody).sortable(
        {
            items: "> tr:not(:first)",
            update: function (event, ui) {
                var elements = document.querySelectorAll('.' + overviewParameters.SortableRowClass);
                var newOrder = "";
                for (var i = 0; i < elements.length; ++i) {
                    var item = elements[i];
                    newOrder += overviewParameters.Seperator + item.id.toString();
                }
                $(overviewParameters.SortFieldObj).val(newOrder);
            }
        });

    //Has problem in FireFox
    //$(tableBody).disableSelection();
});

function addCaseSettingRow() {    
    var displayName = $(overviewParameters.FieldName + " option:selected").text();
    var itemsOrderArray = [];
    $("#dataTable").find(".SortableRow").each(function () {
        itemsOrderArray.push(this.id);
    });
    $.post(overviewParameters.ControllerAddUrl +
        "?usergroupId=" + $(overviewParameters.UserGroupId).val() +
        "&customerId=" + $(overviewParameters.CustomerId).val() +
        "&labellist=" + $(overviewParameters.FieldName).val() +
        "&linelist=1" +
        "&minWidthValue=" + $(overviewParameters.FieldWidth).val() +
        "&colOrderValue=" + $(overviewParameters.FieldOrder).val() +
        "&clientOrder=" + itemsOrderArray,
        function (result) {
            if (result == "Repeated")
                ShowToastMessage(overviewParameters.RepeatedMessage.replace(/\{0\}/, displayName), 'warning', false);
           else
               $(overviewParameters.UserGroupList).html(result);
        }
   );
}

function deleteCaseSettingRow(id) {
    var itemsOrderArray = [];
    $("#dataTable").find(".SortableRow").each(function () {
        itemsOrderArray.push(this.id);
    });
    $.post(overviewParameters.ControllerDeleteUrl + id +
        "?usergroupId=" + $(overviewParameters.UserGroupId).val() +
        "&customerId=" + $(overviewParameters.CustomerId).val() +
        "&clientOrder=" + itemsOrderArray,
        function (result) {
            $(overviewParameters.UserGroupList).html(result);
        }
   );
}