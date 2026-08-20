# CLAUDE.md

Panduan untuk Claude Code saat bekerja di repository ini.

## Gambaran Umum

ESKA_APP adalah aplikasi web add-on untuk **SAP Business One** (backend **SAP HANA**), dipakai
untuk proses gudang & produksi: Goods Receipt PO, Stock Opname, ReProcess (Issue & Receipt),
Process Card, Production Task/Schedule, plus modul Setting/Approval/Report/Alert.

Stack: **ASP.NET MVC 5 / .NET Framework 4.7.2**, **Entity Framework 6 (database-first, EDMX)**,
**DevExpress 20.1 MVC Extensions** untuk seluruh UI, **Crystal Reports** untuk cetakan,
**SAP DI API (SAPbobsCOM)** untuk posting dokumen ke SAP B1, **Elmah** untuk error log.

## Struktur Solution

```
ESKA_APP.sln  (ada di ESKA_APP/ESKA_APP.sln)
├── ESKA_APP/ESKA_APP/    Web project — Controllers, Views, Content, Scripts
└── ESKA_DI/              Class library — semua Model, Service, EF context, SAP helper
dlls/                     DevExpress 20.1 + Sap.Data.Hana + helper DLL (referensi HintPath)
```

- `ESKA_DI` tidak punya UI. Semua logic bisnis, akses DB, dan panggilan SAP ada di sini.
- `ESKA_APP` hanya orkestrasi: baca Session, panggil Service, render PartialView.

Peta folder penting:

| Path | Isi |
|---|---|
| `ESKA_APP/ESKA_APP/Controllers/<Area>/<Fitur>/` | Controller (partial class, dipecah per concern) |
| `ESKA_APP/ESKA_APP/Views/<Area>/<Fitur>/` | View utama + `Partial/` |
| `ESKA_DI/Models/<Area>/` | `<Fitur>Model.cs` (model+service) dan `<Fitur>__List_Model.cs` (grid) |
| `ESKA_DI/Models/_EF/` | EDMX `HANA_APP.edmx` + entity hasil generate (`Tm_*`, `Tx_*`, `Tp_*`, `Ts_*`) |
| `ESKA_DI/Models/_Sap/` | `SAPConnection`, `SAPCachedCompany` (pool koneksi DI API) |
| `ESKA_DI/Models/_Utils/` | `GeneralGetList`, `SpNotif`, `Rpt`, `Excel`, `Csv`, `Encryption`, `ApprovalService` |
| `ESKA_DI/Models/_Cfl/` | "Choose From List" — popup lookup (Item, Warehouse, BP, PO, dst.) |
| `ESKA_APP/ESKA_APP/Scripts/SQLs/` | Script DDL/stored procedure HANA (dijalankan manual) |

Area yang dipakai: `Authentication`, `Master`, `Transaction`, `Production`, `Report`,
`Setting`, `Notification`, plus prefix `_` untuk komponen shared (`_Cfl`, `_CrystalReport`, `_ViewJe`).

## Build & Jalankan

- Buka `ESKA_APP/ESKA_APP.sln` di **Visual Studio 2022**, build dengan MSBuild (bukan `dotnet build` —
  ini .NET Framework classic, `packages.config`, bukan SDK-style project).
- Prasyarat mesin: **SAP B1 DI API** ter-install (COM interop `SAPbobsCOM`), **SAP HANA Client**,
  **Crystal Reports runtime**, dan DevExpress 20.1 (DLL sudah disertakan di `dlls/`).
- Jalankan via IIS Express / IIS. **Tidak ada test project** dan tidak ada CI — verifikasi dilakukan
  dengan menjalankan aplikasi.

## Dua Database HANA

Aplikasi bicara ke **dua schema HANA** sekaligus:

| Nama | Isi | Akses |
|---|---|---|
| `HANA_APP` | Schema aplikasi ini (tabel `Tm_*`/`Tx_*`/`Ts_*`) | EF6 (`new HANA_APP()`), nama schema via `DbProvider.dbApp_Name` |
| `HANA_SAP` | Schema company SAP B1 (`OITM`, `OPOR`, `OPDN`, …) | SQL mentah, nama schema via `DbProvider.dbSap_Name` |

