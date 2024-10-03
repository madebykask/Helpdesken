$(function () {
    (function ($) {

        window.Params = window.Params || {};
        const saveRuleUrl = window.Params.saveRuleUrl;
        const overviewRuleUrl = window.Params.overviewRuleUrl;
        const getAdmininstatorsForWorkingGroupUrl = window.Params.getAdmininstatorsForWorkingGroupUrl;
        const getWorkingGroupsForCustomerUrl = window.Params.getWorkingGroupsForCustomerUrl;
        const getAdministratorsForCustomersUrl = window.Params.getAdministratorsForCustomersUrl;

       
        const elBtnSaveRule = "#btnSaveRule";

        const elCustomerId = "#CustomerId";
        const elRuleId = "#RuleId";

        const elRuleName = "#RuleName";
        const elEventsDropDown = "#lstEvents";
        const elRuleSequence = "#ruleSequence";
        //const elContinueOnSuccess = "#continueOnSuccess";
        //const elContinueOnError = "#continueOnError";
        const elIsRuleActive = "#isRuleActive";

        const elCondition1 = "#BRCondition1";
        const elCondition2 = "#BRCondition2";
        const elCondition3 = "#BRCondition3";
        const elCondition4 = "#BRCondition4";

        const elBRActionMailTemplate = "#BRActionMailTemplate";
        const elBRActionEmailGroup = "#BRActionEmailGroup";

        const elBRActionWorkingGroup = "#BRActionWorkingGroup";
        const elBRActionWorkingGroupSingleSelect = "#BRActionWorkingGroupSingleSelect";

        const elBRActionAdministrator = "#BRActionAdministrator";
        const elBRActionAdministratorSingleSelect = "#BRActionAdministratorSingleSelect";

        const elBRActionRecipients = "#BRActionRecipients";
        const elBRActionCreatedBy = "#BRActionCreatedBy";
        const elBRActionRegistrator = "#BRActionRegistrator";
        const elBRActionAbout = "#BRActionAbout";
        const elBRActionDisableFinishingType = "#BRActionDisableFinishingType";

        const elProcessFromDropDown = "#lstProcessFrom";
        const elProcessToDropDown = "#lstProcessTo";

        const elSubStatusFromDropDown = "#lstSubStatusFrom";
        const elSubStatusToDropDown = "#lstSubStatusTo";

        const elSubStatusFromDropDown2 = "#lstSubStatusFrom2";
        const elSubStatusToDropDown2 = "#lstSubStatusTo2";

        const elStatusFromDropDown = "#lstStatusFrom";
        const elStatusToDropDown = "#lstStatusTo";

        const elStatusFromDropDown2 = "#lstStatusFrom2";
        const elStatusToDropDown2 = "#lstStatusTo2";

        const elStatusFromDropDown3 = "#lstStatusFrom3";
        const elStatusToDropDown3 = "#lstStatusTo3";

        const elSubStatusFromDropDown3 = "#lstSubStatusFrom3";
        const elSubStatusToDropDown3 = "#lstSubStatusTo3";

        const elDomainEquals = "#lstEquals";

        const elEmailTemplatsDropDown = "#lstEmailTemplates";
        const elEmailGroupsDropDown = "#lstEmailGroups";

        const elWorkingGroupsDropDown = "#lstWorkingGroups";
        const elAdministratorsDropDown = "#lstAdministrators";

        const elRecipients = "#recipients";
        const elCaseCreator = "#caseCreator";
        const elInitiator = "#initiator";
        const elCaseIsAbout = "#caseIsAbout";

        const elDisableFinishingType = "#disableFinishingType";

        const isWorkingGroupMandatory = "#isWorkingGroupMandatory";
        const isAdminMandatory = "#isAdminMandatory";

        window.dhHelpdesk = window.dhHelpdesk || {};
        window.dhHelpdesk.businessRule = window.dhHelpdesk.businessRule || {};

        (function ($) {
            let originHighlight = $.validator.defaults.highlight;
            let originUnhighlight = $.validator.defaults.unhighlight;
            let $form = $("#newRule");
            let validator = $form.data("validator");

            $.extend(validator.settings, {
                ignore: ":hidden, .ignore-validation",
                highlight: function (element, errorClass, validClass) {
                    let $element = $(element);
                    if ($element.hasClass("BR-chosen-single-select")
                        || $element.hasClass("BR-chosen-select")) {
                        let chosenSelectInput = $element.next().find(".chosen-choices");
                        if (chosenSelectInput.length > 0) {
                            element = chosenSelectInput[0];
                        } else {
                            element = $element.next()[0];
                        }
                    }

                    originHighlight(element, errorClass, validClass);
                },
                unhighlight: function (element, errorClass, validClass) {
                    let $element = $(element);
                    if ($element.hasClass("BR-chosen-single-select")
                        || $element.hasClass("BR-chosen-select")) {
                        let chosenSelectInput = $element.next().find(".chosen-choices");
                        if (chosenSelectInput.length > 0) {
                            element = chosenSelectInput[0];
                        } else {
                            element = $element.next()[0];
                        }
                    }

                    originUnhighlight(element, errorClass, validClass);
                }
            });
        })(jQuery);

        let getData = function () {
            let data = {
                customerId: 0,
                ruleId: 0,

                ruleName: "",
                event: 0,
                ruleSequence: 0,
                continueOnSuccess: true,
                continueOnError: true,
                ruleActive: true,

                processFrom: "",
                processTo: "",

                subStatusFrom: "",
                subStatusTo: "",
                subStatusFrom2: "",
                subStatusTo2: "",
                subStatusFrom3: "",
                subStatusTo3: "",

                statusFrom: "",
                statusTo: "",
                statusFrom2: "",
                statusTo2: "",
                statusFrom3: "",
                statusTo3: "",

                equals: "",

                emailTemplate: 0,
                emailGroups: "",
                workingGroups: "",
                workingGroup: "",
                administrators: "",
                administrator: "",
                recipients: "",
                caseCreator: true,
                initiator: true,
                caseIsAbout: true,

                disableFinishingType: true,
            };

            data.customerId = $(elCustomerId).val();
            data.ruleId = $(elRuleId).val();
            data.ruleName = $(elRuleName).val();
            data.ruleSequence = $(elRuleSequence).val();
            //data.continueOnSuccess = $(elContinueOnSuccess).bootstrapSwitch('state');
            //data.continueOnError = $(elContinueOnError).bootstrapSwitch('state');
            data.ruleActive = $(elIsRuleActive).bootstrapSwitch("state");

            $(elEventsDropDown + " option:selected").each(function () {
                data.event = $(this).val();
            });

            $(elProcessFromDropDown + " option:selected").each(function () {
                data.processFrom += $(this).val() + ",";
            });

            $(elProcessToDropDown + " option:selected").each(function () {
                data.processTo += $(this).val() + ",";
            });

            $(elSubStatusFromDropDown + " option:selected").each(function () {
                data.subStatusFrom += $(this).val() + ",";
            });

            $(elSubStatusToDropDown + " option:selected").each(function () {
                data.subStatusTo += $(this).val() + ",";
            });

            $(elSubStatusFromDropDown2 + " option:selected").each(function () {
                data.subStatusFrom2 += $(this).val() + ",";
            });

            $(elSubStatusToDropDown2 + " option:selected").each(function () {
                data.subStatusTo2 += $(this).val() + ",";
            });

            $(elSubStatusFromDropDown3 + " option:selected").each(function () {
                data.subStatusFrom3 += $(this).val() + ",";
            });

            $(elSubStatusToDropDown3 + " option:selected").each(function () {
                data.subStatusTo3 += $(this).val() + ",";
            });



            $(elStatusFromDropDown + " option:selected").each(function () {
                data.statusFrom += $(this).val() + ",";
            });

            $(elStatusToDropDown + " option:selected").each(function () {
                data.statusTo += $(this).val() + ",";
            });

            $(elStatusFromDropDown2 + " option:selected").each(function () {
                data.statusFrom2 += $(this).val() + ",";
            });

            $(elStatusToDropDown2 + " option:selected").each(function () {
                data.statusTo2 += $(this).val() + ",";
            });

            $(elStatusFromDropDown3 + " option:selected").each(function () {
                data.statusFrom3 += $(this).val() + ",";
            });

            $(elStatusToDropDown3 + " option:selected").each(function () {
                data.statusTo3 += $(this).val() + ",";
            });


          
            $(elEmailTemplatsDropDown + " option:selected").each(function () {
                data.emailTemplate = $(this).val();
            });

            $(elEmailGroupsDropDown + " option:selected").each(function () {
                data.emailGroups += $(this).val() + ",";
            });

            $(elWorkingGroupsDropDown + " option:selected").each(function () {
                data.workingGroups += $(this).val() + ",";
            });

            $(elAdministratorsDropDown + " option:selected").each(function () {
                data.administrators += $(this).val() + ",";
            });

            data.recipients = $(elRecipients).val();
            data.caseCreator = $(elCaseCreator).bootstrapSwitch("state");
            data.initiator = $(elInitiator).bootstrapSwitch("state");
            data.caseIsAbout = $(elCaseIsAbout).bootstrapSwitch("state");

            data.disableFinishingType = $(elDisableFinishingType).bootstrapSwitch("state");

            data.customerId = $(elCustomerId).val();

            data.equals = $(elDomainEquals).val();

            $(elBRActionAdministratorSingleSelect + " option:selected").each(function () {
                data.administrator = $(this).val();
            });

            $(elBRActionWorkingGroupSingleSelect + " option:selected").each(function () {
                data.workingGroup = $(this).val();
                data.workingGroups = $(this).val() + ",";
            });

            return data;
        };       
      
        dhHelpdesk.businessRule.saveRule = function () {
            if (!dhHelpdesk.businessRule.doValidation())
                return;

            let data = getData();
            $.get(saveRuleUrl,
                {
                    'data.CustomerId': data.customerId,
                    'data.RuleId': data.ruleId,
                    'data.RuleName': data.ruleName,
                    'data.EventId': data.event,
                    'data.RuleSequence': data.ruleSequence,
                    'data.ContinueOnSuccess': data.continueOnSuccess,
                    'data.ContinueOnError': data.continueOnError,
                    'data.RuleActive': data.ruleActive,
                    'data.ProcessFrom': data.processFrom,
                    'data.ProcessTo': data.processTo,
                    'data.SubStatusFrom': data.subStatusFrom,
                    'data.SubStatusTo': data.subStatusTo,
                    'data.SubStatusFrom2': data.subStatusFrom2,
                    'data.SubStatusTo2': data.subStatusTo2,
                    'data.SubStatusFrom3': data.subStatusFrom3,
                    'data.SubStatusTo3': data.subStatusTo3,
                    'data.StatusFrom': data.statusFrom,
                    'data.StatusTo': data.statusTo,
                    'data.StatusFrom2': data.statusFrom2,
                    'data.StatusTo2': data.statusTo2,
                    'data.StatusFrom3': data.statusFrom3,
                    'data.StatusTo3': data.statusTo3,
                    'data.Equals': data.equals,
                    'data.EmailTemplate': data.emailTemplate,
                    'data.EmailGroups': data.emailGroups,
                    'data.WorkingGroups': data.workingGroups,
                    'data.WorkingGroup': data.workingGroup,
                    'data.Administrators': data.administrators,
                    'data.Administrator': data.administrator,
                    'data.Recipients': data.recipients,
                    'data.CaseCreator': data.caseCreator,
                    'data.Initiator': data.initiator,
                    'data.CaseIsAbout': data.caseIsAbout,
                    'data.DisableFinishingType': data.disableFinishingType,
                    curTime: new Date().getTime()
                },
                function (result) {
                    if (result === "OK")
                        window.location.href = overviewRuleUrl;
                    else
                        ShowToastMessage(result, "error");
                }
            );
        };

        dhHelpdesk.businessRule.doValidation = function () {
            "use strict";
            let self = this;
            let $form = $("#newRule");
            return $form.validate().form();
        }

        dhHelpdesk.businessRule.setupEvent = function () {
            let selectedValue = $(elEventsDropDown).val();

            if (selectedValue === '1') {
                $(elCondition1).show();
                $(elCondition2).hide();
                $(elCondition3).hide();
                $(elCondition4).hide();

                $(elBRActionAdministratorSingleSelect).hide();
                $(elBRActionWorkingGroupSingleSelect).hide();

                $(elBRActionMailTemplate).show();
                $(elBRActionEmailGroup).show();
                $(elBRActionWorkingGroup).show();
                $(elBRActionAdministrator).show();
                $(elBRActionRecipients).show();
                $(elBRActionCreatedBy).show();
                $(elBRActionRegistrator).show();
                $(elBRActionAbout).show();
                $(elBRActionDisableFinishingType).hide();


            } else if (selectedValue === '2') {
                //OnCreateCaseM2T
                let wgCount = $('#lstWorkingGroupsSingle option').length;
                console.log("OnCreateCaseM2T - Antal workinggroups: " + wgCount);  // Output the length of options

                $(elCondition1).hide();
                $(elCondition2).show();
                $(elCondition3).hide();
                $(elCondition4).hide();

                $(elBRActionAdministratorSingleSelect).show();

                $(elBRActionMailTemplate).hide();
                $(elBRActionEmailGroup).hide();
                // Show WG if there is any
                if (wgCount > 2) {
                    console.log("Kunden Har wg:s");
                    $(elBRActionWorkingGroupSingleSelect).show();
                }
                else {
                    console.log("Kunden har Inga wg");
                    $(elBRActionWorkingGroupSingleSelect).hide();
                }

                $(elBRActionWorkingGroup).hide();
                $(elBRActionAdministrator).hide();
                $(elBRActionRecipients).hide();
                $(elBRActionCreatedBy).hide();
                $(elBRActionRegistrator).hide();
                $(elBRActionAbout).hide();
                $(elBRActionDisableFinishingType).hide();

            }
            else if (selectedValue === '3') {

                $(elCondition1).hide();
                $(elCondition2).hide();
                $(elCondition3).show();
                $(elCondition4).hide();


                $(elBRActionAdministratorSingleSelect).hide();
                $(elBRActionWorkingGroupSingleSelect).hide();
                $(elBRActionMailTemplate).hide();
                $(elBRActionEmailGroup).hide();
                $(elBRActionWorkingGroup).hide();
                $(elBRActionAdministrator).hide();
                $(elBRActionRecipients).hide();
                $(elBRActionCreatedBy).hide();
                $(elBRActionRegistrator).hide();
                $(elBRActionAbout).hide();
                $(elBRActionDisableFinishingType).show();

            }

            else if (selectedValue === '4') {

                $(elCondition1).hide();
                $(elCondition2).hide();
                $(elCondition3).hide();
                $(elCondition4).show();


                $(elBRActionAdministratorSingleSelect).hide();
                $(elBRActionWorkingGroupSingleSelect).hide();
                $(elBRActionMailTemplate).hide();
                $(elBRActionEmailGroup).hide();
                $(elBRActionWorkingGroup).hide();
                $(elBRActionAdministrator).hide();
                $(elBRActionRecipients).hide();
                $(elBRActionCreatedBy).hide();
                $(elBRActionRegistrator).hide();
                $(elBRActionAbout).hide();
                $(elBRActionDisableFinishingType).show();

            }
        };

        dhHelpdesk.businessRule.init = function () {
           
            let saveButton = $(elBtnSaveRule);

            saveButton.click(function () {
                let selectedValue = $(elEventsDropDown).val();

                let isWgMandatory = $(isWorkingGroupMandatory).val();
                let isAdmMandatory = $(isAdminMandatory).val();
                let wgGroup = $('#lstWorkingGroupsSingle').val();
                let admin = $('#lstAdministratorsSingle').val();
                let valid = true;

                console.log("Wg mandatory: " + isWgMandatory);
                console.log("Admin mandatory: " + isAdmMandatory);
                console.log("Workinggroup: " + wgGroup);
                console.log("Administrator: " + admin);
                
                if ((selectedValue == '2' && wgGroup === null && isWgMandatory === 'true')) {
                    $('#errorWG').show();
                    valid = false;
                }
                if ((selectedValue == '2' && admin === null && isAdmMandatory === 'true')) {
                    $('#errorAdmin').show();
                    valid = false;
                }
                if (valid) {
                    $('#errorWG').hide();
                    $('#errorAdmin').hide();
                    dhHelpdesk.businessRule.saveRule();
                }
            });

            $(".BR-chosen-select").chosen({
                width: "350px",
                'placeholder_text_multiple': placeholder_text_multiple, 
                'placeholder_text_single': placeholder_text_single,
                'no_results_text': no_results_text,
            }).change(function (evt, params) {
                $(evt.target).trigger("focusout");
            });

            $(".BR-chosen-single-select").chosen({
                width: "350px",
                'placeholder_text_multiple': placeholder_text_multiple,
                'placeholder_text_single': placeholder_text_single,
                'no_results_text': no_results_text,
            }).change(function (evt, params) {
                $(evt.target).trigger("focusout");
            });

            $(".BR-text").css("width", "335px");

            $(elEventsDropDown).change(function () {
                dhHelpdesk.businessRule.setupEvent();
            });
            $(document).on('change', '#lstWorkingGroupsSingle', function () {
                $('#errorWG').hide();
                $('#errorAdmin').hide();
                let workingGroupId = $(this).val(); 
                console.log("Ändrat WG: " +workingGroupId);  
                let customerId = $(elCustomerId).val();
                if (workingGroupId !== null) {
                    $.get(getAdmininstatorsForWorkingGroupUrl,
                        {
                            'customerId': customerId,
                            'workingGroupId': workingGroupId
                        },
                        function (result) { 
                            if (result.status === "OK") {  
                                updateAdministratorList(result.allAdmins);
                                if (result.isAdminMandatory === true) {
                                    $(isAdminMandatory).val(true);
                                    console.log("Admin is mandatory");
                                }
                            } else {
                                ShowToastMessage(result, "error");  
                            }
                        }
                    );
                }

            });
            $(document).on('change', '#lstEvents', function () {
                let selectedValue = $(elEventsDropDown).val();
                let customerId = $(elCustomerId).val();
                console.log("Event Changed to: " + selectedValue);
                $.get(getWorkingGroupsForCustomerUrl,
                    {
                        'customerId': customerId
                    },
                    function (result) { 
                        if (result.status === "OK") {  

                            updateWorkingGroupList(result.allWgs);
                            if (result.isWGMandatory === true) {
                                
                                $(isWorkingGroupMandatory).val(true);
                                console.log("WG is mandatory");
                            }
                        } else {
                            ShowToastMessage(result, "error");  
                        }
                    }
                );
                $.get(getAdministratorsForCustomersUrl,
                    {
                        'customerId': customerId
                    },
                    function (result) { 
                        if (result.status === "OK") {  
                            console.log("Hämtar Admins for Customer");
                            updateAdministratorList(result.allAdmins);
                            if (result.isAdminMandatory === true) {
                                $(isAdminMandatory).val(true);
                                console.log("Admin is mandatory");
                            }
                        } else {
                            ShowToastMessage(result, "error"); 
                        }
                    }
                );

            });
               

            dhHelpdesk.businessRule.setupEvent();
           
        }

    })($);


    dhHelpdesk.businessRule.init();
});
function updateAdministratorList(administrators) {
    let $adminDropdown = $('#lstAdministratorsSingle');  

    // Clear existing options
    $adminDropdown.empty();
    $adminDropdown.trigger('change'); 

    $adminDropdown.append($('<option>', {
        value: '',
        text: placeholder_text_single,
        selected: true,  // Make this option selected by default
        disabled: true   // Optionally disable this option so it can't be selected again
    }));

    $.each(administrators, function (i, administrator) {

        // Create the <option> element
        let option = $('<option>', {
            value: administrator.Value,  
            text: administrator.Text     
        });

        // Set the 'disabled' attribute only if administrator.Disabled is true
        if (administrator.Disabled) {
            option.prop('disabled', true);
        }

        // Set the 'selected' attribute only if administrator.Selected is true
        if (administrator.Selected) {
            option.prop('selected', true);
        }

        // Append the new <option> to the <select>
        $adminDropdown.append(option);
    });
    $adminDropdown.trigger('chosen:updated');
    // Log the number of options added for debugging
    console.log('Antal Admins för kund: ', $adminDropdown.find('option').length);
}
function updateWorkingGroupList(workgroups) {
    let $workgroupDropdown = $('#lstWorkingGroupsSingle');
    // Clear existing options
    $workgroupDropdown.empty();
    $workgroupDropdown.trigger('change');

    // Create the <option> element
    $workgroupDropdown.append($('<option>', {
        value: '',
        text: placeholder_text_single,
        selected: true,  // Make this option selected by default
        disabled: true   // Optionally disable this option so it can't be selected again
    }));

    $.each(workgroups, function (i, workgroup) {

        let option = $('<option>', {
            value: workgroup.Value,
            text: workgroup.Text
        });

        // Set the 'disabled' attribute only if administrator.Disabled is true
        if (workgroup.Disabled) {
            option.prop('disabled', true);
        }

        // Set the 'selected' attribute only if administrator.Selected is true
        if (workgroup.Selected) {
            option.prop('selected', true);
        }

        // Append the new <option> to the <select>
        $workgroupDropdown.append(option);
    });
    $workgroupDropdown.trigger('chosen:updated');
    //$workgroupDropdown.trigger('change');
    // Log the number of options added for debugging
    console.log('Antal WG:s:', $workgroupDropdown.find('option').length -1);
}

