var dataTable

$(document).ready(function () {
    loadDataTable()
})

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Teacher/Exam/GetAll"
        },
        "columns": [
            { "data": "name", "width": "60%" },
            {
                "data": {
                    "id": "id",
                    "courseId": "courseId"
                },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Teacher/Exam/Upsert/${data.id}?courseId=${data.courseId}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/Teacher/Exam/Delete/${data.id}") class="btn btn-danger text-white" style="cursor:pointer">
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