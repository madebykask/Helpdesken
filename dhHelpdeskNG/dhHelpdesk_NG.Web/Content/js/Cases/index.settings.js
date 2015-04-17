"use strict";

$(document).ready(function () {
    var inSaving = false;

    function isWideColumnsCountExceed() {
        var res = 0;
        $('tr.SortableRow [name=column_style]').each(function (idx, el) {
            if ($(arguments[1]).val() == 'colwide') {
                res++;
            }
        });
        if (res > 2) {
            ShowToastMessage('Maximum <b>wide</b> columns in the grid is limited to 3');
            return true;
        }

        return false;
    }

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
    $('#btnSaveCaseSetting .btn').click(function (e) {
        e.preventDefault();
        if (window.app.getGridState() !== GRID_STATE.IDLE || inSaving) {
            return false;
        }

        inSaving = true;
        $(this).addClass('disabled');
        $(this).val('Saving...');
        
        /// validation
        if ($('#dataTable tr.SortableRow').length == 0) {
            window.ShowToastMessage('You should have at least one column selected for the "Case overview" grid', 'error');
            return false;
        }

        if (isWideColumnsCountExceed()) {
            return false;
        }
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
                success: function () {
                    inSaving = false;
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
            ShowToastMessage('We already have the "' + fieldInfo.label + '" column in grid');
            return false;
        }
        
        if (isWideColumnsCountExceed()) {
            return false;
        }

        $('#dataTable').append(template.render(fieldInfo)).find('select[fieldid*="' + fieldId + '"]').val($('#newColStyle').val());
    });

    $('#dataTable').on('click', '.deleterow', function () {
        var tr = $(this).closest('tr');
        tr.remove();
    });
});