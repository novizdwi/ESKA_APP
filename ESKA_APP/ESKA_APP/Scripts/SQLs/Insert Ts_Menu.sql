INSERT INTO "Ts_Menu" VALUES ('Master', 'Master', '', '', 10);

INSERT INTO "Ts_Menu" VALUES ('Item', 'Item', '', 'Master', 1001);

INSERT INTO "Ts_Menu" VALUES ('RfidMonitoring', 'RFID', '', 'Item', 100101);
INSERT INTO "Ts_Menu" VALUES ('RfidMonitoring/Detail#User', 'Detail - User', 'RfidMonitoring/Detail', 'RfidMonitoring', 10010101);
INSERT INTO "Ts_Menu" VALUES ('RfidMonitoring/Detail#All', 'Detail - All', 'RfidMonitoring/Detail', 'RfidMonitoring', 10010102);

INSERT INTO "Ts_Menu" VALUES ('ChangeItem', 'Change Item', '', 'Item', 100102);
INSERT INTO "Ts_Menu" VALUES ('ChangeItem/Detail#User', 'Detail - User', 'ChangeItem/Detail', 'ChangeItem', 10010201);
INSERT INTO "Ts_Menu" VALUES ('ChangeItem/Detail#All', 'Detail - All', 'ChangeItem/Detail', 'ChangeItem', 10010202);
INSERT INTO "Ts_Menu" VALUES ('ChangeItem/Add', 'Add', 'ChangeItem/Add', 'ChangeItem', 10010203);
INSERT INTO "Ts_Menu" VALUES ('ChangeItem/Update', 'Update', 'ChangeItem/Update', 'ChangeItem', 10010204);
INSERT INTO "Ts_Menu" VALUES ('ChangeItem/Post', 'Post', 'ChangeItem/Post', 'ChangeItem', 10010205);
INSERT INTO "Ts_Menu" VALUES ('ChangeItem/Cancel', 'Cancel', 'ChangeItem/Cancel', 'ChangeItem', 10010206);
INSERT INTO "Ts_Menu" VALUES ('ChangeItem/Print', 'Print', 'ChangeItem/Print', 'ChangeItem', 10010207);

INSERT INTO "Ts_Menu" VALUES ('ItemMaster', 'Item Master', '', 'Item', 100103);
INSERT INTO "Ts_Menu" VALUES ('ItemMaster/Detail#User', 'Detail - User', 'ItemMaster/Detail', 'ItemMaster', 10010301);
INSERT INTO "Ts_Menu" VALUES ('ItemMaster/Detail#All', 'Detail - All', 'ItemMaster/Detail', 'ItemMaster', 10010302);

INSERT INTO "Ts_Menu" VALUES ('Transaction', 'Transaction', '', '', 20);

INSERT INTO "Ts_Menu" VALUES ('Adjustment', 'Adjustment', '', 'Transaction', 2001);

INSERT INTO "Ts_Menu" VALUES ('AdjustmentIn', 'Adjustment In', 'AdjustmentIn', 'Adjustment', 200101);
INSERT INTO "Ts_Menu" VALUES ('AdjustmentIn/Detail#User', 'Detail - User', 'AdjustmentIn/Detail', 'AdjustmentIn', 200101);
INSERT INTO "Ts_Menu" VALUES ('AdjustmentIn/Detail#All', 'Detail - All', 'AdjustmentIn/Detail', 'AdjustmentIn', 200102);
INSERT INTO "Ts_Menu" VALUES ('AdjustmentIn/Add', 'Add', 'AdjustmentIn/Add', 'AdjustmentIn', 200103);
INSERT INTO "Ts_Menu" VALUES ('AdjustmentIn/Update', 'Update', 'AdjustmentIn/Update', 'AdjustmentIn', 200104);
INSERT INTO "Ts_Menu" VALUES ('AdjustmentIn/Post', 'Post', 'AdjustmentIn/Post', 'AdjustmentIn', 200105);
INSERT INTO "Ts_Menu" VALUES ('AdjustmentIn/Cancel', 'Cancel', 'AdjustmentIn/Cancel', 'AdjustmentIn', 200106);
INSERT INTO "Ts_Menu" VALUES ('AdjustmentIn/IsOpeningBalance', 'Opening Balance', 'AdjustmentIn/IsOpeningBalance', 'AdjustmentIn', 200107);
INSERT INTO "Ts_Menu" VALUES ('AdjustmentIn/Print', 'Print', 'AdjustmentIn/Print', 'AdjustmentIn', 200110);