//Gammalt utkommenterat - ta bort?
//$(function () {
//    _ruleId = window.Params.ruleId;
//    _customerId = window.Params.customerId;
    
//    _rulePageAreaId = window.Params.rulePageAreaId;
//    _rulePageAreaTemplatePath = window.Params.rulePageAreaTemplatePath;

//    _ruleSectionId = window.Params.ruleSectionId;
//    _ruleSecTemplatePath = window.Params.ruleSecTemplatePath;

//    _conditionSectionId = window.Params.conditionSectionId;
//    _conditionSecTemplatePath = window.Params.conditionSecTemplatePath;
//    _conditionRowsTemplatePath = window.Params.conditionRowsTemplatePath;

//    _actionSectionId = window.Params.actionSectionId;            
//    _actionTemplatePath = window.Params.actionTemplatePath;
        
//    BR.StyleManager = {
                        
//        setFieldStyle: function(onChangeEvent){
//            var fieldClass = '.chosen-single-select.dropDownField';
//            $(fieldClass).chosen({
//                width: "100%",
//                height: "100px",
//                placeholder_text_single: "select an item",
//                search_contains: true,
//                'no_results_text': '?',
//            }).on('change', function(evt, params) {
//                onChangeEvent(this, params);                
//            });                
//        },

//        setFieldDataStyle: function(){
//            var fieldDataClass = '.chosen-select.dropDownFieldData';
//            $(fieldDataClass).chosen({
//                width: "100%",
//                height: "100px",
//                placeholder_text: "select an item",
//                search_contains: true,
//                'no_results_text': '?',
//            });
//        },

