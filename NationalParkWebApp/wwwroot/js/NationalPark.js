var dataTable;

$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "NationalPark/GetAll",
            "type": "Get",
            "dataType":"Json"           
        },
        "columns": [
            { "data": "name", "width": "40%" },
            { "data": "state", "width": "40%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                     <div class="text-center">
                      <a href="NationalPark/Upsert/${data}"class="btn btn-info">
                       <i class="fas fa-edit"></i>
                      </a>
                      <a class="btn btn-danger"onclick=Delete("NationalPark/Delete/${data}")>
                       <i class="fas fa-trash-alt"></i>
                      </a>
                     </div>
                    `;
                }
            }
        ]
    })
}

function Delete(url) {
    swal({
        title: "want to delete data?",
        text: "Delete information",
        icon: "warning",
        buttons: true,
        dangerModel: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
} 
