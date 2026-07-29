-- =============================================================================
-- Rename route menu IssueAndReceipt -> ReProcess (untuk user NON-admin).
-- URL/route controller berubah dari "IssueAndReceipt/*" menjadi "ReProcess/*",
-- jadi baris menu & otorisasi yang meng-key ke route lama harus ikut diperbarui.
-- Admin/UserId 1 tidak perlu skrip ini (BaseController bypass).
--
-- Jalankan di schema aplikasi (mis. ESKA_APP_TEST). REPLACE hanya menyentuh
-- baris yang berawalan 'IssueAndReceipt' — TIDAK mengubah nama tabel data.
-- =============================================================================

-- Ts_Menu: kolom Url, MenuCode, ParentCode
UPDATE "Ts_Menu" SET "Url"        = REPLACE("Url",'IssueAndReceipt','ReProcess')        WHERE "Url"        LIKE 'IssueAndReceipt%';
UPDATE "Ts_Menu" SET "ParentCode" = REPLACE("ParentCode",'IssueAndReceipt','ReProcess') WHERE "ParentCode" LIKE 'IssueAndReceipt%';
UPDATE "Ts_Menu" SET "MenuCode"   = REPLACE("MenuCode",'IssueAndReceipt','ReProcess')   WHERE "MenuCode"   LIKE 'IssueAndReceipt%';

-- Tm_Role_Auth: bila menyimpan MenuCode ke route lama (sesuaikan nama kolom bila beda)
UPDATE "Tm_Role_Auth" SET "MenuCode" = REPLACE("MenuCode",'IssueAndReceipt','ReProcess') WHERE "MenuCode" LIKE 'IssueAndReceipt%';

-- Verifikasi:
-- SELECT "MenuCode","MenuName","Url","ParentCode" FROM "Ts_Menu" WHERE "MenuCode" LIKE 'ReProcess%' OR "Url" LIKE 'ReProcess%';
