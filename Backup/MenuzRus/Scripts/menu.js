var Menu = (function () {
    var timeout = 10 * 1000;
    var stayOpened = 240 * timeout; // 4 hours
    var animationSpeed = 3 * 500;

    var $container = $('#rm-container'),
		$wrapper = $container.find('.rm-wrapper')
    init = function (settings) {
        $(".rm-wrapper").css("margin-left", settings.offsetLeft).css("margin-top", settings.offsetTop);

        $(".rm-container").fadeIn(animationSpeed);
        setTimeout(function () { openMenu(); }, timeout);
    },
	openMenu = function () {
	    $container.addClass('rm-open');
	    setTimeout(function () {
	        closeMenu();
	    }, stayOpened);
	},
	closeMenu = function () {
	    $container.removeClass('rm-open rm-nodelay rm-in');
	    setTimeout(function () {
	        openMenu();
	    }, timeout);
	    changeBackground();
	}

    return { init: init };

})();