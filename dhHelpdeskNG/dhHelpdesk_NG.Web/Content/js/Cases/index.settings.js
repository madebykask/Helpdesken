"use strict";

$(document).ready(function () {
    var savingMsg = $('#savingMsg').text();
    var maxWideColumnMsg = $('#maxWideColumnMsg').text();
    var atLeastOneColumnMsg = $('#atLeastOneColumnMsg').text();
    var columnAlreadyExistsMsg = $('#columnAlreadyExists').text();
    var $saveBtn = $('#btnSaveCaseSetting .btn');

    function isWideColumnsCountExceed() {
        var res = 0;
        $('tr.SortableRow [name=column_style]').each(function (idx, el) {
            if ($(arguments[1]).val() == 'colwide') {
                res++;
            }
        });
        if (res > 3) {
            ShowToastMessage(maxWideColumnMsg);
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

    //Has problem in FireFox
    //$("#customerCaseSum tbody").disableSelection();

    //// bind event handlers 
    $saveBtn.click(function (e) {
        e.preventDefault();
        window.app.abortAjaxReq();
        if (!(window.app.getGridState() === GRID_STATE.IDLE || window.app.getGridState() === GRID_STATE.NO_COL_SELECTED) || $saveBtn.hasClass('disabled')) {
            return false;
        }
        
        /// validation
        if ($('#dataTable tr.SortableRow').length == 0) {
            window.ShowToastMessage(atLeastOneColumnMsg, 'error');
            window.scrollTo(0, document.body.scrollHeight);
            return false;
        }

        if (isWideColumnsCountExceed()) {
            return false;
        }
        
        $saveBtn.addClass('disabled');
        $saveBtn.val(savingMsg);

        $.post('/Cases/SaveSetting/', $("#frmCaseSetting").serialize(), function () {
            /// due to strange work with lists[] in .NET we have to send a bit different structure
            var formData = {
                SelectedFontStyle: $("#SelectedFontStyle").val(),
                SelectedPageSize: $("#SelectedPageSize").val(),
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

    $('#divCategorySetting ul.dropdown-menu li a').click(function (e) {
        e.preventDefault();
        var val = $(this).attr('value');
        $("#divBreadcrumbs_CategorySetting").text(getBreadcrumbs(this));
        $("#CategoryId").val(val);
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
            ShowToastMessage(columnAlreadyExistsMsg.replace(/\{0\}/, fieldInfo.label));
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