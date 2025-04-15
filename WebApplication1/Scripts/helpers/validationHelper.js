// 🛠 Kiểm tra Email hợp lệ
function isValidEmail(email) {
    let emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailPattern.test(email);
}

// 🛠 Kiểm tra số điện thoại hợp lệ (Việt Nam)
function isValidPhone(phone) {
    let phonePattern = /^(0[2-9][0-9]{8,9})$/;
    return phonePattern.test(phone);
}

// 🛠 Kiểm tra mật khẩu mạnh (ít nhất 6 ký tự, có chữ cái và số)
function isValidPassword(password) {
    let passwordPattern = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/;
    return passwordPattern.test(password);
}

// Xuất các hàm để có thể sử dụng ở file khác
export { isValidEmail, isValidPhone, isValidPassword };
