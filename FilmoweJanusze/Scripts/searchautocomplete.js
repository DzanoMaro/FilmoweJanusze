/*
    var searchautocomplete = function () {
        var $input = $("SearchString");
        var options = {
            source: $input.attr("data-autocomplete-source"),
            select: function (event, ui) {
                $input.val(ui.item.label);
                var $form = $input.parents("form:first");
                $form.submit();
            }
        };
        $input.autocomplete(options);
    };

    searchautocomplete();

*/
var $input = $("#SearchString");

    $(function () {
        $("#SearchString").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/Home/SearchAutoComplete/',
                    data: "{ 'searchString': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return item;
                        }))
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }

                });
            },
            select: function (event, ui) {
                $input.val(ui.item.val);
                var $form = $input.parents("form:first");
                $form.submit();
            },
            minLength: 2
        });
    });
