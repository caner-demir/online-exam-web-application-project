var dataTable

$(document).ready(function () {
    loadCounterValues()
    loadDataTable()
})

// ------------------------------- Code section for counter panel. ---------------------------
function loadCounterValues() {
    $.ajax({
        type: "GET",
        url: '/Teacher/Course/GetCounter',
        success: function (response) {
            $("#student-counter").html(response.counter.studentCounter)
            $("#course-counter").html(response.counter.courseCounter)
            $("#exam-counter").html(response.counter.examCounter)
            $("#question-counter").html(response.counter.questionCounter)
        }
    })
}

// ------------------------------- Code section for courses table. ---------------------------
function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "autoWidth": false,
        "columnDefs": [
            { className: "table-active table-buttons", "targets": [0] },
            { className: "table-buttons", "targets": [2] },
            { className: "table-active table-buttons", "targets": [3] }
        ],
        "order": [[3, "asc"]],
        "ajax": {
            "url": "/Teacher/Course/GetAll"
        },
        "columns": [
            {
                "data": {
                    "id": "id",
                    "name": "name"
                },
                "render": function (data) {
                    return `
                            <a href="/Teacher/Exam/Index/${data.id}" class="text-dark font-weight-bold" style="font-size: 120%">${data.name}</a>
                            `
                }, "width": "25%"
            },
            {
                "data": "imageUrl",
                "render": function (data) {
                    return `
                            <div class="rounded d-flex align-items-center overflow-hidden" style="max-height:100px;">
                                <img src="${data}" class="w-100" >
                            </div>
                            `
                }, "width": "25%"
            },
            {
                "data": "dateCreated",
                "render": function (data) {
                    return moment(data).format('MMMM DD YYYY, HH:mm')
                },
                "width": "25%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick="openModal('/Teacher/Course/Upsert/${data}', 'Edit Course')" 
                                        class="btn btn-success text-white" style="cursor:pointer; width:94px">
                                    <i class="fas fa-edit"></i>&nbsp; Edit
                                </a>
                                <a onclick=Delete("/Teacher/Course/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>&nbsp; Delete
                                </a>
                            </div>
                            `
                }, "width": "25%"
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