INSERT INTO "Ts_Menu" VALUES ('Master', 'Master', '', '', 10);
INSERT INTO "Ts_Menu" VALUES ('Position', 'Position', '', 'Master', 1001);
INSERT INTO "Ts_Menu" VALUES ('Position/Detail#All', 'Detail - All', 'Position/Detail', 'Position', 100101);
INSERT INTO "Ts_Menu" VALUES ('Position/Add', 'Add', 'Position/Add', 'Position', 100102);
INSERT INTO "Ts_Menu" VALUES ('Position/Cancel', 'Cancel', 'Position/Cancel', 'Position', 100108);
INSERT INTO "Ts_Menu" VALUES ('Position/Print', 'Print', 'Position/Print', 'Position', 100109);

INSERT INTO "Ts_Menu" VALUES ('Production', 'Production', '', '', 20);

INSERT INTO "Ts_Menu" VALUES ('ProcessCardMonitoring', 'Process Card Monitoring', '', 'Production', 2001);
INSERT INTO "Ts_Menu" VALUES ('ProcessCardMonitoring/Detail#User', 'Detail - User', 'ProcessCardMonitoring/Detail', 'ProcessCardMonitoring', 200101);
INSERT INTO "Ts_Menu" VALUES ('ProcessCardMonitoring/Detail#All', 'Detail - All', 'ProcessCardMonitoring/Detail', 'ProcessCardMonitoring', 200102);
INSERT INTO "Ts_Menu" VALUES ('ProcessCardMonitoring/Post', 'Post', 'ProcessCardMonitoring/Post', 'ProcessCardMonitoring', 200103);

INSERT INTO "Ts_Menu" VALUES ('ProcessCard', 'Process Card', '', 'Production', 2002);
INSERT INTO "Ts_Menu" VALUES ('ProcessCard/Detail#User', 'Detail - User', 'ProcessCard/Detail', 'ProcessCard', 200201);
INSERT INTO "Ts_Menu" VALUES ('ProcessCard/Detail#All', 'Detail - All', 'ProcessCard/Detail', 'ProcessCard', 200202);
INSERT INTO "Ts_Menu" VALUES ('ProcessCard/Add', 'Add', 'ProcessCard/Add', 'ProcessCard', 200203);
INSERT INTO "Ts_Menu" VALUES ('ProcessCard/Update', 'Update', 'ProcessCard/Update', 'ProcessCard', 200204);
INSERT INTO "Ts_Menu" VALUES ('ProcessCard/Post', 'Post', 'ProcessCard/Post', 'ProcessCard', 200205);
INSERT INTO "Ts_Menu" VALUES ('ProcessCard/Cancel', 'Cancel', 'ProcessCard/Cancel', 'ProcessCard', 200208);
INSERT INTO "Ts_Menu" VALUES ('ProcessCard/Print', 'Print', 'ProcessCard/Print', 'ProcessCard', 200209);

INSERT INTO "Ts_Menu" VALUES ('ProductionSchedule', 'Production Schedule', '', 'Production', 2003);
INSERT INTO "Ts_Menu" VALUES ('ProductionSchedule/Detail#User', 'Detail - User', 'ProductionSchedule/Detail', 'ProductionSchedule', 200301);
INSERT INTO "Ts_Menu" VALUES ('ProductionSchedule/Detail#All', 'Detail - All', 'ProductionSchedule/Detail', 'ProductionSchedule', 200302);
INSERT INTO "Ts_Menu" VALUES ('ProductionSchedule/Update', 'Update', 'ProductionSchedule/Update', 'ProductionSchedule', 200303);

INSERT INTO "Ts_Menu" VALUES ('Transaction', 'Transaction', '', '', 30);

