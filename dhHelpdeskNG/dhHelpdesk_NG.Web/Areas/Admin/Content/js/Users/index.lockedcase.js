﻿'use strict';

$(function () {

    var filterLockedCaseUrl = window.partialparams.filterLockedCaseUrl;
    var customerFilter = window.partialparams.customerFilter;
    var caseNumberFilter = window.partialparams.caseNumberFilter;
    var textFilter = window.partialparams.textFilter;
    var filterButton = window.partialparams.filterButton;

    var unLockCaseUrl = window.partialparams.unLockCaseUrl;
    var releaseButtonsPrefixName = window.partialparams.releaseButtonsPrefixName;
    var lockTimeTextPrefixName = window.partialparams.lockTimeTextPrefixName;
    var lockedCasePartialName = window.partialparams.lockedCasePartialName;
    var $caselockContainer = $('.caselockinfo.container');

    function unlockHandler() {
        var caseId = $(this).attr('selectedcaseid');
        var customerId = $(customerFilter).val();
        var caseNumber = $(caseNumberFilter).val();
        var searchText = $(textFilter).val();
        if (caseId != '') {            
            $.get(unLockCaseUrl, {
                caseId: caseId,
                selectedCustomerId: customerId,
                caseNumber: caseNumber,
                searchText: searchText,
                curTime: new Date().getTime()
            }, function (_LockedCases) {
                $(lockedCasePartialName).html(_LockedCases);
            });
        }
    }

    $caselockContainer.on('click', '.btn.releasecase', unlockHandler);

    $(caseNumberFilter).keydown(function (e) {
        if (e.keyCode == 13) {
            $(filterButton).click();
        }
    });

    $(textFilter).keydown(function (e) {
        if (e.keyCode == 13) {
            $(filterButton).click();
        }
    });

    $(filterButton).click(function () {
        var customerId = $(customerFilter).val();
        var caseNumber = $(caseNumberFilter).val();
        var searchText = $(textFilter).val();

        $.get(filterLockedCaseUrl,
            {
                selectedCustomerId: customerId,
                caseNumber: caseNumber,
                searchText: searchText,
                curTime: new Date().getTime()
            })
            .done(function(newContent) {
                $caselockContainer.html(newContent);
            });
    });

});

