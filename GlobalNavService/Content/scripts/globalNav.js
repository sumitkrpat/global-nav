(function () {

    var DATA = window.__globalNav__;

    function appendCSS(url) {
        document.write("<link rel='stylesheet' href='" + DATA.StyleGuideRoot + url + "' />");
    }

    function appendJS(url) {
        if (url === "/includes.js") {
            var path = DATA.StyleGuideRoot.split("/");
            path.pop();

            path = path.join("/");

            document.write("<script src='" + path + url + "'></script>");
        } else if (url === "/gn/scripts/globalNavAPI.js") {
            var path = DATA.StyleGuideRoot.split("/");
            path.pop();
            path.pop();

            path = path.join("/");

            document.write("<script src='" + path + url + "'></script>");
        } else {
            document.write("<script src='" + DATA.StyleGuideRoot + url + "'></script>");
        }
    }

    function readCookie(name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }

    window._CDNRoot = DATA.StyleGuideRoot;

    function includeResources() {
        var css = [];
        var js = [];

        if (document.all && !document.addEventListener) {
            css.push("../styles/ie8/globalNav.css");
        } else if (document.all && !window.atob) {
            css.push("../styles/ie9/globalNav.css");
        } else if (document.all) {
            css.push("../styles/ie10/globalNav.css");
        }

        if (DATA.StyleGuideVersion <= "1.3.2") {
            var isThirdStyleGuide = DATA.StyleGuideVersion == "1.3.2";
            var bootstrapLib = isThirdStyleGuide ? "/styles/bootstrap.css" : "/styles/bootstrap.min.css";

            css = [
                bootstrapLib,
                "/styles/acxiom.base.css",
                "/styles/acxiom.ui.global.navigation.css",
                "/styles/acxiom.ui.dialogue.css"
            ];


            // IE
            if (document.all) {
                css.push("/styles/IEstyle.css");
            }

            //IE <= 8
            if (document.all && !document.addEventListener) {
                css.push("/styles/IE8style.css");
            }

            var jqueryLib = isThirdStyleGuide ? "/libraries/jquery/jquery.js" : "/libraries/jquery/jquery-1.7.2.min.js";

            js = [
                "/gn/scripts/globalNavAPI.js",
                jqueryLib
            ];

            if (isThirdStyleGuide) {
                js.push("/libraries/jquery/jquery.plugin.migrate.js");
                js.push("/js/ACXM.js");
                js.push("/js/acxiom.ui.nav.js");
            }

            js.push("/js/acxiom.ui.base.js");
            js.push("/js/acxiom.global.navigation.js");
            js.push("/js/acxiom.dialogue.js");

            var pageLoad = function () {
                if (window.ACXM && window.ACXM.Nav) {
                    window.ACXM.Nav.init();
                }

                var globalNav = document.getElementById("acx-global-nav");
                window.__draw(globalNav, DATA);

                if (window.ACXM) {
                    window.ACXM.GlobalNav.init();
                }

                function listener(event) {
                    if (event.data === 'relogin' && getDomain(event.origin) === getDomain(window.location.hostname))
                        window.location = GlobalNav.loginUrl;

                    function getDomain(subdomain) {
                        var parts = subdomain.split('.');
                        parts.shift();
                        return parts.join('.');
                    }
                }

                if (window.addEventListener) {
                    addEventListener("message", listener, false);
                } else {
                    attachEvent("onmessage", listener);
                }
            };

            if (!window.addEventListener) {
                window.attachEvent("onload", pageLoad);
            } else {
                window.addEventListener("load", pageLoad, false);
            }

        } else if (DATA.StyleGuideVersion >= "1.5") {
            css = [];

            js = [
                "/includes.js",
                "/gn/scripts/globalNavAPI.js",
                "/assets/scripts/jquery.js"
            ];

            // IE
            if (document.all) {
                js.push("/assets/scripts/legacy/_ie/ecma.js");
            }

            js.push("/assets/scripts/ACXM.js");
            js.push("/assets/scripts/loadAPI.js");
        }

        for (var i = 0; i < css.length; ++i) {
            appendCSS(css[i]);
        }

        var path = DATA.StyleGuideRoot.split("/");
        path.pop();
        path.pop();

        path = path.join("/");

        document.write("<link rel='stylesheet' href='" + path + "/gn/styles/globalNav.css' />");

        for (var k = 0; k < js.length; ++k) {
            appendJS(js[k]);
        }
    }

    includeResources();

    DATA.IsNewGN = readCookie("GN2.0") !== null ? true : false;

    if (DATA.IsNewGN) {

        DATA.ViewHelp = DATA.IsHelp;

        document.write('<link href="https://fonts.googleapis.com/css?family=Raleway:700,300,500" rel="stylesheet" type="text/css">');

        //#region Object Methods
        Object.extend = function (d, s, copyFunctions) {
            if (copyFunctions == null) copyFunctions = false;
            if (typeof (s) == "function") return;
            var type = "";
            var p = null;
            for (p in s) {
                type = typeof (s[p]);
                try {
                    if (type == "object") {
                        if (s[p].length != null) {
                            d[p] = s[p];
                        } else {
                            d[p] = Object.extend(d[p], s[p]);
                        }
                    } else if (type == "function") {
                        if (copyFunctions == true) {
                            d[p] = s[p];
                        }
                    } else {
                        d[p] = s[p];
                    }
                } catch (err) {
                    ;
                }
            }
            return d;
        };
//Object.extend = function(d, s) { for (p in s) { d[p] = s[p]; } return d; };
//#endregion
        var is_ie = (/msie/i.test(navigator.userAgent) && !/opera/i.test(navigator.userAgent));
        var is_ie5 = (is_ie && /msie 5\.0/i.test(navigator.userAgent));
        var is_ie6 = (is_ie && /msie 6/i.test(navigator.userAgent));
        var is_ie7 = (is_ie && /msie 7/i.test(navigator.userAgent));
        var is_ie8 = (is_ie && /msie 8/i.test(navigator.userAgent));
        var is_ie8 = (is_ie && /msie 8/i.test(navigator.userAgent));

        function $N(tagName, domProperties, children) {
            if (domProperties != null && domProperties.isArray) {
                children = domProperties;
                domProperties = null;
            }

            var t = document.createElement(tagName);

            //IE7 and down have serious issues with this function. We have to set the name property
            //and id properties via the create Element because why???? Normal browsers will just let
            //you set them via typical setting like Element.name = <name>. IE (any maj ver up to 7 so far)
            //for some unknown reason is not smart enough to do this
            if ((is_ie6 || is_ie7 || is_ie8) && domProperties != null) {
                t = document.createElement("<" + tagName +
                    ((domProperties.name != null) ? " name=\"" + domProperties.name + "\"" : "") +
                    ((domProperties.id != null) ? " id=\"" + domProperties.id + "\"" : "") +
                    ">");
            }

            if (domProperties != null) {
                Object.extend(t, domProperties);

                //Stupid opera will not let you assign the style object via an object.extend so we ahve to go through it manually
                if (domProperties.style != null) {
                    for (var style in domProperties.style) {
                        t.style[style] = domProperties.style[style];
                    }

                    //the browsers will not honor "float" as a dom property as a style.  Of course IE has one way
                    //of naming this and the rest of the world has another way
                    if (domProperties.style["float"] != null) {
                        t.style.styleFloat = domProperties.style["float"]; //IE
                        t.style.cssFloat = domProperties.style["float"]; //the rest of the world
                    }
                }
            }

            if (children != null && !isNaN(children.length)) {
                for (var i = 0; i < children.length; i++) {
                    if (typeof (children[i]) == "string")
                        t.appendChild($TEXT(children[i]));
                    else
                        t.appendChild(children[i]);
                }
            }
            return t;
        }

        function formatUrl(url) {
            return url.replace(new RegExp("([^:]/)/+", "g"), "$1");
        }

        function getGlobalNavResource(content) {
            var path = DATA.StyleGuideRoot.split("/");
            path.pop();
            path.pop();

            path = path.join("/");

            return formatUrl(path + "/gn/" + content);
        }

        function getIconUrl(iconUrl) {
            var path = DATA.StyleGuideRoot.split("/");
            path.pop();
            path.pop();

            path = path.join("/");

            return formatUrl(path + "/productIcons/" + iconUrl);
        }

        var pageLoad = function () {

            GlobalNav.DefaultHelpUrl = DATA.gn.DefaultHelpUrl;
            GlobalNav.appName = DATA.gn.appName;
            GlobalNav.environment = DATA.gn.environment;
            GlobalNav.loginUrl = DATA.gn.loginUrl;

            GlobalNav.nav = new NavObject(DATA.SelectedLevelZeroItem.Name, DATA.SelectedLevelZeroItem.IconURL, DATA.SelectedLevelOneItem ? DATA.SelectedLevelOneItem.Name : "");
            GlobalNav.events.dispatchEvent('ready');

            document.getElementById('acx-global-nav').appendChild(
                $N('div', {id: 'acx-new-gn'}, [
                    $N('div', {id: 'acx-global-nav-bar'}, [
                        $N('div', {
                            className: 'acx-new-gn-branding'
                        }, [
                            $N('a', {
                                href: DATA.AcxiomCorporateUrl
                            }, [
                                $N('img', {
                                    src: getGlobalNavResource("/images/gn-logo.png")
                                })
                            ])
                        ]),
                        $N('div', {
                            id: 'acx-new-gn-app-selection'
                        }, [
                            $N('div', {id: 'acx-new-gn-current-app'}, [
                                $N('h2', null, [
                                    document.createTextNode(DATA.SelectedLevelZeroItem.DisplayName)
                                ])
                            ])
                        ]),
                        $N('div', {
                            id: 'acx-new-gn-app-links'
                        }),
                        $N('div', {id: 'acx-new-gn-right-nav', style: { display: DATA.LoggedUser ? 'block' : 'none' }}, [
                            $N('div', {id: 'acx-new-gn-right-nav-controls'}, [
                                $N('div', {id: 'acx-new-gn-user'}, [
                                    $N('div', {
                                        id: 'acx-new-gn-user-icon', className: 'acx-new-gn-right-nav-control'
                                    }, [
                                        $N('img', {src: getGlobalNavResource('images/gn-user.png')})
                                    ])
                                ]),
                                $N('div', {
                                    id: 'acx-new-gn-help',
                                    style: {display: DATA.ViewHelp ? 'block' : 'none'}
                                }, [
                                    $N('div', {
                                        id: 'acx-new-gn-help-icon', className: 'acx-new-gn-right-nav-control'
                                    }, [
                                        $N('img', {src: getGlobalNavResource('images/gn-help.png')})
                                    ])
                                ]),
                                $N('div', {
                                    id: 'acx-new-gn-settings',
                                    style: {display: DATA.ViewSettings ? 'block' : 'none'}
                                }, [
                                    $N('div', {
                                        id: 'acx-new-gn-settings-icon', className: 'acx-new-gn-right-nav-control'
                                    }, [
                                        $N('img', {src: getGlobalNavResource('images/gn-cog.png')})
                                    ])
                                ]),
                                $N('div', {id: 'acx-new-gn-search', style: { display: DATA.ViewSearch ? 'block' : 'none' }}, [
                                    $N('form', { id: 'acx-new-gn-search-form' }, [
                                        $N('div', {
                                            id: 'acx-new-gn-search-field'
                                        }, [
                                            $N('input', {
                                                id: 'acx-new-gn-search-field-input',
                                                className: 'form-control',
                                                placeholder: 'Search'
                                            })
                                        ]),
                                        $N('button', {
                                            type: 'submit',
                                            id: 'acx-new-gn-search-icon',
                                            className: 'acx-new-gn-right-nav-control'
                                        }, [
                                            $N('img', {src: getGlobalNavResource('images/gn-search.png')})
                                        ])
                                    ])
                                ])
                            ])
                        ])
                    ]),
                    $N('div', {id: 'acx-global-nav-bar-divider'}, [
                        $N('h3', null, [
                            document.createTextNode(DATA.LoggedUser ? DATA.SelectedLevelOneItem.DisplayName : '')
                        ])
                    ]),
                    $N('div', {id: 'acx-new-gn-context'}, [
                        $N('div', {
                            id: 'acx-new-gn-available-apps',
                            className: 'acx-new-gn-context',
                            style: {display: DATA.LevelZeroItems && DATA.LevelZeroItems.length ? 'block' : 'none' }
                        }, [
                            $N('div', {className: 'acx-new-gn-user-context-section'}, [
                                $N('ul')
                            ])
                        ]),
                        $N('div', {id: 'acx-new-gn-user-context', className: 'acx-new-gn-context'}, [
                            $N('div', {className: 'acx-new-gn-user-context-section'}, [
                                $N('div', {className: 'acx-new-gn-context-label'}, [
                                    $N('label', {}, [
                                        document.createTextNode('Welcome' + (DATA.LoggedUser ? ', ' + DATA.LoggedUser.Name : ''))
                                    ])
                                ])
                            ]),
                            $N('div', {className: 'acx-new-gn-user-context-section'}, [
                                $N('div', {id: 'acx-new-gn-tenants'}, [
                                    $N('div', {className: 'acx-new-gn-context-label'}, [
                                        $N('label', {}, [
                                            document.createTextNode('Tenants')
                                        ])
                                    ]),
                                    $N('div', {id: 'acx-new-gn-available-tenants'})
                                ])
                            ]),
                            $N('div', {className: 'acx-new-gn-user-context-section'}, [
                                $N('div', {id: 'acx-new-gn-controls'}, [
                                    $N('a', {href: DATA.LogInUrl, style: { display: DATA.LoggedUser ? 'none' : 'block' }}, [
                                        document.createTextNode('Sign In')
                                    ]),
                                    $N('a', {href: DATA.LogOutUrl, style: { display: DATA.LoggedUser ? 'block' : 'none' }}, [
                                        document.createTextNode('Sign Out')
                                    ]),
                                    $N('a', {href: DATA.AccountsUrl, style: { float: 'left', display: DATA.LoggedUser ? 'block' : 'none' }}, [
                                        document.createTextNode('Account')
                                    ])
                                ])
                            ])
                        ])
                    ])
                ])
            );

            document.getElementById('acx-new-gn-search-form').onsubmit = function() { return false };

            for (var i in DATA.LevelZeroItems) {
                document.getElementById('acx-new-gn-available-apps').getElementsByTagName('UL')[0].appendChild(
                    $N('li', null, [
                        $N('a', {className: DATA.LevelZeroItems[i].Id === DATA.SelectedLevelZeroItem.Id ? 'acx-new-gn-context-section-item acx-new-gn-active' : 'acx-new-gn-context-section-item', href: DATA.LevelZeroItems[i].URL}, [
                            $N('img', {
                                src: getIconUrl(DATA.LevelZeroItems[i].IconURL)
                            }),
                            $N('label', null, [
                                document.createTextNode(DATA.LevelZeroItems[i].Name)
                            ])
                        ])
                    ])
                );
            }

            for (var i in DATA.LevelOneItems) {
                var className = DATA.LevelOneItems[i].Id === DATA.SelectedLevelOneItem.Id ? 'acx-new-gn-active' : '';
                var item = document.getElementById('acx-new-gn-app-links').appendChild(
                    $N('a', {
                        href: DATA.LevelOneItems[i].URL,
                        className: className
                    }, [document.createTextNode(DATA.LevelOneItems[i].DisplayName)]));
                GlobalNav.links.register(DATA.LevelOneItems[i].Name, item);
            }

            if (DATA.IsHelp) {
                GlobalNav.helpItems = DATA.HelpItems;
            }

            function checkForClassName(el, className) {
                return (' ' + el.className + ' ').indexOf(' ' + className + ' ') != -1;
            }

            function toggleElement(elementId, className, deactivate) {
                if (!className) {
                    className = 'acx-new-gn-active';
                }
                var div = document.getElementById(elementId);
                if (checkForClassName(div, className)) {
                    div.className = div.className.replace(new RegExp('\\b' + className + '\\b', 'g'), '');
                } else if (!deactivate) {
                    div.className += ' ' + className;
                }
            };

            document.getElementById('acx-new-gn-user-icon').onclick = function () {
                toggleElement('acx-new-gn-user-context');
                toggleElement('acx-new-gn-available-apps', null, true);
            };

            document.getElementById('acx-new-gn-search-icon').onclick = function () {
                var div = document.getElementById('acx-new-gn-search-field-input');
                if (checkForClassName(div, 'acx-new-gn-active')) {
                    if(div.value !== '') {
                        window.postMessage(div.value, window.location.href);
                        DATA.SearchText = div.value;
                    } else {
                        div.className = div.className.replace(new RegExp('\\bacx-new-gn-active\\b', 'g'), '');
                    }
                } else {
                    div.className += ' acx-new-gn-active';
                }
                toggleElement('acx-new-gn-user-context', null, true);
                toggleElement('acx-new-gn-available-apps', null, true);
            };

            document.getElementById('acx-new-gn-help-icon').onclick = GlobalNav.helpClick;
            document.getElementById('acx-new-gn-settings-icon').onclick = GlobalNav.settingsClick;

            //for (var i in notifications) {
            //    document.getElementById('acx-new-gn-notifications').appendChild(
            //        $N('div', {className: 'acx-new-gn-notification acx-new-gn-context-section-item'}, [
            //            document.createTextNode(notifications[i].message)
            //        ])
            //    );
            //}

            function orderTenants() {
                for (var i in DATA.LoggedUser.Companies) {
                    if (DATA.LoggedUser.Companies[i].Id === DATA.LoggedUser.WorkingCompanyId) {
                        document.getElementById('acx-new-gn-available-tenants').appendChild(
                            $N('a', {
                                href: DATA.LoggedUser.Companies[i].Url,
                                className: 'acx-new-gn-context-section-item acx-new-gn-active'
                            }, [
                                document.createTextNode(DATA.LoggedUser.Companies[i].DisplayName)
                            ])
                        );
                    }
                }
                for (var i in DATA.LoggedUser.Companies) {
                    if (DATA.LoggedUser.Companies[i].Id !== DATA.LoggedUser.WorkingCompanyId) {
                        document.getElementById('acx-new-gn-available-tenants').appendChild(
                            $N('a', {
                                href: DATA.LoggedUser.Companies[i].Url,
                                className: 'acx-new-gn-context-section-item'
                            }, [
                                document.createTextNode(DATA.LoggedUser.Companies[i].DisplayName)
                            ])
                        );
                    }
                }
            }

            if(DATA.LoggedUser && DATA.LoggedUser.Companies.length) {
                orderTenants();
            }

            document.getElementById('acx-new-gn-available-apps').style.top = "-" + (parseInt(document.getElementById('acx-new-gn-available-apps').offsetHeight) - 22) + "px";
            document.getElementById('acx-new-gn-user-context').style.top = "-" + (parseInt(document.getElementById('acx-new-gn-user-context').offsetHeight) - 32) + "px";

            if(document.getElementsByClassName('acxiom-page-container').length) {
                document.getElementsByClassName('acxiom-page-container')[0].onclick = function () {
                    toggleElement('acx-new-gn-available-apps', null, true);
                    toggleElement('acx-new-gn-user-context', null, true);
                    toggleElement('acx-new-gn-search-field-input', null, true);
                };
            } else if(document.getElementById('content')) {
                document.getElementById('content').onclick = function () {
                    toggleElement('acx-new-gn-available-apps', null, true);
                    toggleElement('acx-new-gn-user-context', null, true);
                    toggleElement('acx-new-gn-search-field-input', null, true);
                };
            }

            document.getElementById('acx-new-gn-current-app').onclick = function () {
                toggleElement('acx-new-gn-available-apps');
                toggleElement('acx-new-gn-user-context', null, true);
            };
        }

        if (!window.addEventListener) {
            window.attachEvent("onload", pageLoad);
        } else {
            window.addEventListener("load", pageLoad, false);
        }
    } else {
        (function () {

            function slide_(options) {
                var duration = options.duration || 300;
                var finalHeight = options.height;

                var inc = 1000 / duration;

                var currentHeight = parseInt(window.getComputedStyle ? window.getComputedStyle(options.element).getPropertyValue('height') : options.element.clientHeight);

                options.element.style.overflow = "hidden";

                if (options.isUp) {
                    finalHeight = 0;
                } else {
                    currentHeight = 0;
                    options.element.style.height = currentHeight + "px";
                    options.element.style.display = "block";
                    finalHeight = finalHeight || options.element.scrollHeight;
                }

                var heightResult = Math.abs(currentHeight - finalHeight);

                var _slide = function () {
                    if (currentHeight !== finalHeight) {
                        var diff = Math.abs(currentHeight - finalHeight);
                        if (diff < inc) {
                            inc = diff;
                        }

                        currentHeight += (options.isUp ? -inc : +inc);
                        options.element.style.height = currentHeight + 'px';
                        setTimeout(_slide, duration / (heightResult / inc));
                    } else {
                        if (options.isUp) {
                            options.element.style.block = "none";
                        }

                        if (options.callback) {
                            options.callback();
                        }
                    }
                };

                _slide();
            }

            function slideUp(options) {
                options.isUp = true;

                slide_(options);
            }

            function slideDown(options) {
                slide_(options);
            }

            if (DATA && DATA.IsIframeMode) {
                document.getElementsByTagName("html")[0].className += " acx-frame-mode";
            }

            if (DATA.StyleGuideVersion >= "1.5") {
                var path = DATA.StyleGuideRoot.split("/");
                path.pop();
                path.pop();

                path = path.join("/");
                document.write('<script src="' + path + '/gn/scripts/exec.js"></script>');
            }

            window.__draw = function (gnContainer, data) {

                if (data.IsIframeMode) {
                    drawIframeMode();
                } else {
                    drawFullMode();
                }

                GlobalNav.DefaultHelpUrl = window.__globalNav__.gn.DefaultHelpUrl;
                GlobalNav.appName = window.__globalNav__.gn.appName;
                GlobalNav.environment = window.__globalNav__.gn.environment;
                GlobalNav.loginUrl = window.__globalNav__.gn.loginUrl;

                GlobalNav.nav = new NavObject(data.SelectedLevelZeroItem.Name, data.SelectedLevelZeroItem.IconURL, data.SelectedLevelOneItem ? data.SelectedLevelOneItem.Name : "");
                GlobalNav.events.dispatchEvent('ready');

                // GlobalNav mode=iframe
                function drawIframeMode() {
                    gnContainer.className = "acx-global-navigation-frame";

                    if (data.LoggedUser != null) {
                        var label = gnContainer.appendChild(document.createElement("div"));
                        label.className = "acx-frame-label";
                        label.appendChild(document.createTextNode(data.SelectedLevelZeroItem.DisplayName));
                    }

                    var logo = gnContainer.appendChild(document.createElement("div"));
                    logo.className = "acx-frame-logo";

                    var logoLink = logo.appendChild(document.createElement("a"));
                    logoLink.target = "_blank";
                    logoLink.href = data.AcxiomCorporateUrl;

                    var logoImg = logoLink.appendChild(document.createElement("img"));
                    logoImg.src = getContentUrl(DATA.StyleGuideVersion >= "1.5" ? "/assets/images/logo_dark.png" : "/styles/img/logo_dark.png");

                    // Help button
                    if (data.IsHelp) {
                        GlobalNav.helpItems = data.HelpItems;

                        var help = gnContainer.appendChild(document.createElement("div"));
                        help.className = "acx-topnav-help";
                        help.appendChild(document.createTextNode("?"));
                        help.style.lineHeight = help.clientHeight + "px";
                        help.onclick = function () {
                            return GlobalNav.helpClick();
                        };

                        logo.style.marginRight = "55px";
                    }
                }

                // Standard GlobalNav
                function drawFullMode() {
                    var isEmptyHeader = data.Error != null;
                    var isFullHeader = data.LoggedUser != null;

                    GlobalNav.helpItems = data.HelpItems;

                    gnContainer.className = "acx-global-navigation " + getSchemaColorClass();

                    if (!data.IsLevelZeroMode) {
                        gnContainer.appendChild(document.createElement("div")).className = "acx-background-color-bar";
                    }

                    var navContainer = gnContainer.appendChild(document.createElement("div"));
                    navContainer.className = "acx-nav-container";


                    // level 0 bar
                    (function () {
                        var navInner = navContainer.appendChild(document.createElement("div"));
                        navInner.className = "acx-nav-inner";

                        // level0 links and tenants are visible only on the FullHeader
                        if (isFullHeader) {
                            var navInnerLeft = navInner.appendChild(document.createElement("div"));
                            navInnerLeft.className = "acx-nav-inner-left-first";

                            var settingsLogo = document.createElement("img");
                            settingsLogo.src = getContentUrl("../../gn/images/settings.png");

                            // level0 links
                            if (data.LevelZeroItems.length) {
                                var l0Logos = navInnerLeft.appendChild(document.createElement("div"));
                                l0Logos.className = "acx-div-logo-small";

                                for (var i = 0; i < data.LevelZeroItems.length; i++) {
                                    var logo = l0Logos.appendChild(document.createElement("img"));
                                    logo.src = getIconUrl(data.LevelZeroItems[i].IconURL);
                                }
                            }

                            // Tenants dropdown
                            if (data.IsTenant) {
                                var tenantContainer = navInnerLeft.appendChild(document.createElement("div"));
                                tenantContainer.className = "acx-nav-inner-left";

                                var tenants = tenantContainer.appendChild(document.createElement("div"))
                                    .appendChild(document.createElement("ul"))
                                    .appendChild(document.createElement("li"));

                                var wcLink = tenants.appendChild(document.createElement("a"));
                                wcLink.href = "#";

                                var wcName = wcLink.appendChild(document.createElement("span"));
                                wcName.className = "current-selection";
                                wcName.title = data.LoggedUser.WorkingCompanyName;
                                wcName.appendChild(document.createTextNode(data.LoggedUser.WorkingCompanyName));

                                if (data.LoggedUser.Companies.length != 1) {
                                    wcLink.appendChild(settingsLogo);

                                    var tenantList = tenants.appendChild(document.createElement("ul"));
                                    tenantList.className = "acx-scroll-content acx-globalnav-scroll-content main-list";

                                    for (var j = 0; j < data.LoggedUser.Companies.length; j++) {
                                        var company = data.LoggedUser.Companies[j];

                                        var tenantElement = tenantList.appendChild(document.createElement("li"));
                                        if (company.Id == data.LoggedUser.WorkingCompanyId) {
                                            tenantElement.className = "active";
                                        }
                                        var tenantLink = tenantElement.appendChild(document.createElement("a"));
                                        tenantLink.href = company.Url;
                                        tenantLink.appendChild(document.createTextNode(company.DisplayName));
                                    }
                                }
                            }
                        }

                        // UserInfo(FullHeader) || SignIn button(EmptyHeader)
                        if (isEmptyHeader || isFullHeader) {
                            var navInnerRight = navInner.appendChild(document.createElement("div"));
                            navInnerRight.className = "acx-nav-inner-right";

                            if (!isEmptyHeader) {
                                var userInfo = navInnerRight.appendChild(document.createElement("div"))
                                    .appendChild(document.createElement("ul"))
                                    .appendChild(document.createElement("li"));

                                var welcome = document.createElement("span");
                                welcome.className = "acx-global-welcome";
                                welcome.appendChild(document.createTextNode(DATA.LocalityData.fullHeader.welcome));

                                var userLink = userInfo.appendChild(document.createElement("a"));
                                userLink.href = "#";
                                userLink.appendChild(welcome);
                                userLink.appendChild(document.createTextNode(", " + data.LoggedUser.Name));
                                userLink.appendChild(settingsLogo.cloneNode(true));

                                var userOptions = userInfo.appendChild(document.createElement("ul"));

                                var accountSettings = userOptions.appendChild(document.createElement("li"))
                                    .appendChild(document.createElement("a"));
                                accountSettings.href = data.AccountsUrl;
                                accountSettings.appendChild(document.createTextNode(DATA.LocalityData.fullHeader.accountSettings));

                                var customerSupport = userOptions.appendChild(document.createElement("li"))
                                    .appendChild(document.createElement("a"));
                                customerSupport.href = data.TechSupportUrl;
                                customerSupport.target = "_blank";
                                customerSupport.appendChild(document.createTextNode(DATA.LocalityData.fullHeader.customerSupport));

                                var signOut = userOptions.appendChild(document.createElement("li"))
                                    .appendChild(document.createElement("a"));
                                signOut.href = data.LogOutUrl;
                                signOut.appendChild(document.createTextNode(DATA.LocalityData.fullHeader.signOut));
                                signOut.onclick = function () {
                                    return GlobalNav.events.dispatchEvent('logout', {url: data.LogOutUrl});
                                };
                            } else {
                                var signInLink = navInnerRight.appendChild(document.createElement("a"));
                                signInLink.href = data.LogInUrl;
                                signInLink.appendChild(document.createTextNode(DATA.LocalityData.emptyHeader.signIn));
                                signInLink.className = "acx-globalnav-sign-in";
                            }
                        }
                    })();

                    // Expanded level0 panel
                    if (isFullHeader) {
                        var companyTitle = navContainer.appendChild(document.createElement("span"));
                        companyTitle.className = "acx-company-title";
                        companyTitle.style.zIndex = 100;


                        (function () {
                            var largeL0 = navContainer.appendChild(document.createElement("div"));
                            largeL0.className = "acx-global-menu demo";

                            var items = largeL0.appendChild(document.createElement("div"));
                            items.className = "acx-div-logo-large";

                            for (var i = 0; i < data.LevelZeroItems.length; i++) {
                                var item = data.LevelZeroItems[i];
                                var itemLink = items.appendChild(document.createElement("a"));
                                itemLink.href = item.URL;
                                itemLink.onclick = function () {
                                    var url = item.URL;
                                    var name = item.Name;
                                    var iconUrl = item.IconURL;
                                    return function () {
                                        return GlobalNav.events.dispatchEvent('navClick', {
                                            url: url,
                                            nav: new NavObject(name, iconUrl)
                                        });
                                    };
                                }();

                                var itemLogo = itemLink.appendChild(document.createElement("img"));
                                itemLogo.src = getIconUrl(item.IconURL);
                                itemLogo.setAttribute("data-title", item.Name);

                                items.appendChild(document.createElement("div")).className = "acx-img-separator";
                            }

                            var trayClose = largeL0.appendChild(document.createElement("div")).appendChild(document.createElement("img"));
                            trayClose.className = "tray-close-button";
                            trayClose.src = getGlobalNavResource("/images/trayclose.png");
                        })();
                    }

                    //level1 bar
                    if (!data.IsLevelZeroMode) {
                        (function () {
                            var pageContainer = navContainer.appendChild(document.createElement("div"));
                            pageContainer.className = "acx-page-container";

                            var siteNavigation = pageContainer.appendChild(document.createElement("div"));
                            siteNavigation.className = "acx-site-navigation";

                            var topnavMain = siteNavigation.appendChild(document.createElement("div"));
                            topnavMain.className = "acx-topnav-main";

                            var topnavLogo = topnavMain.appendChild(document.createElement("div"));
                            topnavLogo.className = "acx-topnav-logo";
                            topnavLogo.title = DATA.LocalityData.general.acxiomSiteTitle;

                            var logoLink = topnavLogo.appendChild(document.createElement("a"));
                            logoLink.target = "_blank";
                            logoLink.href = data.AcxiomCorporateUrl;

                            var logoContent = logoLink.appendChild(document.createElement("div"));

                            var logoImg = logoContent.appendChild(document.createElement("img"));
                            logoImg.src = getGlobalNavResource("/images/logo.png");

                            logoContent.appendChild(document.createElement("span")).appendChild(document.createTextNode("TM"));

                            if (data.IsCorrectData && data.LoggedUser) {
                                topnavLogo.appendChild(document.createElement("label"))
                                    .appendChild(document.createTextNode(data.SelectedLevelZeroItem.DisplayName));
                            }

                            // level1 tabs
                            if (data.LevelOneItems) {
                                var topnavTabs = topnavMain.appendChild(document.createElement("div"));
                                topnavTabs.className = "acx-topnav-tabs";

                                if (!(data.LevelOneItems.length == 1 && data.SelectedLevelOneItem.Id == data.LevelOneItems[0].Id)) {
                                    for (var i = 0; i < data.LevelOneItems.length; i++) {
                                        var item = data.LevelOneItems[i];
                                        var itemLink = topnavTabs.appendChild(document.createElement("a"));
                                        itemLink.href = item.URL;
                                        itemLink.onclick = function () {
                                            var url = item.URL;
                                            var name = item.Name;
                                            return function () {
                                                return GlobalNav.events.dispatchEvent('navClick', {
                                                    url: url,
                                                    nav: new NavObject(data.SelectedLevelZeroItem.Name, data.SelectedLevelZeroItem.IconURL, name)
                                                });
                                            };
                                        }();


                                        var itemContent = itemLink.appendChild(document.createElement("div"));
                                        itemContent.title = item.DisplayName;
                                        itemContent.appendChild(document.createTextNode(cropText(item.DisplayName)));
                                        GlobalNav.links.register(item.Name, itemContent);
                                        if (item.Id == data.SelectedLevelOneItem.Id) {
                                            itemContent.className = "active";
                                        }
                                    }
                                }
                            }

                            // Help button
                            if (data.IsHelp) {
                                var help = topnavMain.appendChild(document.createElement("div"));
                                help.className = "acx-topnav-help";
                                help.appendChild(document.createTextNode("?"));
                                help.style.lineHeight = help.clientHeight + "px";
                                help.onclick = function () {
                                    return GlobalNav.helpClick();
                                };
                            }
                        })();
                    } else {
                        document.getElementsByTagName("html")[0].style.background = "#fff";
                        document.getElementsByTagName("body")[0].style.background = "#fff";
                    }

                    // Collapse the big amount of the level1 tabs
                    if (isFullHeader) {
                        (function () {

                            window.AppName = data.AppName;
                            window.EnvName = data.EnvName;

                            var isExpandable = false;

                            var tabs = document.querySelector(".acx-topnav-tabs");
                            var topNav = document.querySelector(".acx-topnav-main");
                            var topNavLogo = document.querySelector(".acx-topnav-logo");

                            function check() {
                                if (tabs && tabs.clientWidth > (topNav.clientWidth - topNavLogo.clientWidth - 160)) {
                                    isExpandable = true;

                                    var container = null;

                                    var expandEl = document.querySelector(".expand");

                                    if (!expandEl) {
                                        container = document.createElement("div");
                                        container.className = "expand";
                                    } else {
                                        container = document.querySelector(".expand");
                                    }
                                    var aEls = document.querySelectorAll(".acx-topnav-tabs a");
                                    var el = aEls[aEls.length - 1];

                                    container.insertBefore(el, container.firstChild);

                                    if (!expandEl) {
                                        var expandControl = document.createElement("div");
                                        expandControl.className = "expand-control";
                                        expandControl.innerHTML = "v";

                                        topNav.appendChild(expandControl);
                                        topNav.appendChild(container);
                                    }
                                    check();
                                }
                            }

                            check();

                            if (isExpandable) {
                                var topNavHelp = document.querySelector(".acx-topnav-help");
                                if (topNavHelp) {
                                    topNavHelp.style.right = "15px";
                                }
                            }

                            var expandEl = document.querySelector(".expand");
                            if (expandEl) {
                                var backgroundColor = window.getComputedStyle ? window.getComputedStyle(topNav).getPropertyValue("background-color") : topNav.currentStyle.backgroundColor;
                                expandEl.style.backgroundColor = backgroundColor;

                                var a = document.querySelector(".expand > a");
                                a.onmouseover = function () {
                                };
                            }

                            window.onresize = function () {
                                var expandEl = document.querySelector(".expand");

                                if (expandEl) {
                                    for (var i = 0; i < expandEl.children.length; ++i) {
                                        tabs.appendChild(expandEl.children[i]);
                                    }
                                    check();
                                }
                            };

                        })();


                        (function () {
                            var globalNavRoot = document.querySelector(".acx-global-navigation");

                            var globalmenuState = false;

                            //company dropdown list
                            var tenantsList = globalNavRoot.querySelector('.main-list li a');
                            if (tenantsList) {
                                tenantsList.onclick = function () {
                                    document.querySelector('.main-list .active').className.replace("active", "");
                                    this.parentNode.className += " active";
                                    document.querySelector('.current-selection').innerHTML = document.querySelector('.main-list .active a').innerHTML;
                                };
                            }

                            // small icons/logos
                            if (globalNavRoot.addEventListener) {
                                globalNavRoot.querySelector('.acx-div-logo-small').addEventListener("click", function () {

                                    if (globalmenuState) {
                                        slideUp({
                                            element: document.querySelector(".acx-global-menu"),
                                            callback: function () {
                                                globalmenuState = false;
                                            }
                                        });
                                    } else {
                                        slideDown({
                                            element: document.querySelector(".acx-global-menu"),
                                            callback: function () {
                                                globalmenuState = true;
                                            }
                                        });
                                    }
                                });

                                // tray arrow/close button
                                globalNavRoot.querySelector(".tray-close-button").addEventListener("click", function () {
                                    slideUp({
                                        element: document.querySelector('.acx-global-menu'),
                                        callback: function () {
                                            globalmenuState = false;
                                        }
                                    });
                                });
                            } else {
                                globalNavRoot.querySelector('.acx-div-logo-small').attachEvent("onclick", function () {

                                    if (globalmenuState) {
                                        slideUp({
                                            element: document.querySelector(".acx-global-menu"),
                                            callback: function () {
                                                globalmenuState = false;
                                            }
                                        });
                                    } else {
                                        slideDown({
                                            element: document.querySelector(".acx-global-menu"),
                                            callback: function () {
                                                globalmenuState = true;
                                            }
                                        });
                                    }
                                });
                                // tray arrow/close button
                                globalNavRoot.querySelector(".tray-close-button").attachEvent("onclick", function () {
                                    slideUp({
                                        element: document.querySelector('.acx-global-menu'),
                                        callback: function () {
                                            globalmenuState = false;
                                        }
                                    });
                                });
                            }

                            // company name under large icons/logos
                            function toggleTitle(title, show) {
                                var companyTitle = globalNavRoot.querySelector(".acx-company-title");

                                if (!show) {
                                    companyTitle.style.display = "none";
                                    return;
                                }
                                companyTitle.innerHTML = title;
                                companyTitle.style.display = "block";

                                var companyWidth = parseInt(window.getComputedStyle ? window.getComputedStyle(companyTitle).getPropertyValue("width") : companyTitle.clientWidth);
                                var titleLeft = ((this.offsetLeft + 39 / 2)) - (companyWidth / 2) + (this.className.toString().match("active") ? 3 : 0);
                                titleLeft = titleLeft < 15 ? 15 : titleLeft;

                                companyTitle.style.left = titleLeft + "px";
                            }

                            // large icons/logos
                            var largeEls = document.querySelector(".acx-div-logo-large");
                            if (largeEls.addEventListener) {
                                largeEls.addEventListener("click", function (e) {
                                    if (e.target.tagName.toUpperCase() === "IMG") {
                                        document.querySelector(".acx-div-logo-large a .active").className.replace("active", "");
                                        this.className += " active";
                                        toggleTitle.call(e.target, e.target.getAttribute("data-title"), 1);
                                    }
                                });

                                largeEls.addEventListener("mouseover", function (e) {
                                    if (e.target.tagName.toUpperCase() === "IMG") {
                                        toggleTitle.call(e.target, e.target.getAttribute("data-title"), 1);
                                    }
                                });

                                largeEls.addEventListener("mouseout", function (e) {
                                    if (e.target.tagName.toUpperCase() === "IMG") {
                                        toggleTitle.call(e.target, "", 0);
                                    }
                                });
                            } else {

                                largeEls.attachEvent("onclick", function (e) {
                                    if (e.srcElement.tagName.toUpperCase() === "IMG") {
                                        document.querySelector(".acx-div-logo-large a .active").className.replace("active", "");
                                        this.className += " active";
                                        toggleTitle.call(e.srcElement, e.srcElement.getAttribute("data-title"), 1);
                                    }
                                });

                                largeEls.attachEvent("onmouseover", function (e) {
                                    if (e.srcElement.tagName.toUpperCase() === "IMG") {
                                        toggleTitle.call(e.srcElement, e.srcElement.getAttribute("data-title"), 1);
                                    }
                                });

                                largeEls.attachEvent("onmouseout", function (e) {
                                    if (e.srcElement.tagName.toUpperCase() === "IMG") {
                                        toggleTitle.call(e.srcElement, "", 0);
                                    }
                                });

                            }

                            if (document.addEventListener) {

                                document.addEventListener("click", function (e) {
                                    var el = e.target;

                                    while (el !== null && !el.className.toString().match("acx-global-navigation")) {
                                        el = el.parentElement;
                                    }

                                    if (globalmenuState && !el) {
                                        slideUp({
                                            element: document.querySelector(".acx-global-menu"),
                                            callback: function () {
                                                globalmenuState = false;
                                            }
                                        });
                                    }
                                });

                                if (document.querySelector(".acx-topnav-tabs a")) {
                                    document.querySelector(".acx-topnav-tabs a").addEventListener('click', selectTab);                                    
                                }
                            } else {

                                document.attachEvent("onclick", function (e) {
                                    var el = e.srcElement;

                                    while (el !== null && !el.className.toString().match("acx-global-navigation")) {
                                        el = el.parentElement;
                                    }

                                    if (globalmenuState && !el) {
                                        slideUp({
                                            element: document.querySelector(".acx-global-menu"),
                                            callback: function () {
                                                globalmenuState = false;
                                            }
                                        });
                                    }
                                });

                                if (document.querySelector(".acx-topnav-tabs a")) {
                                    document.querySelector(".acx-topnav-tabs a").attachEvent('onclick', selectTab);
                                }
                            }

                            var expandContainer = document.querySelector(".expand");
                            if (expandContainer) {
                                if (expandContainer.addEventListener) {

                                    expandContainer.addEventListener('click', selectTab);
                                } else {

                                    expandContainer.attachEvent('onclick', selectTab);
                                }
                            }

                            function selectTab(e) {
                                if ((typeof e.target != 'undefined' && e.target.tagName.toUpperCase() === "DIV") || (e.srcElement.tagName.toUpperCase() === "DIV")) {
                                    var activeTab = document.querySelector(".acx-topnav-tabs .active");
                                    activeTab = activeTab ? activeTab : document.querySelector(".expand .active");

                                    activeTab.className = activeTab.className.replace("active", "");
                                    if (e.target) {
                                        e.target.className += " active";
                                    } else {
                                        e.srcElement.className += " active";
                                    }

                                }
                            }
                        })();
                    }
                }

                function getContentUrl(content) {
                    return formatUrl(data.StyleGuideRoot + "/" + content);
                }

                function getGlobalNavResource(content) {
                    var path = DATA.StyleGuideRoot.split("/");
                    path.pop();
                    path.pop();

                    path = path.join("/");

                    return formatUrl(path + "/gn/" + content);
                }

                function getIconUrl(iconUrl) {
                    var path = DATA.StyleGuideRoot.split("/");
                    path.pop();
                    path.pop();

                    path = path.join("/");

                    return formatUrl(path + "/productIcons/" + iconUrl);
                }

                function formatUrl(url) {
                    return url.replace(new RegExp("([^:]/)/+", "g"), "$1");
                }

                function getSchemaColorClass() {
                    var result = "acx-global-navigation-";

                    switch (parseInt(DATA.StyleGuideRoot.split("/").pop())) {
                        case 1001:
                            result += "grey";
                            break;
                        case 1002:
                            result += "teal";
                            break;
                        case 1003:
                            result += "rust";
                            break;
                        case 1004:
                            result += "blue";
                            break;
                        case 1005:
                            result += "sage";
                            break;
                        case 1006:
                            result += "lavender";
                            break;
                        default:
                            return "";
                    }

                    return result;
                }

                function colorToHex(color) {
                    if (color.substr(0, 1) === '#') {
                        return color;
                    }
                    var digits = /(.*?)rgb\((\d+), (\d+), (\d+)\)/.exec(color);

                    try {
                        var red = parseInt(digits[2]);
                        var green = parseInt(digits[3]);
                        var blue = parseInt(digits[4]);

                        var rgb = blue | (green << 8) | (red << 16);
                        return digits[1] + '#' + rgb.toString(16);
                    } catch (e) {
                        return "";
                    }
                }

                function cropText(text, length) {
                    text = text || "";

                    var maxLength = length || 37;

                    if (text.length > (maxLength + 3)) {
                        return text.slice(0, maxLength) + '...';
                    }

                    return text;
                }
            }
        })();
    }
})();
