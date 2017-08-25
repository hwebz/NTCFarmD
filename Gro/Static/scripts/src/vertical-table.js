var VerticalTable = VerticalTable || function () {
  var i, element;
  //create headers for the mobile view
  function createHeader(table) {
      var headers = table.querySelectorAll(".responsive-table th"),
      index = 1,
      columns = table.querySelectorAll(".responsive-table td"),
      headerCell,
      headerName,
      headerClass,
      sharedRow,
      responsiveHeader,
      collapseRow;

    if (columns.length > 0) {
      for (i = 0; i < columns.length; i += 1) {
        if (index > headers.length) {
          index = 1;
        }

        //mobile headers are actually td
        element = columns[i];

        if (index === 1) {
          var buttonDiv = document.createElement("div");
          buttonDiv.className = "table-expand-info expand-icon-plus responsive-header";
          element.parentNode.insertBefore(buttonDiv, element);
        }

        headerCell = table.querySelector('.responsive-table th:nth-of-type(' + index + ')');
        headerClass = headerCell.className;
        //console.log(headerClass);
        headerName = headerCell.textContent;
        if (headerClass.indexOf('action-control') >= 0 || headerClass.indexOf('header-desktop-only') >= 0) {
          //console.log(headerCell.textContent);
        } else {
          responsiveHeader = document.createElement("td");
          responsiveHeader.className = "responsive-header " + headerClass;
          responsiveHeader.innerHTML = headerName;
          element.parentNode.insertBefore(responsiveHeader, element);
        }

        index += 1;
      }
    }

    collapseRow = $('.responsive-table .collapsed-row');
    collapseRow.addClass('hidden');
  };

  var hasBeenSet = false;

  //addds mobile class for table when table is larger than screen
  function styleTable() {
    var windowWidth = window.innerWidth,
      responsiveTables = document.getElementsByClassName('responsive-table'),
      collapseRow = $('.responsive-table .collapsed-row');

    if (windowWidth <= 768) {
      if (!hasBeenSet) {
        for (i = 0; i < responsiveTables.length; i += 1) {
          element = responsiveTables[i];
          element.classList.add("mobile");
          collapseRow.addClass('hidden');
          $(".table-expand-info").removeClass('expand-icon-minus');
          $(".table-expand-info").addClass('expand-icon-plus');
        }
        hasBeenSet = true;
      }
    } else {
      for (i = 0; i < responsiveTables.length; i += 1) {
        element = responsiveTables[i];
        element.classList.remove("mobile");
        collapseRow.removeClass('hidden');
        $(".table-expand-info").removeClass('expand-icon-plus');
        $(".table-expand-info").addClass('expand-icon-minus');
      }
      hasBeenSet = false;
    }
  }

  function init() {
      var verticalTables = document.querySelectorAll('table.responsive-table');
      if (verticalTables.length > 0) {
          for (var count = 0; count < verticalTables.length; count++) {
              createHeader(verticalTables[count]);
              //console.log(count);
          }
      }

    styleTable();
    $(window).on('resize', function () {
      styleTable();
    });
  }
  return {
    init: init
  }
} ();

$(function () {
  VerticalTable.init();
});

$(document).ready(function (e) {
  $(".table-expand-info").click(function () {
    var currentRow = $($(this).parent());
    var collapseRow = currentRow.find('.collapsed-row');
    if ($(this).hasClass("expand-icon-plus")) {
      $(this).removeClass("expand-icon-plus");
      $(this).addClass("expand-icon-minus");
      collapseRow.removeClass('hidden');
    }
    else {
      $(this).removeClass("expand-icon-minus");
      $(this).addClass("expand-icon-plus");
      collapseRow.addClass('hidden');
    }
  });
  $(".table-expand-info").next().click(function () {
    var currentRow = $($(this).parent());
    var that = currentRow.find('.table-expand-info')[0];
    var collapseRow = currentRow.find('.collapsed-row');
    if ($(that).hasClass("expand-icon-plus")) {
      $(that).removeClass("expand-icon-plus");
      $(that).addClass("expand-icon-minus");
      collapseRow.removeClass('hidden');
    }
    else {
      $(that).removeClass("expand-icon-minus");
      $(that).addClass("expand-icon-plus");
      collapseRow.addClass('hidden');
    }
  });
});