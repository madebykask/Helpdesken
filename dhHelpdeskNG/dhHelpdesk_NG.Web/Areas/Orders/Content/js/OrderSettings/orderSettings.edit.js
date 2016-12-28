(function($) {
    "use strict";

    window.OrderSettings = function (options) {
        this.self = this;
        this._options = $.extend({}, options);
    }

    window.OrderSettings.prototype = {
        init: function() {
            $(".switchcheckbox").bootstrapSwitch("onText", window.trans_yes);
            $(".switchcheckbox").bootstrapSwitch("offText", window.trans_no);
            $(".switchcheckbox").bootstrapSwitch("size", "small");
            $(".switchcheckbox").bootstrapSwitch("onColor", "success");

            $(".order_field_values").on("click", this.openOrderFieldTypePopup());
        },

        openOrderFieldTypePopup: function () {
            var that = this;
            return function(e) {

                e.preventDefault();

                var data = [];
                var $values = $(e.target).next().find("input[type='hidden']");
                for (var i = 0; i < $values.length; i += 2) {
                    var id = $($values[i]).val();
                    var value = $($values[i + 1]).val();
                    data.push(new OrderData(id, value));
                }

                var modal = new OrderFieldTypePopup({
                    data: data,
                    onSave: that.renderData.bind(e.target)
                });
                modal.open();
            }
        },

        renderData: function (data) {
            var that = this;

            var hiddenFieldTypeTemplate = $("#hiddenFieldTypeTemplate").text();
            var $container = $(this).next();
            var html = "";

            if (data.length > 0) {
                $.each(data, function(i, orderData) {
                    var template = hiddenFieldTypeTemplate + ""; //clone

                    template = template.replace(/{index}/gi, i)
                        .replace(/{id}/gi, orderData.id)
                        .replace(/{type}/gi, $(that).data("type"))
                        .replace(/{value}/gi, orderData.value);
                    html += template;
                });
            }
            $container.empty().html(html);
        }
    };

    ////////////////////////////////////////////////OrderData
    function OrderData(id, value) {
        this.id = id;
        this.value = value;
    }

    ////////////////////////////////////////////////OrderFieldTypePopup
    function OrderFieldTypePopup(options) {
        this.$modal = $("#order_field_type_modal");
        this.data = options.data;
        this.rowTemplate = this.$modal.find("#rowTemplate").html();
        this.onSave = options.onSave;
    }

    OrderFieldTypePopup.prototype = {
        open: function() {
            var that = this;

            that.$modal.one("show", function() {
                that.resetAll();
                that.init();
            });

            that.$modal.modal();
        },

        resetAll: function() {
            var that = this;

            that.$modal.find("#addBtn").off("click");
            that.$modal.find("#btnSave").off("click");
            that.resetDataFields();
        },

        resetDataFields: function () {
            var that = this;

            that.$modal.find("#deleteBtn").off("click");
            that.$modal.find("input[type='text']").val("");
        },

        init: function () {
            var that = this;
            that.$modal.find("#addBtn").on("click", function (e) {
                e.preventDefault();
                if (that.$modal.find("#addFrm").valid()) {
                    that.collectData();

                    var value = $("#addValue", that.$modal).val();
                    that.data.push(new OrderData(null, value));
                    that.render(true);
                    $(e.target).val("");
                }
            });
            that.$modal.find("addFrm").validate();

            that.$modal.find("#btnSave").on("click", function (e) {
                e.preventDefault();
                if (that.$modal.find("#dataFrm").valid()) {

                    that.collectData();

                    if (typeof that.onSave === "function") {
                        that.onSave(that.data);
                    }
                    that.$modal.modal("hide");
                }
            });

            that.render(true);
        },

        initDataFields: function () {
            var that = this;

            that.$modal.find("#deleteBtn").on("click", function (e) {
                //TODO: check if it used already, if yes - not allow to delete
                $(e.target).closest("tr").remove();
                that.collectData();
                that.render(true);
            });

            that.$modal.find("dataFrm").validate();
        },

        collectData: function() {
            var that = this;

            var $td = that.$modal.find("#dataFrm tr td:first-child");
            that.data = $td.map(function(i, e) {
                var value = $(e).find("input[type='text']").val();
                var id = $(e).find("input[type='hidden']").val();
                return new OrderData(id, value);
            });
        },

        render: function (init) {
            var that = this;
            that.$modal.find("#deleteBtn").off("click");
            var $html = $("<div>");

            if (that.data.length > 0) {
                $.each(that.data, function(i, orderData) {
                    var $template = $(that.rowTemplate).clone();
                    var $valueCtrl = $template.find("#template_value");
                    var $validMsgCtrl = $valueCtrl.next();
                    var $idCtrl = $validMsgCtrl.next();
                    var newValueName = "value_" + i;
                    var newIdName = "id_" + i;

                    $valueCtrl.attr("id", newValueName);
                    $valueCtrl.attr("name", newValueName);
                    $valueCtrl.val(orderData.value);

                    $validMsgCtrl.attr("data-valmsg-for", newValueName);

                    $idCtrl.attr("id", newIdName);
                    $idCtrl.attr("name", newIdName);
                    $idCtrl.val(orderData.id);

                    $template.appendTo($html);
                });
            }
            that.$modal.find("#dataFrm tbody").empty().html($html.children());

            if (init) that.initDataFields();
        }
    }
    
})(jQuery);