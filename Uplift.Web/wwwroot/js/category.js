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
                data: 'id',
                render: function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Category/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer;width:100px"><i class="fas fa-edit"></i> Edit</a>
                                &nbsp;
                                <a onclick=Delete("/Admin/Category/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer;width:100px"><i class="fas fa-trash-alt"></i> Delete</a>
                            <div>
                            `
                },
                width: '30%'
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: 'Are you sure to delete?',
        text: "You will not be able to recover this category.",
        icon: "warning",
        buttons: [true, "Yes, delete it!"],
        dangerMode: true
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: url,
                    method: "DELETE"
                }).done(function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                    dataTable.ajax.reload();
                });
            }
        });
}