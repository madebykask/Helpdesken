$(function () {
        
    (function ($) {
        window.helpdesk = window.helpdesk || {};
        window.helpdesk.caseRule = window.helpdesk.caseRule || {};
        var params  = window.params;

        var ruleModel = null;
        var $elementsHaveRule = $('.acceptRules');
        var $fieldStateChanger = $('.fieldStateChanger');
        var STANDARD_ID = 'standardid';
        var STATE_ICON_PLACE_PREFIX = '#stateIconPlace_';
              
        /***** Enums *****/
        var _RULE_TYPE = {
            Original: 0,
            NewMode : 1,
            InheritMode : 2,
            SelfService : 3
        };

        var _RELATION_TYPE = {
            OneToOne:1,
            OneToMany:2,
            ManyToMany:3
        };

        var _ACTION_TYPE = {
            ValueSetter: 1,
            ListCleaner: 2,
            ListPopulator: 3,
            StaticRuntimeAction: 9
        };

        var _FIELD_TYPE = {
            TextField: 1,
            SingleSelectField: 2,
            MultiSelectField: 3,
            TreeButtonSelect: 4,
            TextArea: 5,
            CheckBox: 6,
            DateField: 7,
            ElementsGroup: 8,
            ButtonField: 9
        };

        var _FIELD_STATUS_TYPE ={
            Editable:1,
            Readonly: 2,
            Hidden: 3
        };

        var _FORIEGN_DATA_NUMBER ={
            Place1: 1,
            Place2: 2,
            Place3: 3
        }                   


        helpdesk.common = {       
            data: function(){
                this.isNullOrUndefined = function(value){
                    if (value == null || value == undefined)
                        return true;
                    
                    return false;
                }

                this.isNullOrEmpty = function (value) {
                    if (value == null || value == undefined || value == "")
                        return true;

                    return false;
                }
            },

            convertor: function () {
                this.toBoolStr = function (value) {
                    if (value == null || value == undefined )
                        return "False";
                    
                    if (value == 0 || !value || value == "0" || value == "")
                        return "False";

                    return "True";
                }
            }


        }

        helpdesk.caseRuleModels = {
            /* Models */
            FieldItem: function(){
                this.ItemValue = ""; 

                this.ItemText = "";

                this.IsActive = true;

                this.ForeignKeyValue1 = null;

                this.ForeignKeyValue2 = null;

                this.ForeignKeyValue3 = null;

                this.ParentItemValue = "";
            }
        }

        helpdesk.caseRule = {
                      
            /* helpers */ 
            dataHelper: null,
            convertor: null,

            init:function() {
                ruleModel = params.ruleModel;
                dataHelper = new helpdesk.common.data();
                convertor = new helpdesk.common.convertor();
               
                $elementsHaveRule.on('switchChange.bootstrapSwitch', function () {
                    /* Used for bootstrap checkbox */
                    helpdesk.caseRule.elementValueChanged(this);
                });

                $elementsHaveRule.change(function () {
                    helpdesk.caseRule.onElementValueChanged(this);
                });

                $fieldStateChanger.change(function () {
                    helpdesk.caseRule.onStateChanged(this);
                });

                helpdesk.caseRule.initiateFiedStateIcons();
            },

            initiateFiedStateIcons: function(){
                if (dataHelper.isNullOrUndefined(ruleModel) ||
                    dataHelper.isNullOrUndefined(ruleModel.FieldAttributes) ||
                    ruleModel.FieldAttributes.length <= 0)
                    return null;
                
                for (var _fi = 0; _fi < ruleModel.FieldAttributes.length; _fi++) {
                    var field = ruleModel.FieldAttributes[_fi];
                    var element = helpdesk.caseRule.getElementByFieldId(field.FieldId);
                    if (!dataHelper.isNullOrUndefined($(element))) {
                        helpdesk.caseRule.updateElementValue($(element));
                    }
                    //helpdesk.caseRule.updateFieldValue(field, field.Selected.ItemValue);
                    //helpdesk.caseRule.refreshStateIcons(field, false);
                }
            },

            onElementValueChanged: function (element) {
                var $self = $(element);                
                helpdesk.caseRule.updateElementValue($self);
                var elmField = helpdesk.caseRule.getFieldByElement($self);
                if (!dataHelper.isNullOrUndefined(elmField)) {
                    var parentRelatedFields = helpdesk.caseRule.getFieldsHaveRelationTo(elmField.FieldId);
                    if (dataHelper.isNullOrUndefined(parentRelatedFields) || parentRelatedFields.length <= 0)
                        return;
                    for (var pr = 0; pr < parentRelatedFields.length; pr++) {
                        var relatedElm = helpdesk.caseRule.getElementByFieldId(parentRelatedFields[pr].FieldId);
                        if (!dataHelper.isNullOrUndefined(relatedElm))
                            helpdesk.caseRule.updateElementValue($(relatedElm));
                    }
                }
            },

            onStateChanged: function (element) {
                var $self = $(element);
                var newState = $self.val();
                var field = this.getFieldByElement($self);
                if (!dataHelper.isNullOrUndefined(field)) {
                    switch (newState) {
                        case "1":
                            field.StatusType = _FIELD_STATUS_TYPE.Editable;
                            break;

                        case "2":
                            field.StatusType = _FIELD_STATUS_TYPE.Readonly;
                            break;

                        case "3":
                            field.StatusType = _FIELD_STATUS_TYPE.Hidden;
                            break;

                        default:
                            field.StatusType = _FIELD_STATUS_TYPE.Editable;
                    }

                    helpdesk.caseRule.refreshStateIcons(field);
                }                
            },
            
            getFieldById: function(fieldId){
                if (dataHelper.isNullOrUndefined(ruleModel) || 
                    dataHelper.isNullOrUndefined(ruleModel.FieldAttributes) ||
                    dataHelper.isNullOrEmpty(fieldId) ||
                    ruleModel.FieldAttributes.length <= 0)
                    return null;

                fieldId = fieldId.toLowerCase();
                for(var fi=0; fi<ruleModel.FieldAttributes.length; fi++)
                    if (ruleModel.FieldAttributes[fi].FieldId.toLowerCase() == fieldId)
                    {
                        return ruleModel.FieldAttributes[fi];
                    }

                return null;
            },

            getFieldByElement: function ($element) {
                if (dataHelper.isNullOrUndefined($element))
                    return null;

                var fieldId = $element.attr(STANDARD_ID);

                if (fieldId == "")
                    return null;

                return this.getFieldById(fieldId);                
            },

            getElementByFieldId: function (fieldId) {
                var elms = $(document).find("[" + STANDARD_ID + "='" + fieldId + "']");
                if (!dataHelper.isNullOrUndefined(elms) && elms.length > 0)
                    return elms[0];
                else
                    return null;
            },

            getFieldsHaveRelationTo: function(fieldId){
                var ret = [];
                for (var hv = 0; hv < ruleModel.FieldAttributes.length; hv++) {
                    var field = ruleModel.FieldAttributes[hv];
                    if (!dataHelper.isNullOrEmpty(field)) {
                        if (field.FieldId != fieldId && field.Relations.length > 0) {
                            for (var fr = 0; fr < field.Relations.length; fr++) {
                                if (field.Relations[fr].FieldId == fieldId) {
                                    ret.push(field);
                                }
                            }
                        }
                    }
                }
                return ret;
            },

            updateElementValue: function ($element) {                
                var field = this.getFieldByElement($element);
                if (dataHelper.isNullOrEmpty(field))
                    return;

                var curValue = null;
                switch (field.FieldType) {

                    case _FIELD_TYPE.TextField:
                        curVal = $element.val();                        
                        break;

                    case _FIELD_TYPE.CheckBox:
                        curVal = $element.prop('checked');                        
                        break;

                    case _FIELD_TYPE.SingleSelectField:
                        curVal = $element.val();                        
                        break;

                    case _FIELD_TYPE.TreeButtonSelect:
                        curVal = $element.val();                        
                        break;

                    case _FIELD_TYPE.TextArea:
                        curVal = $element.val();
                        break;

                    case _FIELD_TYPE.DateField:                        
                        curVal = $element.val();
                        break;
                }

                helpdesk.caseRule.updateFieldValue(field, curVal);
                if (field.Relations != null && field.Relations.length > 0) {
                    for (var ri = 0; ri < field.Relations.length; ri++) {
                        var relation = field.Relations[ri];
                        var elm = helpdesk.caseRule.getElementByFieldId(relation.FieldId);
                        if (elm != null) {
                            helpdesk.caseRule.updateElementValue($(elm));
                        }
                    }
                }
            },

            updateFieldValue: function (field, curVal) {
                if (dataHelper.isNullOrUndefined(field) || curVal == undefined)
                    return;
                
                switch (field.FieldType) {

                    case _FIELD_TYPE.TextField:                        
                        field.Selected.ItemValue = curVal;
                        break;

                    case _FIELD_TYPE.CheckBox:                        
                        field.Selected.ItemValue = convertor.toBoolStr(curVal);
                        break;

                    case _FIELD_TYPE.SingleSelectField:                        
                        field.Selected = helpdesk.caseRule.getItemByValue(field, curVal);
                        break;

                    case _FIELD_TYPE.TreeButtonSelect:                        
                        field.Selected = helpdesk.caseRule.getItemByValue(field, curVal);
                        if (!dataHelper.isNullOrEmpty(curVal))
                            field.Selected.ItemText = helpdesk.caseRule.resolveTreeName(field, curVal);
                        break;

                    case _FIELD_TYPE.TextArea:
                        field.Selected.ItemValue = curVal;
                        break;

                    case _FIELD_TYPE.DateField:
                        field.Selected.ItemValue = curVal;
                        break;
                }

                helpdesk.caseRule.refreshStateIcons(field, false);
            },
                        
            refreshStateIcons: function (field, preventCycle) {
                var $elementToApply = $(STATE_ICON_PLACE_PREFIX + field.FieldId);
                if (!dataHelper.isNullOrUndefined($elementToApply)) {
                    var iconModel = helpdesk.caseRule.createStateIconModel(field, preventCycle);
                    $elementToApply.html(iconModel);

                    $('body').tooltip({
                        selector: '.tooltipType'
                    });

                }
            },

            createStateIconModel: function (field, preventCycle) {
                if (dataHelper.isNullOrUndefined(field))
                    return "";

                var fieldInfoClass = "";                
                var fieldInfoHint = "";               

                var cuFielInfo = helpdesk.caseRule.checkRules(field, preventCycle);
                if (!dataHelper.isNullOrEmpty(cuFielInfo)) {
                    fieldInfoClass = "icon-info-sign tooltipType";
                    fieldInfoHint = cuFielInfo;
                }

                var mandatoryClass = field.IsMandatory ? "icon-asterisk tooltipType" : "";
                var mandatoryHint = params.mandatoryText;

                var selfSeviceClass = field.IsAvailableOnSelfService ? "icon-share tooltipType" : "";
                var selfSeviceHint = params.showOnSelfSeviceText;

                var ret =
                    '<td style="width:80px;text-align:center"><span title="" class="' + fieldInfoClass + '" data-original-title="' + fieldInfoHint + '" data-html="true" rel="tooltip"></span></td>' +
                    '<td style="width:80px;text-align:center"><span title="" class="' + mandatoryClass + '" data-original-title="' + mandatoryHint + '" data-html="true" rel="tooltip"></span></td>' +
                    '<td style="width:80px;text-align:center"><span title="" class="' + selfSeviceClass + '" data-original-title="' + selfSeviceHint + '" data-html="true" rel="tooltip"></span></td>';

                return ret;
            },

            checkRules: function (field, preventCycle) {
                //if (field.StatusType == _FIELD_STATUS_TYPE.Hidden)
                //    return "";
                
                var ret = "";
                if (field.Relations.length > 0) {
                    for (var r = 0; r < field.Relations.length; r++) {
                        var curRelation = field.Relations[r];                            
                        ret += helpdesk.caseRule.predictAction(field, curRelation, preventCycle);
                    }
                    return ret;
                }
                
                return "";
            },

            predictAction: function (field, relation, preventCycle) {
                if (relation.StaticActionId != null) {
                    switch (relation.StaticActionId) {
                        case 1:
                            if (preventCycle) {                                
                                return helpdesk.caseRule.staticRuleAction1();
                            } else {
                                switch (field.FieldId) {
                                    case "Impact_Id":
                                        helpdesk.caseRule.refreshStateIcons(helpdesk.caseRule.getFieldById("Urgency_Id"), true);
                                        helpdesk.caseRule.refreshStateIcons(helpdesk.caseRule.getFieldById("System_Id"), true);
                                        return helpdesk.caseRule.staticRuleAction1();
                                        break;
                                    case "Urgency_Id":
                                        helpdesk.caseRule.refreshStateIcons(helpdesk.caseRule.getFieldById("Impact_Id"), true);
                                        helpdesk.caseRule.refreshStateIcons(helpdesk.caseRule.getFieldById("System_Id"), true);
                                        return helpdesk.caseRule.staticRuleAction1();
                                        break;
                                    case "System_Id":
                                        helpdesk.caseRule.refreshStateIcons(helpdesk.caseRule.getFieldById("Impact_Id"), true);
                                        helpdesk.caseRule.refreshStateIcons(helpdesk.caseRule.getFieldById("Urgency_Id"), true);
                                        return helpdesk.caseRule.staticRuleAction1();
                                        break;
                                }
                            }                            
                    }
                }

                var selectedItem = field.Selected;
                var relatedField = helpdesk.caseRule.getFieldById(relation.FieldId);                

                if (!dataHelper.isNullOrEmpty(relatedField.Selected.ItemValue))
                    return "";
                                
                switch (relation.RelationType) {
                    case _RELATION_TYPE.OneToOne:
                        var fData = helpdesk.caseRule.getForiegnData(relation.RelationType, selectedItem, relation.ForeignKeyNumber, relatedField);
                        if (fData != null && !dataHelper.isNullOrEmpty(fData.ItemText)) {                            
                            return "<div align='left'> <b>" + relatedField.FieldCaption + "</b>: " + params.willSetToText + " <b>" + fData.ItemText + "</b> </div> <br />";
                        }
                        else
                            return "";

                    case _RELATION_TYPE.OneToMany:
                        if (!dataHelper.isNullOrEmpty(selectedItem.ItemText))
                            return "<div align='left'> <b>" + relatedField.FieldCaption + "</b>: " + params.willShowRelatedItemsText + " <b>" + selectedItem.ItemText + "</b> </div> <br />";
                        else
                            return "";
                                

                    case _RELATION_TYPE.ManyToMany:
                        if (!dataHelper.isNullOrEmpty(selectedItem.ItemText))
                            return "<div align='left'> <b>" + relatedField.FieldCaption + "</b>: " + params.willShowRelatedItemsText + " <b>" + selectedItem.ItemText + "</b> </div> <br />";
                        else
                            return "";                        
                }                                
            },

            getForiegnData: function (relationType, parentSelectedItem, place, relatedField){
                switch (relationType) {
                    case _RELATION_TYPE.OneToOne:
                        if (place == _FORIEGN_DATA_NUMBER.Place1){
                            return helpdesk.caseRule.getItemByValue(relatedField, parentSelectedItem.ForeignKeyValue1)                            
                        }
                        else if (place == _FORIEGN_DATA_NUMBER.Place2) {
                            return helpdesk.caseRule.getItemByValue(relatedField, parentSelectedItem.ForeignKeyValue2)
                        }
                        else if (place == _FORIEGN_DATA_NUMBER.Place3) {
                            return helpdesk.caseRule.getItemByValue(relatedField, parentSelectedItem.ForeignKeyValue3)
                        }
                        break;

                    case _RELATION_TYPE.OneToMany:
                        break;

                    case _RELATION_TYPE.ManyToMany:
                        break;

                }
                return null;
            },

            getItemByValue: function (field, itemValue) {
                var ret = new helpdesk.caseRuleModels.FieldItem();
                if (dataHelper.isNullOrUndefined(field))
                    return ret;

                if (field.FieldType == _FIELD_TYPE.TextField ||
                    field.FieldType == _FIELD_TYPE.TextArea ||
                    field.FieldType == _FIELD_TYPE.DateField ||
                    field.FieldType == _FIELD_TYPE.ButtonField) {
                    ret.ItemValue = itemValue;
                    ret.ItemText = itemValue;
                    return ret;
                }

                if (field.FieldType == _FIELD_TYPE.CheckBox) {
                    ret.ItemValue = itemValue.toBoolStr();
                    ret.ItemText = itemValue;
                    return ret;
                }

                if (field == null || field.Items == null || field.Items.length <= 0)
                    return new helpdesk.caseRuleModels.FieldItem();

                for (var fiv = 0; fiv < field.Items.length; fiv++) {
                    if (field.Items[fiv].ItemValue == itemValue)
                        return field.Items[fiv];
                }

                return ret;
            },

            resolveTreeName: function (field, itemValue) {
                if (dataHelper.isNullOrEmpty(field) && field.Items == null && field.Items.length <= 0)
                    return "";

                for (var fit = 0; fit < field.Items.length; fit++) {
                    if (field.Items[fit].ItemValue == itemValue) {
                        if (dataHelper.isNullOrEmpty(field.Items[fit].ParentItemValue))
                            return field.Items[fit].ItemText;
                        else
                            return this.resolveTreeName(field, field.Items[fit].ParentItemValue) + " - " + field.Items[fit].ItemText;
                    }
                }

                return "";
            }

            ,
            /* Static Rule Actions */
            staticRuleAction1: function(){
                var impactField = helpdesk.caseRule.getFieldById('Impact_Id');
                var urgentField = helpdesk.caseRule.getFieldById('Urgency_Id');
                var priorityField = helpdesk.caseRule.getFieldById('Priority_Id');

                if (!dataHelper.isNullOrUndefined(impactField) && !dataHelper.isNullOrUndefined(urgentField) && !dataHelper.isNullOrUndefined(priorityField)) {
                    if (!dataHelper.isNullOrEmpty(impactField.Selected.ItemValue) && 
                        !dataHelper.isNullOrEmpty(urgentField.Selected.ItemValue) &&
                        !dataHelper.isNullOrEmpty(priorityField.FieldCaption)) {
                        return "<div align='left'> " + params.migthSetText + " <b>" + priorityField.FieldCaption + "</b> </div> <br />";                       
                    }
                }

                return "";
            }
        }
                      
    })($);
       
    helpdesk.caseRule.init();    
});