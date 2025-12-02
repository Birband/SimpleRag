[![CI Pipeline](https://github.com/Birband/SimpleRag/actions/workflows/ci.yml/badge.svg)](https://github.com/Birband/SimpleRag/actions/workflows/ci.yml)

# SimpleRag 

Prosta aplikacja RAG (Retrieval-Augmented Generation) wykorzystująca .NET 8, OpenAI, PostgreSQL i VectorDB do zarządzania dokumentami PDF oraz odpowiadania na pytania na ich podstawie.

## Funkcje

- Przesyłanie i przechowywanie plików PDF/tekstowych
- Ekstrakcja tekstu z plików PDF/tekstowych
- Dzielenie tekstu na fragmenty
- Generowanie osadzeń (embeddings) za pomocą Google GenAI
- Przechowywanie fragmentów i osadzeń w PostgreSQL (pgvector)
- Zapytania o podobne fragmenty na podstawie osadzeń
- Generowanie odpowiedzi na podstawie podobnych fragmentów i zapytań użytkownika

## Technologie
- .NET 8
- Google GenAI
- PostgreSQL z rozszerzeniem pgvector
- Entity Framework Core
- ASP.NET Core Web API

## Dostępne punkty końcowe API

#### FILES - Zarządzanie plikami (Add/Get):

`POST /api/files` - Przesyłanie pliku PDF/tekstowego

`GET /api/files/{fileId}` - Pobieranie informacji o pliku

#### ASK - Komunikacja z modelem językowym:

`POST /api/ask` - Zadawanie pytań na podstawie przesłanych dokumentów  


## Konfiguracja

TODO: Ustawić odpowiednie zmienne środowiskowe i połączenia baz danych w `appsettings.json`.

## Uruchomienie

1. Sklonuj repozytorium
2. Skonfiguruj bazę danych PostgreSQL z rozszerzeniem pgvector
3. Uruchom migracje bazy danych
```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```
4. Uruchom aplikację
```bash
dotnet run
```
5. Użyj Swagger UI do testowania punktów końcowych API

## Przyszłe ulepszenia

### QoL
- Konwersacje per użytkownik
- Ulepszone zarządzanie błędami i logowanie
- Dodać testy jednostkowe i integracyjne
- Dodać więcej GitHub Actions

### Chores
- Interfejs użytkownika do przesyłania plików i zadawania pytań
- Dodać rollback przy uploadzie pliku w przypadku błędów
- Obsługa większej liczby formatów plików

### Pomysły na ulepszenia jakościowe systemu RAG
- WYMYŚLIC LEPSZE PRZESZUKIWANIE PRZESTRZENI WEKTORÓW 
- Ulepszyć sposób dzielenia tekstu na fragmenty