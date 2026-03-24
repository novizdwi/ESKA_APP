using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Web.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using Models;


namespace Models._Utils
{
    public class GetSqlFromGridViewModelState
    {
        //public static string getSqlCriteria(GridViewModel state)
        //{

        //    string resultCriteria = "";



        //    CriteriaOperator opWhere = CriteriaOperator.Parse(state.FilterExpression); //filterControl1.FilterCriteria


        //    string whereString = DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetMsSqlWhere(opWhere);


        //    if (whereString != "")
        //    {
        //        //jika ada [AliasTbl.FieldName] kurung nya di hilangkan saja, tidak akan di kenali ketika where 
        //        for (int i = 0; i < state.Columns.Count; i++)
        //        {
        //            if (state.Columns[i].FieldName.Contains("."))
        //            {
        //                /*
        //                if (state.Columns[i].FieldName == "Tp_TicketStock___.TicketNoRaw")
        //                {
        //                    if (state.Columns[i].FilterExpression != "")
        //                    {
        //                        CriteriaOperator opWhere1 = CriteriaOperator.Parse(state.Columns[i].FilterExpression);
        //                        string whereString1 = DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetMsSqlWhere(opWhere1);

        //                        CriteriaOperator opWhere2;
        //                        if (state.Columns[i].FilterExpression.StartsWith("Not Contains"))
        //                        {
        //                            string str1 = state.Columns[i].FilterExpression;
        //                            string str2 = str1.Substring(str1.Length - (str1.Length - 12)).Replace(" ", "");

        //                            opWhere2 = CriteriaOperator.Parse("Not Contains" + str2);
        //                        }
        //                        else
        //                        {
        //                            opWhere2 = CriteriaOperator.Parse(state.Columns[i].FilterExpression.Replace(" ", ""));
        //                        }


        //                        string whereString2 = DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetMsSqlWhere(opWhere2);

        //                        whereString = whereString.Replace(whereString1, whereString2);

        //                    } 

        //                }

        //                */

        //                //whereString = whereString.Replace("[" + state.Columns[i].FieldName + "]", state.Columns[i].FieldName);
        //                whereString = whereString.Replace("\"" + state.Columns[i].FieldName + "\"", state.Columns[i].FieldName);
        //            }
        //        }

        //        //resultCriteria = "WHERE " + whereString;

        //        if (state.Columns.Count == 0)
        //        {
        //            resultCriteria = "";
        //        }
        //        else
        //        {
        //            resultCriteria = whereString;
        //        }

        //    }

        //    return resultCriteria;

        //}

        //public static string getSqlSort(GridViewModel state)
        //{
        //    string resultSort = "";

        //    for (int i = 0; i < state.SortedColumns.Count; i++)
        //    {
        //        if (resultSort != "")
        //        {
        //            resultSort = resultSort + ",";
        //        }
        //        else
        //        {
        //            resultSort = "ORDER BY ";
        //        }
        //        if (state.SortedColumns[i].SortOrder == ColumnSortOrder.Ascending)
        //        {
        //            if (state.SortedColumns[i].FieldName.Contains("."))
        //            {
        //                resultSort = resultSort + "" + state.SortedColumns[i].FieldName + " ASC";
        //            }
        //            else
        //            {
        //                resultSort = resultSort + "[" + state.SortedColumns[i].FieldName + "] ASC";
        //            }

        //        }
        //        else
        //        {
        //            if (state.SortedColumns[i].FieldName.Contains("."))
        //            {
        //                resultSort = resultSort + "" + state.SortedColumns[i].FieldName + " DESC";
        //            }
        //            else
        //            {
        //                resultSort = resultSort + "[" + state.SortedColumns[i].FieldName + "] DESC";
        //            }

        //        }
        //    }

        //    return resultSort;

        //}

