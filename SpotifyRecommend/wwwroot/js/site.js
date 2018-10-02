$(document).ready(function () {
    $(".fade-in").hide(0).delay(500).fadeIn(3000);
    loadEvents();
});

function loadEvents() {
    $("#recommend").click(function () {
        console.log($("#form").serialize());
        $("#reco-icon").addClass("fa-spin");
        $.ajax({
            url: "Home/Recommendations",
            type: "get",
            data: $("#form").serialize(),
            success: function (result) {
                $("#reco-icon").removeClass("fa-spin");
                //console.log(result);
                $("#recommendations").html(result);
            },
            error: function (xhr) {
                $("#reco-icon").removeClass("fa-spin");
                $("#error").fadeIn(3000);
                $("#error").delay(8000).fadeOut(3000);
            }
        });
    });

    $("#form").submit(function (e) {
        e.preventDefault();
    });

    $("#Categories").change(function () {
        $.ajax({
            url: "Home/CategoryPlaylists",
            type: "get",
            data: $("#form").serialize(),
            success: function (result) {
                $("#category-playlists").html(result);
                $("#SeedPlaylists").imagepicker({
                    hide_select: true,
                    show_label: true
                });
            },
            error: function (xhr) {
                alert(xhr.statusText);
            }
        });
    });
}