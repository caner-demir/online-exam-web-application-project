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
            { className: "table-container-lg", "targets": [0] },
            { className: "table-cells-lg", "targets": [2, 3, 4] },
            { className: "table-active table-cells-lg", "targets": [5] }
        ],
        "order": [[5, "asc"]],
        "ajax": {
            "url": "/Teacher/Course/GetAll"
        },
        "columns": [
            {
                "data": "course",
                "render": function (data) {
                    return `
                            <a href="/Teacher/Exam/Index/${data.id}" class="text-dark font-weight-bold table-button-center" style="font-size: 110%">${data.name}</a>
                            `
                }, "width": "20%"
            },
            {
                "data": "course.imageUrl",
                "render": function (data) {
                    return `
                            <div class="rounded d-flex align-items-center overflow-hidden border border-secondary" style="max-height:100px;">
                                <img src="${data}" class="w-100" >
                            </div>
                            `
                }, "width": "25%"
            },
            {
                "data": "course.dateCreated",
                "render": function (data) {
                    return moment(data).format('MMM DD YYYY, HH:mm')
                },
                "width": "18%"
            },
            {
                "data": "students",
                "render": function (data) {
                    return `${data} <i class="fas fa-user-friends"></i>`
                },
                "width": "12%"
            },
            {
                "data": "exams",
                "render": function (data) {
                    return `${data}&nbsp; <i class="fas fa-clipboard"></i>`
                },
                "width": "12%"
            },
            {
                "data": "course",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick="openModal('/Teacher/Course/Upsert/${data.id}', 'Edit Course')" 
                                        class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/Teacher/Course/Delete/${data.id}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>
                            `
                }, "width": "23%"
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