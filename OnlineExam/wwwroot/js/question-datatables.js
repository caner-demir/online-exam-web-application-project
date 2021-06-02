var dataTable

$(document).ready(function () {
    loadCounterValues()
    loadDataTable()
})

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
        }
    })
}

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "autoWidth": false,
        "order": [[5, "asc"]],
        "columnDefs": [{
                "searchable": false,
                "orderable": false,
                "className": "font-weight-bold table-cells-lg",
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
                            <div class="rounded d-flex align-items-center overflow-hidden" style="max-height:100px;">
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
                    return `<i class="fas fa-star"></i>&nbsp;&nbsp; ${data}`
                },
                "width": "12%"
            },
            {
                "data": "choices",
                "render": function (data) {
                    return `<i class="fas fa-list-ul"></i>&nbsp;&nbsp; ${data.length}`
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
                                        class="btn btn-success text-white" style="cursor:pointer; width:50px">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/Teacher/Question/Delete/${data.id}") class="btn btn-danger text-white" style="cursor:pointer; width:50px">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>
                            `
                }, "width": "25%"
            }
        ]
    })

    //Give index numbers to rows.
    dataTable.on('order.dt search.dt', function () {
        dataTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = `<span style="font-size: 110%">${i + 1}</span>`;
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