INSERT INTO "Ts_Menu" VALUES ('AdjustmentOut', 'Adjustment Out', 'AdjustmentOut', 'Adjustment', 200102);
INSERT INTO "Ts_Menu" VALUES ('AdjustmentOut/Detail#User', 'Detail - User', 'AdjustmentOut/Detail', 'AdjustmentOut', 20010201);
INSERT INTO "Ts_Menu" VALUES ('AdjustmentOut/Detail#All', 'Detail - All', 'AdjustmentOut/Detail', 'AdjustmentOut', 20010202);
INSERT INTO "Ts_Menu" VALUES ('AdjustmentOut/Add', 'Add', 'AdjustmentOut/Add', 'AdjustmentOut', 20010203);
INSERT INTO "Ts_Menu" VALUES ('AdjustmentOut/Update', 'Update', 'AdjustmentOut/Update', 'AdjustmentOut', 20010204);
INSERT INTO "Ts_Menu" VALUES ('AdjustmentOut/Post', 'Post', 'AdjustmentOut/Post', 'AdjustmentOut', 20010205);
INSERT INTO "Ts_Menu" VALUES ('AdjustmentOut/Cancel', 'Cancel', 'AdjustmentOut/Cancel', 'AdjustmentOut', 20010206);
INSERT INTO "Ts_Menu" VALUES ('AdjustmentOut/IsOpeningBalance', 'Opening Balance', 'AdjustmentOut/IsOpeningBalance', 'AdjustmentOut', 20010207);
INSERT INTO "Ts_Menu" VALUES ('AdjustmentOut/Print', 'Print', 'AdjustmentOut/Print', 'AdjustmentOut', 20010210);

INSERT INTO "Ts_Menu" VALUES ('Inventory', 'Inventory', NULL, 'Transaction', 2002);

INSERT INTO "Ts_Menu" VALUES ('TransferRequest', 'Transfer Request', 'TransferRequest', 'Inventory', 200201);
INSERT INTO "Ts_Menu" VALUES ('TransferRequest/Detail#User', 'Detail - User', 'TransferRequest/Detail', 'TransferRequest', 20020101);
INSERT INTO "Ts_Menu" VALUES ('TransferRequest/Detail#All', 'Detail - All', 'TransferRequest/Detail', 'TransferRequest', 20020102);
INSERT INTO "Ts_Menu" VALUES ('TransferRequest/Add', 'Add', 'TransferRequest/Add', 'TransferRequest', 20020103);
INSERT INTO "Ts_Menu" VALUES ('TransferRequest/Update', 'Update', 'TransferRequest/Update', 'TransferRequest', 20020104);
INSERT INTO "Ts_Menu" VALUES ('TransferRequest/Post', 'Post', 'TransferRequest/Post', 'TransferRequest', 20020105);
INSERT INTO "Ts_Menu" VALUES ('TransferRequest/Cancel', 'Cancel', 'TransferRequest/Cancel', 'TransferRequest', 20020106);
INSERT INTO "Ts_Menu" VALUES ('TransferRequest/Print', 'Print', 'TransferRequest/Print', 'TransferRequest', 20020110);

INSERT INTO "Ts_Menu" VALUES ('TransferIn', 'Transfer In', 'TransferIn', 'Inventory', 200202);
INSERT INTO "Ts_Menu" VALUES ('TransferIn/Detail#User', 'Detail - User', 'TransferIn/Detail', 'TransferIn', 20020201);
INSERT INTO "Ts_Menu" VALUES ('TransferIn/Detail#All', 'Detail - All', 'TransferIn/Detail', 'TransferIn', 20020202);
INSERT INTO "Ts_Menu" VALUES ('TransferIn/Add', 'Add', 'TransferIn/Add', 'TransferIn', 20020203);
INSERT INTO "Ts_Menu" VALUES ('TransferIn/Update', 'Update', 'TransferIn/Update', 'TransferIn', 20020204);
INSERT INTO "Ts_Menu" VALUES ('TransferIn/Post', 'Post', 'TransferIn/Post', 'TransferIn', 20020205);
INSERT INTO "Ts_Menu" VALUES ('TransferIn/Cancel', 'Cancel', 'TransferIn/Cancel', 'TransferIn', 20020206);
INSERT INTO "Ts_Menu" VALUES ('TransferIn/Print', 'Print', 'TransferIn/Print', 'TransferIn', 20020210);

