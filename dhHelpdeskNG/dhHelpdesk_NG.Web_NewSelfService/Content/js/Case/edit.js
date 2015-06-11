$(document).ready(function() {
    'use strict';

    function Application() {}

    Application.prototype.init = function (opt) {
        var me = this;
        me.opt = opt || {};
        me.$regionControl = $('#NewCase_Region_Id');
        me.$departmentControl = $('#NewCase_Department_Id');
        me.$orgUnitControl = $('#NewCase_Ou_Id');

        // Remove after implementing http://redmine.fastdev.se/issues/10995
        me.$regionControl.on('change', function () {
            me.refreshDepartment.call(me, $(this).val());
        });

        me.$departmentControl.on('change', function () {
            // Remove after implementing http://redmine.fastdev.se/issues/10995        
            var departmentId = $(this).val();
            me.refreshOrganizationUnit(departmentId);
        });
    };

    /**
    * @private
    * @param { number } regionId
    * @param { number } filterFormat
    */
    Application.prototype.refreshDepartment = function (regionId) {
        var me = this;
        me.$departmentControl.val('').find('option').remove();
        me.$departmentControl.append('<option value="">&nbsp;</option>');
        $.get(me.opt.departmentsURL, {
            'id': regionId,
            'customerId': me.opt.customerId,
            'departmentFilterFormat': me.opt.departmentFilterFormat
        }, function (resp) {
            if (resp != null && resp.success) {
                $(me.$departmentControl).append(me.makeOptionsFromIdName(resp.data));
            }
        }, 'json').always(function () {
            me.$departmentControl.prop('disabled', false);
            me.refreshOrganizationUnit(me.$departmentControl.val());
        });
    };

    /**
    * @private
    * @param { number } selectedRegionId
    * @param { number } filterFormat department filter format
    */
    Application.prototype.refreshOrganizationUnit = function (departmentId) {
        var me = this;
        me.$orgUnitControl.val('').find('option').remove();
        me.$orgUnitControl.append('<option value="">&nbsp;</option>');
        $.get(me.opt.orgUnitURL, {
            'id': departmentId, 
            'customerId': me.opt.customerId, 
            'departmentFilterFormat': me.opt.departmentFilterFormat
        }, function (resp) {
            if (resp != null && resp.success) {
                me.$orgUnitControl.append(me.makeOptionsFromIdName(resp.data));
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
    Application.prototype.makeOptionsFromIdName = function(data) {
        var content = [];
        for (var i = 0; i < data.length; i++) {
            var item = data[i];
            content.push("<option value='" + item.id + "'>" + item.name + "</option>");
        }
        return content.join('');
    }

    var app = new Application();
    app.init(window.appOptions);
});