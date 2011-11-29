using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace ImagineSetup.Controllers
{
    public class ImageController : Controller
    {
      public ActionResult Show(int id)
        {
          var imageData = GetImage(@"C:\Users\CARACARN\Dropbox\Photos\Sample Album\Costa Rican Frog.jpg");
          return File(imageData, "image/jpg");
        }

      byte[] GetImage(string path)
      {

          byte[] bytes = new byte[0];
          using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
          {
              bytes = new byte[fs.Length];
              int numBytesToRead = (int)fs.Length;
              int numBytesRead = 0;
              while (numBytesToRead > 0)
              {
                  int n = fs.Read(bytes, numBytesRead, numBytesToRead);

                  if (n == 0)
                      break;
                  numBytesRead += n;
                  numBytesToRead -= n;
              }
          }

          return bytes;
      }
    }
}