Query lintas schema selalu ditulis manual, contoh:

```csharp
var ssql = "SELECT \"DocNum\" FROM \"" + DbProvider.dbSap_Name + "\".\"OPDN\" T0 WHERE T0.\"DocEntry\" = " + docEntry;
```

**HANA case-sensitive**: identifier tabel/kolom **harus** di-quote (`"TransNo"`), dan sintaks
HANA dipakai (`LIMIT n OFFSET m`, `SELECT CURRENT_TIMESTAMP FROM DUMMY`, `CALL "SpXxx"(...)`).

Perubahan schema dilakukan lewat script di `Scripts/SQLs/` (`CreateTable.sql`,
`AlterTable.sql`, `CreateStoredProcedure.sql`, `InsertInto.sql`) — **tidak ada EF migration**.
Setelah tabel berubah, EDMX `HANA_APP.edmx` harus di-update dari database.

## Konvensi Kunci

### 1. Resolusi View mengikuti namespace controller

`IduRazorViewEngine` (lihat [IduRazorViewEngine.cs](ESKA_APP/ESKA_APP/IduRazorViewEngine.cs))
mengganti token `%1` dengan namespace controller setelah kata `Controllers` dibuang:

`Controllers.Transaction.GoodsReceiptPoController` → view dicari di `~/Views/Transaction/{view}.cshtml`

Konsekuensi: **namespace controller wajib cocok dengan folder Views**. Controller baru di
`namespace Controllers.Production` harus punya view di `Views/Production/...`, kalau tidak
view tidak akan ketemu.

### 2. Anatomi satu fitur transaksi

Ambil GoodsReceiptPo sebagai template kanonik. Satu fitur = 5 bagian:

```
ESKA_DI/Models/Transaction/GoodsReceiptPoService.cs        model + service (satu file)
ESKA_DI/Models/Transaction/GoodsReceiptPo__List_Model.cs   binding grid list
ESKA_APP/Controllers/Transaction/GoodsReceiptPo/
    GoodsReceiptPoController.cs             Detail/DetailPartial/Add/Update/Post/Cancel
    GoodsReceiptPoController.List.cs        ListPartial/ListPaging/ListFiltering/ListSorting
    GoodsReceiptPoController.Nav.cs         NavFirst/NavPrevious/NavNext/NavLast
    GoodsReceiptPoController.Tab.Detail.cs  grid detail di dalam form
    GoodsReceiptPoController.ViewBatch.cs   popup anak (batch, scale, cancel reason)
ESKA_APP/Views/Transaction/GoodsReceiptPo/
    GoodsReceiptPo.cshtml                   root view: semua fungsi JS + rakitan partial
    Partial/*.cshtml                        form, list, panel, tab, popup
```

Controller adalah `public partial class ... : BaseController`; setiap concern satu file.
Nama view disimpan sebagai field string di file controller utama (`VIEW_DETAIL`,
`VIEW_FORM_PARTIAL`, …).

Hierarki dokumen berjenjang tercermin di nama tabel dan model:
`Tx_GoodsReceiptPO` → `_Item` (`DetId`) → `_Item_Batch` (`DetDetId`) → `_Item_Batch_Scale` (`DetDetDetId`).

### 3. Pola Service

```csharp
public long Add(GoodsReceiptPoModel model)
{
    using (var CONTEXT = new HANA_APP())                       // context baru per operasi
    using (var CONTEXT_TRANS = CONTEXT.Database.BeginTransaction())
    {
        try {
            CopyProperty.CopyProperties(model, entity, false); // model -> entity
            var dt = CONTEXT.Database.SqlQuery<DateTime>("SELECT CURRENT_TIMESTAMP AS IDU FROM DUMMY").FirstOrDefault();
            var no = CONTEXT.Database.SqlQuery<string>("CALL \"SpSysGetNumbering\" (...)").SingleOrDefault();
            ...
            CONTEXT_TRANS.Commit();
        } catch (Exception ex) {
            CONTEXT_TRANS.Rollback();
            throw new Exception(string.Format("[VALIDATION] {0}", ex.Message));
        }
    }
}
```

