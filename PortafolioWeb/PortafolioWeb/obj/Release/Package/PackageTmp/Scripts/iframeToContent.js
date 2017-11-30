$(function () {

    var iFrames = $('iframe');

    function iResize() {

        for (var i = 0, j = iFrames.length; i < j; i++) {
            iFrames[i].style.height = iFrames[i].contentWindow.document.body.offsetHeight + 'px';
        }
    }

    var browser = {
        chrome: false,
        mozilla: false,
        opera: false,
        msie: false,
        safari: false
    };

    var sUsrAg = navigator.userAgent;
    if (!sUsrAg.indexOf("Opera") > -1 || !sUsrAg.indexOf("Safari") > -1) {
        browser.opera = true;
        browser.safari = true;
    }

    if (browser.safari || browser.opera) {

        iFrames.load(function () {
            setTimeout(iResize, 0);
        });

        for (var i = 0, j = iFrames.length; i < j; i++) {
            var iSource = iFrames[i].src;
            iFrames[i].src = '';
            iFrames[i].src = iSource;
        }

    } else {
        iFrames.load(function () {
            this.style.height = this.contentWindow.document.body.offsetHeight + 'px';
        });
    }

});
