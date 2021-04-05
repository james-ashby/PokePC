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
    $("#caughtPoke").click(function () {
      console.log("test");
      var pokemonID = $(this).data("id");
      var IsShiny = $(this).data("isshiny");
      var data = {
        id: pokemonID,
        isshiny: IsShiny,
      };
      $("#caughtPoke").hide();
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
    $(".releasePoke").click(function () {
      console.log("test");
      var pokemonID = $(this).data("id");
      var data = {
        id: pokemonID,
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
