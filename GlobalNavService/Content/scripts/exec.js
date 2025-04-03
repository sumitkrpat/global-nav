var loading = new ACXM.load(["BASE", "globalNav", "topNav"]);

loading.event.on("loaded", function() {
  window.__draw(document.getElementById("acx-global-nav"), window.__globalNav__);
  if (ACXM.cmp && ACXM.cmp.globalNav) {
      ACXM.cmp.globalNav.init();
  }
});

loading.init();
