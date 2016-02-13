

function openTab(sender, id) {
  var tabToOpen = sender.id;
  tabToOpen = tabToOpen.replace("TabBtn", "Tab");
  if (!$(sender).hasClass("activeTab")) {
    $(".tabHeaderBtn." + id).removeClass("activeTab");
    $(sender).addClass("activeTab");
    $(".tabCtl." + id).hide();
    $("#" + tabToOpen).show();
  }
}

function openStartTab(tabToOpen, id) {
  $(".tabCtl." + id).hide();
  if (tabToOpen == "") {
    $(".tabHeaderBtn." + id + ".defaultTab").addClass("activeTab");
    $(".tabCtl." + id + ".defaultTab").show();
  }
  else {
    $("#" + tabToOpen + "Btn").addClass("activeTab");
    $("#" + tabToOpen).show();
  }
}