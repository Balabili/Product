$(function () {
    var saveButton = document.getElementById("saveInfo");
    initData();
    saveButton.onclick = function () {
        var userName = document.getElementById("name").value;
        var userAge = document.getElementById("age").value;
        var userEmail = document.getElementById("email").value;
        var userGender = getUserGender();
        var data = {};
        data.Gender = userGender;
        data.Name = userName;
        data.Age = userAge;
        data.Email = userEmail;
        saveUserInformation(data);
    }
})

function initData() {
    if (userInfo.Role != 2) {
        var adminNav = document.getElementById("isAdmin");
        adminNav.removeAttribute("style");
    }
    document.getElementById("name").value = userInfo.Name;
    document.getElementById("age").value = userInfo.Age == 0 ? "" : userInfo.Age;
    document.getElementById("email").value = userInfo.Email;
    setUserGender();
}

function setUserGender() {
    var genderRadio = document.getElementsByName("gender");
    for (var i = 0; i < genderRadio.length; i++) {
        if (genderRadio[i].value == userInfo.Gender) {
            genderRadio[i].checked = true;
        }
    }
}

function getUserGender() {
    var genderRadio = document.getElementsByName("gender");
    for (var i = 0; i < genderRadio.length; i++) {
        if (genderRadio[i].checked) {
            return genderRadio[i].value;
        }
    }
}

function saveUserInformation(data) {
    utility.ajax("/User/SaveInformation", "POST", data, saveSuccessCallBack);
}

function saveSuccessCallBack(result) {
    alert("Save Successful");
}