//        setCheckBoxStyle: function(){
//            var checkBox = '.switchcheckbox';
//            $(checkBox).bootstrapSwitch('onText', 'Yes');
//            $(checkBox).bootstrapSwitch('offText', 'No');
//            $(checkBox).bootstrapSwitch('size', 'small');
//            $(checkBox).bootstrapSwitch('onColor', 'success');
//        }
//    }

//    BR.Templates = {
//        RulePageTemplate: null,
//        RuleTemplate: null,
//        ConditionTemplate: null,
//        ConditionRowsTemplate: null
//    }

//    BR.Models = {

//        RulePageViewModel: function (ruleId, customerId) {
//            this.Id = ruleId;
//            this.CustomerId = customerId;

//            this.RuleSection = [];
//            this.ConditionSection = [];
//            this.ActionSection = [];            
//        },

//        RuleSecViewModel: function (sectionId) {            
//            this.SectionCaption = "";
//            this.Id = 1;
//            this.Name = "Rule Name 1";
//            this.Event = null;
//            this.Sequence = null;
//            this.ContinueOnSuccess = true;
//            this.ContinueOnError = true;
//            this.Status = null;                      
//        },

//        ConditionSecViewModel: function () {
//            this.SectionCaption = "";
//            this.Conditions = [];

//            this.AddCondition = function (condition) {
//                this.Conditions.push(condition);
//            }
//        },

