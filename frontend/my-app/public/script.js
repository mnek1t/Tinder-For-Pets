
document.getElementById('sign-in-form').addEventListener('submit', function(event) {
    event.preventDefault(); 

    
    var email = document.getElementById('email').value;
    var password = document.getElementById('password').value;

    
    if (email === "" || password === "") {
        alert("Пожалуйста, заполните все поля!");
    } else {
        alert("Вход выполнен!"); 
    }
});
