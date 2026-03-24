using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using System.Threading;  

using System.Net;

using Models;
using Models.Authentication.Role;

namespace Controllers.Authentication
{
    public partial class RoleController : BaseController
    { 

        public ActionResult ListPartial()
        {
            var viewModel = GetListModel();
            ProcessCustomBinding(viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Paging
        public ActionResult ListPaging(GridViewPagerState pager)
        {
            var viewModel = GetListModel();
            viewModel.ApplyPagingState(pager);
            ProcessCustomBinding(viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Filtering
       
        public ActionResult ListFiltering(GridViewFilteringState filteringState)
        {
            var viewModel = GetListModel();
            viewModel.ApplyFilteringState(filteringState);
            ProcessCustomBinding(viewModel);
            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

        // Sorting
        public ActionResult ListSorting(GridViewColumnState column, bool reset)
        {
            var viewModel = GetListModel();
            viewModel.ApplySortingState(column, reset);
            ProcessCustomBinding(viewModel);

            return PartialView(VIEW_LIST_PARTIAL, viewModel);
        }

      

        static GridViewModel GetListModel()
        {
            var viewModel = GridViewExtension.GetViewModel("gvRoleList");
            if (viewModel == null)
            {
                viewModel = Role__List_Model.CreateGridViewModel();
            }

            return viewModel;
        }

        static void ProcessCustomBinding(GridViewModel viewModel)
        {
            viewModel.ProcessCustomBinding(
                Role__List_Model.GetDataRowCount,
                Role__List_Model.GetData
            );
           
        }

        public ActionResult PopupListLoadOnDemandPartial()
        {
            var viewModel = GetListModel();
            ProcessCustomBinding(viewModel);

            return PartialView(VIEW_PANEL_LIST_PARTIAL, viewModel);
        } 

    }
}