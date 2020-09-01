$(() => {

    $("#txt-author").on('keyup', function () {
        EnableDisable();
    });

    function EnableDisable() {
        $("#btn-comment-submit").prop('disabled', $("#txt-author").val() === null || $("#txt-author").val() === "");
    }




})