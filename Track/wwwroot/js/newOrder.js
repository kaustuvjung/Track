$(document).ready(function () {
    reloadTable();
    $("#Quantity").change(function () {
        Getit($("#ID").val());
    });
    var now = new Date();

    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);

    var today = now.getFullYear() + "-" + (month) + "-" + (day);

    $('#Date').val(today);
});


function Getit(id) {
    var total = $("#Quantity").val();
    var a;
    if (id === 0) {
        var newObj = "";
        for (a = 0; a < total; a++) {
            newObj += ' <div class="form-group">\
             <label>Serial no of Product</label>\
            <input type="text" class="form-control" id="S-'+ index + '" value="' + item.value + '" placeholder="Serial no">\
             </div>';
        }
        $("#tada").html(newObj);
    }
    else {
        var URL = '/order/SerialId?id=' + id;
        $.ajax({
            url: URL,
            type: 'Get',
            dataType: 'json',
            contentType: 'application/ json; charset = utf - 8;',
            success: function (result) {
                if (result.success) {
                    var Obj = "";
                    var count = result.value.length; 
                    var man11 = 0;
                    result.value.forEach(function (item, index) {
                        if (total <= man11)
                        {
                            return;
                        }
                        Obj += ' <div class="form-group">\
             <label>Serial no of Product</label>\
            <input type="text" class="form-control" id="S-'+ index + '" value="' + item + '" placeholder="Serial no">\
             </div>';
                        man11++;
                    });
                    for (i = man11; i < total; i++) {
                        Obj += ' <div class="form-group">\
             <label>Serial no of Product</label>\
            <input type="text" class="form-control" id="S-'+ i + '" placeholder="Serial no">\
             </div>';
                    }
                    $("#tada").html(Obj);
                }
                else {
                    $("#tada").empty();
                    toastr["error"](result.message, "Something went wrong", { timeOut: 5000 });
                }
            }
        });
    }
}

function Edit(i, product, quantity) {
    $("#ID").val(i);
    $("#ProductName").val(product);
    $("#Quantity").val(quantity);
    Getit(i);
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


function reloadTable() {
    $.ajax({
        type: 'Get',
        url: '/Order/GetBucket',
        data: 'json',
        contentType: 'application/ json; charset = utf - 8;',
        success: function (result) {
            var Obj = "";
            $.each(result, function (index, value) {
                Obj += '<tr>';
                Obj += '<td>' + value.id + '</td>';
                Obj += '<td>' + value.product.name + '</td>';
                Obj += '<td>' + value.quantity + '</td>';
                Obj += '<td><a class="btn btn-danger" onclick=Delete("/Order/DeleteOrderhasproduct?id=' + value.id + '")><i class="bi bi-trash"></i> Delete</a></td>';
                Obj += '<td><button class="btn btn-success" onclick=Edit(' + value.id + ',' + value.product_id + ',' + value.quantity+') data-toggle="modal" \
        data-target="#exampleModal">Edit</button></td>';
                Obj += '</tr>';
            })
            $("#t-body").html(Obj);
        }
    });
}



$("#AddProduct").on("submit", function (e) {
    e.preventDefault();
    var Id = $("#ID").val();
    var ProductName = $("#ProductName").val(); 
    var Quantity = $("#Quantity").val();
    var mylist = [];
    for (let i = 0; i < Quantity; i++) {
        let lett = "#S-" + i;
        mylist.push($(lett).val());
    }

    var obj1 = {
        id: Id,
        product_id: ProductName,
        quantity: Quantity,
    };
    var obj = {
        order_product: obj1,
        serial_no: mylist
    };
    $.ajax({
        url: '/order/Orderhasproductadd',
        type: 'Post',
        dataType: 'json',
        data: obj,
        success: function (response) {
            if (response.success) {
                toastr["success"](response.message, "Value Added", { timeOut: 5000 });
                document.getElementById("AddProduct").reset();
                $("#ID").val(0);
                reloadTable();
            }
            else {
                toastr["error"](response.message, "Not entered", { timeOut: 5000 });
            }
        },
        error: function (xhr, textStatus, error) {
            console.log("error");
            alert(xhr.statusText);
            console.log(textStatus);
            console.log(error);
        }
    });
});

function AddOrder() {
    var ID = 0;
    var Date1 = $("#Date").val();
    var Invoice = $("#Invoice").val();
    var Vendor = $("#vendor_id").val();
    var obj = {
        id: ID,
        arival: Date1,
        invoice_no: Invoice,
        vendor_id: Vendor
    };

    $.ajax({
        url: '/order/AddnewOrder',
        type: 'Post',
        dataType: 'json',
        data: obj,
        success: function (response) {
            if (response.success) {
                toastr["success"](response.message, "Order Added", { timeOut: 5000 });
                document.getElementById("AddProduct").reset();
                document.getElementById("s-form").reset();
                reloadTable();
            }
            else {
                toastr["error"](response.message, "Not entered", { timeOut: 5000 });
            }
        },
        error: function (xhr, textStatus, error) {
            console.log("error");
            alert(xhr.statusText);
            console.log(textStatus);
            console.log(error);
        }
    });
   
}