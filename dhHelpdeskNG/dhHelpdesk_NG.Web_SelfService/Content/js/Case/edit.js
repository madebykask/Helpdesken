window.casePage = (function ($) {

        var fieldSettings = window.fieldSettings || {};
        var caseTypeRelatedFields = window.caseTypeRelatedFields || {};
        
        var uploadCaseFileUrl = window.appParameters.uploadCaseFileUrl;
        var caseFileKey = window.appParameters.caseFileKey;
        var newCaseFilesUrl = window.appParameters.newCaseFilesUrl;
        var fileAlreadyExistsMsg = window.appParameters.fileAlreadyExistsMsg;
        var downloadCaseFileUrl = window.appParameters.downloadCaseFileUrl;
        var downloadCaseFileParamUrl = window.appParameters.downloadCaseFileParamUrl;
        var deleteCaseFileUrl = window.appParameters.deleteCaseFileUrl;
        var searchUserUrl = window.appParameters.searchUserUrl;
        var seachComputerUrl = window.appParameters.seachComputerUrl;
        var changeSystemUrl = window.appParameters.changeSystemUrl;
        var saveNewCaseUrl = window.appParameters.saveNewCaseUrl;
        var departmentsUrl = window.appParameters.fetchDepartmentsUrl;
        var OUsUrl = window.appParameters.fetchOUUrl;
        var setProductAreaByCaseTypeUrl = window.appParameters.setProductAreaByCaseTypeUrl;
        var fileUploadWhiteList = window.appParameters.fileUploadWhiteList;
        var invalidFileExtensionText = window.appParameters.invalidFileExtensionText;
        var requiredMessage = window.appParameters.requiredMessage;
        var customerId;
        var alreadyExistFileIds = [];
        var attrApplyClass = "hasAttribute";
        var attrFieldNameHolder = "standardName";
        var relatedFieldBlockPrefix = "#relatedFieldBlock-";
        var mandatorySignPrefix = "#mandatory_sign_";

        function CasePage() {
        }

        CasePage.prototype.applyFieldAttributes = function () {
            if (fieldSettings == undefined || fieldSettings == null || fieldSettings.length <= 0)
                return;

            var needToApplyAttrs = document.getElementsByClassName(attrApplyClass);
            for (var i = 0; i < needToApplyAttrs.length; i++) {
                var fieldStandardName = $(needToApplyAttrs[i]).attr(attrFieldNameHolder);                
                var fs = getFieldSetting(fieldStandardName);
                if (fs != null) {
                    applySettingOnElement($(needToApplyAttrs[i]), fs);
                }
            }
        }

        CasePage.prototype.isElementReadonly = function ($element) {
            if ($element == undefined || $element == null)
                return;

            var fieldStandardName = $($element).attr(attrFieldNameHolder);
            var fs = getFieldSetting(fieldStandardName);
            if (fs != null && fs.IsReadonly)
                return true;

            return false;
        }

        CasePage.prototype.checkCaseTypeRelationRules = function ($caseType) {
            if ($caseType == undefined || $caseType == null)
                return;

            var caseTypeId = $caseType.val();
            var relatedField = getCaseTypeRelatedField(caseTypeId);

            if (relatedField == undefined || relatedField == null || relatedField == '') {
                removeCaseTypeRule('Impact_Id', '#NewCase_Impact_Id');
                removeCaseTypeRule('Urgency_Id', '#NewCase_Urgency_Id');
                return;
            }

            switch (relatedField.toLowerCase()) {
                case 'impact':
                    addCaseTypeRule('Impact_Id', '#NewCase_Impact_Id');
                    addCaseTypeRule('Urgency_Id', '#NewCase_Urgency_Id');
                    break;
            }
        }

        CasePage.prototype.isNullOrEmpty = function (val) {
            return val == undefined || val == null || val == "";
        }

        CasePage.prototype.isNullOrUndefined = function (val) {
            return val == undefined || val == null;
        }

        CasePage.prototype.setValueToBtnGroup = function (domContainer, domText, domValue, value) {
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

        CasePage.prototype.getBreadcrumbs = function (el) {
            var self = this;
            var path = $(el).text();
            var $parent = $(el).parents("li").eq(1).find("a:first");
            if ($parent.length === 1) {
                path = self.getBreadcrumbs($parent) + " - " + path;
            }
            return path;
        }

        CasePage.prototype.parseDate = function (dateStr) {
            if (dateStr == undefined || dateStr == "")
                return null;

            var dateArray = dateStr.split("-");
            if (dateArray.length != 3)
                return null;

            return new Date(parseInt(dateArray[0]), parseInt(dateArray[1] - 1), parseInt(dateArray[2]));
        }

        CasePage.prototype.dateToDisplayFormat = function (dateValue) {
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

        CasePage.prototype.padLeft = function (value, totalLength, padChar) {
            var valLen = value.toString().length;
            var diff = totalLength - valLen;
            if (diff > 0) {
                for (var i = 0; i < diff; i++)
                    value = padChar + value;
            }
            return value;
        }

        CasePage.prototype.getDate = function (val) {
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

        var hideElement = function ($element) {
            if ($element && $element.length)
                $element.hide();
        }

        var showElement = function ($element) {
            if ($element && $element.length)
                $element.show();
        }

        var canApplyCaseTypeRule = function (standardFieldName) {
            var impactSetting = getFieldSetting(standardFieldName);
            if (impactSetting == null || (impactSetting != null && !impactSetting.IsVisible))
                return true;

            return false;
        }

        var isFileInWhiteList = function (filename, whiteList) {
            if (filename.indexOf('.') !== -1) {
                var extension = filename.split('.').reverse()[0].toLowerCase();
                if (whiteList.indexOf(extension) >= 0)
                    return true;
            }
            else {
                if (whiteList.indexOf('') >= 0)
                    return true;
            }
            return false;
        }

        var addCaseTypeRule = function (standardFieldName, elementId) {
            var impactSetting = getFieldSetting(standardFieldName);
            if (impactSetting == null || (impactSetting != null && !impactSetting.IsVisible)) {
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

        var removeCaseTypeRule = function (standardFieldName, elementId) {
            var impactSetting = getFieldSetting(standardFieldName);
            if (impactSetting == null || (impactSetting != null && !impactSetting.IsVisible)) {
                var $elmToHide = $(relatedFieldBlockPrefix + standardFieldName);
                $(elementId).val('').change();
                hideElement($elmToHide);
            }

            if (impactSetting == null || (impactSetting != null && !impactSetting.IsRequired)) {
                $(elementId).removeAttr('required');
                $(elementId).rules("remove");
                var $mandatorySign = $(mandatorySignPrefix + standardFieldName);
                hideElement($mandatorySign);
            }
        }

        var getCaseTypeRelatedField = function (caseTypeId) {
            if (caseTypeRelatedFields == undefined || caseTypeRelatedFields == null || caseTypeRelatedFields.length <= 0 || caseTypeId == undefined || caseTypeId == null)
                return "";

            for (var ct = 0; ct < caseTypeRelatedFields.length; ct++) {
                if (caseTypeRelatedFields[ct].Key == caseTypeId)
                    return caseTypeRelatedFields[ct].Value;
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
        
        /**
        * @public
        * @param { number } regionId
        * @param { number } departmentId
        * @param { number } selectedOrgUnitId
        */
        CasePage.prototype.setOrganizationData = function (regionId, departmentId, selectedOrgUnitId) {
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
        CasePage.prototype.isIdExistsInSelect = function ($select, optionId) {
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

        CasePage.prototype.refreshDepartment = function (regionId, selectedId, selectedOrgUnitId) {
            var me = this;
            me.$departmentControl.val('').find('option').remove();
            me.$departmentControl.append('<option value="">&nbsp;</option>');

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
        CasePage.prototype.refreshOrganizationUnit = function (departmentId, selectedId) {
            var me = this;
            me.$orgUnitControl.val('').find('option').remove();
            me.$orgUnitControl.append('<option value="">&nbsp;</option>');

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
        CasePage.prototype.makeOptionsFromIdName = function (data) {
            var content = [];
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                content.push("<option value='" + item.id + "'>" + item.name + "</option>");
            }
            return content.join('');
        }

        PluploadTranslation($("#case_languageId").val());
        var lastInitiatorSearchKey = '';

        var $fileUploader = $('#upload_files_popup').find('#file_uploader');

        $fileUploader.pluploadQueue({
            runtimes: 'html5,html4',
            url: uploadCaseFileUrl,
            multipart_params: { Id: caseFileKey, curTime: Date.now() },
            max_file_size: '10mb',

            init: {
                FileUploaded: function () {

                    $.get(newCaseFilesUrl, { id: caseFileKey, curTime: Date.now() }, function (files) {
                        window.casePage.refreshNewCaseFilesTable(files);
                    });
                },

                UploadComplete: function (up, file) {
                    //plupload_add
                    $(".plupload_buttons").css("display", "inline");
                    $(".plupload_upload_status").css("display", "inline");
                    up.refresh();
                },
                FilesAdded: function (up, files) {
                    if (fileUploadWhiteList != null) {
                        var whiteList = fileUploadWhiteList;

                        files.forEach(function (e) {
                            if (!isFileInWhiteList(e.name, whiteList)) {
                                up.removeFile(e);
                                alert(e.name + ' ' + invalidFileExtensionText);
                            }
                        })

                    }
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
                        $fileUploader.find('ul[class="plupload_filelist"] li[id="' + fileId + '"] div[class="plupload_file_action"] a').prop('title', fileAlreadyExistsMsg);
                    }

                    alreadyExistFileIds = [];
                    uploader.refresh();
                }
            }
        });

        $('#upload_files_popup').on('shown.bs.modal', function () {
            //refresh required to make file open dlg work correctly 
            $fileUploader.pluploadQueue().refresh();
        });

        $('#upload_files_popup').on('hidden.bs.modal', function () {
            if ($fileUploader && $fileUploader.length) {
                if ($fileUploader.pluploadQueue().files.length > 0) {
                    if ($fileUploader.pluploadQueue().state == plupload.UPLOADING)
                        $fileUploader.pluploadQueue().stop();

                    while ($fileUploader.pluploadQueue().files.length > 0) {
                        $fileUploader.pluploadQueue().removeFile($fileUploader.pluploadQueue().files[0]);
                    }
                    $fileUploader.pluploadQueue().refresh();
                }
            }
        });

        CasePage.prototype.refreshNewCaseFilesTable = function (files) {
            $('#NewCasefiles_table > tbody > tr').remove();

            var fileMarkup;

            for (var i = 0; i < files.length; i++) {
                var file = files[i];

                fileMarkup =
                    $('<tr>' +
                        '<td>' +
                            '<i class="glyphicon glyphicon-file">&nbsp;</i><a style="color:blue" href="' + downloadCaseFileUrl + '?' + downloadCaseFileParamUrl + 'fileName=' + file + '">' + file + '</a>' +
                        '</td>' +
                        '<td style="text-align:right !important;">' +
                            '<a id="delete_file_button_' + i + '" class="btn btn-default btn-sm" ><span class="glyphicon glyphicon-remove"></span> </a>' +
                        '</td>' +
                        '</tr>');

                $('#NewCasefiles_table > tbody').append(fileMarkup);
            }
            
            this.bindDeleteNewCaseFileBehaviorToDeleteButtons();
            this.countNrOfAttatchedFiles();
        }

        CasePage.prototype.countNrOfAttatchedFiles = function () {
            if ($('#attachment-tab').length) {
                var nrOfFiles = parseInt($('#NewCasefiles_table tr').length) || 0;
                if (nrOfFiles > 0 && $('#nrOfAttachedFiles')[0])
                    {
                    $('#nrOfAttachedFiles').html('(' + nrOfFiles + ')');
                }
                else {
                    $('#nrOfAttachedFiles').html('');
                }
            }
        }

        CasePage.prototype.bindDeleteNewCaseFileBehaviorToDeleteButtons = function () {
            
            $('#NewCasefiles_table a[id^="delete_file_button_"]').click(function () {
                var fileName = $(this).parents('tr:first').children('td:first').children('a').text();
                var pressedDeleteFileButton = this;

                $.post(deleteCaseFileUrl, { id: caseFileKey, fileName: fileName, curTime: Date.now() }, function () {
                    $(pressedDeleteFileButton).parents('tr:first').remove();
                    CasePage.prototype.countNrOfAttatchedFiles();
                });
            });
        }

        CasePage.prototype.generateRandomKey = function () {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                  .toString(16)
                  .substring(1);
            }
            return s4() + '-' + s4() + '-' + s4();
        }

        CasePage.prototype._GetComputerUserSearchOptions = function () {
            var me = this;
            var fieldsVisibility = [];
            var options = {
                items: 20,
                minLength: 2,

                source: function (query, process) {
                    lastInitiatorSearchKey = me.generateRandomKey();

                    return me.runComputerUserSearchDataQuery(query, lastInitiatorSearchKey)
                        .done(function (result) {
                        if (result.searchKey !== lastInitiatorSearchKey)
                            return;
                        fieldsVisibility = result.fieldsVisibility;
                        var resultList = jQuery.map(result.result,
                            function(item) {
                                var aItem = {
                                    id: item.Id,
                                    num: item.UserId,
                                    name: item.FirstName + ' ' + item.SurName,
                                    email: item.Email,
                                    place: item.Location,
                                    phone: item.Phone,
                                    usercode: item.UserCode,
                                    cellphone: item.CellPhone,
                                    regionid: item.Region_Id,
                                    regionname: item.RegionName,
                                    departmentid: item.Department_Id,
                                    departmentname: item.DepartmentName,
                                    ouid: item.OU_Id,
                                    ouname: item.OUName,
                                    name_family: item.SurName + ' ' + item.FirstName,
                                    customername: item.CustomerName,
                                    costcentre: item.CostCentre

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
                    me.updateInitiator(item);

                    return item.num;
                }
            };

            return options;
    }

    CasePage.prototype.runComputerUserSearchDataQuery = function(query, lastInitiatorSearchKey) {
        return $.ajax({
            url: searchUserUrl,
            type: 'post',
            data: {
                query: query,
                customerId: $('#NewCase_Customer_Id').val(),
                searchKey: lastInitiatorSearchKey
            },
            dataType: 'json'
        });
    }

    CasePage.prototype.updateInitiator = function(item) {
        $('#NewCase_ReportedBy').val(item.num);
        $('#NewCase_PersonsName').val(item.name);
        $('#NewCase_PersonsEmail').val(item.email);
        $('#NewCase_PersonsPhone').val(item.phone);
        $('#NewCase_PersonsCellphone').val(item.cellphone);
        $('#NewCase_Place').val(item.place);
        $('#NewCase_UserCode').val(item.usercode);
        $('#NewCase_Region_Id').val(item.regionid);
        $('#NewCase_CostCentre').val(item.costcentre);
        $('#NewCase_Department_Id').val(item.departmentid);

        this.setOrganizationData(item.regionid, item.departmentid, item.ouid);
    }

        CasePage.prototype._GetComputerSearchOptions = function () {

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

        function refreshCaseFiles(res) {
            var data = { id: caseFileKey, curTime: Date.now() };
            $.get(newCaseFilesUrl, $.param(data), function (files) {
                window.casePage.refreshNewCaseFilesTable(files);
            });
        }

        /*********** Paste image from clipboard (Case) ************/
        $("#addCaseFileFromClipboard").on('click', function (e) {
            e.preventDefault();
            var opt = {
                refreshCallback: refreshCaseFiles,
                fileKey: caseFileKey,
                submitUrl: uploadCaseFileUrl
            };
            var clipboardFileUpload = new ClipboardFileUpload(opt);
            clipboardFileUpload.show();

            return false;
        });
        /*****************************************************/

    function bindProductAreasEvents() {

            $('#divProductArea ul.dropdown-menu li a').click(function (e) {
                e.preventDefault();
                var val = $(this).attr('value');
                if (val == undefined)
                    val = "";
                var isMandatory = $('#isMandatory').val();
                if (isMandatory == 'true') {
                    $("#NewCase_ProductArea_Id").rules("add", {
                        required: true,
                        messages: {
                            required: requiredMessage
                        }
                    });
                }
                $("#divBreadcrumbs_ProductArea").text(getBreadcrumbs(this));
                
                var ee = document.getElementById("NewCase_ProductArea_Id");
                ee.setAttribute('value', val);
                $(ee).trigger('change');
                
            });


            $('#divProductArea .dropdown-submenu.DynamicDropDown_Up').off('mousemove').on('mousemove', function (event) {
                dynamicDropDownBehaviorOnMouseMove(event.target.parentElement);
            });

    }

       
        CasePage.prototype.init = function (settings) {
            var self = this;
            customerId = $('#NewCase_Customer_Id').val();
            self.$regionControl = $('#NewCase_Region_Id');
            self.$departmentControl = $('#NewCase_Department_Id');
            self.$orgUnitControl = $('#NewCase_Ou_Id');
            self.$caseTypeControl = $(document.getElementById("NewCase.CaseType_Id"));
            self.settings = $.extend({ useInitiatorAutocomplete: true }, settings);
            
            //tab arrows
            $("button.dropdown-toggle[data-toggle=dropdown]").on("click", function (e) {
                $(this).parent().find("li.dropdown-submenu > ul").css("display", "");
            });

            //$("button.dropdown-toggle[data-toggle=dropdown], ul.dropdown-menu").on("keydown", function (e) {
            //    if (!/(37|38|39|40|27)/.test(e.keyCode)) return true;

            //    var $this = $(this);

            //    e.preventDefault();
            //    e.stopPropagation();

            //    if ($this.is(".disabled, :disabled")) return true;

            //    var $group = $this.closest(".btn-group");
            //    var isActive = $group.hasClass("open");
            //    var $parent = $this.parent();
            //    var $items = $parent.children("ul.dropdown-menu").children("li:not(.divider):visible").children("a");
            //    var index = 0;

            //    if (isActive && e.keyCode === 27) {
            //        if (e.which === 27) $group.find("button.dropdown-toggle[data-toggle=dropdown]").focus();
            //        return $this.click();
            //    }

            //    if (!isActive && e.keyCode === 40) {
            //        var open = $this.click();
            //        $items = $group.children("ul.dropdown-menu").children("li:not(.divider):visible").children("a");
            //        if (!$items.length) return open;
            //        $items.eq(index).focus();
            //        return open;
            //    }

            //    if (!$items.length) return true;

            //    index = $items.index($items.filter(":focus"));

            //    if (e.keyCode === 38 && index > 0) index--; // up
            //    if (e.keyCode === 40 && index < $items.length - 1) index++; // down
            //    if (!~index) index = 0;

            //    var currentItem = $items.eq(index);

            //    if (e.keyCode === 39) {
            //        var currentLi = currentItem.parent();
            //        if (currentLi.hasClass("dropdown-submenu")) {
            //            currentLi.children("ul.dropdown-menu").css("display", "block");
            //            currentItem = currentLi.children("ul.dropdown-menu").children("li:not(.divider):visible:first").children("a").first();
            //        }
            //    }

            //    if (e.keyCode === 37) {
            //        if ($parent.hasClass("dropdown-submenu")) {
            //            currentItem = $parent.children("a:first");
            //            $this.css("display", "");
            //        }
            //    }

            //    currentItem.focus();

            //    return true;
            //});

            $("ul.dropdown-menu li a").click(function (e) {
                //var toggler = $(this).parents("ul.dropdown-menu").prevAll("button.dropdown-toggle[data-toggle=dropdown]");
                var toggler = $(this).closest(".btn-group.open").find("button.dropdown-toggle[data-toggle=dropdown]");
                if (toggler.length) {
                    toggler.focus();
                }
                return true;
            });

            const reportedBy$ = $('#NewCase_ReportedBy');
            if (self.settings.useInitiatorAutocomplete) {
                reportedBy$.typeahead(self._GetComputerUserSearchOptions()).focus();
            } else {
                reportedBy$.on('blur', function() {
                    var value = $(this).val();
                    if (value && value.length < 2) return;

                    lastInitiatorSearchKey = self.generateRandomKey();
                    self.runComputerUserSearchDataQuery(value, lastInitiatorSearchKey)
                        .done(function(result) {
                            if (result.searchKey !== lastInitiatorSearchKey || result.result.length > 1 || result.result.length === 0)
                                return;
                            var resultList = $.map(result.result,
                                function (item) {
                                    return {
                                        id: item.Id,
                                        num: item.UserId,
                                        name: item.FirstName + ' ' + item.SurName,
                                        email: item.Email,
                                        place: item.Location,
                                        phone: item.Phone,
                                        usercode: item.UserCode,
                                        cellphone: item.CellPhone,
                                        regionid: item.Region_Id,
                                        regionname: item.RegionName,
                                        departmentid: item.Department_Id,
                                        departmentname: item.DepartmentName,
                                        ouid: item.OU_Id,
                                        ouname: item.OUName,
                                        name_family: item.SurName + ' ' + item.FirstName,
                                        customername: item.CustomerName,
                                        costcentre: item.CostCentre

                                    };
                                });
                            self.updateInitiator(resultList[0]);
                        });
                });
                reportedBy$.focus();
            }
            $('#NewCase_InventoryNumber').typeahead(self._GetComputerSearchOptions());

            // Remove after implementing http://redmine.fastdev.se/issues/10995
            self.$regionControl.on('change', function () {
                self.refreshDepartment.call(self, $(this).val());
            });

            self.$departmentControl.on('change', function () {
                // Remove after implementing http://redmine.fastdev.se/issues/10995        
                var departmentId = $(this).val();
                self.refreshOrganizationUnit(departmentId);
            });

            $('#NewCase_System_Id').on('change', function () {
                var data = { id: $('#NewCase_System_Id').val() };
                $.getJSON(changeSystemUrl, $.param(data), function(res) {
                    $('#NewCase_Urgency_Id').val(res.urgencyId || '').change();
                });
            });

            self.$caseTypeControl.change(function () {
                $.post(setProductAreaByCaseTypeUrl, { caseTypeId: $(this).val() }, function (result) {
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

                self.checkCaseTypeRelationRules($(this));
            });

            self.applyFieldAttributes();
            self.$caseTypeControl.change();
        };

        return new CasePage();
})(jQuery);
