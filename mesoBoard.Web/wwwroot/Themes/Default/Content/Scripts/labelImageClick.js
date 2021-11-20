window.onload = function () {
    if (document.all && navigator.appVersion.indexOf("MSIE") > -1 && navigator.appVersion.indexOf("Windows") > -1) {
        var a = document.getElementsByTagName("label");
        for (var i = 0, j = a.length; i < j; i++) {
            if (a[i].hasChildNodes && a[i].childNodes.item(0).tagName == "IMG") {
                a[i].childNodes.item(0).forid = a[i].htmlFor;
                a[i].childNodes.item(0).onclick = function () {
                    var e = document.getElementById(this.forid);
                    switch (e.type) {
                        case "radio": e.checked |= 1; break;
                        case "checkbox": e.checked = !e.checked; break;
                        case "text": case "password": case "textarea": e.focus(); break;
                    }
                }
            }
        }
    }
}