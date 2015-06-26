$(document).ready(function () {
    'use strict';

    // caled from $.validate.submitHandler in Edit.cshtml and New.cstml
    window.submitForm = function() {
        for (var i = 0; i < document.getElementById("CsSelected").length; i++) {
            document.getElementById("CsSelected")[i].selected = true;
        }

        $("#AAsSelected option").attr("selected", "selected");
        $("#OTsSelected option").attr("selected", "selected");
    };

    $("#addCs").click(function() {
        if (document.getElementById("CsAvailable").length != 0 && document.getElementById("CsAvailable").selectedIndex != -1) {
            // Add to selected
            $("#CsAvailable option:selected").remove().appendTo("#CsSelected");
            $("#CsAvailable").get(0).selectedIndex = -1;
            $("#CsSelected").get(0).selectedIndex = -1;

            var doAdd = true;
            var customerid = 0;
            for (var i = 0; i < document.getElementById("CsSelected").length; i++) {
                doAdd = true;
                for (var j = 0; j < document.getElementById("User_Customer_Id").length; j++) {
                    customerid = document.getElementById("User_Customer_Id")[j].value;
                    if (document.getElementById("CsSelected").options[i].value == customerid) {
                        doAdd = false;
                        break;
                    }
                }
                if (doAdd)
                    document.getElementById("User_Customer_Id").options[document.getElementById("User_Customer_Id").length] =
                        new Option(document.getElementById("CsSelected")[i].text,
                            document.getElementById("CsSelected")[i].value, false, false);
            }
        }
        return false;
    });

    $("#removeCs").click(function() {

        if (document.getElementById("CsSelected").length != 0 && document.getElementById("CsSelected").selectedIndex != -1) {

            $("#CsSelected option:selected").remove().appendTo("#CsAvailable");
            $("#CsAvailable").get(0).selectedIndex = -1;
            $("#CsSelected").get(0).selectedIndex = -1;

            var doRemove = true;
            var customerid = 0;
            for (var i = 0; i < document.getElementById("User_Customer_Id").length; i++) {
                doRemove = true;
                for (var j = 0; j < document.getElementById("CsSelected").length; j++) {
                    customerid = document.getElementById("CsSelected")[j].value;
                    if (document.getElementById("User_Customer_Id").options[i].value == customerid) {
                        doRemove = false;
                        break;
                    }
                    //
                }
                if (doRemove)
                    document.getElementById("User_Customer_Id").options[i] = null;
            }
        }

        return false;
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