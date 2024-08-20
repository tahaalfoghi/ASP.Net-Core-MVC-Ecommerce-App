var tbl;

$(document).ready(function () {
    LoadData();
});

function LoadData() {
    tbl = $("#pdata").DataTable({
        "ajax": {
            "url": "/Admin/Product/GetProducts",
             "type":"GET",
            "datatype": "JSON"
          
        },
        "columns": [
           
            { "data": "name" },
            { "data": "description" },
            { "data": "price" },
            { "data": "category.name" },
            {
                data: "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group " role="group">
                     <a href="/Admin/Product/Update/${data}" class="btn btn-warning text-white "> <i class="bi bi-pencil-square"></i> Edit</a>               
                     <a onClick=Delete('/Admin/Product/Delete/${data}') class="btn btn-danger  text-white"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`;
                },
            }
        ]
    });
}

function Delete(delete_url) {
    Swal.fire({
        title: "Are you sure you want to delete this product",
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
