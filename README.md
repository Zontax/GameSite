# Веб-сайт ігрової тематики для публікації ігрових новин, оглядів, гайдів
Є валідація на сервері і в jQuery, редагування постів за допомогою ckeditor4 в локальній БД SQLite

# Технічне
## Запуск проекта
В консоль:

```dotnet watch run```


```dotnet run``` 

## tool для міграцій
dotnet tool install --global dotnet-ef

## Щоб робити міграції
dotnet tool install -g dotnet-ef
## Міграції
### в консолі
dotnet ef migrations add InitialCreate
dotnet ef migrations add Initial -c ApplicationDbContext
dotnet ef database update -c ApplicationDbContext
ef migrations remove
### в консолі NumGet
Add-Migration InitialCreate
Update-Database
## NumGet пакети
Humanizer.Core.uk

## Відкад то коміта
```git reset --hard <хеш_коміта>```

```git push --force```

## Зробити представлення з БД
```dotnet ef dbcontext scaffold "строка підключення" провайдер_бд```
```Scaffold-DbContext "Data Source=D:\\GameSite.db" Microsoft.EntityFrameworkCore.Sqlite```
## Стрічка підключення до БД
```Data Source=GameSite.db```
## Зібрати релізну версію
```dotnet publish -c Release -o GameSite```
## Публікація
```file:///D:/Programming/CS/WebApplication/GameSite/bin/Release/net7.0/publish/```
