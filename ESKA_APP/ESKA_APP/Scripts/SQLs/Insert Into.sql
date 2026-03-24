INSERT INTO "Ts_FormatNumbering" VALUES('AdjustmentIn', 'Adjustment In', 'ADIN-','YYMM',4);
INSERT INTO "Ts_FormatNumbering" VALUES('AdjustmentOut', 'Adjustment Out', 'ADOU-','YYMM',4);
INSERT INTO "Ts_FormatNumbering" VALUES('ChangeItem', 'Change Item', 'IC-','YYMM',4);

INSERT INTO "Ts_List" VALUES('RFIDStatus', '01', 'A', 'Active', '');
INSERT INTO "Ts_List" VALUES('RFIDStatus', '02', 'I', 'Inactive', '');
INSERT INTO "Ts_List" VALUES('RFIDStatus', '03', 'P', 'Pending', '');

INSERT INTO "Ts_List" VALUES('ItemTagStatus', '01', 'Active', 'Active', '');
INSERT INTO "Ts_List" VALUES('ItemTagStatus', '02', 'Inactive', 'Inactive', '');
INSERT INTO "Ts_List" VALUES('ItemTagStatus', '03', 'Pending', 'Pending', '');

INSERT INTO "Ts_List" VALUES('RFIDTransType', '01', 'TransferSummaryOut', 'Transfer Summary Out', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '02', 'GoodsReceiptPO', 'GoodsReceipt PO', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '03', 'TransferSummaryIn', 'Transfer Summary In', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '04', 'StockSummaryOpname', 'Stock Summary Opname', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '05', 'AdjustmentIn', 'Adjustment In', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '06', 'AdjustmentOut', 'Adjustment Out', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '07', 'DeactiveTags', 'Deactive Tags', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '08', 'ReactiveTags', 'Reactive Tags', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '09', 'ReplaceTags', 'Replace Tags', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '10', 'ChangeItem', 'Change Item', '');

INSERT INTO "Ts_List" VALUES('RFIDTransType', '07', 'DeactiveTags', 'Deactive Tags', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '08', 'ReactiveTags', 'Reactive Tags', '');
INSERT INTO "Ts_List" VALUES('RFIDTransType', '09', 'ReplaceTags', 'Replace Tags', ''); 

INSERT INTO "Ts_LayoutForm" VALUES('AdjustmentIn','Adjustment In','0201');
INSERT INTO "Ts_LayoutForm" VALUES('AdjustmentOut','Adjustment Out','0202');

INSERT INTO "Ts_LayoutForm" VALUES('TransferRequest','Transfer Request','0301');
INSERT INTO "Ts_LayoutForm" VALUES('TransferOutScan','Transfer Out Scan','0302');
INSERT INTO "Ts_LayoutForm" VALUES('TransferOutSummary','Transfer Out Summary','0303');
INSERT INTO "Ts_LayoutForm" VALUES('TransferInScan','Transfer In Scan','0304');
INSERT INTO "Ts_LayoutForm" VALUES('TransferInSummary','Transfer In Summary','0305');

INSERT INTO "Ts_LayoutForm" VALUES('StockOpnameRequest','Stock Opname Request','0401');
INSERT INTO "Ts_LayoutForm" VALUES('StockOpnameScan','Stock Opname Scan','0402');
INSERT INTO "Ts_LayoutForm" VALUES('StockSummaryOpname','Stock Summary Opname','0403');

INSERT INTO "Ts_LayoutForm" VALUES('PurchaseOrderScan','Purchase Order Scan','0501');
INSERT INTO "Ts_LayoutForm" VALUES('StockOpnameScan','Stock Opname Scan','0502');
INSERT INTO "Ts_LayoutForm" VALUES('GoodsReceiptPO','Goods Receipt PO','0503');

INSERT INTO "Ts_List" VALUES('LineStatus', '01', 'Open', 'Open', '');
INSERT INTO "Ts_List" VALUES('LineStatus', '02', 'Closed', 'Closed', '');
