$(document).ready(function(){
    reloadTable();
});

function PView() {
    var URL = '/bill/lokPay?id=' + $("#Bid").val();
    $.ajax({
        url: URL,
        data: 'json',
        type: 'Get',
        contentType: 'application/json; charset= utf-8;',
        success: function (result) {
            var Obj = "";
            $.each(result, function (index, value) {
                Obj += '<tr>';
                Obj += '<td>' + String(value.date).slice(0, 10) + '</td>';
                Obj += '<td>' + value.method + '</td>';
                Obj += '<td>' + value.amount + '</td>';
                Obj += '<td>' + value.commission + '</td>';
                Obj += '<td>' + value.commissionper + ' %</td>';
                Obj += '<td>' + value.commissino_to + ' </td>';
                Obj += '<td><a class="btn btn-danger" onclick=DeletePayment(' + value.id + ')>Delete</a>';
                Obj += '</td>';
            });
            $("#Pd-body").html(Obj);
        }
    })
}

function DeletePayment(id) {
    var Url = '/bill/deletePayment?id=' + id;
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
                        $("#Payment").val(data.pay);
                        PView();
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
function View(id) {
    var URL = '/bill/stockid?id=' + id;
    $.ajax({
        url: URL,
        data: 'json',
        type: 'Get',
        contentType: 'application/json; charset= utf-8;',
        success: function (result) {
            var Obj = "";
            $.each(result, function (index, value) {
                Obj += '<tr>';
                Obj += '<td>' + value.serial_number + '</td>';
                Obj += '<td>' + value.id + '</td>';
                Obj += '</td>';
            });
            $("#tm-body").html(Obj);
        },

    });

}

$("#Payment").change(function () {
    $(".ToRemove1").remove();
    calVat();
});



function calVat()
{
    var payment = $("#Payment").val();
    var after_TDS = Number($("#t-otal").html()) - Number($("#TDS").html());
    var after_vat = after_TDS + Number($("#VAT").html());
    //console.log(Number($("#t-otal").html()));
    //console.log(after_vat);
    if (payment < after_TDS) {
        var Obj = '<tr class="ToRemove1">';
        Obj += '<td><td>';
        Obj += '<th>Note</th>';
        Obj += '<td >Payment not enough to calculate VAT</td>';
        Obj += '</tr>';
        $("#t-body").append(Obj);
    } else if (payment >after_vat)
    {
        var Obj = '<tr class="ToRemove1">';
        Obj += '<td><td>';
        Obj += '<th>Note</th>';
        Obj += '<td >Something Went Wrong Payment is more than required amt</td>';
        Obj += '</tr>';
        $("#t-body").append(Obj);
    }
    else
    {
        var received_vat = payment - after_TDS;
        var vat_percent = received_vat / Number($("#VAT").html()) * 100;
        
        var Obj = '<tr class="ToRemove1">';
        Obj += '<td><td>';
        Obj += '<th>Received VAT</th>';
        Obj += '<td >' + received_vat + '</td>';
        Obj += '</tr>';

        Obj += '<tr class="ToRemove1">';
        Obj += '<td><td>';
        Obj += '<th>received VAT percent</th>';
        Obj += '<td >' + vat_percent.toPrecision(4) + ' %</td>';
        Obj += '</tr>';

        Obj += '<tr class="ToRemove1">';
        Obj += '<td><td>';
        Obj += '<th>Govenment VAT percent</th>';
        Obj += '<td >' + (100 - vat_percent.toPrecision(4)).toPrecision(4) + ' %</td>';
        Obj += '</tr>';
        $("#t-body").append(Obj);
    }

}
function reloadTable() {
    var URL = '/bill/getCombill?id=' + $("#ID").val();
    $.ajax({
        url: URL,
        data: 'json',
        type: 'Get',
        contentType: 'application/ json; charset = utf - 8;',
        success: function (result) {
            var Obj = "";
            var total = 0;
            $.each(result, function (index, value) {
                Obj += '<tr>';
                if (value.product === null) {
                    Obj += '<td>' + value.extra_items + '</td>';
                }
                else {
                    Obj += '<td>' + value.product.name + '</td>';
                }
                Obj += '<td>' + value.rate + '</td>';
                Obj += '<td>' + value.quantity + '</td>';
                Obj += '<td>' + value.total + '</td>';
                Obj += '<td class="no-print"> <button onclick=View(' + value.id + ') class="btn btn-success"  data-toggle="modal" data-target="#exampleModal">View</button> '; 
                total += value.total;
                Obj += '</tr>';
            });
            Obj += '<tr>';
            Obj += '<td><td>';
            Obj += '<th>Total</th>';
            Obj += '<td id="t-otal">' + total + '</td>';
            Obj += '</tr>';
            var VAT = total * 0.13;
            Obj += '<tr>';
            //
            Obj += '<td><td>';
            Obj += '<th>13% VAT</th>';
            Obj += '<td id="VAT">' + VAT + '</td>';
            Obj += '</tr>';
            //
            var TDS = (total * 0.015);
            Obj += '<tr>';
            Obj += '<td><td>';
            Obj += '<th>TDS</th>';
            Obj += '<td id="TDS">' + TDS + '</td>';
            Obj += '</tr>';
            //
            Obj += '<tr>';
            Obj += '<td><td>';
            Obj += '<th>G. Total</th>';
            Obj += '<td id="G-total">' + (total + VAT - TDS) + '</td>';
            Obj += '</tr>';
            //
            $("#t-body").html(Obj);
           
            if ($("#Payment").val != null) {
                calVat();
            }
        }
    });
}

function printThisq() {
    $("#Toprint").printThis();
}

function printBill() {
    $("#billPrint").printThis();
}



$("#PaymentForm").on("submit", function (e) {
    e.preventDefault();
    var Id = 0;
    var Mthod = $("#Pmethod").val();
    var Amount = $("#PAmount").val();
    var HiDate = $("#PDate").val();
    var Bill_id = $("#Bid").val();
    var User_id = $("#sold_By").val();
    var commission = $("#Commission").val();
    var Obj = {
        id: Id,
        method: Mthod,
        amount: Amount,
        commission: commission,
        bill_id: Bill_id,
        commissino_to: User_id
    }
    var man = {
        obj: Obj,
        hiDate: HiDate
    }
    $.ajax({
        url: '/bill/Paid',
        type: 'POST',
        dataType: 'json',
        data: man,
        success: function (response) {
            if (response.success) {
                toastr["success"](response.message, "Payment Added", { timeOut: 5000 });
                $("#Payment").val(response.pay);
                document.getElementById("PaymentForm").reset();
                reloadTable();
            }
            else {
                toastr["error"](response.message, "Not entered", { timeOut: 5000 });
            }
        },
        error: function (xhr, textStatus, error) {
            //console.log("error");
            //alert(xhr.statusText);
            console.log(textStatus);
            console.log(error);
        }
    });
});