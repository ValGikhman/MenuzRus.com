using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface ICommentService {

        Boolean DeleteComment(Int32 id, Int32 parentId, Common.CommentType type);

        List<CommentUnion> GetComments(Int32 customerId, Int32 parentId, Common.CommentType type);

        String GetItemComment(Int32 parentId, Common.CommentType type, Int32 customerId);

        Int32 Save(String commentText, Int32 customerId);

        Int32 SaveComment(Int32 id, Int32 parentId, Common.CommentType type);
    }
}