


const toggle = document.getElementById('toggle');
const status = document.getElementById('status');
const customEmailInput = document.getElementById('customEmail');
const customEmailWarning = document.getElementById('customEmailWarning');

let customEmailValue = "";

toggle.addEventListener('change', function () {
    if (this.checked) {
        status.textContent = 'Custom Email';
        status.style.color = 'black';
        customEmailInput.removeAttribute('readonly');
        customEmailWarning.style.display = 'none'; // Uyarıyı gizle
    } else {
        status.textContent = 'Auto Email';
        status.style.color = 'green';
        customEmailValue = customEmailInput.value;

        customEmailInput.value = ""; // Custom Email alanını temizle

        customEmailInput.setAttribute('readonly', 'readonly');
    }
});

function kontrolEtEmail() {
    if (toggle.checked) {
        const emailValue = customEmailInput.value.trim();
        if (emailValue === "") {
            customEmailWarning.textContent = "E-posta adresi bo\u015F b\u0131rak\u0131lamaz.";  // Unicode kaçış dizileri
            customEmailWarning.style.color = 'red';
            customEmailWarning.style.display = 'block';
            return false; // Form gönderimini iptal et
        } else {
            customEmailWarning.style.display = 'none'; // Uyarıyı gizle
            return true; // Form gönderimine izin ver
        }
    } else {
        customEmailWarning.style.display = 'none'; // "Auto Email" seçiliyken uyarıyı gizle
        customEmailInput.removeAttribute('readonly'); // Bu satırı ekledik
        return true; // Form gönderimine izin ver

    }
}

// E-posta alanı değeri her değiştiğinde kontrol işlemini tetikleyin
customEmailInput.addEventListener('input', kontrolEtEmail);

// Form gönderimini kontrol et
document.querySelector('form').addEventListener('submit', function (event) {
    if (!kontrolEtEmail()) {
        event.preventDefault(); // Form gönderimini iptal et
    }
});




