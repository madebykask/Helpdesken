"use strict";

$(document).ready(function () {
    //// initialization
    $("#customerCaseSum tbody").sortable(
       {
           items: "> tr:not(:first)",
           update: function (event, ui) {
               var elements = document.querySelectorAll('.SortableRow');
               var newOrder = "";
               for (var i = 0; i < elements.length; ++i) {
                   var item = elements[i];
                   newOrder += item.id.toString() + "|";
               }
           }
       });

    $("#customerCaseSum tbody").disableSelection();

    //// bind event handlers 
    $('#btnSaveCaseSetting').click(function (e) {
        e.preventDefault();
        $.post('/Cases/SaveSetting/', $("#frmCaseSetting").serialize(), function () {
            /// due to strange work with lists[] in .NET we have to send a bit different structure
            var formData = {
                SelectedFontStyle: $("#SelectedFontStyle").val(),
                FieldStyle: $.map($('#dataTable tr.SortableRow'), function(el) {
                    var colName = $(el).find('[name="column_name"]').val();
                    var colStyle = $(el).find('[name="column_style"]').val();
                    return { 'ColumnName': colName, 'Style': colStyle };
                })
            };
            $.ajax({
                url: '/Cases/SaveColSetting/',
                type: "POST",
                contentType: 'application/json;',
                dataType: 'json',
                data: JSON.stringify(formData),
                success: function() {
                    window.location.href = '/cases/';
                }
            });
        });
    });

    $('#divProductAreaSetting ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_ProductAreaSetting").text(getBreadcrumbs(this));
        $("#ProductAreaId").val(val);
    });

    $('#divCaseTypeSetting ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_CaseTypeSetting").text(getBreadcrumbs(this));
        $("#CaseTypeId").val(val);
    });

    $('#divClosingReasonSetting ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_ClosingReasonSetting").text(getBreadcrumbs(this));
        $("#ClosingReasonId").val(val);
    });

    $('.addrow').click(function () {
        var fieldId = $('#labellist').val();
        var orderIdx = $('#dataTable tr').length - 1;/// excluding first row tr.rowAdding

        var fieldInfo = {
            orderIdx: orderIdx,
            fieldId: fieldId,
            label: $('#labellist option:selected').text()
        };
        var template = $.templates("#caseFieldRow");
        if ($('tr.SortableRow input[type="hidden"][value="' + fieldId + '"]').length > 0) {
            /// prevent to add duplicate columns
            return false;
        }
        $('#dataTable').append(template.render(fieldInfo)).find('select[fieldid=' + fieldId + ']').val($('#newColStyle').val());
    });

    $('#dataTable').on('click', '.deleterow', function () {
        var tr = $(this).closest('tr');
        tr.remove();
    });
});