        public static string getHanaCriteria_Backup(object mantap, string whereString)
        {

            if (mantap.GetType().Name == "GroupOperator")
            {
                var operands = ((DevExpress.Data.Filtering.GroupOperator)(mantap)).Operands;

                for (int i = 0; i < operands.Count; i++)
                {
                    whereString = getHanaCriteria2(operands[i], whereString);
                }
            }
            else if (mantap.GetType().Name == "BinaryOperator")
            {
                var binaryOperator = ((DevExpress.Data.Filtering.BinaryOperator)(mantap));
                ////Equal
                //if (binaryOperator.OperatorType == BinaryOperatorType.Equal)
                //{
                //    var operandProperty = ((DevExpress.Data.Filtering.OperandProperty)(binaryOperator.LeftOperand));
                //    var operandValue = ((DevExpress.Data.Filtering.OperandValue)(binaryOperator.RightOperand));
                //    whereString = whereString + " AND \"" + operandProperty.PropertyName + "\" = '" + operandValue.Value.ToString().Replace("'", "''") + "'";
                //}
                ////NotEqual
                //else if (binaryOperator.OperatorType == BinaryOperatorType.NotEqual)
                //{
                //    var operandProperty = ((DevExpress.Data.Filtering.OperandProperty)(binaryOperator.LeftOperand));
                //    var operandValue = ((DevExpress.Data.Filtering.OperandValue)(binaryOperator.RightOperand));
                //    whereString = whereString + " AND \"" + operandProperty.PropertyName + "\" <> '" + operandValue.Value.ToString().Replace("'", "''") + "'"; 
                //}

                whereString = whereString + DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetOracleWhere(binaryOperator, true);

            }
            else if (mantap.GetType().Name == "UnaryOperator")
            {
                var unaryOperator = ((DevExpress.Data.Filtering.UnaryOperator)(mantap));
                //NotContains
                //if (unaryOperator.OperatorType == UnaryOperatorType.Not)
                //{

                //}
                whereString = whereString + DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetOracleWhere(unaryOperator, true);
            }
            else if (mantap.GetType().Name == "FunctionOperator")
            {
                var functionOperator = ((DevExpress.Data.Filtering.FunctionOperator)(mantap));
                //StartsWith
                if (functionOperator.OperatorType == FunctionOperatorType.StartsWith)
                {
                    var operandProperty = ((DevExpress.Data.Filtering.OperandProperty)(functionOperator.Operands[0]));
                    var constantValue = ((DevExpress.Data.Filtering.ConstantValue)(functionOperator.Operands[1]));

                    whereString = whereString + " AND \"" + operandProperty.PropertyName + "\" LIKE '" + constantValue.Value.ToString().Replace("'", "''") + "%'";
                }
                //Contains
                else if (functionOperator.OperatorType == FunctionOperatorType.Contains)
                {
                    var operandProperty = ((DevExpress.Data.Filtering.OperandProperty)(functionOperator.Operands[0]));
                    var constantValue = ((DevExpress.Data.Filtering.ConstantValue)(functionOperator.Operands[1]));

                    whereString = whereString + " AND \"" + operandProperty.PropertyName + "\" LIKE '%" + constantValue.Value.ToString().Replace("'", "''") + "%'";
                }
                //EndsWith
                else if (functionOperator.OperatorType == FunctionOperatorType.EndsWith)
                {
                    var operandProperty = ((DevExpress.Data.Filtering.OperandProperty)(functionOperator.Operands[0]));
                    var constantValue = ((DevExpress.Data.Filtering.ConstantValue)(functionOperator.Operands[1]));

                    whereString = whereString + " AND \"" + operandProperty.PropertyName + "\" LIKE '%" + constantValue.Value.ToString().Replace("'", "''") + "'";
                }
            }

            return whereString;

        }

