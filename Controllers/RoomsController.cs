using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImagineSetup.PersistanceInterface.Repositories;
using Moq;
using ImagineSetup.PersistanceInterface.DTO;
using System.IO;
using System.Drawing;

namespace ImagineSetup.Controllers
{
    public class RoomsController : Controller
    {
        IRoomsRepository _repos;

        public RoomsController()
        {
            Mock<IRoomsRepository> mock = new Mock<IRoomsRepository>();
            mock.Setup(x => x.AllRooms()).Returns(FakeRooms);
            mock.Setup(x => x.RoomInfo(It.IsAny<string>())).Returns(FakeRoomInfo());
            _repos = mock.Object;
        }
        //
        // GET: /Rooms/

        public ActionResult Index()
        {
            return View(_repos.AllRooms());
        }


        public ActionResult Room(string roomName)
        {
            var stuff = _repos.RoomInfo(roomName);
            var items = stuff.Photos; 
            return View(stuff);
        }

        IEnumerable<IRoom> FakeRooms()
        {

            List<IRoom> rooms = new List<IRoom>();


            for (int i = 0; i < 10; i++)
            {
                Mock<IRoom> mock = new Mock<IRoom>();
                mock.Setup(x => x.Id).Returns(i);
                mock.Setup(x => x.Name).Returns("Room" + i.ToString());
                mock.Setup(x => x.Description).Returns("This is a breif description of Room" + i.ToString());

                rooms.Add(mock.Object);
            }
            return rooms;
        }

        IRoomInfo FakeRoomInfo()
        {
            Mock<IRoomInfo> mock = new Mock<IRoomInfo>();

            mock.Setup(x => x.Photos).Returns(FakePictures());
            mock.Setup(x => x.Instructions).Returns(FakeInstructions());
            mock.Setup(x => x.Items).Returns(FakeItems()); 
            return mock.Object;
        }

        IEnumerable<IRoomProduct> FakeItems()
        {
            List<IRoomProduct> products = new List<IRoomProduct>();
            products.Add(FakeProduct("box", 2));
            products.Add(FakeProduct("scissors", 1));
            products.Add(FakeProduct("tape", 4)); 

            return products;
        }

        IEnumerable<IPicture> FakePictures()
        {
            List<IPicture> pictures = new List<IPicture>();

            for (int i = 0; i < 4; i++)
            {
                Mock<IPicture> mock = new Mock<IPicture>();
                mock.Setup(x => x.Name).Returns("Picture" + i);
                mock.Setup(x => x.Description).Returns("This is Picture" + i.ToString());
                //mock.Setup(x => x.ImageData).Returns(GetImage(@"C:\Users\CARACARN\Dropbox\Photos\Sample Album\Costa Rican Frog.jpg"));
                pictures.Add(mock.Object);
            }
            return pictures;
        }

        IEnumerable<IInstruction> FakeInstructions()
        {
            List<IInstruction> instructions = new List<IInstruction>();
            instructions.Add(FakeInstruction(0, "Find a box"));
            instructions.Add(FakeInstruction(1, "Stack another box on top of it"));
            instructions.Add(FakeInstruction(2, "Tape the two boxes together"));
            instructions.Add(FakeInstruction(3, "You now have a fort"));
            return instructions; 
            
        }

        IInstruction FakeInstruction(int order, string text )
        {
            Mock<IInstruction> mock = new Mock<IInstruction>();
            mock.Setup(x => x.Order).Returns(order);
            mock.Setup(x => x.Text).Returns(text);
            return mock.Object; 
        }

        IRoomProduct FakeProduct(string productName, int count)
        {
            Mock<IRoomProduct> mock = new Mock<IRoomProduct>();

            mock.Setup(x => x.ProductName).Returns(productName);
            mock.Setup(x => x.Count).Returns(count);
            mock.Setup(x => x.Id).Returns(1); 

            return mock.Object; 
        }

   

        Image ImageItem(string path)
        {
            Image retImage = null;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (retImage = Image.FromStream(fs))
                { }
            }
            return retImage;
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
