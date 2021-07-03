openModal = async (url, title) => {
    $("#loader-body").removeClass('d-none');
    //Delete the content of the other table to avoid class conflicting
    $("#modal-result").html(`<table id="table-questions" class="" style="width:100%">
                                <thead class="thead-dark">
                                    <tr>
                                        <th class="text-light"></th>
                                    </tr>
                                </thead>
                            </table>`);
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

            $("form").removeData("validator")
            $("form").removeData("unobtrusiveValidation")
            $.validator.unobtrusive.parse("form")

            addButtons()
        }
    })
}

openModalResult = async (url, title) => {
    $("#loader-body").removeClass('d-none');
    $("#modal-result").html(`<table id="table-questions" class="" style="width:100%">
                                <thead class="thead-dark">
                                    <tr>
                                        <th class="text-light"></th>
                                    </tr>
                                </thead>
                            </table>`);
    await new Promise(r => setTimeout(r, 400));
    $("#loader-body").addClass('d-none');
    loadQuestionTable(url)
    $("#form-result-modal .modal-title").html(title)
    $("#form-result-modal").modal("show")
    $('.modal-dialog').draggable({
        handle: ".modal-header"
    })
}

// ------------------------------- Code section for adding, removing choices. -----------------------------
addButtons = () => {
    $(".input-group-append").remove()
    var length = $(".input-group").length
    if (length == 3) {
        $(".input-group").last().append(`
                                <div class="input-group-append">
                                    <button onclick="addChoice()" class="btn btn-outline-secondary" style="border-color:#dcdcdc" type="button"><i class="fas fa-plus"></i></button>
                                </div>`
        )
    }
    else if (length <= 5) {
        $(".input-group").last().append(`
                                <div class="input-group-append">
                                    <button onclick="removeChoice()" class="btn btn-outline-secondary" style="border-color:#dcdcdc" type="button"><i class="fas fa-minus"></i></button>
                                    <button onclick="addChoice()" class="btn btn-outline-secondary" style="border-color:#dcdcdc" type="button"><i class="fas fa-plus"></i></button>
                                </div>`
        )
    }
    else if (length == 6) {
        $(".input-group").last().append(`
                                <div class="input-group-append">
                                    <button onclick="removeChoice()" class="btn btn-outline-secondary" style="border-color:#dcdcdc" type="button"><i class="fas fa-minus"></i></button>
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
                                    <div class="input-group-text border">
                                        <input type="radio" id="choice-${length}" value="${length}" name="CorrectChoice">
                                        <label class="form-check-label" for="choice-${length}">&nbsp; Choice ${letter} </label>
                                    </div>
                                </div>
                                <textarea rows="1" class="form-control border" name="Choices[${length}].Description"></textarea>
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
            swal("Error!", "Please select the Correct Choice.", "error")
            return false
        }
        if ($("#Points").val() == 0) {
            swal("Error!", "Please enter a value greater than 0 to the Points field.", "error")
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
                questionTable.ajax.reload()
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


// ------------------------------- Code section for result table. -----------------------------
function loadQuestionTable(url) {
    var arrayIndex = 0;
    var questionTable = $('#table-questions').DataTable({
        "pageLength": 5,
        "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
        "ordering": false,
        "searching": true,
        "lengthChange": true,
        "info": true,
        "ajax": {
            "url": url,
            "type": "GET",
            "datatype": "JSON"
        },
        "columns": [
            {
                "data": {},
                "render": function (data) {
                    arrayIndex++
                    var choices = ""
                    for (var i = 0; i < Object.keys(data.question.choices).length; i++) {
                        var letter = String.fromCharCode(65 + i)
                        choices += `                            
                            <div class="input-group mb-2">
                                <div class="input-group-prepend">
                                    <div class="input-group-text border">
                                        <input type="radio" class="choice-radio" index="${data.id}" name="CorrectChoice" id="${data.question.choices[i].id}" value="${i}" disabled ` + `${data.choiceSelected == i ? 'checked' : '' }` + `>
                                        <label class="form-check-label" for="${data.question.choices[i].id}">
                                        &nbsp;Choice ${letter}
                                        </label>
                                    </div>
                                </div>
                                <label class="form-control border h-100" for="${data.question.choices[i].id}">
                                ${data.question.choices[i].description}
                                </label>
                            </div>`
                    }

                    return `
                    <div class="card mb-2 py-2">
                        <p class="text-dark my-0 pl-3" style="font-size:150%"><i class="text-danger fas fa-pen-square"></i> Question ${arrayIndex}:</p>
                        <hr class="my-1">
                        <div class="text-center">
                            <img src="${data.question.imageUrl}" class="img-fluid">
                        </div>
                        <form>
                            <div class="col-md-12 pt-2">
                                <input type="hidden" value="${data.question.examId}">
                                <input type="hidden" value="${data.id}">
                                <input type="hidden" name="Points" value="${data.question.points}">
                                ${choices}
                                <div class="d-flex justify-content-between align-items-center">
                                    <p class="text-dark my-0" style="font-size:120%">Correct Choice: ${String.fromCharCode(65 + data.question.correctChoice)} <i class="` + `${data.choiceSelected == data.question.correctChoice ? 'text-success far fa-check-circle' : 'text-danger far fa-times-circle'}` + `"></i></p>
                                    <p class="text-dark my-0" style="font-size:120%"><i class="text-info fas fa-hashtag" style="font-size:90%"></i> Points: ${data.question.points}</p>
                                </div>
                            </div>
                        </form>
                    </div>
                    `;
                }, "width": "100%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });

    //Scroll top after clicking a page button.
    questionTable.on('page.dt', function () {
        $('.modal').animate({
            scrollTop: $("#table-questions_wrapper").offset().top -200
        }, 'slow');
    });
}