

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
        
        //<td style="width:80px;text-align:center"><i class="icon-info-sign"></i> &nbsp; &nbsp;</td>
        //<td style="width:80px;text-align:center"><i class=""></i> &nbsp; &nbsp;</td>
        //<td style="width:80px;text-align:center"><i class="icon-share"></i> &nbsp; &nbsp;</td>

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
            ListPopulator: 3
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
                
                for (var fi = 0; fi < ruleModel.FieldAttributes.length; fi++) {
                    helpdesk.caseRule.refreshStateIcons(ruleModel.FieldAttributes[fi]);
                }
            },

            onElementValueChanged: function (element) {
                var $self = $(element);
                helpdesk.caseRule.updateFieldValue($self);                
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
                }

                helpdesk.caseRule.refreshStateIcons(field);
            },

            refreshStateIcons: function (field) {
                var $elementToApply = $(STATE_ICON_PLACE_PREFIX + field.FieldId);
                if (!dataHelper.isNullOrUndefined($elementToApply)) {
                    var iconModel = helpdesk.caseRule.createStateIconModel(field);                    
                    $elementToApply.html(iconModel);

                    $('body').tooltip({
                        selector: '.tooltipType'
                    });
                }
            },

            getFieldById: function(fieldId){
                if (dataHelper.isNullOrUndefined(ruleModel) || 
                    dataHelper.isNullOrUndefined(ruleModel.FieldAttributes) ||
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

            updateFieldValue: function($element){
                var field = this.getFieldByElement($element);
                if (dataHelper.isNullOrUndefined(field))
                    return;
                
                switch (field.FieldType) {

                    case _FIELD_TYPE.TextField:
                        var curVal = $element.val();
                        field.Selected.ItemValue = curVal;
                        break;

                    case _FIELD_TYPE.CheckBox:
                        var curVal = $element.prop('checked');
                        field.Selected.ItemValue = convertor.toBoolStr(curVal);
                        break;

                    case _FIELD_TYPE.SingleSelectField:
                        var curVal = $element.val();
                        field.Selected = helpdesk.caseRule.getItemByValue(field, curVal);
                        break;

                    case _FIELD_TYPE.TreeButtonSelect:
                        var curVal = $element.val();
                        field.Selected = helpdesk.caseRule.getItemByValue(field, curVal);
                        if (!dataHelper.isNullOrEmpty(curVal))
                            field.Selected.ItemText = helpdesk.caseRule.resolveTreeName(field, curVal);
                        break;
                }

                helpdesk.caseRule.refreshStateIcons(field);

            },

            getItemByValue: function (field, itemValue) {
                if (field == null && field.Items == null && field.Items.length <= 0)
                    return new helpdesk.caseRuleModels.FieldItem();

                for (var fiv = 0; fiv < field.Items.length; fiv++)
                {
                    if (field.Items[fiv].ItemValue == itemValue)
                        return field.Items[fiv];
                }

                return new helpdesk.caseRuleModels.FieldItem();
            },

            resolveTreeName: function (field, itemValue) {
                if (field == null && field.Items == null && field.Items.length <= 0)
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
            },
            
            createStateIconModel: function (field) {
                if (dataHelper.isNullOrUndefined(field))
                    return "";

                var fieldInfoClass = "";                
                var fieldInfoHint = "";
                var cuFielInfo = helpdesk.caseRule.checkRules(field);
                if (!dataHelper.isNullOrEmpty(cuFielInfo)) {
                    fieldInfoClass = "icon-info-sign tooltipType";
                    fieldInfoHint = cuFielInfo;
                }

                var mandatoryClass = field.IsMandatory ? "icon-asterisk tooltipType" : "";
                var mandatoryHint = "";

                var selfSeviceClass = field.IsAvailableOnSelfService ? "icon-share tooltipType" : "";
                var selfSeviceHint = "";

                var ret =
                    '<td style="width:70px;text-align:center"><span title="" class="' + fieldInfoClass + '" data-original-title="' + fieldInfoHint + '" rel="tooltip"></span></td>' + 
                    '<td style="width:70px;text-align:center"><span title="" class="' + mandatoryClass + '" data-original-title="' + mandatoryHint + '" rel="tooltip"></span></td>' +
                    '<td style="width:70px;text-align:center"><span title="" class="' + selfSeviceClass + '" data-original-title="' + selfSeviceHint + '" rel="tooltip"></span></td>';

                return ret;
            },

            checkRules: function (field) {
                if (field.StatusType == _FIELD_STATUS_TYPE.Hidden)
                    return "";
                else {
                    var ret = "";
                    if (field.Relations.length > 0) {
                        for (var r = 0; r < field.Relations.length; r++) {
                            var curRelation = field.Relations[r];                            
                            ret += helpdesk.caseRule.predictAction(field, curRelation);
                        }
                        return ret;
                    }
                }
                return "";
            },

            predictAction: function (field, relation) {
                var selectedItem = field.Selected;
                var relatedField = helpdesk.caseRule.getFieldById(relation.FieldId);
                var fData = helpdesk.caseRule.getForiegnData(relation.RelationType, selectedItem, relation.ForeignKeyNumber, relatedField);
                if (fData != null && !dataHelper.isNullOrEmpty(fData.ItemText))
                    return "Will set " + relatedField.FieldCaption + " to: " + fData.ItemText;
                else
                    return "";
            },

            getForiegnData: function (relationType, parentSelectedItem, place, relatedField){
                switch (relationType) {
                    case _RELATION_TYPE.OneToOne:
                        if (place == _FORIEGN_DATA_NUMBER.Place1){
                            return helpdesk.caseRule.getItemByValue(relatedField, parentSelectedItem.ForeignKeyValue1)                            
                        }
                        break;

                    case _RELATION_TYPE.OneToMany:
                        break;

                    case _RELATION_TYPE.ManyToMany:
                        break;

                }
                return null;
            }
        }
                      
    })($);
       
    //helpdesk.caseRule.init();    
});