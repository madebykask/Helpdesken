function applyOrderBehavior(parameters) {
    if (!parameters.id) throw new Error('id must be specified.');
    if (!parameters.deleteOrderUrl) throw new Error('deleteOrderUrl must be specified.');
    if (!parameters.uploadFileUrl) throw new Error('uploadFileUrl must be specified.');
    if (!parameters.deleteFileUrl) throw new Error('deleteFileUrl must be specified.');
    if (!parameters.deleteLogUrl) throw new Error('deleteLogUrl must be specified.');
    if (!parameters.logSubtopic) throw new Error('logSubtopic must be specified.');
    if (!parameters.fileNameSubtopic) throw new Error('fileNameSubtopic must be specified.');

    var lastInitiatorSearchKey = '';
    var lastRecieverSearchKey = '';

    $('#fileName_files_uploader').pluploadQueue({
        url: parameters.uploadFileUrl,
        multipart_params: { entityId: parameters.id, subtopic: parameters.fileNameSubtopic },
        max_file_size: '10mb',

        init: {
            FileUploaded: function (uploader, uploadedFile, responseContent) {
                $('#filename_files_container').html(responseContent.response);
            }
        }
    });

    $('#log_send_to_button').button().click(function () {
        $('#log_send_to_dialog').dialog('open');
    });

    window.deleteFile = function (subtopic, fileName, filesContainerId) {
        $.post(parameters.deleteFileUrl, { entityId: parameters.id, subtopic: subtopic, fileName: fileName }, function (markup) {
            $('#' + filesContainerId).html(markup);
        });
    };

    window.deleteLog = function (subtopic, logId, logsContainerId) {
        $.post(parameters.deleteLogUrl, { orderId: parameters.id, subtopic: subtopic, logId: logId }, function (markup) {
            $('#' + logsContainerId).html(markup);
        });
    };

    window.fillEmailsTextArea = function (textAreaId, emails) {
        var emailsText = '';

        for (var i = 0; i < emails.length; i++) {
            if (i != 0) {
                emailsText += '\n';
            }

            emailsText += emails[i];
        }

        $('#' + textAreaId).val(emailsText);
    };

    window.fillLogSendLogNoteToEmailsTextArea = function (emails) {
        this.fillEmailsTextArea('log_send_to_emails_textarea', emails);
    };

    function getOrderComputerUserSearchOptionsForOrderer() {

        var options = {
            items: 20,
            minLength: 2,

            source: function (query, process) {
                lastInitiatorSearchKey = generateRandomKey();
                return $.ajax({
                    url: '/cases/search_user',
                    type: 'post',
                    data: { query: query, customerId: $('#order_customerId').val(), searchKey: lastInitiatorSearchKey },
                    dataType: 'json',
                    success: function (result) {
                        if (result.searchKey != lastInitiatorSearchKey)
                            return;
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
                var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
                while (aItem = items.shift()) {
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
                var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
                var result = item.name + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email + ' - ' + item.departmentname + ' - ' + item.usercode;
                var resultBy_NameFamily = item.name_family + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email + ' - ' + item.departmentname + ' - ' + item.usercode;

                if (result.toLowerCase().indexOf(orgQuery.toLowerCase()) > -1)
                    return result.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                        return '<strong>' + match + '</strong>';
                    });
                else
                    return resultBy_NameFamily.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                        return '<strong>' + match + '</strong>';
                    });

            },

            updater: function (obj) {
                var item = JSON.parse(obj);
              
                    $('#Orderer_OrdererId_Value').val(item.num);
                    $('#order_OrdererName').val(item.name);
                    $('#order_OrdererLocation').val(item.place);
                    $('#order_OrdererEmail').val(item.email);
                    $('#order_OrdererPhone').val(item.phone);
                    $('#order_OrdererCode').val(item.usercode);
                    $('#orderer_departmentId').val(item.departmentid);
              
                return item.num;
            }
        };

        return options;
    }

    function getOrderComputerUserSearchOptionsForReciever() {

        var options = {
            items: 20,
            minLength: 2,

            source: function (query, process) {
                lastRecieverSearchKey = generateRandomKey();
                return $.ajax({
                    url: '/cases/search_user',
                    type: 'post',
                    data: { query: query, customerId: $('#order_customerId').val(), searchKey: lastRecieverSearchKey },
                    dataType: 'json',
                    success: function (result) {
                        if (result.searchKey != lastRecieverSearchKey)
                            return;
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
                var beginswith = [], caseSensitive = [], caseInsensitive = [], item;
                while (aItem = items.shift()) {
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
                var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
                var result = item.name + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email + ' - ' + item.departmentname + ' - ' + item.usercode;
                var resultBy_NameFamily = item.name_family + ' - ' + item.num + ' - ' + item.phone + ' - ' + item.email + ' - ' + item.departmentname + ' - ' + item.usercode;

                if (result.toLowerCase().indexOf(orgQuery.toLowerCase()) > -1)
                    return result.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                        return '<strong>' + match + '</strong>';
                    });
                else
                    return resultBy_NameFamily.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                        return '<strong>' + match + '</strong>';
                    });

            },

            updater: function (obj) {
                var item = JSON.parse(obj);
                $('#Receiver_ReceiverId_Value').val(item.num);
                $('#order_ReceiverName').val(item.name);
                $('#order_ReceiverLocation').val(item.place);
                $('#order_ReceiverEmail').val(item.email);
                $('#order_ReceiverPhone').val(item.phone);
                $('#order_ReceiverLocation').val(item.place);

                return item.num;
            }
        };

        return options;
    }

    $('#Orderer_OrdererId_Value').typeahead(getOrderComputerUserSearchOptionsForOrderer());
    $('#Receiver_ReceiverId_Value').typeahead(getOrderComputerUserSearchOptionsForReciever());
    
    function generateRandomKey() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
              .toString(16)
              .substring(1);
        }
        return s4() + '-' + s4() + '-' + s4();
    }

}


(function ($) {
    $.validator.setDefaults({
        ignore: ""
    });
}(jQuery));