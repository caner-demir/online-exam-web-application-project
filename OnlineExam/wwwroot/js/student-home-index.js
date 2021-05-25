function sendRequest(button, id) {
    $.ajax({
        type: "POST",
        url: '/Student/Home/SendRequest/',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message)
                $(button).prop("onclick", null).off("click")
                $(button).animate({
                    width: '120px'
                }, 'fast')
                $(button).fadeOut(600, function () {
                    $(this).text('Request Sent').fadeIn(600)
                })
            }
            else {
                toastr.error(data.message)
            }
        }
    })
}

function redirectToLogin() {
    swal({
        title: "Warning",
        text: "You must be logged in to enroll in a course",
        icon: "warning",
        timer: 5000
    }).then(() => {
        window.location.href = "/Identity/Account/Login"
    })
}