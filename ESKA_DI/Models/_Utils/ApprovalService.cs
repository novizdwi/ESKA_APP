using Models._Ef;
using Models._Utils;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models._Utils
{

    public class NotifApprovalDto
    {
        public string TransNo { get; set; }
        public string TransType { get; set; }
        public DateTime RequestDate { get; set; }
        public string Message { get; set; }
    }





    public class ApprovalService
    {
        public static Boolean ApprovalProcedure(SAPbobsCOM.Company oCompany, int userId, string objectCode, string tableName, string id, string divCode)
        {

            var sqlApprovalStatus = "SELECT T0.ApprovalStatus FROM [" + DbProvider.dbApp_Name + "].dbo.[" + tableName + "] T0 WHERE T0.Id='" + id + "'";
            var approvalStatus = SapCompany.RetRstField(oCompany, sqlApprovalStatus);

            if (approvalStatus == "Approve") //jika sebelumnya sudah melalui approval; scenario nya user2 approver melakukan approve baru di add
            {
                oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
            }

            //Jika Waiting, Reject : bisa langsung ke post tanpa melewati approval selama sudah memenuhi kondisi
            //else if ((approvalStatus == "Waiting") || (approvalStatus == "Reject"))
            else if (approvalStatus == "Waiting")
            {
                var sqlApproval = "CALL [" + DbProvider.dbApp_Name + "].dbo.[SpSysTransApproval] '" + userId.ToString() + "','" + objectCode + "','" + id + "','" + "Y" + "','" + divCode + "'";
                var resultApproval = SapCompany.RetRstField(oCompany, sqlApproval);

                if (string.IsNullOrEmpty(resultApproval))
                {
                    oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                }
                else
                {
                    oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
            }
            else
            {
                var sqlApproval = "CALL [" + DbProvider.dbApp_Name + "].dbo.[SpSysTransApproval] '" + userId.ToString() + "','" + objectCode + "','" + id + "','" + "Y" + "','" + divCode + "'";
                var resultApproval = SapCompany.RetRstField(oCompany, sqlApproval);

                if (string.IsNullOrEmpty(resultApproval))
                {
                    oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                }
                else
                {
                    oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);

                    oCompany.StartTransaction();

                    sqlApproval = "CALL [" + DbProvider.dbApp_Name + "].dbo.[SpSysTransApproval] '" + userId.ToString() + "','" + objectCode + "','" + id + "','" + "N" + "','" + divCode + "'";
                    resultApproval = SapCompany.RetRstField(oCompany, sqlApproval);
                    if (resultApproval == "Waiting")
                    {
                        sqlApproval = "UPDATE T0 SET T0.ApprovalStatus='Waiting' FROM [" + DbProvider.dbApp_Name + "].dbo.[" + tableName + "] T0 WHERE T0.Id='" + id + "'";
                        SapCompany.ExecuteQuery(oCompany, sqlApproval);
                    }

                    oCompany.EndTransaction(BoWfTransOpt.wf_Commit);

                    return true;
                }
            }


            return false;
        }


        public static Boolean ApprovalProcedure(int userId, string objectCode, string tableName, string id, string deptCode)
        {
            var sqlApprovalStatus = "SELECT T0.ApprovalStatus FROM [" + tableName + "] T0 WHERE T0.Id=@0"; //"SELECT TOP 1 ISNULL(T0.U_IDU_AirlineCode,'') AS AirlineCode FROM Tp_Airline T0 (NOLOCK) WHERE T0.U_IDU_TicketCode=@0 ORDER BY T0.U_IDU_AirlineCode ASC ";
            var approvalStatus = DbProvider.dbApp.Database.SqlQuery<string>(sqlApprovalStatus, id).Single(); ;

            if (approvalStatus == "Approve") //jika sebelumnya sudah melalui approval; scenario nya user2 approver melakukan approve baru di add
            {
                return true;
            }
            //Jika Waiting, Reject : bisa langsung ke post tanpa melewati approval selama sudah memenuhi kondisi
            else if ((approvalStatus == "Waiting"))
            {
                var sqlApproval = "CALL \"SpSysTransApproval\" '" + userId.ToString() + "','" + objectCode + "','" + id + "','" + "Y" + "','" + deptCode + "'";
                var resultApproval = DbProvider.dbApp.Database.SqlQuery<string>(sqlApproval).Single();

                if (string.IsNullOrEmpty(resultApproval))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var sqlApproval = "CALL \"SpSysTransApproval\" '" + userId.ToString() + "','" + objectCode + "','" + id + "','" + "Y" + "','" + deptCode + "'";
                var resultApproval = DbProvider.dbApp.Database.SqlQuery<string>(sqlApproval).Single();

                if (string.IsNullOrEmpty(resultApproval))
                {
                    return true;
                }
                else
                {
                    sqlApproval = "CALL [" + DbProvider.dbApp_Name + "].dbo.[SpSysTransApproval] '" + userId.ToString() + "','" + objectCode + "','" + id + "','" + "N" + "','" + deptCode + "'";
                    resultApproval = DbProvider.dbApp.Database.SqlQuery<string>(sqlApproval).Single();
                    if (resultApproval == "Waiting")
                    {
                        sqlApproval = "UPDATE T0 SET T0.ApprovalStatus='Waiting' FROM [" + tableName + "] T0 WHERE T0.Id='" + id + "'";
                        DbProvider.dbApp.Database.SqlQuery<string>(sqlApproval);
                    }

                    return false;
                }
            }
        }

    }
}
