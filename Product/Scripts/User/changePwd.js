$(function () {
    _getItems();
    var saveBtn = document.getElementById("savePwd");
    saveBtn.onclick = function () {
        var pwdOne = document.getElementById("password").value;
        var pwdTwo = document.getElementById("passwordAgain").value;
        if (pwdOne == pwdTwo) {
            utility.ajax("/User/UpdatePassword", "POST", { password: pwdOne, userId: userInfo.Id }, saveSuccessCallBack);
        } else {
            alert("password wrong");
        }
    }
})
function _getItems() {
    if (userInfo.Role != 2) {
        var adminNav = document.getElementById("isAdmin");
        adminNav.removeAttribute("style");
    }
}

function saveSuccessCallBack(result) {
    alert("change successful");
}