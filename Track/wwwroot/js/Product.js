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
        url: '/Product/GetAll',
        type: 'Get',
        dataType: 'json',
        ContentType: 'application/json;charset=utf-8;',
        success: function (result, status, xhr) {
            var Obj = "";
            var Instock = "";
            $.each(result, function (index, value) {
                if (mval == "" || mval == String(value.id).toUpperCase()
                    || mval == String(value.name).toUpperCase() || mval == String(value.description).toUpperCase() ||
                    mval == String(value.category).toUpperCase() || mval == String(value.company).toUpperCase()) {
                    Obj += '<tr>';
                    Obj += '<td>' + value.id + '</td>';
                    mySet.add(String(value.id).toUpperCase());
                    Obj += '<td>' + value.name + '</td>';
                    mySet.add(String(value.name).toUpperCase());
                    Obj += '<td>' + value.modal + '</td>';
                    mySet.add(String(value.modal).toUpperCase());
                    Obj += '<td>' + value.description + '</td>';
                    mySet.add(String(value.description).toUpperCase());
                    Obj += '<td>' + value.category + '</td>';
                    mySet.add(String(value.category).toUpperCase());
                    Obj += '<td>' + value.company + '</td>';
                    mySet.add(String(value.company).toUpperCase());
                    if (typeof value.in_stock != "number") {
                        Instock = 0;
                    }
                    else {
                        Instock = value.in_stock;
                    }
                    Obj += '<td>' + Instock + '</td>';
                    Obj += '<td><a class="btn btn-danger" onclick=Delete("/Product/Delete?id=' + value.id + '")><i class="bi bi-trash"></i> Delete</a></td>';

                }
            });
            autocomplete(document.getElementById("myInput"), Array.from(mySet));
            $("#t-body").html(Obj);
        },
        error: function () {
            alert("there was an error");
        }
    })
}

function Delete(Url)
{

    $.confirm({
        title: 'Delete',
        content: 'Are you Sure?',
        buttons: {
            confirm: function () {
                $.ajax({
                    url: Url,
                    type: 'delete',
                    success: function (data) {
                        if (data.success) {
                            toastr["success"](data.message, "Value Deleted", { timeOut: 5000 });
                            reloadTable("");
                        }
                        else {
                            toastr["error"](data.message, "Error", { timeOut: 5000 });
                        }
                        
                    },

                })
            },
            cancel: function () {
                $.alert('Canceled!');
            }
        }
    });
}

$("#my-form").on("submit", function (e) {
    e.preventDefault();
    var Id = $("#id").val();
    var Name = $("#Name").val();
    var Description = $("#Description").val();
    var Catagory = $("#C-at").val();
    var Company = $("#Company").val();
    var obj = {
        id: Id,
        name: Name,
        description: Description,
        category: Catagory,
        company: Company
    };
    $.ajax({
        url: '/product/Index',
        type: 'Post',
        dataType: 'json',
        data: obj,
        success: function (response) {
            if (response.success) {
                toastr["success"](response.message, "Value Added", { timeOut: 5000 });
                document.getElementById("my-form").reset();
                reloadTable("");
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