var dataTable

$(document).ready(function () {
    loadDataTable()
})

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Teacher/Question/GetAll"
        },
        "columns": [
            {
                "data": {
                    "id": "id",
                    "name": "name"
                },
                "render": function (data) {
                    return `
                            <a onclick="openModal('/Teacher/Question/Upsert/${data.id}?examId=${data.examId}', 'Edit Question')"
                                    class="text-dark" style="cursor:pointer">${data.name}</a>
                            `
                }, "width": "60%"
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
                                        class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/Teacher/Question/Delete/${data.id}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>
                            `
                }, "width": "40%"
            }
        ]
    })
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