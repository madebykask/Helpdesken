﻿$(document).ready(function () {
    'use strict';

    // caled from $.validate.submitHandler in Edit.cshtml and New.cstml
    window.submitForm = function() {
        for (var i = 0; i < document.getElementById("CsSelected").length; i++) {
            document.getElementById("CsSelected")[i].selected = true;
        }

        $("#AAsSelected option").attr("selected", "selected");
        $("#OTsSelected option").attr("selected", "selected");
    };

    var $customersAvailableEl = $('#CsAvailable');
    var $customersSelectedEl = $('#CsSelected');
    var $defaultCustomerEl = $('#User_Customer_Id');

    function UpdateDefaultCustomerList() {
        var selectedVal = $defaultCustomerEl.val();
        $defaultCustomerEl.find('option').remove();
        $customersSelectedEl.find('option').clone().appendTo($defaultCustomerEl);
        $defaultCustomerEl.val(selectedVal);
    }

    $("#addCs").click(function () {
        $customersAvailableEl.find('option:selected').detach().appendTo($customersSelectedEl);
        UpdateDefaultCustomerList();
    });

    $("#removeCs").click(function () {
        $customersSelectedEl.find('option:selected').detach().appendTo($customersAvailableEl);
        UpdateDefaultCustomerList();
    });

    $("#addAAs").click(function() {
        $("#AAsAvailable option:selected").remove().appendTo("#AAsSelected");
        $("#AAsAvailable").get(0).selectedIndex = -1;
        $("#AAsSelected").get(0).selectedIndex = -1;
        return false;
    });

    $("#removeAAs").click(function() {
        $("#AAsSelected option:selected").remove().appendTo("#AAsAvailable");
        $("#AAsAvailable").get(0).selectedIndex = -1;
        $("#AAsAvailable").get(0).selectedIndex = -1;
        return false;
    });

    $("#addOTs").click(function() {
        $("#OTsAvailable option:selected").remove().appendTo("#OTsSelected");
        $("#OTsAvailable").get(0).selectedIndex = -1;
        $("#OTsSelected").get(0).selectedIndex = -1;
        return false;
    });

    $("#removeOTs").click(function() {
        $("#OTsSelected option:selected").remove().appendTo("#OTsAvailable");
        $("#OTsAvailable").get(0).selectedIndex = -1;
        $("#OTsAvailable").get(0).selectedIndex = -1;
        return false;
    });
});