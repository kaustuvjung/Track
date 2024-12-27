var Customer_name = new Set();
var Serial_no = new Set();
var product_name = new Set();
var order_no = new Set();

$(document).ready(function () {
    reloadTable(0, '');
    $("input[type=checkbox]").click(function () {
        if ($("#idDamaged").is(':checked')) {
            people = prompt("How was it Damaged?");
            if (people == null) {
                $("#Dmessage").val(null);
            }
            else {
                $("#Dmessage").val(people);
            }
        }
    });
});
 
// Search  stock by name seriual-No and customername and productname
$("#Search").change(function () {
    var value = $("#Search").val();
    var data = [];
    if (value == 1) {
        data = Array.from(Serial_no);
    }
    else if (value == 2) {
        data = Array.from(product_name);
    }
    else if (value == 3) {
        data = Array.from(Customer_name);
    }
    else {
        data = Array.from(order_no);
    }
    autocomplete(document.getElementById("myInput"), data);

});

// seacrch logic
$("#form-search").on("submit", function (e) {
    e.preventDefault();
    var wh = $("#Search").val();
    var mval = String($("#myInput").val()).toUpperCase();
   
    reloadTable(wh, mval);
});


function reloadTable(wh, mval) {
    var Order_id = $("#Value-id").val();
    var URL = "";
    if (Order_id == 0) {
        URL = '/order/getMost';
    }
    else
    {
        URL = '/order/get?id=' + Order_id;
    }
    
    $.ajax({
        url: URL,
        type: 'Get',
        dataType: 'json',
        contentType: 'application/jon; charset=utf-8',
        success: function (result, status, xhr) {
            var Obj = "";
            var one = "";
            $.each(result, function (index, value) {
                //if (value.serial_number != null)
                //{
                //    return;
                //}
                if (wh == 1) {
                    one = String(value.serial_number).toUpperCase();
                }
                else if (wh == 2) {
                    one = String(value.product.name).toUpperCase();
                }
                else if (wh == 3) {
                    if (value.customer_id != null) {
                        one = String(value.customer.name).toUpperCase();
                    }
                    else {
                        one = 'NULL';
                    }
                }
                else if (wh == 4) {
                    one = String(value.order_id).toUpperCase();
                }
                if (wh == 0 || one == mval) {

                    Obj += '<tr>';
                    Obj += '<td>' + value.product.modal + '</td>';
                    Obj += '<td>' + value.serial_number + '</td>';
                    Serial_no.add(value.serial_number);
                    Obj += '<td>' + value.product.name + '</td>';
                    product_name.add(value.product.name);
                    Obj += '<td>' + value.inStock + '</td>';
                    if (value.isDamaged != null) {
                        Obj += '<td><i class="text-danger bi bi-x-circle h1"></i></td>';
                    }
                    else {
                        Obj += '<td><i class="text-success bi bi-check2-circle h1"></i></td>';
                    }
                    Obj += '<td>' + String(value.customer_id) + '</td>';
                    if (value.customer_id != null) {
                        Obj += '<td>' + value.customer.name + '</td>';
                        Customer_name.add(value.customer.name)
                    }
                    else {
                        Obj += '<td>null</td>'
                    }
                    Obj += '<td><button type="button" class="btn btn-success" data-toggle="modal"\
                               data-target="#exampleModal" onclick="OneAdd(' + value.id + ', \'' +
                        value.serial_number + '\',\'' + value.isDamaged + '\')"><i class="bi bi-pencil-square"></i> Edit</button></td>';

                    Obj += '<td><a class="btn btn-danger" onclick=Delete("/Order/Deletestock?id=' + value.id + '")><i class="bi bi-trash"></i> Delete</a></td>';

                }


            });
            $('#t-body').html(Obj);
                
        },
        Error: function (result) {
            alert(result);
        }
    })
}

function Delete(Url) {
    $.confirm({
        title: 'Delete',
        content: 'Are you Sure?',
        buttons: {
            confirm: function () {
                $.ajax({
                    url: Url,
                    type: 'delete',
                    success: function (data) {
                        toastr["success"](data.message, "Value Deleted", { timeOut: 5000 });
                        reloadTable(0, '');
                    },

                })
            },
            cancel: function () {
                $.alert('Canceled!');
            }
        }
    });
}

function OneAdd(id, serial_no, isDamaged)
{
    $("#Stock_NO").val(id);
    $("#serial_no").val(serial_no);
    console.log(isDamaged);
    if (isDamaged != "null") {
        $("#idDamaged").prop('checked', true);
    }
}

$("#myForm").on("submit", function (e) {
    e.preventDefault();
    var Id = $("#Stock_NO").val(); 
    var Serial_no = $("#serial_no").val();
    var Customer_id = $("#Customer").val();
    var IsDamaged = $("#idDamaged").is(':checked') ? 'F' : null;
    var Damaged_why = $("#Dmessage").val();
    var obj = {
        id: Id,
        serial_no: Serial_no,
        customer_id: Customer_id,
        isDamaged: IsDamaged,
        damaged_why: Damaged_why
    };

    $.ajax({
        url: '/Order/AddSerial',
        type: 'Post',
        dataType: 'json',
        data: obj,
        success: function (response) {
            if (response.success) {
                toastr["success"](response.message, "Value Added", { timeOut: 5000 });
                document.getElementById("myForm").reset();
                reloadTable(0,'');

            }
            else {
                toastr["error"](response.message, "Not entered", { timeOut: 5000 });
            }
        },
        error: function (how) {
            alert("Missing Form data");
        }
    })
});



