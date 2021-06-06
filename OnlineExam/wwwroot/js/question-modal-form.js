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

            $("form").removeData("validator")
            $("form").removeData("unobtrusiveValidation")
            $.validator.unobtrusive.parse("form")

            addButtons()
        }
    })
}

// ------------------------------- Code section for adding, removing choices. -----------------------------
addButtons = () => {
    $(".input-group-append").remove()
    var length = $(".input-group").length
    if (length == 3) {
        $(".input-group").last().append(`
                                <div class="input-group-append">
                                    <button onclick="addChoice()" class="btn btn-outline-secondary" type="button"><i class="fas fa-plus"></i></button>
                                </div>`
        )
    }
    else if (length <= 5) {
        $(".input-group").last().append(`
                                <div class="input-group-append">
                                    <button onclick="removeChoice()" class="btn btn-outline-secondary" type="button"><i class="fas fa-minus"></i></button>
                                    <button onclick="addChoice()" class="btn btn-outline-secondary" type="button"><i class="fas fa-plus"></i></button>
                                </div>`
        )
    }
    else if (length == 6) {
        $(".input-group").last().append(`
                                <div class="input-group-append">
                                    <button onclick="removeChoice()" class="btn btn-outline-secondary" type="button"><i class="fas fa-minus"></i></button>
                                </div>`
        )
    }
    $("form").removeData("validator")
    $("form").removeData("unobtrusiveValidation")
    $.validator.unobtrusive.parse("form")
}

addChoice = () => {
    var length = $(".input-group").length - 1
    var letter = String.fromCharCode(65 + length)
    $(".input-group").last().after(`
                            <input type="hidden" name="Choices[${length}].Id" value="0" />
                            <input type="hidden" name="Choices[${length}].ChoiceNumber" value="${length}" />
                            <input type="hidden" name="Choices[${length}].QuestionId" value="0" />
                            <div class="input-group mb-3">
                                <div class="input-group-prepend">
                                    <div class="input-group-text border border-secondary">
                                        <input type="radio" id="choice-${length}" value="${length}" name="CorrectChoice">
                                        <label class="form-check-label" for="choice-${length}">&nbsp; Choice ${letter} </label>
                                    </div>
                                </div>
                                <textarea rows="1" class="form-control border border-secondary" name="Choices[${length}].Description"></textarea>
                            </div>
    `)
    addButtons()
}

removeChoice = () => {
    for (var i = 0; i < 3; i++) {
        $('input[name^="Choices"]').last().remove()
    }
    $(".input-group").last().remove()
    addButtons()
}


closeModal = () => {
    $('#form-modal').modal('hide');
}


//Post modal form.
postModal = form => {
    if ($('#question-imageurl').length == 0) {
        if ($('#upload-box').val() == "") {
            swal("Error!", "Please select an Image.", "error")
            return false
        }
        if ($("input[name=CorrectChoice]").is(":checked") == false) {
            swal("Error!", "Please select Correct Choice.", "error")
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

//Display input and input filename.
$(document).on('change', '.custom-file-input', function (event) {
    var fileReader = new FileReader();
    fileReader.onload = function (e) {
        $('#img-preview').attr('src', e.target.result)
    }
    fileReader.readAsDataURL(event.target.files[0])
    if (event.target.files[0].name.length < 30) {
        $(this).next('.custom-file-label').html(event.target.files[0].name)
    }
    else {
        let fileName = event.target.files[0].name
        $(this).next('.custom-file-label').html(fileName.substr(0, 20) + "..." + fileName.substr(fileName.length - 7));
    }
})