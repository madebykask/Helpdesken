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
         
        /***** Const ****/
        var _CURRENT_RUN_TIME_ITEM_ID = "-1";
        var _ARRAY_SEPARATOR_CHARACTER = ",";
        var _NULL_STR = "null";

        /***** Enums *****/
        var _RULE_MODE = {
            TemplateUserChangeMode   : 0,
            CaseUserChangeMode       : 1,
            CaseInheritTemplateMode  : 2,
            CaseNewTemplateMode      : 3,
            SelfService              : 4
        };

        var _RELATION_TYPE = {
            OneToOne:1,
            OneToMany:2,
            ManyToMany: 3,
            Virtual : 4
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

        var _FOREIGN_DATA_NUMBER = {
            Place0: 0, // It refers to Field.Selected.ItemValue
            Place1: 1,
            Place2: 2,
            Place3: 3
        };

        var _VIRTUAL_DATA_STORE ={
            Store1: 1,
            Store2: 2,
            Store3: 3
        } 

        var _CONDITION_OPERATOR = {
            HasValue    : 1,
            HasNotValue : 2,
            Equal       : 3,
            NotEqual    : 4,            
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

                this.arrayContains = function (strArray, value) {
                    if (strArray == null || strArray.length <= 0)
                        return false;

                    return $.inArray(value, strArray) > -1;
                }
            },

            convertor: function () {
                this.toBoolStr = function (value) {
                    if (value == null || value == undefined )
                        return "False";
                    
                    if (value == 0 || !value || value == "0" || value == "")
                        return "False";

                    return "True";
                },

                this.toBool = function (value) {
                    if (value == null || value == undefined)
                        return false;

                    if (value == 0 || !value || value == "0" || value == "")
                        return false;

                    return true;
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

                this.ResultKeyValue = null;

                this.ParentItemValue = "";
            }
        }

        helpdesk.caseRule = {
                      
            /* helpers */

            _GLOBAL_RULE_STATE_MODE: -1,
            dataHelper: null,
            convertor: null,
            _DATE_FORMAT: '',

            init:function() {
                ruleModel = params.ruleModel;
                this._GLOBAL_RULE_STATE_MODE = ruleModel.RuleMode;
                dataHelper = new helpdesk.common.data();
                convertor = new helpdesk.common.convertor();
                this._DATE_FORMAT = ruleModel.DateFormat;

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

                helpdesk.caseRule.updateAllFields();
                helpdesk.caseRule.refreshAllStateIcons();
            },

            updateAllFields: function () {
                if (dataHelper.isNullOrUndefined(ruleModel) ||
                    dataHelper.isNullOrUndefined(ruleModel.FieldAttributes) ||
                    ruleModel.FieldAttributes.length <= 0)
                    return null;

                // Update fields
                for (var _fi = 0; _fi < ruleModel.FieldAttributes.length; _fi++) {
                    var field = ruleModel.FieldAttributes[_fi];
                    var element = helpdesk.caseRule.getElementByFieldId(field.FieldId);
                    if (!dataHelper.isNullOrUndefined($(element))) {
                        helpdesk.caseRule.updateFieldByElementValue($(element));
                    }
                }                                
            },
           
            refreshAllStateIcons: function () {
                // Refresh icon states
                if (this._GLOBAL_RULE_STATE_MODE == _RULE_MODE.TemplateUserChangeMode) {
                    for (var _fi = 0; _fi < ruleModel.FieldAttributes.length; _fi++) {
                        var field = ruleModel.FieldAttributes[_fi];
                        helpdesk.caseRule.refreshStateIcons(field);
                    }
                }
            },

            onElementValueChanged: function (element) {
                this.updateAllFields();

                var field = this.getFieldByElement($(element));
                if (!dataHelper.isNullOrUndefined(field))
                    helpdesk.caseRule.applyRules(field);
                                
                this.refreshAllStateIcons();
            },

            onStateChanged: function (element) {
                var $self = $(element);
                if (dataHelper.isNullOrUndefined($self))
                    return;

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
            },                                              

            updateFieldByElementValue: function($element){
                var field = this.getFieldByElement($element);
                if (dataHelper.isNullOrEmpty(field))
                    return;

                var curVal = null;
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
            },

            updateFieldValue: function (field, curVal) {
                if (dataHelper.isNullOrUndefined(field))
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
            },
                        
            refreshStateIcons: function (field) {
                if (dataHelper.isNullOrUndefined(field))
                    return;

                var $elementToApply = $(STATE_ICON_PLACE_PREFIX + field.FieldId);
                if (!dataHelper.isNullOrUndefined($elementToApply)) {
                    var iconModel = helpdesk.caseRule.createStateIconModel(field);
                    $elementToApply.html(iconModel);

                    $('body').tooltip({
                        selector: '.tooltipType'
                    });
                }
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
                var mandatoryHint = params.mandatoryText;

                var selfSeviceClass = field.IsAvailableOnSelfService ? "icon-share tooltipType" : "";
                var selfSeviceHint = params.showOnSelfSeviceText;

                var ret =
                    '<td style="width:80px;text-align:center"><span title="" class="' + fieldInfoClass + '" data-original-title="' + fieldInfoHint + '" data-html="true" rel="tooltip"></span></td>' +
                    '<td style="width:80px;text-align:center"><span title="" class="' + mandatoryClass + '" data-original-title="' + mandatoryHint + '" data-html="true" rel="tooltip"></span></td>' +
                    '<td style="width:80px;text-align:center"><span title="" class="' + selfSeviceClass + '" data-original-title="' + selfSeviceHint + '" data-html="true" rel="tooltip"></span></td>';

                return ret;
            },            

            isActionApplicableForCurrentMode: function(relation){
                var acceptModes = relation.ApplicableIn;
                if (dataHelper.arrayContains(acceptModes, this._GLOBAL_RULE_STATE_MODE)) {
                    return true;
                }
                return false;
            },

            checkRules: function (field) {
                var ret = "";

                if (dataHelper.isNullOrUndefined(field))
                    return ret;

                if (field.Relations.length > 0) {
                    for (var r = 0; r < field.Relations.length; r++) {
                        var curRelation = field.Relations[r];
                        if (this.checkConditions(curRelation.Conditions)) {
                            ret += helpdesk.caseRule.predictAction(field, curRelation);
                        }
                    }
                    return ret;
                }

                return "";
            },
           
            /* Return predicted action text */
            predictAction: function (field, relation) {
                
                if (dataHelper.isNullOrUndefined(field))
                    return "";

                // No information to show
                if (relation.GeneralInformation == "" && !relation.ShowDetailsInformation)
                    return "";

                var selectedItem = field.Selected;
                var relatedField = this.getFieldById(relation.FieldId);

                if (relation.RelationType == _RELATION_TYPE.Virtual)
                {
                    if (!dataHelper.isNullOrEmpty(relation.ResultDataKey)) {
                        var resultField = this.getFieldById(relation.ResultDataKey);
                        if (!dataHelper.isNullOrEmpty(resultField) &&
                            !dataHelper.isNullOrEmpty(resultField.Selected.ItemValue) &&
                            resultField.FieldType != _FIELD_TYPE.CheckBox) {
                            return "";
                        }

                    }
                }

                if (!dataHelper.isNullOrEmpty(relatedField.Selected.ItemValue) && relatedField.FieldType != _FIELD_TYPE.CheckBox)
                    return "";                                        

                var fItem = this.getForeignItem(relation, selectedItem, relation.ForeignKeyNumber, relatedField);
                if (fItem == null || dataHelper.isNullOrEmpty(fItem.ItemText))
                    return "";
                
                var ret = "";
                switch (relation.ActionType) {
                    case _ACTION_TYPE.ValueSetter:                        
                        //ret =  "<div align='left'> <b>" + relatedField.FieldCaption + "</b>: " + params.willSetToText + " <b>" + fItem.ItemText + "</b> </div> <br />";
                        return "<div align='left'> <b>" + relatedField.FieldCaption + "</b>: " + params.willSetToText + " <b>" + fItem.ItemText + "</b> </div> <br />";

                    case _ACTION_TYPE.ListPopulator:                        
                        return "<div align='left'> <b>" + relatedField.FieldCaption + "</b>: " + params.willShowRelatedItemsText + " <b>" + fItem.ItemText + "</b> </div> <br />";

                    case _ACTION_TYPE.ListCleaner:
                        return "<div align='left'> <b>" + relatedField.FieldCaption + "</b>: will be clear </div> <br />";                                            
                }
            },

            applyRules: function (field) {
                var ret = "";

                if (dataHelper.isNullOrUndefined(field))
                    return ret;
                
                if (field.Relations.length > 0) {
                    for (var r = 0; r < field.Relations.length; r++) {
                        var curRelation = field.Relations[r];
                        if (this.isActionApplicableForCurrentMode(curRelation)){
                            if (this.checkConditions(curRelation.Conditions)) {
                                ret += helpdesk.caseRule.runAction(field, curRelation);
                            }
                        }
                    }
                    return ret;
                }

                return "";
            },

            checkConditions: function(conditions){
                if (dataHelper.isNullOrEmpty(conditions) || conditions.length <= 0)
                    return true;

                for (var ci = 0; ci < conditions.length; ci++) {
                    var res = this.checkCondition(conditions[ci]);
                    if (!res)
                        return false;
                }

                return true;
            },

            checkCondition: function(condition){
                var conField = this.getFieldById(condition.FieldId);
                if (dataHelper.isNullOrEmpty(conField))
                    return false;

                if (condition.ForeignKeyNum != _FOREIGN_DATA_NUMBER.Place0 && (conField.Selected == null || conField.Selected.ItemValue == "")) {
                    return false;
                }

                var curFieldValue = this.getForeignValue(conField, condition.ForeignKeyNum);
                var otherSideValue = condition.OtherSideValue;

                switch (condition.ConditionOperator) {
                    case _CONDITION_OPERATOR.HasValue:
                        if (!dataHelper.isNullOrEmpty(curFieldValue) && curFieldValue != _NULL_STR)
                            return true;
                        break;

                    case _CONDITION_OPERATOR.HasNotValue:
                        if (dataHelper.isNullOrEmpty(curFieldValue) || curFieldValue == _NULL_STR)
                            return true;
                        break;

                    case _CONDITION_OPERATOR.Equal:
                        if (curFieldValue == otherSideValue)
                            return true;
                        break;

                    case _CONDITION_OPERATOR.NotEqual:
                        if (curFieldValue != otherSideValue)
                            return true;
                        break;
                }

                return false;
            },

            runAction: function (field, relation) {
                if (dataHelper.isNullOrUndefined(field))
                    return;

                var selectedItem = field.Selected;
                var relatedField = helpdesk.caseRule.getFieldById(relation.FieldId);

                switch (relation.ActionType) {
                    case _ACTION_TYPE.ValueSetter:
                        var fItem = this.getForeignItem(relation, selectedItem, relation.ForeignKeyNumber, relatedField);
                        if (fItem != null) {
                            var fieldToSet = relatedField;
                            if (relation.RelationType == _RELATION_TYPE.Virtual) {
                                fieldToSet = this.getFieldById(relation.ResultDataKey);
                            }

                            if (!dataHelper.isNullOrEmpty(fieldToSet))
                                this.applySetterAction(fieldToSet, fItem);
                        }
                        break;

                    case _ACTION_TYPE.ListPopulator:                        
                        this.applyListPopulatorAction(relatedField, selectedItem.ItemValue, relation);                                                                                                                
                        break;

                    case _ACTION_TYPE.ListCleaner:
                        break;
                }
            },

            applySetterAction: function (field, item) {
                if (dataHelper.isNullOrUndefined(field))
                    return;

                var element = this.getElementByFieldId(field.FieldId);
                if (dataHelper.isNullOrUndefined(element))
                    return;

                var $element = $(element);
                if (dataHelper.isNullOrUndefined($element))
                    return;

                field.Selected = item;

                switch (field.FieldType) {

                    case _FIELD_TYPE.TextField:
                        $element.val(field.Selected.ItemValue);//.change();
                        break;

                    case _FIELD_TYPE.CheckBox:
                        $element.prop('checked', convertor.toBool(field.Selected.ItemValue));//.change();
                        break;

                    case _FIELD_TYPE.SingleSelectField:
                        $element.val(field.Selected.ItemValue);//.change();
                        break;

                    case _FIELD_TYPE.TreeButtonSelect:
                        // TODO: Need to check again
                        $element.val(field.Selected.ItemValue);//.change();
                        break;

                    case _FIELD_TYPE.TextArea:
                        $element.val(field.Selected.ItemValue);//.change();
                        break;

                    case _FIELD_TYPE.DateField:
                        // TODO: Need to check again Date format
                        $($element).datepicker({
                            format: this._DATE_FORMAT.toLowerCase(),
                            autoclose: true
                        }).datepicker('setDate', field.Selected.ItemValue);                        
                        $element.change();
                        break;
                }
            },
            
            applyListPopulatorAction: function (field, primaryKeyValue, relation) {

                if (dataHelper.isNullOrUndefined(field) ||                     
                    dataHelper.isNullOrUndefined(relation.ForeignKeyNumber))
                    return;

                var element = this.getElementByFieldId(field.FieldId);
                if (dataHelper.isNullOrUndefined(element))
                    return;

                var $element = $(element);
                if (dataHelper.isNullOrUndefined($element))
                    return;                

                switch (field.FieldType) {

                    case _FIELD_TYPE.TextField:                        
                        break;

                    case _FIELD_TYPE.CheckBox:                        
                        break;

                    // TODO: Capablility to keep selected value
                    case _FIELD_TYPE.SingleSelectField:
                        $element.val('');
                        $element.empty();

                        var newOption = $("<option value></option>");
                        $element.append(newOption);

                        var itemToCheck = null;
                        var canAdd = false;

                        for (var i = 0; i < field.Items.length; i++) {
                            canAdd = false;
                            var curItem = field.Items[i];

                            if ((relation.ShowAllIfKeyIsNull && dataHelper.isNullOrEmpty(primaryKeyValue)) ||
                                (relation.ShowRunTimeCurrentValue && curItem.ItemValue == _CURRENT_RUN_TIME_ITEM_ID)) {
                                canAdd = true;
                            }
                            else {                                
                                if (relation.ForeignKeyNumber == _FOREIGN_DATA_NUMBER.Place1)
                                    itemToCheck = curItem.ForeignKeyValue1;

                                if (relation.ForeignKeyNumber == _FOREIGN_DATA_NUMBER.Place2)
                                    itemToCheck = curItem.ForeignKeyValue2;

                                if (relation.ForeignKeyNumber == _FOREIGN_DATA_NUMBER.Place3)
                                    itemToCheck = curItem.ForeignKeyValue3;

                                if (relation.RelationType == _RELATION_TYPE.ManyToMany) {
                                    // Check array of keys
                                    var aryItemToCheck = [];
                                    if (!dataHelper.isNullOrEmpty(itemToCheck))
                                        aryItemToCheck = itemToCheck.split(_ARRAY_SEPARATOR_CHARACTER);

                                    if (dataHelper.arrayContains(aryItemToCheck, primaryKeyValue)) {
                                        canAdd = true;
                                    }
                                } else {
                                    if (itemToCheck == primaryKeyValue) {
                                        canAdd = true;
                                    }
                                }
                            }
                            
                            if (canAdd){
                                newOption = $("<option value='" + curItem.ItemValue + "'>" + curItem.ItemText + "</option>");
                                $element.append(newOption);
                            }
                        }

                        $element.change();
                        break;

                    case _FIELD_TYPE.TreeButtonSelect:
                        // TODO: Need to check again
                        //$element.val(field.itemValue).change();
                        break;

                    case _FIELD_TYPE.TextArea:                        
                        break;

                    case _FIELD_TYPE.DateField:                        
                        break;
                }
            },

            /****************  Get models **************/
            getFieldById: function (fieldId) {
                if (dataHelper.isNullOrUndefined(ruleModel) ||
                    dataHelper.isNullOrUndefined(ruleModel.FieldAttributes) ||
                    ruleModel.FieldAttributes.length <= 0 ||
                    dataHelper.isNullOrUndefined(fieldId))
                    return null;

                fieldId = fieldId.toLowerCase();
                for (var fi = 0; fi < ruleModel.FieldAttributes.length; fi++)
                    if (ruleModel.FieldAttributes[fi].FieldId.toLowerCase() == fieldId) {
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
                if (elms != undefined && elms.length > 0)
                    return elms[0];
                else
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
                    ret.ItemValue = convertor.toBoolStr(itemValue);
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

            getVirtualDataFor: function (virtualFieldId, key1, key2, key3, resultKey){
                var curValueItem1 = helpdesk.caseRule.getFieldById(key1);
                var curValueItem2 = helpdesk.caseRule.getFieldById(key2);
                var curValueItem3 = helpdesk.caseRule.getFieldById(key3);

                var resultField = helpdesk.caseRule.getFieldById(resultKey);

                if (resultField == null || (curValueItem1 == null && curValueItem2 == null && curValueItem3 == null))
                    return null;

                var vf = helpdesk.caseRule.getFieldById(virtualFieldId);
                if (dataHelper.isNullOrUndefined(vf) || vf.Items == null || vf.Items.length <= 0)
                    return null;

                var found = false;
                for (var vfi = 0; vfi < vf.Items.length; vfi++) {
                    var curData = vf.Items[vfi];
                    
                    if (curValueItem1 != null) {
                        if (curValueItem1.Selected.ItemValue != curData.ForeignKeyValue1)
                            continue;
                    }

                    if (curValueItem2 != null) {
                        if (curValueItem2.Selected.ItemValue != curData.ForeignKeyValue2)
                            continue;
                    }

                    if (curValueItem3 != null) {
                        if (curValueItem3.Selected.ItemValue != curData.ForeignKeyValue3)
                            continue;
                    }

                    /* Item found */
                    return helpdesk.caseRule.getItemByValue(resultField, curData.ResultKeyValue);
                }

                return null;
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
            },

            getForeignItem: function (relation, parentSelectedItem, place, relatedField) {
                switch (relation.RelationType) {
                    case _RELATION_TYPE.OneToOne:
                        if (place == _FOREIGN_DATA_NUMBER.Place1) {
                            return helpdesk.caseRule.getItemByValue(relatedField, parentSelectedItem.ForeignKeyValue1)
                        }
                        else if (place == _FOREIGN_DATA_NUMBER.Place2) {
                            return helpdesk.caseRule.getItemByValue(relatedField, parentSelectedItem.ForeignKeyValue2)
                        }
                        else if (place == _FOREIGN_DATA_NUMBER.Place3) {
                            return helpdesk.caseRule.getItemByValue(relatedField, parentSelectedItem.ForeignKeyValue3)
                        }
                        return null;

                    case _RELATION_TYPE.OneToMany:
                        return parentSelectedItem;

                    case _RELATION_TYPE.ManyToMany:
                        return parentSelectedItem;

                    case _RELATION_TYPE.Virtual:
                        if (dataHelper.isNullOrEmpty(relation.FieldId) ||
                            dataHelper.isNullOrEmpty(relation.ResultDataKey))
                            return null;

                        var keyName1, keyName2, keyName3, resultKey = null;
                        if (!dataHelper.isNullOrEmpty(relation.DataStore1))
                            keyName1 = relation.DataStore1;

                        if (!dataHelper.isNullOrEmpty(relation.DataStore2))
                            keyName2 = relation.DataStore2;

                        if (!dataHelper.isNullOrEmpty(relation.DataStore3))
                            keyName3 = relation.DataStore3;

                        resultKey = relation.ResultDataKey;
                        return this.getVirtualDataFor(relation.FieldId, keyName1, keyName2, keyName3, resultKey);
                        break;
                }

                return null;
            },
           
            getForeignValue: function (field, place, primaryKey) {
                var ret = null;

                if (dataHelper.isNullOrUndefined(field)) 
                    return ret;

                var expectedItem = null;
                if (!dataHelper.isNullOrEmpty(primaryKey) && !dataHelper.isNullOrUndefined(field.Items)) {
                    for (var fiv = 0; fiv < field.Items.length; fiv++) {
                        if (field.Items[fiv].ItemValue == primaryKey)
                            expectedItem = field.Items[fiv];
                        break;
                    }
                } else if (!dataHelper.isNullOrEmpty(primaryKey) && dataHelper.isNullOrUndefined(field.Items)) {
                    return null;
                } else if (!dataHelper.isNullOrUndefined(field.Selected)) {
                    expectedItem = field.Selected;
                }

                if (expectedItem == null)
                    return null;

                switch (place) {
                    case _FOREIGN_DATA_NUMBER.Place0:
                        ret = expectedItem.ItemValue;
                        break;

                    case _FOREIGN_DATA_NUMBER.Place1:
                        ret = expectedItem.ForeignKeyValue1;
                        break;

                    case _FOREIGN_DATA_NUMBER.Place2:
                        ret = expectedItem.ForeignKeyValue2;
                        break;

                    case _FOREIGN_DATA_NUMBER.Place3:
                        ret = expectedItem.ForeignKeyValue3;
                        break;
                }
               
                return ret;
            }
        }
                      
    })($);
       
    helpdesk.caseRule.init();    
});