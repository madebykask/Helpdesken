'use strict';

var properties = window.fieldProperties;

var input = "input[name$='].";

var hideSelector = 'input[name$=".Hide"]:checkbox';
var requiredSelector = 'input[name$=".Required"]:checkbox';
var requiredOnReopenSelector = 'input[name$=".RequiredIfReopened"]:checkbox';
var showSelector = 'input[name$=".ShowOnStartPage"]:checkbox';
var showExternalSelector = 'input[name$=".ShowExternal"]:checkbox';
var lockedSelector = 'select[name$=".Locked"]';

$(function () {
 
    //show has meaning of active - resets all other controls 
    $(showSelector).on('click', function (e) {

        var $this = $(this);
        var $parent = $this.parents('tr');
        
        if (!this.checked) {
            resetFieldControls($parent, false);
        } else {
            $parent.find(lockedSelector).prop('disabled', false);
        }
    });

    var ctrls = [hideSelector, showExternalSelector, requiredSelector, requiredOnReopenSelector];

    $(ctrls.join(',')).on('click', function (ev) {
        var $this = $(this);
        var $parent = $this.parents('tr');
        if (!isMainChecked($parent)) {
            ev.preventDefault();
            return false;
        }
        return true;
    });

    $(lockedSelector).each(function (i, e) {
        var $this = $(this);
        var $parent = $this.parents('tr');

        if (!isMainChecked($parent)) {
            $this.find('option:eq(0)').prop('selected', true);
            $this.prop('disabled', true);
        } else {
            $this.prop('disabled', false);
        }
    });

    function isMainChecked($parent) {
        var $ctrl = $parent.find(showSelector);
        var isChecked = $ctrl[0].checked;
        return isChecked;
    }

    function resetFieldControls($parent) {
        
        var $hideCtrl = $parent.find(hideSelector);
        $hideCtrl.prop('checked', false);

        var $showExternalCtrl = $parent.find(showExternalSelector);
        $showExternalCtrl.prop('checked',false);

        var $requiredElement = $parent.find(requiredSelector).not(':disabled');
        $requiredElement.prop('checked', false);

        var $requiredIfReopendElement = $parent.find(requiredOnReopenSelector).not(':disabled');
        $requiredIfReopendElement.prop('checked', false);

        //locked:
        var $lockedControl = $parent.find(lockedSelector).not(':disabled');
        $lockedControl.find('option:eq(0)').prop('selected', true);
        $lockedControl.prop('disabled', true);
    }

    $(".header-chosen").chosen({
        width: "315px",
        'placeholder_text_multiple': placeholder_text_multiple,
        'no_results_text': no_results_text,
        max_selected_options: 4
    });

    $('#showStatusBarIds, #showExternalStatusBarIds').chosen({
        width: '315px',
        max_selected_options: 10,
        hide_results_on_select: false,
        placeholder_text_multiple: placeholder_text_multiple,
        no_results_text: no_results_text
    });

    //$("#showStatusBarIds, #showExternalStatusBarIds").bind("chosen:maxselected", function () { alert("max elements selected"); });

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
                SectionId: sectionElement.val(),
                CustomerId: customerId,
                IsNewCollapsed: isNewCollapsed,
                IsEditCollapsed: isEditCollapsed,
                SectionType: sectionType,
                SelectedFields: sectionDrop,
                LanguageId: languageId
            },
            dataType: "json",
            success: function(result) {
                if (result.success) {
                    sectionElement.val(result.sectionId);
                    return;
                } else {
                    ShowToastMessage("Operation failed", "error");
                }
                return;
            }
        });
    });

    $("#case_header_settings_popup").on("hide", function () {
        $("#sectionDiv" + sectionType).addClass("hidden-desktop");
    });
});

$(document).ready(function () {
    $(".th1").css({ 'width': ($(".tableth2x").width() + 'px') });
    $(".th2").css({ 'width': ($(".tableth9x").width() + 'px') });
    $(".th3").css({ 'width': ($(".tableth5x").width() + 'px') });
    $(".th4").css({ 'width': ($(".tableth5x").width() + 'px') });
    $(".th5").css({ 'width': ($(".tableth8x").width() + 'px') });
    $(".th6").css({ 'width': ($(".tableth20x").width() + 'px') });
    $(".th7").css({ 'width': ($(".tableth8x").width() + 'px') });
    $(".th8").css({ 'width': ($(".tableth8x").width() + 'px') });
    $(".th9").css({ 'width': ($(".tableth6x").width() + 'px') });
    $(".th10").css({ 'width': ($(".tableth20x").width() + 'px') });
    $(".th11").css({ 'width': ($(".tableth18x").width() + 'px') });

    $(input + properties.Label).attr('maxlength', '50');
    $(input + properties.FieldHelp).attr('maxlength', '200');
});
