## API - endpointy

### API — endpointy

- POST /api/ask  
    Krótkie zapytanie do serwisu AI (Google AI). Zwraca odpowiedź na podstawie przesłanych danych.

- POST /api/files  
    Upload pliku. Przyjmuje multipart/form-data, zapisuje plik w magazynie i zwraca metadane (np. fileId).

- GET /api/files/{fileId}  
    Pobranie pliku po identyfikatorze.

### Kontrolery
- FileController — endpointy związane z przesyłaniem i pobieraniem plików.  
- AskController — endpoint do zadawania pytań i zwracania odpowiedzi z Google AI.

### Serwisy (logika biznesowa)
- FileService — logika zapisu/odczytu plików, walidacja, generowanie metadanych.  
- AskService — obsługa zapytań do Google AI, przetwarzanie i formatowanie odpowiedzi.

### Przechowywanie
- StoreFile — interfejs magazynu plików (zapisywanie, odczyt, usuwanie). Implementacje mogą używać systemu plików, chmury itp.

### Szybkie uruchomienie
Uruchom aplikację w katalogu projektu:
```bash
dotnet run
```

### Wersja środowiska
- SDK: Microsoft.NET.Sdk.Web  
- .NET: 8.0