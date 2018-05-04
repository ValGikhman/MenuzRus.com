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

        #region Private Fields

        private ICommentService _commentService;

        #endregion Private Fields

        #region Public Constructors

        public CommentsController(ISessionData sessionData, ICommentService commentService)
            : base(sessionData) {
            _commentService = commentService;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpPost]
        public ActionResult DeleteComment(Int32 id, Int32 parentId, String type) {
            try {
                _commentService.DeleteComment(id, parentId, EnumHelper<CommonUnit.CommentType>.Parse(type));
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
            return RenderViewToString(this.ControllerContext, "_CommentsPartial", GetModel(parentId, EnumHelper<CommonUnit.CommentType>.Parse(type)));
        }

        [HttpPost]
        public void Save(String commentText) {
            try {
                _commentService.Save(commentText, SessionData.customer.id);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
        }

        [HttpPost]
        public void SaveComment(Int32 id, Int32 parentId, String type) {
            try {
                _commentService.SaveComment(id, parentId, EnumHelper<CommonUnit.CommentType>.Parse(type));
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
        }

        #endregion Public Methods

        #region Private Methods

        private CommentsModel GetModel(Int32 parentId, CommonUnit.CommentType type) {
            CommentsModel model = new CommentsModel();
            model.Comments = new List<Models.Comment>();

            try {
                List<CommentUnion> comments = _commentService.GetComments(SessionData.customer.id, parentId, type);

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
            }

            return model;
        }

        #endregion Private Methods
    }
}