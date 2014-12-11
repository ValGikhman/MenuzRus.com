using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class CommentsController : BaseController {

        [HttpPost]
        public ActionResult DeleteComment(Int32 id, Int32 parentId, String type) {
            CommentService service = new CommentService();
            try {
                service.DeleteComment(id, parentId, EnumHelper<Common.CommentType>.Parse(type));
            }
            catch (Exception ex) {
                base.Log(ex);
                return RedirectToAction("Index", "Error");
            }
            finally {
            }

            return Json("OK");
        }

        [HttpGet]
        public String GetComments(Int32 parentId, String type) {
            return RenderViewToString(this.ControllerContext, "_CommentsPartial", GetModel(parentId, EnumHelper<Common.CommentType>.Parse(type)));
        }

        [HttpPost]
        public void Save(String commentText) {
            CommentService service = new CommentService();
            try {
                service.Save(commentText);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }
        }

        [HttpPost]
        public void SaveComment(Int32 id, Int32 parentId, String type) {
            CommentService service = new CommentService();
            try {
                service.SaveComment(id, parentId, EnumHelper<Common.CommentType>.Parse(type));
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }
        }

        private CommentsModel GetModel(Int32 parentId, Common.CommentType type) {
            CommentsModel model = new CommentsModel();
            model.Comments = new List<Models.Comment>();

            CommentService service = new CommentService();
            List<CommentUnion> comments = service.GetComments(SessionData.customer.id, parentId, type);

            try {
                foreach (CommentUnion comment in comments) {
                    Models.Comment comm = new Models.Comment();
                    comm.id = comment.id;
                    comm.ParentId = comment.ParentId;
                    comm.CommentText = comment.CommentText;
                    comm.Selected = comment.Selected;
                    model.Comments.Add(comm);
                }
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }

            return model;
        }
    }
}