# Tesztelési terv (TDD)

Élő dokumentum: a projekt bővülésével együtt frissítjük. A számok **ratchet** jellegűek – csak felfelé léptetjük őket.

## 1. Elvek

- **TDD ciklus:** Red (bukó teszt a viselkedésről) → Green (legkisebb működő kód) → Refactor. Új üzleti logika / hibajavítás tesztet kap *előbb*; hibajavításnál először a hibát reprodukáló bukó teszt.
- **Tesztnév:** `Metódus_Elvárt_viselkedés_Feltétel` (pl. `Upload_Rejects_Empty_Files`).
- Egy teszt egy viselkedést ellenőriz, AAA (Arrange–Act–Assert), nincs logika a tesztben.
- Determinisztikus: nincs hálózat, valós idő/kultúra-függés (kultúrát explicit állítunk, dátumot a mai naphoz viszonyítunk).
- A Cosmos / Blob / Key Vault SDK-t **nem** mockoljuk a saját kódon túl: ami tényleges szolgáltatást igényel, az integrációs teszt.

## 2. Tesztpiramis és projektek

| Szint | Projekt | Mit tesztel | Eszközök | Állapot |
|---|---|---|---|---|
| Unit – logika | `tests/Heiwase.App.Tests.Unit` | `Shared` (DTO-k), `Infrastructure` (media validáció, repository CRUD, options, DI) | xUnit, NSubstitute | ✅ 75 teszt |
| Unit – Blazor | `tests/Heiwase.App.Tests.Blazor` | ViewModel-ek (`ApplicantModel`), később komponensek | xUnit, (bUnit) | ✅ 16 teszt |
| Integration | `tests/Heiwase.App.Tests.Integration` *(még nincs)* | Repository LINQ lekérdezések, Blob feltöltés/törlés | Testcontainers: Cosmos emulator, Azurite | ⏳ |
| API | ugyanott | Azure Functions végpontok (`Heiwase.App.Api`) | in-process host | ⏳ (amint lesz valódi végpont) |
| E2E / smoke | `tests/Heiwase.App.Tests.E2E` *(opcionális)* | Kapcsolat űrlap, nyelvváltás | Playwright | ⏳ |

Miért külön a Blazor projekt? A WebAssembly SDK-projekt referencia megakasztja a Stryker.NET-et, ezért a logikai (Unit) projekt WASM-mentes marad.

Új réteg/projekt esetén: új `Heiwase.App.Tests.<Réteg>` projekt, felvétel a `src/Heiwase.App.slnx` `/tests/` mappájába, és a `tests/check-coverage.py` THRESHOLDS táblájába.

## 3. Kritériumok (reális célok)

| Kritérium | Cél | Jelenlegi mérés | Megjegyzés |
|---|---|---|---|
| Line / branch coverage – `Shared` | ≥ 90% / ≥ 90% | 93% / 100% | CI-ban kapuzott |
| Line / branch coverage – `Infrastructure` | ≥ 70% / ≥ 70% (cél: 80/75, integrációs tesztekkel) | 74% / 82% | CI-ban kapuzott; a LINQ lekérdezések integrációs szintre tartoznak |
| Coverage – `Blazor` | mérjük, nem kapuzzuk | ~6% | a Razor komponensek bUnit-tal kerülnek be; ViewModel/logika ≥ 80% |
| Globális 90% branch | **nem cél** | – | UI, `Program.cs`, generált kód torzítaná |
| Mutation score (Stryker.NET) | ≥ 60% (cél: 70%) a logikai projekteken; 80% csak tiszta logikán | 57% (`Infrastructure`) | PR-en csak mér (`break = 0`), a változott fájlokon (`--since`) |
| SonarQube/SonarCloud Quality Gate | PASS az **új kódra** | – | lásd 5. pont |
| Unit teszt minden üzleti logikára | Definition of Done: minden publikus logikai metódus/szabály tesztelt | – | PR-ellenőrzőlista |
| Integration teszt kritikus folyamatokra | lásd 4. pont | – | |

### Kizárások a coverage-ből
`*.Designer.cs`, `Program.cs`, `obj/`, generált kód, `[ExcludeFromCodeCoverage]` (lásd `tests/coverage.runsettings`). Kizárást csak indoklással adjunk hozzá.

## 4. Kritikus folyamatok (integrációs tesztekhez)

1. Jelentkezési űrlap: validáció → mentés (`ApplicationFormRepository`).
2. Cikk publikálás: mentés → `GetPublishedAsync` szűrés + rendezés (`CreatedAt` csökkenő).
3. Hall of Fame: `GetOrderedAsync` sorrend (`Order`).
4. Képfeltöltés: valódi Azurite ellen feltöltés → URL → törlés (`BlobMediaService`).
5. Konfiguráció: hiányzó/hibás beállításnál az app induláskor elbukik (`ValidateOnStart`).

## 5. Minőségkapuk

- **Pull request (blokkoló):** minden teszt zöld; coverage-küszöb (`tests/check-coverage.py`); build `TreatWarningsAsErrors`.
- **Pull request (tájékoztató):** Stryker a változott fájlokon; riport artifactként.
- **SonarCloud:** `sonar.projectKey` / org beállítása és `SONAR_TOKEN` secret után kapcsolható be. Quality Gate az új kódra: coverage ≥ 80%, duplikáció < 3%, nincs új bug/vulnerability, Security Hotspot átnézve. A coverage-jelentés útvonala: `**/coverage.opencover.xml` (`sonar-project.properties`).
- A küszöböket negyedévente felülvizsgáljuk; emelés után nem csökkentjük.

## 6. Helyi futtatás

```powershell
# összes teszt
dotnet test src/Heiwase.App.slnx

# coverage
dotnet test src/Heiwase.App.slnx --settings tests/coverage.runsettings --collect:"XPlat Code Coverage"

# mutation (első alkalommal: dotnet tool restore)
cd tests/Heiwase.App.Tests.Unit
dotnet tool run dotnet-stryker
```

## 7. Definition of Done (PR-hoz)

- [ ] Az új/változott viselkedéshez van teszt, amely a változtatás előtt bukott.
- [ ] Tesztek zöldek lokálisan és CI-ban.
- [ ] A coverage nem csökkent a kapuzott rétegeken.
- [ ] Új réteg/szolgáltatás esetén tesztprojekt + küszöb felvéve.
- [ ] A Stryker-riportban az új kódra nincs triviálisan életben maradt mutáns (ahol értelmes).

## 8. Következő lépések

1. `Heiwase.App.Tests.Integration` projekt: Testcontainers + Azurite + Cosmos emulator, a 4. pont folyamataival.
2. bUnit a `ContactSection` (training type szabályok: `IsTrainingTypeDisabled`, `OnTrainingTypeChanged`) és a `HallOfFameSection` komponensekhez; a logikát érdemes kiemelni tesztelhető osztályba.
3. SonarCloud bekötése, a Quality Gate PR-decorationnel.
4. A Stryker-küszöb (`break`) fokozatos emelése 50 → 60 → 70-re.
