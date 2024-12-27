var mySet = new Set();
$(document).ready(function () {
    reloadTable("");
    fillOptions();
});
function fillCustomer()
{

}
$("#form-search").on("submit", function (e) {
    e.preventDefault();
    var mval = String($("#myInput").val()).toUpperCase();
    reloadTable(mval);
});
function reloadTable(mval) {

    $.ajax({
        url: '/Main/getAllcutomer',
        type: 'Get',
        dataType: 'json',
        contentType: 'application/jon; charset=utf-8',
        success: function (result) {
            var Obj = "";
            $.each(result, function (index, value) {
                if (mval == "" || mval == String(value.id).toUpperCase()
                    || mval == String(value.name).toUpperCase() || mval == String(value.phoneNumber).toUpperCase() ||
                    mval == String(value.province.name).toUpperCase() || mval == String(value.district.name).toUpperCase()
                    || mval == String(value.localBody.name).toUpperCase())
                {
                    Obj += '<tr class="text-center">';
                    Obj += '<td>' + value.id + '</td>';
                    mySet.add(String(value.id).toUpperCase());
                    Obj += '<td>' + value.name + '</td>';
                    mySet.add(String(value.name).toUpperCase());
                    nameSet.add(String(value.name).toUpperCase());
                    if (value.phoneNumber != null) {
                        Obj += '<td>' + value.phoneNumber + '</td>';
                        mySet.add(String(value.phoneNumber).toUpperCase());

                    }

                    if (value.provinceId != null) {
                        Obj += '<td>' + value.province.name + '</td>';
                        mySet.add(String(value.province.name).toUpperCase());
                        Obj += '<td>' + value.district.name + '</td>';
                        mySet.add(String(value.district.name).toUpperCase());
                        Obj += '<td>' + value.localBody.name + '</td>';
                        mySet.add(String(value.localBody.name).toUpperCase());
                        Obj += '<td>' + value.address + '</td>';
                        mySet.add(String(value.address).toUpperCase());

                    }
                    Obj += '<td><button type="button" class="btn  btn-success" data-toggle="modal" data-target="#exampleModal" \
                onclick=OneAdd(' + value.id + ',\'' + encodeURIComponent(value.name) + '\',' + value.phoneNumber + ',' + value.provinceId + ',' + value.districtId + ',' + value.localBodyId + ')><i class="bi bi-pencil-square"></i> Edit</button></td>';
                    Obj += '<td><a class="btn btn-danger" onclick=Delete("/Main/Delete?id=' + value.id + '")><i class="bi bi-trash"></i> Delete</a></td>';
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

