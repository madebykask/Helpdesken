

$(document).ready(function () {
    $(".tabmenu > li").click(function (e) {

        var href = $("#" + e.target.id).attr("href");
    
        switch (e.target.id) {
            case "tab1":
                //change status & style menu
                $("#tab1").addClass("active");
                $("#tab2").removeClass("active");
                $("#tab3").removeClass("active");
                $("#tab4").removeClass("active");
                $("#tab5").removeClass("active");
                $("#tab6").removeClass("active");
                $("#tab7").removeClass("active");
                $("#tab8").removeClass("active");
                $("#tab9").removeClass("active");
                $("#tab10").removeClass("active");
                $("#tab11").removeClass("active");
                $("#tab12").removeClass("active");
                //display selected division, hide others
                $("div.tab1").css("display", "block");
                $("div.tab2").css("display", "none");
                $("div.tab3").css("display", "none");
                $("div.tab4").css("display", "none");
                $("div.tab5").css("display", "none");
                $("div.tab6").css("display", "none");
                $("div.tab7").css("display", "none");
                $("div.tab8").css("display", "none");
                $("div.tab9").css("display", "none");
                $("div.tab10").css("display", "none");
                $("div.tab11").css("display", "none");
                $("div.tab12").css("display", "none");
                break;
            case "tab2":
                //change status & style menu
                $("#tab1").removeClass("active");
                $("#tab2").addClass("active");
                $("#tab3").removeClass("active");
                $("#tab4").removeClass("active");
                $("#tab5").removeClass("active");
                $("#tab6").removeClass("active");
                $("#tab7").removeClass("active");
                $("#tab8").removeClass("active");
                $("#tab9").removeClass("active");
                $("#tab10").removeClass("active");
                $("#tab11").removeClass("active");
                $("#tab12").removeClass("active");
                //display selected division, hide others
                $("div.tab2").css("display", "block");
                $("div.tab1").css("display", "none");
                $("div.tab3").css("display", "none");
                $("div.tab4").css("display", "none");
                $("div.tab5").css("display", "none");
                $("div.tab6").css("display", "none");
                $("div.tab7").css("display", "none");
                $("div.tab8").css("display", "none");
                $("div.tab9").css("display", "none");
                $("div.tab10").css("display", "none");
                $("div.tab11").css("display", "none");
                $("div.tab12").css("display", "none");
                break;
            case "tab3":
                //change status & style menu
                $("#tab1").removeClass("active");
                $("#tab2").removeClass("active");
                $("#tab3").addClass("active");
                $("#tab4").removeClass("active");
                $("#tab5").removeClass("active");
                $("#tab6").removeClass("active");
                $("#tab7").removeClass("active");
                $("#tab8").removeClass("active");
                $("#tab9").removeClass("active");
                $("#tab10").removeClass("active");
                $("#tab11").removeClass("active");
                $("#tab12").removeClass("active");
                //display selected division, hide others
                $("div.tab3").css("display", "block");
                $("div.tab1").css("display", "none");
                $("div.tab2").css("display", "none");
                $("div.tab4").css("display", "none");
                $("div.tab5").css("display", "none");
                $("div.tab6").css("display", "none");
                $("div.tab7").css("display", "none");
                $("div.tab8").css("display", "none");
                $("div.tab9").css("display", "none");
                $("div.tab10").css("display", "none");
                $("div.tab11").css("display", "none");
                $("div.tab12").css("display", "none");
                break;
            case "tab4":
                //change status & style menu
                $("#tab1").removeClass("active");
                $("#tab2").removeClass("active");
                $("#tab3").removeClass("active");
                $("#tab4").addClass("active");
                $("#tab5").removeClass("active");
                $("#tab6").removeClass("active");
                $("#tab7").removeClass("active");
                $("#tab8").removeClass("active");
                $("#tab9").removeClass("active");
                $("#tab10").removeClass("active");
                $("#tab11").removeClass("active");
                $("#tab12").removeClass("active");
                //display selected division, hide others
                $("div.tab1").css("display", "none");
                $("div.tab2").css("display", "none");
                $("div.tab3").css("display", "none");
                $("div.tab4").css("display", "block");
                $("div.tab5").css("display", "none");
                $("div.tab6").css("display", "none");
                $("div.tab7").css("display", "none");
                $("div.tab8").css("display", "none");
                $("div.tab9").css("display", "none");
                $("div.tab10").css("display", "none");
                $("div.tab11").css("display", "none");
                $("div.tab12").css("display", "none");
                break;
            case "tab5":
                //change status & style menu
                $("#tab1").removeClass("active");
                $("#tab2").removeClass("active");
                $("#tab3").removeClass("active");
                $("#tab4").removeClass("active");
                $("#tab5").addClass("active");
                $("#tab6").removeClass("active");
                $("#tab7").removeClass("active");
                $("#tab8").removeClass("active");
                $("#tab9").removeClass("active");
                $("#tab10").removeClass("active");
                $("#tab11").removeClass("active");
                $("#tab12").removeClass("active");
                //display selected division, hide others
                $("div.tab1").css("display", "none");
                $("div.tab2").css("display", "none");
                $("div.tab3").css("display", "none");
                $("div.tab4").css("display", "none");
                $("div.tab5").css("display", "block");
                $("div.tab6").css("display", "none");
                $("div.tab7").css("display", "none");
                $("div.tab8").css("display", "none");
                $("div.tab9").css("display", "none");
                $("div.tab10").css("display", "none");
                $("div.tab11").css("display", "none");
                $("div.tab12").css("display", "none");
                break;
            case "tab6":
                //change status & style menu
                $("#tab1").removeClass("active");
                $("#tab2").removeClass("active");
                $("#tab3").removeClass("active");
                $("#tab4").removeClass("active");
                $("#tab5").removeClass("active");
                $("#tab6").addClass("active");
                $("#tab7").removeClass("active");
                $("#tab8").removeClass("active");
                $("#tab9").removeClass("active");
                $("#tab10").removeClass("active");
                $("#tab11").removeClass("active");
                $("#tab12").removeClass("active");
                //display selected division, hide others
                $("div.tab1").css("display", "none");
                $("div.tab2").css("display", "none");
                $("div.tab3").css("display", "none");
                $("div.tab4").css("display", "none");
                $("div.tab5").css("display", "none");
                $("div.tab6").css("display", "block");
                $("div.tab7").css("display", "none");
                $("div.tab8").css("display", "none");
                $("div.tab9").css("display", "none");
                $("div.tab10").css("display", "none");
                $("div.tab11").css("display", "none");
                $("div.tab12").css("display", "none");
                break;
            case "tab7":
                //change status & style menu
                $("#tab1").removeClass("active");
                $("#tab2").removeClass("active");
                $("#tab3").removeClass("active");
                $("#tab4").removeClass("active");
                $("#tab5").removeClass("active");
                $("#tab6").removeClass("active");
                $("#tab7").addClass("active");
                $("#tab8").removeClass("active");
                $("#tab9").removeClass("active");
                $("#tab10").removeClass("active");
                $("#tab11").removeClass("active");
                $("#tab12").removeClass("active");
                //display selected division, hide others
                $("div.tab1").css("display", "none");
                $("div.tab2").css("display", "none");
                $("div.tab3").css("display", "none");
                $("div.tab4").css("display", "none");
                $("div.tab5").css("display", "none");
                $("div.tab6").css("display", "none");
                $("div.tab7").css("display", "block");
                $("div.tab8").css("display", "none");
                $("div.tab9").css("display", "none");
                $("div.tab10").css("display", "none");
                $("div.tab11").css("display", "none");
                $("div.tab12").css("display", "none");
                break;
            case "tab8":
                //change status & style menu
                $("#tab1").removeClass("active");
                $("#tab2").removeClass("active");
                $("#tab3").removeClass("active");
                $("#tab4").removeClass("active");
                $("#tab5").removeClass("active");
                $("#tab6").removeClass("active");
                $("#tab7").removeClass("active");
                $("#tab8").addClass("active");
                $("#tab9").removeClass("active");
                $("#tab10").removeClass("active");
                $("#tab11").removeClass("active");
                $("#tab12").removeClass("active");
                //display selected division, hide others
                $("div.tab1").css("display", "none");
                $("div.tab2").css("display", "none");
                $("div.tab3").css("display", "none");
                $("div.tab4").css("display", "none");
                $("div.tab5").css("display", "none");
                $("div.tab6").css("display", "none");
                $("div.tab7").css("display", "none");
                $("div.tab8").css("display", "block");
                $("div.tab9").css("display", "none");
                $("div.tab10").css("display", "none");
                $("div.tab11").css("display", "none");
                $("div.tab12").css("display", "none");
                break;
            case "tab9":
                //change status & style menu
                $("#tab1").removeClass("active");
                $("#tab2").removeClass("active");
                $("#tab3").removeClass("active");
                $("#tab4").removeClass("active");
                $("#tab5").removeClass("active");
                $("#tab6").removeClass("active");
                $("#tab7").removeClass("active");
                $("#tab8").removeClass("active");
                $("#tab9").addClass("active");
                $("#tab10").removeClass("active");
                $("#tab11").removeClass("active");
                $("#tab12").removeClass("active");
                //display selected division, hide others
                $("div.tab1").css("display", "none");
                $("div.tab2").css("display", "none");
                $("div.tab3").css("display", "none");
                $("div.tab4").css("display", "none");
                $("div.tab5").css("display", "none");
                $("div.tab6").css("display", "none");
                $("div.tab7").css("display", "none");
                $("div.tab8").css("display", "none");
                $("div.tab9").css("display", "block");
                $("div.tab10").css("display", "none");
                $("div.tab11").css("display", "none");
                $("div.tab12").css("display", "none");
                break;
            case "tab10":
                //change status & style menu
                $("#tab1").removeClass("active");
                $("#tab2").removeClass("active");
                $("#tab3").removeClass("active");
                $("#tab4").removeClass("active");
                $("#tab5").removeClass("active");
                $("#tab6").removeClass("active");
                $("#tab7").removeClass("active");
                $("#tab8").removeClass("active");
                $("#tab9").removeClass("active");
                $("#tab10").addClass("active");
                $("#tab11").removeClass("active");
                $("#tab12").removeClass("active");
                //display selected division, hide others
                $("div.tab1").css("display", "none");
                $("div.tab2").css("display", "none");
                $("div.tab3").css("display", "none");
                $("div.tab4").css("display", "none");
                $("div.tab5").css("display", "none");
                $("div.tab6").css("display", "none");
                $("div.tab7").css("display", "none");
                $("div.tab8").css("display", "none");
                $("div.tab9").css("display", "none");
                $("div.tab10").css("display", "block");
                $("div.tab11").css("display", "none");
                $("div.tab12").css("display", "none");
                break;
            case "tab11":
                //change status & style menu
                $("#tab1").removeClass("active");
                $("#tab2").removeClass("active");
                $("#tab3").removeClass("active");
                $("#tab4").removeClass("active");
                $("#tab5").removeClass("active");
                $("#tab6").removeClass("active");
                $("#tab7").removeClass("active");
                $("#tab8").removeClass("active");
                $("#tab9").removeClass("active");
                $("#tab10").removeClass("active");
                $("#tab11").addClass("active");
                $("#tab12").removeClass("active");
                //display selected division, hide others
                $("div.tab1").css("display", "none");
                $("div.tab2").css("display", "none");
                $("div.tab3").css("display", "none");
                $("div.tab4").css("display", "none");
                $("div.tab5").css("display", "none");
                $("div.tab6").css("display", "none");
                $("div.tab7").css("display", "none");
                $("div.tab8").css("display", "none");
                $("div.tab9").css("display", "none");
                $("div.tab10").css("display", "none");
                $("div.tab11").css("display", "block");
                $("div.tab12").css("display", "none");
                break;
            case "tab12":
                //change status & style menu
                $("#tab1").removeClass("active");
                $("#tab2").removeClass("active");
                $("#tab3").removeClass("active");
                $("#tab4").removeClass("active");
                $("#tab5").removeClass("active");
                $("#tab6").removeClass("active");
                $("#tab7").removeClass("active");
                $("#tab8").removeClass("active");
                $("#tab9").removeClass("active");
                $("#tab10").removeClass("active");
                $("#tab11").removeClass("active");
                $("#tab12").addClass("active");
                //display selected division, hide others
                $("div.tab1").css("display", "none");
                $("div.tab2").css("display", "none");
                $("div.tab3").css("display", "none");
                $("div.tab4").css("display", "none");
                $("div.tab5").css("display", "none");
                $("div.tab6").css("display", "none");
                $("div.tab7").css("display", "none");
                $("div.tab8").css("display", "none");
                $("div.tab9").css("display", "none");
                $("div.tab10").css("display", "none");
                $("div.tab11").css("display", "none");
                $("div.tab12").css("display", "block");
                break;
        }
        //alert(e.target.id);
        return false;
    });
});