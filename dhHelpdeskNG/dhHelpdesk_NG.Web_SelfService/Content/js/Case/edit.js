$(function () {    

    (function ($) {

        window.Application = window.Application || {};
        window.Application.prototype = window.Application.prototype || {};

        var fieldSettings = window.fieldSettings || {};
        var caseTypeRelatedFields = window.caseTypeRelatedFields || {};
        var priorityRelatedFields = window.priorityRelatedFields || {};
        
        var uploadCaseFileUrl = window.appParameters.uploadCaseFileUrl;
        var caseFileKey = window.appParameters.caseFileKey;
        var newCaseFilesUrl = window.appParameters.newCaseFilesUrl;
        var fileAlreadyExistsMsg = window.appParameters.fileAlreadyExistsMsg;
        var downloadCaseFileUrl = window.appParameters.downloadCaseFileUrl;
        var downloadCaseFileParamUrl = window.appParameters.downloadCaseFileParamUrl;
        var deleteCaseFileUrl = window.appParameters.deleteCaseFileUrl;
        var searchUserUrl = window.appParameters.searchUserUrl;
        var seachComputerUrl = window.appParameters.seachComputerUrl;
        var saveNewCaseUrl = window.appParameters.saveNewCaseUrl;
        var departmentsUrl = window.appParameters.fetchDepartmentsUrl;
        var OUsUrl = window.appParameters.fetchOUUrl;

        var customerId;
        var alreadyExistFileIds = [];
        var attrApplyClass = "hasAttribute";
        var attrFieldNameHolder = "standardName";
        var relatedFieldBlockPrefix = "#relatedFieldBlock-";
        var mandatorySignPrefix = "#mandatory_sign_";
        var caseTypeRelationName = "casetype";
        var priorityRelationName = "priority";
        
        Application.prototype.applyFieldAttributes = function () {
            if (fieldSettings == undefined || fieldSettings == null || fieldSettings.length <= 0)
                return;

            var needToApplyAttrs = document.getElementsByClassName(attrApplyClass);
            for (var i = 0; i < needToApplyAttrs.length; i++) {
                var fieldStandardName = $(needToApplyAttrs[i]).attr(attrFieldNameHolder);                
                var fs = getFieldSetting(fieldStandardName);
                if (fs != null) {
                    applySettingOnElement($(needToApplyAttrs[i]), fs)
                }
            }
        }

        Application.prototype.isElementReadonly = function ($element) {
            if ($element == undefined || $element == null)
                return;

            var fieldStandardName = $($element).attr(attrFieldNameHolder);
            var fs = getFieldSetting(fieldStandardName);
            if (fs != null && fs.IsReadonly)
                return true;

            return false;
        }

        Application.prototype.checkRelationRules = function ($element, fields, relationType) {
            if ($element == undefined || $element == null)
                return;

            var elementId = $element.val();
            var relatedField = getRelatedField(elementId, fields);

            if (relatedField == undefined || relatedField == null || relatedField == '') {
                switch (relationType.toLowerCase()) {
                    case caseTypeRelationName:
                        removeRelationRule('Impact_Id', '#NewCase_Impact_Id');
                        removeRelationRule('Urgency_Id', '#NewCase_Urgency_Id');
                        break;
                    case priorityRelationName:
                        removeRelationRule('Available', '#NewCase_Available');
                        break;
                }
                return;
            }
                
            switch (relatedField.toLowerCase()) {
                case window.caseTypeRelatedFieldName:
                    addRelationRule('Impact_Id', '#NewCase_Impact_Id');
                    addRelationRule('Urgency_Id', '#NewCase_Urgency_Id');
                    break;
                case window.priorityRelatedFieldName:
                    addRelationRule('Available', '#NewCase_Available');
                    break;
            }
        }


        Application.prototype.isNullOrEmpty = function (val) {
            return val == undefined || val == null || val == "";
        }

        Application.prototype.isNullOrUndefined = function (val) {
            return val == undefined || val == null;
        }

        Application.prototype.setValueToBtnGroup = function (domContainer, domText, domValue, value) {
            var self = this;
            var $domValue = $(domValue);
            var oldValue = $domValue.val();
            var el = $(domContainer).find('a[value="' + value + '"]');
            if (el) {
                var _txt = self.getBreadcrumbs(el);
                if (_txt == undefined || _txt == "")
                    _txt = "--";
                $(domText).text(_txt);
                $domValue.val(value);
                if (oldValue !== value) {
                    $domValue.trigger('change');
                }
            }
        }

        Application.prototype.getBreadcrumbs = function (el) {
            self = this;
            var path = $(el).text();
            var $parent = $(el).parents("li").eq(1).find("a:first");
            if ($parent.length == 1) {
                path = self.getBreadcrumbs($parent) + " - " + path;
            }
            return path;
        }

        Application.prototype.parseDate = function (dateStr) {
            if (dateStr == undefined || dateStr == "")
                return null;

            var dateArray = dateStr.split("-");
            if (dateArray.length != 3)
                return null;

            return new Date(parseInt(dateArray[0]), parseInt(dateArray[1] - 1), parseInt(dateArray[2]));
        }

        Application.prototype.dateToDisplayFormat = function (dateValue) {
            var self = this;
            if (Object.prototype.toString.call(dateValue) === "[object Date]") {
                if (isNaN(dateValue.getTime())) {
                    return "";
                }
                else {
                    return dateValue.getFullYear() + "-" +
                           self.padLeft(dateValue.getMonth() + 1, 2, "0") + "-" +
                           self.padLeft(dateValue.getDate(), 2, "0");
                }
            }
            else {
                return "";
            }
        }

        Application.prototype.padLeft = function (value, totalLength, padChar) {
            var valLen = value.toString().length;
            var diff = totalLength - valLen;
            if (diff > 0) {
                for (var i = 0; i < diff; i++)
                    value = padChar + value;
            }
            return value;
        }

        Application.prototype.getDate = function (val) {
            if (val == undefined || val == null || val == "")
                return null;
            else {
                var dateStr = val.split(' ');
                if (dateStr.length > 0)
                    return new Date(dateStr[0]);
                else
                    return null;
            }
        };       

        var canApplyCaseTypeRule = function (standardFieldName) {
            var impactSetting = getFieldSetting(standardFieldName);
            if (impactSetting == null || (impactSetting != null && !impactSetting.IsVisible))
                return true;

            return false;
        }

        var addRelationRule = function (standardFieldName, elementId) {
            var setting = getFieldSetting(standardFieldName);
            if (setting == null || (setting != null && !setting.IsVisible)) {
                var $elmToShow = $(relatedFieldBlockPrefix + standardFieldName);
                $(elementId).val('').change();
                showElement($elmToShow);
            }

            $(elementId).attr("required", "required");
            $(elementId).rules("add", "required");
            var $mandatorySign = $(mandatorySignPrefix + standardFieldName);
            showElement($mandatorySign);
            $(elementId).rules("add", {
                required: true
            });
        }

        var removeRelationRule = function (standardFieldName, elementId) {
            var setting = getFieldSetting(standardFieldName);
            if (setting == null || (setting != null && !setting.IsVisible)) {
                var $elmToHide = $(relatedFieldBlockPrefix + standardFieldName);
                $(elementId).val('').change();
                hideElement($elmToHide);
            }
            if (setting == null || (setting != null && !setting.IsRequired)) {
                $(elementId).removeAttr('required');
                $(elementId).rules("remove");
                var $mandatorySign = $(mandatorySignPrefix + standardFieldName);
                hideElement($mandatorySign);
            }
        }

        var getFieldSetting = function (fieldName) {
            if (fieldSettings == undefined || fieldSettings == null || fieldSettings.length <= 0 || fieldName == undefined || fieldName == null)
                return null;

            fieldName = fieldName.toLowerCase();
            for (var fn = 0; fn < fieldSettings.length; fn++) {
                if (fieldSettings[fn].FieldName.toLowerCase() == fieldName)
                    return fieldSettings[fn];
            }

            return null;
        }

        var getRelatedField = function (fieldId, fields) {
            if (fields == undefined || fields == null || fields.length <= 0 || fieldId == undefined || fieldId == null)
                return "";
            
            for (var ct = 0; ct < fields.length; ct++) {
                if (fields[ct].Key == fieldId)
                    return fields[ct].Value;
            }
            return "";
        }

        var applySettingOnElement = function ($element, settings) {
            if (fieldSettings == undefined || fieldSettings == null || fieldSettings.length <= 0)
                return null;

            if (settings.IsReadonly) {                
                disableElement($element);
            }

            if (!settings.IsVisible) {
                var $elToHide = $('.form-group.row.' + settings.FieldName);
                hideElement($elToHide);
            }
        }

        var disableElement = function($element){
            if ($element.is("input")) {
                $element.attr("disabled", "disabled");
            } else {
                $element.attr("disabled", "disabled");
                $element.css("pointer-events", "none");
            };
            $element.addClass("disabled");            
        }

        var hideElement = function ($element) {
            if ($element != undefined && $element != null)
                $element.hide();            
        }

        var showElement = function ($element) {
            if ($element != undefined && $element != null)
                $element.show();
        }

        /**
        * @public
        * @param { number } regionId
        * @param { number } departmentId
        * @param { number } selectedOrgUnitId
        */
        Application.prototype.setOrganizationData = function (regionId, departmentId, selectedOrgUnitId) {
            var me = this;

            me.$departmentControl.children().remove();
            me.$orgUnitControl.children().remove();            
            me.refreshDepartment(regionId, departmentId, selectedOrgUnitId);            
        };

        /**
        * @private
        * @param { jQueryElement } $select
        * @param { optionId } int
        */
        Application.prototype.isIdExistsInSelect = function ($select, optionId) {
            var keyToFind = 'option[value=' + optionId + ']';
            var opt = $select.find(keyToFind);
            return opt != null && opt.length > 0;
        }

        /**
        * @private
        * @param { number } regionId
        * @param { number } selectedId = null
        * @param { number } selectedOrgUnitId = null
        */

        Application.prototype.refreshDepartment = function (regionId, selectedId, selectedOrgUnitId) {
            var me = this;
            me.$departmentControl.val('').find('option').remove();
            me.$departmentControl.append('<option value="">&nbsp;</option>');
            
            if (me.isElementReadonly(me.$departmentControl)) {
                me.$orgUnitControl.val('').find('option').remove();
                me.$orgUnitControl.append('<option value="">&nbsp;</option>');
                return;
            }

            $.get(departmentsUrl, {
                'id': regionId,
                'customerId': customerId,
                'departmentFilterFormat': 0
            }, function (resp) {
                if (resp != null && resp.success) {
                    me.$departmentControl.append(me.makeOptionsFromIdName(resp.data));
                    if (selectedId != null && me.isIdExistsInSelect(me.$departmentControl, selectedId)) {
                        me.$departmentControl.val(selectedId);
                    }
                }
            }, 'json').always(function () {
                me.$departmentControl.prop('disabled', false);
                me.refreshOrganizationUnit(me.$departmentControl.val(), selectedOrgUnitId);
            });
        };

        /**
        * @private
        * @param { number } selectedRegionId
        * @param { number } filterFormat department filter format
        * @param { number } selectedId = null
        */
        Application.prototype.refreshOrganizationUnit = function (departmentId, selectedId) {
            var me = this;
            me.$orgUnitControl.val('').find('option').remove();
            me.$orgUnitControl.append('<option value="">&nbsp;</option>');

            if (me.isElementReadonly(me.$orgUnitControl)) {
                return;
            }

            $.get(OUsUrl, {
                'id': departmentId,
                'customerId': customerId,
                'departmentFilterFormat': 0
            }, function (resp) {
                if (resp != null && resp.success) {
                    me.$orgUnitControl.append(me.makeOptionsFromIdName(resp.data));
                    if (selectedId != null && me.isIdExistsInSelect(me.$orgUnitControl, selectedId)) {
                        me.$orgUnitControl.val(selectedId);
                    }
                }
            }, 'json').always(function () {
                me.$orgUnitControl.prop('disabled', false);
            });
        };

        /**
        * @private
        * @param { { number: id, string: name}[] } data
        * @returns string
        */
        Application.prototype.makeOptionsFromIdName = function (data) {
            var content = [];
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                content.push("<option value='" + item.id + "'>" + item.name + "</option>");
            }
            return content.join('');
        }

        PluploadTranslation($("#case_languageId").val());
        var lastInitiatorSearchKey = ''

        $('#NewCasefile_uploader').pluploadQueue({
            url: uploadCaseFileUrl,
            multipart_params: { Id: caseFileKey, curTime: Date.now() },
            max_file_size: '10mb',

            init: {
                FileUploaded: function () {

                    $.get(newCaseFilesUrl, { id: caseFileKey, curTime: Date.now() }, function (files) {
                        Application.prototype.refreshNewCaseFilesTable(files);
                    });
                },

                UploadComplete: function (up, file) {
                    //plupload_add
                    $(".plupload_buttons").css("display", "inline");
                    $(".plupload_upload_status").css("display", "inline");
                    up.refresh();
                },

                Error: function (uploader, e) {
                    if (e.status != 409) {
                        return;
                    }

                    alreadyExistFileIds.push(e.file.id);
                },

                StateChanged: function (uploader) {
                    if (uploader.state != plupload.STOPPED) {
                        return;
                    }

                    for (var i = 0; i < alreadyExistFileIds.length; i++) {
                        var fileId = alreadyExistFileIds[i];
                        $('#NewCasefile_uploader ul[class="plupload_filelist"] li[id="' + fileId + '"] div[class="plupload_file_action"] a').prop('title', fileAlreadyExistsMsg);
                    }

                    alreadyExistFileIds = [];
                    uploader.refresh();
                }
            }
        });

        $('#NewCase_upload_files_popup').on('hidden.bs.modal', function () {
            if ($('#NewCasefile_uploader') != undefined) {
                if ($('#NewCasefile_uploader').pluploadQueue().files.length > 0) {
                    if ($('#NewCasefile_uploader').pluploadQueue().state == plupload.UPLOADING)
                        $('#NewCasefile_uploader').pluploadQueue().stop();

                    while ($('#NewCasefile_uploader').pluploadQueue().files.length > 0) {
                        $('#NewCasefile_uploader').pluploadQueue().removeFile($('#NewCasefile_uploader').pluploadQueue().files[0]);
                    }
                    $('#NewCasefile_uploader').pluploadQueue().refresh();
                }
            }
        });

        Application.prototype.refreshNewCaseFilesTable = function (files) {
            $('#NewCasefiles_table > tbody > tr').remove();

            var fileMarkup;

            for (var i = 0; i < files.length; i++) {
                var file = files[i];

                fileMarkup =
                    $('<tr>' +
                        '<td>' +
                            '<i class="glyphicon glyphicon-file">&nbsp;</i><a style="color:blue" href=' + downloadCaseFileUrl + '?' + downloadCaseFileParamUrl + 'fileName=' + file + '>' + file + '</a>' +
                        '</td>' +
                        '<td>' +
                            '<a id="delete_file_button_' + i + '" class="btn btn-default btn-sm" ><span class="glyphicon glyphicon-remove"></span> </a>' +
                        '</td>' +
                        '</tr>');

                $('#NewCasefiles_table > tbody').append(fileMarkup);
            }

            Application.prototype.bindDeleteNewCaseFileBehaviorToDeleteButtons();
        }

        Application.prototype.bindDeleteNewCaseFileBehaviorToDeleteButtons = function () {
            $('#NewCasefiles_table a[id^="delete_file_button_"]').click(function () {
                var fileName = $(this).parents('tr:first').children('td:first').children('a').text();
                var pressedDeleteFileButton = this;

                $.post(deleteCaseFileUrl, { id: caseFileKey, fileName: fileName, curTime: Date.now() }, function () {
                    $(pressedDeleteFileButton).parents('tr:first').remove();
                });
            });
        }

        Application.prototype.generateRandomKey = function () {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                  .toString(16)
                  .substring(1);
            }
            return s4() + '-' + s4() + '-' + s4();
        }

        Application.prototype._GetComputerUserSearchOptions = function () {
            var me = this;
            var fieldsVisibility = [];
            var options = {
                items: 20,
                minLength: 2,

                source: function (query, process) {
                    lastInitiatorSearchKey = Application.prototype.generateRandomKey();

                    return $.ajax({
                        url: searchUserUrl,
                        type: 'post',
                        data: { query: query, customerId: $('#NewCase_Customer_Id').val(), searchKey: lastInitiatorSearchKey },
                        dataType: 'json',
                        success: function (result) {
                            if (result.searchKey != lastInitiatorSearchKey)
                                return;
                            fieldsVisibility = result.fieldsVisibility;
                            var resultList = jQuery.map(result.result, function (item) {
                                var aItem = {
                                    id: item.Id
                                    , num: item.UserId
                                    , name: item.FirstName + ' ' + item.SurName
                                    , email: item.Email
                                    , place: item.Location
                                    , phone: item.Phone
                                    , usercode: item.UserCode
                                    , cellphone: item.CellPhone
                                    , regionid: item.Region_Id
                                    , regionname: item.RegionName
                                    , departmentid: item.Department_Id
                                    , departmentname: item.DepartmentName
                                    , ouid: item.OU_Id
                                    , ouname: item.OUName
                                    , name_family: item.SurName + ' ' + item.FirstName
                                    , customername: item.CustomerName
                                    , costcentre: item.CostCentre

                                };
                                return JSON.stringify(aItem);
                            });
                            if (resultList.length === 0) {
                                var noRes = {
                                    name: window.parameters.noResultLabel,
                                    isNoResult: true
                                }
                                resultList.push(JSON.stringify(noRes));
                            }
                            return process(resultList);
                        }
                    });
                },

                matcher: function (obj) {
                    var item = JSON.parse(obj);
                    if (~item.isNoResult) {
                        return 1;
                    }
                    return ~item.name.toLowerCase().indexOf(this.query.toLowerCase())
                        || ~item.name_family.toLowerCase().indexOf(this.query.toLowerCase())
                        || ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
                        || ~item.phone.toLowerCase().indexOf(this.query.toLowerCase())
                        || ~item.email.toLowerCase().indexOf(this.query.toLowerCase())
                        || ~item.usercode.toLowerCase().indexOf(this.query.toLowerCase());
                },

                sorter: function (items) {
                    var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
                    while (aItem = items.shift()) {
                        var item = JSON.parse(aItem);
                        if (!item.isNoResult) {
                            if (!item.num.toLowerCase().indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
                            else if (~item.num.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
                            else caseInsensitive.push(JSON.stringify(item));
                        } else caseInsensitive.push(JSON.stringify(item));
                    }

                    return beginswith.concat(caseSensitive, caseInsensitive);
                },

                highlighter: function (obj) {
                    var item = JSON.parse(obj);
                    if (item.isNoResult) {
                        return item.name;
                    }
                    var orgQuery = this.query;
                    var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
                    var resultArr = [];
                    var resultByNameFamilyArr = [];
                    if (fieldsVisibility.Name && item.name) {
                        resultArr.push(item.name);
                        resultByNameFamilyArr.push(item.name_family);
                    }
                    resultArr.push(item.num);
                    resultByNameFamilyArr.push(item.num);
                    if (fieldsVisibility.Phone && item.phone) {
                        resultArr.push(item.phone);
                        resultByNameFamilyArr.push(item.phone);
                    }
                    if (fieldsVisibility.Email && item.email) {
                        resultArr.push(item.email);
                        resultByNameFamilyArr.push(item.email);
                    }
                    if (fieldsVisibility.Department && item.departmentname) {
                        resultArr.push(item.departmentname);
                        resultByNameFamilyArr.push(item.departmentname);
                    }
                    if (fieldsVisibility.UserCode && item.usercode) {
                        resultArr.push(item.usercode);
                        resultByNameFamilyArr.push(item.usercode);
                    }
                    var result = resultArr.join(" - ");
                    var resultByNameFamily = resultByNameFamilyArr.join(" - ");

                    if (result.toLowerCase().indexOf(orgQuery.toLowerCase()) > -1)
                        return result.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                            return '<strong>' + match + '</strong>';
                        });
                    else
                        return resultByNameFamily.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                            return '<strong>' + match + '</strong>';
                        });

                },

                updater: function (obj) {

                    var item = JSON.parse(obj);
                    if (item.isNoResult) {
                        return this.query;
                    }
                    //console.log(JSON.stringify(item));
                    $('#NewCase_ReportedBy').val(item.num);
                    $('#NewCase_PersonsName').val(item.name);
                    $('#NewCase_PersonsEmail').val(item.email);
                    $('#NewCase_PersonsPhone').val(item.phone);
                    $('#NewCase_PersonsCellphone').val(item.cellphone);
                    $('#NewCase_Place').val(item.place);
                    $('#NewCase_UserCode').val(item.usercode);
                    $('#NewCase_Region_Id').val(item.regionid);
                    $('#NewCase_CostCentre').val(item.costcentre);
                    
                    me.setOrganizationData(item.regionid, item.departmentid, item.ouid);

                    return item.num;
                }
            };

            return options;
        }

        Application.prototype._GetComputerSearchOptions = function () {

            var options = {
                items: 20,
                minLength: 2,

                source: function (query, process) {
                    return $.ajax({
                        url: seachComputerUrl,
                        type: 'post',
                        data: { query: query, customerId: $('#NewCase_Customer_Id').val() },
                        dataType: 'json',
                        success: function (result) {
                            var resultList = jQuery.map(result, function (item) {
                                var aItem = {
                                    id: item.Id
                                    , num: item.ComputerName
                                    , location: item.Location
                                    , computertype: item.ComputerTypeDescription
                                };
                                return JSON.stringify(aItem);
                            });
                            if (resultList.length === 0) {
                                var noRes = {
                                    name: window.parameters.noResultLabel,
                                    isNoResult: true
                                }
                                resultList.push(JSON.stringify(noRes));
                            }
                            return process(resultList);
                        }
                    });
                },

                matcher: function (obj) {
                    var item = JSON.parse(obj);
                    if (~item.isNoResult) {
                        return 1;
                    }
                    return ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
                        || ~item.computertype.toLowerCase().indexOf(this.query.toLowerCase())
                        || ~item.location.toLowerCase().indexOf(this.query.toLowerCase());
                },

                sorter: function (items) {
                    var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
                    while (aItem = items.shift()) {
                        var item = JSON.parse(aItem);
                        if (!item.isNoResult) {
                            if (!item.num.toLowerCase().indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
                            else if (~item.num.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
                            else caseInsensitive.push(JSON.stringify(item));
                        } else caseInsensitive.push(JSON.stringify(item));
                    }

                    return beginswith.concat(caseSensitive, caseInsensitive);
                },

                highlighter: function (obj) {
                    var item = JSON.parse(obj);
                    if (item.isNoResult) {
                        return item.name;
                    }
                    var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
                    var result = item.num + ' - ' + item.location + ' - ' + (item.computertype == null ? ' ' : item.computertype);

                    return result.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                        return '<strong>' + match + '</strong>';
                    });
                },

                updater: function (obj) {
                    var item = JSON.parse(obj);
                    if (item.isNoResult) {
                        return "";
                    }
                    $('#NewCase_InventoryNumber').val(item.num);
                    $('#NewCase_InventoryType').val(item.computertype);
                    $('#NewCase_InventoryLocation').val(item.location);

                    return item.num;
                }
            };

            return options;
        } 

        bindProductAreasEvents();
