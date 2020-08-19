var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            url: '/Admin/Category/GetAll',
            dataSrc: 'data'
        },
        columns: [
            { data: 'name', width: '50%' },
            { data: 'displayOrder', width: '20%' },
            {
                data: function (row) {
                    return `<div class="btn-toolbar" role="group">
                                <div class="btn-group mr-2">
                                    <a class="btn btn-info" href="/Admin/Category/Upsert/${row.id}"><i class="fas fa-edit"></i> Edit</a>
                                </div>
                                <div class="btn-group mr-2">
                                    <a class="btn btn-danger" onclick=Delete(${row.id}) asp-action="Upsert"><i class="fas fa-trash-alt"></i> Delete</a>
                                </div>
                            <div>
                            `
                }
                , width: '30%'
            }
        ]
    });
}

function Delete(id) {
    swal({
        title: 'Are you sure to delete?',
        text: "You will not be able to recover this category.",
        icon: "warning",
        buttons: true,
        dangerMode: true
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: "/Admin/Category/Delete/" + id,
                    method: "DELETE"
                }).done(function (data) {
                    if (data.success) {
                        toastr["success"](data.message);
                    }
                    else {
                        toastr["error"](data.message);
                    }
                    dataTable.ajax.reload();
                });
            }
        });
}