//        ConditionRowModel: function () {
//            this.Id = 1;
//            this.Field = null;
//            this.FromValue = null;
//            this.ToValue = null;
//            this.Sequence = null;
//            this.Status = null;
//        },

//        ActionViewModel: function () {
//            this.Id = null;
//            this.Field = null;
//            this.FromValue = null;
//            this.ToValue = null;
//            this.Sequence = null;
//            this.Status = null;
//        },           

//        ActionSectionModel: function (sectionId, templatePath) {
//            this.SectionCaption = "";
//            this.Actions = [];

//            this.AddAction = function (action) {
//                this.Actions.push(action);
//                this.Draw();
//            };

//            this.DrawSection = function () {
//            }

//        }
//    }

//    BR.Views = {
        
//        BusinessRulePage: function () {
//            rulePage = this;
                                               
//            var loadRulePageTemplate = function () {
//                return $.get(_rulePageAreaTemplatePath, function (rulePageTemplate) {
//                    BR.Templates.RulePageTemplate = $.templates("rulePageTemp", rulePageTemplate);
//                });
//            };

//            var loadRuleTemplate = function () {                
//                return $.get(_ruleSecTemplatePath, function (ruleSecTemplate) {
//                    BR.Templates.RuleTemplate = $.templates("ruleSecTemp", ruleSecTemplate);
//                });
//            };

