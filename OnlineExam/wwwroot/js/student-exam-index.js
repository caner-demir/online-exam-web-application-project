function beginExam(id) {
    var startDate = dayjs($('#exam-' + id + '-startDate').val())
    var endDate = dayjs($('#exam-' + id + '-endDate').val())
    if (startDate.isAfter(dayjs())) {
        swal({
            title: "Error!",
            text: "This exam hasn't started yet.",
            icon: "error"
        })
        return false
    }
    else if (!endDate.isAfter(dayjs())) {
        swal({
            title: "Error!",
            text: "This exam is over.",
            icon: "error"
        })
        return false
    }
    $.ajax({
        type: "GET",
        url: "/student/question/index/" + id,
        success: function (data) {
            if (data.status != null && !data.status) {
                swal({
                    title: "Error!",
                    text: "You have already submitted your answers for the exam.",
                    icon: "error"
                })
                return false
            }
            else {
                swal({
                    title: "Warning!",
                    text: "Are you sure you want to begin exam?",
                    icon: "warning",
                    buttons: true
                }).then((accept) => {
                    if (accept) {
                        window.location.href = "/student/question/index/" + id
                    }
                })
            }
        }
    })

}