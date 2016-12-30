(function($) {
    "use strict";

    window.EditOrder = function (options) {
        this._options = $.extend({}, options);
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
                        var sel = $("#department_dropdown");
                        sel.empty();
                        sel.prepend("<option></option>");
                        for (var i = 0; i < json.length; i++) {
                            var e = json[i];
                            $("<option>").text(e.Name).val(e.Value).appendTo(sel);
                        }
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

                $("#InformOrderer").val($("#InformOrderer_action").prop("checked"));
                $("#InformReceiver").val($("#InformReceiver_action").prop("checked"));
                $("#CreateCase").val($("#CreateCase_action").prop("checked"));

                $("#edit_form").submit();
                return false;
            });

            if (that._options.isUserDepartmentVisible) {

                var $region = $("#region_dropdown");
                if ($region.val()) {
                    var $department = $("#department_dropdown");
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

                var $ordererDep = $("#Orderer_DepartmentId");
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


        }
    }
})(jQuery);