Hal yang selalu berlaku:
- `DbProvider.dbApp` mengembalikan **instance `HANA_APP` baru setiap akses** — jangan asumsikan
  identity map / change tracking lintas pemanggilan.
- Nomor dokumen **tidak pernah** dibuat di C#; selalu `CALL "SpSysGetNumbering"`.
- Timestamp diambil dari database (`CURRENT_TIMESTAMP FROM DUMMY`), bukan `DateTime.Now`.
- Audit column diisi manual: `CreatedDate/CreatedUser/ModifiedDate/ModifiedUser`.
- Edit master-detail dari grid datang sebagai `Details_.insertedRowValues` /
  `modifiedRowValues` / `deletedRowKeys`, diproses berurutan di dalam satu transaksi.

### 4. Validasi bisnis ada di stored procedure `Sp<Controller>__TransNotif`

**Ini konvensi terpenting di project ini.** Validasi bisnis (bukan validasi bentuk input)
**tidak** ditulis di C# — ditulis sebagai stored procedure HANA bernama
`Sp<NamaController>__TransNotif`. Service hanya memanggil hook-nya sebelum dan sesudah
operasi berubah data:

```csharp
SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPo", CONTEXT, "before", "Tx_GoodsReceiptPO", "post", "Id", keyValue);
// ... operasi ...
SpNotif.SpSysControllerTransNotif(userId, "GoodsReceiptPo", CONTEXT, "after",  "Tx_GoodsReceiptPO", "post", "Id", keyValue);
```

Kalau mau menambah/mengubah aturan validasi, **tempatnya di procedure, bukan di service**.
Cari dulu procedure-nya di database sebelum menambah `if` di C#.

**Signature procedure** (selalu sama persis untuk semua controller):

| Parameter | Tipe | Isi |
|---|---|---|
| `UserId` | `INT` | user yang login |
| `Category` | `NVARCHAR(100)` | `before` atau `after` (untuk `add` tidak ada `before`) |
| `ObjCode` | `NVARCHAR(100)` | nama tabel / object code, mis. `Tx_GoodsReceiptPO` |
| `TransType` | `NVARCHAR(100)` | `add`, `update`, `delete`, `post`, `cancel`, `close`, `finish`, `starttask`, `requestApproval` |
| `FieldKeys` | `NVARCHAR(255)` | nama kolom kunci, hampir selalu `Id` |
| `FieldValues` | `NVARCHAR(255)` | nilai kunci baris yang sedang diproses |
| `FieldParentValues` | `NVARCHAR(255)` | kunci induk (opsional, default `''`) |

**Kontrak return** — procedure wajib mengembalikan satu baris berisi dua kolom bernama
persis `error` dan `error_message`:

```sql
SELECT :error AS "error", :error_message AS "error_message" FROM DUMMY;
```

