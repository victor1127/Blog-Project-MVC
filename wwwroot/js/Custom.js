
var index = document.getElementById("tagList").options.length;

function AddTag() {

    var userInput = document.getElementById("tagInput");
    var searchResult = SearchTag(userInput.value);

    if (searchResult != "") {
        DisplayError.fire({
            html: `<span class="fw-bold t-bg my-0">${searchResult.toLocaleUpperCase()}</span>`
        });
    }
    else {
        var newOptions = new Option(userInput.value, userInput.value);
        document.getElementById("tagList").options[index++] = newOptions;
    }

    userInput.value = "";
}

function RemoveTag() {

    var selectedIndex = document.getElementById("tagList").selectedIndex;
    document.getElementById("tagList").options[selectedIndex] = null;
    index--;
}

$("form").on("submit", function () {
    $("#tagList option").prop("selected", "selected");
})

function SearchTag(tag) {

    if (tag == "") return 'Empty tags';

    var currentTags = document.getElementById("tagList");

    if (currentTags) {
        var tagOptions = currentTags.options;
        for (var index = 0; index < tagOptions.length; index++) {
            if (tag == tagOptions[index].value) return "Duplicate tags";
        }

    }

    return "";
}

const DisplayError = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-color btn-sm btn-block btn-outline-dark'
    },
    imageUrl: '/assets/img/oopsError.jpg',
    buttonsStyling: false,
});


