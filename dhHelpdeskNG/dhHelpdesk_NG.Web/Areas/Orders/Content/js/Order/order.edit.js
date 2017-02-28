(function($) {
    "use strict";

    window.EditOrder = function (options) {
        this._options = $.extend({
            statuses: []
        }, options);
    }

    window.EditOrder.prototype = {
        init: function () {
            var that = this;
            
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
                $("input[type='hidden'][multitext]").each(function(i, e) {
                    var fieldId = $(e).prop("id");
                    var allRows = $(".multitext[data-field-id='" + fieldId + "']");
                    var result = "";
                    allRows.each(function(index, el) {
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

            if (that._options.isUserDepartmentVisible) {

                var $region = $("#region_dropdown");
                if ($region.val()) {
                    var $department = $("#orderer_departmentId");
                    var originVal = $department.val();

                    applyUserDepartmentFilter($region.val())
                    .done(function () {
                        $department.val(originVal);
                    });
                }


                $region.on("change", function () {
                    applyUserDepartmentFilter($(this).val());
                });
            }

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
                var $row = $(this.closest(".multitext"));
                var fieldId = $row.data("fieldId");
                var allRows = $(".multitext[data-field-id='" + fieldId + "']");
                if (allRows.length >= maxFields) {
                    return;
                }

                var $templateHtml = $($row.clone().wrapAll("<div/>").parent().html());
                $templateHtml.find(".number").html(allRows.length + 1);
                $templateHtml.find("input[name$='id']").val("");
                $templateHtml.find("input[name$='name']").val("");
                $templateHtml.find("i[name='add']").on("click", addMultiTextField);
                $templateHtml.find("i[name='remove']").on("click", removeMultiTextField);
                allRows.last().after($templateHtml);
            }

            function removeMultiTextField(e) {
                var minFields = 1;
                var $row = $(this.closest(".multitext"));
                var fieldId = $row.data("fieldId");
                var allRows = $(".multitext[data-field-id='" + fieldId + "']");
                if (allRows.length <= minFields) {
                    return;
                }

                $row.remove();
                allRows = $(".multitext[data-field-id='" + fieldId + "']");
                allRows.find(".number").each(function(i, e) {
                    $(e).html(i + 1);
                });
            }

            $(".multitext i[name='add']")
                .on("click", addMultiTextField);
            $(".multitext i[name='remove']")
                .on("click", removeMultiTextField);
        }
    }
})(jQuery);