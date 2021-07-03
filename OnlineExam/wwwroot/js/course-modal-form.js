openModal = async (url, title) => {
    $("#loader-body").removeClass('d-none');
    await new Promise(r => setTimeout(r, 400));
    $.ajax({
        type: "GET",
        url: url,
        success: function (response) {
            $("#loader-body").addClass('d-none');
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
                selector: '#detailed-description',
                plugins: 'lists',
                menubar: false,
                toolbar: 'undo redo | styleselect | fontselect | bold italic | alignleft aligncenter alignright alignjustify | outdent indent | removeformat',
                toolbar_mode: 'floating',
                height: 190,
            })
        }
    })
}

closeModal = () => {
    $('#form-modal').modal('hide');
}

postModal = form => {

    if ($("#ImageUrl").length == 0) {
        if ($("#uploadBox").val() == "") {
            swal("Error!", "Please select an Image.", "error")
            return false
        }
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

//Display input filename.
$(document).on('change', '.custom-file-input', function (event) {
    var fileReader = new FileReader();
    fileReader.onload = function (e) {
        $('#img-preview').attr('src', e.target.result)
    }
    fileReader.readAsDataURL(event.target.files[0])
    if (event.target.files[0].name.length < 15) {
        $(this).next('.custom-file-label').html(event.target.files[0].name)
    }
    else {
        let fileName = event.target.files[0].name
        $(this).next('.custom-file-label').html(fileName.substr(0, 9) + "..." + fileName.substr(fileName.length - 7));
    }
})