//        $('#divProductArea ul.dropdown-menu li a').click(function (e) {
//            e.preventDefault();
//            var val = $(this).attr('value');
//            $("#divBreadcrumbs_ProductArea").text(getBreadcrumbs(this));
//            var ee = document.getElementById("NewCase_ProductArea_Id");
//            ee.setAttribute('value', val);
//        });

        $('#divCategory ul.dropdown-menu li a').click(function (e) {
            e.preventDefault();
            var val = $(this).attr('value');
            $("#divBreadcrumbs_Category").text(getBreadcrumbs(this));
            var ee = document.getElementById("NewCase_Category_Id");
            ee.setAttribute('value', val);
        });

        $('#divCaseType ul.dropdown-menu li a').click(function (e) {
            e.preventDefault();
            var val = $(this).attr('value');
            $("#divBreadcrumbs_CaseType").text(getBreadcrumbs(this));
            var ee = document.getElementById("NewCase.CaseType_Id");
            ee.setAttribute('value', val);
            $(ee).trigger('change');            
        });

        $('#divCaseTypeSetting ul.dropdown-menu li a').click(function (e) {
            e.preventDefault();
            var val = $(this).attr('value');
            $("#divBreadcrumbs_CaseTypeSetting").text(getBreadcrumbs(this));
            var ee = document.getElementById("NewCase_CaseType_Id");
            ee.setAttribute('value', val);
        });

        $('#divProductAreaSetting ul.dropdown-menu li a').click(function (e) {
            e.preventDefault();
            var val = $(this).attr('value');
            $("#divBreadcrumbs_ProductAreaSetting").text(getBreadcrumbs(this));
            var ee = document.getElementById("NewCase_ProductArea_Id");
            ee.setAttribute('value', val);
        });

        var curFileName;
        var globalClipboard;

        function ClipboardClass() {
            var self = this;
            var ctrlPressed = false;
            var pasteCatcher;
            var pasteMode;
            var source;
            var mimeTypes = { "3dm": "x-world/x-3dmf", "3dmf": "x-world/x-3dmf", "a": "application/octet-stream", "aab": "application/x-authorware-bin", "aam": "application/x-authorware-map", "aas": "application/x-authorware-seg", "abc": "text/vnd.abc", "acgi": "text/html", "afl": "video/animaflex", "ai": "application/postscript", "aif": "audio/x-aiff", "aifc": "audio/x-aiff", "aiff": "audio/x-aiff", "aim": "application/x-aim", "aip": "text/x-audiosoft-intra", "ani": "application/x-navi-animation", "aos": "application/x-nokia-9000-communicator-add-on-software", "aps": "application/mime", "arc": "application/octet-stream", "arj": "application/octet-stream", "art": "image/x-jg", "asf": "video/x-ms-asf", "asm": "text/x-asm", "asp": "text/asp", "asx": "video/x-ms-asf-plugin", "au": "audio/x-au", "avi": "video/x-msvideo", "avs": "video/avs-video", "bcpio": "application/x-bcpio", "bin": "application/x-macbinary", "bm": "image/bmp", "bmp": "image/x-windows-bmp", "boo": "application/book", "book": "application/book", "boz": "application/x-bzip2", "bsh": "application/x-bsh", "bz": "application/x-bzip", "bz2": "application/x-bzip2", "c": "text/x-c", "c++": "text/plain", "cat": "application/vnd.ms-pki.seccat", "cc": "text/x-c", "ccad": "application/clariscad", "cco": "application/x-cocoa", "cdf": "application/x-netcdf", "cer": "application/x-x509-ca-cert", "cha": "application/x-chat", "chat": "application/x-chat", "class": "application/x-java-class", "com": "text/plain", "conf": "text/plain", "cpio": "application/x-cpio", "cpp": "text/x-c", "cpt": "application/x-cpt", "crl": "application/pkix-crl", "crt": "application/x-x509-user-cert", "csh": "text/x-script.csh", "css": "text/css", "cxx": "text/plain", "dcr": "application/x-director", "deepv": "application/x-deepv", "def": "text/plain", "der": "application/x-x509-ca-cert", "dif": "video/x-dv", "dir": "application/x-director", "dl": "video/x-dl", "doc": "application/msword", "dot": "application/msword", "dp": "application/commonground", "drw": "application/drafting", "dump": "application/octet-stream", "dv": "video/x-dv", "dvi": "application/x-dvi", "dwf": "model/vnd.dwf", "dwg": "image/x-dwg", "dxf": "image/x-dwg", "dxr": "application/x-director", "el": "text/x-script.elisp", "elc": "application/x-elc", "env": "application/x-envoy", "eps": "application/postscript", "es": "application/x-esrehber", "etx": "text/x-setext", "evy": "application/x-envoy", "exe": "application/octet-stream", "f": "text/x-fortran", "f77": "text/x-fortran", "f90": "text/x-fortran", "fdf": "application/vnd.fdf", "fif": "image/fif", "fli": "video/x-fli", "flo": "image/florian", "flx": "text/vnd.fmi.flexstor", "fmf": "video/x-atomic3d-feature", "for": "text/x-fortran", "fpx": "image/vnd.net-fpx", "frl": "application/freeloader", "funk": "audio/make", "g": "text/plain", "g3": "image/g3fax", "gif": "image/gif", "gl": "video/x-gl", "gsd": "audio/x-gsm", "gsm": "audio/x-gsm", "gsp": "application/x-gsp", "gss": "application/x-gss", "gtar": "application/x-gtar", "gz": "application/x-gzip", "gzip": "multipart/x-gzip", "h": "text/x-h", "hdf": "application/x-hdf", "help": "application/x-helpfile", "hgl": "application/vnd.hp-hpgl", "hh": "text/x-h", "hlb": "text/x-script", "hlp": "application/x-winhelp", "hpg": "application/vnd.hp-hpgl", "hpgl": "application/vnd.hp-hpgl", "hqx": "application/x-mac-binhex40", "hta": "application/hta", "htc": "text/x-component", "htm": "text/html", "html": "text/html", "htmls": "text/html", "htt": "text/webviewhtml", "htx": "text/html", "ice": "x-conference/x-cooltalk", "ico": "image/x-icon", "idc": "text/plain", "ief": "image/ief", "iefs": "image/ief", "iges": "application/iges", "iges": "model/iges", "igs": "model/iges", "ima": "application/x-ima", "imap": "application/x-httpd-imap", "inf": "application/inf", "ins": "application/x-internett-signup", "ip": "application/x-ip2", "isu": "video/x-isvideo", "it": "audio/it", "iv": "application/x-inventor", "ivr": "i-world/i-vrml", "ivy": "application/x-livescreen", "jam": "audio/x-jam", "jav": "text/x-java-source", "java": "text/plain", "java": "text/x-java-source", "jcm": "application/x-java-commerce", "jfif": "image/pjpeg", "jfif-tbnl": "image/jpeg", "jpe": "image/pjpeg", "jpeg": "image/pjpeg", "jpg": "image/pjpeg", "jps": "image/x-jps", "js": "application/x-javascript", "jut": "image/jutvision", "kar": "music/x-karaoke", "ksh": "text/x-script.ksh", "la": "audio/x-nspaudio", "lam": "audio/x-liveaudio", "latex": "application/x-latex", "lha": "application/x-lha", "lhx": "application/octet-stream", "list": "text/plain", "lma": "audio/x-nspaudio", "log": "text/plain", "lsp": "text/x-script.lisp", "lst": "text/plain", "lsx": "text/x-la-asf", "ltx": "application/x-latex", "lzh": "application/x-lzh", "lzx": "application/x-lzx", "m": "text/x-m", "m1v": "video/mpeg", "m2a": "audio/mpeg", "m2v": "video/mpeg", "m3u": "audio/x-mpequrl", "man": "application/x-troff-man", "map": "application/x-navimap", "mar": "text/plain", "mbd": "application/mbedlet", "mc$": "application/x-magic-cap-package-1.0", "mcd": "application/x-mathcad", "mcf": "text/mcf", "mcp": "application/netmc", "me": "application/x-troff-me", "mht": "message/rfc822", "mhtml": "message/rfc822", "mid": "x-music/x-midi", "midi": "x-music/x-midi", "mif": "application/x-mif", "mime": "www/mime", "mjf": "audio/x-vnd.audioexplosion.mjuicemediafile", "mjpg": "video/x-motion-jpeg", "mm": "application/x-meme", "mme": "application/base64", "mod": "audio/x-mod", "moov": "video/quicktime", "mov": "video/quicktime", "movie": "video/x-sgi-movie", "mp2": "video/x-mpeq2a", "mp3": "video/x-mpeg", "mpa": "video/mpeg", "mpc": "application/x-project", "mpe": "video/mpeg", "mpeg": "video/mpeg", "mpg": "video/mpeg", "mpga": "audio/mpeg", "mpp": "application/vnd.ms-project", "mpt": "application/x-project", "mpv": "application/x-project", "mpx": "application/x-project", "mrc": "application/marc", "ms": "application/x-troff-ms", "mv": "video/x-sgi-movie", "my": "audio/make", "mzz": "application/x-vnd.audioexplosion.mzz", "nap": "image/naplps", "naplps": "image/naplps", "nc": "application/x-netcdf", "ncm": "application/vnd.nokia.configuration-message", "nif": "image/x-niff", "niff": "image/x-niff", "nix": "application/x-mix-transfer", "nsc": "application/x-conference", "nvd": "application/x-navidoc", "o": "application/octet-stream", "oda": "application/oda", "omc": "application/x-omc", "omcd": "application/x-omcdatamaker", "omcr": "application/x-omcregerator", "p": "text/x-pascal", "p10": "application/x-pkcs10", "p12": "application/x-pkcs12", "p7a": "application/x-pkcs7-signature", "p7c": "application/x-pkcs7-mime", "p7m": "application/x-pkcs7-mime", "p7r": "application/x-pkcs7-certreqresp", "p7s": "application/pkcs7-signature", "part": "application/pro_eng", "pas": "text/pascal", "pbm": "image/x-portable-bitmap", "pcl": "application/x-pcl", "pct": "image/x-pict", "pcx": "image/x-pcx", "pdb": "chemical/x-pdb", "pdf": "application/pdf", "pfunk": "audio/make.my.funk", "pgm": "image/x-portable-greymap", "pic": "image/pict", "pict": "image/pict", "pkg": "application/x-newton-compatible-pkg", "pko": "application/vnd.ms-pki.pko", "pl": "text/x-script.perl", "plx": "application/x-pixclscript", "pm": "text/x-script.perl-module", "pm4": "application/x-pagemaker", "pm5": "application/x-pagemaker", "png": "image/png", "pnm": "image/x-portable-anymap", "pot": "application/vnd.ms-powerpoint", "pov": "model/x-pov", "ppa": "application/vnd.ms-powerpoint", "ppm": "image/x-portable-pixmap", "pps": "application/vnd.ms-powerpoint", "ppt": "application/x-mspowerpoint", "ppz": "application/mspowerpoint", "pre": "application/x-freelance", "prt": "application/pro_eng", "ps": "application/postscript", "psd": "application/octet-stream", "pvu": "paleovu/x-pv", "pwz": "application/vnd.ms-powerpoint", "py": "text/x-script.phyton", "pyc": "applicaiton/x-bytecode.python", "qcp": "audio/vnd.qcelp", "qd3": "x-world/x-3dmf", "qd3d": "x-world/x-3dmf", "qif": "image/x-quicktime", "qt": "video/quicktime", "qtc": "video/x-qtc", "qti": "image/x-quicktime", "qtif": "image/x-quicktime", "ra": "audio/x-realaudio", "ram": "audio/x-pn-realaudio", "ras": "image/x-cmu-raster", "rast": "image/cmu-raster", "rexx": "text/x-script.rexx", "rf": "image/vnd.rn-realflash", "rgb": "image/x-rgb", "rm": "audio/x-pn-realaudio", "rmi": "audio/mid", "rmm": "audio/x-pn-realaudio", "rmp": "audio/x-pn-realaudio-plugin", "rng": "application/vnd.nokia.ringing-tone", "rnx": "application/vnd.rn-realplayer", "roff": "application/x-troff", "rp": "image/vnd.rn-realpix", "rpm": "audio/x-pn-realaudio-plugin", "rt": "text/vnd.rn-realtext", "rtf": "text/richtext", "rtx": "text/richtext", "rv": "video/vnd.rn-realvideo", "s": "text/x-asm", "s3m": "audio/s3m", "saveme": "application/octet-stream", "sbk": "application/x-tbook", "scm": "video/x-scm", "sdml": "text/plain", "sdp": "application/x-sdp", "sdr": "application/sounder", "sea": "application/x-sea", "set": "application/set", "sgm": "text/x-sgml", "sgml": "text/x-sgml", "sh": "text/x-script.sh", "shar": "application/x-shar", "shtml": "text/html", "shtml": "text/x-server-parsed-html", "sid": "audio/x-psid", "sit": "application/x-stuffit", "skd": "application/x-koan", "skm": "application/x-koan", "skp": "application/x-koan", "skt": "application/x-koan", "sl": "application/x-seelogo", "smi": "application/smil", "smil": "application/smil", "snd": "audio/x-adpcm", "sol": "application/solids", "spc": "text/x-speech", "spl": "application/futuresplash", "spr": "application/x-sprite", "sprite": "application/x-sprite", "src": "application/x-wais-source", "ssi": "text/x-server-parsed-html", "ssm": "application/streamingmedia", "sst": "application/vnd.ms-pki.certstore", "step": "application/step", "stl": "application/x-navistyle", "stp": "application/step", "sv4cpio": "application/x-sv4cpio", "sv4crc": "application/x-sv4crc", "svf": "image/x-dwg", "svr": "x-world/x-svr", "swf": "application/x-shockwave-flash", "t": "application/x-troff", "talk": "text/x-speech", "tar": "application/x-tar", "tbk": "application/x-tbook", "tcl": "text/x-script.tcl", "tcsh": "text/x-script.tcsh", "tex": "application/x-tex", "texi": "application/x-texinfo", "texinfo": "application/x-texinfo", "text": "text/plain", "tgz": "application/x-compressed", "tif": "image/x-tiff", "tiff": "image/x-tiff", "tr": "application/x-troff", "tsi": "audio/tsp-audio", "tsp": "audio/tsplayer", "tsv": "text/tab-separated-values", "turbot": "image/florian", "txt": "text/plain", "uil": "text/x-uil", "uni": "text/uri-list", "unis": "text/uri-list", "unv": "application/i-deas", "uri": "text/uri-list", "uris": "text/uri-list", "ustar": "multipart/x-ustar", "uu": "text/x-uuencode", "uue": "text/x-uuencode", "vcd": "application/x-cdlink", "vcs": "text/x-vcalendar", "vda": "application/vda", "vdo": "video/vdo", "vew": "application/groupwise", "viv": "video/vnd.vivo", "vivo": "video/vnd.vivo", "vmd": "application/vocaltec-media-desc", "vmf": "application/vocaltec-media-file", "voc": "audio/x-voc", "vos": "video/vosaic", "vox": "audio/voxware", "vqe": "audio/x-twinvq-plugin", "vqf": "audio/x-twinvq", "vql": "audio/x-twinvq-plugin", "vrml": "x-world/x-vrml", "vrt": "x-world/x-vrt", "vsd": "application/x-visio", "vst": "application/x-visio", "vsw": "application/x-visio", "w60": "application/wordperfect6.0", "w61": "application/wordperfect6.1", "w6w": "application/msword", "wav": "audio/x-wav", "wb1": "application/x-qpro", "wbmp": "image/vnd.wap.wbmp", "web": "application/vnd.xara", "wiz": "application/msword", "wk1": "application/x-123", "wmf": "windows/metafile", "wml": "text/vnd.wap.wml", "wmlc": "application/vnd.wap.wmlc", "wmls": "text/vnd.wap.wmlscript", "wmlsc": "application/vnd.wap.wmlscriptc", "word": "application/msword", "wp": "application/wordperfect", "wp5": "application/wordperfect6.0", "wp6": "application/wordperfect", "wpd": "application/x-wpwin", "wq1": "application/x-lotus", "wri": "application/x-wri", "wrl": "x-world/x-vrml", "wrz": "x-world/x-vrml", "wsc": "text/scriplet", "wsrc": "application/x-wais-source", "wtk": "application/x-wintalk", "xbm": "image/xbm", "xdr": "video/x-amt-demorun", "xgz": "xgl/drawing", "xif": "image/vnd.xiff", "xl": "application/excel", "xla": "application/x-msexcel", "xlb": "application/x-excel", "xlc": "application/x-excel", "xld": "application/x-excel", "xlk": "application/x-excel", "xll": "application/x-excel", "xlm": "application/x-excel", "xls": "application/x-msexcel", "xlt": "application/x-excel", "xlv": "application/x-excel", "xlw": "application/x-msexcel", "xm": "audio/xm", "xml": "text/xml", "xmz": "xgl/movie", "xpix": "application/x-vnd.ls-xpix", "xpm": "image/xpm", "x-png": "image/png", "xsr": "video/x-amt-showrun", "xwd": "image/x-xwindowdump", "xyz": "chemical/x-pdb", "z": "application/x-compressed", "zip": "multipart/x-zip", "zoo": "application/octet-stream", "zsh": "text/x-script.zsh" };
            var getExtensionByType = function (type) {
                for (var i in mimeTypes) {
                    if (mimeTypes.hasOwnProperty(i)) {
                        if (mimeTypes[i] === type) {
                            return i;
                        }
                    }
                }
                return "png";//default extension
            }

            //constructor - prepare
            this.init = function (src) {
                source = src;

                var $document = $(document);
                //handlers
                $document.on('keydown', function (e) {
                    self.on_keyboard_action(e);
                }); //firefox fix
                $document.on('keyup', function (e) {
                    self.on_keyboardup_action(e);
                }); //firefox fix
                $document.on('paste', function (e) {
                    self.paste_auto(e);
                }); //official paste handler

                //if using auto
                if (window.Clipboard)
                    return true;

                pasteCatcher = document.createElement("div");
                pasteCatcher.setAttribute("id", "paste_ff");
                pasteCatcher.setAttribute("contenteditable", "");
                pasteCatcher.style.cssText = 'opacity:0;position:fixed;top:0px;left:0px;';
                pasteCatcher.style.marginLeft = "-20px";
                pasteCatcher.style.width = "10px";
                document.body.appendChild(pasteCatcher);
                document.getElementById('paste_ff').addEventListener('DOMSubtreeModified', function () {
                    if (pasteMode === 'auto' || ctrlPressed === false)
                        return true;
                    //if paste handle failed - capture pasted object manually
                    if (pasteCatcher.children.length === 1) {
                        if (pasteCatcher.firstElementChild.src != undefined) {
                            //image
                            clearScene();                            
                            self.paste_createImage(pasteCatcher.firstElementChild.src);
                            var blob = self.dataURItoBlob(pasteCatcher.firstElementChild.src);
                            self.allowSave(blob);
                        }
                    }
                    //register cleanup after some time.
                    setTimeout(function () {
                        pasteCatcher.innerHTML = '';
                    }, 20);
                }, false);
            };

            this.reset = function () {
                var $document = $(document);
                $document.off('keydown');
                $document.off('keyup');
                $document.off('paste');
                $("#paste_ff").remove();
            };
            //default paste action
            this.paste_auto = function (e) {
                pasteMode = '';
                pasteCatcher.innerHTML = '';
                var clipboardData = (e.clipboardData || e.originalEvent.clipboardData);
                var isIe = !clipboardData && window.clipboardData; //IE
                if (isIe) {
                    clipboardData = window.clipboardData;
                }
                if (clipboardData) {
                    var items = clipboardData.items;
                    if (isIe) {
                        items = clipboardData.files; //IE
                    }
                    if (items) {
                        pasteMode = 'auto';
                        var blob = null;
                        //access data directly
                        for (var i = 0; i < items.length; i++) {
                            if (items[i].type.indexOf("image") !== -1) {
                                //image
                                if (isIe) {
                                    blob = items[i];
                                } else {
                                    blob = items[i].getAsFile();
                                }
                            }
                        }
                        if (blob !== null) {
                            var URLObj = window.URL || window.webkitURL;
                            var source = URLObj.createObjectURL(blob);
                            this.paste_createImage(source);
                            this.allowSave(blob);
                        }
                        e.preventDefault();
                    }
                }
            };
            //on keyboard press - 
            this.on_keyboard_action = function (event) {
                k = event.keyCode;
                //ctrl
                if (k === 17 || event.metaKey || event.ctrlKey) {
                    if (ctrlPressed === false)
                        ctrlPressed = true;
                }
                //v
                if (k === 86) {
                    if (ctrlPressed === true && !window.Clipboard)
                        pasteCatcher.focus();
                }
            };
            //on kaybord release
            this.on_keyboardup_action = function (event) {
                k = event.keyCode;
                //ctrl
                if (k === 17 || event.metaKey || event.ctrlKey || event.key == 'Meta')
                    ctrlPressed = false;
            };

            this.dataURItoBlob = function (dataURI) {
                var byteString;

                if (dataURI.split(',')[0].indexOf('base64') !== -1) {
                    byteString = atob(dataURI.split(',')[1]);
                } else {
                    byteString = decodeURI(dataURI.split(',')[1]);
                }

                var mimestring = dataURI.split(',')[0].split(':')[1].split(';')[0];

                var content = new Array();
                for (var i = 0; i < byteString.length; i++) {
                    content[i] = byteString.charCodeAt(i);
                }

                return new Blob([new Uint8Array(content)], { type: mimestring });
            };

            //draw image
            this.paste_createImage = function (dataUrl) {
                
                var $previewPnl = $('#previewPnl');
                var imgCtrl = $previewPnl.find('img');
                if (imgCtrl.length === 0) {
                    $previewPnl.append('<img style="width:400px;height:400px;" />');
                    imgCtrl = $previewPnl.find('img');
                }
                imgCtrl[0].src = dataUrl;
            };

            this.allowSave = function (blob) {                
                var uploadModal = $('#upload_clipboard_file_popup');
                var $btnSave = uploadModal.find('#btnSave');
                var extension = getExtensionByType(blob.type);
                var imgFilenameCtrl = uploadModal.find("#imgFilename");
                var key;
                var submitUrl;
                var refredhCallback;
                var imgFilename = imgFilenameCtrl.val();

                if (source == 'case') {
                    key = caseFileKey;
                    submitUrl = uploadCaseFileUrl;
                    refredhCallback = function () {
                        $.get(newCaseFilesUrl, { id: caseFileKey, curTime: Date.now() }, function (files) {
                            Application.prototype.refreshNewCaseFilesTable(files);
                        });
                    }
                }

                if (imgFilename.length === 0) {
                    imgFilename = 'image_' + Application.prototype.generateRandomKey();
                }
                if (imgFilename.indexOf('.') === -1) {
                    imgFilename = imgFilename + '.' + extension;
                }
                imgFilenameCtrl.val(imgFilename);

                $btnSave.on('click', function () {
                    if (imgFilenameCtrl.val() == "")
                        return;

                    var fd = new FormData();
                    uploadModal.find('form').submit();
                    if (imgFilenameCtrl[0].validity.valid) {
                        fd.append('name', imgFilenameCtrl.val());
                        fd.append('id', key);
                        fd.append('file', blob);                        
                        $.ajax({
                            type: 'POST',
                            url: submitUrl,
                            data: fd,
                            processData: false,
                            contentType: false
                        }).done(function (data) {
                            //console.log(data);
                            refredhCallback();
                            uploadModal.modal("hide");
                        });
                    }
                });
                $btnSave.show();
            }
        }       

        $("#imgFilename").on('change', function () {
            if ($("#imgFilename").val() == "")
                $('#imageNameRequired').show();
            else
                $('#imageNameRequired').hide();
        });

        function clearScene() {
            curFileName = 'image_' + Application.prototype.generateRandomKey();
            $("#previewPnl").empty();
            var $uploadModal = $('#upload_clipboard_file_popup');
            var $btnSave = $uploadModal.find('#btnSave');
            $btnSave.hide();
            $btnSave.off('click');
            $uploadModal.find("input").val('');
            $('#imageNameRequired').hide();
        }

        function resetClipboard() {
            clearScene();
            globalClipboard.reset();
        }

        $("a[href='#upload_clipboard_file_popup']").on('click', function (e) {
            var $src = $(this);
            var $target = $('#upload_clipboard_file_popup');
            e.preventDefault();            

            $target.attr('data-src', $src.attr('data-src'));

            globalClipboard = new ClipboardClass();
            resetClipboard();
            $target.modal('show');            
            globalClipboard.init.call(globalClipboard, $(e.target).attr('data-src'));
        });

        function bindProductAreasEvents() {
            $('#divProductArea ul.dropdown-menu li a').click(function (e) {
                e.preventDefault();
                var val = $(this).attr('value');
                $("#divBreadcrumbs_ProductArea").text(getBreadcrumbs(this));
                var ee = document.getElementById("NewCase_ProductArea_Id");
                ee.setAttribute('value', val);
            });
        }
       
        Application.prototype.init = function () {
            var self = this;
            customerId = $('#NewCase_Customer_Id').val();
            self.$regionControl = $('#NewCase_Region_Id');
            self.$departmentControl = $('#NewCase_Department_Id');
            self.$orgUnitControl = $('#NewCase_Ou_Id');
            self.$caseTypeControl = $(document.getElementById("NewCase.CaseType_Id"));
            self.$priorityControl = $(document.getElementById("NewCase_Priority_Id"));

            $('#NewCase_ReportedBy').typeahead(Application.prototype._GetComputerUserSearchOptions());
            $('#NewCase_InventoryNumber').typeahead(Application.prototype._GetComputerSearchOptions());

            // Remove after implementing http://redmine.fastdev.se/issues/10995
            self.$regionControl.on('change', function () {
                self.refreshDepartment.call(self, $(this).val());
            });

            self.$departmentControl.on('change', function () {
                // Remove after implementing http://redmine.fastdev.se/issues/10995        
                var departmentId = $(this).val();
                self.refreshOrganizationUnit(departmentId);
            });

            self.$caseTypeControl.change(function () {
                $.post('/Case/GetProductAreaByCaseType/', { caseTypeId: $(this).val() }, function (result) {
                    if (result.success) {
                        $('#divProductArea > ul.dropdown-menu')
                            .html("<li><a href='#'>--</a></li>" + result.data);
                        var paId = parseInt($("#NewCase_ProductArea_Id").val());
                        if (result.paIds && result.paIds.indexOf(paId) < 0) {
                            var emptyElement = $('#divProductArea > ul.dropdown-menu').children().first();
                            $("#divBreadcrumbs_ProductArea").text(getBreadcrumbs(emptyElement));
                            $("#NewCase_ProductArea_Id").val("").trigger('change');
                        }
                        bindProductAreasEvents();
                    }
                }, 'json');

                self.checkRelationRules($(this), caseTypeRelatedFields, caseTypeRelationName);
            });

            self.$priorityControl.change(function () {
                self.checkRelationRules($(this), priorityRelatedFields, priorityRelationName);
            });

            self.applyFieldAttributes();
            self.$caseTypeControl.change();
            self.$priorityControl.change();
        };
    })($);
    
    //tab arrows
    $("button.dropdown-toggle[data-toggle=dropdown]").on("click", function (e) {
        $(this).parent().find("li.dropdown-submenu > ul").css("display", "");
    });

    $("button.dropdown-toggle[data-toggle=dropdown], ul.dropdown-menu").on("keydown", function (e) {
        if (!/(37|38|39|40|27)/.test(e.keyCode)) return true;

        var $this = $(this);

        e.preventDefault();
        e.stopPropagation();

        if ($this.is(".disabled, :disabled")) return true;

        var $group = $this.closest(".btn-group");
        var isActive = $group.hasClass("open");
        var $parent = $this.parent();
        var $items = $parent.children("ul.dropdown-menu").children("li:not(.divider):visible").children("a");
        var index = 0;

        if (isActive && e.keyCode === 27) {
            if (e.which === 27) $group.find("button.dropdown-toggle[data-toggle=dropdown]").focus();
            return $this.click();
        }

        if (!isActive && e.keyCode === 40) {
            var open = $this.click();
            $items = $group.children("ul.dropdown-menu").children("li:not(.divider):visible").children("a");
            if (!$items.length) return open;
            $items.eq(index).focus();
            return open;
        }

        if (!$items.length) return true;

        index = $items.index($items.filter(":focus"));

        if (e.keyCode === 38 && index > 0) index--; // up
        if (e.keyCode === 40 && index < $items.length - 1) index++; // down
        if (!~index) index = 0;

        var currentItem = $items.eq(index);

        if (e.keyCode === 39) {
            var currentLi = currentItem.parent();
            if (currentLi.hasClass("dropdown-submenu")) {
                currentLi.children("ul.dropdown-menu").css("display", "block");
                currentItem = currentLi.children("ul.dropdown-menu").children("li:not(.divider):visible:first").children("a").first();
            }
        }

        if (e.keyCode === 37) {
            if ($parent.hasClass("dropdown-submenu")) {
                currentItem = $parent.children("a:first");
                $this.css("display", "");
            }
        }

        currentItem.focus();

        return true;
    });

    $("ul.dropdown-menu li a").click(function (e) {
        //var toggler = $(this).parents("ul.dropdown-menu").prevAll("button.dropdown-toggle[data-toggle=dropdown]");
        var toggler = $(this).closest(".btn-group.open").find("button.dropdown-toggle[data-toggle=dropdown]");
        if (toggler.length) {
            toggler.focus();
        }
        return true;
    });

    Application.prototype.init();
});