INSERT INTO "Ts_Menu" VALUES ('IssueAndReceipt', 'Issue And Receipt', '', 'Transaction', 3001);
INSERT INTO "Ts_Menu" VALUES ('IssueAndReceipt/Detail#User', 'Detail - User', 'StockOpname/Detail', 'IssueAndReceipt', 300101);
INSERT INTO "Ts_Menu" VALUES ('IssueAndReceipt/Detail#All', 'Detail - All', 'StockOpname/Detail', 'IssueAndReceipt', 300102);
INSERT INTO "Ts_Menu" VALUES ('IssueAndReceipt/Add', 'Add', 'IssueAndReceipt/Add', 'IssueAndReceipt', 300103);
INSERT INTO "Ts_Menu" VALUES ('IssueAndReceipt/Update', 'Update', 'IssueAndReceipt/Update', 'IssueAndReceipt', 300104);
INSERT INTO "Ts_Menu" VALUES ('IssueAndReceipt/Post', 'Post', 'IssueAndReceipt/Post', 'IssueAndReceipt', 300105);
INSERT INTO "Ts_Menu" VALUES ('IssueAndReceipt/Cancel', 'Cancel', 'IssueAndReceipt/Cancel', 'IssueAndReceipt', 300108);
INSERT INTO "Ts_Menu" VALUES ('IssueAndReceipt/Print', 'Print', 'IssueAndReceipt/Print', 'IssueAndReceipt', 300109);

INSERT INTO "Ts_Menu" VALUES ('InventoryTransfer', 'Inventory Transfer', '', 'Transaction', 3002);
INSERT INTO "Ts_Menu" VALUES ('InventoryTransfer/Detail#User', 'Detail - User', 'InventoryTransfer/Detail', 'InventoryTransfer', 300201);
INSERT INTO "Ts_Menu" VALUES ('InventoryTransfer/Detail#All', 'Detail - All', 'InventoryTransfer/Detail', 'InventoryTransfer', 300202);
INSERT INTO "Ts_Menu" VALUES ('InventoryTransfer/Add', 'Add', 'InventoryTransfer/Add', 'InventoryTransfer', 300203);
INSERT INTO "Ts_Menu" VALUES ('InventoryTransfer/Update', 'Update', 'InventoryTransfer/Update', 'InventoryTransfer', 300204);
INSERT INTO "Ts_Menu" VALUES ('InventoryTransfer/Post', 'Post', 'InventoryTransfer/Post', 'InventoryTransfer', 300205);
INSERT INTO "Ts_Menu" VALUES ('InventoryTransfer/Cancel', 'Cancel', 'InventoryTransfer/Cancel', 'InventoryTransfer', 300208);
INSERT INTO "Ts_Menu" VALUES ('InventoryTransfer/Print', 'Print', 'InventoryTransfer/Print', 'InventoryTransfer', 300209);
     
INSERT INTO "Ts_Menu" VALUES ('StockOpname', 'Stock Opname', '', 'Transaction', 3003);
INSERT INTO "Ts_Menu" VALUES ('StockOpname/Detail#User', 'Detail - User', 'StockOpname/Detail', 'StockOpname', 300301);
INSERT INTO "Ts_Menu" VALUES ('StockOpname/Detail#All', 'Detail - All', 'StockOpname/Detail', 'StockOpname', 300302);
INSERT INTO "Ts_Menu" VALUES ('StockOpname/Add', 'Add', 'StockOpname/Add', 'StockOpname', 300303);
INSERT INTO "Ts_Menu" VALUES ('StockOpname/Update', 'Update', 'StockOpname/Update', 'StockOpname', 300304);
INSERT INTO "Ts_Menu" VALUES ('StockOpname/Post', 'Post', 'StockOpname/Post', 'StockOpname', 300305);
INSERT INTO "Ts_Menu" VALUES ('StockOpname/Cancel', 'Cancel', 'StockOpname/Cancel', 'StockOpname', 300308);
INSERT INTO "Ts_Menu" VALUES ('StockOpname/Print', 'Print', 'StockOpname/Print', 'StockOpname', 300309);

INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO', 'Goods Receipt PO', '', 'Transaction', 3004);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Detail#User', 'Detail - User', 'StockOpname/Detail', 'GoodsReceiptPO', 300401);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Detail#All', 'Detail - All', 'StockOpname/Detail', 'GoodsReceiptPO', 300402);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Add', 'Add', 'GoodsReceiptPO/Add', 'GoodsReceiptPO', 300403);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Update', 'Update', 'GoodsReceiptPO/Update', 'GoodsReceiptPO', 300404);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Post', 'Post', 'GoodsReceiptPO/Post', 'GoodsReceiptPO', 300405);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Cancel', 'Cancel', 'GoodsReceiptPO/Cancel', 'GoodsReceiptPO', 300408);
INSERT INTO "Ts_Menu" VALUES ('GoodsReceiptPO/Print', 'Print', 'GoodsReceiptPO/Print', 'GoodsReceiptPO', 300409);




