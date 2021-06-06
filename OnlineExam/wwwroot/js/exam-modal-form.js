openModal = (url, title) => {
    
    $.ajax({
        type: "GET",
        url: url,
        success: function (response) {
            $("#form-modal .modal-body").html(response)
            $("#form-modal .modal-title").html(title)
            $("#form-modal").modal("show")
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            })

            $("form").removeData("validator");
            $("form").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse("form");

            tinyMCE.remove()
            tinymce.init({
                selector: 'textarea',
                plugins: 'lists',
                menubar: 'file edit format',
                height: 280,
            })
        }
    })
}

closeModal = () => {
    $('#form-modal').modal('hide');
}

postModal = form => {
    var startDate = $("#start-date").val()
    var startTime = $("#start-time").val()
    var startDateTime = dayjs(startDate + " " + startTime)
    if (!startDateTime.isAfter(dayjs())) {
        swal({
            title: "Error!",
            text: "Start Date cannot be less than or equal to today's date.",
            icon: "error"
        })
        return false
    }

    var endTime = $("#end-time").val()
    var endDateTime = dayjs(startDate + " " + endTime)
    if (!endDateTime.isAfter(startDateTime)) {
        swal({
            title: "Error!",
            text: "End Time cannot be less than or equal to Start Time.",
            icon: "error"
        })
        return false
    }

    tinyMCE.triggerSave();
    $.ajax({
        type: 'POST',
        url: form.action,
        data: new FormData(form),
        contentType: false,
        processData: false,
        success: function (res) {
            if (res.isValid) {
                dataTable.ajax.reload()
                loadCounterValues()
                $('#form-modal .modal-body').html('');
                $('#form-modal .modal-title').html('');
                $('#form-modal').modal('hide');
            }
        },
        error: function (err) {
            console.log(err)
        }
    })
    //to prevent default form submit event
    return false;
}


