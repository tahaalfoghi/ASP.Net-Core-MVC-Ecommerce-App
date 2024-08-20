var tbl;

$(document).ready(function() {

    LoadData();
});

function LoadData() {
    
    tbl = $("#tbldata").DataTable({

        "ajax": {
            "url": '/Admin/Category/GetCategories',
            "type": "GET",
            "datatype":"json"
        },

      
        "columns": [
            { data: "name" },
            { data: "description" },
            { data: "createdAt" },
            {
                data: "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/Admin/Category/Update/${data}" class="btn btn-warning "> <i class="bi bi-pencil-square"></i> Edit</a>               
                     <a onClick=Delete('/Admin/Category/Delete/${data}') class="btn btn-danger "> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`;
                }
            }
        ]
    });
}


function Delete(delete_url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: delete_url,
                type: 'DELETE',
                success: function (data) {
                    tbl.ajax.reload();
                    toastr.success(data.message);
                    Swal.fire('Deleted!', 'Your file has been deleted.', 'success');
                }
            });
        }
        else {
            console.log("Oooop! something went wrong");
        }
    });
}
