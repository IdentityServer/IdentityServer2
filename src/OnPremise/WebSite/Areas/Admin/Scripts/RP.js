$(function () {
    $("button.base64").click(function () {
        var self = $(this);
        var url = self.closest("form").data("base64url");
        var target = self.data("base64target");
        $.ajax({
            url: url,
            success: function (result) {
                $("#" + target).val(result);
            }
        });
    });
});