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
        url: '/Main/GetVendor',
        type: 'Get',
        dataType: 'json',
        contentType: 'application/jon; charset=utf-8',
        success: function (result) {
           
            var Obj = "";
            $.each(result, function (index, value) {
                if (mval == String(value.id).toUpperCase() || mval == String(value.name).toUpperCase() 
                || mval == String(value.description).toUpperCase() || mval == String(value.phoneNumber).toUpperCase() || mval == "") {
                    Obj += '<tr class="text-center">';
                    Obj += '<td>' + value.id + '</td>';
                    mySet.add(String(value.id).toUpperCase());
                    Obj += '<td>' + value.name + '</td>';
                    mySet.add(String(value.name).toUpperCase());
                    Obj += '<td>' + value.description + '</td>';
                    mySet.add(String(value.description).toUpperCase());
                    Obj += '<td>' + value.phoneNumber + '</td>';
                    mySet.add(String(value.phoneNumber).toUpperCase());
                    Obj += '<td><button type="button" class="btn  btn-success" data-toggle="modal" data-target="#exampleModal" onclick="OneAdd(' + value.id + ', \'' + value.name + '\',\'' + value.description + '\',' + value.phoneNumber + ')"><i class="bi bi-pencil-square"></i> Edit</button></td>';
                    Obj += '<td><a class="btn btn-danger" onclick=Delete("/Main/DeleteVendor?id=' + value.id + '")><i class="bi bi-trash"></i> Delete</a></td>';
                    Obj += '</tr>'
                }
                
            });
            autocomplete(document.getElementById("myInput"), Array.from(mySet));
            $("#t-body").html(Obj);

        },
        error: function () {
            alert("There was error fetching Data");
        }
    })
}

function OneAdd(id, name, Description, PhoneNumber) {
    $("#ID").val(id);
    $("#Name").val(name);
    $("#Description").val(Description);
    $("#Phonenumber").val(PhoneNumber);
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
                        if (data.success) {
                            toastr["success"](data.message, "Value Deleted", { timeOut: 5000 });
                            reloadTable("");
                        }
                        else {
                            toastr["error"](data.message, "Value Deleted", { timeOut: 5000 });
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

$("#myForm").on("submit", function (e) {
    e.preventDefault();
    var Id = $("#ID").val();
    var Name = $("#Name").val();
    var PhoneNumber = $("#Phonenumber").val();
    var Description = $("#Description").val();
    var obj = {
        id: Id,
        name: Name,
        phonenumber: PhoneNumber,
        description: Description,
    };

    $.ajax({
        url: '/Main/Addvendor',
        type: 'Post',
        dataType: 'json',
        data: obj,
        success: function (response) {
            if (response.success) {
                toastr["success"](response.message, response.type, { timeOut: 5000 });
                document.getElementById("myForm").reset();
                reloadTable("");

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