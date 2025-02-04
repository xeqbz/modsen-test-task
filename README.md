# mosen-test-task
## Настройка проекта
### Шаг 1. Клонирование репозитория
Склонируйте проект на на ваш ПК:
```bash
git clone https://github.com/xeqbz/modsen-test-task
cd modsen-test-task
```
### Шаг 2. Запуск контейнера
Соберите и запустите контейнер
```bash
docker-compose up --build
```
### Шаг 3. Тестирование API 
Откройте [Swagger](http://localhost:8080/swagger/index.html)
1. **Получение JWT-токена**:
    - Выберите POSt-запрос /api/Auth/Register/register и отправьте его с телом:
    ```json
     {
        "usernameReg": "admin",
        "passwordReg": "admin",
        "role": "Admin"
     }
     ```
    - Выберите POST-запрос /api/Auth/Login/login и отправьте его с телом:
    ```json
     {
        "username": "admin",
        "password": "admin",
     }
     ```
     - Ответ будет содержать JWT-токен
2. **Использование токена**
    - На странице Swagger'a нажмите кнопку Authorize и введите токен в виде: Bearer your_token
    - Обязательно перед токеном через пробел написать Bearer
3. **Теперь вы можете выполнять все запросы**
