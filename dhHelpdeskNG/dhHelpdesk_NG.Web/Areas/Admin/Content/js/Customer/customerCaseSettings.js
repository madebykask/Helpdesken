'use strict';

var properties = window.fieldProperties;

var input = "input[name$='].";
$(function () {
    $(input + properties.ShowOnStartPage).click(function () {
        var elementName = this.name.replace(properties.ShowOnStartPage, properties.ShowExternal);
        var curElement = jQuery('[name="' + elementName + '"]');
        console.log('catch me');
        var $requiredElement = $(this).parents('tr').find('[name$="Required"]:checkbox').not(':disabled');
        var $requiredIfReopendElement = $(this).parents('tr').find('[name$="RequiredIfReopened"]:checkbox').not(':disabled');
        var visibleElement = curElement.filter(':visible').get(0);
        if (this.checked == false && visibleElement != undefined) {
            visibleElement.checked = false;
            $requiredElement.prop('checked', false);
            $requiredIfReopendElement.prop('checked', false);
        }
    });

    $(input + properties.ShowOnStartPage).click(function () {
        var _this = $(this);
        var elemLocked = _this.closest('tr').find('select[name$=".Locked"]');
        if (!_this[0].checked) {
            elemLocked.prop('disabled', true);
            elemLocked.find('option:eq(0)').prop('selected', true);
        } else {
            elemLocked.prop('disabled', false);
        }
    });

    $('select[name$=".Locked"]').each(function (i, e) {
        var _this = $(this);
        var elemShowOnstartPage = _this.closest('tr').find('input[name$=".ShowOnStartPage"]');
        if (!elemShowOnstartPage[0].checked) {
            _this.prop('disabled', true);
            _this.find('option:eq(0)').prop('selected', true);
        } else {
            _this.prop('disabled', false);
        }

    });

    $(input + properties.ShowExternal).click(function () {
        var elementName = this.name.replace(properties.ShowExternal, properties.ShowOnStartPage);
        var curElement = jQuery('[name="' + elementName + '"]');
        var visibleElement = curElement.filter(':visible').get(0);
        if (visibleElement != undefined && visibleElement.checked == false) {
            this.checked = false;
        }
    });

    $('input[name$=".Required"]').on('click', function (ev) {
        var $mainEl = $(this).parents('tr').find('[name$=".ShowOnStartPage"]:checkbox');
        if (!$mainEl.prop('checked')) {
            ev.preventDefault();
            return false;
        }
        return true;
    });

    $('input[name$=".RequiredIfReopened"]').on('click', function (ev) {
        var $mainEl = $(this).parents('tr').find('[name$=".ShowOnStartPage"]:checkbox');
        if (!$mainEl.prop('checked')) {
            ev.preventDefault();
            return false;
        }
        return true;
    });

    $(".header-chosen").chosen({
        width: "315px",
        'placeholder_text_multiple': placeholder_text_multiple,
        'no_results_text': no_results_text,
        max_selected_options: 4
    });

    var sectionElement;
    var sectionType;

    $("a.section-settbtn").on("click", function (e) {
        e.preventDefault();
        var $src = $(this);
        sectionElement = $src.children("input.section-id");
        sectionType = $src.children("input.section-type").val();
        var customerId = $("#customerId").val();
        var languageId = $("#LanguageId").val();

        $.ajax({
            url: "/customercasefieldsettings/GetCaseSection",
            type: "post",
            data: { sectionId: sectionElement.val(), customerId: customerId, languageId: languageId },
            dataType: "json",
            success: function (result) {
                if (result.success) {
                    $("#isNewCollapsed").val(result.isNewCollapsed);
                    $("#isEditCollapsed").val(result.isEditCollapsed);
                    $("#modal_header").text(result.sectionHeader);
                    $("#sectionDrop" + sectionType).val(result.sectionFields);
                    $("#sectionDrop" + sectionType).trigger("chosen:updated");
                    $("#sectionDiv" + sectionType).removeClass("hidden-desktop");
                }
                return;
            }
        });
        var $target = $("#case_header_settings_popup");
        $target.attr("data-src", $src.attr("data-src"));
        $target.modal("show");
    });

    $("#btnSaveDlg").on("click", function () {
        var isNewCollapsed = $("#isNewCollapsed").val();
        var isEditCollapsed = $("#isEditCollapsed").val();
        var customerId = $("#customerId").val();
        var sectionDrop = $("#sectionDrop" + sectionType).val();
        var languageId = $("#LanguageId").val();
        $.ajax({
            url: "/customercasefieldsettings/SaveCaseSectionOptions",
            type: "post",
            data: {
                sectionId: sectionElement.val(),
                customerId: customerId,
                isNewCollapsed: isNewCollapsed,
                isEditCollapsed: isEditCollapsed,
                sectionType: sectionType,
                selectedFields: sectionDrop,
                languageId: languageId
            },
            dataType: "json",
            success: function (result) {
                if (result.success) {
                    sectionElement.val(result.sectionId);
                    return;
                } else {
                    ShowToastMessage("Bad", "error");
                }
                return;
            }
        });
    });

    $("#case_header_settings_popup").on("hide", function () {
        $("#sectionDiv" + sectionType).addClass("hidden-desktop");
    });

    $(document).ready(function () {
        $(".th1").css({ 'width': ($(".tableth2x").width() + 'px') });
        $(".th2").css({ 'width': ($(".tableth9x").width() + 'px') });
        $(".th3").css({ 'width': ($(".tableth5x").width() + 'px') });
        $(".th4").css({ 'width': ($(".tableth8x").width() + 'px') });
        $(".th5").css({ 'width': ($(".tableth20x").width() + 'px') });
        $(".th6").css({ 'width': ($(".tableth8x").width() + 'px') });
        $(".th7").css({ 'width': ($(".tableth8x").width() + 'px') });
        $(".th8").css({ 'width': ($(".tableth6x").width() + 'px') });
        $(".th9").css({ 'width': ($(".tableth20x").width() + 'px') });
        $(".th10").css({ 'width': ($(".tableth18x").width() + 'px') });

        
    });
});

$(function () {    
    $(input + properties.Label).attr('maxlength', '50');    
    $(input + properties.FieldHelp).attr('maxlength', '200');
});