INSERT INTO "Ts_Menu" VALUES ('TransferOut', 'Transfer Out', 'TransferOut', 'Inventory', 200203);
INSERT INTO "Ts_Menu" VALUES ('TransferOut/Detail#User', 'Detail - User', 'TransferOut/Detail', 'TransferOut', 20020301);
INSERT INTO "Ts_Menu" VALUES ('TransferOut/Detail#All', 'Detail - All', 'TransferOut/Detail', 'TransferOut', 20020302);
INSERT INTO "Ts_Menu" VALUES ('TransferOut/Add', 'Add', 'TransferOut/Add', 'TransferOut', 20020303);
INSERT INTO "Ts_Menu" VALUES ('TransferOut/Update', 'Update', 'TransferOut/Update', 'TransferOut', 20020304);
INSERT INTO "Ts_Menu" VALUES ('TransferOut/Post', 'Post', 'TransferOut/Post', 'TransferOut', 20020305);
INSERT INTO "Ts_Menu" VALUES ('TransferOut/Cancel', 'Cancel', 'TransferOut/Cancel', 'TransferOut', 20020306);
INSERT INTO "Ts_Menu" VALUES ('TransferOut/Print', 'Print', 'TransferOut/Print', 'TransferOut', 20020310);

INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn', 'Transfer Summary In', 'TransferSummaryIn', 'Inventory', 200204);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn/Detail#User', 'Detail - User', 'TransferSummaryIn/Detail', 'TransferSummaryIn', 20020401);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn/Detail#All', 'Detail - All', 'TransferSummaryIn/Detail', 'TransferSummaryIn', 20020402);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn/Add', 'Add', 'TransferSummaryIn/Add', 'TransferSummaryIn', 20020403);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn/Update', 'Update', 'TransferSummaryIn/Update', 'TransferSummaryIn', 20020404);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn/Post', 'Post', 'TransferSummaryIn/Post', 'TransferSummaryIn', 20020405);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn/Cancel', 'Cancel', 'TransferSummaryIn/Cancel', 'TransferSummaryIn', 20020406);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn/Approve','Approve','TransferSummaryIn/Approve','TransferSummaryIn', 20020407);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn/Reject','Reject','TransferSummaryIn/Reject','TransferSummaryIn', 20020408);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryIn/Print', 'Print', 'TransferSummaryIn/Print', 'TransferSummaryIn', 20020410);

INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut', 'Transfer Summary Out', 'TransferSummaryOut', 'Inventory', 200205);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut/Detail#User', 'Detail - User', 'TransferSummaryOut/Detail', 'TransferSummaryOut', 20020501);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut/Detail#All', 'Detail - All', 'TransferSummaryOut/Detail', 'TransferSummaryOut', 20020502);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut/Add', 'Add', 'TransferSummaryOut/Add', 'TransferSummaryOut', 20020503);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut/Update', 'Update', 'TransferSummaryOut/Update', 'TransferSummaryOut', 20020504);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut/Post', 'Post', 'TransferSummaryOut/Post', 'TransferSummaryOut', 20020505);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut/Cancel', 'Cancel', 'TransferSummaryOut/Cancel', 'TransferSummaryOut', 20020506);
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut/Approve','Approve','TransferSummaryOut/Approve','TransferSummaryOut','20020507');
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut/Reject','Reject','TransferSummaryOut/Reject','TransferSummaryOut','20020508');
INSERT INTO "Ts_Menu" VALUES ('TransferSummaryOut/Print', 'Print', 'TransferSummaryOut/Print', 'TransferSummaryOut', 20020510);

INSERT INTO "Ts_Menu" VALUES ('StockOpname', 'Stock Opname', NULL, 'Transaction', 2003);