INSERT INTO "Ts_Menu" VALUES ('Reports', 'Report', '', '', 70);
INSERT INTO "Ts_Menu" VALUES ('ReportQuery', 'Query', '', 'Reports', 7002);
INSERT INTO "Ts_Menu" VALUES ('ReportQuery/Detail', 'Detail', 'ReportQuery/Detail', 'ReportQuery', 700201);
INSERT INTO "Ts_Menu" VALUES ('ReportCustom', 'Custom Report', NULL, 'Reports', 7003);
INSERT INTO "Ts_Menu" VALUES ('ReportCustom/Detail', 'Detail', 'ReportCustom/Detail', 'ReportCustom', 700301);

INSERT INTO "Ts_Menu" VALUES ('Setting', 'Setting', '', '', 80);

INSERT INTO "Ts_Menu" VALUES ('GeneralSetting', 'General Setting', '', 'Setting', 8001);
INSERT INTO "Ts_Menu" VALUES ('GeneralSetting/Detail', 'Detail', 'GeneralSetting/Detail', 'GeneralSetting', 800101);
INSERT INTO "Ts_Menu" VALUES ('GeneralSetting/Update', 'Update', 'GeneralSetting/Update', 'GeneralSetting', 800102);

INSERT INTO "Ts_Menu" VALUES ('Setting-Report', 'Report', '', 'Setting', 8002);

INSERT INTO "Ts_Menu" VALUES ('Layout', 'Form Layout', NULL, 'Setting-Report', 800201);
INSERT INTO "Ts_Menu" VALUES ('Layout/Detail', 'Detail', 'Layout/Detail', 'Layout', 80020101);
INSERT INTO "Ts_Menu" VALUES ('Layout/Add', 'Add', 'Layout/Add', 'Layout', 80020102);
INSERT INTO "Ts_Menu" VALUES ('Layout/Update', 'Update', 'Layout/Update', 'Layout', 80020103);
INSERT INTO "Ts_Menu" VALUES ('Layout/Delete', 'Delete', 'Layout/Delete', 'Layout', 80020104);

INSERT INTO "Ts_Menu" VALUES ('ReportGroup', 'Report Group', NULL, 'Setting-Report', 800202);
INSERT INTO "Ts_Menu" VALUES ('ReportGroup/Detail', 'Detail', 'ReportGroup/Detail', 'ReportGroup', 80020201);
INSERT INTO "Ts_Menu" VALUES ('ReportGroup/Add', 'Add', 'ReportGroup/Add', 'ReportGroup', 80020202);
INSERT INTO "Ts_Menu" VALUES ('ReportGroup/Update', 'Update', 'ReportGroup/Update', 'ReportGroup', 80020203);
INSERT INTO "Ts_Menu" VALUES ('ReportGroup/Delete', 'Delete', 'ReportGroup/Delete', 'ReportGroup', 80020204);

INSERT INTO "Ts_Menu" VALUES ('Report', 'Report', NULL, 'Setting-Report', 800203);
INSERT INTO "Ts_Menu" VALUES ('Report/Detail', 'Detail', 'Report/Detail', 'Report', 80020301);
INSERT INTO "Ts_Menu" VALUES ('Report/Add', 'Add', 'Report/Add', 'Report', 80020302);
INSERT INTO "Ts_Menu" VALUES ('Report/Update', 'Update', 'Report/Update', 'Report', 80020303);
INSERT INTO "Ts_Menu" VALUES ('Report/Delete', 'Delete', 'Report/Delete', 'Report', 80020304);

INSERT INTO "Ts_Menu" VALUES ('Setting-Query', 'Query', '', 'Setting', 4003);

