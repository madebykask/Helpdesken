function CaseCharge(options) {
    "use strict";

    this.options = $.extend({}, this.getDefaults(), options);
    this.init();

}

CaseCharge.prototype = {
    init: function () {
        "use strict";
        var self = this;

        $("#caseChargePopup #btnSave").on("click", function () {
            self._saveData();
        });
    },

    getDefaults: function () {
        "use strict";

        return {

        };
    },

    show: function () {
        "use strict";
        var self = this;

        self._getData();
    },

    _getData: function () {
        "use strict";
        var self = this;

        $.ajax({
            url: self.options.getListUrl,
            data: {
                CaseId: self.options.caseId,
                DepartmentCharge: false
    }
        })
        .done(function(responseData) {
            if (!responseData) {
                throw new Error("getdata error");
            }

            //build
            if (responseData.length && responseData.length === 1) {
                self._buildLogInvoicesGrid(responseData[0].LogInvoices);
                self._buildExternalInvoicesGrid(responseData[0].ExternalInvoices);

                self.data = responseData[0];
            }

            $("#caseChargePopup #btnSave").prop('disabled', false);
            $("#caseChargePopup").modal("show");
        });
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

    _minutesToTimeString: function (mins) {
        "use string";
        var self = this;

        var time = self._minutesToTime(mins);
        return time.hours + self.options.hourText + " " + time.minutes + self.options.minText;
    },

    _minutesToTime: function (mins) {
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

    _formatNumber: function (v, n, x, s, c) {
        var re = "\\d(?=(\\d{" + (x || 3) + "})+" + (n > 0 ? "\\D" : "$") + ")",
            num = v.toFixed(Math.max(0, ~~n));

        return (c ? num.replace(".", c) : num).replace(new RegExp(re, "g"), "$&" + (s || ","));
    },

    _getStatusText: function (status) {
        "use strict";
        var self = this;

        return status && self.options.statusList[status] ? self.options.statusList[status] : "";
    },

    _buildLogInvoicesGrid: function (logInvoices) {
        "use strict";
        var self = this;
      
        var _showTime = self.options.showInvoiceTime === "True";
        var _showOvertime = self.options.showInvoiceOvertime === "True";
        var _showPrice = self.options.showInvoicePrice === "True";
        var _showMaterial = self.options.showInvoiceMaterial === "True";

        var res = [];
        for (var i = 0; i < logInvoices.length; i++) {
            var isRowReadOnly = self._isInvoiceValueReadOnly(logInvoices[i].InvoiceRow.Status);
            var rowString =
            "<tr>" +
            "<td>" + "<input type=hidden id='Id' value='" + logInvoices[i].Id + "'/>" + self._formatDate(logInvoices[i].Date) + "</td>" +
		    "<td>" + $("<span/>").text(logInvoices[i].Text).html() + "</td>" +
		    "<td>" + (isRowReadOnly ? self._minutesToTimeString(logInvoices[i].WorkingTime) :
                                      self._getTimeEditor(logInvoices[i].WorkingTime, "WorkingTime", _showTime)) + "</td>" +

		    "<td>" + (isRowReadOnly ? self._minutesToTimeString(logInvoices[i].Overtime) :
                                      self._getTimeEditor(logInvoices[i].Overtime, "Overtime", _showOvertime)) + "</td>" +

		    "<td>" + (isRowReadOnly ? self._formatNumber(logInvoices[i].Material, 2, 3, " ", ",") :
                                      self._getTextBox(logInvoices[i].Material, "txtMaterial", _showMaterial)) + "</td>" +

		    "<td>" + (isRowReadOnly ? self._formatNumber(logInvoices[i].Price, 2, 3, " ", ",") :
                                      self._getTextBox(logInvoices[i].Price, "txtPrice", _showPrice)) + "</td>" +

		    "<td class='align-center'>" + self._getChargeBox(isRowReadOnly, logInvoices[i].Charge) + "</td>" +
		    "<td>" + self._getStatusText(logInvoices[i].InvoiceRow.Status) + "</td>" +
            "</tr>";
            var row = $(rowString);
            res.push(row[0]);
        }

        $("#logInvoiceGrid tbody").html(res);
        //$("#logInvoiceGrid").DataTable();
    },

    _buildExternalInvoicesGrid: function (externalInvoices) {
        "use strict";
        var self = this;

        var res = [];
        for (var i = 0; i < externalInvoices.length; i++) {
            var isRowReadOnly = self._isInvoiceValueReadOnly(externalInvoices[i].InvoiceRow.Status);
            
            var rowString =
            "<tr>" +
		    "<td>" + "<input type=hidden id='Id' value='" + externalInvoices[i].Id + "'/>" + $("<span/>").text(externalInvoices[i].Name).html() + "</td>" +
		    "<td class='align-right'>" + (isRowReadOnly ? self._formatNumber(externalInvoices[i].Amount, 2, 3, " ", ",") : self._getTextBox(externalInvoices[i].Amount, "txtExternalAmount")) + "</td>" +
		    "<td class='align-center'>" + self._getChargeBox(isRowReadOnly, externalInvoices[i].Charge) + "</td>" +
		    "<td>" + self._getStatusText(externalInvoices[i].InvoiceRow.Status) + "</td>" +
            "</tr>";
            var row = $(rowString);
            res.push(row[0]);
        }

        $("#externalInvoiceGrid tbody").html(res);
    },

    _getTimeEditor: function (mins, name, visible) {
        "use strict";
        var self = this;

        var time = self._minutesToTime(mins);

        if (visible == undefined || visible) {
            var hours = $("<select id='ddl" + name + "Hours' name='ddl" + name + "Hours' class='inputw55'></select>");
            for (var i = 0; i < 100; i++) {
                hours.append($('<option>', {
                    value: i,
                    text: i,
                    selected: time.hours === i
                }));
            }
            var minutes = $("<select id='ddl" + name + "Minutes' name='ddl" + name + "Minutes' class='inputw55'></select>");
            for (i = 0; i < 60; i += self.options.minStep) {
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
        } else {
            var hours = $("<input type='hidden' id='ddl" + name + "Hours' name='ddl" + name + "Hours' class='inputw55' value=" + time.hours + ">");
            var minutes = $("<input type='hidden' id='ddl" + name + "Minutes' name='ddl" + name + "Minutes' class='inputw55' value=" + time.minutes + ">");
            var res = $("<span/>")
             .append($("<div class='nowrap'/>")
                 .append($("<span/>")
                     .append(hours))
                 .append($("<span/>")
                     .append(" ")
                     .append(minutes))
             );
        }
        
        return res.html();
    },

    _isInvoiceValueReadOnly: function (status) {
        return status && (status === 2 || status === 3);
    },

    _getTextBox: function (val, name, visible) {
        var _type = visible == undefined || visible ? "text" : "hidden";
        var res = "<input type='"+ _type +"' class='inputw55' id='" + name + "' name='" + name + "' placeholder='0.00' value='{1}'>"
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

    _saveData: function () {
        "use strict";
        var self = this;

        var externalInvoices = [];
        var logInvoices = [];

        //var totalWorkHours = 0;
        //var totalOvertimeHours = 0;
        //var totalPrice = 0;
        //var totalMaterial = 0;

        $("#logInvoiceGrid tbody tr").each(function (i, row) {
            var logId = Number($("#Id", row).val());
            var workingTimeControl = $("#ddlWorkingTimeHours", row);
            var workingHours = Number($("#ddlWorkingTimeHours", row).val()) * 60 + Number($("#ddlWorkingTimeMinutes", row).val());
            var overtimeHours = Number($("#ddlOvertimeHours", row).val()) * 60 + Number($("#ddlOvertimeMinutes", row).val());
            var price = Number($("#txtPrice", row).val());
            var material = Number($("#txtMaterial", row).val());
            if (workingTimeControl.length) {
                var logInvoice = {
                    Id: logId,
                    WorkingTime: workingHours,
                    Overtime: overtimeHours,
                    Material: material,
                    Price: price,
                    Charge: $("#cbCharge", row).is(':checked')
                }
                logInvoices.push(logInvoice);

                //refresh data;
                self._refreshLogData(logInvoice);
            }
            
        });

        $("#externalInvoiceGrid tbody tr").each(function (i, row) {
            var externalId = Number($("#Id", row).val());
            var amountControl = $("#txtExternalAmount", row);
            var amount = Number($("#txtExternalAmount", row).val());
            if (amountControl.length) {
                var externalInvoice = {
                    Id: externalId,
                    Amount: amount,
                    Charge: $("#cbCharge", row).is(':checked')
                };
                externalInvoices.push(externalInvoice);

                //refresh data;
                self._refreshExternalData(externalInvoice);
            }

        });

        if (logInvoices.length || externalInvoices.length) {
            $("#caseChargePopup #btnSave").prop('disabled', true);
            $.postAntiForgery({
                    url: self.options.saveValuesUrl,
                    data: {
                        LogInvoices: logInvoices,
                        ExternalInvoices: externalInvoices
                    }
                })
                .done(function(responseData) {
                    if (!responseData) {
                        throw new Error('saverow error');
                    }

                    //refresh total
                    $("#totalWorkingTime").html(self._minutesToTimeString(self._getWorkingTime(self.data)));
                    $("#totalOvertime").html(self._minutesToTimeString(self._getOvertime(self.data)));
                    $("#totalMaterial").html(self._getMaterial(self.data));
                    $("#totalPrice").html(self._getPrice(self.data));
                    $("#totalExternalAmount").html(self._getExternalInvoice(self.data));

                    $("#caseChargePopup").modal("hide");

                })
                .always(function() {
                    //spinner
                    $("#caseChargePopup #btnSave").prop('disabled', false);
                });
        } 
    },

    _refreshLogData: function(logInvoice) {
        "use strict";
        var self = this;
        
        var dataEntry = null;
        for (var i = 0; i < self.data.LogInvoices.length; i++) {
            if (self.data.LogInvoices[i].Id === logInvoice.Id) {
                dataEntry = self.data.LogInvoices[i];
                break;
            }
        }

        if (dataEntry) {
            dataEntry.WorkingTime = logInvoice.WorkingTime;
            dataEntry.Overtime = logInvoice.Overtime;
            dataEntry.Material = logInvoice.Material;
            dataEntry.Price = logInvoice.Price;
        }
    },

    _refreshExternalData: function (externalInvoice) {
        "use strict";
        var self = this;

        var dataEntry = null;
        for (var i = 0; i < self.data.ExternalInvoices.length; i++) {
            if (self.data.ExternalInvoices[i].Id === externalInvoice.Id) {
                dataEntry = self.data.ExternalInvoices[i];
                break;
            }
        }

        if (dataEntry) {
            dataEntry.Amount = externalInvoice.Amount;
        }
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
    }
}