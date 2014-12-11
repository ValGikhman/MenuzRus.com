using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MenuzRus.Models {

    public class Comment {

        public String CommentText { get; set; }

        public Int32 id { get; set; }

        public Int32 ParentId { get; set; }

        public Boolean Selected { get; set; }
    }

    public class CommentsModel : BaseModel {

        public List<Comment> Comments { get; set; }
    }
}