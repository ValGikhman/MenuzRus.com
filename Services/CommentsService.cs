using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class CommentService : BaseService, ICommentService {

        public Boolean DeleteComment(Int32 id, Int32 parentId, Common.CommentType type) {
            CheckMenuComment query;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    query = db.CheckMenuComments.FirstOrDefault(m => m.CommentId == id && m.ParentId == parentId && m.Type == (Int32)type);
                    if (query != default(CheckMenuComment)) {
                        db.CheckMenuComments.DeleteOnSubmit(query);
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }

        public List<CommentUnion> GetComments(Int32 customerId, Int32 parentId, Common.CommentType type) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            CheckMenuComment checkMenuComment;
            List<CommentUnion> retVal = new List<CommentUnion>();
            IEnumerable<Comment> comments = db.Comments.Where(m => m.CustomerId == customerId);
            IEnumerable<CheckMenuComment> checkMenuComments = db.CheckMenuComments.Where(m => m.ParentId == parentId && m.Type == (Int32)type);
            foreach (Comment comment in comments) {
                CommentUnion comm = new CommentUnion();
                comm.id = comment.id;
                comm.CommentText = comment.CommentText;
                comm.ParentId = 0;
                comm.Selected = false;
                checkMenuComment = checkMenuComments.FirstOrDefault(m => m.CommentId == comment.id);
                if (checkMenuComment != default(CheckMenuComment)) {
                    comm.ParentId = checkMenuComment.ParentId;
                    comm.Selected = true;
                }
                retVal.Add(comm);
            }

            return retVal;
        }

        public String GetItemComment(Int32 parentId, Common.CommentType type, Int32 customerId) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            String[] query = (from com in db.Comments
                              join cmc in db.CheckMenuComments on com.id equals cmc.CommentId
                              where com.CustomerId == customerId
                                && cmc.ParentId == parentId
                                && cmc.Type == (Int32)type
                              select com.CommentText).ToArray();
            return String.Join(" | ", query);
        }

        public Int32 Save(String commentText, Int32 customerId) {
            Comment query = new Comment();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    query.CommentText = commentText;
                    query.CustomerId = customerId;
                    query.DateModified = DateTime.UtcNow;
                    db.Comments.InsertOnSubmit(query);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
            }
            return query.id;
        }

        public Int32 SaveComment(Int32 id, Int32 parentId, Common.CommentType type) {
            CheckMenuComment query = new CheckMenuComment();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    query.DateModified = DateTime.UtcNow;
                    query.Type = (Int32)type;
                    query.ParentId = parentId;
                    query.CommentId = id;
                    db.CheckMenuComments.InsertOnSubmit(query);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
            }
            return query.id;
        }
    }
}

public class CommentUnion {
    public String CommentText { get; set; }

    public Int32 id { get; set; }

    public Int32 ParentId { get; set; }

    public Boolean Selected { get; set; }
}