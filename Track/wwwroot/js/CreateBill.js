var nameSet = new Set();
$(document).ready(function () {
    $("#ClientName").change(function () {
        var selectedOption = $(this).val();
        $("#ClientNumber").val(selectedOption);
    });
    fillCustomer();
    fillOptions();
    reloadTable();
    $("#Quantity").change(function () {
        Getit();
    });
    $("#ProductName").change(function () {
        Getit();
    });

});



$("#AddProduct").on("submit", function (e) {
    e.preventDefault();
    var ID = $("#ID").val();
    var productName = $("#ProductName").val();
    var Rate = $("#Rate").val();
    var Quantity = $("#Quantity").val();
    var Serialn_no = [];
    var Bill_id = $("#Bill_id").val();
    for (var i = 0; i < Quantity; i++) {
        var sid = "#S-" + i;
        var iid = "#I-" + i;
        var data = {
            Id: $(iid).val(),
            value:$(sid).val()
        };
        Serialn_no.push(data);
    }
    var Class = {
        id: ID,
        product_id: productName,
        rate: Rate,
        quantity: Quantity,
        bill_id: Bill_id
    };
    var DATA = {
        class: Class,
        serial_no: Serialn_no
    }
    $.ajax({
        url: '/bill/addCom',
        type: 'Post',
        dataType: 'json',
        data: DATA,
        success: function (response) {
            if (response.success) {
                toastr["success"](response.message, "Value Added", { timeOut: 5000 });
                document.getElementById("AddProduct").reset();
                reloadTable();
            }
            else {
                toastr["error"](response.message, "Not entered", { timeOut: 5000 });
            }
        },
        error: function (xhr, textStatus, error) {
            //console.log("error");
            alert(xhr.statusText);
            console.log(textStatus);
            console.log(error);
        }
    });
});

function fillCustomer() {
    $.ajax({
        url: '/Main/getAllcutomer',
        type: 'Get',
        dataType: 'json',
        contentType: 'application/jon; charset=utf-8',
        success: function (result) {
            $("#ClientName").empty();
            $('#ClientName').append('<option disabled selected>--Select Customer--</option>');
            $("#ClientNumber").empty();
            $('#ClientNumber').append('<option disabled selected>--Customer Number--</option>');
            $.each(result, function (index, value) {
                nameSet.add(String(value.name).toUpperCase());
                $('#ClientName').append('<option value="' + value.id + '">' + value.name + '</option>');
                $('#ClientNumber').append('<option value="' + value.id + '">' + value.phoneNumber + '</option>');

            });
        }
    });
}

function reloadTable() {
    let bill_id = $("#Bill_id").val();
    var URL;
    if (bill_id != 0) {
        URL = '/bill/getCom?id=' + bill_id;
    }
    else {
        URL ='/bill/getCom'
    }
    $.ajax({
        url: URL,
        type: 'Get',
        data: 'json',
        contentType: 'application/ json; charset = utf - 8;',
        success: function (result) {
            var Obj = "";
            var total = 0;
            $.each(result, function (index, value) {
                Obj += '<tr>';
                if (value.product=== null) {
                    Obj += '<td>' + value.extra_items + '</td>';
                }
                else {
                    Obj += '<td>' + value.product.name + '</td>';
                }
                Obj += '<td>' + value.rate + '</td>';
                Obj += '<td>' + value.quantity + '</td >';
                Obj += '<td>' + value.total + '</td>';
                total += value.total;
                Obj += '<td><a class="btn btn-danger" onclick=Delete("/bill/DeleteCom?id=' + value.id + '")><i class="bi bi-trash"></i> Delete</a></td>';
                if (value.product == null) {
                    Obj += '<td><button class="btn btn-success" onclick=EditService(' + value.id + ',\'' + value.extra_items + '\',' + value.rate + ',' + value.quantity + ') data-toggle="modal" \
                    data-target="#ServiceModalLong">Edit</button></td>';
                }
                else {
                    Obj += '<td><button class="btn btn-success" onclick=Edit(' + value.id + ',' + value.product_id + ',' + value.rate + ',' + value.quantity + ') data-toggle="modal" \
        data-target="#exampleModal">Edit</button></td>';
                }
                Obj += '</tr>';
            });
            Obj += '<tr>';
            Obj += '<td><td>';
            Obj += '<th>Total</th>';
            Obj += '<td id="t-otal">' + total + '</td>';
            $("#sub-total").val(total);
            Obj += '</tr>';
            var VAT = total * 0.13;
            Obj += '<tr>';
            Obj += '<td><td>';
            Obj += '<th>13% VAT</th>';
            Obj += '<td>' + VAT + '</td>';
            Obj += '</tr>';
            Obj += '<tr>';
            Obj += '<td><td>';
            Obj += '<th>G. Total</th>';
            Obj += '<td>' + (total + VAT) + '</td>';
            Obj += '</tr>';
            console.log(result);
            $("#t-body").html(Obj);
        }
    });
}

