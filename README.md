# Průvodce do kapsy

Webová aplikace pro usnadnění orientace návštěvníků během akcí pro veřejnost pořádaných školou.

Aplikace nahrazuje klasické papírové plánky interaktivní mapou dostupnou z mobilního telefonu. Návštěvníkům umožňuje zobrazit stanoviště, učebny, filtrovat obsah podle specializací a získat doplňující informace o jednotlivých místech.

Součástí projektu je také administrační rozhraní pro správu akcí, stanovišť a dalších dat.

🌐 [Nasazená aplikace](https://id-136.pslib.cloud/app/)
🔒 [Administrační rozhraní](https://id-136.pslib.cloud/login)
🎨 [Grafický návrh ve Figmě](https://www.figma.com/design/xygrAv1nfEujoHUdkVFuCo/DOD?node-id=23-3&t=ZHtbaU5PjS4aqu6D-1)

## Funkce

### Návštěvnická část

- Interaktivní mapa budovy
- Přepínání mezi patry
- Detail stanoviště
- Filtrování podle typu a specializace
- Optimalizace pro mobilní zařízení

### Administrační část

- Přihlášení pomocí ASP.NET Identity
- Správa akcí
- Správa stanovišť
- Správa učitelů, předmětů a specializací
- Studentské rozhraní pro doplňování poznámek

## Použité technologie

| Oblast | Technologie |
|---------|------------|
| Frontend | React 19 |
| Jazyk | TypeScript |
| Backend | ASP.NET Core |
| Databáze | SQLite |
| ORM | Entity Framework Core |
| Autentizace | ASP.NET Identity |
| Návrh UI | Figma |

## Spuštění projektu

### Frontend

```bash
npm install
npm run dev
```

### Backend

```bash
dotnet restore
dotnet run
```

## Struktura projektu

```text
/
├── Client
├── Server
├── Documentace
└── README.md
```
