$(document).ready(function () {
  console.log("ready!");
  $("#historyBackBtn").click(function () {
    history.go(-1);
    console.log("test");
  });
  $(".menu-toggle").click(function () {
    $(".site-nav").toggleClass("site-nav-open", 300);
    $(this).toggleClass("menu-open");
  });
  $("#logoutBtn").on("click", function () {
    $("#logoutForm").submit();
  });
  $(".multi-list").select2({
    theme: "classic",
    dropdownCssClass: "multi-list",
  });
  $(function () {
    $("#catchPoke").click(function () {
      var pokemonID = $(this).data("id");
      var IsShiny = $(this).data("isshiny");
      var areaID = $(this).data("areaid");
      var data = {
        id: pokemonID,
        isshiny: IsShiny,
        areaId: areaID
      };
      console.log(areaID);
      $("#catchPoke").hide();
      $.ajax({
        type: "POST",
        url: "/PokePC/CaughtPokemon",
        data: data,
        dataType: "html",
        success: function (response) {
          $("#myModalContent").html(response);
          $("#myModal").slideDown();
        },
        failure: function (response) {
          alert(response.responseText);
        },
        error: function (response) {
          alert(response.responseText);
        },
      });
    });
    $("#closeBtn").click(function () {
      $("#myModal").slideUp();
    });
  });
  $(function () {
    $(".deleteArea").click(function () {
      var areaID = $(this).data("id");
      var data = {
        id: areaID,
      };
      $.ajax({
        type: "POST",
        url: "ConfirmDeleteArea",
        data: data,
        dataType: "html",
        success: function (response) {
          $("#myModalContent").html(response);
          $("#myModal").slideDown();
        },
        failure: function (response) {
          alert(response.responseText);
        },
        error: function (response) {
          alert(response.responseText);
        },
      });
    });
    $("#closeBtn").click(function () {
      $("#myModal").slideUp();
    });
  });
  $(function () {
    $(".releasePoke").click(function () {
      var pokemonID = $(this).data("id");
      var prevUrl = $(this).data("url");
      var data = {
        id: pokemonID,
        url: prevUrl,
      };
      $.ajax({
        type: "POST",
        url: "ConfirmReleasePokemon",
        data: data,
        dataType: "html",
        success: function (response) {
          $("#myModalContent").html(response);
          $("#myModal").slideDown();
        },
        failure: function (response) {
          alert(response.responseText);
        },
        error: function (response) {
          alert(response.responseText);
        },
      });
    });
    $("#closeBtn").click(function () {
      $("#myModal").slideUp();
    });
  });
  $(function () {
    $(".deletePoke").click(function () {
      var pokemonID = $(this).data("id");
      var data = {
        id: pokemonID,
      };
      $.ajax({
        type: "POST",
        url: "ConfirmDeletePokemon",
        data: data,
        dataType: "html",
        success: function (response) {
          $("#myModalContent").html(response);
          $("#myModal").slideDown();
        },
        failure: function (response) {
          alert(response.responseText);
        },
        error: function (response) {
          alert(response.responseText);
        },
      });
    });
    $("#closeBtn").click(function () {
      $("#myModal").slideUp();
    });
  });
  
});