INSERT INTO "Ts_Menu" VALUES ('RequestStockOpname', 'Request Stock Opname', 'RequestStockOpname', 'StockOpname', 200301);
INSERT INTO "Ts_Menu" VALUES ('RequestStockOpname/Detail#User', 'Detail - User', 'RequestStockOpname/Detail', 'RequestStockOpname', 20030101);
INSERT INTO "Ts_Menu" VALUES ('RequestStockOpname/Detail#All', 'Detail - All', 'RequestStockOpname/Detail', 'RequestStockOpname', 20030102);
INSERT INTO "Ts_Menu" VALUES ('RequestStockOpname/Add', 'Add', 'RequestStockOpname/Add', 'RequestStockOpname', 20030103);
INSERT INTO "Ts_Menu" VALUES ('RequestStockOpname/Update', 'Update', 'RequestStockOpname/Update', 'RequestStockOpname', 20030104);
INSERT INTO "Ts_Menu" VALUES ('RequestStockOpname/Post', 'Post', 'RequestStockOpname/Post', 'RequestStockOpname', 20030105);
INSERT INTO "Ts_Menu" VALUES ('RequestStockOpname/Cancel', 'Cancel', 'RequestStockOpname/Cancel', 'RequestStockOpname', 20030106);
INSERT INTO "Ts_Menu" VALUES ('RequestStockOpname/Print', 'Print', 'RequestStockOpname/Print', 'RequestStockOpname', 20030110);

INSERT INTO "Ts_Menu" VALUES ('StockOpnameScan', 'Stock Opname Scan List', 'StockOpnameScan', 'StockOpname', 200302);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameScan/Detail#User', 'Detail - User', 'StockOpnameScan/Detail', 'StockOpnameScan', 20030201);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameScan/Detail#All', 'Detail - All', 'StockOpnameScan/Detail', 'StockOpnameScan', 20030202);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameScan/Add', 'Add', 'StockOpnameScan/Add', 'StockOpnameScan', 20030203);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameScan/Update', 'Update', 'StockOpnameScan/Update', 'StockOpnameScan', 20030204);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameScan/Post', 'Post', 'StockOpnameScan/Post', 'StockOpnameScan', 20030205);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameScan/Cancel', 'Cancel', 'StockOpnameScan/Cancel', 'StockOpnameScan', 20030206);
INSERT INTO "Ts_Menu" VALUES ('StockOpnameScan/Print', 'Print', 'StockOpnameScan/Print', 'StockOpnameScan', 20030210);

INSERT INTO "Ts_Menu" VALUES ('StockSummaryOpname', 'Stock Opname Summary', 'StockSummaryOpname', 'StockOpname', 200303);
INSERT INTO "Ts_Menu" VALUES ('StockSummaryOpname/Detail#User', 'Detail - User', 'StockSummaryOpname/Detail', 'StockSummaryOpname', 20030301);
INSERT INTO "Ts_Menu" VALUES ('StockSummaryOpname/Detail#All', 'Detail - All', 'StockSummaryOpname/Detail', 'StockSummaryOpname', 20030302);
INSERT INTO "Ts_Menu" VALUES ('StockSummaryOpname/Add', 'Add', 'StockSummaryOpname/Add', 'StockSummaryOpname', 20030303);
INSERT INTO "Ts_Menu" VALUES ('StockSummaryOpname/Update', 'Update', 'StockSummaryOpname/Update', 'StockSummaryOpname', 20030304);
INSERT INTO "Ts_Menu" VALUES ('StockSummaryOpname/Post', 'Post', 'StockSummaryOpname/Post', 'StockSummaryOpname', 20030305);
INSERT INTO "Ts_Menu" VALUES ('StockSummaryOpname/Cancel', 'Cancel', 'StockSummaryOpname/Cancel', 'StockSummaryOpname', 20030306);
INSERT INTO "Ts_Menu" VALUES ('StockSummaryOpname/Print', 'Print', 'StockSummaryOpname/Print', 'StockSummaryOpname', 20030310);



INSERT INTO "Ts_Menu" VALUES ('Purchasing', 'Purchasing', NULL, 'Transaction', 2004);

