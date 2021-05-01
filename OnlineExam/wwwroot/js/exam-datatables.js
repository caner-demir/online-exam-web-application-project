var dataTable
var dataTableWaiting

$(document).ready(function () {
    loadTableExams()
    loadTableWaiting()
})

function loadTableExams() {
    dataTable = $("#table-exam").DataTable({
        "ajax": {
            "url": "/Teacher/Exam/GetAll"
        },
        "columns": [
            {
                "data": {
                    "id": "id",
                    "name": "name"
                },
                "render": function (data) {
                    return `
                            <a href="/Teacher/Question/Index/${data.id}" class="text-dark">${data.name}</a>
                            `
                }, "width": "60%"
            },
            {
                "data": {
                    "id": "id",
                    "courseId": "courseId"
                },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick="openModal('/Teacher/Exam/Upsert/${data.id}?courseId=${data.courseId}', 'Edit Exam')" 
                                        class="btn btn-success text-white" style="cursor:pointer">
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

function loadTableWaiting() {
    dataTableWaiting = $("#table-waiting").DataTable({
        "ajax": {
            "url": "/Teacher/Student/GetWaiting"
        },
        "columns": [
            { "data": "userName", "width": "60%" },
            {
                "data": {
                    "name": "name",
                    "userName": "userName"
                },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick=acceptWaiting('${data.userName}') class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-check-square"></i> Confirm
                                </a>
                                <a onclick=deleteWaiting('${data.userName}') class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> Delete
                                </a>
                            </div>
                            `
                }, "width": "40%"
            }
        ]
    })
}

function acceptWaiting(userName) {
    $.ajax({
        type: "POST",
        url: '/Teacher/Student/Accept/',
        data: JSON.stringify(userName),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message)
                dataTableWaiting.ajax.reload()
            }
            else {
                toastr.error(data.message)
            }
        }
    })
}

function deleteWaiting(userName) {
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
                url: "/Teacher/Student/Delete/",
                data: JSON.stringify(userName),
                contentType: "application/json",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message)
                        dataTableWaiting.ajax.reload()
                    }
                    else {
                        toastr.error(data.message)
                    }
                }
            })
        }
    })
}