function Getit() {
    var total = $("#Quantity").val();
    var productId = $("#ProductName").val();
    var URL = '/Bill/SerialId?id=' + $("#ID").val() + '&Qua=' + total + '&pro=' + productId;
    $.ajax({
        url: URL,
        type: 'Get',
        dataType: 'json',
        contentType: 'application/ json; charset = utf - 8;',
        success: function (result) {
            if (result.success) {
                var Obj = "";
                result.value.forEach(function (item, index) {
                    Obj += ' <div class="form-group">\
            <label>Serial no of Product ' + item.id + '</label>\
                <input type="number" hidden id="I-'+index+'" value="'+ item.id+'">\
                <input type="text" class="form-control" id="S-'+ index + '" value="' + item.value + '" placeholder="Serial no">\
                </div>';
                });
                $("#tada").html(Obj);
            }
            else {
                $("#tada").empty();
                toastr["error"](result.message, "Something went wrong", { timeOut: 5000 });
            }
        }
    });
}

function Delete(Url) {

    $.confirm({
        title: 'Delete',
        content: 'Are you sure?',
        buttons: {
            confirm: function () {
                $.ajax({
                    url: Url,
                    type: 'delete',
                    success: function (data) {
                        toastr["success"](data.message, "Value Deleted", { timeOut: 5000 });
                        reloadTable();
                    },

                })
            },
            cancel: function () {
                $.alert('Canceled!');
            }
        }
    });
}

function Edit(i, product, rate, quantity) {
    $("#ID").val(i);
    $("#ProductName").val(product);
    $("#Rate").val(rate);
    $("#Quantity").val(quantity);
    Getit();
}

function Add() {
    $("#ID").val(0);
    $("#ProductName").val(0);
    $("#Rate").val(0);
    $("#Quantity").val(0);
    Getit();
}

function EditService(id, service, rate, quantity) {
    $("#NService").val(service);
    $("#rate").val(rate);
    $("#quantity").val(quantity);
    $("#billS_id").val(id)
}

function AddService() {
    $("#NService").val(null);
    $("#rate").val(0);
    $("#quantity").val(0);
    $("#billS_id").val(0)
}
function onClickme() {
    let Id = $("#billS_id").val();
    let Bill_id = $("#Bill_id").val();
    let service_name = $("#NService").val();
    let Rate = $("#rate").val();
    let Quantity = $("#quantity").val();
    let obj = {
        id: Id,
        bill_id: Bill_id,
        rate: Rate,
        quantity: Quantity,
        extra_items: service_name
    };
    $.ajax({
        type: 'POST',
        url: '/ChalaniToBill/addService',
        data: 'json',
        data: obj,
        success: function (result) {
            if (result.success) {
                toastr["success"](result.message, "Service added", { timeOut: 5000 });
                reloadTable();
            }
            else {
                toastr["error"](reuslt.message, "Something went Wrong", { timeOut: 5000 });
            }
        }
    });
}