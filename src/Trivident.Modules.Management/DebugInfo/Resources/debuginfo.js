document.addEventListener('DOMContentLoaded', function () {
    console.log('attaching click event to element with id dyndle-mgmt-debuginfo');
    var debuginfo = getCookie('debuginfo');
    var buttons = document.getElementsByClassName('dyndle-mgmt-debuginfo-btn');
    for (var i = 0; i < buttons.length; i++) {
        buttons[i].addEventListener('click', toggleDebug);
        var debuginfoName = getDataAttributeFromElement(buttons[i], 'debuginfoName');
        console.log('found ' + debuginfoName);
        if (debuginfo.includes(debuginfoName)) {
            buttons[i].style.filter = 'brightness(185%)';
        }
        //filter: invert(1);
    }
});

function toggleDebug() {
    var targetElement = event.target || event.srcElement;
    var debuginfoName = getDataAttributeFromElement(targetElement, 'debuginfoName');
    if (debuginfoName == null) {
        console.log('warning: called toggleDebug but target element has no "data-debuginfo-name" attribute');
        return;
    }
    var debuginfoLocation = getDataAttributeFromElement(targetElement, 'debuginfoLocation');
    console.log('toggleDebug is called for debug info name ' + debuginfoName + ' and location ' + debuginfoLocation);
    if (debuginfoLocation != null && debuginfoLocation != '') {
        window.open(debuginfoLocation, 'win_' + debuginfoname);
        return;
    }
    var debuginfo = getCookie('debuginfo');
    if (debuginfo.includes(debuginfoName)) {
        debuginfo = debuginfo.replace('/' + debuginfoName, '');
    } else {
        debuginfo += '/' + debuginfoName;
    }
    setCookie('debuginfo', debuginfo);
    window.location.reload();
}

function getDataAttributeFromElement(elmt, name) {
    var value = elmt.dataset[name];
    if (value == null) {
        value = elmt.parentElement.dataset[name];
    }
    return value;
}

const setCookie = (name, value) => {
    document.cookie = name + '=' + encodeURIComponent(value) + ';'
}

const getCookie = (name) => {
    return document.cookie.split('; ').reduce((r, v) => {
        const parts = v.split('=')
        return parts[0] === name ? decodeURIComponent(parts[1]) : r
    }, '')
}

const deleteCookie = (name, path) => {
    setCookie(name, '', -1, path)
}
