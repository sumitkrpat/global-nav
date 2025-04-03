if (window._LOAD_API_DATA && window._LOAD_API_DATA.globalNav) {
    _LOAD_API_DATA.globalNav = {
        js: [],
        css: [],
        components: [],
        _js: false,
        _css: false
    };
}

function NavObject(levelZeroName, iconUrl, levelOneName) {
    this.level0 = {
        name: levelZeroName || "",
        iconurl: iconUrl || "#"
    };

    this.level1 = {
        name: levelOneName || ""
    };
}

GlobalNav = {
    nav: new NavObject(),
    setAutoCompleteList: function (list) {
    },
    events: (function () {
        var returnObject = {};

        function getContainer() {
            return document.getElementsByTagName('html')[0];
        }

        return {
            addListener: function (evType, func, returnObj) {
                jQuery(getContainer()).bind(evType, func);
                returnObject[evType] = returnObj;
            },
            dispatchEvent: function (evType, optParams) {
                var event = jQuery.Event(evType);
                event.detail = optParams;
                jQuery(getContainer()).trigger(event);

                return returnObject[evType];
            },
            isListened: function (evType) {
                return evType in returnObject;
            }
        };
    })(),

    links: (function () {
        var links = {};

        return {
            register: function (name, link) {
                links[name] = link;
            },

            select: function (link) {
                var linkElem = links[link.Name];
                if (link) {
                    window.__globalNav__.SelectedLevelOneItem.Id = link.Id;
                }
                if (linkElem) {
                    var activeTab = null;
                    if(window.__globalNav__.IsNewGN) {
                        activeTab = document.querySelector("#acx-new-gn-app-links a.acx-new-gn-active");
                        GlobalNav.setTitle(linkElem.innerText);
                        activeTab = activeTab ? activeTab : document.querySelector(".acx-new-gn-active");

                        activeTab.className = activeTab.className.replace("acx-new-gn-active", "");
                        linkElem.className += "acx-new-gn-active";
                    } else {
                        activeTab = document.querySelector(".acx-topnav-tabs .active");
                        activeTab = activeTab ? activeTab : document.querySelector(".expand .active");

                        activeTab.className = activeTab.className.replace("active", "");
                        linkElem.className += "active";
                    }
                }
            }
        };
    })(),

    setTitle: function(title) {
        window.__globalNav__.SelectedLevelOneItem.DisplayName = title;
        document.querySelector("#acx-global-nav-bar-divider h3").innerText = title;
    },

    openHelp: function (topic) {
        var helpItem = null;
        var topicProp = typeof topic == "number" ? "id" : "name";
        for (var i in GlobalNav.helpItems) {
            if (GlobalNav.helpItems[i][topicProp] == topic) {
                helpItem = GlobalNav.helpItems[i];
            }
        }

        if (helpItem && helpItem.helpLoc) {
            window.open(helpItem.helpLoc);

            return true;
        }

        return false;
    },

    helpClick: function () {
        if (GlobalNav.events.isListened('helpClick')) {
            return GlobalNav.events.dispatchEvent('helpClick');
        } else {
            if (!GlobalNav.OpenHelp(0)) {
                window.open(GlobalNav.DefaultHelpUrl);
            }

            return false;
        }
    },

    OpenHelp: function (topic) {
        return GlobalNav.openHelp(topic);
    },

    HelpClick: function () {
        return GlobalNav.helpClick();
    }
};

if(window.__globalNav__.IsNewGN) {
    GlobalNav.toggleHelp = function (view) {
        if (typeof view !== 'boolean') {
            view = true;
        }
        window.__globalNav__.ViewHelp = view;
        if (document.querySelector('#acx-new-gn-help')) {
            document.querySelector('#acx-new-gn-help').style.display = view ? 'block' : 'none';
        }
    };
    GlobalNav.toggleSettings = function (view) {
        if (typeof view !== 'boolean') {
            view = true;
        }
        window.__globalNav__.ViewSettings = view;
        if (document.querySelector('#acx-new-gn-settings')) {
            document.querySelector('#acx-new-gn-settings').style.display = view ? 'block' : 'none';
        }
    };
    GlobalNav.toggleSearch = function (view) {
        if (typeof view !== 'boolean') {
            view = true;
        }
        window.__globalNav__.ViewSearch = view;
        if (document.querySelector('#acx-new-gn-search')) {
            document.querySelector('#acx-new-gn-search').style.display = view ? 'block' : 'none';
        }
    };
    GlobalNav.settingsClick = function () {
        if (GlobalNav.events.isListened('settingsClick')) {
            return GlobalNav.events.dispatchEvent('settingsClick');
        } else {
            return false;
        }
    };
}