﻿(function($) {
    "use strict";
    window.EditOrder = function (options) {
        this._options = $.extend({
            statuses: []
        }, options);       
    }
    
    window.EditOrder.prototype = {        
        init: function () {
            var that = this;

            ///////////////////////////////////////////////////////////////////////////////Old code
            var lastInitiatorSearchKey = "";

            var fileUploadWhiteList = window.parameters.fileUploadWhiteList;
            var invalidFileExtensionText = window.parameters.invalidFileExtensionText;

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
            };

            $("#fileName_files_uploader").pluploadQueue({

                url: that._options.uploadFileUrl,
                multipart_params: { entityId: that._options.id, subtopic: that._options.fileNameSubtopic },
                max_file_size: "10mb",
                multi_selection: false,
                max_files: 1,
                multiple_queues: true,
                
                init: {
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

                        if (up.files.length >= up.settings.max_files) {
                            up.splice(up.settings.max_files);
                            if (typeof up.settings.browse_button === "string") {
                                $("#" + up.settings.browse_button).hide();
                            } else {
                                $(up.settings.browse_button).hide();
                            }
                            $("#fileName_files_uploader_popup").find("input[type='file']")
                                .prop("disabled", true).parent().css("z-index", -10000);
                        }
                    },
                    FilesRemoved: function(up, files) {
                        if (up.files.length < up.settings.max_files) {
                            if (typeof up.settings.browse_button === "string") {
                                $("#" + up.settings.browse_button).show();
                            } else {
                                $(up.settings.browse_button).show();
                            }
                            $("#fileName_files_uploader_popup").find("input[type='file']")
                                .prop("disabled", false).parent().css("z-index", 0);
                        }
                    },
                    FileUploaded: function (uploader, uploadedFile, responseContent) {
                        $("#file_container").html(responseContent.response);
                    }
                }
            });

            $("#log_send_to_button").button().on("click",function () {
                $("#log_send_to_dialog").dialog("open");
            });

          
            window.deleteFile = function (subtopic, fileName, filesContainerId) {
                $.post(that._options.deleteFileUrl, { entityId: that._options.id, subtopic: subtopic, fileName: fileName }, function (markup) {
                    $("#" + filesContainerId).html(markup);
                });
            };

            window.deleteLog = function (subtopic, logId, logsContainerId) {
                $.post(that._options.deleteLogUrl, { orderId: that._options.id, subtopic: subtopic, logId: logId }, function (markup) {
                    $("#" + logsContainerId).html(markup);
                });
            };

            window.fillEmailsTextArea = function (textAreaId, emails) {
                var emailsText = "";

                for (var i = 0; i < emails.length; i++) {
                    if (i !== 0) {
                        emailsText += "\n";
                    }

                    emailsText += emails[i];
                }

                $("#" + textAreaId).val(emailsText);
            };

            window.fillLogSendLogNoteToEmailsTextArea = function (emails) {
                this.fillEmailsTextArea("log_send_to_emails_textarea", emails);
            };

            function getOrderComputerUserSearchOptions(updateCallback) {

                var options = {
                    items: 20,
                    minLength: 2,

                    source: function (query, process) {
                        lastInitiatorSearchKey = generateRandomKey();
                        return $.ajax({
                            url: that._options.searchUserUrl,
                            type: "post",
                            data: { query: query, customerId: $("#order_customerId").val(), searchKey: lastInitiatorSearchKey },
                            dataType: "json",
                            success: function (result) {
                                if (result.searchKey != lastInitiatorSearchKey)
                                    return;
                                var resultList = jQuery.map(result.result, function (item) {
                                    var aItem = {
                                        id: item.Id
                                                , num: item.UserId
                                                , name: item.FirstName + " " + item.SurName
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
                                                , name_family: item.SurName + " " + item.FirstName
                                                , customername: item.CustomerName
                                                , costcentre: item.CostCentre

                                    };
                                    return JSON.stringify(aItem);

                                });

                                return process(resultList);
                            }
                        });
                    },

                    matcher: function (obj) {
                        var item = JSON.parse(obj);
                        //console.log(JSON.stringify(item));
                        return ~item.name.toLowerCase().indexOf(this.query.toLowerCase())
                            || ~item.name_family.toLowerCase().indexOf(this.query.toLowerCase())
                            || ~item.num.toLowerCase().indexOf(this.query.toLowerCase())
                            || ~item.phone.toLowerCase().indexOf(this.query.toLowerCase())
                            || ~item.email.toLowerCase().indexOf(this.query.toLowerCase())
                            || ~item.usercode.toLowerCase().indexOf(this.query.toLowerCase());
                    },

                    sorter: function (items) {
                        var beginswith = [], caseSensitive = [], caseInsensitive = [];
                        while (items.length > 0) {
                            var aItem = items.shift();
                            var item = JSON.parse(aItem);
                            if (!item.num.toLowerCase().indexOf(this.query.toLowerCase())) beginswith.push(JSON.stringify(item));
                            else if (~item.num.indexOf(this.query)) caseSensitive.push(JSON.stringify(item));
                            else caseInsensitive.push(JSON.stringify(item));
                        }

                        return beginswith.concat(caseSensitive, caseInsensitive);
                    },

                    highlighter: function (obj) {
                        var item = JSON.parse(obj);
                        var orgQuery = this.query;
                        if (item.departmentname == null)
                            item.departmentname = ""
                        var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, "\\$&");
                        var result = item.name + " - " + item.num + " - " + item.phone + " - " + item.email + " - " + item.departmentname + " - " + item.usercode;
                        var resultBy_NameFamily = item.name_family + " - " + item.num + " - " + item.phone + " - " + item.email + " - " + item.departmentname + " - " + item.usercode;

                        if (result.toLowerCase().indexOf(orgQuery.toLowerCase()) > -1)
                            return result.replace(new RegExp("(" + query + ")", "ig"), function ($1, match) {
                                return "<strong>" + match + "</strong>";
                            });
                        else
                            return resultBy_NameFamily.replace(new RegExp("(" + query + ")", "ig"), function ($1, match) {
                                return "<strong>" + match + "</strong>";
                            });

                    },

                    updater: updateCallback
                };

                return options;
            }

            $("#orderer_OrdererId").typeahead(getOrderComputerUserSearchOptions(function (obj) {
                var item = JSON.parse(obj);

                $("#orderer_OrdererId").val(item.num);
                $("#order_OrdererName").val(item.name);
                $("#order_OrdererLocation").val(item.place);
                $("#order_OrdererEmail").val(item.email);
                $("#order_OrdererPhone").val(item.phone);
                $("#order_OrdererCode").val(item.usercode);
                $("#orderer_departmentId").val(item.departmentid);
                $("#orderer_departmentId").trigger("change");

                return item.num;
            }));
            $("#order_ReceiverId").typeahead(getOrderComputerUserSearchOptions(function (obj) {
                var item = JSON.parse(obj);
                $("#order_ReceiverId").val(item.num);
                $("#order_ReceiverName").val(item.name);
                $("#order_ReceiverLocation").val(item.place);
                $("#order_ReceiverEmail").val(item.email);
                $("#order_ReceiverPhone").val(item.phone);
                $("#order_ReceiverLocation").val(item.place);

                return item.num;
            }));
    
            function generateRandomKey() {
                function s4() {
                    return Math.floor((1 + Math.random()) * 0x10000)
                      .toString(16)
                      .substring(1);
                }
                return s4() + "-" + s4() + "-" + s4();
            }

        
            $.validator.setDefaults({
                ignore: ""
            });
            ////////////////////////////////////////////////////////////////////////
            
            function applyUserDepartmentFilter(orderId) {
                var data = {
                    id: orderId
                };
                return $.get(that._options.searchDepartmentsByRegionIdUrl,
                    data,
                    function (json) {
                        var $ordererDep = $("#orderer_departmentId");
                        $ordererDep.empty();
                        $ordererDep.prepend("<option></option>");
                        for (var i = 0; i < json.length; i++) {
                            var e = json[i];
                            $("<option>").text(e.Name).val(e.Value).appendTo($ordererDep);
                        }
                        $ordererDep.trigger("change");
                    });
            };

            function applyOrdererUnitsFilter(depId) {
                if (!that._options.units) return;
                var units = that._options.units ;

                var unitSelect = $("#orderer_unit_select");
                unitSelect.empty();
                var availableUnits = $.grep(units,
                    function (unit) {
                        return (unit.DependentId === depId || depId === "");
                    });
                unitSelect.prepend("<option value='' selected='selected'></option>");
                $.each(availableUnits,
                    function (index, unit) {
                        unitSelect.append($("<option/>",
                        {
                            value: unit.Value,
                            text: unit.Name
                        }));
                    });

            }

            $("#btnSave").on("click", function () {

                $("#informOrderer").val($("#informOrderer_action").prop("checked"));
                $("#informReceiver").val($("#informReceiver_action").prop("checked"));
                $("#createCase").val($("#createCase_action").prop("checked"));
                $("input[type='hidden'][multitext]").each(function (i, e) {
                    var fieldId = $(e).prop("id");
                    var allRows = $(".multitext[data-field-id='" + fieldId + "']");
                    var result = "";
                    allRows.each(function (index, el) {
                        var $el = $(el);
                        var id = $el.find("input[name$='id']").val();
                        var name = $el.find("input[name$='name']").val();
                        if (!id.length && !name.length) return;
                        result = result + id + that._options.valuesSplitter
                            + name + that._options.pairSplitter;
                    });
                    $(e).val(result);
                });

                $("#edit_form").submit();
                return false;
            });

            //if (that._options.isUserDepartmentVisible) {

            //    var $region = $("#region_dropdown");
            //    if ($region.val()) {
            //        var $department = $("#orderer_departmentId");
            //        var originVal = $department.val();

            //        applyUserDepartmentFilter($region.val())
            //        .done(function () {
            //            $department.val(originVal);
            //        });
            //    }


            //    $region.on("change", function () {
            //        applyUserDepartmentFilter($(this).val());
            //    });
            //}

            if (that._options.isOrdererUnitVisible) {

                var $ordererDep = $("#orderer_departmentId");
                if ($ordererDep.val()) {
                    var $unit = $("#orderer_unit_select");
                    var originUnitVal = $unit.val();

                    applyOrdererUnitsFilter($ordererDep.val());
                    $unit.val(originUnitVal);
                }

                $ordererDep.on("change", function() {
                    applyOrdererUnitsFilter($(this).val());
                });

            }

            var $status = $("#general_status:not(:hidden)");
            if ($status.length > 0) {

                var $informOrderer = $("#informOrderer_action");
                if($informOrderer.length > 0)
                {
                
                }

                var $informReceiver = $("#informReceiver_action");
                if ($informReceiver.length > 0) {

                }

                var $createCase = $("#createCase_action:not(:hidden)");
                if ($createCase.length > 0) {

                }

                $status.on("change", function () {
                    var val = $(this).val();
                    var item = that._options.statuses.filter(function(e) {
                        return e.Value === val;
                    });
                    if (item.length <= 0) return;

                    item = item[0];

                    var $informOrderer = $("#informOrderer_action");
                    if ($informOrderer.length > 0 && item.NotifyOrderer) {
                        $informOrderer.prop("checked", true);
                    }

                    var $informReceiver = $("#informReceiver_action");
                    if ($informReceiver.length > 0 && item.NotifyReceiver) {
                        $informReceiver.prop("checked", true);
                    }

                    var $createCase = $("#createCase_action:not(:hidden)");
                    if ($createCase.length > 0 && item.NotifyReceiver) {
                        $createCase.prop("checked", true);
                    }

                });
            }

            $("#case-url").off("click").on("click", function (e) {
                e.preventDefault();

                var href = $(this).attr("href");
                document.location.href = href;
            });

            function addMultiTextField(e) {
                var maxFields = 10;
                var $row = $(this).closest(".multitext");
                var fieldId = $row.data("fieldId");
                var allRows = $(".multitext[data-field-id='" + fieldId + "']");
                if (allRows.length >= maxFields) {
                    return;
                }

                var $rowClone = $row.clone();
                $rowClone.find("div:eq(2)").remove(); //remove helpcolumn
                var $templateHtml = $($rowClone.wrapAll("<div/>").parent().html());
                var $id = $templateHtml.find("input[name$='id']");
                var $name = $templateHtml.find("input[name$='name']");
                $templateHtml.find(".number").html(allRows.length + 1);
                $id.val("");
                $name.val("");
                $templateHtml.find("i[name='add']").on("click", addMultiTextField);
                $templateHtml.find("i[name='remove']").on("click", removeMultiTextField);
                allRows.last().after($templateHtml);

                $id.typeahead(getOrderComputerUserSearchOptions(function (obj) {
                    var item = JSON.parse(obj);

                    $name.val(item.name);
                    return item.num;
                }));
            }

            function removeMultiTextField(e) {
                var minFields = 1;
                var $row = $(this).closest(".multitext");
                var fieldId = $row.data("fieldId");
                var allRows = $(".multitext[data-field-id='" + fieldId + "']");
                if (allRows.length <= minFields) {
                    return;
                }

                var helpText = allRows.first().find("div:eq(2)");
                $row.remove();
                allRows = $(".multitext[data-field-id='" + fieldId + "']");
                //insert helptext if deleted
                var $firstRow = allRows.first();
                if ($firstRow.find("div").length < 3) {
                    $firstRow.append(helpText);
                }
                //reindex names
                allRows.find(".number").each(function (i, e) {
                    $(e).html(i + 1);
                });
            }

            var $ids = $(".multitext input[name$='id']");

            $ids.each(function (i, el) {
                var $id = $(el);
                $id.typeahead(getOrderComputerUserSearchOptions(function (obj) {
                    var item = JSON.parse(obj);
                    var $row = $id.closest(".multitext");
                    $row.find("input[name$='name']").val(item.name);
                    return item.num;
                }));
            });
            
            $(".multitext i[name='add']")
                .on("click", addMultiTextField);
            $(".multitext i[name='remove']")
                .on("click", removeMultiTextField);
        }
    }
})(jQuery);
