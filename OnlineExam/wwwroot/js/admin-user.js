var dataTable

$(document).ready(function () {
    loadCounterValues()
    loadDataTable()
})

// ------------------------------- Code section for counter panel. ---------------------------
function loadCounterValues() {
    $.ajax({
        type: "GET",
        url: '/Student/Home/GetCounter',
        success: function (response) {
            $("#user-counter").html(response.counter.userCounter)
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
            { className: "table-active table-cells-sm", "targets": [0, 5] },
            { className: "table-cells-sm", "targets": [0, 1, 2, 3, 4] }
        ],
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            {
                "data": "applicationUser.name",
                "render": function (data) {
                    return `<span class="text-dark font-weight-bold" style="font-size: 110%">${data}</span>`
                },
                "width": "20%"
            },
            { "data": "applicationUser.userName", "width": "25%" },
            { "data": "applicationUser.role", "width": "14%" },
            {
                "data": "courseCount",
                "render": function (data) {
                    return `${data}&nbsp;&nbsp; <i class="fas fa-book-open text-secondary"></i>`
                },
                "width": "12%"
            },
            {
                "data": "studentCount",
                "render": function (data) {
                    return `${data}&nbsp;&nbsp; <i class="fas fa-user-friends text-secondary"></i>`
                },
                "width": "12%"
            },
            {
                "data": "applicationUser",
                "render": function (data) {
                    var today = new Date().getTime()
                    var lockout = new Date(data.lockoutEnd).getTime()
                    if (lockout > today) {
                        return `
                            <div class="text-center">
                                <a onclick=lockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px">
                                    <i class="fas fa-lock-open"></i> Unlock
                                </a>
                            </div>
                            `
                    }
                    else {
                        return `
                            <div class="text-center">
                                <a onclick=lockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:100px">
                                    <i class="fas fa-lock"></i>&nbsp; Lock
                                </a>
                            </div>
                            `
                    }
                }, "width": "25%"
            }
        ]
    })

    //Scroll top after clicking a page button.
    dataTable.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top - 100
        }, 'fast');
    });
}


function lockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/LockUnlock/',
        data: JSON.stringify(id),
        contentType: "application/json",
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