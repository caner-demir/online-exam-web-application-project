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
            });
        }
    })
}

closeModal = () => {
    $('#form-modal').modal('hide');
}


postModal = form => {
    if ($('#upload-box').val() == "") {
        swal("Error", "Please select an image", "error")
        return false
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

