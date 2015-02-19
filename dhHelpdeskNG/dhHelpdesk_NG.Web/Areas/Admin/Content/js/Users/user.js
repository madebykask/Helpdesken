'use strict';
$(function () {
    if (!window.dhHelpdesk) {
        window.dhHelpdesk = {};
    }

    if (!window.dhHelpdesk.admin) {
        window.dhHelpdesk.admin = {};
    }

    if (!window.dhHelpdesk.admin.users) {
        window.dhHelpdesk.admin.users = {};
    }

    dhHelpdesk.admin.users.userGroup = {
        user: 1,
        administrator: 2,
        customAdministrator: 3,
        systemAdministrator: 4
    }

    dhHelpdesk.admin.users.permissionType = {
        performerPermission : 'performerPermission',
        createCasePermission: 'createCasePermission',
        copyCasePermission: 'copyCasePermission',
        deleteCasePermission: 'deleteCasePermission',
        deleteAttachedFilePermission: 'deleteAttachedFilePermission',
        moveCasePermission: 'moveCasePermission',
        activateCasePermission: 'activateCasePermission',
        closeCasePermission: 'closeCasePermission',
        restrictedCasePermission: 'restrictedCasePermission',
        followUpPermission: 'followUpPermission',
        dataSecurityPermission: 'dataSecurityPermission',
        caseSolutionPermission: 'caseSolutionPermission',
        reportPermission: 'reportPermission',
        faqPermission: 'faqPermission',
        calendarPermission: 'calendarPermission',
        createOrderPermission: 'createOrderPermission',
        administerOrderPermission: 'administerOrderPermission'
    }

    dhHelpdesk.admin.users.object = function (spec, my) {
        spec = spec || {};
        my = my || {};
        var that = {};

        var element = spec.element || {};

        my.element = element;

        return that;
    }

    dhHelpdesk.admin.users.permission = function (spec, my) {
        my = my || {};
        var that = dhHelpdesk.admin.users.object(spec, my);

        var type = spec.type || '';
        var hasPermission = spec.hasPermission || my.element.prop('checked');

        var getHasPermission = function() {
            return hasPermission;
        }

        var setHasPermission = function(p) {
            hasPermission = p;
            my.element.prop('checked', p);
        }

        var getType = function() {
            return type;
        }

        var setAccess = function(isHasAccess) {
            my.element.prop('disabled', !isHasAccess);
        }

        that.getType = getType;
        that.getHasPermission = getHasPermission;
        that.setHasPermission = setHasPermission;
        that.setAccess = setAccess;

        my.element.click(function() {
            hasPermission = my.element.prop('checked');
        });

        return that;
    }

    dhHelpdesk.admin.users.security = function (spec, my) {
        my = my || {};
        var that = dhHelpdesk.admin.users.object(spec, my);

        var userGroup = spec.userGroup || null;
        var casePermissions = spec.casePermissions || [];
        var caseTemplatePermissions = spec.caseTemplatePermissions || [];
        var reportsPermissions = spec.reportsPermissions || [];
        var faqPermissions = spec.faqPermissions || [];
        var calendarPermissions = spec.calendarPermissions || [];
        var orderPermissions = spec.orderPermissions || [];        

        var walkPermissions = function (walk) {
            var allPermissions = [
                            casePermissions,
                            caseTemplatePermissions,
                            reportsPermissions,
                            faqPermissions,
                            calendarPermissions,
                            orderPermissions];

            for (var i = 0; i < allPermissions.length; i++) {
                var permissions = allPermissions[i];
                for (var j = 0; j < permissions.length; j++) {
                    var permission = permissions[j];
                    if (!walk(permission)) {
                        return;
                    }
                }
            }
        }

        var getUserGroup = function() {
            return userGroup;
        }

        var setUserGroup = function(ug) {
            userGroup = ug;

            switch (ug) {
                case dhHelpdesk.admin.users.userGroup.user:
                    walkPermissions(function (permission) {
                        permission.setAccess(false);
                        permission.setHasPermission(permission.getType() == dhHelpdesk.admin.users.permissionType.createCasePermission);
                        return true;
                    });
                    break;
                default:
                    walkPermissions(function (permission) {
                        permission.setAccess(true);
                        return true;
                    });
                    break;
            }
        }

        var getCasePermissions = function () {
            return casePermissions;
        }

        var getCaseTemplatePermissions = function () {
            return caseTemplatePermissions;
        }

        var getReportsPermissions = function () {
            return reportsPermissions;
        }

        var getFaqPermissions = function () {
            return faqPermissions;
        }

        var getCalendarPermissions = function () {
            return calendarPermissions;
        }

        var getOrderPermissions = function () {
            return orderPermissions;
        }

        that.getUserGroup = getUserGroup;
        that.setUserGroup = setUserGroup;
        that.getCasePermissions = getCasePermissions;
        that.getCaseTemplatePermissions = getCaseTemplatePermissions;
        that.getReportsPermissions = getReportsPermissions;
        that.getFaqPermissions = getFaqPermissions;
        that.getCalendarPermissions = getCalendarPermissions;
        that.getOrderPermissions = getOrderPermissions;

        var uge = my.element.find('[data-field="userGroup"]');
        var onChangeUserGroup = function () {
            var ug = parseInt(uge.val());
            setUserGroup(ug);
        }
        onChangeUserGroup();
        uge.change(onChangeUserGroup);

        return that;
    }

    dhHelpdesk.admin.users.user = function (spec, my) {
        my = my || {};
        var that = dhHelpdesk.admin.users.object(spec, my);

        var security = spec.security || null;

        var getSecurity = function() {
            return security;
        }

        that.getSecurity = getSecurity;

        return that;
    }

    dhHelpdesk.admin.users.scope = function (spec, my) {
        spec = spec || {};
        my = my || {};
        var that = {};

        var casePermissions = [];
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="performerPermission"]'), type: dhHelpdesk.admin.users.permissionType.performerPermission }));
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="createCasePermission"]'), type: dhHelpdesk.admin.users.permissionType.createCasePermission }));
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="copyCasePermission"]'), type: dhHelpdesk.admin.users.permissionType.copyCasePermission }));
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="deleteCasePermission"]'), type: dhHelpdesk.admin.users.permissionType.deleteCasePermission }));
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="deleteAttachedFilePermission"]'), type: dhHelpdesk.admin.users.permissionType.deleteAttachedFilePermission }));
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="moveCasePermission"]'), type: dhHelpdesk.admin.users.permissionType.moveCasePermission }));
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="activateCasePermission"]'), type: dhHelpdesk.admin.users.permissionType.activateCasePermission }));
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="closeCasePermission"]'), type: dhHelpdesk.admin.users.permissionType.closeCasePermission }));
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="restrictedCasePermission"]'), type: dhHelpdesk.admin.users.permissionType.restrictedCasePermission }));
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="followUpPermission"]'), type: dhHelpdesk.admin.users.permissionType.followUpPermission }));
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="dataSecurityPermission"]'), type: dhHelpdesk.admin.users.permissionType.dataSecurityPermission }));
        var caseTemplatePermissions = [];
        caseTemplatePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="caseSolutionPermission"]'), type: dhHelpdesk.admin.users.permissionType.caseSolutionPermission }));
        var reportsPermissions = [];
        reportsPermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="reportPermission"]'), type: dhHelpdesk.admin.users.permissionType.reportPermission }));
        var faqPermissions = [];
        faqPermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="faqPermission"]'), type: dhHelpdesk.admin.users.permissionType.faqPermission }));
        var calendarPermissions = [];
        calendarPermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="calendarPermission"]'), type: dhHelpdesk.admin.users.permissionType.calendarPermission }));
        var orderPermissions = [];
        orderPermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="createOrderPermission"]'), type: dhHelpdesk.admin.users.permissionType.createOrderPermission }));
        orderPermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="administerOrderPermission"]'), type: dhHelpdesk.admin.users.permissionType.administerOrderPermission }));

        var security = dhHelpdesk.admin.users.security({
            element: $('[data-user="security"]'),
            casePermissions: casePermissions,
            caseTemplatePermissions: caseTemplatePermissions,
            reportsPermissions: reportsPermissions,
            faqPermissions: faqPermissions,
            calendarPermissions: calendarPermissions,
            orderPermissions: orderPermissions
        });

        var user = dhHelpdesk.admin.users.user({
            security: security
        });

        return that;
    }

    var scope = dhHelpdesk.admin.users.scope();
});