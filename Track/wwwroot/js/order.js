var mySet = new Set();
$(document).ready(function () {
    reloadTable("");
});

$("#form-search").on("submit", function (e) {
    e.preventDefault();
    var mval = String($("#myInput").val()).toUpperCase();
    reloadTable(mval);
});

function reloadTable(mval) {
    $.ajax({
        url: '/Order/getall',
        type: 'Get',
        dataType: 'json',
        ContentType: 'application/jon; charset=utf-8',
        success: function (result, status, xhr) {
            var Obj = "";
            $.each(result, function (index, value) {
                if (mval == "" || mval == String(value.id).toUpperCase() ||
                    mval == String(value.arival.substr(0, 10)).toUpperCase() ||
                    mval == String(value.invoice_no).toUpperCase() ||
                    mval == String(value.vendor.name).toUpperCase()
                ) {
                    Obj += '<tr>';
                    Obj += '<td>' + value.id + '</td>';
                    mySet.add(String(value.id).toUpperCase());
                    Obj += '<td>' + value.arival.substr(0, 10) + '</td>';
                    mySet.add(String(value.arival.substr(0, 10)).toUpperCase());
                    Obj += '<td>' + value.invoice_no + '</td>';
                    mySet.add(String(value.invoice_no).toUpperCase());
                    Obj += '<td>' + value.vendor.name + '</td>';
                    mySet.add(String(value.vendor.name).toUpperCase());
                    Obj += '<td> <a class="btn btn-secondary" href="/Order/AddSerial?id=' + value.id + '"><i class="bi bi-door-open-fill"></i> View Order</a></td>';
                    Obj += '<td> <a Onclick=Delete("/Order/Delete?id=' + value.id + '") class="btn btn-danger"><i class="bi bi-trash"></i> Delete</a></td>';
                    Obj += '<td> <a class="btn btn-success" href="/Order/ViewOrder?id=' + value.id + '"><i class="bi bi-view-list"></i> View</td>';
                }
            });
            autocomplete(document.getElementById("myInput"), Array.from(mySet));
            $('#t-body').html(Obj);
        },
        Error: function (result) {
            alert(result);
        }
    });
}


function Delete(Url) {
    $.confirm({
        title: 'Delete!',
        content: 'Are you Sure?',
        buttons: {
            confirm: function () {
                $.ajax({
                    url: Url,
                    type: 'Delete',
                    success: function (data) {
                        toastr["success"](data.message, "Value Deleted" ,{ timeOut: 5000 });

                        reloadTable("");
                    }
                })

            },
            cancel: function () {
                $.alert('Canceled!');
            }
        }
    });
}