INSERT INTO "Ts_Menu" VALUES ('PurchaseOrderScan', 'PO Scan List', 'PurchaseOrderScan', 'Purchasing', 200401);
INSERT INTO "Ts_Menu" VALUES ('PurchaseOrderScan/Detail#User', 'Detail - User', 'PurchaseOrderScan/Detail', 'PurchaseOrderScan', 20040101);
INSERT INTO "Ts_Menu" VALUES ('PurchaseOrderScan/Detail#All', 'Detail - All', 'PurchaseOrderScan/Detail', 'PurchaseOrderScan', 20040102);
INSERT INTO "Ts_Menu" VALUES ('PurchaseOrderScan/Add', 'Add', 'PurchaseOrderScan/Add', 'PurchaseOrderScan', 20040103);
INSERT INTO "Ts_Menu" VALUES ('PurchaseOrderScan/Post', 'Post', 'PurchaseOrderScan/Post', 'PurchaseOrderScan', 20040105);
INSERT INTO "Ts_Menu" VALUES ('PurchaseOrderScan/Cancel', 'Cancel', 'PurchaseOrderScan/Cancel', 'PurchaseOrderScan', 20040106);
INSERT INTO "Ts_Menu" VALUES ('PurchaseOrderScan/Print', 'Print', 'PurchaseOrderScan/Print', 'PurchaseOrderScan', 20040110);

INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO', 'Goods Receipt PO', 'GoodsReceiptPO', 'Purchasing', 200402);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Detail#User', 'Detail - User', 'GoodsReceiptPO/Detail', 'GoodsReceiptPO', 20040201);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Detail#All', 'Detail - All', 'GoodsReceiptPO/Detail', 'GoodsReceiptPO', 20040202);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Add', 'Add', 'GoodsReceiptPO/Add', 'GoodsReceiptPO', 20040203);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Post', 'Post', 'GoodsReceiptPO/Post', 'GoodsReceiptPO', 20040205);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Cancel', 'Cancel', 'GoodsReceiptPO/Cancel', 'GoodsReceiptPO', 20040206);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/RefreshItem', 'RefreshItem', 'GoodsReceiptPO/RefreshItem', 'GoodsReceiptPO', 20040207);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Print', 'Print', 'GoodsReceiptPO/Print', 'GoodsReceiptPO', 20040210);

INSERT INTO "Ts_Menu" VALUES ('Reports', 'Report', NULL, NULL, 30);
INSERT INTO "Ts_Menu" VALUES ('ReportCustom', 'Custom Report', NULL, 'Reports', 3003);
INSERT INTO "Ts_Menu" VALUES ('ReportCustom/Detail', 'Detail', 'ReportCustom/Detail', 'ReportCustom', 300301);

INSERT INTO "Ts_Menu" VALUES ('Setting', 'Setting', NULL, NULL, 40);
INSERT INTO "Ts_Menu" VALUES ('Setting-Report', 'Report', NULL, 'Setting', 4002);
INSERT INTO "Ts_Menu" VALUES ('ReportGroup', 'Report Group', NULL, 'Setting-Report', 400202);
INSERT INTO "Ts_Menu" VALUES ('ReportGroup/Detail', 'Detail', 'Report/Detail', 'ReportGroup', 40020201);
INSERT INTO "Ts_Menu" VALUES ('ReportGroup/Add', 'Add', 'Report/Add', 'ReportGroup', 40020202);
INSERT INTO "Ts_Menu" VALUES ('ReportGroup/Update', 'Update', 'Report/Update', 'ReportGroup', 40020203);
INSERT INTO "Ts_Menu" VALUES ('ReportGroup/Delete', 'Delete', 'Report/Delete', 'ReportGroup', 40020204);

INSERT INTO "Ts_Menu" VALUES ('Report', 'Report', NULL, 'Setting-Report', 400203);
INSERT INTO "Ts_Menu" VALUES ('Report/Detail', 'Detail', 'Report/Detail', 'Report', 40020301);
INSERT INTO "Ts_Menu" VALUES ('Report/Add', 'Add', 'Report/Add', 'Report', 40020302);
INSERT INTO "Ts_Menu" VALUES ('Report/Update', 'Update', 'Report/Update', 'Report', 40020303);
INSERT INTO "Ts_Menu" VALUES ('Report/Delete', 'Delete', 'Report/Delete', 'Report', 40020304);

