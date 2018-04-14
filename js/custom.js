$(document).ready(function(e) {
	/* Top Menu Js Start Here */
    var pageheight = $(window).height();
    $("nav#menu").prepend("<div class='menuIcon'></div><div class='closeIcon'></div>");
    var e = "closed";
    $(".menuIcon, .closeIcon, .overlay").click(function () {
        $("nav#menu > ul").css("height", pageheight+71);
        if (e == "closed") {
            $("nav#menu > ul").css("display", "block");
            $("nav#menu > ul").css("right", "0");
			$("header").addClass('addBg');
            $("nav#menu > ul").css("overflow-y", "scroll");
            $(".overlay").fadeIn();
            $("html, body").css("overflow", "hidden");
            $(".closeIcon").fadeIn();
            e = "opened"
        }
        else if (e == "opened") {
            e = "closed";
            $(".overlay").fadeOut();
			$("header").removeClass('addBg');
            $("nav#menu > ul").css("right", "-84%");
            $("html, body").css("overflow-y", "scroll");
            $(".closeIcon").hide();
        }
    });	
	/* Top Menu Js End Here */
	$('nav#menu ul li a').click(function(){
		$(this).next().slideToggle();
		$(this).toggleClass('active');
	});
});