function InvoicesOverview(options) {
    "use strict";

    this.options = $.extend({}, this.getDefaults(), options);
    this.init();

}

InvoicesOverview.prototype = {
    init: function () {
        "use strict";
        var self = this;

        self.$totalRows = $("#totalRows");
        self._initGrid();
        $("#invoiceFilter #btnSearch").on("click", function() {
            self.table.ajax.reload();
        });
        $("#btnInvoiceAction").on("click", function () {
            self._invoiceAction();
        });
        $("#btnExportGrid").on("click", function () {
            self._exportGrid();
        });
    },

    getDefaults: function () {
        "use strict";

        return {
           
        };
    },

    _initGrid: function () {
        "use strict";
        var self = this;

        self.table = InitDataTable("invoiceGrid", self.options.perPageText, self.options.perShowingText,
            {
                "sDom": "<'row-fluid'r>t",
                processing: true,
                serverSide: false,
                ordering: true,
                ajax: {
                    url: self.options.getListUrl,
                    type: "GET",
                    data: function (data) {
                        var params = self._getInvoicesGridParams();
                        //params.push({ name: "start", value: data.start });
                        //params.push({ name: "length", value: data.length });
                        //params.push({ name: "order", value: data.order.length === 1 ? data.columns[data.order[0].column].data : "" });
                        //params.push({ name: "dir", value: data.order.length === 1 ? data.order[0].dir : "" });
                        return params;
                    },
                   dataSrc: ""
                },
                createdRow: function (row, data, dataIndex) {
                    if (data) {
                        //        $(row).addClass(self.getClsRow(data) + " caseid=" + data.case_id);
                        //        row.cells[0].innerHTML = strJoin('<a href="/Cases/Edit/', data.case_id, '"><img title="', data.caseIconTitle, '" alt="', data.caseIconTitle, '" src="', data.caseIconUrl, '" /></a>');
                        $(row).attr("data-rowid", data.CaseId);
                        $(row).on("click", "td.expand-col", function () {
                            var tr = $(this).closest("tr");
                            self._toggleChild(tr);
                        });
                        $(row).on("click", "td.infoShower", function () {
                            showCaseInfo(data.CaseId, data.CaseNumber);
                        });
                    }
                },
                rowId: "CaseId",
                columns: [                
                    {
                        "className": "expand-col",
                        "sortable": false,
                        "defaultContent": "<i class='icon-plus-sign' style='cursor: pointer;'></i>"
                    },
                    {
                        "className": "infoShower",
                        "sortable": false,
                        "defaultContent": "<i class='icon-list-alt icon-4x ' style='cursor: pointer;' </i> "                         
                    },
                    {
                       "data": "CaseNumber"                    
                    },
                    { "data": "Caption" },
                    { "data": "Category" },
                    {
                        "data": "FinishingDate",
                        "render": function (data, type, row) {
                            return self._formatDate(data);
                        }
                    },
                    { "data": "Department" },
                    {
                      "data": "",
                      "render": function ( data, type, row ) {
                          return self._minutesToTimeString(self._getWorkingTime(row));
                      } 
                    },
                    {
                        "data": "",
                        "render": function (data, type, row) {
                            return self._minutesToTimeString(self._getOvertime(row));
                        }
                    },
                    {
                        "data": "",
                        "className": "align-right nowrap",
                        "render": function (data, type, row) {
                            return self._formatNumber(self._getAmount(row), 2, 3, " ", ",");
                        }
                    },
                    {
                        "data": "",
                        "className": "align-right nowrap",
                        "render": function (data, type, row) {
                            return self._formatNumber(self._getMaterial(row), 2, 3, " ", ",");
                        }
                    },
                    {
                        "data": "",
                        "className": "align-right nowrap",
                        "render": function (data, type, row) {
                            return self._formatNumber(self._getPrice(row), 2, 3, " ", ",");
                        }
                    },
                    {
                        "data": "",
                        "className": "align-right nowrap",
                        "render": function (data, type, row) {
                            return self._formatNumber(self._getExternalInvoice(row), 2, 3, " ", ",");
                        }
                    },
                    {
                        "data": "",
                        "className": "align-center",
                        "render": function (data, type, row) {
                            return !self._isSectionReadOnly(row) ? "<input type='button' id='btnInvoiceChargedChildren' value='" + self.options.copyText + "'/input>" : "";
                        },
                        "sortable": false
                    },
                    //{ "defaultContent": "", "sortable": false },
                    {
                        "data": "",
                        "className": "align-center invoiceCol",
                        "render": function (data, type, row) {
                            return !self._isSectionReadOnly(row) ? "<input type='checkbox' id='cbInvoiceChildren' data-accessId='invoiceParentCol-" + row.CaseId + "'>" : "";
                        },
                        "sortable": false
                    },
                    {
                        "data": "",
                        "className": "align-center notInvoiceCol",
                        "render": function (data, type, row) {
                            return !self._isSectionReadOnly(row) ? "<input type='checkbox' id='cbNotInvoiceChildren' data-accessId='notInvoiceParentCol-" + row.CaseId + "'>" : "";
                        },
                        "sortable": false
                    }
                ],               
                order: [[2, "asc"]],
                "bPaginate": false,
                //"bAutoWidth": false,
                //"lengthMenu": [appSettings.gridSettings.pageSizeList, appSettings.gridSettings.pageSizeList],
                //"iDisplayLength": appSettings.gridSettings.pageOptions.recPerPage,
                //"displayStart": appSettings.gridSettings.pageOptions.recPerPage * appSettings.gridSettings.pageOptions.pageIndex
            }, function (e, settings, techNote, message) {
                console.log("An error has been reported by DataTable: ", message);
            });

        self.table.on("xhr.dt",
            function(e, settings, json) {

                if (!json) {
                    console.log("An empty result");
                }

                self.$totalRows.text(json.length);
            });

        $('#invoiceGrid tbody').on('click', 'tr.expanded-toolbar #btnCancel', function (e) {
            self._cancelExpansionForChild($(this).closest("tr"));
            return false;
        });

        $('#invoiceGrid tbody').on('click', 'tr.expanded-toolbar #btnSave', function (e) {
            self._saveRow($(this).closest("tr"));
            return false;
        });

        $('#invoiceGrid tbody').on('change', 'tr.expanded-row #cbInvoice', function (e) {
            self._selectAction($(this).closest("tr"), $(this).is(":checked") ? 2 : 0);
            return false;
        });

        $('#invoiceGrid tbody').on('change', 'tr.expanded-row #cbNotInvoice', function (e) {
            self._selectAction($(this).closest("tr"), $(this).is(":checked") ? 3 : 0);
            return false;
        });

        $('#invoiceGrid tbody').on('change', 'tr #cbInvoiceChildren', function (e) {
            self._selectChildActions($(this).closest("tr"), $(this).is(":checked") ? 2 : 0);
            return false;
        });

        $('#invoiceGrid tbody').on('change', 'tr #cbNotInvoiceChildren', function (e) {
            self._selectChildActions($(this).closest("tr"), $(this).is(":checked") ? 3 : 0);
            return false;
        });

        $('#invoiceGrid tbody').on('click', 'tr #btnInvoiceChargedChildren', function (e) {
            self._selectChildActionsByCharge($(this).closest("tr"));
            return false;
        });      
        
        function showCaseInfo(_caseId, _caseNumber) {
            $.get(self.options.showCaseInfoUrl,
              {
                  caseId: _caseId,
                  caseNumber: _caseNumber,
                  popupShow: true,
                  showPrintButton: false,
                  curTime: Date.now()
              },

              function (_reportPresentation) {
                  $('#CasePrintArea').html(_reportPresentation);

                  $('#PrintCaseDialog').draggable({
                      handle: ".modal-header"
                  });
              
                  $('#PrintCaseDialog').modal({
                      "backdrop": "static",
                      "keyboard": true,
                      "show": true
                  });            
              });
        }       

    },

    _getAmount: function (data) {
        "use strict";
        var self = this;

        var sum = 0;
        for (var i = 0; i < data.LogInvoices.length; i++) {
            sum += self._getLogInvoiceAmount(data.LogInvoices[i]);
        }
        return sum;
    },

    _getPrice: function (data) {
        var sum = 0;
        for (var i = 0; i < data.LogInvoices.length; i++) {
            sum += data.LogInvoices[i].Price;
        }
        return sum;
    },

    _getMaterial: function (data) {
        var sum = 0;
        for (var i = 0; i < data.LogInvoices.length; i++) {
            sum += data.LogInvoices[i].Material;
        }
        return sum;
    },

    _getWorkingTime: function (data) {
        var sum = 0;
        for (var i = 0; i < data.LogInvoices.length; i++) {
            sum += data.LogInvoices[i].WorkingTime;
        }
        return sum;
    },

    _getOvertime: function (data) {
        var sum = 0;
        for (var i = 0; i < data.LogInvoices.length; i++) {
            sum += data.LogInvoices[i].Overtime;
        }
        return sum;
    },

    _getExternalInvoice: function (data) {
        var sum = 0;
        for (var i = 0; i < data.ExternalInvoices.length; i++) {
            sum += data.ExternalInvoices[i].Amount;
        }
        return sum;
    },

    _getLogInvoiceAmount: function (data) {
        return (data.WorkingHourRate * data.WorkingTime / 60) + (data.OvertimeHourRate * data.Overtime / 60);
    },

    _getInvoicesGridParams: function() {
        //return [
        //    { DepartmentId: $("#invoiceFilter #ddlDepartment").val() },
        //    { DateFrom: $("#invoiceFilter #dateFrom").val() },
        //    { DateTo: $("#invoiceFilter #dateTo").val() }, 
        //    { Status: $("#invoiceFilter #ddlStatus").val()}
        //];
        return {
            DepartmentId: $("#invoiceFilter #ddlDepartment").val(),
            DateFrom: $("#invoiceFilter #dateFrom input").val(),
            DateTo: $("#invoiceFilter #dateTo input").val(),
            Status: $("#invoiceFilter #ddlStatus").val(),
            DepartmentCharge: true
        }
    },

    _minutesToTimeString: function(mins) {
        "use string";
        var self = this;

        var time = self._minutesToTime(mins);
        return time.hours + self.options.hourText + " " + time.minutes + self.options.minText;
    },

    _minutesToTime: function(mins) {
        var time = {};
        time.hours = Math.floor(mins / 60);
        time.minutes = mins % 60;
        return time;
    },

    _formatDate: function (date) {
        if (!date)
            return "";
        var dt = new Date(date);
        var year = dt.getFullYear();
        var month = (1 + dt.getMonth()).toString();
        month = month.length > 1 ? month : "0" + month;
        var day = dt.getDate().toString();
        day = day.length > 1 ? day : "0" + day;
        return year + "-" + month + "-" + day;
    },

    _formatNumber: function(v, n, x, s, c) {
        var re = "\\d(?=(\\d{" + (x || 3) + "})+" + (n > 0 ? "\\D" : "$") + ")",
            num = v.toFixed(Math.max(0, ~~n));

        return (c ? num.replace(".", c) : num).replace(new RegExp(re, "g"), "$&" + (s || ","));
    },

    _getStatusText: function (status) {
        "use strict";
        var self = this;

        return status && self.options.statusList[status] ? self.options.statusList[status] : "";
    },

    _getRowByChild: function (tr) {
        "use strict";
        var self = this;

        return self.table.row("[data-rowid='" + $(tr).data("parentrowid") + "']");
    },

    _findById: function(entries, id) {
        var resultEntries = jQuery.grep(entries, function (a) {
            return a.Id === id;
        });
        if (resultEntries.length > 0) {
            return resultEntries[0];
        }

        return null;
    },

    _toggleChild: function(tr) {
        "use strict";
        var self = this;

        var currentRow = self.table.row(tr);
        

        if (currentRow.child.isShown()) {
            currentRow.child.hide();
        }
        else {
            //self.table.rows().indexes().each(function (value, index) {
            //    if (self.table.row(index).child.isShown())
            //        self.table.row(index).child.hide();
            //});
            currentRow.child(self._getExpansion(currentRow.data())).show();
        }

        self._setToggleIcon(currentRow);
    },

    _setToggleIcon: function(row) {
        var triggerTd = $("td:first", row.node());
        if (row.child.isShown()) {
            triggerTd.html("<i class='icon-minus-sign'></i>");
        }
        else {
            triggerTd.html("<i class='icon-plus-sign'></i>");
        }
    },
    
    _generatePriceFieldId: function (rowId, index){
        return "priceField_" + rowId + "_" + index;
    },

    _getExpansion: function (data) {
        "use strict";
        var self = this;

        var res = [];
        var isSectionReadOnly = self._isSectionReadOnly(data);
        for (var i = 0; i < data.LogInvoices.length; i++) {
            var isRowReadOnly = self._isInvoiceValueReadOnly(data.LogInvoices[i].InvoiceRow.Status);
            if (!isRowReadOnly) {
                isSectionReadOnly = false;
            }
            var priceFieldId = self._generatePriceFieldId(data.CaseId, i);
            var rowString =
            "<tr class='expanded-row' data-parentrowid='" + data.CaseId + "' data-loginvoice='" + data.LogInvoices[i].Id + "'>" +
            "<td></td>" +
            "<td></td>" +
		    "<td></td>" +
		    "<td>" + $("<span/>").text(data.LogInvoices[i].Text).html() + "</td>" +
		    "<td></td>" +
		    "<td></td>" +
		    "<td></td>" +
		    "<td>" + (isRowReadOnly ? self._minutesToTimeString(data.LogInvoices[i].WorkingTime) : self._getTimeEditor(data.LogInvoices[i].WorkingTime, "WorkingTime")) + "</td>" +
		    "<td>" + (isRowReadOnly ? self._minutesToTimeString(data.LogInvoices[i].Overtime) : self._getTimeEditor(data.LogInvoices[i].Overtime, "Overtime")) + "</td>" +
		    "<td id='"+ priceFieldId + "' class='align-right'>" + self._formatNumber(self._getLogInvoiceAmount(data.LogInvoices[i]), 2, 3, " ", ",") + "</td>" +
		    "<td class='align-right'>" + (isRowReadOnly ? self._formatNumber(data.LogInvoices[i].Material, 2, 3, " ", ",") : self._getTextBox(data.LogInvoices[i].Material, "txtMaterial")) + "</td>" +
		    "<td class='align-right'>" + (isRowReadOnly ? self._formatNumber(data.LogInvoices[i].Price, 2, 3, " ", ",") : self._getTextBox(data.LogInvoices[i].Price, "txtPrice")) + "</td>" +
		    "<td></td>" +
		    "<td class='align-center'>" + self._getChargeBox(true, data.LogInvoices[i].Charge) + "</td>" +
		    //"<td>" + self._getStatusText(data.LogInvoices[i].InvoiceRow.Status) + "</td>" +
		    "<td class='align-center'>" + self._getInvoiceActionBox(data.CaseId, isRowReadOnly, data.LogInvoices[i].InvoiceRow.Status, data.LogInvoices[i].InvoiceAction) + "</td>" +
		    "<td class='align-center'>" + self._getNotInvoiceActionBox(data.CaseId, isRowReadOnly, data.LogInvoices[i].InvoiceRow.Status, data.LogInvoices[i].InvoiceAction) + "</td>" +
            "</tr>";
            var row = $(rowString);
            res.push(row[0]);
        }

        for (var i = 0; i < data.ExternalInvoices.length; i++) {
            isRowReadOnly = self._isInvoiceValueReadOnly(data.ExternalInvoices[i].InvoiceRow.Status);
            if (!isRowReadOnly) {
                isSectionReadOnly = false;
            }
            rowString =
            "<tr class='expanded-row' data-parentrowid='" + data.CaseId + "' data-externalinvoice='" + data.ExternalInvoices[i].Id + "'>" +
            "<td></td>" +
		    "<td></td>" +
		    "<td></td>" +
		    "<td>" + $("<span/>").text(data.ExternalInvoices[i].Name).html() + "</td>" +
		    "<td></td>" +
		    "<td></td>" +
		    "<td></td>" +
		    "<td></td>" +
		    "<td></td>" +
		    "<td></td>" +
		    "<td></td>" +
            "<td></td>" +
		    "<td class='align-right'>" + (isRowReadOnly ? self._formatNumber(data.ExternalInvoices[i].Amount, 2, 3, " ", ",") : self._getTextBox(data.ExternalInvoices[i].Amount, "txtExternalAmount")) + "</td>" +
		    "<td class='align-center'>" + self._getChargeBox(true, data.ExternalInvoices[i].Charge) + "</td>" +
		    //"<td>" + self._getStatusText(data.ExternalInvoices[i].InvoiceRow.Status) + "</td>" +
		    "<td class='align-center'>" + self._getInvoiceActionBox(data.CaseId, isRowReadOnly, data.ExternalInvoices[i].InvoiceRow.Status, data.ExternalInvoices[i].InvoiceAction) + "</td>" +
		    "<td class='align-center'>" + self._getNotInvoiceActionBox(data.CaseId, isRowReadOnly, data.ExternalInvoices[i].InvoiceRow.Status, data.ExternalInvoices[i].InvoiceAction) + "</td>" +
            "</tr>";
            row = $(rowString);
            res.push(row[0]);
        }

        if (!isSectionReadOnly) {
            rowString =
            "<tr class='expanded-toolbar' data-parentrowid='" + data.CaseId + "'>" +
            "<td colspan=20 class='align-right'>" +
            "<div class='width100'>" +
            "<input class='btn btn small ' type=button value='" + self.options.saveText + "' id='btnSave'>" +
            "<input class='btn btn small ' type=button value='" + self.options.cancelText + "' id='btnCancel'>" +
            "</div>" +
            "</td>" +
            "</tr>";
            row = $(rowString);
            res.push(row[0]);


        }

        return res;
    },

    _getTimeEditor: function(mins, name) {
        "use strict";
        var self = this;

        var time = self._minutesToTime(mins);
        var hours = $("<select id='ddl" + name + "Hours' name='ddl" + name + "Hours' class='inputw55'></select>");
        for (var i = 0; i < 100; i++) {
            hours.append($('<option>', { 
                value: i,
                text: i,
                selected: time.hours === i
            }));
        }
        var minutes = $("<select id='ddl" + name + "Minutes' name='ddl" + name + "Minutes' class='inputw55'></select>");
        for (i = 0; i < 60; i+= self.options.minStep) {
            minutes.append($('<option>', {
                value: i,
                text: i,
                selected: time.minutes === i
            }));
        }

       var res = $("<span/>")
            .append($("<div class='nowrap'/>")
                .append($("<span/>")
                    .append(hours)
                    .append(" " + self.options.hourText))
                .append($("<span/>")
                    .append(" ")
                    .append(minutes)
                    .append(" " + self.options.minText))
            );

        return res.html();
    },

    _isInvoiceValueReadOnly: function(status) {
        return status && (status === 2 || status === 3);
    },

    _getTextBox: function(val, name) {
        var res = "<input type='text' class='inputw55' id='" + name + "' name='" + name + "' placeholder='0.00' value='{1}'>"
            .replace(/\{1\}/g, val);

        return res;
    },

    _getChargeBox: function (isRowReadOnly, val) {
        "use strict";
        var self = this;

        var res = "<input type='checkbox' id='cbCharge' name='cbCharge' {1} {2}>"
            .replace(/\{1\}/g, val ? "checked" : "")
            .replace(/\{2\}/g, isRowReadOnly ? "disabled" : "");

        return res;
    },

    _getInvoiceActionBox: function (groupId, isRowReadOnly, status, val) {
        "use strict";
        var self = this;

        var res = "<input type='checkbox' data-InvoiceGroupId='"+ groupId +"' id='cbInvoice' name='cbInvoice' {1} {2}>"
            .replace(/\{1\}/g, ((status && status === 2) || (val && val === 2)) ? "checked" : "")
            .replace(/\{2\}/g, isRowReadOnly ? "disabled" : "");

        return res;
    },

    _getNotInvoiceActionBox: function (groupId, isRowReadOnly, status, val) {
        "use strict";
        var self = this;

        var res = "<input type='checkbox'  data-NotInvoiceGroupId='" + groupId + "' id='cbNotInvoice' name='cbNotInvoice' {1} {2}>"
            .replace(/\{1\}/g, ((status && status === 3) || (val && val === 3)) ? "checked" : "")
            .replace(/\{2\}/g, isRowReadOnly ? "disabled" : "");

        return res;
    },

    _cancelExpansionForChild: function (tr) {
        "use strict";
        var self = this;

        self._toggleChild(self._getRowByChild(tr));
    },

    _saveRow: function (tr) {
        "use strict";
        var self = this;

        var externalInvoices = [];
        var logInvoices = [];
        var parent = self._getRowByChild(tr);
        var childRows = parent.child();

        for (var i = 0; i < childRows.length; i++) {
            var row = childRows[i];
            var logInvoiceId = $(row).data("loginvoice");
            if (logInvoiceId) {
                var dataEntry = self._findById(parent.data().LogInvoices, logInvoiceId);
                if (!dataEntry || self._isInvoiceValueReadOnly(dataEntry.InvoiceRow.Status))
                    continue;
                logInvoices.push({
                    Id: logInvoiceId,
                    WorkingTime: Number($("#ddlWorkingTimeHours", row).val()) * 60 + Number($("#ddlWorkingTimeMinutes", row).val()),
                    Overtime: Number($("#ddlOvertimeHours", row).val()) * 60 + Number($("#ddlOvertimeMinutes", row).val()),
                    Material: Number($("#txtMaterial", row).val()),
                    Price: Number($("#txtPrice", row).val()),
                    Charge: $("#cbCharge", row).is(':checked')
                });
            }

            var externalInvoiceId = $(row).data("externalinvoice");
            if (externalInvoiceId) {
                dataEntry = self._findById(parent.data().ExternalInvoices, externalInvoiceId);
                if (!dataEntry || self._isInvoiceValueReadOnly(dataEntry.InvoiceRow.Status))
                    continue;
                externalInvoices.push({
                    Id: externalInvoiceId,
                    Amount: Number($("#txtExternalAmount", row).val()),
                    Charge: $("#cbCharge", row).is(':checked')
                });
            }
        }

        $("#btnSave, #btnCancel", childRows).prop('disabled', true);

        $.postAntiForgery({
            url: self.options.saveValuesUrl,
            data: {
                LogInvoices: logInvoices,
                ExternalInvoices: externalInvoices
            }
        })
        .done(function (responseData) {
            if (!responseData) {
                throw new Error('saverow error');
            }

            //refresh
            var data = parent.data();
            for (var i = 0; i < logInvoices.length; i++) {
                var dataEntry = self._findById(data.LogInvoices, logInvoices[i].Id);
                if (dataEntry) {
                    dataEntry.WorkingTime = logInvoices[i].WorkingTime;
                    dataEntry.Overtime = logInvoices[i].Overtime;
                    dataEntry.Material = logInvoices[i].Material;
                    dataEntry.Price = logInvoices[i].Price;
                    dataEntry.Charge = logInvoices[i].Charge;
                }
                var priceFieldId = self._generatePriceFieldId(data.CaseId, i);
                $('#' + priceFieldId).html(self._formatNumber(self._getLogInvoiceAmount(dataEntry), 2, 3, " ", ","));
            }
            for (i = 0; i < externalInvoices.length; i++) {
                dataEntry = self._findById(data.ExternalInvoices, externalInvoices[i].Id);
                if (dataEntry) {
                    dataEntry.Amount = externalInvoices[i].Amount;
                    dataEntry.Charge = externalInvoices[i].Charge;
                }
            }
            parent.invalidate();          
            self.table.draw();
            self._setToggleIcon(parent);
        })
        .always(function () {
            //spinner
            $("#btnSave, #btnCancel", childRows).prop('disabled', false);
        });

    },

    _invoiceAction: function () {
        "use strict";
        var self = this;

        var externalInvoices = [];
        var logInvoices = [];
        var data = self.table.data();

        for (var i = 0; i < data.length; i++) {
            var row = data[i];
            for (var j = 0; j < row.LogInvoices.length; j++) {
                if (!self._isInvoiceValueReadOnly(row.LogInvoices[j])) {
                    if (row.LogInvoices[j].InvoiceAction === 2 || row.LogInvoices[j].InvoiceAction === 3) {
                        logInvoices.push({
                            Id: row.LogInvoices[j].Id,
                            Status: row.LogInvoices[j].InvoiceAction
                        });
                    }
                }
            }

            for (j = 0; j < row.ExternalInvoices.length; j++) {
                if (!self._isInvoiceValueReadOnly(row.ExternalInvoices[j])) {
                    if (row.ExternalInvoices[j].InvoiceAction === 2 || row.ExternalInvoices[j].InvoiceAction === 3) {
                        externalInvoices.push({
                            Id: row.ExternalInvoices[j].Id,
                            Status: row.ExternalInvoices[j].InvoiceAction
                        });
                    }
                }
            }
        }

        if (logInvoices.length || externalInvoices.length) {
            $("#btnInvoiceAction").prop('disabled', true);

            $.postAntiForgery({
                url: self.options.actionUrl,
                data: {
                    LogInvoices: logInvoices,
                    ExternalInvoices: externalInvoices
                }
            })
           .done(function (responseData) {
               if (!responseData) {
                   throw new Error('invoiceaction error');
               }
               
               //refresh
               self.table.ajax.reload();
           })
           .always(function () {
               //spinner
               $("#btnInvoiceAction").prop('disabled', false);
           });
        }
    },

    _exportGrid: function() {
        "use strict";
        var self = this;

        var params = self._getInvoicesGridParams();
        window.open(self.options.exportUrl + "?" + $.param(params));
    },

    _selectActionData: function (parent, id, collection, action) {
        "use strict";
        var self = this;

        var dataEntry = self._findById(collection, id);

        if (dataEntry) {
            dataEntry.InvoiceAction = action;
        }
    },

    _selectActionUi: function (row, action) {
        "use strict";
        var self = this;
                
        switch (action) {
            case 2:
                $("#cbInvoice", row).attr("checked", "checked");                
                $("#cbNotInvoice", row).removeAttr("checked");
                break;
            case 3:
                $("#cbInvoice", row).removeAttr("checked");
                $("#cbNotInvoice", row).attr("checked", "checked");
                break;
            default:
                $("#cbInvoice", row).removeAttr("checked");
                $("#cbNotInvoice", row).removeAttr("checked");
        }
    },

    _selectAction: function (row, action) {
        "use strict";
        var self = this;

        var parent = self._getRowByChild(row);

        var logInvoiceId = $(row).data("loginvoice");
        if (logInvoiceId) {
            self._selectActionData(parent, logInvoiceId, parent.data().LogInvoices, action);
        }

        var externalInvoiceId = $(row).data("externalinvoice");
        if (externalInvoiceId) {
            self._selectActionData(parent, externalInvoiceId, parent.data().ExternalInvoices, action);
        }

        self._selectActionUi(row, action);

        var data = parent.data();
        self._setOverallCheckBoxes(data.CaseId);        

    },

    _selectChildActions: function (tr, action) {
        "use strict";
        var self = this;

        var parent = self.table.row(tr);
        var data = parent.data();

        for (var i = 0; i < data.LogInvoices.length; i++) {
            if (!self._isInvoiceValueReadOnly(data.LogInvoices[i].InvoiceRow.Status))
                self._selectActionData(parent, data.LogInvoices[i].Id, data.LogInvoices, action);
        }
        for (i = 0; i < data.ExternalInvoices.length; i++) {
            if (!self._isInvoiceValueReadOnly(data.ExternalInvoices[i].InvoiceRow.Status))
                self._selectActionData(parent, data.ExternalInvoices[i].Id, data.ExternalInvoices, action);
        }

        if (parent.child.isShown()) {
            var childRows = parent.child();
            for (var i = 0; i < childRows.length; i++) {
                self._selectActionUi(childRows[i], action);
            }
            self._setOverallCheckBoxes(data.CaseId);
        } else {
                        
            if (action == 2)
            {
                var parentNotInvoiceCheckbox = $(document).find("[data-accessId='notInvoiceParentCol-" + data.CaseId + "']")[0];
                parentNotInvoiceCheckbox.indeterminate = false;
                $(parentNotInvoiceCheckbox).removeAttr("checked");                
            }

            if (action == 3) {
                var parentInvoiceCheckbox = $(document).find("[data-accessId='invoiceParentCol-" + data.CaseId + "']")[0];
                parentInvoiceCheckbox.indeterminate = false;                
                $(parentInvoiceCheckbox).removeAttr("checked");
            }
        }

        
    },

    _selectChildActionsByCharge: function (tr) {
        "use strict";
        var self = this;

        var parent = self.table.row(tr);
        var data = parent.data();
        var overallInvoice = false;
        var overallNoInvoice = false;
        for (var i = 0; i < data.LogInvoices.length; i++) {
            if (!self._isInvoiceValueReadOnly(data.LogInvoices[i].InvoiceRow.Status)) {
                var action = data.LogInvoices[i].Charge ? 2 : 3;
                self._selectActionData(parent, data.LogInvoices[i].Id, data.LogInvoices, action);
                if (parent.child.isShown()) {
                    var childRow = self._getTrForLogInvoice(data.LogInvoices[i].Id);
                    self._selectActionUi(childRow, action);                    
                }
                if (action == 2) {
                    overallInvoice = true;
                }
                if (action == 3) {
                    overallNoInvoice = true;
                }
            }            
        }

        for (i = 0; i < data.ExternalInvoices.length; i++) {
            if (!self._isInvoiceValueReadOnly(data.ExternalInvoices[i].InvoiceRow.Status)) {
                var action = data.ExternalInvoices[i].Charge ? 2 : 3;
                self._selectActionData(parent, data.ExternalInvoices[i].Id, data.ExternalInvoices, action);
                if (parent.child.isShown()) {
                    var childRow = self._getTrForExternalInvoice(data.ExternalInvoices[i].Id);
                    self._selectActionUi(childRow, action);
                }
                if (action == 2) {
                    overallInvoice = true;
                }
                if (action == 3) {
                    overallNoInvoice = true;
                }
            }
        }

        self._setOverallCheckBoxes(data.CaseId, overallInvoice, overallNoInvoice);
        
    },

    _setOverallCheckBoxes: function (groupId, _overallInvoice, _overallNoInvoice) {
        "use strict";
        var self = this;
       
        var overallInvoice = false;
        var overallNoInvoice = false;

        if (_overallInvoice == undefined) {
            $(document).find("[data-InvoiceGroupId='" + groupId + "']").each(function () {
                if ($(this).is(":checked")) {
                    overallInvoice = true;
                    return;
                }
            });
        } else {
            overallInvoice = _overallInvoice;
        }
        if (_overallNoInvoice == undefined) {
            $(document).find("[data-NotInvoiceGroupId='" + groupId + "']").each(function () {
                if ($(this).is(":checked")) {
                    overallNoInvoice = true;
                    return;
                }
            });
        } else {
            overallNoInvoice = _overallNoInvoice;
        }

        var parentInvoiceCheckbox = $(document).find("[data-accessId='invoiceParentCol-" + groupId + "']")[0];
        var parentNotInvoiceCheckbox = $(document).find("[data-accessId='notInvoiceParentCol-" + groupId + "']")[0];
        if (parentInvoiceCheckbox != undefined && parentNotInvoiceCheckbox != undefined) {
            parentInvoiceCheckbox.indeterminate = false;
            parentNotInvoiceCheckbox.indeterminate = false;
            $(parentNotInvoiceCheckbox).removeAttr("checked");
            $(parentInvoiceCheckbox).removeAttr("checked");
            if (overallInvoice && overallNoInvoice) {
                parentInvoiceCheckbox.indeterminate = true;
                parentNotInvoiceCheckbox.indeterminate = true;
            }
            else if (overallInvoice) {
                $(parentInvoiceCheckbox).attr("checked", "checked");
                $(parentNotInvoiceCheckbox).removeAttr("checked");
            }
            else if (overallNoInvoice) {
                $(parentNotInvoiceCheckbox).attr("checked", "checked");
                $(parentInvoiceCheckbox).removeAttr("checked");
            }
        }
    },

    _getTrForLogInvoice: function(id) {
        return $("tr[data-loginvoice='" + id + "']");
    },

    _getTrForExternalInvoice: function(id) {
        return $("tr[data-externalinvoice='" + id + "']");
    },

    _isSectionReadOnly: function (data) {
        "use strict";
        var self = this;

        var isSectionReadOnly = true;
        for (var i = 0; i < data.LogInvoices.length; i++) {
            var isRowReadOnly = self._isInvoiceValueReadOnly(data.LogInvoices[i].InvoiceRow.Status);
            if (!isRowReadOnly) {
                isSectionReadOnly = false;
            }
        }

        for (var i = 0; i < data.ExternalInvoices.length; i++) {
            isRowReadOnly = self._isInvoiceValueReadOnly(data.ExternalInvoices[i].InvoiceRow.Status);
            if (!isRowReadOnly) {
                isSectionReadOnly = false;
            }
        }

        return isSectionReadOnly;
    },
    
}