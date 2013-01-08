using mesoBoard.Framework.Models;
using NUnit.Framework;

namespace mesoBoard.Tests.Models
{
    [TestFixture]
    public class PaginationTests
    {
        [Test]
        public void Pagination_With_Action_Controller_Strings()
        {
            string action = "MyAction";
            string controller = "MyController";

            Pagination Pagination = new Pagination(1, 10, 10, action, controller);

            Assert.AreEqual(action, Pagination.RouteValues["action"]);
            Assert.AreEqual(controller, Pagination.RouteValues["controller"]);
        }

        [Test]
        public void Pagination_With_Action_Controller_Strings_With_Route_Values()
        {
            string action = "MyAction";
            string controller = "MyController";

            Pagination Pagination = new Pagination(1, 25, 10, action, controller, new { id = 1 });
            Assert.AreEqual(1, Pagination.RouteValues["id"]);

            Assert.AreEqual(action, Pagination.RouteValues["action"]);
            Assert.AreEqual(controller, Pagination.RouteValues["controller"]);
        }

        [Test]
        public void Pagination_With_Route_Values_Object()
        {
            string action = "MyAction";
            string controller = "MyController";

            Pagination Pagination = new Pagination(1, 25, 10, new { action = action, controller = controller, id = 1 });
            Assert.AreEqual(action, Pagination.RouteValues["action"]);
            Assert.AreEqual(action, Pagination.Action);

            Assert.AreEqual(controller, Pagination.RouteValues["controller"]);
            Assert.AreEqual(controller, Pagination.Controller);

            Assert.AreEqual(1, Pagination.RouteValues["id"]);
        }

        [Test]
        public void Is_Valid_Total_Pages()
        {
            Pagination Pagination = new Pagination(1, 25, 10, "Action", "Controller");
            Assert.AreEqual(3, Pagination.TotalPages);
        }

        [Test]
        public void GetDictionary_Page_Value()
        {
            int count = 20;
            Pagination Pagination = new Pagination(1, count, 10, new { controller = "MyAction", action = "MyAction" });
            for (int i = 1; i < Pagination.TotalPages; i++)
            {
                Assert.AreEqual(i, Pagination.GetDictionary(i)["page"]);
            }
        }
    }
}