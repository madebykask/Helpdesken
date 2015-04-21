'use strict';

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

    $(tableBody).disableSelection();
});

function addCaseSettingRow() {    
    $.post(overviewParameters.ControllerAddUrl +
        '?usergroupId=' + $(overviewParameters.UserGroupId).val() +
        '&customerId=' + $(overviewParameters.CustomerId).val() +
        '&labellist=' + $(overviewParameters.FieldName).val() +
        '&linelist=1' +
        '&minWidthValue=' + $(overviewParameters.FieldWidth).val() +
        '&colOrderValue=' + $(overviewParameters.FieldOrder).val(),
        function (result) {
            if (result == "Repeated")
                ShowToastMessage(overviewParameters.RepeatedMessage, 'warning', false);
           else
               $(overviewParameters.UserGroupList).html(result);
        }
   );
}

function deleteCaseSettingRow(id) {    
    $.post(overviewParameters.ControllerDeleteUrl + id +
        '?usergroupId=' + $(overviewParameters.UserGroupId).val() +
        '&customerId=' + $(overviewParameters.CustomerId).val(),
        function (result) {
            $(overviewParameters.UserGroupList).html(result);
        }
   );
}