        public static string getHanaCriteria2(object mantap, string whereString)
        {
            if (mantap.GetType().Name == "GroupOperator")
            {
                var operands = ((DevExpress.Data.Filtering.GroupOperator)(mantap)).Operands;

                for (int i = 0; i < operands.Count; i++)
                {
                    whereString = getHanaCriteria2(operands[i], whereString);
                }
            }
            else if (mantap.GetType().Name == "BinaryOperator")
            {
                var binaryOperator = ((DevExpress.Data.Filtering.BinaryOperator)(mantap));
                if (string.IsNullOrEmpty(whereString))
                {
                    whereString = DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetOracleWhere(binaryOperator, true);
                }
                else
                {
                    whereString = whereString + " AND " + DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetOracleWhere(binaryOperator, true);
                }
                

            }
            else if (mantap.GetType().Name == "UnaryOperator")
            {
                var unaryOperator = ((DevExpress.Data.Filtering.UnaryOperator)(mantap));
                if (string.IsNullOrEmpty(whereString))
                {
                    whereString = DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetOracleWhere(unaryOperator, true);
                }
                else
                {
                    whereString = whereString + " AND " + DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetOracleWhere(unaryOperator, true);
                }
               
            }
            else if (mantap.GetType().Name == "FunctionOperator")
            {
                var functionOperator = ((DevExpress.Data.Filtering.FunctionOperator)(mantap));
                //StartsWith
                if (functionOperator.OperatorType == FunctionOperatorType.StartsWith)
                {
                    if (string.IsNullOrEmpty(whereString))
                    {
                        whereString = DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetOracleWhere(functionOperator, true);
                    }
                    else
                    {
                        whereString = whereString + " AND " + DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetOracleWhere(functionOperator, true);
                    }
                   
                }
                //Contains
                else if (functionOperator.OperatorType == FunctionOperatorType.Contains)
                {
                    if (string.IsNullOrEmpty(whereString))
                    {
                        whereString = DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetOracleWhere(functionOperator, true);
                    }
                    else
                    {
                        whereString = whereString + " AND " + DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetOracleWhere(functionOperator, true);
                    }
                    
                }
                //EndsWith
                if (functionOperator.OperatorType == FunctionOperatorType.EndsWith)
                {
                    var operandProperty = ((DevExpress.Data.Filtering.OperandProperty)(functionOperator.Operands[0]));
                    var constantValue = ((DevExpress.Data.Filtering.ConstantValue)(functionOperator.Operands[1]));
                    if (string.IsNullOrEmpty(whereString))
                    {
                        whereString = " \"" + operandProperty.PropertyName + "\" LIKE '%" + constantValue.Value.ToString().Replace("'", "''") + "'";
                    }
                    else
                    {
                        whereString = whereString + " AND \"" + operandProperty.PropertyName + "\" LIKE '%" + constantValue.Value.ToString().Replace("'", "''") + "'";
                    }
                    
                }
            }

            return whereString;

        }


        //public static string getHanaCriteria(GridViewModel state)
        //{

        //    string resultCriteria = "";

        //    CriteriaOperator opWhere = CriteriaOperator.Parse(state.FilterExpression); //filterControl1.FilterCriteria  

        //    //string whereString = DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetOracleWhere(opWhere,true);
        //    //if (string.IsNullOrEmpty(whereString))
        //    //{
        //    //    whereString = " AND " + whereString;
        //    //}
        //    //return whereString;

        //    if (opWhere != null)
        //    {

        //        resultCriteria = getHanaCriteria2(opWhere, resultCriteria);
        //    }

        //    return resultCriteria;

        //}

        public static string getHanaCriteria(GridViewModel state)
        {

            string resultCriteria = "";

            CriteriaOperator opWhere = CriteriaOperator.Parse(state.FilterExpression); //filterControl1.FilterCriteria  

            //string whereString = DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetOracleWhere(opWhere,true);
            //if (string.IsNullOrEmpty(whereString))
            //{
            //    whereString = " AND " + whereString;
            //}
            //return whereString;

            if (opWhere != null)
            {

                resultCriteria = getHanaCriteria2(opWhere, resultCriteria);
                resultCriteria = resultCriteria.ToUpper();

                for (int i = 0; i < state.Columns.Count; i++)
                {
                    resultCriteria = resultCriteria.Replace("(\"" + state.Columns[i].FieldName.ToUpper() + "\"", "(UPPER(\"" + state.Columns[i].FieldName + "\")");

                    resultCriteria = resultCriteria.Replace("\"" + state.Columns[i].FieldName.ToUpper() + "\"", "UPPER(\"" + state.Columns[i].FieldName + "\")");
                }

            }

            return resultCriteria;

        }

        public static string getHanaSort(GridViewModel state)
        {
            string resultSort = "";

            for (int i = 0; i < state.SortedColumns.Count; i++)
            {
                if (resultSort != "")
                {
                    resultSort = resultSort + ",";
                }
                else
                {
                    resultSort = "ORDER BY ";
                }
                if (state.SortedColumns[i].SortOrder == ColumnSortOrder.Ascending)
                {
                    resultSort = resultSort + "UPPER(\"" + state.SortedColumns[i].FieldName + "\") ASC";
                }
                else
                {
                    resultSort = resultSort + "UPPER(\"" + state.SortedColumns[i].FieldName + "\") DESC";
                }
            }

            return resultSort;

        }

    }
}