//            var loadConditionTemplate = function () {                
//                return $.get(_conditionSecTemplatePath, function (conditionSecTemplate) {
//                    BR.Templates.ConditionTemplate = $.templates("conditionSecTemp", conditionSecTemplate);
//                });
//            };

//            var loadConditionRowsTemplate = function () {
//                return $.get(_conditionRowsTemplatePath, function (conditionRowsTemplate) {
//                    BR.Templates.ConditionRowsTemplate = $.templates("conditionRows", conditionRowsTemplate);
//                });
//            };
            
//            var loadAllTemplates = function() {
//                loadRulePageTemplate()
//                    .then(loadRuleTemplate)
//                    .then(loadConditionTemplate)
//                    .then(loadConditionRowsTemplate)
//                    .then(function () {

//                        var rulePageModel = new BR.Models.RulePageViewModel(_ruleId, _customerId);
                        
                        
//                        var ruleSecModel = new BR.Models.RuleSecViewModel();
//                        rulePageModel.RuleSection = [];
//                        rulePageModel.RuleSection.push(ruleSecModel);

//                        var conditionSecModel = new BR.Models.ConditionSecViewModel();
//                        conditionSecModel.SectionCaption = 'Conditions';
//                        rulePageModel.ConditionSection = [];
                        
//                        var conditionRowModel = new BR.Models.ConditionRowModel();
//                        conditionRowModel.Id = 1;
//                        conditionSecModel.Conditions.push(conditionRowModel);

