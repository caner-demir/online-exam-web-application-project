var dataTable

$(document).ready(function () {
    loadDataTable()
})

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "autoWidth": false,
        "order": [[1, "asc"]],
        "columnDefs": [{
                "searchable": false,
                "orderable": false,
                "className": "table-active font-weight-bold table-buttons",
                "targets": 0,
            },
            { className: "table-buttons", "targets": [2] },
            { className: "table-active table-buttons", "targets": [3] }
        ],
        "ajax": {
            "url": "/Teacher/Question/GetAll"
        },
        "columns": [
            {"data": "id", "width": "10%" },
            {
                "data": "imageUrl",
                "render": function (data) {
                    return `
                            <div class="rounded d-flex align-items-center overflow-hidden" style="max-height:100px;">
                                <img src="${data}" class="w-100" >
                            </div>
                            `
                }, "width": "35%"
            },
            {
                "data": "dateCreated",
                "render": function (data) {
                    return moment(data).format('MMMM DD YYYY, HH:mm')
                },
                "width": "25%"
            },
            {
                "data": {
                    "id": "id",
                    "examId": "examId"
                },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick="openModal('/Teacher/Question/Upsert/${data.id}?examId=${data.examId}', 'Edit Question')" 
                                        class="btn btn-success text-white" style="cursor:pointer; width:94px">
                                    <i class="fas fa-edit"></i>&nbsp; Edit
                                </a>
                                <a onclick=Delete("/Teacher/Question/Delete/${data.id}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>&nbsp; Delete
                                </a>
                            </div>
                            `
                }, "width": "30%"
            }
        ]
    })

    //Give index numbers to rows.
    dataTable.on('order.dt search.dt', function () {
        dataTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = `<span style="font-size: 120%">${i + 1}</span>`;
        });
    }).draw();
}

function Delete(url) {
    swal({
        title: "Are you sure you want to delete?",
        text: "You will not be able to recover the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message)
                        dataTable.ajax.reload()
                    }
                    else {
                        toastr.error(data.message)
                    }
                }
            })
        }
    })
}