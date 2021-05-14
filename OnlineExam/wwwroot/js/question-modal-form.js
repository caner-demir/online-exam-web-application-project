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
        }
    })
}

closeModal = () => {
    $('#form-modal').modal('hide');
}


postModal = form => {
    if ($('#question-imageurl').length == 0) {
        if ($('#upload-box').val() == "") {
            swal("Error", "Please select an image.", "error")
            return false
        }
        if ($("input[name=CorrectChoice]").is(":checked") == false) {
            swal("Error", "Please select correct choice.", "error")
            return false
        }
    }    
    $.ajax({
        type: 'POST',
        url: form.action,
        data: new FormData(form),
        contentType: false,
        processData: false,
        success: function (res) {
            if (res.isValid) {
                dataTable.ajax.reload()
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

