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
        createSubCasePermission: 'createSubCasePermission',
        copyCasePermission: 'copyCasePermission',
        deleteCasePermission: 'deleteCasePermission',
        deleteAttachedFilePermission: 'deleteAttachedFilePermission',
        moveCasePermission: 'moveCasePermission',
        activateCasePermission: 'activateCasePermission',
        closeCasePermission: 'closeCasePermission',
        //restrictedCasePermission: 'restrictedCasePermission',
        followUpPermission: 'followUpPermission',
        dataSecurityPermission: 'dataSecurityPermission',
        caseSolutionPermission: 'caseSolutionPermission',
        reportPermission: 'reportPermission',
        faqPermission: 'faqPermission',
        calendarPermission: 'calendarPermission',
        createOrderPermission: 'createOrderPermission',
        administerOrderPermission: 'administerOrderPermission',
        dailyReportPermission: 'dailyReportPermission',
        bulletinBoardPermission: 'bulletinBoardPermission',
        invoicePermission: 'invoicePermission',
        documentPermission: 'documentPermission',
        inventoryAdminPermission: 'inventoryAdminPermission',
        inventoryViewPermission: 'inventoryViewPermission',
        contractPermission: 'contractPermission',
        caseUnlockPermission: 'caseUnlockPermission',
        caseInternalLogPermission: 'caseInternalLogPermission',
        invoiceTimePermission: 'invoiceTimePermission'
    }

    dhHelpdesk.admin.users.object = function (spec, my) {
        spec = spec || {};
        my = my || {};
        var that = {};

        var element = spec.element || {};

        var getElement = function() {
            return element;
        }

        my.element = element;

        that.getElement = getElement;

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
            if (isHasAccess) {
                $(my.element).parents('td').removeClass('disabled');
            } else {
                $(my.element).parents('td').addClass('disabled');
            }
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
        var dailyReportPermissions = spec.dailyReportPermissions || [];
        var bulletinBoardPermissions = spec.bulletinBoardPermissions || [];
        var invoicePermissions = spec.invoicePermissions || [];
        var documentPermissions = spec.documentPermissions || [];
        var inventoryAdminPermissions = spec.inventoryAdminPermissions || [];
        var inventoryViewPermissions = spec.inventoryViewPermissions || [];
        var contractPermissions = spec.contractPermissions || [];
        var caseUnlockPermissions = spec.caseUnlockPermissions || [];
        var caseInternalLogPermissions = spec.caseInternalLogPermissions || [];
        var invoiceTimePermissions = spec.invoiceTimePermissions || [];
        /**
        * @param { fn } walk
        */
        var walkPermissions = function (walk) {
            var allPermissions = [
                            casePermissions,
                            caseTemplatePermissions,
                            reportsPermissions,
                            faqPermissions,
                            calendarPermissions,
                            orderPermissions,
                            dailyReportPermissions,
                            bulletinBoardPermissions,
                            invoicePermissions,
                            documentPermissions,
                            inventoryAdminPermissions,
                            inventoryViewPermissions,
                            contractPermissions,
                            caseUnlockPermissions,
                            caseInternalLogPermissions,
                            invoiceTimePermissions];

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

        /**
        * Handler when user group was changed
        */
        var setUserGroup = function (ug, sp) {
            var permissions = dhHelpdesk.admin.users.permissionType;
            var setPermissions = sp !== 'undefined' ? sp : true;
            userGroup = ug;

            switch (ug) {
                case dhHelpdesk.admin.users.userGroup.user:
                    walkPermissions(function (permission) {
                        var type = permission.getType();
                        var hasAccess = type == dhHelpdesk.admin.users.permissionType.createCasePermission ||
                                        type == dhHelpdesk.admin.users.permissionType.createSubCasePermission ||
                                        type == dhHelpdesk.admin.users.permissionType.closeCasePermission ||
                                        //type == dhHelpdesk.admin.users.permissionType.restrictedCasePermission ||
                                        type == dhHelpdesk.admin.users.permissionType.caseInternalLogPermission;

                        permission.setAccess(hasAccess);

                        if (setPermissions || !permission.getIsHasAccess()) {
                            var hasPermission = type == dhHelpdesk.admin.users.permissionType.createCasePermission ||
                                                type == dhHelpdesk.admin.users.permissionType.createSubCasePermission ||
                                                //type == dhHelpdesk.admin.users.permissionType.restrictedCasePermission ||
                                                type == dhHelpdesk.admin.users.permissionType.closeCasePermission;
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
                            var isChecked = type == permissions.performerPermission ||
                                type == permissions.createCasePermission ||
                                type == permissions.createSubCasePermission ||
                                type == permissions.copyCasePermission ||
                                type == permissions.activateCasePermission ||
                                type == permissions.closeCasePermission ||
                                type == permissions.reportPermission ||
                                type == permissions.caseUnlockPermission ||
                                type == permissions.caseInternalLogPermission;
                            permission.setHasPermission(isChecked);
                        }

                        return true;
                    });
                    break;
                case dhHelpdesk.admin.users.userGroup.customerAdministrator:
                    walkPermissions(function (permission) {
                        var type = permission.getType();
                        //var hasAccess = type != dhHelpdesk.admin.users.permissionType.restrictedCasePermission; // permission was moved to customer level
                        permission.setAccess(true);
                        if (setPermissions || !permission.getIsHasAccess()) {
                            var hasPermission = 
                                (/*type != dhHelpdesk.admin.users.permissionType.restrictedCasePermission && */// permission was moved to customer level
                                  type != dhHelpdesk.admin.users.permissionType.dailyReportPermission);
                            permission.setHasPermission(hasPermission);
                        }
                        return true;
                    });
                    break;
                case dhHelpdesk.admin.users.userGroup.systemAdministrator:
                    walkPermissions(function (permission) {
                        var type = permission.getType();
                        var hasAccess = type == dhHelpdesk.admin.users.permissionType.dailyReportPermission;
                        permission.setAccess(hasAccess);
                        if (setPermissions || !permission.getIsHasAccess()) {
                            var hasPermission = true; //type != dhHelpdesk.admin.users.permissionType.restrictedCasePermission; // permission was moved to customer level
                            permission.setHasPermission(hasPermission);
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

        var getDailyReportPermissions = function () {
            return dailyReportPermissions;
        }

        var getBulletinBoardPermissions = function () {
            return bulletinBoardPermissions;
        }

        var getInvoicePermissions = function () {
            return invoicePermissions;
        }

        var getDocumentPermissions = function () {
            return documentPermissions;
        }

        var getInventoryAdminPermissions = function () {
            return inventoryAdminPermissions;
        }

        var getInventoryViewPermissions = function () {
            return inventoryViewPermissions;
        }

        var getContractPermissions = function () {
            return contractPermissions;
        }

        var getCaseUnlockPermissions = function () {
            return caseUnlockPermissions;
        }
        var getCaseInternalLogPermissions = function () {
            return caseInternalLogPermissions;
        }

        var getInvoiceTimePermissions = function () {
            return invoiceTimePermissions;
        }
        that.getUserGroup = getUserGroup;
        that.setUserGroup = setUserGroup;
        that.getCasePermissions = getCasePermissions;
        that.getCaseTemplatePermissions = getCaseTemplatePermissions;
        that.getReportsPermissions = getReportsPermissions;
        that.getFaqPermissions = getFaqPermissions;
        that.getCalendarPermissions = getCalendarPermissions;
        that.getOrderPermissions = getOrderPermissions;
        that.getDailyReportPermissions = getDailyReportPermissions;
        that.getBulletinBoardPermissions = getBulletinBoardPermissions;
        that.getInvoicePermissions = getInvoicePermissions;
        that.getDocumentPermissions = getDocumentPermissions;
        that.getInventoryAdminPermissions = getInventoryAdminPermissions;
        that.getInventoryViewPermissions = getInventoryViewPermissions;
        that.getContractPermissions = getContractPermissions;
        that.getCaseUnlockPermissions = getCaseUnlockPermissions;
        that.getCaseInternalLogPermissions = getCaseInternalLogPermissions;
        that.getInvoiceTimePermissions = getInvoiceTimePermissions;

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

        var id = spec.id || null;
        var isDefault = spec.isDefault || false;
        var customerId = spec.customerId || null;
        var markerElement = spec.markerElement || null;

        var getId = function() {
            return id;
        }

        var getIsDefault = function() {
            return isDefault;
        }

        var getCustomerId = function() {
            return customerId;
        }

        var isEmpty = function() {
            return my.element.val() == '-1';
        }

        var getMarkerElement = function() {
            return markerElement;
        }

        that.getId = getId;
        that.getIsDefault = getIsDefault;
        that.getCustomerId = getCustomerId;
        that.isEmpty = isEmpty;
        that.getMarkerElement = getMarkerElement;

        return that;
    }

    dhHelpdesk.admin.users.workingGroups = function (spec, my) {
        my = my || {};
        var that = dhHelpdesk.admin.users.object(spec, my);

        var workingGroups = spec.workingGroups || [];
        var clearUserWorkingGroups = spec.clearUserWorkingGroups || null;

        var getWorkingGroups = function() {
            return workingGroups;
        }

        var getEmpty = function(customerId) {
            for (var i = 0; i < workingGroups.length; i++) {
                var wg = workingGroups[i];
                if (wg.isEmpty() && wg.getCustomerId() == customerId) {
                    return wg;
                }
            }

            return null;
        }

        var getById = function(id) {
            for (var i = 0; i < workingGroups.length; i++) {
                var wg = workingGroups[i];
                if (wg.getId() == id) {
                    return wg;
                }
            }

            return null;
        }

        var markAll = function(mark, customerId) {
            for (var i = 0; i < workingGroups.length; i++) {
                var wg = workingGroups[i];
                if (wg.getCustomerId() == customerId) {
                    wg.getMarkerElement().val(mark);
                }
            }
        }

        that.getWorkingGroups = getWorkingGroups;

        if (clearUserWorkingGroups != null) {
            clearUserWorkingGroups.getElement().click(function() {
                for (var i = 0; i < workingGroups.length; i++) {
                    var wg = workingGroups[i];
                    wg.getElement().prop('checked', false);
                }
            });
        }

        for (var j = 0; j < workingGroups.length; j++) {
            var workingGroup = workingGroups[j];
            workingGroup.getElement().click(function () {
                var $this = $(this);
                var wg = getById($this.attr('data-field-id'));
                var checked = wg.getMarkerElement().val() == '1';
                if (checked) {
                    wg.getMarkerElement().val('0');
                    var empty = getEmpty(wg.getCustomerId());
                    if (empty != null) {
                        empty.getElement().prop('checked', true);
                    }
                } else {
                    markAll('0', wg.getCustomerId());
                    wg.getMarkerElement().val('1');
                }                
            });
        }

        return that;
    }

    //var user = dhHelpdesk.admin.users.user({ security: security, workingGroups: workingGroups });
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
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="createSubCasePermission"]'), type: dhHelpdesk.admin.users.permissionType.createSubCasePermission }));
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="copyCasePermission"]'), type: dhHelpdesk.admin.users.permissionType.copyCasePermission }));
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="deleteCasePermission"]'), type: dhHelpdesk.admin.users.permissionType.deleteCasePermission }));
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="deleteAttachedFilePermission"]'), type: dhHelpdesk.admin.users.permissionType.deleteAttachedFilePermission }));
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="moveCasePermission"]'), type: dhHelpdesk.admin.users.permissionType.moveCasePermission }));
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="activateCasePermission"]'), type: dhHelpdesk.admin.users.permissionType.activateCasePermission }));
        casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="closeCasePermission"]'), type: dhHelpdesk.admin.users.permissionType.closeCasePermission }));
        //casePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="restrictedCasePermission"]'), type: dhHelpdesk.admin.users.permissionType.restrictedCasePermission }));
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

        var dailyReportPermissions = [];
        dailyReportPermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="dailyReportPermission"]'), type: dhHelpdesk.admin.users.permissionType.dailyReportPermission }));

        var bulletinBoardPermissions = [];
        bulletinBoardPermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="bulletinBoardPermission"]'), type: dhHelpdesk.admin.users.permissionType.bulletinBoardPermission }));

        var invoicePermissions = [];
        invoicePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="invoicePermission"]'), type: dhHelpdesk.admin.users.permissionType.invoicePermission }));

        var documentPermissions = [];
        documentPermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="documentPermission"]'), type: dhHelpdesk.admin.users.permissionType.documentPermission }));

        var inventoryAdminPermissions = [];
        inventoryAdminPermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="inventoryAdminPermission"]'), type: dhHelpdesk.admin.users.permissionType.inventoryAdminPermission }));

        var inventoryViewPermissions = [];
        inventoryViewPermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="inventoryViewPermission"]'), type: dhHelpdesk.admin.users.permissionType.inventoryViewPermission }));


        var contractPermissions = [];
        contractPermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="contractPermission"]'), type: dhHelpdesk.admin.users.permissionType.contractPermission }));

        var caseUnlockPermissions = [];
        caseUnlockPermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="caseUnlockPermission"]'), type: dhHelpdesk.admin.users.permissionType.caseUnlockPermission }));

        var caseInternalLogPermissions = [];
        caseInternalLogPermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="caseInternalLogPermission"]'), type: dhHelpdesk.admin.users.permissionType.caseInternalLogPermission }));

        var invoiceTimePermissions = [];
        invoiceTimePermissions.push(dhHelpdesk.admin.users.permission({ element: $('[data-field="invoiceTimePermission"]'), type: dhHelpdesk.admin.users.permissionType.invoiceTimePermission }));

        var security = dhHelpdesk.admin.users.security({
            element: $('[data-user="security"]'),
            casePermissions: casePermissions,
            caseTemplatePermissions: caseTemplatePermissions,
            reportsPermissions: reportsPermissions,
            faqPermissions: faqPermissions,
            calendarPermissions: calendarPermissions,
            orderPermissions: orderPermissions,
            dailyReportPermissions: dailyReportPermissions,
            bulletinBoardPermissions: bulletinBoardPermissions,
            invoicePermissions: invoicePermissions,
            documentPermissions: documentPermissions,
            inventoryAdminPermissions: inventoryAdminPermissions,
            inventoryViewPermissions: inventoryViewPermissions,
            contractPermissions: contractPermissions,
            caseUnlockPermissions: caseUnlockPermissions,
            caseInternalLogPermissions: caseInternalLogPermissions,
            invoiceTimePermissions: invoiceTimePermissions,
        });

        var wGs = [];
        $('[data-field="userWorkingGroup"]').each(function () {
            var $this = $(this);
            var id = $this.attr('data-field-id');
            var wg = dhHelpdesk.admin.users.workingGroup({
                id: id,
                element: $this,
                customerId: $this.attr('name'),
                markerElement: $('[data-field="userWorkingGroup' + id + '"]')
            });

            wGs.push(wg);
        });

        var workingGroups = dhHelpdesk.admin.users.workingGroups({
            workingGroups: wGs,
            clearUserWorkingGroups: dhHelpdesk.admin.users.object({ element: $('[data-field="clearUserWorkingGroups"]') })
        });

        var user = dhHelpdesk.admin.users.user({
            security: security,
            workingGroups: workingGroups
        });

        var getUser = function() {
            return user;
        }

        var showInventoryAdminPermission = function(show)
        {
            var q = $('.inventoryAdminPermissionRow');
            if (show) {
                q.show();
            }
            else {
                q.hide();
            }
                
        }

        $('input[name="User.InventoryViewPermission"]').change(function () {
            var show = this.checked;
            showInventoryAdminPermission(show);
        });

        that.getUser = getUser;

        return that;
    }

    var scope = dhHelpdesk.admin.users.scope();
});