var Menu = (function () {
    var timeout = 10 * 1000;
    var mins = 120 // 2 hours
    var stayOpened = mins * timeout;
    var animationSpeed = 3 * 500;

    var $container = $('#rm-container'),
		$wrapper = $container.find('.rm-wrapper')
    init = function () {
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
	}

    return { init: init };
})();