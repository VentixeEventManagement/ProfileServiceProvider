# ProfileServiceProvider

# User API

Ett RESTful API för att hantera användarprofilinformation. Byggt med .NET och uppdelat i tre lager: Presentation (Controller), Business (Service), och Data (Repository).

---

## Projektstruktur

```
- Presentation
  - Controllers
    - UserController.cs
- Business
  - Services
    - UserService.cs
  - Interfaces
    - IUserService.cs
  - Models
    - UserRegistrationForm.cs
    - UserUpdateForm.cs
    - ResponseResult.cs
- Domain
  - Models
    UserUpdateForm.cs
- Data
  - Entities
    - UserEntity.cs
  - Interfaces
    - IUserRepository.cs
```

---

## Funktioner

| Funktion            | HTTP-Verb | Route              | Beskrivning                      |
| ------------------- | --------- | ------------------ | -------------------------------- |
| Lägg till användare | POST      | `/api/user/add`    | Registrerar ny användare         |
| Hämta användare     | GET       | `/api/user/get`    | Hämtar information baserat på ID |
| Uppdatera användare | POST      | `/api/user/update` | Uppdaterar användarprofil        |
| Radera användare    | POST      | `/api/user/delete` | Raderar användare                |

---

## API-specifikation

### `POST /api/user/add`

**Body:**

```json
{
  "FirstName": "Anna",
  "LastName": "Andersson",
  "Email": "anna@example.com",
  "Password": "SäkertLösenord123"
}
```

**Respons:**

```json
{
  "succeeded": true,
  "message": "Profile information was created successfully.",
  "statusCode": 201
}
```

---

### `GET /api/user/get?userId=123`

**Respons:**

```json
{
  "succeeded": true,
  "statusCode": 200,
  "result": {
    "userId": "123",
    "firstName": "Anna",
    "lastName": "Andersson",
    "email": "anna@example.com"
  }
}
```

---

### `POST /api/user/update?userId=123`

**Body:**

```json
{
  "FirstName": "Anna",
  "LastName": "Andersson",
  "Email": "anna.ny@example.com"
}
```

**Respons:**

```json
{
  "succeeded": true,
  "message": "User information updated successfully.",
  "statusCode": 200
}
```

---

### `POST /api/user/delete?userId=123`

**Respons:**

```json
{
  "succeeded": true,
  "message": "User information deleted successfully.",
  "statusCode": 200
}
```

---

## Teknisk information

* `UserService` innehåller affärslogiken och anropar `IUserRepository` för databasoperationer.
* `UserController` hanterar HTTP-anrop och validerar inkommande data.
* `ResponseResult<T>` används för att konsekvent strukturera svar.
* `UserFactory` används för att mappa mellan formulär/entiteter och domänmodeller.

---

## Kom igång

### Förutsättningar

* .NET 9
* Swagger (Swashbuckle) installerat i projektet

### Starta API\:t

Länk till Swagger api (https://profileserviceprovider.azurewebsites.net)

Ta ner ProfileServiceProvider på din dator. Skapa en ny appsettings.json. 
Skapa sedan en lokal databas och lägg till sökvägen till den i appsettings.json.
Starta ProfileServiceProvider lokalt och Swagger ska startas automatiskt.


## Konfigurera API-nyckel

För att skydda API:t används en API-nyckel som måste skickas med i varje förfrågan via headern `X-API-KEY`.

### 🛠️ Så här lägger du till API-nyckeln i `appsettings.json`

Öppna filen `appsettings.json` eller `appsettings.Development.json` och lägg till följande:

```json
"ApiKeys": {
  "StandardApiKey": "din-api-nyckel-här"
}
```
