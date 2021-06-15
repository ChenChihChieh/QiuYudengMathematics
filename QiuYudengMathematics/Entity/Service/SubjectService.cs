using QiuYudengMathematics.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace QiuYudengMathematics.Entity.Service
{
    public class SubjectService
    {
        public List<GradeViewModel> getGradeSubject()
        {
            using (var db = new QiuYudengMathematicsEntities())
            {
                var data = db.GroupGrade.Where(x => x.Enable)
                    .Select(item => new GradeViewModel()
                    {
                        GradeId = item.ID,
                        GradeName = item.Grade
                    }).ToList();

                var SubJectList = db.GroupGradeSubject.Where(x => x.Enable).ToList();

                data.ForEach(x =>
                {
                    x.Subject = SubJectList.Where(y => y.GradeID == x.GradeId)
                    .Select(itemD => new Subject()
                    {
                        ID = itemD.ID,
                        SubjectName = itemD.Subject,
                        Detriment = false
                    }).ToList();
                });

                return data;
            }
        }
    }
}