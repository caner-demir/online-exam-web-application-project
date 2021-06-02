var dataTable
var dataTableStudents
var dataTableRequests

$(document).ready(function () {
    loadCounterValues()
    loadTableExams()
    loadTableStudents()
    loadTableRequests()
})


// ------------------------------- Code section for counter panel. ---------------------------
function loadCounterValues() {
    $.ajax({
        type: "GET",
        url: '/Teacher/Exam/GetCounter',
        success: function (response) {
            $("#student-counter").html(response.counter.studentCounter)
            $("#exam-counter").html(response.counter.examCounter)
            $("#question-counter").html(response.counter.questionCounter)
            $("#request-counter").html(response.counter.requestCounter)
        }
    })
}


// ------------------------------- Code section for exams table. -----------------------------
function loadTableExams() {
    dataTable = $("#table-exam").DataTable({
        "autoWidth": false,
        "columnDefs": [
            { className: "table-active table-container-sm", "targets": [0] },
            { className: "table-cells-sm", "targets": [1, 2, 3, 4] },
            { className: "table-active", "targets": [5] }
        ],
        "order": [[5, "asc"]],
        "ajax": {
            "url": "/Teacher/Exam/GetAll"
        },
        "columns": [
            {
                "data": "exam",
                "render": function (data) {
                    return `
                            <a href="/Teacher/Question/Index/${data.id}" class="text-dark font-weight-bold table-button-center" style="font-size: 110%">${data.name}</a>
                            `
                }, "width": "20%"
            },
            {
                "data": "exam",
                "render": function (data) {
                    return moment(data.dateCreated).format('MMM DD YYYY')
                },
                "width": "16%"
            },
            {
                "data": "exam",
                "render": function (data) {
                    return moment(data.startDate).format('MMM DD YYYY, HH:mm')
                },
                "width": "16%"
            },
            {
                "data": "exam",
                "render": function (data) {
                    return data.duration.hours + " Hours, " + data.duration.minutes + " Minutes"
                },
                "width": "16%"
            },
            {
                "data": "questions",
                "render": function (data) {
                    return data
                },
                "width": "12%"
            },
            {
                "data": "exam",
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
                }, "width": "50%"
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
                        loadCounterValues()
                    }
                    else {
                        toastr.error(data.message)
                    }
                }
            })
        }
    })
}

// ------------------------------- Code section for students table. -----------------------------
function loadTableStudents() {
    dataTableStudents = $("#table-students").DataTable({
        "autoWidth": false,
        "columnDefs": [
            { className: "table-active", "targets": [0, 3] },
            { className: "table-cells-sm", "targets": [0, 1, 2] }
        ],
        "ajax": {
            "url": "/Teacher/Student/GetStudents"
        },
        "columns": [
            {
                "data": "name",
                "render": function (data) {
                    return `<span class="text-dark font-weight-bold" style="font-size: 110%">${data}</span>`
                },
                "width": "25%"
            },
            { "data": "userName", "width": "25%" },
            {
                "data": "dateCreated",
                "render": function (data) {
                    return moment(data).format('MMMM DD YYYY, HH:mm')
                }, "width": "25%"
            },
            {
                "data": {
                    "name": "name",
                    "userName": "userName"
                },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick=deleteStudent('${data.userName}') class="btn btn-danger text-white" style="cursor:pointer; width:113px">
                                    <i class="fas fa-user-times"></i>&nbsp; Delete
                                </a>
                            </div>
                            `
                }, "width": "25%"
            }
        ]
    })
}

function deleteStudent(userName) {
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
                        dataTableStudents.ajax.reload()
                        loadCounterValues()
                    }
                    else {
                        toastr.error(data.message)
                    }
                }
            })
        }
    })
}

// ------------------------------- Code section for enrollment requests table. -----------------------------
function loadTableRequests() {
    dataTableRequests = $("#table-requests").DataTable({
        "autoWidth": false,
        "columnDefs": [
            { className: "table-active", "targets": [0] },          
            { className: "table-cells-sm", "targets": [0, 1, 2] },
            { className: "table-active", "targets": [3] }
        ],
        "ajax": {
            "url": "/Teacher/Student/GetRequests"
        },
        "columns": [
            {
                "data": "name",
                "render": function (data) {
                    return `<span class="text-dark font-weight-bold" style="font-size: 110%">${data}</span>`
                },
                "width": "25%"
            },
            { "data": "userName", "width": "25%" },
            {
                "data": "dateCreated",
                "render": function (data) {
                    return moment(data).format('MMMM DD YYYY, HH:mm')
                }, "width": "25%"
            },
            {
                "data": {
                    "name": "name",
                    "userName": "userName"
                },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick=acceptRequest('${data.userName}') class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-user-check"></i>&nbsp; Confirm
                                </a>
                                <a onclick=deleteRequest('${data.userName}') class="btn btn-danger text-white" style="cursor:pointer; width:113px">
                                    <i class="fas fa-trash-alt"></i>&nbsp; Delete
                                </a>
                            </div>
                            `
                }, "width": "25%"
            }
        ]
    })
}

function acceptRequest(userName) {
    $.ajax({
        type: "POST",
        url: '/Teacher/Student/Accept/',
        data: JSON.stringify(userName),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message)
                dataTableRequests.ajax.reload()
                dataTableStudents.ajax.reload()
                loadCounterValues()
            }
            else {
                toastr.error(data.message)
            }
        }
    })
}

function deleteRequest(userName) {
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
                        dataTableRequests.ajax.reload()
                        loadCounterValues()
                    }
                    else {
                        toastr.error(data.message)
                    }
                }
            })
        }
    })
}