INSERT INTO "Ts_Menu" VALUES ('QueryGroup', 'Query Group', NULL, 'Setting-Query', 400301);
INSERT INTO "Ts_Menu" VALUES ('QueryGroup/Detail', 'Detail', 'QueryGroup/Detail', 'QueryGroup', 40030101);
INSERT INTO "Ts_Menu" VALUES ('QueryGroup/Add', 'Add', 'QueryGroup/Add', 'QueryGroup', 40030102);
INSERT INTO "Ts_Menu" VALUES ('QueryGroup/Update', 'Update', 'QueryGroup/Update', 'QueryGroup', 40030103);
INSERT INTO "Ts_Menu" VALUES ('QueryGroup/Delete', 'Delete', 'QueryGroup/Delete', 'QueryGroup', 40030104);
INSERT INTO "Ts_Menu" VALUES ('Query', 'Query', NULL, 'Setting-Query', 400302);
INSERT INTO "Ts_Menu" VALUES ('Query/Detail', 'Detail', 'Query/Detail', 'Query', 40030201);
INSERT INTO "Ts_Menu" VALUES ('Query/Add', 'Add', 'Query/Add', 'Query', 40030202);
INSERT INTO "Ts_Menu" VALUES ('Query/Update', 'Update', 'Query/Update', 'Query', 40030203);
INSERT INTO "Ts_Menu" VALUES ('Query/Delete', 'Delete', 'Query/Delete', 'Query', 40030204);

INSERT INTO "Ts_Menu" VALUES ('Setting-Approval', 'Approval', '', 'Setting', 4004);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStage', 'Approval Stage', NULL, 'Setting-Approval', 400401);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStage/Detail', 'Detail', 'ApprovalStage/Detail', 'ApprovalStage', 40040101);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStage/Add', 'Add', 'ApprovalStage/Add', 'ApprovalStage', 40040102);
INSERT INTO "Ts_Menu" VALUES ('ApprovalStage/Update', 'Update', 'ApprovalStage/Update', 'ApprovalStage', 40040103);
INSERT INTO "Ts_Menu" VALUES ('ApprovalTemplate', 'Approval Template', NULL, 'Setting-Approval', 400402);
INSERT INTO "Ts_Menu" VALUES ('ApprovalTemplate/Detail', 'Detail', 'ApprovalTemplate/Detail', 'ApprovalTemplate', 40040201);
INSERT INTO "Ts_Menu" VALUES ('ApprovalTemplate/Add', 'Add', 'ApprovalTemplate/Add', 'ApprovalTemplate', 40040202);
INSERT INTO "Ts_Menu" VALUES ('ApprovalTemplate/Update', 'Update', 'ApprovalTemplate/Update', 'ApprovalTemplate', 40040203);

INSERT INTO "Ts_Menu" VALUES ('Authentication', 'Authentication', '', '', 90);

INSERT INTO "Ts_Menu" VALUES ('Role', 'Role', '', 'Authentication', 9001);
INSERT INTO "Ts_Menu" VALUES ('Role/Detail', 'Detail', 'Role/Detail', 'Role', 900101);
INSERT INTO "Ts_Menu" VALUES ('Role/Add', 'Add', 'Role/Add', 'Role', 900102);
INSERT INTO "Ts_Menu" VALUES ('Role/Update', 'Update', 'Role/Update', 'Role', 900103);
INSERT INTO "Ts_Menu" VALUES ('Role/Delete', 'Delete', 'Role/Delete', 'Role', 900104);
INSERT INTO "Ts_Menu" VALUES ('Role/Duplicate', 'Duplicate', 'Role/Duplicate', 'Role', 900105);

INSERT INTO "Ts_Menu" VALUES ('User', 'User', '', 'Authentication', 9002);
INSERT INTO "Ts_Menu" VALUES ('User/Detail', 'Detail', 'User/Detail', 'User', 900201);
INSERT INTO "Ts_Menu" VALUES ('User/Add', 'Add', 'User/Add', 'User', 900202);
INSERT INTO "Ts_Menu" VALUES ('User/Update', 'Update', 'User/Update', 'User', 900203);
INSERT INTO "Ts_Menu" VALUES ('User/SetApproval', 'Set Approval', 'BusinessPartner/SetApproval', 'User', 900204);
INSERT INTO "Ts_Menu" VALUES ('User/Delete', 'Delete', 'User/Delete', 'User', 900204);

INSERT INTO "Ts_Menu" VALUES ('ChangePassword', 'Change Password', '', 'Authentication', 9003);
INSERT INTO "Ts_Menu" VALUES ('ChangePassword/Detail', 'Detail', 'ChangePassword/Detail', 'ChangePassword', 900301);
INSERT INTO "Ts_Menu" VALUES ('ChangePassword/Change', 'Change Password', 'ChangePassword/Change', 'ChangePassword', 900302);
