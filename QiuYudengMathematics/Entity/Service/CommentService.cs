using QiuYudengMathematics.Models;
using QiuYudengMathematics.Models.ViewModels;
using System.Linq;
using System;
using QiuYudengMathematics.Comm;

namespace QiuYudengMathematics.Entity.Service
{
    public class CommentService
    {
        private readonly LogService logService;
        public CommentService()
        {
            logService = new LogService();
        }
        public RtnModel InsertComment(CommentModel model)
        {
            try
            {
                RtnModel rtn = CheckField(model);
                if (!rtn.Success) return rtn;
                using (var db = new QiuYudengMathematicsEntities())
                {
                    db.BoardComment.Add(new BoardComment()
                    {
                        BulletinBoardSeq = model.Seq,
                        Account = WebSiteComm.CurrentUserAccount,
                        Comment = model.Commentary
                    });
                    rtn.Success = db.SaveChanges() > 0;
                    rtn.Msg = rtn.Success ? "新增成功" : "新增失敗";
                    return rtn;
                }
            }
            catch (Exception e)
            {
                logService.Insert(e);
                return new RtnModel() { Success = false, Msg = "新增發生錯誤，請通知工程師" };
            }
        }
        public RtnModel InsertSubComment(CommentModel model)
        {
            try
            {
                RtnModel rtn = CheckField(model);
                if (!rtn.Success) return rtn;
                using (var db = new QiuYudengMathematicsEntities())
                {
                    db.BoardSubComment.Add(new BoardSubComment()
                    {
                        CommentSeq = model.Seq,
                        Account = WebSiteComm.CurrentUserAccount,
                        Comment = model.Commentary
                    });
                    rtn.Success = db.SaveChanges() > 0;
                    rtn.Msg = rtn.Success ? "新增成功" : "新增失敗";
                    return rtn;
                }
            }
            catch (Exception e)
            {
                logService.Insert(e);
                return new RtnModel() { Success = false, Msg = "新增發生錯誤，請通知工程師" };
            }
        }
        private RtnModel CheckField(CommentModel model)
        {
            if (string.IsNullOrEmpty(model.Commentary)) return new RtnModel() { Success = false, Msg = "請輸入留言" };
            return new RtnModel() { Success = true, Msg = string.Empty };
        }
    }
}