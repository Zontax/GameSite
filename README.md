# Веб-сайт ігрової тематики для публікації новин, рецензій, оглядів для ігор
# Валідація на сервері і в jQuery

# Щоб робити міграції
dotnet tool install -g dotnet-ef
# Міграція
dotnet ef migrations add InitialCreate
dotnet ef database update
ef migrations remove
або 
Add-Migration InitialCreate
Update-Database
# NumGet
Humanizer.Core.uk
Newtonsoft.Json

## Запуск проекта
```dotnet run```
```dotnet watch run```

## Відкад то коміта
git reset --hard <хеш_коміта>
git push --force

## Зробити представлення з БД
```dotnet ef dbcontext scaffold "строка підключення" провайдер_бд```
```Scaffold-DbContext "Data Source=D:\\GameSite.db" Microsoft.EntityFrameworkCore.Sqlite```