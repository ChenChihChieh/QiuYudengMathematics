using QiuYudengMathematics.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;


namespace QiuYudengMathematics.Entity.Service
{
    public class DeviceService
    {
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
    }
}