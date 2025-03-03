// Функціонал гри (коміти)
document.addEventListener("DOMContentLoaded", () => {
    const commitBtn = document.getElementById("commit-btn");
    const commitCountEl = document.getElementById("commit-count");
    const timerEl = document.getElementById("timer");

    let commitCount = 0;
    let timeLeft = 10;
    let gameActive = false;

    if (commitBtn) {
        commitBtn.addEventListener("click", () => {
            if (!gameActive) {
                gameActive = true;
                commitCount = 0;
                timeLeft = 10;
                commitCountEl.textContent = "Комітів: 0";
                timerEl.textContent = "Час: 10";
                commitBtn.textContent = "Коміт!";

                let interval = setInterval(() => {
                    timeLeft--;
                    timerEl.textContent = `Час: ${timeLeft}`;
                    if (timeLeft === 0) {
                        clearInterval(interval);
                        gameActive = false;
                        commitBtn.textContent = "Грати ще раз";
                        alert(`Ти зробив ${commitCount} комітів! 🚀`);
                    }
                }, 1000);
            } else {
                commitCount++;
                commitCountEl.textContent = `Комітів: ${commitCount}`;
            }
        });
    }
});

// Функція для генерації випадкового імені
function generateUsername() {
    const randomNumber = Math.floor(Math.random() * 1000) + 1;
    return `programist${randomNumber}`;
}

// Отримуємо значення isAdmin з Razor
var isAdminFlag = document.getElementById("is-admin").dataset.value === "true";

// Єдиний обробник події для відправки повідомлення
$(document).ready(function () {
    $("#send-message-btn").click(function () {
        let messageText = $("#message-text").val().trim();
        if (messageText === "") {
            alert("Повідомлення не може бути порожнім!");
            return;
        }

        $.ajax({
            url: "/chat/sendMessage",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ text: messageText }),
            success: function (response) {
                let messageId = response.messageId;
                let newMessageHtml = `
                    <div class="message" id="message-${messageId}">
                        <strong>${response.username}:</strong> ${response.text}
                        ${isAdminFlag ? `<button class="btn btn-danger" onclick="deleteMessage('${messageId}')">Видалити</button>` : ""}
                    </div>`;
                $("#messages-container").append(newMessageHtml);
                $("#message-text").val(""); // Очистка поля вводу
            },
            error: function () {
                alert("Помилка при відправці повідомлення!");
            }
        });
    });

    // Функція для отримання повідомлень та оновлення контейнера
    function getMessages() {
        $.get("/chat/getMessages", function (messages) {
            var messagesContainer = $("#messages-container");
            messagesContainer.empty();

            messages.forEach(function (message) {
                var deleteButton = isAdminFlag ? `<button class="btn btn-danger" onclick="deleteMessage('${message.id}')">Видалити</button>` : "";
                var messageHtml = `
                    <div class="message" id="message-${message.id}">
                        <strong>${message.username}:</strong> ${message.text} ${deleteButton}
                    </div>`;
                messagesContainer.append(messageHtml);
            });
            messagesContainer.scrollTop(messagesContainer[0].scrollHeight);
        });
    }

    getMessages();
    setInterval(getMessages, 5000);
});

// Функція видалення повідомлення
function deleteMessage(messageId) {
    if (!confirm("Ви впевнені, що хочете видалити це повідомлення?")) return;

    $.ajax({
        url: "/chat/deleteMessage",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ messageId: messageId }),
        success: function () {
            $(`#message-${messageId}`).remove();
        },
        error: function () {
            alert("Помилка при видаленні повідомлення!");
        }
    });
}

// Отримання повідомлень при завантаженні сторінки
$(document).ready(function () {
    getMessages();
    setInterval(getMessages, 5000); // Оновлюємо чат кожні 5 секунд
});
