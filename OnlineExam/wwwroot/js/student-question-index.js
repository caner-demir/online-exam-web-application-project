var questionTable;
var arrayIndex = 0;

$(document).ready(function () {
    loadQuestionTable();
});

function loadQuestionTable() {
    questionTable = $('#table-questions').DataTable({
        "pageLength": 1,
        "ordering": false,
        "searching": false,
        "lengthChange": false,
        "info": false,
        "pagingType": "full_numbers",
        "ajax": {
            "url": "/Student/Question/GetAll",
            "type": "GET",
            "datatype": "JSON"
        },
        "columns": [
            {
                "data": { },
                "render": function (data) {
                    arrayIndex++
                    var choices = ""
                    for (var i = 0; i < Object.keys(data.choices).length; i++) {
                        var letter = String.fromCharCode(65 + i)
                        choices += `                            
                            <div class="input-group mb-2">
                                <div class="input-group-prepend">
                                    <div class="input-group-text border">
                                        <input type="radio" class="choice-radio" index="${data.id}" name="CorrectChoice" id="${data.choices[i].id}" value="${i}">
                                        <label class="form-check-label" for="${data.choices[i].id}">
                                        &nbsp;Choice ${letter}
                                        </label>
                                    </div>
                                </div>
                                <label class="form-control border h-100" style="background:#e3e7ea" for="${data.choices[i].id}">
                                ${data.choices[i].description}
                                </label>
                            </div>`
                    }

                    return `
                    <div class="text-center p-2 border-bottom">
                        <img src="${data.imageUrl}" class="img-fluid rounded-top">
                    </div>
                    <form>
                        <div class="col-md-12 pt-3 pb-4">
                            <input type="hidden" id="question-id" name="ExamId" value="${data.examId}">
                            <input type="hidden" id="q-id" name="Id" value="${data.id}">
                            <input type="hidden" id="q-points-${arrayIndex}" name="Points" value="${data.points}">
                            ${choices}
                        </div>
                    </form>
                    `;
                }, "width": "100%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}

// ------------------------------- Code section for button behaviours. ---------------------------
var questionSel = "btn-warning q-selected";
var questionSelBottom = "btn-outline-warning";
var questionDef = "btn-primary";
var questionDefBottom = "btn-outline-primary";
var successIcon = "fa-check";
var durationWait = 400;


// Question no headerinin yönetimi.
function changeQuestionNo() {
    index = $(".q-selected").text();
    $("#q-header").text(index);
    var points = $("#q-points-" + index).val();
    $("#q-points").text(points + " pts");
}

// İlk "Mevcut soru" ikonunun yüklenmesi.
$(document).ready(function () {
    $(async function () {
        var first = $("#q-palette div:nth-child(2)").children().first();
        var firstBottom = $("#q-palette div:nth-child(2)").children().first().next();

        first.addClass(questionSel);
        first.removeClass(questionDef);
        firstBottom.addClass(questionSelBottom);
        firstBottom.removeClass(questionDefBottom);
        await new Promise(r => setTimeout(r, durationWait));
        changeQuestionNo()
    });
});

// Success ikonunun yönetimi.
$(function () {
    $("#table-questions").on("click", ".choice-radio", function () {
        var id = $(this).attr("index");
        $("#" + id).removeClass("btn-light btn-outline-warning");
        $("#" + id).addClass("btn-success");
        $("#" + id).children().removeClass("fa fas fa-question");
        $("#" + id).children().addClass("fa fas fa-check");
    });
});

// "Mevcut soru" ikonunun tabloda yönetimi.
$(function () {
    $("#q-palette").on("click", ".q-no", async function () {
        await new Promise(r => setTimeout(r, durationWait));
        // Onceki "Mevcut soru" ikonunun kaldirilmasi.
        var btn = $(".q-selected");
        var btnBottom = $(".q-selected").next();

        btnBottom.removeClass(questionSelBottom);
        if (!btnBottom.children().first().hasClass(successIcon)) {
            btnBottom.addClass(questionDefBottom);
        }
        btn.addClass(questionDef);
        btn.removeClass(questionSel);

        // Yeni "Mevcut soru" ikonunun eklenmesi.
        var btnNew = $(this).children().first();
        var btnNewBottom = $(this).children().first().next();

        btnNew.removeClass(questionDef);
        btnNew.addClass(questionSel);
        if (!btnNewBottom.children().first().hasClass(successIcon)) {
            btnNewBottom.removeClass(questionDefBottom);
            btnNewBottom.addClass(questionSelBottom);
        }
    });
});

// "Mevcut soru" ikonunun previous ve next butonlarinda yönetimi.
$(function () {
    $("#q-prev").click(async function () {
        $("#table-questions").addClass('d-none');
        $("#loader-body").removeClass('d-none');
        await new Promise(r => setTimeout(r, durationWait));
        $("#table-questions").removeClass('d-none');
        $("#loader-body").addClass('d-none');
        $(".pagination li:nth-child(2)").click();
        // "Mevcut soru" ikonunun yönetimi.
        var btn = $(".q-selected");
        var btnBottom = $(".q-selected").next();
        var btnPrev = $(".q-selected").parent().prev().children().first();
        var btnPrevBottom = $(".q-selected").parent().prev().children().first().next();

        var index = $(".q-selected").text();

        if (index > 1) {
            // "Mevcut soru" ikonunun eklenmesi.
            btnPrev.removeClass(questionDef);
            btnPrev.addClass(questionSel);
            if (!btnPrevBottom.children().first().hasClass(successIcon)) {
                btnPrevBottom.removeClass(questionDefBottom);
                btnPrevBottom.addClass(questionSelBottom);
            }

            // Onceki "Mevcut soru" ikonunun kaldirilmasi.
            btn.removeClass(questionSel);
            btn.addClass(questionDef);
            if (!btnBottom.children().first().hasClass(successIcon)) {
                btnBottom.addClass(questionDefBottom);
            }
            btnBottom.removeClass(questionSelBottom);
        }
        changeQuestionNo();
    });
    $("#q-next").click(async function () {
        $("#table-questions").addClass('d-none');
        $("#loader-body").removeClass('d-none');
        await new Promise(r => setTimeout(r, durationWait));
        $("#table-questions").removeClass('d-none');
        $("#loader-body").addClass('d-none');
        $(".pagination li:nth-last-child(2)").click();
        // "Mevcut soru" ikonunun yönetimi.
        var btn = $(".q-selected");
        var btnBottom = $(".q-selected").next();
        var btnNext = $(".q-selected").parent().next().children().first();
        var btnNextBottom = $(".q-selected").parent().next().children().first().next();

        var index = $(".q-selected").text();
        var last = $("#q-palette :nth-last-child(2)").children().text();

        if (parseInt(index) < last) {
            // "Mevcut soru" ikonunun eklenmesi.
            btnNext.removeClass(questionDef);
            btnNext.addClass(questionSel);
            if (!btnNextBottom.children().first().hasClass(successIcon)) {
                btnNextBottom.removeClass(questionDefBottom);
                btnNextBottom.addClass(questionSelBottom);
            }

            // Onceki "Mevcut soru" ikonunun kaldirilmasi.            
            btn.addClass(questionDef);
            btn.removeClass(questionSel);
            if (!btnBottom.children().first().hasClass(successIcon)) {
                btnBottom.addClass(questionDefBottom);
            }
            btnBottom.removeClass(questionSelBottom);
        }
        changeQuestionNo();
    });
});

// Soru tablosunun paginationdaki linklere baglanmasi.
$(function () {
    $("#q-palette").on("click", ".q-no", async function () {
        $("#table-questions").addClass('d-none');
        $("#loader-body").removeClass('d-none');
        await new Promise(r => setTimeout(r, durationWait));
        $("#table-questions").removeClass('d-none');
        $("#loader-body").addClass('d-none');
        var id = $(this).children().first().text();
        $(".page-link").first().click();
        for (i = 0; i < id - 1; i++) {
            $(".pagination li:nth-last-child(2)").click();
        }
        changeQuestionNo();
    });
});

// Datatablesda kozmetik duzenlemeler.
$(function () {
    $("#table-questions_paginate").hide();
    $(".thead-dark").hide();
});

// ------------------------------- Code section for the timer. ---------------------------
var duration = $("#duration").val().split(":");
var hours = duration[0];
var minutes = duration[1];
var minutesTotal = (hours * 15) + minutes;

function getTimeRemaining(endtime) {
    const total = Date.parse(endtime) - Date.parse(new Date());
    const seconds = Math.floor((total / 1000) % 60);
    const minutes = Math.floor((total / 1000 / 60) % 60);
    const hours = Math.floor((total / (1000 * 60 * 60)) % 24);

    return {
        total,
        hours,
        minutes,
        seconds
    };
}

function initializeClock(id, endtime) {
    const clock = document.getElementById(id);
    const hoursSpan = clock.querySelector('.hours');
    const minutesSpan = clock.querySelector('.minutes');
    const secondsSpan = clock.querySelector('.seconds');

    function updateClock() {
        const t = getTimeRemaining(endtime);

        hoursSpan.innerHTML = ('0' + t.hours).slice(-2) + ":";
        minutesSpan.innerHTML = ('0' + t.minutes).slice(-2) + ":";
        secondsSpan.innerHTML = ('0' + t.seconds).slice(-2);

        if (t.total <= 0) {
            Redirect();

            // Orjinal dosyada yer alan fonksiyon.
            clearInterval(timeinterval);
        }
    }

    updateClock();
    const timeinterval = setInterval(updateClock, 1000);
}

const deadline = new Date(Date.parse(new Date()) + minutesTotal * 60 * 1000);
initializeClock('clockdiv', deadline);

// "Süre bitti" uyarı fonksiyonu.
function Redirect() {
    swal({
        title: "Time's Up!",
        text: "You will be redirected to the course page.",
        icon: "warning",
    }).then(() => {
        var courseId = $("#course-id").val()
        setTimeout(function () { window.location.href = "/Student/Exam/Index/" + courseId; }, 1000);
    });
}

// ------------------------------- Code section for finish button. ---------------------------
function finishExam() {
    swal({
        title: "Are you sure?",
        text: "You are about to submit the answers.",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((finish) => {
        if (finish) {
            var finalArr = []
            questionTable.$('form').each(function (index) {
                var formData = $(this).serializeArray()
                var returnObj = {}
                for (var i = 0; i < formData.length; i++) {
                    returnObj[formData[i]['name']] = formData[i]['value']
                }
                finalArr[index] = returnObj
            });

            $.ajax({
                type: "POST",
                url: "/Student/Question/Index",
                dataType: "JSON",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(finalArr),
                success: function (result) {
                    window.location.href = result;
                }
            });
        }        
    })
}