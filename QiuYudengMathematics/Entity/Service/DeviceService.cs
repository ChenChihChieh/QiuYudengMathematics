using QiuYudengMathematics.Models;
using QiuYudengMathematics.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;


namespace QiuYudengMathematics.Entity.Service
{
    public class DeviceService
    {
        private readonly LogService logService;
        public DeviceService()
        {
            logService = new LogService();
        }
        public bool CheckDevice(string Id, string Device)
        {
            using (var db = new QiuYudengMathematicsEntities())
            {
                var data = db.StudentDevice.Where(x => x.Account == Id).ToList();
                if (data.Where(x => x.Device.ToUpper() == Device.ToUpper()).Any())
                    return true;
                else
                {
                    if (data.Count >= 2)
                        return false;
                    else
                    {
                        db.StudentDevice.Add(new StudentDevice()
                        {
                            Account = Id,
                            Device = Device
                        });
                        db.SaveChanges();
                        return true;
                    }
                }
            }
        }
        public RtnModel DeleteDevice(string Id)
        {
            try
            {
                using (var db = new QiuYudengMathematicsEntities())
                {
                    var data = db.StudentDevice.Where(x => x.Account == Id).ToList();
                    foreach (var d in data)
                        db.StudentDevice.Remove(d);
                    db.SaveChanges();
                    return new RtnModel() { Success = true, Msg = "刪除成功" };
                }
            }
            catch (Exception e)
            {
                logService.Insert(e);
                return new RtnModel() { Success = false, Msg = "刪除發生錯誤，請通知工程師" };
            }
        }
    }
}