`SpNotif` membaca hasil itu ([SpNotif.cs:88](ESKA_DI/Models/_Utils/SpNotif.cs#L88)):
`error = 0` → lanjut; selain itu dilempar sebagai
`Exception("[VALIDATION] {error} - {error_message}")`, yang lalu mengalir ke UI lewat
protokol `[VALIDATION]` di poin 5. Karena hook dipanggil **di dalam** transaksi EF,
exception itu otomatis memicu rollback.

**Skeleton standar** — semua procedure memakai kerangka yang sama; guard `IF error = 0 AND ...`
dipakai supaya hanya error pertama yang dilaporkan:

```sql
CREATE PROCEDURE "SpProductionTaskActivity__TransNotif"
(
      UserId INT,
      Category NVARCHAR(100),
      ObjCode NVARCHAR(100),
      TransType NVARCHAR(100),
      FieldKeys NVARCHAR(255),
      FieldValues NVARCHAR(255),
      FieldParentValues NVARCHAR(255)
)
AS
BEGIN
    DECLARE error INT;                      -- 0 = tidak ada error
    DECLARE error_message NVARCHAR(200);    -- pesan yang tampil ke user

    error = 0;
    error_message = 'Ok';

    IF Category = 'after' THEN
        IF TransType IN('finish') THEN

            -- Item tidak boleh muncul lebih dari satu baris
            IF error = 0 AND EXISTS(
                SELECT 1 FROM "Tx_ProductionTask_Activity_Item" T0
                WHERE "DetId" = :FieldValues
                GROUP BY "ItemCode" HAVING COUNT(*) > 1
            ) THEN
                error = -1;
                error_message = 'Item must not more than one line';
            END IF;

            -- Batch wajib diisi
            IF error = 0 AND EXISTS(
                SELECT 1 FROM "Tx_ProductionTask_Activity" T0
                WHERE "DetId" = :FieldValues
                AND COALESCE(TRIM("Batch"), '') = ''
            ) THEN
                error = -1;
                error_message = 'Batch must not null';
            END IF;

            -- Kalau Direction = 'Out', batch anaknya wajib ada minimal satu
            IF error = 0 AND EXISTS(
                SELECT 1 FROM "Tx_ProductionTask_Activity_Item" T0
                WHERE T0."DetId" = :FieldValues
                AND T0."Direction" = 'Out'
                AND NOT EXISTS(
                    SELECT 1 FROM "Tx_ProductionTask_Activity_Item_Batch" T1
                    WHERE T1."DetDetId" = T0."DetDetId"
                    AND COALESCE(TRIM(T1."Batch"), '') <> ''
                )
            ) THEN
                error = -1;
                error_message = 'Batch for Out item must be filled';
            END IF;

        END IF;
    END IF;

    SELECT :error AS "error", :error_message AS "error_message" FROM DUMMY;
END;
```

Aturan penulisan yang konsisten dipakai:
- Cabang selalu dibungkus `IF Category = ... THEN` lalu `IF TransType IN(...) THEN`, supaya
  satu procedure melayani semua operasi satu modul.
- Pemeriksaan ditulis sebagai `EXISTS(...)` yang mencari **kondisi salah** (bukan mencari yang benar).
- `FieldValues` dipakai sebagai kunci (`:FieldValues`) — perhatikan untuk modul berjenjang
  nilainya bisa `DetId`, bukan selalu `Id` header (contoh di atas memakai `DetId`).
- `error = -1` untuk error validasi biasa; `error_message` adalah teks yang dibaca user
  sehingga harus jelas dan singkat.
- Procedure yang belum punya aturan tetap dibuat dengan badan kosong (set `error = 0` lalu
  `SELECT`), bukan tidak dibuat sama sekali — karena service tetap memanggilnya.

Controller yang saat ini memanggil hook ini: `GoodsReceiptPo`, `StockOpname`, `ProcessCard`,
`ProductionTask`, `ProductionTaskActivity`, `ProductionSchedule`, `IssueAndReceipt`,
`Position`, `Approval`, `ApprovalStage`, `ApprovalTemplate`. Menambah controller transaksi
baru berarti procedure `Sp<Nama>__TransNotif` **harus** dibuat lebih dulu, kalau tidak
pemanggilan pertama langsung gagal.

Ada juga varian generik `SpNotif.SpSysTransNotif(...)` yang memanggil `SpSysTransNotif`
(satu procedure untuk semua object), dan overload yang menerima `SAPbobsCOM.Company` alih-alih
`HANA_APP` untuk dipanggil dari dalam transaksi DI API.

> **Sumber kebenaran ada di database.** `Scripts/SQLs/CreateStoredProcedure.sql` hanya memuat
> sebagian kecil procedure (`SpApprovalStage__TransNotif`, `SpApprovalTemplate__TransNotif`).
> Procedure untuk modul transaksi utama hanya hidup di schema HANA. Jadi saat menelusuri
> "kenapa validasi ini muncul", jangan berhenti di kode C# — aturannya kemungkinan besar
> ada di procedure. Idealnya procedure baru/berubah ikut dituliskan ke file script itu.

### 5. Protokol error `[VALIDATION]`

Kontrak error end-to-end memakai prefix string, bukan status code. Sumber pesan
`[VALIDATION]` terbanyak adalah hook `Sp<Controller>__TransNotif` di poin 4:

1. Service/controller melempar `Exception` dengan pesan diawali `[VALIDATION] `.
2. `BaseController.OnActionExecuted` mendeteksi prefix itu, mengubah respons jadi
   `text/plain` dengan status `500 <pesan>` dan `TrySkipIisCustomErrors = true`.
3. JS di view (`OnFailure`) mengecek `jqXhr.responseText.substring(0, 12) == "[VALIDATION]"`
   lalu menampilkan `alert`; selain itu di-parse dari `<title>` halaman error.

Pesan error yang ingin dilihat user **harus** memakai prefix ini, kalau tidak akan muncul
sebagai HTML error page. Perhatikan: beberapa kode lama memakai `ex.Message.Substring(12)`
(bukan `StartsWith`) yang akan melempar jika pesan lebih pendek dari 12 karakter — pakai
`StartsWith("[VALIDATION]")` untuk kode baru.

### 6. Autentikasi & otorisasi (berbasis Session)

Login mengisi `Session["userId"]`, `["userName"]`, `["roleName"]`, `["isAdmin"]`,
`["branchCode"]`, `["branchName"]`. Tidak ada Forms Authentication / `[Authorize]`.

`BaseController.OnActionExecuting` melakukan:
- redirect ke `Login/Detail` kalau session kosong (atau throw `[VALIDATION]` untuk request AJAX);
- kalau `isAdmin != "Y"`, cek `GeneralGetList.GetAuthAction(userId, "<Controller>/<Action>")`
  untuk action bernama `detail, add, update, post, cancel, approve, reject, waiting`
  (juga `checklayout`, `layout`, `print`).

Artinya **nama action menentukan apakah dicek otorisasi**. Action baru yang perlu diproteksi
harus memakai salah satu nama itu, atau daftarnya di `BaseController` harus ditambah.

Otorisasi tingkat baris memakai `GeneralGetList.GetFormTransAuthorizeSqlWhere(...)` yang
menghasilkan potongan `WHERE` dan di-AND-kan ke query list.

### 7. Grid DevExpress (custom binding + SQL manual)

`<Fitur>__List_Model.cs` memegang `ViewSql` (SQL dasar) dan mengimplementasi
`GetRowCount` + `GetDataList` dengan paging manual (`LIMIT/OFFSET`).
`GetSqlFromGridViewModelState.getHanaCriteria/getHanaSort` menerjemahkan state filter & sort
grid menjadi SQL HANA. Controller menyediakan empat action standar
(`ListPartial`, `ListPaging`, `ListFiltering`, `ListSorting`) yang semuanya memanggil
`ProcessCustomBinding`.

Catatan: `GetDataList` sengaja menambahkan **satu baris kosong dummy** kalau hasil query kosong,
supaya grid tetap merender kolom. Jangan dihapus tanpa mengecek tampilan grid.

### 8. Posting ke SAP B1 (DI API)

Method `Post` pada service transaksi memanggil `PostSAP`, yang menjalankan **dua transaksi
bersarang**: transaksi EF (schema aplikasi) dan transaksi SAP DI API.

```csharp
oCompany = SAPCachedCompany.GetCompany();   // pooled, di-lock (TransactionLock)
oCompany.StartTransaction();
...  // AddGoodsReceiptPO(...) -> business object DI API
if (oCompany.InTransaction) oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
CONTEXT_TRANS.Commit();
```

Aturan yang harus dijaga:
- Setiap jalur keluar (termasuk `catch`) wajib menutup transaksi SAP dengan
  `wf_Commit`/`wf_RollBack`, jika tidak koneksi pooled akan tertinggal dalam transaksi.
- Setelah sukses, `DocEntry`/`DocNum` hasil SAP ditulis balik ke header, dan `LineNum` per baris
  ditulis balik lewat `UPDATE ... CASE WHEN "DetId" = ... THEN ...`.
- Status dokumen: `Draft` → `Posted` → `Cancel` (`IsAfterPosted = "Y"` setelah posting).
  Tombol di UI diaktifkan berdasarkan status ini di fungsi JS `SetBtnStatus()`.
- Objek COM dibersihkan lewat `SapCompany.CleanUp(obj)` (`Marshal.ReleaseComObject`).

### 9. Report & Layout

- Layout Crystal disimpan sebagai **blob di tabel `Tm_Layout`**, ditulis ke
  `~/Content/Temp/<Uid>.rpt` saat dipakai pertama kali.
- `BaseController` menyediakan `DisplayPdf/DisplayExcel/DisplayCsv/DisplayText` (+varian `*Param`).
- Hak akses layout/report dicek lewat `Rpt.GetAuthLayout` / `Rpt.GetAuthReport`.
- Modul `Setting/Query` + `Report/ReportQuery` adalah engine query/report dinamis
  yang definisinya tersimpan di database (`Tm_Query`, `Tm_Report`).
- Error log Elmah tersedia di route area `Admin` → `/Admin/Elmah`.

### 10. Konvensi penamaan database

| Prefix | Arti |
|---|---|
| `Tm_` | Master / setting (`Tm_User`, `Tm_Role`, `Tm_Layout`, `Tm_ApprovalTemplate`) |
| `Tx_` | Transaksi (`Tx_GoodsReceiptPO`, `Tx_ProcessCard`, `Tx_StockOpname`) |
| `Tp_` | Proses/pending (`Tp_Approval`, `Tp_UserAlert`) |
| `Ts_` | Sistem (`Ts_Menu`, `Ts_List`, `Ts_FormatNumbering`, `Ts_ObjectApproval`) |
| `Sp…` | Stored procedure (`SpSysGetNumbering`, `SpApproval_Authorize`, `Sp<Ctrl>__TransNotif`) |

Kolom kunci berjenjang: `Id` → `DetId` → `DetDetId` → `DetDetDetId`.
Properti model dengan akhiran `_` / `___` adalah properti non-persisted untuk kebutuhan view
(`ListDetails_`, `Details_`, `GoodsReceiptPoView___`).

## Hal yang perlu diperhatikan

- **Kredensial ter-commit.** `ESKA_APP/ESKA_APP/Web.config` berisi user/password HANA dan SAP
  dalam plaintext. Jangan menyalin nilainya ke file lain, jangan menampilkannya di output, dan
  jangan menambah kredensial baru ke file yang ter-commit.
- **`Scripts/` dan `Scripts2/` hampir identik.** Duplikasi lama; cek `Web.config`/view mana yang
  benar-benar dirujuk sebelum mengubah salah satunya.
- **Banyak kode mati** dalam bentuk blok komentar besar (versi lama sebuah method sering
  dibiarkan di bawah versi aktif). Pastikan mengedit versi yang aktif, bukan yang dikomentari.
- **Komentar dan pesan validasi memakai bahasa Indonesia**; ikuti gaya ini saat menambah kode.
- **Modul `ReProcess` sebelumnya bernama `IssueAndReceipt`.** Tabel EF masih bernama
  `Tx_IssueAndReceipt*` sementara controller/model sudah `ReProcess` — ini memang begitu,
  bukan bug (lihat `Scripts/SQLs/Rename_Menu_IssueAndReceipt_To_ReProcess.sql`).
- **Tidak ada test otomatis.** Perubahan diverifikasi dengan menjalankan aplikasi dan mencoba
  alur terkait (list → form → add/update → post).

## Saat menambah fitur baru

1. Buat tabel + stored procedure di `Scripts/SQLs/`, jalankan ke HANA, update EDMX.
   Termasuk `Sp<Nama>__TransNotif` (boleh berbadan kosong dulu) — wajib ada sebelum service
   memanggilnya, dan di situlah semua validasi bisnis nanti ditulis.
2. Tambah `<Fitur>Model.cs` (model + service) dan `<Fitur>__List_Model.cs` di `ESKA_DI/Models/<Area>/`.
3. Buat controller partial di `Controllers/<Area>/<Fitur>/` dengan namespace `Controllers.<Area>`.
4. Buat view di `Views/<Area>/<Fitur>/` — salin struktur dari fitur sejenis yang sudah ada.
5. Daftarkan menu di `Ts_Menu` dan hak akses di `Tm_Role_Auth` (lihat `Insert Ts_Menu.sql`).
