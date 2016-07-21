(function () {

    $(function () {

        // prompt for name
        $("#name-modal").modal({
            backdrop: 'static'
        }).modal('show');

        $("#join").on('click', function () {
            
            var name = $("#name").val();

            if (name.length > 0) {
                // connect
                joinGame(name);

                // dismiss
                $("#name-modal").modal('hide');
            }
        });
    });

})();