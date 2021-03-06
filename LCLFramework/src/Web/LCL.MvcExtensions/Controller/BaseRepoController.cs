﻿using LCL.ComponentModel;
using LCL.MetaModel;
using LCL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LCL.MvcExtensions
{
    [Authorize]
    public class BaseRepoController<TAggregateRoot> : BaseController where TAggregateRoot : class, IAggregateRoot
    {
        IRepository<TAggregateRoot> repo= RF.FindRepo<TAggregateRoot>();
        public IPlugin Bundle { get; set; }
        public bool Check(string permissionId)
        {
            if (PermissionMgr.IsOpenPermission)
            {
                if (Bundle == null)
                    throw new ArgumentException("Bundle");
                return PermissionMgr.HasCommand(Bundle.Assembly.GetName().Name, this.GetType().Name, permissionId);
            }
            else
                return true;
        }
        public ActionResult NoPermissionView()
        {
            return View(PermissionAttribute.NoPermissionView);
        }
        [Permission("列表", "List")]
        public virtual ActionResult List(int? currentPageNum, int? pageSize, FormCollection collection)
        {
            //if (!Check("List"))
            //{
            //    return NoPermissionView();
            //}
            if (!currentPageNum.HasValue)
            {
                currentPageNum = 1;
            }
            if (!pageSize.HasValue)
            {
                pageSize = PagedListViewModel<TAggregateRoot>.DefaultPageSize;
            }
            int pageNum = currentPageNum.Value;

            var villagelist = repo.FindAll().ToList();

            var contactLitViewModel = new PagedListViewModel<TAggregateRoot>(pageNum, pageSize.Value, villagelist.ToList());
            return View(contactLitViewModel);

        }
        [Permission("首页", "Index")]
        public virtual ActionResult Index(int? currentPageNum, int? pageSize, FormCollection collection)
        {
            //if (!Check("Index"))
            //{
            //    return NoPermissionView();
            //}
            return List(currentPageNum, pageSize, collection);
        }
        public virtual ActionResult AddOrEdit(int? currentPageNum, int? pageSize, Guid? id, FormCollection collection)
        {
            if (!currentPageNum.HasValue)
            {
                currentPageNum = 1;
            }
            if (!pageSize.HasValue)
            {
                pageSize = PagedListViewModel<TAggregateRoot>.DefaultPageSize;
            }
            if (!id.HasValue)
            {
                //if (!Check("Add"))
                //{
                //    return NoPermissionView();
                //}
                ViewBag.Action = "Add";
                return View(new AddOrEditViewModel<TAggregateRoot>
                {
                    Added = true,
                    CurrentPageNum = currentPageNum.Value,
                    PageSize = pageSize.Value
                });
            }
            else
            {
                //if (!Check("Edit"))
                //{
                //    return NoPermissionView();
                //}
                ViewBag.Action = "Edit";
                var repo = RF.FindRepo<TAggregateRoot>();
                var village = repo.GetByKey(id.Value);
                return View(new AddOrEditViewModel<TAggregateRoot>
                {
                    Added = false,
                    Entity = village,
                    CurrentPageNum = currentPageNum.Value,
                    PageSize = pageSize.Value
                });
            }
        }
        [Permission("删除", "Delete")]
        public virtual ActionResult Delete(TAggregateRoot village, int? currentPageNum, int? pageSize, FormCollection collection)
        {
            if (!currentPageNum.HasValue)
            {
                currentPageNum = 1;
            }
            if (!pageSize.HasValue)
            {
                pageSize = PagedListViewModel<TAggregateRoot>.DefaultPageSize;
            }
            repo.Delete(village);
            repo.Context.Commit();

            return RedirectToAction("Index", new { currentPageNum = currentPageNum, pageSize = pageSize });
        }
        [Permission("添加", "Add")]
        [HttpPost]
        public virtual ActionResult Add(AddOrEditViewModel<TAggregateRoot> model, FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                return View("AddOrEdit", model);
            }

            repo.Create(model.Entity);
            repo.Context.Commit();

            return RedirectToAction("Index", new { currentPageNum = model.CurrentPageNum, pageSize = model.PageSize });
        }
        [Permission("修改", "Edit")]
        [HttpPost]
        public virtual ActionResult Edit(AddOrEditViewModel<TAggregateRoot> model, FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                return View("AddOrEdit", model);
            }

            repo.Update(model.Entity);
            repo.Context.Commit();

            return RedirectToAction("Index", new { currentPageNum = model.CurrentPageNum, pageSize = model.PageSize });
        }
    }
}
