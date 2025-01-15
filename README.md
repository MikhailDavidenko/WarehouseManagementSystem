# WarehouseManagementSystem

# WMS Web API

Система управления на складе с хранением данных в SQLite или PostgreSQL

Ключевые проекты в решении:
* `Web` - Web API
* `IntegrationTests` - интеграционные тесты
* `UnitTests` - интеграционные тесты

## Описание API

Приложение предоставляет доступ для выполнения CRUD операций c паллетами и коробками(и другими возможными сущностями)

### Контроллер Pallet
* `GET /api/v1/pallets` - получение всех паллет
* `GET /api/v1/pallets/{palletId}` - получение паллеты
* `POST /api/v1/pallets` - добавление паллеты
* `PUT /api/v1/pallets/{palletId}` - изменение паллеты
* `DELETE /api/v1/pallets/{palletId}` - удаление паллеты

### Контроллер Box
* `POST /api/v1/pallets/{palletId}/boxes` - добавление коробки на паллету
* `PUT /api/v1/pallets/{palletId}/boxes/{boxId}` - изменение коробки
* `DELETE /api/v1/pallets/{palletId}/boxes/{boxId}` - удаление коробки

## Установка и запуск

Необходимые зависимости:
* [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
* [EF Core CLI](https://docs.microsoft.com/en-us/ef/core/cli/dotnet) (для работы с миграциями)
* [Docker](https://www.docker.com/products/docker-desktop) (для запуска тестов)

Заполните файл конфигураций в соответствии с выбранной базой данных(PostgreSQL или SQLite) - [appsettings.json](src/Web/appsettings.json).

Запуск из корневой директории проекта:

```shell
dotnet run --project .\src\Web
```

### Тестирование

Для тестов используется TestContainer, поэтому необходимо запустить докер

Запуск тестов из корневой директории проекта:

```shell
dotnet test
```

## Миграции

Для создания миграций используется EF Core CLI. Необходимо выполнить следующие команды:

PostgreSql:

``` bash
cd src | dotnet ef migrations add Init --project Data.Migrations.Postgre  --startup-project Web --context WarehouseManagementSystem.Data.Engine.DataContext
```

Sqlite:

``` bash
cd src | dotnet ef migrations add Init --project Data.Migrations.Sqlite  --startup-project Web --context WarehouseManagementSystem.Data.Engine.DataContext
```

Миграция применится при следующем запуске приложения.
