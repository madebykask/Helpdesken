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

    dhHelpdesk.admin.users.utils = {
        showMessage: function (message, type) {
            $().toastmessage('showToast', {
                text: dhHelpdesk.admin.users.utils.replaceAll(message, '|', '<br />'),
                sticky: true,
                position: 'top-center',
                type: type || 'notice',
                closeText: '',
                stayTime: 10000,
                inEffectDuration: 1000,
                width: 700
            });
        },

        showWarning: function(message) {
            dhHelpdesk.admin.users.utils.showMessage(message, 'warning');
        },

        replaceAll: function (string, omit, place, prevstring) {
            if (prevstring && string === prevstring)
                return string;
            prevstring = string.replace(omit, place);
            return dhHelpdesk.admin.users.utils.replaceAll(prevstring, omit, place, string);
        }
    }

    dhHelpdesk.admin.users.userGroup = {
        user: 1,
        administrator: 2,
        customerAdministrator: 3,
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
        var isHasAccess = spec.isHasAccess || true;
        var hidden = $(document).find('[name="' + my.element.attr('name') + '"]');
        hidden.val(hasPermission ? 1 : 0);

        var getHasPermission = function() {
            return hasPermission;
        }

        var setHasPermission = function(p) {
            hasPermission = p;
            my.element.prop('checked', p);
            hidden.val(hasPermission ? 1 : 0);
        }

        var getType = function() {
            return type;
        }

        var setAccess = function (hasAccess) {
            isHasAccess = hasAccess;
            my.element.prop('disabled', !isHasAccess);
        }

        var getIsHasAccess = function() {
            return isHasAccess;
        }

        that.getType = getType;
        that.getHasPermission = getHasPermission;
        that.setHasPermission = setHasPermission;
        that.setAccess = setAccess;
        that.getIsHasAccess = getIsHasAccess;

        my.element.click(function() {
            hasPermission = my.element.prop('checked');
            hidden.val(hasPermission ? 1 : 0);
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

        var setUserGroup = function (ug, sp) {
            userGroup = ug;

            var setPermissions = sp !== 'undefined' ? sp : true;

            switch (ug) {
                case dhHelpdesk.admin.users.userGroup.user:
                    walkPermissions(function (permission) {
                        permission.setAccess(false);

                        if (setPermissions || !permission.getIsHasAccess()) {
                            var hasPermission = permission.getType() == dhHelpdesk.admin.users.permissionType.createCasePermission;
                            permission.setHasPermission(hasPermission);
                        }

                        return true;
                    });
                    break;
                case dhHelpdesk.admin.users.userGroup.administrator:
                    walkPermissions(function (permission) {
                        var type = permission.getType();
                        
                        permission.setAccess(true);

                        if (setPermissions || !permission.getIsHasAccess()) {
                            var hasPermission = type != dhHelpdesk.admin.users.permissionType.faqPermission;
                            permission.setHasPermission(hasPermission);
                        }

                        return true;
                    });
                    break;
                case dhHelpdesk.admin.users.userGroup.customerAdministrator:
                case dhHelpdesk.admin.users.userGroup.systemAdministrator:
                    walkPermissions(function (permission) {
                        var type = permission.getType();

                        var hasAccess = type != dhHelpdesk.admin.users.permissionType.faqPermission;
                        permission.setAccess(hasAccess);

                        if (setPermissions || !permission.getIsHasAccess()) {
                            permission.setHasPermission(true);
                        }

                        return true;
                    });
                    break;
                default:
                    walkPermissions(function (permission) {
                        permission.setAccess(false);

                        if (setPermissions || !permission.getIsHasAccess()) {
                            permission.setHasPermission(false);
                        }

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
        var onChangeUserGroup = function (setPermissions) {
            var ug = parseInt(uge.val());
            setUserGroup(ug, setPermissions);
        }
        onChangeUserGroup(false);
        uge.change(onChangeUserGroup);

        return that;
    }

    dhHelpdesk.admin.users.workingGroup = function (spec, my) {
        my = my || {};
        var that = dhHelpdesk.admin.users.object(spec, my);

        var isDefault = spec.isDefault || false;

        var getIsDefault = function() {
            return isDefault;
        }

        that.getIsDefault = getIsDefault;

        return that;
    }

    dhHelpdesk.admin.users.workingGroups = function (spec, my) {
        my = my || {};
        var that = dhHelpdesk.admin.users.object(spec, my);

        var workingGroups = spec.workingGroups || [];

        var getWorkingGroups = function() {
            return workingGroups;
        }

        that.getWorkingGroups = getWorkingGroups;

        return that;
    }

    dhHelpdesk.admin.users.user = function (spec, my) {
        my = my || {};
        var that = dhHelpdesk.admin.users.object(spec, my);

        var security = spec.security || null;
        var workingGroups = spec.workingGroups || null;

        var getSecurity = function() {
            return security;
        }

        var getWorkingGroups = function() {
            return workingGroups;
        }

        that.getSecurity = getSecurity;
        that.getWorkingGroups = getWorkingGroups;

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

        var wGs = [];
        $('[data-field="UserWorkingGroup"]').each(function() {
            var $this = $(this);
            var wg = dhHelpdesk.admin.users.workingGroup({
                element: $this
            });

            wGs.push(wg);
        });

        var workingGroups = dhHelpdesk.admin.users.workingGroups({
            workingGroups: wGs
        });

        var user = dhHelpdesk.admin.users.user({
            security: security,
            workingGroups: workingGroups
        });

        var getUser = function() {
            return user;
        }

        that.getUser = getUser;

        return that;
    }

    var scope = dhHelpdesk.admin.users.scope();
});