﻿var questionTable
var resultTable

$(document).ready(function () {
    loadCounterValues()
    loadTableQuestions()
    loadTableResults()
    changeNav()
})

// Change nav content on click event.
function changeNav() {
    $(".container-teacher").on("click", ".nav-link", function () {
        $(".nav-question").each(function () {
            $(this).addClass("d-none")
        })
        $("." + $(this).attr("id")).removeClass("d-none")
    })
}

// ------------------------------- Code section for counter panel. ---------------------------
function loadCounterValues() {
    var url = window.location.href
    var index = url.lastIndexOf('/')
    var id = url.substring(index + 1)

    $.ajax({
        type: "GET",
        url: '/Teacher/Question/GetCounter/'+id,
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (response) {
            $("#question-counter").html(response.counter.questionCounter)
            $("#points-counter").html(response.counter.pointsCounter)
            $("#student-counter").html(response.counter.studentCounter)
            $("#hours-counter").html(response.counter.durationHours)
            $("#minutes-counter").html(response.counter.durationMinutes)
        }
    })
}

// ------------------------------- Code section for question table. ---------------------------
function loadTableQuestions() {
    questionTable = $("#table-question").DataTable({
        "autoWidth": false,
        "order": [[5, "asc"]],
        "columnDefs": [{
                "searchable": false,
                "orderable": false,
                "className": "font-weight-bold table-cells-lg table-active",
                "targets": 0,
            },
            { className: "table-cells-lg", "targets": [2, 3, 4] },
            { className: "table-active table-cells-lg", "targets": [5] }
        ],
        "ajax": {
            "url": "/Teacher/Question/GetAll"
        },
        "columns": [
            {"data": "id", "width": "8%" },
            {
                "data": "imageUrl",
                "render": function (data) {
                    return `
                            <div class="rounded d-flex align-items-center border overflow-hidden" style="max-height:100px;">
                                <img src="${data}" class="w-100" >
                            </div>
                            `
                }, "width": "36%"
            },
            {
                "data": "dateCreated",
                "render": function (data) {
                    return moment(data).format('MMM DD YYYY, HH:mm')
                },
                "width": "18%"
            },
            {
                "data": "points",
                "render": function (data) {
                    return `${data}&nbsp;&nbsp; <i class="fas fa-hashtag text-secondary"></i>`
                },
                "width": "12%"
            },
            {
                "data": "choices",
                "render": function (data) {
                    return `${data.length}&nbsp;&nbsp; <i class="fas fa-bars text-secondary"></i>`
                },
                "width": "12%"
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
                }, "width": "25%"
            }
        ]
    })

    //Give index numbers to rows.
    questionTable.on('order.dt search.dt', function () {
        questionTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = `<span style="font-size: 110%">${i + 1}</span>`;
        });
    }).draw();

    //Scroll top after clicking a page button.
    questionTable.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top - 100
        }, 'fast');
    });
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
                        questionTable.ajax.reload()
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

// ------------------------------- Code section for result table. ---------------------------
function loadTableResults() {
    resultTable = $("#table-results").DataTable({
        "autoWidth": false,
        "columnDefs": [
            { className: "table-active table-cells-sm", "targets": [0, 3] },
            { className: "table-cells-sm", "targets": [0, 1, 2, 3] }
        ],
        "ajax": {
            "url": "/Teacher/Question/GetResults"
        },
        "columns": [
            {
                "data": "applicationUser.name",
                "render": function (data) {
                    return `<span class="text-dark font-weight-bold" style="font-size: 110%">${data}</span>`
                },
                "width": "25%"
            },
            { "data": "applicationUser.userName", "width": "25%" },
            { "data": "score", "width": "25%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick="openModalResult('/Teacher/Question/ViewResult/${data}', 'View Results')"
                                        class="btn btn-success text-white" style="cursor:pointer; width:113px">
                                    <i class="fas fa-search"></i>&nbsp; View
                                </a>
                            </div>
                            `
                }, "width": "25%"
            }
        ]
    })

    //Scroll top after clicking a page button.
    resultTable.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top - 100
        }, 'fast');
    });
}