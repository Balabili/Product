$(function () {
    if (user.Role == 0) {
        window.location.href = "/Admin/UserList";
    } else {
        window.location.href = "/Product/ProductList";
    }
})();