INSERT INTO "Ts_Menu" VALUES ('Setting-Approval', 'Approval', NULL, 'Setting', 4004);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStage', 'Approval Stage', NULL, 'Setting-Approval', 400401);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStage/Detail', 'Detail', 'ApprovalStage/Detail', 'ApprovalStage', 40040101);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStage/Add', 'Add', 'ApprovalStage/Add', 'ApprovalStage', 40040102);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStage/Update', 'Update', 'ApprovalStage/Update', 'ApprovalStage', 40040103);
INSERT INTO "Ts_Menu" VALUES ('ApprovalTemplate', 'Approval Template', NULL, 'Setting-Approval', 400402);
INSERT INTO "Ts_Menu" VALUES ('ApprovalTemplate/Detail', 'Detail', 'ApprovalTemplate/Detail', 'ApprovalTemplate', 40040201);
INSERT INTO "Ts_Menu" VALUES ('ApprovalTemplate/Add', 'Add', 'ApprovalTemplate/Add', 'ApprovalTemplate', 40040202);
INSERT INTO "Ts_Menu" VALUES ('ApprovalTemplate/Update', 'Update', 'ApprovalTemplate/Update', 'ApprovalTemplate', 40040203);

INSERT INTO "Ts_Menu" VALUES ('Authentication', 'Authentication', NULL, NULL, 50);
INSERT INTO "Ts_Menu" VALUES ('Role', 'Role', NULL, 'Authentication', 5001);
INSERT INTO "Ts_Menu" VALUES ('Role/Detail', 'Detail', 'Role/Detail', 'Role', 500101);
INSERT INTO "Ts_Menu" VALUES ('Role/Add', 'Add', 'Role/Add', 'Role', 500102);
INSERT INTO "Ts_Menu" VALUES ('Role/Update', 'Update', 'Role/Update', 'Role', 500103);
INSERT INTO "Ts_Menu" VALUES ('Role/Delete', 'Delete', 'Role/Delete', 'Role', 500104);
INSERT INTO "Ts_Menu" VALUES ('User', 'User', NULL, 'Authentication', 5002);
INSERT INTO "Ts_Menu" VALUES ('User/Detail', 'Detail', 'User/Detail', 'User', 500201);
INSERT INTO "Ts_Menu" VALUES ('User/Add', 'Add', 'User/Add', 'User', 500202);
INSERT INTO "Ts_Menu" VALUES ('User/Update', 'Update', 'User/Update', 'User', 500203);
INSERT INTO "Ts_Menu" VALUES ('User/Delete', 'Delete', 'User/Delete', 'User', 500204);
INSERT INTO "Ts_Menu" VALUES ('ChangePassword', 'Change Password', NULL, 'Authentication', 5003);
INSERT INTO "Ts_Menu" VALUES ('ChangePassword/Change', 'Change Password - Change', 'ChangePassword/Detail', 'ChangePassword', 500301);
INSERT INTO "Ts_Menu" VALUES ('ChangePassword/Detail', 'Change Password - Detail', 'ChangePassword/Change', 'ChangePassword', 500302);

INSERT INTO "Ts_Menu" VALUES ('Approval', 'Approval', NULL, NULL, 60);
INSERT INTO "Ts_Menu" VALUES ('ApprovalDecision', 'Approval Decision', NULL, 'Approval', 6001);
INSERT INTO "Ts_Menu" VALUES ('ApprovalDecision/Detail', 'Detail', 'ApprovalDecision/Detail', 'ApprovalDecision', 600101);
INSERT INTO "Ts_Menu" VALUES ('ApprovalDecision/Update', 'Update', 'ApprovalDecision/Update', 'ApprovalDecision', 600102);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStatus', 'Approval Status', NULL, 'Approval', 6002);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStatus/Detail#All', 'Detail - All', 'ApprovalStatus/Detail', 'ApprovalStatus', 600201);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStatus/Detail#User', 'Detail - User', 'ApprovalStatus/Detail', 'ApprovalStatus', 600202);
