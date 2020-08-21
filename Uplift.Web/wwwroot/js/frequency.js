var frequencyTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    frequencyTable = $('#tblFrequency').DataTable({
        ajax: {
            url: '/Admin/Frequency/GetAll',
            dataSrc: 'data'
        },
        columns: [
            { data: 'name', width: '20%' },
            { data: 'frequencyCount', width: '50%' },
            {
                data: 'id',
                render: function (data) {
                    return `<div class="row">
                                <a href="/Admin/Frequency/Upsert/${data}" class="btn btn-success"><i class="fas fa-edit"></i>&nbsp;Edit</a>
                                &nbsp;
                                <a onclick=deleteFrequency("/Admin/Frequency/Delete/${data}") class="btn btn-danger"><i class="fas fa-trash-alt"></i>&nbsp;Delete</a>
                            </div>
                    `
                },
                width: '30%'
            }
        ]
    });
}

function deleteFrequency(url) {
    swal({
        title: "Are you sure to delete?",
        text: "You will not be able to restore this frequency!",
        icon: "warning",
        buttons: [true, "Yes, delete it!"],
        dangerMode: true
    }).then(willDelete => {
        if (willDelete) {
            $.ajax({
                url: url,
                method: 'delete'
            }).done(data => {
                if (data.success) {
                    toastr.success(data.message);
                }
                else {
                    toastr.error(data.message);
                }
                frequencyTable.ajax.reload();
            });
        }
    });
}