//                        var conditionRowModel = new BR.Models.ConditionRowModel();
//                        conditionRowModel.Id = 2;
//                        conditionSecModel.Conditions.push(conditionRowModel);

//                        rulePageModel.ConditionSection.push(conditionSecModel);

//                        rulePage.drawPage(rulePageModel);                        
//                    });
//            };

//            this.drawPage = function(pageModel){
//                $(_rulePageAreaId).html(BR.Templates.RulePageTemplate.render(pageModel));
//                this.drawRuleSection(pageModel.RuleSection);
//                this.drawConditionSection(pageModel.ConditionSection);
//            }

//            this.drawRuleSection = function (models) {                
//                if (BR.Templates.RuleTemplate != null)
//                    $(_ruleSectionId).html(BR.Templates.RuleTemplate.render(models));
//            }

//            this.drawConditionSection = function (models) {
//                if (models.length > 0) {
//                    for (var mo = 0; mo < models.length; mo++) {
//                        var model = models[mo];
//                        if (BR.Templates.ConditionTemplate != null) {
//                            var container = $($(_conditionSectionId).html(BR.Templates.ConditionTemplate.render(model)));

//                            var rows = container.find(".rule-condition-rows");
//                            rows.html('');

//                            for (var i = 0; i < model.Conditions.length; i++) {
//                                var row = $(BR.Templates.ConditionRowsTemplate.render(model.Conditions[i]));
//                                rows.append(row);
//                            }
//                        }
//                    }
//                }

//                BR.StyleManager.setFieldStyle(this.onChangeField);
//                BR.StyleManager.setFieldDataStyle();
//                BR.StyleManager.setCheckBoxStyle();
               
//            }

//            this.Initialize = function () {
//                loadAllTemplates();
//            };

//            this.onChangeField = function (sender, params) {
//                var selectedValue = params.selected;
                

//                var tt = $(sender).chosen();
//                var selectedId = sender.selectedIndex;
//                alert(selectedValue);
//            };

//            this.SavePage = function () {
//                // Save to database
//            };
//        }
//    }

//    BR.Controllers = {        
//        GenerateBR: function () {
//            var BRPage = new BR.Views.BusinessRulePage();
//            BRPage.Initialize();           
//        }        
//    }   

//    var loadPage = function () {        
//        var p = BR.Controllers.GenerateBR();
//    };

//    loadPage();
    
//});