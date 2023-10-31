# Веб-сайт ігрової тематики для публікації новин, рецензій, оглядів для ігор
# Валідація на сервері і в jQuery

# Щоб робити міграції
dotnet tool install -g dotnet-ef
# Міграція
dotnet ef database update
# NumGet
Humanizer.Core.uk
Newtonsoft.Json

## Запуск проекта
```dotnet run```
```dotnet watch run```





## Створення docker контейнера
```docker build -t gamesite-app .```

## Запуск docker контейнера
```docker run -d -p 80:80 -p 443:443 gamesite-app``` 
### Опція -d робить контейнер фоновим, а опції -p 80:80 -p 443:443 вказують на те, що порти 80 та 443 на локальній машині повинні бути спрямовані на порти 80 та 443 у контейнері. Тепер ваш C# ASP.NET MVC додаток повинен бути доступний за адресою http://localhost. Будьте впевнені, що ваш контейнер дотримується всіх налаштувань та залежностей, які потрібні для правильного функціонування вашого додатку.
```http://localhost```

## Перегляд активних контейнерів
```docker ps```

## Зупинити контейнер
```docker stop gamesite-app```

## Видалити контейнер
```docker rm gamesite-app```

## Зробити представлення з БД
```dotnet ef dbcontext scaffold "строка підключення" провайдер_бд```
```Scaffold-DbContext "Data Source=D:\\GameSite.db" Microsoft.EntityFrameworkCore.Sqlite```