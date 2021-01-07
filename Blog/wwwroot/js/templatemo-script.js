$(function() {
    $(".navbar-toggler").on("click", function(e) {
        $(".tm-header").toggleClass("show");
        e.stopPropagation();
      });
      $("#tm-nav .nav-link").click(function(e) {
        $(".tm-header").removeClass("show");
      });
});