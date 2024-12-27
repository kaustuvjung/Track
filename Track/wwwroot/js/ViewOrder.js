$(document).ready(function () {
    reloadTable();
});

function reloadTable() {
    var URL = "/Order/getVewOrder?id=" + $("#ID").val();
    $.ajax({
        type: 'Get',
        url: URL,
        data: 'json',
        contentType: 'application/ json; charset = utf - 8;',
        success: function (result) {
            var Obj = "";
            $.each(result, function (index, value) {
                Obj += '<tr>';
                Obj += '<td>' + value.id + '</td>';
                Obj += '<td>' + value.product.name + '</td>';
                Obj += '<td>' + value.quantity + '</td>';
            })
            $("#t-body").html(Obj);
        }
    });
}