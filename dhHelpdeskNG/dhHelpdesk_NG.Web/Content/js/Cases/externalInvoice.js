function ExternalInvoice(options) {
    "use strict";

    this.options = $.extend({}, this.getDefaults(), options);
    this.init();

}

ExternalInvoice.prototype = {
    init: function () {
        "use strict";
        var self = this;

        self.$btnAdd = $('#externalInvoices > tbody:last-child #btnAddExternalInvoice');
        self.$btnAdd.on("click", function (e) {
            self.addRow();
        });
        
        $('#externalInvoices [id^=btnDeleteExternalInvoice]').on("click", function (e) {
            self.deleteRow(e);
        });

        //set validation for loaded rows
        $('#externalInvoices > tbody tr:not(:last-child)').each(function(i, el) {
            self._addValidation($(el), true);
        });

        self._addValidation($('#externalInvoices > tbody tr:last-child'), false);
    },

    getDefaults: function () {
        "use strict";

        return {
            requiredMessage: "*",
            mustBeNumberMessage: "#.##"
        };
    },

    addRow: function() {
        "use strict";
        var self = this;

        var lastRow = $('#externalInvoices > tbody tr:last-child');
        var lastId = $('[id^=btnDeleteExternalInvoice]', lastRow).data('rowid');
        var row = $('<tr><td><input type="text" id="ExternalInvoices[{0}].Name" name="ExternalInvoices[{0}].Name"></td><td><input type="text" class="inputw100" id="ExternalInvoices[{0}].Value" name="ExternalInvoices[{0}].Value" placeholder="0.00"><input type="hidden" id="ExternalInvoices[{0}].Id" name="ExternalInvoices[{0}].Id" value="0"> </td><td><a id="btnDeleteExternalInvoice{0}" class="btn bt-small" title="Delete" data-rowid="{0}"><i class="icon-remove"></i></a></td></tr>'
            .replace(/\{0\}/g, ++lastId)
        );

        $('#externalInvoices > tbody').append(row);
        $('[id^=btnDeleteExternalInvoice]', row).on("click", function (e) {
            self.deleteRow(e);
        });
        self.$btnAdd.appendTo($('td:nth-child(3)', row));
        self._addValidation(lastRow, true);
        self._addValidation(row, false);
    },

    deleteRow: function(e) {
        "use strict";
        var self = this;

        var btn = $(e.currentTarget);
        var row = btn.parents(':eq(1)');
        var rowId = $('[id^=btnDeleteExternalInvoice]', row).data('rowid');
        var isLast = $('#btnAddExternalInvoice', row).length === 1;
        var isOnly = $('#externalInvoices > tbody tr').length === 1;
        var lastRow = $('#externalInvoices > tbody tr:last-child');
        
        if (isOnly) {
            $('input', row).val('');
            return;
        }

        if (isLast) {
            self.$btnAdd.appendTo($('td:nth-child(3)', row.prev()));
        }

        self._removeValidaton(lastRow, false);
        self._removeValidaton(lastRow.prev(), true);
        row.remove();

        $('#externalInvoices tr:gt(' + rowId + ')').each(function(i, el) {
            var index = rowId + i;
            var prefix = 'ExternalInvoices[{0}]'.replace('{0}', index);
            var txtName = $('td:nth-child(1) input[type="text"]', el);
            txtName.prop('id', prefix + '.Name');
            txtName.prop('name', prefix + '.Name');
            var txtValue = $('td:nth-child(2) input[type="text"]', el);
            txtValue.prop('id', prefix + '.Value');
            txtValue.prop('name', prefix + '.Value');
            var txtId = $('td:nth-child(2) input[type="hidden"]', el);
            txtId.prop('id', prefix + '.Id');
            txtId.prop('name', prefix + '.Id');
            var btnDelete = $('a[data-rowid]', el);
            btnDelete.data('rowid', index);
            btnDelete.prop('id', 'btnDeleteExternalInvoice' + index);
        });
    },

    _addValidation: function (row, setRequired) {
        "use strict";
        var self = this;

        var txtName = $("td:nth-child(1) input[type=\"text\"]", row);
        var txtValue = $("td:nth-child(2) input[type=\"text\"]", row);
        if (setRequired) {
            txtName.rules("add", {
                required: true,
                messages: {
                    required: self.options.requiredMessage,
                }
            });
            txtValue.rules("add", {
                required: true,
                messages: {
                    required: self.options.requiredMessage,
                }
            });
        }
        
        txtValue.rules("add", {
            regex: "^\\d+(?:[.,]\\d{1,2})?$",
            messages: {
                regex: self.options.mustBeNumberMessage,
            }
        });
    },

    _removeValidaton: function (row, removeRequiredOnly) {
        "use strict";

        var txtName = $("td:nth-child(1) input[type=\"text\"]", row);
        var txtValue = $("td:nth-child(2) input[type=\"text\"]", row);
        txtName.rules("remove", removeRequiredOnly ? "required" : "");
        txtValue.rules("remove", removeRequiredOnly ? "required" : "");

        txtName.valid();
        txtValue.valid();
    }
}