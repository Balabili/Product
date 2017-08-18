$(function () {
    initData();
})
function initData() {
    document.getElementById("loginName").innerText = userInfo.LoginName;
    if (userInfo.UserInformation) {
        document.getElementById("name").innerText = userInfo.UserInformation.Name;
        document.getElementById("gender").innerText = userInfo.UserInformation.Gender == 0 ? "Woman" : "Man";
        document.getElementById("age").innerText = userInfo.UserInformation.Age;
        document.getElementById("email").innerText = userInfo.UserInformation.Email;
    }
    if (userInfo.Role != 2) {
        var adminNav = document.getElementById("isAdmin");
        adminNav.removeAttribute("style");
    }
}