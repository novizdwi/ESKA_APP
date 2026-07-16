INSERT INTO "Ts_FormatNumbering" VALUES('StockOpname','StockOpname', 'STO-','YYMM',3);
INSERT INTO "Ts_FormatNumbering" VALUES('ProcessCard','ProcessCard','PC-','YYMM',4);
INSERT INTO "Ts_FormatNumbering" VALUES('ProductionTask','ProductionTask','PDT-','YYMM',4);

INSERT INTO "Ts_List" VALUES('BooleanActive',1,'Active','Active', NULL);
INSERT INTO "Ts_List" VALUES('BooleanActive',2,'Inactive','Inactive', NULL);

INSERT INTO "Ts_List" VALUES('ProductionStatus',1,'Hold','Hold', NULL);
INSERT INTO "Ts_List" VALUES('ProductionStatus',2,'Released','Released', NULL);

INSERT INTO "Ts_List" VALUES('ProcessCardDetailStatus', 1, 'O', 'Open', NULL);
INSERT INTO "Ts_List" VALUES('ProcessCardDetailStatus', 2, 'C', 'Closed', NULL);

INSERT INTO "Ts_List" VALUES('ActivityPauseReason', 1, 'Empty', 'Empty Material', NULL);
INSERT INTO "Ts_List" VALUES('ActivityPauseReason', 2, 'Machine', 'Machine Problem', NULL);
INSERT INTO "Ts_List" VALUES('ActivityPauseReason', 3, 'QC', 'QC Process', NULL);
INSERT INTO "Ts_List" VALUES('ActivityPauseReason', 4, 'Breaktime', 'Breaktime', NULL);
INSERT INTO "Ts_List" VALUES('ActivityPauseReason', 5, 'Other', 'Other', NULL);